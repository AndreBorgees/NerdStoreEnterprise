using Microsoft.Extensions.Options;
using NSE.Core.Comunication;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{

    public interface ICustomerService
    {
        Task<AddressViewModel> GetAddress();
        Task<ResponseResult> AddAddress(AddressViewModel address);
    }

    public class CustomerService : Service, ICustomerService
    {

        private readonly HttpClient _httpClient;
        public CustomerService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
        }

        public async Task<AddressViewModel> GetAddress()
        {
            var response = await _httpClient.GetAsync("/customer/address/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandleErrorsResponse(response);

            return await DeserealizeObjectResponse<AddressViewModel>(response);
        }


        public async Task<ResponseResult> AddAddress(AddressViewModel address)
        {
            var addressContent = SeralizeHttpContent(address);

            var response = await _httpClient.PostAsync("/customer/address/", addressContent);

            if(!HandleErrorsResponse(response)) return await DeserealizeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
