using EasyNetQ;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Cliente.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Services
{
    public class RegisterCustomerIntegrationHandler : BackgroundService
    {
        private IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegisterCustomerIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //Tarefas em background rodando
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus(connectionString: "host=localhost:5672");

            _bus.Rpc.RespondAsync<UserRegistredIntegrationEvent, ResponseMessage>(async request =>
                new ResponseMessage(await RegisterCustomer(request)));

            return Task.CompletedTask;
        }

        public async Task<ValidationResult> RegisterCustomer(UserRegistredIntegrationEvent message)
        {
            var customerCommand = new RegisterCustomerCommand(message.Id, message.Name, message.Email, message.Cpf);
            ValidationResult success;

            //Pratrica utilizada para objetos que estejam fora do padrão de lifecycle da sua aplicação ou que não possam ser injetados no cosntrutor.
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                success = await mediator.SendCommand(customerCommand);
            }

            return success;
        }
    }
}
