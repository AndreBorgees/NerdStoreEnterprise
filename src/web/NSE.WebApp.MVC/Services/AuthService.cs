using Microsoft.Extensions.Options;
using NSE.Core.Comunication;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);
        Task<UserResponseLogin> Registry(UserRegistry userRegistry);
    }
    public class AuthService : Service, IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);
            _httpClient = httpClient;
        }

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContent = SeralizeHttpContent(userLogin);

            var response = await _httpClient.PostAsync(requestUri: "/api/identity/authenticate",
                loginContent);

            if (!HandleErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserealizeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserealizeObjectResponse<UserResponseLogin>(response);
        }

        public async Task<UserResponseLogin> Registry(UserRegistry userRegistry)
        {
            var registryContent = SeralizeHttpContent(userRegistry);

            var response = await _httpClient.PostAsync(requestUri: "/api/identity/new",
                registryContent);

            if (!HandleErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserealizeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserealizeObjectResponse<UserResponseLogin>(response);
        }
    }
}
