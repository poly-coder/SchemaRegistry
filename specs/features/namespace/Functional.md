
# Namespace Functional Requirements

## Purpose

This section describes the purpose and scope of the namespace feature in the schema registry. It explains why namespaces are needed, how they help organize schemas, and what benefits they provide to clients using the registry.

This document outlines the functional requirements for the namespace feature in the schema registry.

## Definitions

- **Namespace**: A logical grouping of schemas related by client usage, providing isolation and organization within the registry.
- **Schema**: A data structure definition registered in the system, associated with a namespace.
- **Soft Delete**: The process of marking a namespace as deleted (using a timestamp) without permanently removing it, allowing for restoration.

## Requirements

- [ ] `REQ-NS-001`: The system MUST allow users to create a new namespace by providing a unique `Name` and optional `DisplayName`, `Description`, and `Documentation`.
- [ ] `REQ-NS-002`: The system MUST ensure that the `Name` is unique across all namespaces in the registry.
- [ ] `REQ-NS-003`: The system MUST allow users to soft delete a namespace by setting the `DeletedAt` timestamp.
- [ ] `REQ-NS-004`: The system MUST prevent modifications or new schema registrations in a soft-deleted namespace.
- [ ] `REQ-NS-005`: The system MUST allow users to restore a soft-deleted namespace by clearing the `DeletedAt` timestamp.
- [ ] `REQ-NS-006`: The system MUST allow users to permanently delete a namespace and all associated schemas, but only if it has been soft-deleted first.
- [ ] `REQ-NS-007`: The system MUST allow users to list all namespaces, with support for filtering and sorting by `Name`, `DisplayName`, `Description`, `CreatedAt`, and `DeletedAt`.
- [ ] `REQ-NS-008`: The system MUST allow users to retrieve detailed information about a specific namespace by its `Name`, including all metadata fields.
- [ ] `REQ-NS-009`: The system MUST allow users to explicitly request soft-deleted namespaces in queries.
- [ ] `REQ-NS-010`: The system MUST allow users to export all schemas within a specific namespace, including schema definitions and metadata, in a format suitable for import or code generation.

## Value Types

| Field           | Type        | Description                                         | Required |
| --------------- | ----------- | --------------------------------------------------- | -------- |
| `Name`          | `String`    | Unique identifier for the namespace                 | Yes      |
| `DisplayName`   | `String`    | Human-readable name for the namespace               | No       |
| `Description`   | `String`    | Brief description of the namespace's purpose        | No       |
| `Documentation` | `Markdown`  | Detailed documentation or notes about the namespace | No       |
| `CreatedAt`     | `Timestamp` | Indicates when the namespace was created            | Yes      |
| `DeletedAt`     | `Timestamp` | Indicates when the namespace was soft-deleted       | No       |

## Mutations

### Create Namespace

Allows users to create a new namespace by providing the following fields:

- `Name` (required)
- `DisplayName` (optional)
- `Description` (optional)
- `Documentation` (optional)

The system MUST ensure that the `Name` is unique across all namespaces in the registry.

### Soft Delete Namespace

Allows users to mark a namespace as deleted by setting the `DeletedAt` timestamp. The namespace should not be permanently removed from the system, but it should be excluded from standard listings and queries.

A soft-deleted namespace cannot be modified or used for new schema registrations until it is restored.

### Restore Namespace

Allows users to restore a soft-deleted namespace by clearing the `DeletedAt` timestamp. Once restored, the namespace becomes active again and can be used for schema registrations and modifications.

### Delete Namespace

Allows users to permanently delete a namespace and all associated schemas from the registry. This action cannot be undone. The namespace must be soft-deleted before it can be permanently deleted.

## Queries

### List Namespaces

Retrieves a list of all namespaces in the registry, including:

| Field         | Filterable | Sortable |
| ------------- | ---------- | -------- |
| `Name`        | Yes        | Yes      |
| `DisplayName` | Yes        | Yes      |
| `Description` | Yes        | No       |
| `CreatedAt`   | Yes        | Yes      |
| `DeletedAt`   | Yes        | Yes      |

The list can be filtered to include soft-deleted namespaces.

### Get Namespace Details

Retrieves detailed information about a specific namespace by its `Name`, including:

- `Name`
- `DisplayName`
- `Description`
- `Documentation`
- `CreatedAt`
- `DeletedAt`

To obtain a soft-deleted namespace, the query must explicitly request it.

### Export Schemas in Namespace

Allows users to export all schemas within a specific namespace. The export should include schema definitions and metadata, formatted for easy import into other systems or for code generation purposes.
