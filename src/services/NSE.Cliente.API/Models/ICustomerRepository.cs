using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Models
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        void Add(Customer customer);
        Task<IEnumerable<Customer>> Getall();
        Task<Customer> GetaByCpf(string cpf);
        void AddAddress(Address endereco);
        Task<Address> GetAddressByUserId(Guid id);
    }
}
