using NSE.Pedidos.Domain.Orders;
using System;
using System.Collections.Generic;

namespace NSE.Pedido.API.Application.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int Code { get; set; }

        public int Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal TotalValue { get; set; }

        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }
        public bool UsedVoucher { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; }
        public AddressDTO Address { get; set; }

        public static OrderDTO ForOrderDTO(Order order)
        {
            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                Code = order.Code,
                Status = (int)order.OrderStatus,
                RegistrationDate = order.RegistrationDate,
                TotalValue = order.TotalValue,
                Discount = order.Discount,
                UsedVoucher = order.VoucherUsed,
                OrderItems = new List<OrderItemDTO>(),
                Address = new AddressDTO()
            };

            foreach (var item in order.OrderItems)
            {
                orderDTO.OrderItems.Add(new OrderItemDTO
                {
                    Name = item.ProductName,
                    Image = item.Image,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    Price = item.Value,
                    OrderId = item.OrderId
                });
            }

            orderDTO.Address = new AddressDTO
            {
                Street = order.Address.Street,
                Number = order.Address.Number,
                Complement = order.Address.Complement,
                District = order.Address.District,
                Cep = order.Address.PostalCode,
                City = order.Address.City,
                State = order.Address.UF,
            };

            return orderDTO;
        }
    }
}
