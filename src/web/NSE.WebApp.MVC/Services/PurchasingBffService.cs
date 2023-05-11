using Microsoft.Extensions.Options;
using NSE.Core.Comunication;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IPurchasingBffService
    {
        Task<CartViewModel> GetCart();
        Task<int> GetCartQuantity();
        Task<ResponseResult> AddCartItem(ItemCartViewModel productItemViewModel);
        Task<ResponseResult> UpdateCartItem(Guid productId, ItemCartViewModel productItemViewModel);
        Task<ResponseResult> RemoveCartItem(Guid productId);
        Task<ResponseResult> ApplyVocuherCart(string voucher);
    }
    public class PurchasingBffService : Service, IPurchasingBffService
    {
        private readonly HttpClient _httpClient;

        public PurchasingBffService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PurchasingBffUrl);
        }

        public async Task<CartViewModel> GetCart()
        {
            var response = await _httpClient.GetAsync("/purchasing/cart/");

            HandleErrorsResponse(response);

            return await DeserealizeObjectResponse<CartViewModel>(response);
        }

        public async Task<int> GetCartQuantity()
        {
            var response = await _httpClient.GetAsync("/purchasing/cart-quantity/");

            HandleErrorsResponse(response);

            return await DeserealizeObjectResponse<int>(response);
        }

        public async Task<ResponseResult> AddCartItem(ItemCartViewModel productItemViewModel)
        {
            var contentItem = SeralizeHttpContent(productItemViewModel);

            var response = await _httpClient.PostAsync("/purchasing/cart/items/", contentItem);

            if (!HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateCartItem(Guid productId, ItemCartViewModel productItemViewModel)
        {
            var contentItem = SeralizeHttpContent(productItemViewModel);

            var response = await _httpClient.PutAsync($"/purchasing/cart/items/{productItemViewModel.ProductId}", contentItem);

            if (!HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveCartItem(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/purchasing/cart/items/{productId}");

            if (!HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> ApplyVocuherCart(string voucher)
        {
            var itemContent = SeralizeHttpContent(voucher);

            var response = await _httpClient.PostAsync("/purchasing/cart/apply-voucher/", itemContent);

            if (!HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
