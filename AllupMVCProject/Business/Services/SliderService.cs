using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Services;

public class SliderService : ISliderService
{
    private readonly AllupDbContext _context;
    private readonly IWebHostEnvironment _env;
    public SliderService(AllupDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _env = webHostEnvironment;
    }
    public async Task CreateAsync(Slider slider)
    {
        if (slider.ImageFile is null)
        {
            throw new RequiredPropertyException("ImageFile", "This area is required!");
        }

        if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
        {
            throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
        }
        if(slider.ImageFile.Length> 2097152)
        {
            throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
        }
        string FileName= slider.ImageFile.FileName;
        if (FileName.Length > 64)
        {
            FileName = FileName.Substring(FileName.Length - 64, 64);
        }

        FileName = Guid.NewGuid().ToString() + FileName;

        string path = Path.Combine(_env.WebRootPath, "Uploads/Sliders", FileName);

        using (FileStream fileStream = new FileStream(path,FileMode.Create))
        {
            slider.ImageFile.CopyTo(fileStream);    
        }

        slider.CreatedDate = DateTime.UtcNow.AddHours(4);
        slider.UpdatedDate= DateTime.UtcNow.AddHours(4);
        slider.ImageUrl = FileName;

        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Slider? slider=await _context.Sliders.FirstOrDefaultAsync(s=>s.Id == id);

        if (slider is null)
        {
            throw new NotFoundException("Slide is not found");
        }

        string path = Path.Combine(_env.WebRootPath, "Uploads/Sliders", slider.ImageUrl);

        if(File.Exists(path))
        {
            File.Delete(path);
        }

        _context.Sliders.Remove(slider);   
        await _context.SaveChangesAsync();

    }

    public async Task<List<Slider>> GetAllAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes)
    {
        var query=_context.Sliders.AsQueryable();
        _GetIncludes(query, includes);
        return expression is not null 
            ? await query.Where(expression).ToListAsync()
            : await query.ToListAsync();
    }

    public async Task<Slider> GetByIdAsync(int id)
    {
       Slider Slider=await _context.Sliders.FindAsync(id);
        return Slider;
    }

    public async Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Sliders.AsQueryable();
        _GetIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();
    }
    
    public async Task UpdateAsync(Slider slider)
    {
       Slider? currentSlider= await _context.Sliders.FirstOrDefaultAsync(s => s.Id == slider.Id);

        if(currentSlider is null)
        {
            throw new NotFoundException("Slider is not found");
        }
        if(slider.ImageFile is not null)
        {
            if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
            }
            if (slider.ImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string FileName = slider.ImageFile.FileName;
            if (FileName.Length > 64)
            {
                FileName = FileName.Substring(FileName.Length - 64, 64);
            }

            FileName = Guid.NewGuid().ToString() + FileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/Sliders", FileName);

            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                slider.ImageFile.CopyTo(filestream);
            }

            string Lastpath = Path.Combine(_env.WebRootPath, "Uploads/Sliders", currentSlider.ImageUrl);

            if (File.Exists(Lastpath))
            {
                File.Delete(Lastpath);
            }
            currentSlider.ImageUrl = FileName;
        }
        

        currentSlider.UpdatedDate = DateTime.UtcNow.AddHours(4);
        currentSlider.Title = slider.Title;  
        currentSlider.Desc = slider.Desc;
        currentSlider.IsActivated= slider.IsActivated;
        await _context.SaveChangesAsync();
    }
    private IQueryable<Slider> _GetIncludes(IQueryable<Slider> query,params string[]includes)
    {
        if(includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;       
    }
}
