using AllupMVCProject.Models;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Interfaces
{
    public interface IBlogService
    {
        public Task<List<Blog>> GetAllAsync(Expression<Func<Blog, bool>>? expression = null, params string[] includes);
        public Task<Blog> GetSingleAsync(Expression<Func<Blog, bool>>? expression = null, params string[] includes);
        public Task<Blog> GetByIdAsync(int id);
        public Task CreateAsync(Blog blog);
        public Task UpdateAsync(Blog blog);
        public Task DeleteAsync(int id);
    }
}
