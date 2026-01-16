namespace PayFlow.Application.DTOs;
// Customer Simulation Request
public sealed class C2BSimulateRequest
{
    public string Phone { get; init; } = null!;
    public string Amount { get; init; } = null!;
    public string BillRef { get; init; } = null!;
    public string C2BTransactionType { get; init; } = null!; 
    public string ShortCode { get; init; } = null!; 
}

public enum C2BTransactionType
{
    PayBill,    // CustomerPayBillOnline
    BuyGoods    // CustomerBuyGoodsOnline
}

// Validation Callback Sent by M-PESA if External Validation enabled
public record C2BValidationRequest(
    string TransactionType,     //"Pay Bill" or "Buy Goods"
    string TransID,             // M-PESA transaction ID
    string TransTime,           // YYYYMMDDHHmmss
    decimal TransAmount,        // Amount being transacted
    string BusinessShortCode,   // Merchant Paybill/Till
    string BillRefNumber,       // Account reference PayBill only
    string MSISDN,              // Customer phone
    string FirstName,
    string MiddleName,
    string LastName
);

public record C2BValidationResponse(
    string ResultCode,          // 0 = Accepted, others = Rejected
    string ResultDesc           // "Accepted" or "Rejected"
);


// Confirmation Callback Sent by M-PESA after payment completed
public record C2BConfirmationRequest(
    string TransactionType,     // "Pay Bill" or "Buy Goods"
    string TransID,             // M-PESA transaction ID
    string TransTime,           // YYYYMMDDHHmmss
    decimal TransAmount,        // Amount transacted
    string BusinessShortCode,   // Merchant Paybill/Till
    string BillRefNumber,       // Account reference PayBill only
    decimal? OrgAccountBalance, // Merchant balance after transaction
    string MSISDN,              // Customer phone
    string FirstName,
    string MiddleName,
    string LastName
);

public record C2BConfirmationResponse(
    string ResultCode,          // Always "0" to acknowledge receipt
    string ResultDesc           // "Accepted"
);

public class PaymentStatusDto
{
    public Guid PaymentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}