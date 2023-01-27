using NSE.Pedido.API.Application.DTO;
using NSE.Pedidos.Domain.Vouchers;
using System.Threading.Tasks;

namespace NSE.Pedido.API.Application.Queries
{
    public interface IVoucherQueries
    {
        Task<VoucherDTO> GetVoucherByCode(string code);
    }

    public class VoucherQueries : IVoucherQueries
    {
        private IVoucherRepository _voucherRepository;

        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> GetVoucherByCode(string code)
        {
            var voucher = await _voucherRepository.GetVoucherByCode(code);

            if(voucher == null) return null;

            if (!voucher.IsValidForUse()) return null;

            return new VoucherDTO
            {
                Code = voucher.Code,
                DiscountType = (int)voucher.DiscountType,
                Percentage = voucher.Percentage,
                DiscountValue = voucher.DiscountValue
            };
        }
    }
}
