using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Services
{
    public class BrandService : IBrandService
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandService(AllupDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task CreateAsync(Brand brand)
        {
            Brand brandd = await _context.Brands.FirstOrDefaultAsync(b => b.Name == brand.Name);

            if (brandd is not null)
            {
                throw new AlreadyExistException("Name", "This Name already exist!");
            }

            if(brand.LogoImageFile is null)
            {
                throw new RequiredPropertyException("LogoImageFile", "This area is required!");
            }

            if (brand.LogoImageFile.ContentType != "image/jpeg" && brand.LogoImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("LogoImageFile", "Please,You enter jpeg or png file");
            }
            if (brand.LogoImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("LogoImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string FileName = brand.LogoImageFile.FileName;
            if (FileName.Length > 64)
            {
                FileName = FileName.Substring(FileName.Length - 64, 64);
            }

            FileName = Guid.NewGuid().ToString() + FileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/Brands", FileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                brand.LogoImageFile.CopyTo(fileStream);
            }

            brand.CreatedDate = DateTime.UtcNow.AddHours(4);
            brand.UpdatedDate = DateTime.UtcNow.AddHours(4);
            brand.LogoUrl=FileName;
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Brand brand)
        {
            Brand? currentBrand = await _context.Brands.FirstOrDefaultAsync(cb => cb.Id == brand.Id);

            if (currentBrand is null)
            {
                throw new NotFoundException("Brand is not found");
            }
            if (brand.LogoImageFile is not null)
            {
                if (brand.LogoImageFile.ContentType != "image/jpeg" && brand.LogoImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("LogoImageFile", "Please,You enter jpeg or png file");
                }
                if (brand.LogoImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("LogoImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string FileName = brand.LogoImageFile.FileName;
                if (FileName.Length > 64)
                {
                    FileName = FileName.Substring(FileName.Length - 64, 64);
                }

                FileName = Guid.NewGuid().ToString() + FileName;

                string path = Path.Combine(_env.WebRootPath, "Uploads/Brands", FileName);

                using (FileStream filestream = new FileStream(path, FileMode.Create))
                {
                    brand.LogoImageFile.CopyTo(filestream);
                }

                string Lastpath = Path.Combine(_env.WebRootPath, "Uploads/Brands", currentBrand.LogoUrl);

                if (File.Exists(Lastpath))
                {
                    File.Delete(Lastpath);
                }
                currentBrand.LogoUrl = FileName;
            }

            currentBrand.UpdatedDate= DateTime.UtcNow.AddHours(4);
            currentBrand.Name=brand.Name;
            currentBrand.IsActivated=brand.IsActivated;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            Brand? brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand is null)
            {
                throw new NotFoundException("Brand is not found");
            }

            string path = Path.Combine(_env.WebRootPath, "Uploads/Brands", brand.LogoUrl);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Brand>> GetAllAsync(Expression<Func<Brand, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Brands.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).ToListAsync()
                : await query.ToListAsync();
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            Brand brand = await _context.Brands.FindAsync(id);
            return brand;
        }

        public async Task<Brand> GetSingleAsync(Expression<Func<Brand, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Brands.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }   
        private IQueryable<Brand> _GetIncludes(IQueryable<Brand> query, params string[] includes)
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
}
