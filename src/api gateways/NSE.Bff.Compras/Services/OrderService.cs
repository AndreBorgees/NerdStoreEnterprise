using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using System.Net.Http;

namespace NSE.Bff.Compras.Services
{
    public interface IOrderService { }
    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.OrderUrl);
        }
    }
}
