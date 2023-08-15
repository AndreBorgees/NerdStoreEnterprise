using FluentValidation;
using NSE.Core.Messages;
using System;

namespace NSE.Cliente.API.Application.Commands
{
    public class AddAddressCommand : Command
    {
        public Guid CustomerId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public AddAddressCommand(Guid customerId, string street, string number, string complement, string district, string postalCode, string city, string state)
        {
            CustomerId = customerId;
            Street = street;
            Number = number;
            Complement = complement;
            District = district;
            PostalCode = postalCode;
            City = city;
            State = state;
        }

        public AddAddressCommand() { }

        public override bool IsValid()
        {
            ValidationResult = new AddressValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddressValidation : AbstractValidator<AddAddressCommand>
        {
            public AddressValidation()
            {
                RuleFor(c => c.Street)
                    .NotEmpty()
                    .WithMessage("Informe o Logradouro");

                RuleFor(c => c.Number)
                   .NotEmpty()
                   .WithMessage("Informe o Número");

                RuleFor(c => c.PostalCode)
                   .NotEmpty()
                   .WithMessage("Informe o CEP");

                RuleFor(c => c.District)
                   .NotEmpty()
                   .WithMessage("Informe o Bairro");

                RuleFor(c => c.City)
                   .NotEmpty()
                   .WithMessage("Informe o Cidade");

                RuleFor(c => c.State)
                  .NotEmpty()
                  .WithMessage("Informe o Estado");
            }
        }
    }
}
