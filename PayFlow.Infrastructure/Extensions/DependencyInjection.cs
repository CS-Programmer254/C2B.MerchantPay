using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PayFlow.Application.Interfaces;
using PayFlow.Infrastructure.Daraja;
using PayFlow.Infrastructure.Persistence;
using PayFlow.Infrastructure.Persistence.Repositories;

namespace PayFlow.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PayFlowDbContext>((provider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? "Data Source=payflow.db"; 
            options.UseSqlite(connectionString);
        });


        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IC2BSimulator, C2BSimulator>();
        services.AddScoped<IDarajaAuthService, DarajaAuthService>();

        return services;
    }
}
