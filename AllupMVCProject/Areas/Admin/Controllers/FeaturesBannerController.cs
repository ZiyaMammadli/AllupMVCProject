using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.Business.Services;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AllupMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeaturesBannerController : Controller
    {
        private readonly IFeaturesBannerService _featuresBannerService;
        public FeaturesBannerController(IFeaturesBannerService featuresBannerService)
        {
            _featuresBannerService = featuresBannerService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _featuresBannerService.GetAllAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeaturesBanner featuresBanner)
        {
            if(!ModelState.IsValid) return View();

            try
            {
                await _featuresBannerService.CreateAsync(featuresBanner);
            }
            catch (AlreadyExistException ex)
            {
                ModelState.AddModelError(ex._PropertyName, ex.Message);
                return View();
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
            return View(await _featuresBannerService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(FeaturesBanner featuresBanner)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _featuresBannerService.UpdateAsync(featuresBanner);
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
                await _featuresBannerService.DeleteAsync(id);
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
