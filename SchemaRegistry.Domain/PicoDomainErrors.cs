using FluentValidation;

namespace Pico.Domain.Errors;

public class PicoDomainException : Exception
{
    public PicoDomainException() { }

    public PicoDomainException(string message)
        : base(message) { }

    public PicoDomainException(string message, Exception inner)
        : base(message, inner) { }
}

public class AlreadyExistsException : PicoDomainException
{
    public AlreadyExistsException(string entityType, string entityId)
        : base($"Entity '{entityType}' with ID '{entityId}' already exists.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public string EntityType { get; }
    public string EntityId { get; }
}

public class DoesNotExistException : PicoDomainException
{
    public DoesNotExistException(string entityType, string entityId)
        : base($"Entity '{entityType}' with ID '{entityId}' does not exist.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public string EntityType { get; }
    public string EntityId { get; }
}
