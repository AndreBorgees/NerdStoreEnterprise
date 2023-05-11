using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Pedidos.Domain.Orders
{
    public class Order : Entity, IAggregateRoot
    {
        public int Code { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool UsedVoucher { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalValue { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Address Address { get; private set; }
        public Voucher Voucher { get; private set; }

        public Order(Guid clientId, decimal totalValue, List<OrderItem> orderItems,
            bool usedVoucher = false, decimal discount = 0, Guid? voucherId = null)
        {
            ClientId = clientId;
            TotalValue = totalValue;
            _orderItems = orderItems;
            Discount = discount;
            UsedVoucher = usedVoucher;
            VoucherId = voucherId;
        }

        public Order() { }

        public void AuthorizeOrder()
        {
            OrderStatus = OrderStatus.Authorize;
        }

        public void AddVoucher(Voucher voucher)
        {
            UsedVoucher = true;
            VoucherId = voucher.Id;
            Voucher = voucher;
        }

        public void AddAddress(Address address)
        {
            Address = address;
        }

        public void CalculateOrderValue()
        {
            TotalValue = OrderItems.Sum(p => p.CalculateValue());
            CalculateDiscountAmount();
        }

        private void CalculateDiscountAmount()
        {
            if (!UsedVoucher) return;

            decimal discount = 0;
            var value = TotalValue;

            if (Voucher.DiscountType == DiscountTypeVouhcer.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = (value * Voucher.Percentage.Value) / 100;
                    value -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    value -= discount;
                }
            }

            TotalValue = value < 0 ? 0 : value;
            Discount = discount;
        }
    }
}
