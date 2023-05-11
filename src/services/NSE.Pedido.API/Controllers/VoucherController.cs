using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Pedido.API.Application.DTO;
using NSE.Pedido.API.Application.Queries;
using NSE.WebAPI.Core.Controllers;
using System.Net;
using System.Threading.Tasks;

namespace NSE.Pedido.API.Controllers
{
    [Authorize]
    public class VoucherController : BaseController
    {
        public readonly IVoucherQueries _voucherQueries;

        public VoucherController(IVoucherQueries voucherQueries)
        {
            _voucherQueries = voucherQueries;
        }

        [HttpGet("voucher/{code}")]
        [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return NotFound();

            var voucher = await _voucherQueries.GetVoucherByCode(code);

            return voucher == null ? NotFound() : CustomResponse(voucher);
        }
    }
}
