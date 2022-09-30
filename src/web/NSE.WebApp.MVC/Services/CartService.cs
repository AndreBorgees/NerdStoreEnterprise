using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Services
{
    public class CartService: Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CartViewModel> GetCart()
        {
            var response = await _httpClient.GetAsync("/cart/");

            HandleErrorsResponse(response);

            return await DeserealizeObjectResponse<CartViewModel>(response);
        }

        public async Task<ResponseResult> AddCartItem(ProductItemViewModel productItemViewModel)
        {
            var contentItem = SeralizeHttpContent(productItemViewModel);

            var response = await _httpClient.PostAsync("/cart/", contentItem);

            if (HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateCartItem(Guid productId, ProductItemViewModel productItemViewModel)
        {
            var contentItem = SeralizeHttpContent(productItemViewModel);

            var response = await _httpClient.PutAsync($"/cart/{productItemViewModel.ProductId}", contentItem);

            if(HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveCartItem(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/cart/{productId}");

            if (HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
