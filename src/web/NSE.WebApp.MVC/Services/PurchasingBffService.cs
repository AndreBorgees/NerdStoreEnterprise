using Microsoft.Extensions.Options;
using NSE.Core.Comunication;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
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

        Task<ResponseResult> Checkout(OerderTransactionViewModel oerderTransactionViewModel);
        Task<OrderViewModel> GetLastOrder();
        Task<IEnumerable<OrderViewModel>> GetOrderByCustomerId();
        OerderTransactionViewModel MapForOrder(CartViewModel cartViewModel, AddressViewModel addressViewModel);
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

        public async Task<ResponseResult> Checkout(OerderTransactionViewModel oerderTransactionViewModel)
        {
            var orderDTOContent = SeralizeHttpContent(oerderTransactionViewModel);

            var response = await _httpClient.PostAsync("/purchasing/order/", orderDTOContent);

            if(!HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<OrderViewModel> GetLastOrder()
        {
            var response = await _httpClient.GetAsync("/purchasing/order/last/");

            HandleErrorsResponse(response);

            return await DeserealizeObjectResponse<OrderViewModel>(response);
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrderByCustomerId()
        {
            var response = await _httpClient.GetAsync("/purchasing/order/get-customer/");

            HandleErrorsResponse(response);

            return await DeserealizeObjectResponse<IEnumerable<OrderViewModel>>(response);
        }

        public OerderTransactionViewModel MapForOrder(CartViewModel cartViewModel, AddressViewModel addressViewModel)
        {
            var order = new OerderTransactionViewModel
            {
                TotalValue = cartViewModel.TotalPrice,
                OrderItems = cartViewModel.Items,
                Discount = cartViewModel.Discount,
                UsedVoucher = cartViewModel.UsedVoucher,
                VoucherCode = cartViewModel.Voucher?.Code
            };

            if (addressViewModel != null)
            {
                order.Address = new AddressViewModel
                {
                    Street = addressViewModel.Street,
                    Number = addressViewModel.Number,
                    District = addressViewModel.District,
                    PostalCode = addressViewModel.PostalCode,
                    Complement = addressViewModel.Complement,
                    City = addressViewModel.City,
                    State = addressViewModel.State
                };
            }

            return order;
        }
    }
}
