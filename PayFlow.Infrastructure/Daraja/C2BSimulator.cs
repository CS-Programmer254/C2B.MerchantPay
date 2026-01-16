using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayFlow.Application.DTOs;
using PayFlow.Application.Interfaces;
using PayFlow.Domain.Shared;
using PayFlow.Infrastructure.Configurations;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PayFlow.Infrastructure.Daraja;

public sealed class C2BSimulator : IC2BSimulator
{
    private readonly HttpClient _http;
    private readonly IDarajaAuthService _auth;
    private readonly DarajaOptions _options;
    private readonly ILogger<C2BSimulator>? _logger;

    public C2BSimulator(
        HttpClient http,
        IDarajaAuthService auth,
        IOptions<DarajaOptions> options,
        ILogger<C2BSimulator>? logger = null)
    {
        _http = http;
        _auth = auth;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SimulateAsync(
        string phone,
        string amount,
        string billRef,
        C2BTransactionType type)
    {
        _logger?.LogInformation("Starting C2B Simulation - Phone: {Phone}, Amount: {Amount}", phone, amount);

        var token = await _auth.GetAccessTokenAsync();

        var commandId = type switch
        {
            C2BTransactionType.PayBill => "CustomerPayBillOnline",
            C2BTransactionType.BuyGoods => "CustomerBuyGoodsOnline",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
        // Parse the amount string to int
        if (!int.TryParse(amount, out var parsedAmount))
        {
            throw new BusinessRuleException($"Invalid amount: {amount}");
        }

        var payload = new
        {
            ShortCode = _options.ShortCode.ToString(),
            CommandID = commandId,
            Amount = parsedAmount,
            Msisdn = phone,
            BillRefNumber = type == C2BTransactionType.PayBill ? billRef : null
        };

        _logger?.LogInformation("Sending C2B request - ShortCode: {ShortCode}, CommandID: {CommandID}",
            _options.ShortCode, commandId);

        var request = new HttpRequestMessage(HttpMethod.Post, _options.SimulateUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(payload);

        var response = await _http.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        _logger?.LogInformation("Response Status: {StatusCode}", response.StatusCode);
        _logger?.LogInformation("Response Body: {ResponseBody}", responseContent);

        if (!response.IsSuccessStatusCode)
        {
            _logger?.LogError("C2B Simulation failed: {StatusCode} - {Response}", response.StatusCode, responseContent);
            throw new HttpRequestException(
                $"C2B Simulation failed with status {response.StatusCode}: {responseContent}");
        }

        _logger?.LogInformation("C2B Simulation completed successfully");
    }
}