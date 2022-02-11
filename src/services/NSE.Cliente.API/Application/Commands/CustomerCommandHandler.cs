using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Models;
using NSE.Core.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler,
        IRequestHandler<RegisterCustomerCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(RegisterCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.isValid()) return message.ValidationResult;

            var customer = new Customer(message.Id, message.Name, message.Email, message.Cpf);

            //validações de negócio

            //persistir no banco

            if(true)
            {
                AddError("Este CPF já está em uso.");
                return ValidationResult;
            }
          
            return message.ValidationResult;
        }
    }
}
