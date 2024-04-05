using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Services
{
    public class FeaturesBannerService : IFeaturesBannerService
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        public FeaturesBannerService(AllupDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _env = webHostEnvironment;
        }
        public async Task CreateAsync(FeaturesBanner featuresBanner)
        {
            FeaturesBanner featuersBannerr = await _context.FeaturesBanners.FirstOrDefaultAsync(f => f.Title == featuresBanner.Title);

            if (featuersBannerr is not null)
            {
                throw new AlreadyExistException("Title", "This Title already exist!");
            }

            if (featuresBanner.ImageFile is null)
            {
                throw new RequiredPropertyException("ImageFile", "This area is required!");
            }

            if (featuresBanner.ImageFile.ContentType != "image/jpeg" && featuresBanner.ImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
            }
            if (featuresBanner.ImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string FileName = featuresBanner.ImageFile.FileName;
            if (FileName.Length > 64)
            {
                FileName = FileName.Substring(FileName.Length - 64, 64);
            }

            FileName = Guid.NewGuid().ToString() + FileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/FeaturesBanners", FileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                featuresBanner.ImageFile.CopyTo(fileStream);
            }

            featuresBanner.CreatedDate = DateTime.UtcNow.AddHours(4);
            featuresBanner.UpdatedDate = DateTime.UtcNow.AddHours(4);
            featuresBanner.ImageUrl = FileName;
            await _context.FeaturesBanners.AddAsync(featuresBanner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FeaturesBanner featuresBanner)
        {
            FeaturesBanner? currentFeaturesBanner = await _context.FeaturesBanners.FirstOrDefaultAsync(f => f.Id == featuresBanner.Id);

            if (featuresBanner is null)
            {
                throw new NotFoundException("FeaturesBanner is not found");
            }
            if (featuresBanner.ImageFile is not null)
            {
                if (featuresBanner.ImageFile.ContentType != "image/jpeg" && featuresBanner.ImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
                }
                if (featuresBanner.ImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string FileName = featuresBanner.ImageFile.FileName;
                if (FileName.Length > 64)
                {
                    FileName = FileName.Substring(FileName.Length - 64, 64);
                }

                FileName = Guid.NewGuid().ToString() + FileName;

                string path = Path.Combine(_env.WebRootPath, "Uploads/FeaturesBanners", FileName);

                using (FileStream filestream = new FileStream(path, FileMode.Create))
                {
                    featuresBanner.ImageFile.CopyTo(filestream);
                }

                string Lastpath = Path.Combine(_env.WebRootPath, "Uploads/FeaturesBanners", currentFeaturesBanner.ImageUrl);

                if (File.Exists(Lastpath))
                {
                    File.Delete(Lastpath);
                }
                currentFeaturesBanner.ImageUrl = FileName;
            }

            currentFeaturesBanner.Title = featuresBanner.Title;
            currentFeaturesBanner.Desc = featuresBanner.Desc;
            currentFeaturesBanner.UpdatedDate = DateTime.UtcNow.AddHours(4);
            currentFeaturesBanner.IsActivated = featuresBanner.IsActivated;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            FeaturesBanner? featuresBanner = await _context.FeaturesBanners.FirstOrDefaultAsync(f => f.Id == id);

            if (featuresBanner is null)
            {
                throw new NotFoundException("FeaturesBanner is not found");
            }

            string path = Path.Combine(_env.WebRootPath, "Uploads/FeaturesBanners", featuresBanner.ImageUrl);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _context.FeaturesBanners.Remove(featuresBanner);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FeaturesBanner>> GetAllAsync(Expression<Func<FeaturesBanner, bool>>? expression = null, params string[] includes)
        {
            var query = _context.FeaturesBanners.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).ToListAsync()
                : await query.ToListAsync();
        }

        public async Task<FeaturesBanner> GetByIdAsync(int id)
        {
            FeaturesBanner featuresBanner = await _context.FeaturesBanners.FindAsync(id);
            return featuresBanner;
        }

        public async Task<FeaturesBanner> GetSingleAsync(Expression<Func<FeaturesBanner, bool>>? expression = null, params string[] includes)
        {
            var query = _context.FeaturesBanners.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }


        private IQueryable<FeaturesBanner> _GetIncludes(IQueryable<FeaturesBanner> query, params string[] includes)
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
