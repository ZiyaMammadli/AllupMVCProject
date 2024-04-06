using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AllupMVCProject.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DashboardController : Controller
	{
        private readonly AllupDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DashboardController(AllupDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
		{
			return View();
		}

        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        FullName = "Eli Memmedov",
        //        UserName = "elimemmedov",
        //        Email = "eli@gmail.com"

        //    };
        //    string password = "Salam123@";
        //    var result = await _userManager.CreateAsync(admin, password);
        //    return Ok(result);
        //}

        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role1 = new IdentityRole("SuperAdmin");
        //    IdentityRole role2 = new IdentityRole("Admin");
        //    IdentityRole role3 = new IdentityRole("Member");

        //    await _roleManager.CreateAsync(role1);
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role3);
        //    return Ok();
        //}

        //public async Task<IActionResult> AddRole()
        //{
        //    AppUser admin = await _userManager.FindByNameAsync("elimemmedov");

        //    var result = await _userManager.AddToRoleAsync(admin, "Member");

        //    return Ok(result);
        //}
    }
}
