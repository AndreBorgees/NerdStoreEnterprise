﻿using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace NSE.Pedidos.Domain.Orders
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<Order> GetById(Guid id);
        Task<IEnumerable<Order>> GetListByClientId(Guid clientId);
        void Add(Order order);
        void Update(Order order);
        Task<OrderItem> GetItemById(Guid id);
        Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId);
        DbConnection GetConnection();
    }
}
