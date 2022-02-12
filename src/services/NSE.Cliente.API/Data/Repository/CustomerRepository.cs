using Microsoft.EntityFrameworkCore;
using NSE.Cliente.API.Models;
using NSE.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomersContext _customersContext;

        public CustomerRepository(CustomersContext customersContext)
        {
            _customersContext = customersContext;
        }

        IUnitOfWork UnityOfWork => _customersContext;

        public void Add(Customer customer)
        {
            _customersContext.Add(customer);
        }

        public void Dispose()
        {
            _customersContext?.Dispose();
        }

        public async Task<Customer> GetaByCpf(string cpf)
        {
            return await _customersContext.Customers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);
        }

        public async Task<IEnumerable<Customer>> Getall()
        {
            return await _customersContext.Customers.AsNoTracking().ToListAsync();
        }
    }
}
