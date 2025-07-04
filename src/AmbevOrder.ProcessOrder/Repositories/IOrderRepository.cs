namespace AmbevOrder.ProcessOrder.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Entities.Order order);
        Task<Entities.Order?> GetByExternIdAsync(Guid externId);
        Task<Entities.Order?> GetByIdAsync(Guid id);
        Task UpdateAsync(Entities.Order order);
        Task<IEnumerable<Entities.Order>> GetAllAsync();
    }
}
