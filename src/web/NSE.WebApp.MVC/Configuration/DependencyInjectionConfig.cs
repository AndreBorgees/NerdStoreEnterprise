using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebAPI.Core.User;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            #region HttpService

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthService, AuthService>()
                .AddPolicyHandler(PollyExtensions.WaitTry())
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                //.AddTransientHttpErrorPolicy(
                //p => p.WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: _ => TimeSpan.FromMilliseconds(600)));
                .AddPolicyHandler(PollyExtensions.WaitTry())
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IPurchasingBffService, PurchasingBffService>()
               .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
               .AddPolicyHandler(PollyExtensions.WaitTry())
               .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30)));

            #region Refit
            //services.AddHttpClient("Refit", options =>
            //{
            //    options.BaseAddress = new Uri(configuration.GetSection("CatalogUrl").Value);
            //})
            //   .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            //   .AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);
            #endregion

            #endregion
        }
    }

    #region PollyExtensions
    public class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> WaitTry()
        {
            var retry = HttpPolicyExtensions
               .HandleTransientHttpError()
               .WaitAndRetryAsync(new[]
               {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
               }, onRetry: (outcome, timespan, retryCount, context) =>
               {
                   Console.ForegroundColor = ConsoleColor.Blue;
                   Console.WriteLine(value: $"Tentando pela {retryCount} vez!");
                   Console.ForegroundColor = ConsoleColor.White;

               });

            return retry;
        }
    }
    #endregion

}
