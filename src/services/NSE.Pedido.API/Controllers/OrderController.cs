using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Core.Mediator;
using NSE.Pedido.API.Application.Commands;
using NSE.Pedido.API.Application.Queries;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.User;
using System.Threading.Tasks;

namespace NSE.Pedido.API.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IAspNetUser _user;
        private readonly IOrderQueries _orderQueries;

        public OrderController(IMediatorHandler mediator,
            IAspNetUser user,
            IOrderQueries orderQueries)
        {
            _mediator = mediator;
            _user = user;
            _orderQueries = orderQueries;
        }

        [HttpPost("order")]
        public async Task<IActionResult> AddOrder(AddOrderCommand addOrderCommand)
        {
            addOrderCommand.ClientId = _user.GetUserId();

            return CustomResponse(await _mediator.SendCommand(addOrderCommand));
        }

        [HttpGet("order/last")]
        public async Task<IActionResult> LastOrder()
        {
            var order = await _orderQueries.GetLastOrder(_user.GetUserId());

            return order == null ? null : CustomResponse(order);

        }

        [HttpGet("order/list-customer")]
        public async Task<IActionResult> GetListByCustomer()
        {
            var order = await _orderQueries.GetListByClientId(_user.GetUserId());

            return order == null ? null : CustomResponse(order);
        }
    }
}
