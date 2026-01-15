namespace PayFlow.Application.Interfaces;

public interface IMobileMoneyGateway
{
    Task InitiateAsync(string phone, decimal amount, string reference);
}
