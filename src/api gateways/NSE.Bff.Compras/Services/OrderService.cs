using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{
    public interface IOrderService 
    {
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

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var response = await _httpClient.GetAsync($"/voucher/{code}/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<VoucherDTO>(response);
        }
    }
}
