using FluentValidation;
using NSE.Core.Messages;
using System;

namespace NSE.Cliente.API.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public RegisterCustomerCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new RegisterCustomerValidation().Validate(this);
            return ValidationResult.IsValid;
        }
        public class RegisterCustomerValidation : AbstractValidator<RegisterCustomerCommand>
        {
            public RegisterCustomerValidation()
            {
                RuleFor(x => x.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(x => x.Name)
                   .NotEmpty()
                   .WithMessage("O nome do cliente não foi informado");

                RuleFor(x => x.Cpf)
                    .Must(ValidateCpf)
                    .WithMessage("O CPF informado é inválido");


                RuleFor(x => x.Email)
                    .Must(ValidateEmail)
                    .WithMessage("O E-mail informado é inválido");
            }

            public static bool ValidateCpf(string cpf)
            {
                return Core.DomainObjects.Cpf.Validate(cpf);
            }

            public static bool ValidateEmail(string email)
            {
                return Core.DomainObjects.Email.Validate(email);
            }
        }
    }   
}
