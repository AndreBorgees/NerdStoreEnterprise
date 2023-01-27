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
        public bool VoucherUsed { get; private set; }
        public decimal Discount { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime RegistrationDate { get; set; }
        public OrderStatus OrderStatus { get; set; }

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Address Address { get; private set; }
        public Voucher Voucher { get; private set; }

        public Order(Guid clientId, decimal totalValue, List<OrderItem> orderItems,
            bool voucherUsed = false, decimal discount = 0, Guid? voucherId = null)
        {
            ClientId = clientId;
            TotalValue = totalValue;
            _orderItems = orderItems;

            Discount = discount;
            VoucherUsed = voucherUsed;
            VoucherId = voucherId;
        }

        public void AuthorizeOrder()
        {
            OrderStatus = OrderStatus.Authorize;
        }

        public void AddVoucher(Voucher voucher)
        {
            VoucherUsed = true;
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
            if (!VoucherUsed) return;

            decimal discount = 0;
            var value = TotalValue;

            if(Voucher.DiscountType == DiscountTypeVouhcer.Percentage)
            {
                if(Voucher.Percentage.HasValue)
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
