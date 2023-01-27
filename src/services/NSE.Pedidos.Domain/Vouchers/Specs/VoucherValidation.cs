using NSE.Core.Specification.Validation;

namespace NSE.Pedidos.Domain.Vouchers.Specs
{
    public class VoucherValidation : SpecValidator<Voucher>
    {
        public VoucherValidation()
        {
            var dataSpec = new DateVoucherSpecification();
            var qtdSpec = new QuantityVoucherSpecification();
            var actSpec = new ActiveVoucherSpecification();

            Add("dataSpec", new Rule<Voucher>(dataSpec, "Este voucher está expirado"));
            Add("qtdSpec", new Rule<Voucher>(qtdSpec, "Este voucher já foi utilizado"));
            Add("actSpec", new Rule<Voucher>(actSpec, "Este voucher não está mais ativo"));
        }
    }
}
