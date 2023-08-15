using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class OrderController : MainController
    {

        private readonly ICustomerService _customerService;
        private readonly IPurchasingBffService _purchasingBffService;

        public OrderController(ICustomerService customerService, IPurchasingBffService purchasingBffService)
        {
            _customerService = customerService;
            _purchasingBffService = purchasingBffService;
        }

        [HttpGet]
        [Route("delivery-address")]
        public async Task<IActionResult> DeliveryAddress()
        {
            var cart = await _purchasingBffService.GetCart();
            if (cart.Items.Count == 0) return RedirectToAction("Index", "Cart");

            var address = await _customerService.GetAddress();
            var order = _purchasingBffService.MapForOrder(cart, address);

            return View(order);
        }

        [HttpGet]
        [Route("payment")]
        public async Task<IActionResult> Payment()
        {
            var cart = await _purchasingBffService.GetCart();
            if (cart.Items.Count == 0) return RedirectToAction("Index", "Cart");

            var order = _purchasingBffService.MapForOrder(cart, null);

            return View(order);
        }

        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout(OerderTransactionViewModel oerderTransactionViewModel)
        {
            if (!ModelState.IsValid) return View("Payment", _purchasingBffService.MapForOrder(
                await _purchasingBffService.GetCart(), null));

            var response = await _purchasingBffService.Checkout(oerderTransactionViewModel);

            if (HasErrorsResponse(response))
            {
                var cart = await _purchasingBffService.GetCart();
                if (cart.Items.Count == 0) return RedirectToAction("Index", "Cart");

                var order = _purchasingBffService.MapForOrder(cart, null);
                return View("Payment", order);
            }

            return RedirectToAction("OrderConfirmation");
        }

        [HttpGet]
        [Route("order-realized")]
        public async Task<IActionResult> OrderConfirmation()
        {
            return View("OrderConfirmation", await _purchasingBffService.GetLastOrder());
        }

        [HttpGet]
        [Route("my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _purchasingBffService.GetOrderByCustomerId());    
        }

    }
}
