using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.Business.Services;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AllupMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;
        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        [HttpGet]
        public async Task <IActionResult> Index()
        {
            return View(await _bannerService.GetAllAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner)
        {
            if(!ModelState.IsValid) return View();

            try
            {
                await _bannerService.CreateAsync(banner);
            }
            catch (RequiredPropertyException ex)
            {
                ModelState.AddModelError(ex._PropertyName, ex.Message);
                return View();
            }
            catch (InvalidContentTypeException ex)
            {

                ModelState.AddModelError(ex._PropertyName, ex.Message);
                return View();
            }
            catch (SizeOfFileException ex)
            {
                ModelState.AddModelError(ex._PropertName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            return View(await _bannerService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Banner banner)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _bannerService.UpdateAsync(banner);
            }
            catch (NotFoundException ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (InvalidContentTypeException ex)
            {
                ModelState.AddModelError(ex._PropertyName, ex.Message);
                return View();
            }
            catch (SizeOfFileException ex)
            {
                ModelState.AddModelError(ex._PropertName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return RedirectToAction("index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bannerService.DeleteAsync(id);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return NotFound();
            }
            return Ok();
        }


    }
}
