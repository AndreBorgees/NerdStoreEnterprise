using NSE.WebApp.MVC.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(Guid id);
    }

    public interface ICatalogServiceRefit
    {
        [Get("/catalog/products")]
        Task<IEnumerable<ProductViewModel>> GetAll();

        [Get("/catalog/products/{id}")]
        Task<ProductViewModel> GetById(Guid id);
    }
}
