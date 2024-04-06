using Microsoft.AspNetCore.Mvc;

namespace AllupMVCProject.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
