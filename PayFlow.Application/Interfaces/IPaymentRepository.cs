using PayFlow.Domain.Aggregates;

namespace PayFlow.Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
 
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment?> GetByInternalReferenceAsync(string internalReference, CancellationToken cancellationToken = default);
    void Update(Payment payment);
}

public interface IWalletRepository
{
    Task<Wallet?> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default);
}
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
}
public interface IMerchantRepository
{
    Task<Merchant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Merchant?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);
    Task AddAsync(Merchant merchant, CancellationToken cancellationToken = default);
}
