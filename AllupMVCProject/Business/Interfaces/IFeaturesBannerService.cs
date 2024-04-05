using AllupMVCProject.Models;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Interfaces;

public interface IFeaturesBannerService
{
    public Task<List<FeaturesBanner>> GetAllAsync(Expression<Func<FeaturesBanner, bool>>? expression = null, params string[] includes);
    public Task<FeaturesBanner> GetSingleAsync(Expression<Func<FeaturesBanner, bool>>? expression = null, params string[] includes);
    public Task<FeaturesBanner> GetByIdAsync(int id);
    public Task CreateAsync(FeaturesBanner featuresBanner);
    public Task UpdateAsync(FeaturesBanner featuresBanner);
    public Task DeleteAsync(int id);
}
