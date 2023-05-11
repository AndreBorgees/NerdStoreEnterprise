namespace NSE.Carrinho.API.Model
{
    public class Voucher
    {
        public string Code { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? DiscountValue { get; set; }
        public DiscountTypeVouhcer DiscountType { get; set; }
    }

    public enum DiscountTypeVouhcer
    {
        Percentage = 0,
        Value = 1
    }
}
