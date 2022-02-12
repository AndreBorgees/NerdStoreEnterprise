using NSE.Core.Messages;
using System;

namespace NSE.Cliente.API.Application.Events
{
    public class RegisteredCustomerEvent: Event
    {
        public RegisteredCustomerEvent(Guid id, string name, string email, string cpf)
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
    }
}
