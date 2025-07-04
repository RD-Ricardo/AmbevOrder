namespace AmbevOrder.ProcessOrder.Repositories
{
    public interface ITransaction
    {
        void BeginTransactionAsync();
        void CommitTransactionAsync();
        void RollbackTransactionAsync();
    }
}
