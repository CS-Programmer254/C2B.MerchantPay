using PayFlow.Application.Interfaces;

namespace PayFlow.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly PayFlowDbContext _context;

    public IPaymentRepository Payments { get; }
    public IWalletRepository Wallets { get; }
    public ICustomerRepository Customers { get; }
    public IMerchantRepository Merchants { get; }

    public UnitOfWork(
        PayFlowDbContext context,
        IPaymentRepository payments,
        IWalletRepository wallets,
        ICustomerRepository customers,
        IMerchantRepository merchants)
    {
        _context = context;
        Payments = payments;
        Wallets = wallets;
        Customers = customers;
        Merchants = merchants;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.RollbackTransactionAsync(cancellationToken);
    }
}