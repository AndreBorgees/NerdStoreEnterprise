using NSE.Core.DomainObjects;
using System;

namespace NSE.Cliente.API.Models
{
    public class Customer : Entity, IAggregateRoot
    {
        protected Customer(){}
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public bool isRemoved { get; private set; }
        public Address Address { get; private set; }

        public Customer(Guid id, string name, string email, string cpf)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            isRemoved = false;
        }

        public void ChangeEmail(string email)
        {
            Email = new Email(email);
        }

        public void AddAddress(Address address)
        {
            Address = address;
        }
    }
}
