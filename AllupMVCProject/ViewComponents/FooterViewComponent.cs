using AllupMVCProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllupMVCProject.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly IBrandService _brandService;
        public FooterViewComponent(IBrandService brandService)
        {
            _brandService = brandService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands=await _brandService.GetAllAsync(null,null);

            return View(brands);
        }
    }
}
