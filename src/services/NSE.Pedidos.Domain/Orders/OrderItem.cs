using NSE.Core.DomainObjects;
using System;

namespace NSE.Pedidos.Domain.Orders
{
    public class OrderItem: Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }
        public string Image { get; set; }
        public Order Order { get; set; }

        public OrderItem(Guid productId, string productName, int quantity, decimal value, string image)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Value = value;
            Image = image;
        }

        protected OrderItem() { }

        internal decimal CalculateValue()
        {
            return Quantity * Value;
        }
    }
}
