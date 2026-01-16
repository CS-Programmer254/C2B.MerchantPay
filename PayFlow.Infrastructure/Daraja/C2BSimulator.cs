
using Microsoft.Extensions.Options;
using PayFlow.Application.DTOs;
using PayFlow.Application.Interfaces;
using PayFlow.Infrastructure.Configurations;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PayFlow.Infrastructure.Daraja;

public sealed class C2BSimulator : IC2BSimulator
{
    private readonly HttpClient _http;
    private readonly IDarajaAuthService _auth;
    private readonly DarajaOptions _options;

    public C2BSimulator(
        HttpClient http,
        IDarajaAuthService auth,
        IOptions<DarajaOptions> options)
    {
        _http = http;
        _auth = auth;
        _options = options.Value;
    }

    public async Task SimulateAsync(
        string phone,
        decimal amount,
        string billRef,
        C2BTransactionType type)
    {
        var token = await _auth.GetAccessTokenAsync();

        var commandId = type switch
        {
            C2BTransactionType.PayBill => "CustomerPayBillOnline",
            C2BTransactionType.BuyGoods => "CustomerBuyGoodsOnline",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        var payload = new
        {
            ShortCode = _options.ShortCode,
            CommandID = commandId,
            Amount = amount,
            Msisdn = phone,
            BillRefNumber = type == C2BTransactionType.PayBill ? billRef : null
        };

        var request = new HttpRequestMessage(HttpMethod.Post, _options.SimulateUrl);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(payload);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    //public int GetShortCode()
    //    => _options.ShortCode;
}
