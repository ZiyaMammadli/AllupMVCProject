using AllupMVCProject.Models;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Interfaces;

public interface IBrandService
{
    public Task<List<Brand>> GetAllAsync(Expression<Func<Brand, bool>>? expression = null, params string[] includes);
    public Task<Brand> GetSingleAsync(Expression<Func<Brand, bool>>? expression = null, params string[] includes);
    public Task<Brand> GetByIdAsync(int id);
    public Task CreateAsync(Brand brand);
    public Task UpdateAsync(Brand brand);
    public Task DeleteAsync(int id);
}
