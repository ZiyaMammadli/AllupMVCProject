using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AllupMVCProject.Business.Services
{
    public class BlogService : IBlogService
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogService(AllupDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _env = webHostEnvironment;
        }
        public async Task CreateAsync(Blog blog)
        {

            if (blog.Imagefile is null)
            {
                throw new RequiredPropertyException("Imagefile", "This area is required!");
            }

            if (blog.Imagefile.ContentType != "image/jpeg" && blog.Imagefile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("Imagefile", "Please,You enter jpeg or png file");
            }
            if (blog.Imagefile.Length > 2097152)
            {
                throw new SizeOfFileException("Imagefile", "Please,You just can send low size file from 2 mb!");
            }
            string FileName = blog.Imagefile.FileName;
            if (FileName.Length > 64)
            {
                FileName = FileName.Substring(FileName.Length - 64, 64);
            }

            FileName = Guid.NewGuid().ToString() + FileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/Blogs", FileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                blog.Imagefile.CopyTo(fileStream);
            }

            blog.CreatedDate = DateTime.UtcNow.AddHours(4);
            blog.UpdatedDate = DateTime.UtcNow.AddHours(4);
            blog.ImageUrl = FileName;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Blog blog)
        {
            Blog? currentBlog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == blog.Id);

            if (currentBlog is null)
            {
                throw new NotFoundException("Blog is not found");
            }
            if (blog.Imagefile is not null)
            {
                if (blog.Imagefile.ContentType != "image/jpeg" && blog.Imagefile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("Imagefile", "Please,You enter jpeg or png file");
                }
                if (blog.Imagefile.Length > 2097152)
                {
                    throw new SizeOfFileException("Imagefile", "Please,You just can send low size file from 2 mb!");
                }
                string FileName = blog.Imagefile.FileName;
                if (FileName.Length > 64)
                {
                    FileName = FileName.Substring(FileName.Length - 64, 64);
                }

                FileName = Guid.NewGuid().ToString() + FileName;

                string path = Path.Combine(_env.WebRootPath, "Uploads/Blogs", FileName);

                using (FileStream filestream = new FileStream(path, FileMode.Create))
                {
                    blog.Imagefile.CopyTo(filestream);
                }

                string Lastpath = Path.Combine(_env.WebRootPath, "Uploads/Blogs", currentBlog.ImageUrl);

                if (File.Exists(Lastpath))
                {
                    File.Delete(Lastpath);
                }
                currentBlog.ImageUrl = FileName;
            }

            currentBlog.RedirectUrl = blog.RedirectUrl;
            currentBlog.Title = blog.Title;
            currentBlog.Desc = blog.Desc;
            currentBlog.UpdatedDate = DateTime.UtcNow.AddHours(4);
            currentBlog.IsActivated = blog.IsActivated;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null)
            {
                throw new NotFoundException("Blog is not found");
            }

            string path = Path.Combine(_env.WebRootPath, "Uploads/Blogs", blog.ImageUrl);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Blog>> GetAllAsync(Expression<Func<Blog, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Blogs.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).ToListAsync()
                : await query.ToListAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            Blog blog = await _context.Blogs.FindAsync(id);
            return blog;
        }

        public async Task<Blog> GetSingleAsync(Expression<Func<Blog, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Blogs.AsQueryable();
            query = _GetIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }

        private IQueryable<Blog> _GetIncludes(IQueryable<Blog> query, params string[] includes)
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
