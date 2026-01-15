using Microsoft.Extensions.Options;
using PayFlow.Application.Interfaces;
using PayFlow.Infrastructure.Configurations;
using System.Net.Http.Json;

namespace PayFlow.Infrastructure.Daraja;

public sealed class DarajaGateway : IMobileMoneyGateway
{
    private readonly HttpClient _http;
    private readonly DarajaOptions _options;

    public DarajaGateway(
        HttpClient http,IOptions<DarajaOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task InitiateAsync(
        string phone,
        decimal amount,
        string reference)
    {
        var payload = new
        {
            BusinessShortCode = _options.ShortCode,
            Password = _options.Password,
            Timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
            TransactionType = "CustomerPayBillOnline",
            Amount = amount,
            PartyA = phone,
            PartyB = _options.ShortCode,
            PhoneNumber = phone,
            CallBackURL = _options.CallbackUrl,
            AccountReference = reference,
            TransactionDesc = "Utility Payment"
        };

        await _http.PostAsJsonAsync( _options.StkPushUrl,payload
        );
    }
}
