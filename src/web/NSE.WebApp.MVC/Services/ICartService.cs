using NSE.WebApp.MVC.Models;
using System.Threading.Tasks;
using System;

namespace NSE.WebApp.MVC.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCart();
        Task<ResponseResult> AddCartItem(ProductItemViewModel productItemViewModel);
        Task<ResponseResult> UpdateCartItem(Guid productId, ProductItemViewModel productItemViewModel);
        Task<ResponseResult> RemoveCartItem(Guid productId);
    }
}
