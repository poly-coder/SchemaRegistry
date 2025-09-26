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

public class OperationConflictException : PicoDomainException
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

public class AlreadyDeletedException : OperationConflictException
{
    public AlreadyDeletedException(string entityType, string entityId)
        : base(entityType, entityId, "Delete", "Already deleted") { }
}

public class CannotRestoreException : OperationConflictException
{
    public CannotRestoreException(string entityType, string entityId)
        : base(entityType, entityId, "Delete", "Cannot restore") { }
}
