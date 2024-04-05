using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.Business.Services;
using AllupMVCProject.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISliderService,SliderService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<IBrandService,BrandService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddDbContext<AllupDbContext>(opt =>
{
	opt.UseSqlServer("Server=WIN-PRIFU0D7GO7\\SQLEXPRESS;Database=AllupDb;Trusted_Connection=true;TrustServerCertificate=True");
});

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
	name: "area",
	pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
