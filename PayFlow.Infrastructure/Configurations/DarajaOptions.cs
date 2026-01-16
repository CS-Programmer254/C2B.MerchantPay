

using static System.Net.WebRequestMethods;

namespace PayFlow.Infrastructure.Configurations;

public sealed class DarajaOptions
{
    public string ConsumerKey { get; init; } = null!;
    public string ConsumerSecret { get; init; } = null!;
    public int ShortCode { get; init; }
    public string SimulateUrl { get; init; } = null!;
    public string BaseUrl { get; init; } = null!;
}
