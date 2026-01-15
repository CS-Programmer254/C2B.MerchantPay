namespace PayFlow.Application.Interfaces;

public interface IUnitOfWork
{
    IPaymentRepository Payments { get; }
    IWalletRepository Wallets { get; }
    ICustomerRepository Customers { get; }
    IMerchantRepository Merchants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}