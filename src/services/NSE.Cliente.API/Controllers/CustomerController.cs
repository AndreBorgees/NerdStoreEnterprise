using Microsoft.AspNetCore.Mvc;
using NSE.Cliente.API.Application.Commands;
using NSE.Cliente.API.Models;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.User;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAspNetUser _aspNetUser;

        public CustomerController(ICustomerRepository customerRepository,
            IMediatorHandler mediatorHandler, IAspNetUser aspNetUser)
        {
            _mediatorHandler = mediatorHandler;
            _customerRepository = customerRepository;
            _aspNetUser = aspNetUser;
        }

        [HttpGet("customer/address")]
        public async Task<IActionResult> GetAddress()
        {
            var address = await _customerRepository.GetAddressByUserId(_aspNetUser.GetUserId());

            return address == null ? NotFound() : CustomResponse(address);
        }

        [HttpPost("customer/address")]
        public async Task<IActionResult> AddAddress(AddAddressCommand address)
        {
            address.CustomerId = _aspNetUser.GetUserId();

            return CustomResponse(await _mediatorHandler.SendCommand(address));
        }
    }
}
