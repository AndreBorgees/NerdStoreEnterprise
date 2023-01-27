using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers.Specs;
using System;

namespace NSE.Pedidos.Domain.Vouchers
{
    public class Voucher: Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public int Quantity { get; private set; }
        public DiscountTypeVouhcer DiscountType { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public DateTime? UseDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        public bool IsValidForUse()
        {
            return new ActiveVoucherSpecification()
                .And(new DateVoucherSpecification())
                .And(new QuantityVoucherSpecification())
                .IsSatisfiedBy(this);
        }

        public void MarkAsUsed()
        {
            Active = false;
            Used = true;
            Quantity = 0;
            UseDate = DateTime.Now;
        }

        public void DebitAmount()
        {
            Quantity -= 1;
            if (Quantity >= 1) return;

            MarkAsUsed();
        }
    }
}
