using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CartController : MainController
    {
        private readonly IPurchasingBffService _purchasinBffService;


        public CartController(IPurchasingBffService purchasinBffService)
        {
            _purchasinBffService = purchasinBffService;
        }

        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            return View(await _purchasinBffService.GetCart());
        }

        [HttpPost]
        [Route("cart/add-item")]
        public async Task<IActionResult> AddCartItem(ProductItemViewModel productItem)
        {
            var response = await _purchasinBffService.AddCartItem(productItem);

            if (HasErrorsResponse(response)) return View("Index", await _purchasinBffService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, int quantity)
        {

            var productItem = new ProductItemViewModel { ProductId = productId, Quantity = quantity };
            var response = await _purchasinBffService.UpdateCartItem(productId, productItem);

            if (HasErrorsResponse(response)) return View("Index", await _purchasinBffService.GetCart());

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("cart/remove-item")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var response = await _purchasinBffService.RemoveCartItem(productId);

            if (HasErrorsResponse(response)) return View("Index", await _purchasinBffService.GetCart());

            return RedirectToAction("Index");
        }
    }
}
