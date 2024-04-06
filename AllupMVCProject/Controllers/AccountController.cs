using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupMVCProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AllupDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, AllupDbContext context, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel RegisterVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (await _context.Users.AnyAsync(a => a.NormalizedUserName == RegisterVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This username already exist");
                return View();
            }
            if (await _context.Users.AnyAsync(a => a.NormalizedEmail == RegisterVM.Email.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This email already exist");
                return View();
            }
            if (await _context.Users.AnyAsync(a => a.PhoneNumber == RegisterVM.PhoneNumber))
            {
                ModelState.AddModelError("UserName", "This phone number already exist");
                return View();
            }
            AppUser member = new AppUser()
            {
                FullName = RegisterVM.UserName,
                UserName = RegisterVM.UserName,
                Email = RegisterVM.Email,
                PhoneNumber = RegisterVM.PhoneNumber,
            };
            string password = RegisterVM.Password;

            var result = await _userManager.CreateAsync(member, password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }

            var result1 = await _userManager.AddToRoleAsync(member, "Member");

            if (!result1.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
    }
}
