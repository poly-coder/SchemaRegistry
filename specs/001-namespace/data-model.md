# Data Model: Namespace Management

**Date**: September 25, 2025
**Feature**: Namespace Management for Schema Registry

## Domain Entities

### Namespace Entity

**Primary Entity**: Represents a logical grouping container for schemas

**Fields**:

- `Name` (string, required, unique): 1-40 characters, pattern `^([a-z][a-z0-9]*)(\-([a-z][a-z0-9]*))*$`
- `DisplayName` (string, optional): 0-80 characters after trimming
- `Description` (string, optional): 0-1000 characters after trimming
- `Documentation` (string, optional): 0-10,000 characters markdown
- `Status` (enum): Active, SoftDeleted
- `CreatedAt` (DateTimeOffset, required): Creation timestamp
- `ModifiedAt` (DateTimeOffset, required): Last modification timestamp
- `DeletedAt` (DateTimeOffset, optional): Soft deletion timestamp

**Relationships**:

- One-to-many with Schemas (future entity, cascade soft delete)

**Validation Rules** (from spec FR-016-019):

- Name: Required, unique, pattern validation, 1-40 chars
- DisplayName: Optional, trimmed, max 80 chars
- Description: Optional, trimmed, max 1000 chars
- Documentation: Optional, max 10,000 chars
- Status transitions: Active ↔ SoftDeleted only

**State Transitions**:

```mermaid
[*] --> Active : create
Active --> SoftDeleted : delete
SoftDeleted --> Active : restore
```

### Domain Events (for Audit Trail)

**NamespaceCreated**:

- NamespaceId (string)
- Name (string)
- DisplayName (string?)
- Description (string?)
- Documentation (string?)
- CreatedAt (DateTimeOffset)
- CreatedBy (string, future - for now system)

**NamespaceUpdated**:

- NamespaceId (string)
- Changes (NamespaceChanges)
- UpdatedAt (DateTimeOffset)
- UpdatedBy (string, future)

**NamespaceDeleted**:

- NamespaceId (string)
- DeletedAt (DateTimeOffset)
- DeletedBy (string, future)

**NamespaceRestored**:

- NamespaceId (string)
- RestoredAt (DateTimeOffset)
- RestoredBy (string, future)

**NamespaceChanges** (value object):

- DisplayName (FieldChange<string?>?)
- Description (FieldChange<string?>?)
- Documentation (FieldChange<string?>?)

**FieldChange&lt;T&gt;** (value object):

- OldValue (T)
- NewValue (T)

## Data Transfer Objects

### Command Models (existing, validated)

**CreateNamespaceCommand**:

- Name (string, required)
- DisplayName (string?, optional)
- Description (string?, optional)
- Documentation (string?, optional)

**UpdateNamespaceDescriptionsCommand**:

- Name (string, required)
- DisplayName (string?, optional)
- Description (string?, optional)

**UpdateNamespaceDocumentationCommand**:

- Name (string, required)
- Documentation (string?, optional)

**DeleteNamespaceCommand**:

- Name (string, required)

**RestoreNamespaceCommand**:

- Name (string, required)

### Query Models (NEW - needed for read operations)

**NamespaceQuery**:

- Deleted (bool, optional, default false): Include soft-deleted namespaces
- ContinuationToken (string?, optional): For pagination
- PageSize (int, optional, default 50, max 100): Results per page

**NamespaceIdsQuery**:

- Ids (string[], required): List of namespace names to retrieve

### Response Models (NEW - replace simple boolean responses)

**NamespaceDetailsResponse**:

- Name (string)
- DisplayName (string?)
- Description (string?)
- Documentation (string?)
- Status (NamespaceStatus)
- CreatedAt (DateTimeOffset)
- ModifiedAt (DateTimeOffset)
- DeletedAt (DateTimeOffset?)
- Operations (NamespaceOperationsResponse)

**NamespaceOperationsResponse**:

- CanUpdateDescriptions (bool)
- CanUpdateDocumentation (bool)
- CanDelete (bool)
- CanRestore (bool)

**NamespaceListResponse**:

- Items (NamespaceDetailsResponse[])
- ContinuationToken (string?)
- TotalCount (int?) // Optional, may be expensive to compute

**NamespaceCommandResponse**:

- Success (bool)
- Namespace (NamespaceDetailsResponse?)
- Message (string?)

**NamespaceBatchResponse**:

- Found (NamespaceDetailsResponse[])
- NotFound (string[])

### Audit Trail Models (NEW - for FR-025-027)

**NamespaceAuditEntry**:

- EventId (Guid)
- NamespaceName (string)
- EventType (NamespaceEventType)
- Timestamp (DateTimeOffset)
- Data (object) // Event-specific data
- Metadata (Dictionary<string, object>?)

**NamespaceEventType** (enum):

- Created
- Updated
- Deleted
- Restored

**NamespaceAuditQuery**:

- NamespaceName (string, required)
- FromDate (DateTimeOffset?, optional)
- ToDate (DateTimeOffset?, optional)
- EventTypes (NamespaceEventType[]?, optional)
- ContinuationToken (string?, optional)
- PageSize (int, optional, default 50, max 100)

**NamespaceAuditResponse**:

- Entries (NamespaceAuditEntry[])
- ContinuationToken (string?)

## Database Schema (Marten Document Store)

### Document Storage

```json
{
  "id": "namespace-{name}",
  "name": "payment-schemas",
  "displayName": "Payment Related Schemas",
  "description": "All schemas related to payment processing",
  "documentation": "# Payment Schemas\n\nThis namespace contains...",
  "status": "Active",
  "createdAt": "2025-09-25T10:00:00Z",
  "modifiedAt": "2025-09-25T10:30:00Z",
  "deletedAt": null
}
```

### Event Store Schema

Events stored in Marten's `mt_events` table with stream identification by namespace name.

### Indexes (for Performance)

- Unique index on `name` field
- Index on `status` for filtering active/deleted
- Index on `createdAt` for sorting
- Index on `modifiedAt` for sorting
- Compound index on `(status, createdAt)` for efficient list queries

## Validation Summary

All validation implemented via FluentValidation in Domain layer:

1. **Name Validation**: Pattern, length, uniqueness
2. **DisplayName Validation**: Length, trimming
3. **Description Validation**: Length, trimming
4. **Documentation Validation**: Length
5. **State Transition Validation**: Valid status changes only
6. **Command Validation**: All required fields present

## Performance Considerations

- **Namespace Limit**: Support 10,000+ namespaces per NFR-001
- **Query Performance**: <100ms p95 per NFR-002
- **Pagination**: Continuation token to handle large result sets
- **Indexing**: Strategic indexes for common query patterns
- **Caching**: Orleans grain caching for frequently accessed namespaces
