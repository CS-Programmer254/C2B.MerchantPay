
namespace PayFlow.Domain.Shared;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.") { }

    public NotFoundException(string message) : base(message) { }
}

public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message) { }
}

public class ValidationException : DomainException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string propertyName, string errorMessage)
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { errorMessage } }
        };
    }
}

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message) : base(message) { }
    public UnauthorizedException() : base("You are not authorized to perform this action.") { }
}

public class ForbiddenException : DomainException
{
    public ForbiddenException(string message) : base(message) { }
    public ForbiddenException() : base("You do not have permission to perform this action.") { }
}

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}

