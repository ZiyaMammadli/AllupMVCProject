using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AllupMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        public ProductController(IProductService productService,ICategoryService categoryService,IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAllAsync(null,"Category","Brand","ProductImages"));
        }
        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            ViewData["category"] = await _categoryService.GetAllAsync();
            ViewData["brand"] = await _brandService.GetAllAsync();
            return View( );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product) 
        {
            ViewData["category"] = await _categoryService.GetAllAsync();
            ViewData["brand"] = await _brandService.GetAllAsync();
            if (!ModelState.IsValid) return View();

            try
            {
                await _productService.CreateAsync(product);
            }
            catch(RequiredPropertyException ex)
            {
                ModelState.AddModelError(ex._PropertyName, ex.Message);
                return View();
            }
            catch(InvalidContentTypeException ex)
            {
                ModelState.AddModelError(ex._PropertyName, ex.Message);
                return View();
            }  
            catch(SizeOfFileException ex)
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
        public async Task <IActionResult> Update(int id)
        {
            ViewData["category"] = await _categoryService.GetAllAsync();
            ViewData["brand"] = await _brandService.GetAllAsync();
            Product? product = null;
            try
            {
                product = await _productService.GetSingleAsync(b => b.Id == id, "Category", "Brand", "ProductImages");
            }
            catch (Exception)
            {

                throw;
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product)
        {
            ViewData["category"] = await _categoryService.GetAllAsync();
            ViewData["brand"] = await _brandService.GetAllAsync();

            if (!ModelState.IsValid) return View();
            try
            {
                await _productService.UpdateAsync(product);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (AlreadyExistException ex)
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

            return RedirectToAction("index");

        }
        [HttpGet]
        public async Task <IActionResult> Delete(int id)
        {
            try
            {
               await _productService.DeleteAsync(id);

            }
            catch(NotFoundException ex)
            {
                ModelState.AddModelError("",ex.Message);
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
