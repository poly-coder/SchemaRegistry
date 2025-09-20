namespace Pico.Domain.Errors;

public abstract class PicoDomainException : Exception
{
    public PicoDomainException() { }

    public PicoDomainException(string message)
        : base(message) { }

    public PicoDomainException(string message, Exception inner)
        : base(message, inner) { }
}

public sealed class AlreadyExistsException : PicoDomainException
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

public sealed class EntityNotFoundException : PicoDomainException
{
    public EntityNotFoundException(string entityType, string entityId)
        : base($"Entity '{entityType}' with ID '{entityId}' does not exist.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public string EntityType { get; }
    public string EntityId { get; }
}

public sealed class OperationConflictException : PicoDomainException
{
    public OperationConflictException(
        string entityType,
        string entityId,
        string operation,
        string reason
    )
        : base(
            $"Conflict on entity '{entityType}' with ID '{entityId}' during '{operation}': {reason}"
        )
    {
        EntityType = entityType;
        EntityId = entityId;
        Reason = reason;
        Operation = operation;
    }

    public string EntityType { get; }
    public string EntityId { get; }
    public string Reason { get; }
    public string Operation { get; }
}
