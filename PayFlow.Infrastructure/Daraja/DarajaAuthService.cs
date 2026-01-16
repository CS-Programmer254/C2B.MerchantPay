using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayFlow.Application.Interfaces;
using PayFlow.Infrastructure.Configurations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PayFlow.Infrastructure.Daraja;

public sealed class DarajaAuthService : IDarajaAuthService
{
    private readonly HttpClient _http;
    private readonly DarajaOptions _options;
    private readonly ILogger<DarajaAuthService>? _logger;

    public DarajaAuthService(
        HttpClient http,
        IOptions<DarajaOptions> options,
        ILogger<DarajaAuthService>? logger = null)
    {
        _http = http;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        _logger?.LogInformation("Requesting access token from Daraja");

        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{_options.ConsumerKey}:{_options.ConsumerSecret}"));

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_options.BaseUrl}/oauth/v1/generate?grant_type=client_credentials");

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        var response = await _http.SendAsync(request);

        _logger?.LogInformation("Token response status: {StatusCode}", response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        _logger?.LogInformation("Token response body: {ResponseBody}", content);

        response.EnsureSuccessStatusCode();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, options);

        if (string.IsNullOrEmpty(tokenResponse?.AccessToken))
        {
            _logger?.LogError("AccessToken not found in response: {Response}", content);
            throw new InvalidOperationException("AccessToken not found in response");
        }

        _logger?.LogInformation("Token obtained successfully");
        return tokenResponse.AccessToken;
    }

    private sealed record TokenResponse(
        [property: System.Text.Json.Serialization.JsonPropertyName("access_token")]
        string? AccessToken,
        [property: System.Text.Json.Serialization.JsonPropertyName("expires_in")]
        string? ExpiresIn);
}