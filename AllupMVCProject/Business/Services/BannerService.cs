using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Services;

public class BannerService : IBannerService
{
    private readonly AllupDbContext _context;
    private readonly IWebHostEnvironment _env;
    public BannerService(AllupDbContext context,IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _env = webHostEnvironment;
    }
    public async Task CreateAsync(Banner banner)
    {

        if (banner.ImageFile is null)
        {
            throw new RequiredPropertyException("ImageFile", "This area is required!");
        }

        if (banner.ImageFile.ContentType != "image/jpeg" && banner.ImageFile.ContentType != "image/png")
        {
            throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
        }
        if (banner.ImageFile.Length > 2097152)
        {
            throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
        }
        string FileName = banner.ImageFile.FileName;
        if (FileName.Length > 64)
        {
            FileName = FileName.Substring(FileName.Length - 64, 64);
        }

        FileName = Guid.NewGuid().ToString() + FileName;

        string path = Path.Combine(_env.WebRootPath, "Uploads/Banners", FileName);

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            banner.ImageFile.CopyTo(fileStream);
        }

        banner.CreatedDate = DateTime.UtcNow.AddHours(4);
        banner.UpdatedDate = DateTime.UtcNow.AddHours(4);
        banner.ImageUrl = FileName;
        await _context.Banners.AddAsync(banner);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Banner banner)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Banner>> GetAllAsync(Expression<Func<Banner, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Banners.AsQueryable();
        query = _GetIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).ToListAsync()
            : await query.ToListAsync();
    }

    public async Task<Banner> GetByIdAsync(int id)
    {
        Banner banner = await _context.Banners.FindAsync(id);
        return banner;
    }

    public async Task<Banner> GetSingleAsync(Expression<Func<Banner, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Banners.AsQueryable();
        query = _GetIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();
    }

    private IQueryable<Banner> _GetIncludes(IQueryable<Banner> query, params string[] includes)
    {
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
    }

}
