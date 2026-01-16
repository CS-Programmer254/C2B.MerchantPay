using Microsoft.Extensions.Options;
using PayFlow.Application.Interfaces;
using PayFlow.Infrastructure.Configurations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PayFlow.Infrastructure.Daraja;

public sealed class DarajaAuthService : IDarajaAuthService
{
    private readonly HttpClient _http;
    private readonly DarajaOptions _options;

    public DarajaAuthService(HttpClient http, IOptions<DarajaOptions> options)
    {
        _http = http;
        _options = options.Value;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var credentials =
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    $"{_options.ConsumerKey}:{_options.ConsumerSecret}"));

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_options.BaseUrl}/oauth/v1/generate?grant_type=client_credentials");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Basic", credentials);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();

        return json!.AccessToken;
    }

    private sealed record AuthTokenResponse(string AccessToken);
}
