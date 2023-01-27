using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Pedidos.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Pedidos.Infra.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public DbConnection GetConnection() => _context.Database.GetDbConnection();

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<Order> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetListByClientId(Guid clientId)
        {
            return await _context.Orders
                .Include(p => p.OrderItems)
                .AsNoTracking()
                .Where(p => p.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<OrderItem> GetItemById(Guid id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem> GetItemByOrder(Guid orderId, Guid productId)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.OrderId == orderId);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
