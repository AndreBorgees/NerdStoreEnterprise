using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders.Testing;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _catalogContext;

        public ProductRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public IUnitOfWork UnitOfWork => _catalogContext;

        public async Task<Product> GetById(Guid id)
        {
            return await _catalogContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _catalogContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task<List<Product>> GetProdcutsById(string ids)
        {
            var idsGuid = ids.Split(',')
               .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Product>();

            var idsValue = idsGuid.Select(id => id.Value);

            var teste = await _catalogContext.Products.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Active).ToListAsync();

            return teste;
        }

        public void Add(Product product)
        {
            _catalogContext.Products.Add(product);
        }

        public void Update(Product product)
        {
            _catalogContext.Products.Update(product);
        }

        public void Dispose()
        {
            _catalogContext?.Dispose();
        }
    }
}
