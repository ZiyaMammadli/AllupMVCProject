using AllupMVCProject.Models;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Interfaces;

public interface IBannerService
{
    public Task<List<Banner>> GetAllAsync(Expression<Func<Banner, bool>>? expression = null, params string[] includes);
    public Task<Banner> GetSingleAsync(Expression<Func<Banner, bool>>? expression = null, params string[] includes);
    public Task<Banner> GetByIdAsync(int id);
    public Task CreateAsync(Banner banner);
    public Task UpdateAsync(Banner banner);
    public Task DeleteAsync(int id);
}
