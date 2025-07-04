using AmbevOrder.ProcessOrder.Data;
using AmbevOrder.ProcessOrder.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmbevOrder.ProcessOrder.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AmbevOrderProcessOrderDbContext _dbContext;
        public OrderRepository(AmbevOrderProcessOrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Order order)
        {
            _dbContext.Orders.Add(order);
            return _dbContext.SaveChangesAsync();
        }

        public Task<List<Order>> GetAllAsync()
        {
            return _dbContext.Orders.Include(x => x.Items).ToListAsync();
        }

        public Task<Order?> GetByExternIdAsync(Guid externId)
        {
            return _dbContext.Orders.SingleOrDefaultAsync(o => o.ExternId == externId && o.ProcessedAt != null);
        }

        public Task<Order?> GetByIdAsync(Guid id)
        {
            return _dbContext.Orders.SingleOrDefaultAsync(o => o.Id == id);
        }

        public Task UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            return _dbContext.SaveChangesAsync();
        }
    }
}
