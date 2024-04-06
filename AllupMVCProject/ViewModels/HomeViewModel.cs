using AllupMVCProject.Models;

namespace AllupMVCProject.ViewModels;

public class HomeViewModel
{
    public List<Slider> sliders { get; set; }
    public List<Banner> banners { get; set; }
    public List <FeaturesBanner> featuresBanners { get; set; }
    public List<Brand> brands { get; set; }
    public List<Blog> blogs { get; set; }   
}
