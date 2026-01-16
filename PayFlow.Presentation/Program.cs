using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using PayFlow.Application.Commands;
using PayFlow.Infrastructure.Configurations;
using PayFlow.Infrastructure.Extensions;
using PayFlow.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(InitiateC2BPaymentCommand).Assembly);
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PayFlowDbContext>();
    var daraja = scope.ServiceProvider.GetRequiredService<IOptions<DarajaOptions>>();

    await DatabaseSeeder.SeedAsync(db, daraja);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var serverAddressesFeature = app.Services
            .GetRequiredService<Microsoft.AspNetCore.Hosting.Server.IServer>()
            .Features.Get<IServerAddressesFeature>();
        var url = serverAddressesFeature?.Addresses.FirstOrDefault() ?? "https://localhost:7129";
        var swaggerUrl = $"{url}/swagger/index.html";

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = swaggerUrl,
                UseShellExecute = true
            });
        }
        catch
        {
            // ignore if browser fails to open
        }
    });
}

app.Run();
