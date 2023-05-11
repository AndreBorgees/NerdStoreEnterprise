using Dapper;
using NSE.Pedido.API.Application.DTO;
using NSE.Pedidos.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Pedido.API.Application.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDTO> GetLastOrder(Guid clientId);
        Task<IEnumerable<OrderDTO>> GetListByClientId(Guid clientId);
    }

    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepostiroy;

        public OrderQueries(IOrderRepository orderRepostiroy)
        {
            _orderRepostiroy = orderRepostiroy;
        }

        public async Task<OrderDTO> GetLastOrder(Guid clientId)
        {
            const string sql = @"SELECT
                                    O.ID AS 'ProductId', O.CODE, O.VOUCHERUSED, O.DISCOUNT, O.TOTALVALUE, O.STATUS, O.STREET, O.NUMBER, O.DISTRICT, 
                                    O.POSTALCODE, O.COMPLEMENT, O.CITY, O.UF, 
                                    OIT.ORDERID AS 'ProductItemId', OIT.NAME,  OIT.QUANTITY, OIT.IMAGE, OIT.VALUE
                                FROM Order O
                                INNER JOIN ORDERITENS OIT ON O.ID = OIT.ORDERID
                                WHERE O.CLIENTID = @clientid
                                AND O.REGISTRATIONDATE BETWEEN DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND O.STATUS = 1
                                ORDER BY O.DATE DESC";

            var order = await _orderRepostiroy.GetConnection()
                .QueryAsync<dynamic>(sql, new { clientId });

            return OrderMap(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetListByClientId(Guid clientId)
        {
            var orders = await _orderRepostiroy.GetListByClientId(clientId);

            return orders.Select(OrderDTO.ForOrderDTO);
        }

        private OrderDTO OrderMap(dynamic result)
        {
            var order = new OrderDTO
            {
                Code = result[0].CODE,
                Status = result[0].TOTALVALUE,
                TotalValue = result[0].TOTALVALUE,
                Discount = result[0].DISCOUNT,
                UsedVoucher = result[0].VOUCHERUSED,

                OrdemItems = new List<OrderItemDTO>(),
                Address = new AddressDTO
                {
                    Street = result[0].STREET,
                    District = result[0].DISTRICT,
                    PostalCode = result[0].POSTALCODE,
                    City = result[0].CITY,
                    Complement = result[0].COMPLEMENT,
                    UF = result[0].UF,
                    Number = result[0].NUMBER
                }
            };

            foreach (var item in result)
            {
                var orderItem = new OrderItemDTO
                {
                    Name = item.NAME,
                    Value = item.VALUE,
                    Quantity = item.QUANTITY,
                    Image = item.IMAGE
                };

                order.OrdemItems.Add(orderItem);
            }

            return order;
        }
    }
}

