using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.Business.Services;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISliderService,SliderService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IBrandService,BrandService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IBannerService,BannerService>();
builder.Services.AddScoped<IFeaturesBannerService, FeaturesBannerService>();
builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequiredLength = 8;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireDigit = true;
    opt.User.RequireUniqueEmail = true;
})
              .AddEntityFrameworkStores<AllupDbContext>()
              .AddDefaultTokenProviders();

builder.Services.AddDbContext<AllupDbContext>(opt =>
{
	opt.UseSqlServer("Server=WIN-PRIFU0D7GO7\\SQLEXPRESS;Database=AllupDBContext;Trusted_Connection=true;TrustServerCertificate=True");
});

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "area",
	pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
