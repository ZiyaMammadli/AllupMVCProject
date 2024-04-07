using AllupMVCProject.Models;

namespace AllupMVCProject.ViewModels;

public class HomeViewModel
{
    public List<Slider> sliders { get; set; }
    public List<Banner> banners { get; set; }
    public List <FeaturesBanner> featuresBanners { get; set; }
    public List<Brand> brands { get; set; }
    public List<Blog> blogs { get; set; }   
    public List<Product> products { get; set; }
    public List<Category> categories { get; set; }
    public List<ProductImage> productsImages { get; set; }
}
