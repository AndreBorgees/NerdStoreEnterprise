using NSE.Core.DomainObjects;

namespace NSE.Cliente.API.Models
{
    public class Customer : Entity, IAggregateRoot
    {
        public Customer(string name, string email, string cpf)
        {
            Name = name;
            Email = email;
            Cpf = cpf;
            isRemoved = false;
        }

        protected Customer(){}

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public bool isRemoved { get; private set; }
        public Address Address { get; private set; }
    }
}
