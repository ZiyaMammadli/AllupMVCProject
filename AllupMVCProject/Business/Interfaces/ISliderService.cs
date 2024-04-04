using AllupMVCProject.Models;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Interfaces;

public interface ISliderService
{
    public Task <List<Slider>> GetAllAsync(Expression <Func<Slider,bool>>? expression=null,params string[] includes);
    public Task<Slider>GetSingleAsync(Expression <Func<Slider,bool>>? expression=null,params string[]includes);
    public Task <Slider> GetByIdAsync(int id);
    public Task CreateAsync(Slider slider);
    public Task UpdateAsync(Slider slider);
    public Task DeleteAsync(int id);


}
