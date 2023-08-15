using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{

    public interface ICustomerService
    {
        Task<AddressDTO> GetAddress();
    }

    public class CustomerService: Service, ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient, IOptions<AppServicesSettings> settings) 
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
        }

        public async Task<AddressDTO> GetAddress()
        {
            var response = await _httpClient.GetAsync("/customer/address/");

            if(response.StatusCode == HttpStatusCode.NotFound) return null;

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<AddressDTO>(response);
        }
    }
}
