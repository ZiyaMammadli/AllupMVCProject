
using AllupMVCProject.DAL;
using AllupMVCProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AllupMVCProject.Controllers
{
    public class HomeController : Controller
	{
        private readonly AllupDbContext _context;
        public HomeController(AllupDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
            HomeViewModel homeVM = new HomeViewModel()
            {
                sliders=_context.Sliders.ToList(),
                banners=_context.Banners.ToList(),
                featuresBanners=_context.FeaturesBanners.ToList(),
                brands=_context.Brands.ToList(),
                blogs=_context.Blogs.ToList(),
                products=_context.Products.Include(p=>p.ProductImages).Include(p=>p.Category).Include(p=>p.Brand).ToList(),
                categories=_context.Categories.ToList(),
                productsImages=_context.ProductImages.ToList(),
            };
			return View(homeVM);
		}
	}
}
