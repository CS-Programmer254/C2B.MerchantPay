

namespace PayFlow.Infrastructure.Configurations;

public class DarajaOptions
{
    public string ShortCode { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string CallbackUrl { get; set; } = default!;
    public string StkPushUrl { get; set; } = default!;
}