using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Core.Mediator;
using NSE.Pedido.API.Application.Commands;
using NSE.Pedido.API.Application.Events;
using NSE.Pedido.API.Application.Queries;
using NSE.Pedidos.Domain.Orders;
using NSE.Pedidos.Domain.Vouchers;
using NSE.Pedidos.Infra.Data;
using NSE.Pedidos.Infra.Data.Repository;
using NSE.WebAPI.Core.User;


namespace NSE.Pedido.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Commands
            services.AddScoped<IRequestHandler<AddOrderCommand, ValidationResult>, OrderCommandHandler>();
            
            // Events
            services.AddScoped<INotificationHandler<OrderRealizedEvent>, OrderEventHandler>();

            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQueries, VoucherQueries>();
            services.AddScoped<IOrderQueries, OrderQueries>();

            // Data
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<OrderContext>();
        }
    }
}
