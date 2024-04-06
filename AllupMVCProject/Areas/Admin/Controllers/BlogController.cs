using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.Business.Services;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AllupMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        [HttpGet]
        public async Task< IActionResult> Index()
        {
            return View(await _blogService.GetAllAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _blogService.CreateAsync(blog);
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
            return View(await _blogService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Blog blog)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await _blogService.UpdateAsync(blog);
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
        public async Task< IActionResult> Delete(int id)
        {
            try
            {
                await _blogService.DeleteAsync(id);
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
