using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Core.DomainObjects;
using NSE.Core.Mediator;
using NSE.Core.Messages;
using NSE.Pedidos.Domain.Orders;
using NSE.Pedidos.Domain.Vouchers;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace NSE.Pedidos.Infra.Data
{
    public class OrderContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public OrderContext(DbContextOptions<OrderContext> options, IMediatorHandler mediatorHandler)
           : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
              e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("RegistrationDate") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("RegistrationDate").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("RegistrationDate").IsModified = false;
                }
            }

            var success = await base.SaveChangesAsync() > 0;
            if (success) await _mediatorHandler.PublishEvent(this);

            return success;
        }
    }

    public static class MediatorExtensions
    {
        public static async Task PublishEvent<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvent());

            var tasks = domainEvents
                .Select(async (domainEvents) =>
                {
                    await mediator.PublishEvent(domainEvents);
                });

            await Task.WhenAll(tasks);
        }
    }
}