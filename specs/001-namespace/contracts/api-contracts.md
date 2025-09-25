# API Contracts: Namespace Management

**Date**: September 25, 2025
**Feature**: Namespace Management for Schema Registry

## Base Path

All namespace endpoints use the base path: `/ns`

## Standard Entity Query Patterns (Constitutional Requirement)

### 1. List Operations - GET /ns

**Endpoint**: `GET /ns`
**Purpose**: Retrieve filtered, sorted, and paginated list of namespaces

**Query Parameters**:

- `deleted` (bool, optional): Include soft-deleted namespaces (default: false)
- `continuationToken` (string, optional): Continuation token for pagination
- `pageSize` (int, optional): Page size (default: 50, max: 100)
- `sortBy` (string, optional): Sort field (name, createdAt, modifiedAt)
- `sortOrder` (string, optional): Sort direction (asc, desc, default: asc)

**Response**: 200 OK

```json
{
  "items": [
    {
      "name": "payment-schemas",
      "displayName": "Payment Related Schemas",
      "description": "All schemas related to payment processing",
      "documentation": "# Payment Schemas\n\nThis namespace contains...",
      "status": "Active",
      "createdAt": "2025-09-25T10:00:00Z",
      "modifiedAt": "2025-09-25T10:30:00Z",
      "deletedAt": null,
      "operations": {
        "canUpdateDescriptions": true,
        "canUpdateDocumentation": true,
        "canDelete": true,
        "canRestore": false
      }
    }
  ],
  "continuationToken": "eyJjcmVhdGVkQXQiOiIyMDI1LTA5LTI1VDEwOjMwOjAwWiJ9",
  "totalCount": 150
}
```

**Error Responses**:

- 400 Bad Request: Invalid query parameters
- 500 Internal Server Error: Server error

### 2. Get by ID - GET /ns/{name}

**Endpoint**: `GET /ns/{name}`
**Purpose**: Retrieve single namespace by name

**Path Parameters**:

- `name` (string, required): Namespace name

**Query Parameters**:

- `deleted` (bool, optional): Allow retrieving soft-deleted namespace (default: false)

**Response**: 200 OK

```json
{
  "name": "payment-schemas",
  "displayName": "Payment Related Schemas",
  "description": "All schemas related to payment processing",
  "documentation": "# Payment Schemas\n\nThis namespace contains...",
  "status": "Active",
  "createdAt": "2025-09-25T10:00:00Z",
  "modifiedAt": "2025-09-25T10:30:00Z",
  "deletedAt": null,
  "operations": {
    "canUpdateDescriptions": true,
    "canUpdateDocumentation": true,
    "canDelete": true,
    "canRestore": false
  }
}
```

**Error Responses**:

- 400 Bad Request: Invalid namespace name format
- 404 Not Found: Namespace not found or soft-deleted (when deleted=false)
- 500 Internal Server Error: Server error

### 3. Get by IDs - POST /ns/batch

**Endpoint**: `POST /ns/batch`
**Purpose**: Retrieve multiple namespaces by names

**Request Body**:

```json
{
  "ids": ["payment-schemas", "user-schemas", "analytics-schemas"],
  "includeDeleted": false
}
```

**Response**: 200 OK

```json
{
  "found": [
    {
      "name": "payment-schemas",
      "displayName": "Payment Related Schemas",
      "description": "All schemas related to payment processing",
      "documentation": "# Payment Schemas\n\nThis namespace contains...",
      "status": "Active",
      "createdAt": "2025-09-25T10:00:00Z",
      "modifiedAt": "2025-09-25T10:30:00Z",
      "deletedAt": null,
      "operations": {
        "canUpdateDescriptions": true,
        "canUpdateDocumentation": true,
        "canDelete": true,
        "canRestore": false
      }
    }
  ],
  "notFound": ["analytics-schemas"]
}
```

**Error Responses**:

- 400 Bad Request: Invalid request body or empty IDs array
- 500 Internal Server Error: Server error

## Command Operations

### Create Namespace - POST /ns/{name}

**Endpoint**: `POST /ns/{name}`
**Purpose**: Create a new namespace

**Path Parameters**:

- `name` (string, required): Namespace name (validated against pattern)

**Request Body**:

```json
{
  "displayName": "Payment Related Schemas",
  "description": "All schemas related to payment processing",
  "documentation": "# Payment Schemas\n\nThis namespace contains..."
}
```

**Response**: 201 Created

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Related Schemas",
    "description": "All schemas related to payment processing",
    "documentation": "# Payment Schemas\n\nThis namespace contains...",
    "status": "Active",
    "createdAt": "2025-09-25T10:00:00Z",
    "modifiedAt": "2025-09-25T10:00:00Z",
    "deletedAt": null,
    "operations": {
      "canUpdateDescriptions": true,
      "canUpdateDocumentation": true,
      "canDelete": true,
      "canRestore": false
    }
  },
  "message": "Namespace created successfully"
}
```

**Error Responses**:

- 400 Bad Request: Validation errors (invalid name pattern, length violations)
- 409 Conflict: Namespace name already exists
- 500 Internal Server Error: Server error

### Update Namespace Descriptions - PUT /ns/{name}/descriptions

**Endpoint**: `PUT /ns/{name}/descriptions`
**Purpose**: Update display name and description of namespace

**Path Parameters**:

- `name` (string, required): Namespace name

**Request Body**:

```json
{
  "displayName": "Updated Payment Schemas",
  "description": "Updated description for payment processing schemas"
}
```

**Response**: 200 OK

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Updated Payment Schemas",
    "description": "Updated description for payment processing schemas",
    "documentation": "# Payment Schemas\n\nThis namespace contains...",
    "status": "Active",
    "createdAt": "2025-09-25T10:00:00Z",
    "modifiedAt": "2025-09-25T11:00:00Z",
    "deletedAt": null,
    "operations": {
      "canUpdateDescriptions": true,
      "canUpdateDocumentation": true,
      "canDelete": true,
      "canRestore": false
    }
  },
  "message": "Namespace descriptions updated successfully"
}
```

**Error Responses**:

- 400 Bad Request: Validation errors (length violations)
- 404 Not Found: Namespace not found
- 500 Internal Server Error: Server error

### Update Namespace Documentation - PUT /ns/{name}/documentation

**Endpoint**: `PUT /ns/{name}/documentation`
**Purpose**: Update documentation of namespace

**Path Parameters**:

- `name` (string, required): Namespace name

**Request Body**:

```json
{
  "documentation": "# Updated Payment Schemas\n\nThis namespace now contains..."
}
```

**Response**: 200 OK

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Schemas",
    "description": "All schemas related to payment processing",
    "documentation": "# Updated Payment Schemas\n\nThis namespace now contains...",
    "status": "Active",
    "createdAt": "2025-09-25T10:00:00Z",
    "modifiedAt": "2025-09-25T11:30:00Z",
    "deletedAt": null,
    "operations": {
      "canUpdateDescriptions": true,
      "canUpdateDocumentation": true,
      "canDelete": true,
      "canRestore": false
    }
  },
  "message": "Namespace documentation updated successfully"
}
```

**Error Responses**:

- 400 Bad Request: Validation errors (documentation too long)
- 404 Not Found: Namespace not found
- 500 Internal Server Error: Server error

### Delete Namespace - DELETE /ns/{name}

**Endpoint**: `DELETE /ns/{name}`
**Purpose**: Soft delete a namespace

**Path Parameters**:

- `name` (string, required): Namespace name

**Response**: 200 OK

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Schemas",
    "description": "All schemas related to payment processing",
    "documentation": "# Payment Schemas\n\nThis namespace contains...",
    "status": "SoftDeleted",
    "createdAt": "2025-09-25T10:00:00Z",
    "modifiedAt": "2025-09-25T12:00:00Z",
    "deletedAt": "2025-09-25T12:00:00Z",
    "operations": {
      "canUpdateDescriptions": false,
      "canUpdateDocumentation": false,
      "canDelete": false,
      "canRestore": true
    }
  },
  "message": "Namespace soft deleted successfully"
}
```

**Error Responses**:

- 400 Bad Request: Invalid namespace name
- 404 Not Found: Namespace not found
- 422 Unprocessable Entity: Namespace already deleted
- 500 Internal Server Error: Server error

### Restore Namespace - PUT /ns/{name}/restore

**Endpoint**: `PUT /ns/{name}/restore`
**Purpose**: Restore a soft-deleted namespace

**Path Parameters**:

- `name` (string, required): Namespace name

**Response**: 200 OK

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Schemas",
    "description": "All schemas related to payment processing",
    "documentation": "# Payment Schemas\n\nThis namespace contains...",
    "status": "Active",
    "createdAt": "2025-09-25T10:00:00Z",
    "modifiedAt": "2025-09-25T12:30:00Z",
    "deletedAt": null,
    "operations": {
      "canUpdateDescriptions": true,
      "canUpdateDocumentation": true,
      "canDelete": true,
      "canRestore": false
    }
  },
  "message": "Namespace restored successfully"
}
```

**Error Responses**:

- 400 Bad Request: Invalid namespace name
- 404 Not Found: Namespace not found
- 422 Unprocessable Entity: Namespace not deleted
- 500 Internal Server Error: Server error

## Audit Trail Operations (Future - FR-025-027)

### Get Namespace Audit Trail - GET /ns/{name}/audit

**Endpoint**: `GET /ns/{name}/audit`
**Purpose**: Retrieve audit trail for a namespace

**Path Parameters**:

- `name` (string, required): Namespace name

**Query Parameters**:

- `fromDate` (ISO 8601, optional): Filter events from date
- `toDate` (ISO 8601, optional): Filter events to date
- `eventTypes` (string[], optional): Filter by event types
- `continuationToken` (string, optional): Pagination token
- `pageSize` (int, optional): Page size (default: 50, max: 100)

**Response**: 200 OK

```json
{
  "entries": [
    {
      "eventId": "550e8400-e29b-41d4-a716-446655440000",
      "namespaceName": "payment-schemas",
      "eventType": "Created",
      "timestamp": "2025-09-25T10:00:00Z",
      "data": {
        "name": "payment-schemas",
        "displayName": "Payment Schemas",
        "description": "All schemas related to payment processing"
      },
      "metadata": {
        "source": "SchemaRegistry.WebApi",
        "version": "1.0.0"
      }
    }
  ],
  "continuationToken": "eyJ0aW1lc3RhbXAiOiIyMDI1LTA5LTI1VDEwOjAwOjAwWiJ9"
}
```

**Error Responses**:

- 400 Bad Request: Invalid query parameters
- 404 Not Found: Namespace not found
- 500 Internal Server Error: Server error

## Common Error Response Format

All endpoints follow RFC 7807 ProblemDetails format:

```json
{
  "type": "https://schemas.registry.com/problems/validation-error",
  "title": "Validation Error",
  "status": 400,
  "detail": "The namespace name must match the pattern '^([a-z][a-z0-9]*)(\-([a-z][a-z0-9]*))*$'.",
  "instance": "/ns/Invalid-Name",
  "errors": {
    "Name": ["The namespace name must match the specified pattern."]
  }
}
```

## HTTP Status Code Usage

- **200 OK**: Successful GET, PUT operations
- **201 Created**: Successful POST operations
- **400 Bad Request**: Validation errors, malformed requests
- **404 Not Found**: Resource not found
- **409 Conflict**: Resource already exists
- **422 Unprocessable Entity**: Invalid state transitions
- **500 Internal Server Error**: Server errors
