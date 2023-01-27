using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {

        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService,
            ICatalogService catalogService,
            IOrderService orderService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
            _orderService = orderService;
        }

        [HttpGet]
        [Route("purchasing/cart")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _cartService.GetCart());
        }

        [HttpGet]
        [Route("purchasing/cart-quantity")]
        public async Task<int> GetCartQuantity()
        {
            var quantity = await _cartService.GetCart();
            return quantity?.Items.Sum(i => i.Quantity) ?? 0;
        }

        [HttpPost]
        [Route("purchasing/cart/items")]
        public async Task<IActionResult> AddCartItem(CartItenDTO itemCart)
        {
            var product = await _catalogService.GetById(itemCart.ProductId);

            await ValidateItemCart(product, itemCart.Quantity);
            if (!ValidOperation()) return CustomResponse();

            itemCart.Name = product.Name;
            itemCart.Price = product.Value;
            itemCart.Image = product.Image;

            var response = await _cartService.AddItemCart(itemCart);
            return CustomResponse(response);
        }

        [HttpPut]
        [Route("purchasing/cart/items/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItenDTO itemCart)
        {
            var product = await _catalogService.GetById(itemCart.ProductId);

            await ValidateItemCart(product, itemCart.Quantity);
            if (!ValidOperation()) return CustomResponse();

            var resposen = await _cartService.UpdateItemCart(productId, itemCart);
            return CustomResponse(resposen);
        }

        [HttpDelete]
        [Route("purchasing/cart/items/{productId}")]
        public async Task<IActionResult> DeleteCartItem(Guid productId)
        {
            var product = await _catalogService.GetById(productId);

            if (product == null)
            {
                AddProcessingError("Produto inexistente!");
                return CustomResponse();
            }

            var resposen = await _cartService.RemoveItemCart(productId);
            return CustomResponse();
        }

        [HttpPost]
        [Route("purchasing/cart/apply-voucher")]
        public async Task<IActionResult> ApplyVoucher([FromBody] string voucherCode)
        {
            var voucher = await _orderService.GetVoucherByCode(voucherCode);

            if(voucher is null)
            {
                AddProcessingError("Vouhcer inválido ou não encontrado!");
                return CustomResponse();
            }

            var response = await _cartService.ApplyVoucherCart(voucher);

            return CustomResponse(response);
        }

        private async Task ValidateItemCart(ProductItemDTO productItem, int quantity)
        {
            if (productItem == null) AddProcessingError("Produto inexistente!");
            if (quantity < 1) AddProcessingError($"Escolha ao menos uma uniade do produto {productItem.Name}");

            var cart = await _cartService.GetCart();
            var itemCart = cart.Items.FirstOrDefault(p => p.ProductId == productItem.Id);

            if (itemCart != null && itemCart.Quantity + quantity > productItem.StockQuantity)
            {
                AddProcessingError($"O produto {productItem.Name} possui {productItem.StockQuantity} unidades em estoque, você selecionou {quantity}");
                return;
            }

            if (quantity > productItem.StockQuantity) AddProcessingError($"O produto {productItem.Name} possui {productItem.StockQuantity} unidades em estoque, você selecionou {quantity}");
        }
    }
}
