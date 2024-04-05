using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryService(AllupDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env; 
        }
        public async Task CreateAsync(Category category)
        {
            Category categoryy = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category.Name);

            if (categoryy is not null)
            {
                throw new AlreadyExistException("Name", "This Name already exist!");
            }

            if (category.ImageFile is null)
            {
                throw new RequiredPropertyException("ImageFile", "This area is required!");
            }

            if (category.ImageFile.ContentType != "image/jpeg" && category.ImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
            }
            if (category.ImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string FileName = category.ImageFile.FileName;
            if (FileName.Length > 64)
            {
                FileName = FileName.Substring(FileName.Length - 64, 64);
            }

            FileName = Guid.NewGuid().ToString() + FileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/Categories", FileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                category.ImageFile.CopyTo(fileStream);
            }

            category.CreatedDate = DateTime.UtcNow.AddHours(4);
            category.UpdatedDate = DateTime.UtcNow.AddHours(4);
            category.ImageUrl=FileName;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();  
        }

        public async Task UpdateAsync(Category category)
        {
            Category? currentCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (currentCategory is null)
            {
                throw new NotFoundException("Category is not found");
            }
            if (category.ImageFile is not null)
            {
                if (category.ImageFile.ContentType != "image/jpeg" && category.ImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
                }
                if (category.ImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string FileName = category.ImageFile.FileName;
                if (FileName.Length > 64)
                {
                    FileName = FileName.Substring(FileName.Length - 64, 64);
                }

                FileName = Guid.NewGuid().ToString() + FileName;

                string path = Path.Combine(_env.WebRootPath, "Uploads/Categories", FileName);

                using (FileStream filestream = new FileStream(path, FileMode.Create))
                {
                    category.ImageFile.CopyTo(filestream);
                }

                string Lastpath = Path.Combine(_env.WebRootPath, "Uploads/Categories", currentCategory.ImageUrl);

                if (File.Exists(Lastpath))
                {
                    File.Delete(Lastpath);
                }
                currentCategory.ImageUrl = FileName;
            }

            currentCategory.Name=category.Name;
            currentCategory.UpdatedDate = DateTime.UtcNow.AddHours(4);
            currentCategory.IsActivated=category.IsActivated;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                throw new NotFoundException("Category is not found");
            }

            string path = Path.Combine(_env.WebRootPath, "Uploads/Categories", category.ImageUrl);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
        {
            var query=_context.Categories.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).ToListAsync()
                : await query.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            return category;
        }

        public async Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Categories.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }

        private IQueryable<Category> _GetIncludes(IQueryable<Category> query, params string[] includes)
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
