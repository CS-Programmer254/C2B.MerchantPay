using Microsoft.EntityFrameworkCore;
using PayFlow.Application.Interfaces;
using PayFlow.Domain.Aggregates;

namespace PayFlow.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PayFlowDbContext _context;

    public PaymentRepository(PayFlowDbContext context)
    {
        _context = context;
    }

    /// <summary>Get payment by its internal system ID</summary>
    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>Get payment by internal reference for C2B validation/confirmation)</summary>
    public async Task<Payment?> GetByInternalReferenceAsync(string internalReference, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.InternalReferenceNumber == internalReference, cancellationToken);
    }

    /// <summary>Add new payment</summary>
    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await _context.Payments.AddAsync(payment, cancellationToken);
    }

    /// <summary>update payment</summary>
    public void Update(Payment payment)
    {
        _context.Payments.Update(payment);
    }
}

public class WalletRepository : IWalletRepository
{
    private readonly PayFlowDbContext _context;

    public WalletRepository(PayFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _context.Wallets
            .FirstOrDefaultAsync(w => w.OwnerId == ownerId, cancellationToken);
    }

    public async Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Wallets
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        await _context.Wallets.AddAsync(wallet, cancellationToken);
    }
}

public class CustomerRepository : ICustomerRepository
{
    private readonly PayFlowDbContext _context;

    public CustomerRepository(PayFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
    }
}

public class MerchantRepository : IMerchantRepository
{
    private readonly PayFlowDbContext _context;

    public MerchantRepository(PayFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Merchant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Merchants
            .Include(m => m.KycDocuments)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }
    public async Task<Merchant?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return await _context.Merchants
            .Include(m => m.KycDocuments)
            .FirstOrDefaultAsync(m => m.ShortCode == shortCode, cancellationToken);
    }

    public async Task AddAsync(Merchant merchant, CancellationToken cancellationToken = default)
    {
        await _context.Merchants.AddAsync(merchant, cancellationToken);
    }
}