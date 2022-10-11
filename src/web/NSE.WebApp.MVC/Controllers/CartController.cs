using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CartController : MainController
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;

        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            return View(await _cartService.GetCart());
        }

        [HttpPost]
        [Route("cart/add-item")]
        public async Task<IActionResult> AddCartItem(ProductItemViewModel productItem)
        {
            var product = await _catalogService.GetById(productItem.ProductId);

            CartItemValidate(product, productItem.Quantity);
            if (!IsValid()) return View("Index", await _cartService.GetCart());

            productItem.Name = product.Name;
            productItem.Price = product.Value;
            productItem.Image = product.Image;

            var response = await _cartService.AddCartItem(productItem);

            if (HasErrorsResponse(response)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, int quantity)
        {
            var product = await _catalogService.GetById(productId);

            CartItemValidate(product, quantity);
            if (!IsValid()) return View("Index", await _cartService.GetCart());

            var productItem = new ProductItemViewModel { ProductId = productId, Quantity = quantity };
            var response = await _cartService.UpdateCartItem(productId, productItem);

            if (HasErrorsResponse(response)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/remove-item")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var product = await _catalogService.GetById(productId);

            if (product == null)
            {
                HandleErrorsResponse("Produto Inexistente");
                return View("Index", await _cartService.GetCart());
            }

            var response = await _cartService.RemoveCartItem(productId);

            if (HasErrorsResponse(response)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        private void CartItemValidate(ProductViewModel product, int quantity)
        {
            if (product == null) HandleErrorsResponse("Produto Inexistente");
            if (quantity < 1) HandleErrorsResponse($"Escolha ao menos uma unidade do produto {product.Name}");
            if (quantity > product.StockQuantity) HandleErrorsResponse($"O produto {product.Name} possui {product.StockQuantity} unidades em estoque, você selecionou {quantity}");
        }
    }
}
