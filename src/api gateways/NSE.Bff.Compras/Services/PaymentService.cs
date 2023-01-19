using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using System.Net.Http;

namespace NSE.Bff.Compras.Services
{
    public interface IPaymentService { }
    public class PaymentService : Service, IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.PaymentUrl);
        }
    }
}

