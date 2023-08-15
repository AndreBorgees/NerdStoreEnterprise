using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using NSE.Core.Comunication;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{
    public interface IOrderService
    {
        Task<ResponseResult> Checkout(OrderDTO orderDTO);
        Task<OrderDTO> GetLastOrder();
        Task<IEnumerable<OrderDTO>> GetListOrderByClientId();
        Task<VoucherDTO> GetVoucherByCode(string code);
    }

    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.OrderUrl);
        }

        public async Task<ResponseResult> Checkout(OrderDTO orderDTO)
        {
            var orderContent = GetContent(orderDTO);

            var response = await _httpClient.PostAsync("/order/", orderContent);

            if (!ProcessErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<OrderDTO> GetLastOrder()
        {
            var response = await _httpClient.GetAsync("order/last");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<OrderDTO>(response);
        }

        public async Task<IEnumerable<OrderDTO>> GetListOrderByClientId()
        {
            var response = await _httpClient.GetAsync("order/list-customer");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<IEnumerable<OrderDTO>>(response);
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var response = await _httpClient.GetAsync($"/voucher/{code}/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<VoucherDTO>(response);
        }
    }
}
