
using PayFlow.Application.DTOs;

namespace PayFlow.Application.Interfaces;

public interface IC2BSimulator
{
    Task SimulateAsync(
        string phone,
        string amount,
        string billRef,
        C2BTransactionType type);
}
