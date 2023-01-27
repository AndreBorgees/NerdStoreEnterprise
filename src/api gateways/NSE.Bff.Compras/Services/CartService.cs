using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using NSE.Core.Comunication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{
    public interface ICartService
    {
        Task<CartDTO> GetCart();
        Task<ResponseResult> AddItemCart(CartItenDTO cartItem);
        Task<ResponseResult> UpdateItemCart(Guid productId, CartItenDTO cart);
        Task<ResponseResult> RemoveItemCart(Guid productId);
        Task<ResponseResult> ApplyVoucherCart(VoucherDTO voucher);
    }

    public class CartService : Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.CartUrl);
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _httpClient.GetAsync("/cart/");

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<CartDTO>(response);
        }

        public async Task<ResponseResult> AddItemCart(CartItenDTO cartItem)
        {
            var itemContent = GetContent(cartItem);

            var response = await _httpClient.PostAsync("/cart/", itemContent);

            if (!ProcessErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateItemCart(Guid productId, CartItenDTO cart)
        {
            var itemContent = GetContent(cart);

            var response = await _httpClient.PutAsync($"/cart/{cart.ProductId}", itemContent);

            if (!ProcessErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();

        }

        public async Task<ResponseResult> RemoveItemCart(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/cart/{productId}");

            if (!ProcessErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> ApplyVoucherCart(VoucherDTO voucher)
        {
            var itemContent = GetContent(voucher);

            var response = await _httpClient.PostAsync("/cart/apply-voucher/", itemContent);

            if (!ProcessErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
