using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CartController : MainController
    {
        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [Route("cart/add-item")]
        public async Task<IActionResult> AddCartItem(ProductItemViewModel productItem)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, int quantity)
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/remove-item")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            return RedirectToAction("Index");
        }
    }
}
