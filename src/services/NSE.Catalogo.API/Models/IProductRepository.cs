﻿using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Models
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
        Task<List<Product>> GetProdcutsById(string ids);
        void Add(Product product);
        void Update(Product product);
    }
}
