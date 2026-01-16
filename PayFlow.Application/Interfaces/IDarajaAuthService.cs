namespace PayFlow.Application.Interfaces;

public interface IDarajaAuthService
{
    Task<string> GetAccessTokenAsync();
}
