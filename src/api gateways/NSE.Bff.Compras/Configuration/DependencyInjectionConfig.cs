using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Extensions;
using NSE.WebAPI.Core.User;
using Polly;
using System;

namespace NSE.Bff.Compras.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.WaitTry())
                .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICartService, CartService>()
               .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
               .AddPolicyHandler(PollyExtensions.WaitTry())
               .AddTransientHttpErrorPolicy(
               p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IOrderService, OrderService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
             .AddPolicyHandler(PollyExtensions.WaitTry())
             .AddTransientHttpErrorPolicy(
             p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IPaymentService, PaymentService>()
             .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
             .AddPolicyHandler(PollyExtensions.WaitTry())
             .AddTransientHttpErrorPolicy(
             p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        }
    }
}
