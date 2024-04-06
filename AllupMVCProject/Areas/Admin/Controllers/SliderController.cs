using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AllupMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        public async Task <IActionResult> Index()
        {       
            return View(await _sliderService.GetAllAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> CreateAsync(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            try
            {
               await _sliderService.CreateAsync(slider);
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
            catch(SizeOfFileException ex)
            {
                ModelState.AddModelError(ex._PropertName, ex.Message);
                return View();
            }
            catch(Exception ex) 
            { 
                ModelState.AddModelError("",ex.Message);
                return View();
            }
            return RedirectToAction("index");
        }
        [HttpGet]
        public async Task <IActionResult> Update(int id)
        {
            return View(await _sliderService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            try
            {
                await _sliderService.UpdateAsync(slider);
            }
            catch (NotFoundException ex)
            {

                ModelState.AddModelError("",ex.Message);
                return View();
            }
            catch(InvalidContentTypeException ex)
            {
                ModelState.AddModelError(ex._PropertyName,ex.Message);
                return View();
            }
            catch(SizeOfFileException ex)
            {
                ModelState.AddModelError(ex._PropertName,ex.Message);
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
        [Authorize(Roles = "SuperAdmin")]
        public async Task <IActionResult> Delete(int id)
        {
            try
            {
                await _sliderService.DeleteAsync(id);
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
