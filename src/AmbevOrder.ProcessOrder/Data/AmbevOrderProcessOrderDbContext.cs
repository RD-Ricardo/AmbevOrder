using AmbevOrder.ProcessOrder.Entities;
using AmbevOrder.ProcessOrder.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AmbevOrder.ProcessOrder.Data
{
    public class AmbevOrderProcessOrderDbContext : DbContext, ITransaction
    {
        public AmbevOrderProcessOrderDbContext(DbContextOptions<AmbevOrderProcessOrderDbContext> options) : base(options)
        {
                
        }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        private IDbContextTransaction? _currentTransaction;

        public void BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = Database.BeginTransaction();
        }

        public void CommitTransactionAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Nenhuma transação ativa para commit.");

            _currentTransaction.Commit();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }

        public void RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Nenhuma transação ativa para rollback.");

            _currentTransaction.Rollback();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }
}
