using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;

namespace AllupMVCProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly AllupDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductService _productService;
        public ShopController(AllupDbContext Context,UserManager<AppUser> userManager,IProductService productService)
        {
            _context = Context;
            _userManager = userManager;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddToBasket(int productId)
        {
            if (!await _context.Products.AnyAsync(b => b.Id == productId)) return NotFound();

            List<BasketViewModel> basketVMs = new List<BasketViewModel>();
            BasketViewModel basketVM = null;
            AppUser appUser = null;
            var basketVMstr = HttpContext.Request.Cookies["basketVMs"];

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                if (basketVMstr is not null)
                {
                    basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVMstr);

                    var basketViewM = basketVMs.FirstOrDefault(bvm => bvm.ProductId == productId);

                    if (basketViewM is not null)
                    {
                        basketViewM.Count++;

                    }
                    else
                    {
                        basketVM = new BasketViewModel
                        {
                            ProductId = productId,
                            Count = 1
                        };
                        basketVMs.Add(basketVM);
                    }
                }
                else
                {
                    basketVM = new BasketViewModel
                    {
                        ProductId = productId,
                        Count = 1
                    };
                    basketVMs.Add(basketVM);
                }

                var BasketVMstr = JsonConvert.SerializeObject(basketVMs);

                HttpContext.Response.Cookies.Append("basketVMs", BasketVMstr);
            }
            else
            {
                string Username = HttpContext.User.Identity.Name;
                appUser = await _userManager.FindByNameAsync(Username);
                var basketItem = await _context.BasketItems.FirstOrDefaultAsync(bi => bi.AppUserId == appUser.Id && bi.ProductId == productId);
                if (basketItem is not null)
                {
                    basketItem.Count++;
                }
                else
                {
                    BasketItem BasketItem = new BasketItem
                    {
                        AppUserId = appUser.Id,
                        ProductId = productId,
                        Count = 1,
                        IsActivated = true,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow,
                    };
                    _context.BasketItems.Add(BasketItem);
                }
            }
            _context.SaveChanges();

            return Ok();
        }
        public IActionResult GetBasket()
        {
            List<BasketViewModel> BasketVMs = new List<BasketViewModel>();

            var basketVMs = HttpContext.Request.Cookies["BasketVMs"];
            if (basketVMs is not null)
            {
                BasketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVMs);
            }


            return Ok(BasketVMs);
        }
        public async Task<IActionResult> RemoveBasket(int productId)
        {
            if (!await _context.Products.AnyAsync(b => b.Id == productId)) return NotFound();

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                var BasketItems =await _context.BasketItems.Where(bi => bi.ProductId == productId).ToListAsync();

                List<BasketViewModel> BasketVMs = new List<BasketViewModel>();

                var basketVM = HttpContext.Request.Cookies["basketVMs"];

                if (basketVM is not null)
                {
                    BasketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVM);

                    var BasketVM2 = BasketVMs.Where(bvm => bvm.Count == 1 && bvm.ProductId == productId).FirstOrDefault();
                    if (BasketVM2 is not null)
                    {
                        BasketVMs.Remove(BasketVM2);
                    }
                    var BasketVM = BasketVMs.Where(bvm => bvm.Count > 1 && bvm.ProductId == productId).FirstOrDefault();
                    if (BasketVM is not null)
                    {
                        BasketVM.Count--;
                    }
                }
                var BasketVMstr = JsonConvert.SerializeObject(BasketVMs);

                HttpContext.Response.Cookies.Append("basketVMs", BasketVMstr);
            }
            else
            {
                var username=HttpContext.User.Identity.Name;
                AppUser user = await _userManager.FindByNameAsync(username);
                if(user is not null)
                {
                    var basketItem=await _context.BasketItems.Where(bi=>bi.ProductId == productId && bi.AppUserId==user.Id).FirstOrDefaultAsync();
                    if (basketItem is not null && basketItem.Count>1) 
                    {
                        basketItem.Count--;
                    }
                    else if (basketItem is not null && basketItem.Count == 1)
                    {
                        _context.BasketItems.Remove(basketItem);
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                else
                {
                    return NotFound();
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("checkout");
        }
        public async Task<IActionResult> CheckOut()
        {
            List<CheckOutViewModel> checkOutVMs = new List<CheckOutViewModel>();
            var basketVM = HttpContext.Request.Cookies["basketVMs"];
            double TotalPrice = 0;
            AppUser user = null;
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                if (basketVM is not null)
                {
                    var basketVMs = JsonConvert.DeserializeObject<List<BasketViewModel>>(basketVM);
                    foreach (var basketvm in basketVMs)
                    {
                        Product product = await _context.Products.Where(b => b.Id == basketvm.ProductId).FirstOrDefaultAsync();
                        CheckOutViewModel checkOutVM = new CheckOutViewModel()
                        {
                            ProductId = basketvm.ProductId,
                            ProductCount = basketvm.Count,
                            ProductName = product.Name,
                            ProductPrice = product.SalePrice * basketvm.Count,
                        };

                        TotalPrice += product.SalePrice * basketvm.Count;
                        checkOutVMs.Add(checkOutVM);
                    }
                }
            }
            else
            {
                string Username = HttpContext.User.Identity.Name;
                user = await _userManager.FindByNameAsync(Username);
                if (user is not null)
                {
                    var basketItems = await _context.BasketItems.Where(bi => bi.AppUserId == user.Id).ToListAsync();
                    if (basketItems is not null)
                    {
                        foreach (var basketItem in basketItems)
                        {
                            Product product = await _context.Products.Where(b => b.Id == basketItem.ProductId).FirstOrDefaultAsync();
                            CheckOutViewModel checkOutVM = new CheckOutViewModel()
                            {
                                ProductId = basketItem.ProductId,
                                ProductCount = basketItem.Count,
                                ProductName = product.Name,
                                ProductPrice = product.SalePrice * basketItem.Count,
                            };

                            TotalPrice += product.SalePrice * basketItem.Count;
                            checkOutVMs.Add(checkOutVM);
                        }
                    }
                }
            }


            ViewData["user"] = user;
            ViewData["TotalPrice"] = TotalPrice;

            return View(checkOutVMs);
        }
        [HttpGet]
        public async Task< IActionResult> Detail(int ProductId)
        {
            var product=await _productService.GetSingleAsync(p=>p.Id==ProductId,"Category","Brand","ProductImages");
            var products=await _productService.GetAllAsync(null,"Category","Brand","ProductImages");

            ViewData["products"] = products;

            return View(product);
        }
    }
}
