﻿using NSE.Core.Comunication;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected StringContent SeralizeHttpContent(object dado)
        {
            return new StringContent(
               JsonSerializer.Serialize(dado),
               Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserealizeObjectResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool HandleErrorsResponse(HttpResponseMessage httpResponse)
        {
            switch((int)httpResponse.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(httpResponse.StatusCode);
                case 400:
                    return false;
            }

            httpResponse.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult ReturnOk()
        {
            return new ResponseResult();
        }
    }
}
