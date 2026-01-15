
namespace PayFlow.Application.DTOs;

public sealed class DarajaCallbackDto
{
    public string ResultCode { get; set; } = default!;
    public string CheckoutRequestID { get; set; } = default!;
    public string MerchantRequestID { get; set; } = default!;
    public string AccountReference { get; set; } = default!;
}
public class PaymentStatusDto
{
    public Guid PaymentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}