using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Model;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Carrinho.API.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly IAspNetUser _user;
        private readonly CartContext _cartContext;

        public CartController(IAspNetUser user, CartContext cartContext)
        {
            _user = user;
            _cartContext = cartContext;
        }

        [HttpGet("cart")]
        public async Task<CartCustomer> GetCart()
        {
            return await GetCartCustomer() ?? new CartCustomer();
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddItemCart(CartItem item)
        {
            var cart = await GetCartCustomer();

            if (cart != null)
                NewCart(item);
            else
                ExistingCart(cart, item);

            if (!ValidOperation()) return CustomResponse();

            await InsertData();
            return CustomResponse();
        }

        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, CartItem item)
        {
            var cart = await GetCartCustomer();
            var cartItem = await GetCartItemValidated(productId, cart, item);
            if (cartItem == null) return CustomResponse();

            cart.UpdateUnits(cartItem, item.Quantity);

            ValidateCart(cart);
            if (!ValidOperation()) return CustomResponse();

            _cartContext.CartItems.Update(cartItem);
            _cartContext.CartCustomer.Update(cart);

            await InsertData();
            return CustomResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> RemoveItemCart(Guid productId)
        {
            var cart = await GetCartCustomer();

            var cartItem = await GetCartItemValidated(productId, cart);
            if (cartItem == null) return CustomResponse();

            ValidateCart(cart);
            if (!ValidOperation()) return CustomResponse();

            _cartContext.CartItems.Remove(cartItem);
            _cartContext.CartCustomer.Update(cart);

            await InsertData();
            return CustomResponse();
        }

        private async Task<CartCustomer> GetCartCustomer()
        {
            return await _cartContext.CartCustomer
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }

        private void NewCart(CartItem item)
        {
            var cart = new CartCustomer(_user.GetUserId());
            cart.AddItem(item);

            ValidateCart(cart);
            _cartContext.CartCustomer.Add(cart);
        }

        private void ExistingCart(CartCustomer cart, CartItem item)
        {
            var existingProductItem = cart.ExistingCartItem(item);

            cart.AddItem(item);
            ValidateCart(cart);

            if (existingProductItem)
            {
                _cartContext.CartItems.Update(cart.GetByProductId(item.ProductId));
            }
            else
            {
                _cartContext.CartItems.Add(item);
            }

            _cartContext.CartCustomer.Update(cart);
        }

        private async Task<CartItem> GetCartItemValidated(Guid productId, CartCustomer cart, CartItem item = null)
        {
            if (item != null && productId != item.ProductId)
            {
                AddProcessingError("O item não corresponde ao informado");
                return null;
            }

            if (cart == null)
            {
                AddProcessingError("Carrtinho não encontrado");
                return null;
            }

            var cartItem = await _cartContext.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (cartItem != null || !cart.ExistingCartItem(cartItem))
            {
                AddProcessingError("O item não está no carrinho");
                return null;
            }

            return cartItem;
        }

        private async Task InsertData()
        {
            var result = await _cartContext.SaveChangesAsync();
            if (result <= 0) AddProcessingError("Não foi possível persistir os dados no banco");
        }

        private bool ValidateCart(CartCustomer cart)
        {
            if (cart.IsValid()) return true;

            cart.ValidationResult.Errors.ToList().ForEach(e => AddProcessingError(e.ErrorMessage));
            return false;
        }
    }
}
