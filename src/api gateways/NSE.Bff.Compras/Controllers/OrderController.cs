using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Controllers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Controllers
{
    [Authorize]
    public class OrderController: BaseController
    {
        private readonly ICatalogService _catalogService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(
            ICatalogService catalogService, 
            ICartService cartService, 
            IOrderService orderService, 
            ICustomerService customerService)
        {
            _catalogService = catalogService;
            _cartService = cartService;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost]
        [Route("purchasing/order")]
        public async Task<IActionResult> AddOrder(OrderDTO orderDTO)
        {
            var cart = await _cartService.GetCart();
            var products = await _catalogService.GetItems(cart.Items.Select(p => p.ProductId));
            var address = await _customerService.GetAddress();

            if (! await ValidateCartProducts(cart, products)) return CustomResponse();

            PopulateDataOrder(cart, address, orderDTO);

            return CustomResponse(await _orderService.Checkout(orderDTO));
        }

        [HttpGet]
        [Route("purchasing/order/last")]
        public async Task<IActionResult> GetLastOrder()
        {
            var order = await _orderService.GetLastOrder();

            if(order is null)
            {
                AddProcessingError("Pedido não encontrado!");
            }

            return CustomResponse(order);
        }

        [HttpGet]
        [Route("purchasing/order/get-customer")]
        public async Task<IActionResult> GetListOrderByClientId()
        {
            var order = await _orderService.GetListOrderByClientId();

            return order == null ? NotFound() : CustomResponse(order);
        }

        private async Task<bool> ValidateCartProducts(CartDTO cart, IEnumerable<ProductItemDTO> products)
        {
            if(cart.Items.Count != products.Count())
            {
                var unavailableItems = cart.Items.Select(c => c.ProductId).Except(products.Select(p => p.Id)).ToList();

                foreach (var itemId in unavailableItems)
                {
                    var cartItem = cart.Items.FirstOrDefault(c => c.ProductId == itemId);
                    AddProcessingError($"O item {cartItem.Name} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
                }

                return false;
            }

            foreach (var cartItem in cart.Items)
            {
                var productCatalog = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                if (productCatalog.Value != cartItem.Price)
                {
                    var errorMessage = $"O produto {cartItem.Name} mudou de valor (de: " +
                                 $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", cartItem.Price)} para: " +
                                 $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", productCatalog.Value)}) desde que foi adicionado ao carrinho.";

                    AddProcessingError(errorMessage);

                    var responseRemove = await _cartService.RemoveItemCart(cartItem.ProductId);

                    if (ResponseHasErrors(responseRemove))
                    {
                        AddProcessingError($"Não foi possível remover automaticamente o produto {cartItem.Name} do seu carrinho, _" +
                                                   "remova e adicione novamente caso ainda deseje comprar este item");

                        return false;
                    }

                    cartItem.Price = productCatalog.Value;
                    var responseAdd = await _cartService.AddItemCart(cartItem);

                    if (ResponseHasErrors(responseAdd))
                    {
                        AddProcessingError($"Não foi possível atualizar automaticamente o produto {cartItem.Name} do seu carrinho, _" +
                                                   "adicione novamente caso ainda deseje comprar este item");

                        return false;
                    }

                    CelarProcessingError();
                    AddProcessingError(errorMessage + "Atualizamos o valor em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                    return false;
                }
            }

            return true;
        }

        private void PopulateDataOrder(CartDTO cart, AddressDTO address, OrderDTO order)
        {
            order.VocuherCode = cart.Voucher?.Code;
            order.UsedVoucher = cart.UsedVoucher;
            order.TotalValue = cart.TotalPrice;
            order.Discount = cart.Discount;
            order.OrderItems = cart.Items;

            order.Address = address;
        }
    }
}
