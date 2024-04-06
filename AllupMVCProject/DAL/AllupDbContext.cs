using AllupMVCProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AllupMVCProject.DAL
{
	public class AllupDbContext:IdentityDbContext<AppUser>
	{
        public AllupDbContext(DbContextOptions <AllupDbContext> option) : base(option) { }

		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Banner> Banners { get; set; }
		public DbSet<FeaturesBanner> FeaturesBanners { get; set; }
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<AppUser> Users { get; set; }
    }
}
