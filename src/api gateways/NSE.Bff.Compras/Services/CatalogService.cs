using Microsoft.Extensions.Options;
using Newtonsoft.Json.Schema;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using NSE.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{
    public interface ICatalogService
    {
        Task<ProductItemDTO> GetById(Guid id);
        Task<IEnumerable<ProductItemDTO>> GetItems(IEnumerable<Guid> listaIds);
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

        public async Task<IEnumerable<ProductItemDTO>> GetItems(IEnumerable<Guid> listaIds)
        {
            var ids = string.Join(",", listaIds);

            var response = await _httpClient.GetAsync($"/catalog/products/list/{ids}/");

            ProcessErrorsResponse(response);

            return await DeserializeObjectResponse<IEnumerable<ProductItemDTO>>(response);
        }
    }
}
