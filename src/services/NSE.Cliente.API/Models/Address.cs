using NSE.Core.DomainObjects;
using System;

namespace NSE.Cliente.API.Models
{
    public class Address : Entity
    {
        public Address(string publicPlace, string number, string complement, string district, string cep, string city, string state)
        {
            PublicPlace = publicPlace;
            Number = number;
            Complement = complement;
            District = district;
            Cep = cep;
            City = city;
            State = state;
        }

        public string PublicPlace { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public string Cep { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }
    }
}
