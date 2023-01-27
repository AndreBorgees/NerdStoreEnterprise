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
        public bool VoucherUsed { get; set; }

        public List<OrderItemDTO> OrdemItems { get; set; }
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
                VoucherUsed = order.VoucherUsed,
                OrdemItems = new List<OrderItemDTO>(),
                Address = new AddressDTO()
            };

            foreach (var item in order.OrderItems)
            {
                orderDTO.OrdemItems.Add(new OrderItemDTO
                {
                    Name = item.ProductName,
                    Image = item.Image,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    Value = item.Value,
                    OrderId = item.OrderId
                });
            }

            orderDTO.Address = new AddressDTO
            {
                Street = order.Address.Street,
                Number = order.Address.Number,
                Complement = order.Address.Complement,
                District = order.Address.District,
                PostalCode = order.Address.PostalCode,
                City = order.Address.City,
                UF = order.Address.UF,
            };

            return orderDTO;
        }
    }
}
