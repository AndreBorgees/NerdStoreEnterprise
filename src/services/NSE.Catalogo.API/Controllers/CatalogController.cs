using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Controllers
{
    [ApiController]
    public class CatalogController : Controller
    {
      private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("catalog/products")]
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        [HttpGet("catalog/prodcuts/{id}")]
        public async Task<Product> GetById(Guid id)
        {
            return await _productRepository.GetById(id);
        }
    }
}
