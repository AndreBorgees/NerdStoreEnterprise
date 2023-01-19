using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPurchasingBffService _purchasingBffService;

        public CartViewComponent(IPurchasingBffService purchasingBffService)
        {
            _purchasingBffService = purchasingBffService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _purchasingBffService.GetCartQuantity());
        }
    }
}
