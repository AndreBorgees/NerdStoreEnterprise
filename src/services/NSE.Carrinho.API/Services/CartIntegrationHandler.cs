using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Carrinho.API.Data;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Carrinho.API.Services
{
    public class CartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CartIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderRealizedIntegrationEvent>("PedidoRealizado", async request => await DeleteCart(request));
        }

        private async Task DeleteCart(OrderRealizedIntegrationEvent orderRealizedIntegrationEvent)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CartContext>();

            var cart = await context.CartCustomer
                .FirstOrDefaultAsync(c => c.CustomerId == orderRealizedIntegrationEvent.ClientId);

            if(cart != null) 
            {
                context.CartCustomer.Remove(cart);
                await context.SaveChangesAsync();
            }
        }
    }
}
