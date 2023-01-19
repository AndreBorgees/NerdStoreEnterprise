using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{
    public interface ICatalogService
    {
        Task<ProductItemDTO> GetById(Guid id);
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.CatalogUrl);
        }

        public async Task<ProductItemDTO> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}");

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<ProductItemDTO>(response);
        }
    }
}
