# Quickstart: Namespace Management

**Date**: September 25, 2025
**Feature**: Namespace Management for Schema Registry

## Overview

This quickstart guide demonstrates the complete namespace management functionality through practical examples. Each scenario maps to the functional requirements and user stories defined in the specification.

## Prerequisites

- Schema Registry service running locally or accessible endpoint
- HTTP client (curl, Postman, or similar)
- Valid API endpoint: `http://localhost:5000` (adjust as needed)

## Base URL

All examples use: `http://localhost:5000/ns`

## Quick Start Scenarios

### Scenario 1: Create a New Namespace (FR-001, FR-002)

**User Story**: As a schema registry client, I want to create a new namespace with a unique name.

**Example**:

```bash
# Create a payment-related namespace
curl -X POST "http://localhost:5000/ns/payment-schemas" \
  -H "Content-Type: application/json" \
  -d '{
    "displayName": "Payment Related Schemas",
    "description": "All schemas related to payment processing",
    "documentation": "# Payment Schemas\n\nThis namespace contains schemas for:\n- Payment requests\n- Payment responses\n- Payment events"
  }'
```

**Expected Response** (201 Created):

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Related Schemas",
    "description": "All schemas related to payment processing",
    "documentation": "# Payment Schemas\n\nThis namespace contains schemas for:\n- Payment requests\n- Payment responses\n- Payment events",
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

**Validation Test** (400 Bad Request):

```bash
# Try to create with invalid name
curl -X POST "http://localhost:5000/ns/Invalid-Name" \
  -H "Content-Type: application/json" \
  -d '{"displayName": "Invalid"}'
```

### Scenario 2: Retrieve Namespace Information (FR-009)

**User Story**: As a schema registry client, I want to view namespace information including name, display name, description, and documentation.

**Example**:

```bash
# Get specific namespace details
curl -X GET "http://localhost:5000/ns/payment-schemas"
```

**Expected Response** (200 OK):

```json
{
  "name": "payment-schemas",
  "displayName": "Payment Related Schemas",
  "description": "All schemas related to payment processing",
  "documentation": "# Payment Schemas\n\nThis namespace contains schemas for:\n- Payment requests\n- Payment responses\n- Payment events",
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
}
```

### Scenario 3: List Active Namespaces (FR-011)

**User Story**: As a schema registry client, I want to see a list of active namespaces.

**Example**:

```bash
# List all active namespaces
curl -X GET "http://localhost:5000/ns"
```

**Expected Response** (200 OK):

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
      "modifiedAt": "2025-09-25T10:00:00Z",
      "deletedAt": null,
      "operations": {
        "canUpdateDescriptions": true,
        "canUpdateDocumentation": true,
        "canDelete": true,
        "canRestore": false
      }
    }
  ],
  "continuationToken": null,
  "totalCount": 1
}
```

### Scenario 4: Update Namespace Descriptions (FR-006)

**User Story**: As a schema registry client, I want to update the display name and description of existing namespaces.

**Example**:

```bash
# Update namespace descriptions
curl -X PUT "http://localhost:5000/ns/payment-schemas/descriptions" \
  -H "Content-Type: application/json" \
  -d '{
    "displayName": "Payment Processing Schemas",
    "description": "Updated description for payment processing related schemas"
  }'
```

**Expected Response** (200 OK):

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Processing Schemas",
    "description": "Updated description for payment processing related schemas",
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

### Scenario 5: Update Namespace Documentation (FR-006)

**User Story**: As a schema registry client, I want to update the documentation of existing namespaces.

**Example**:

```bash
# Update namespace documentation
curl -X PUT "http://localhost:5000/ns/payment-schemas/documentation" \
  -H "Content-Type: application/json" \
  -d '{
    "documentation": "# Payment Processing Schemas\n\nThis namespace contains schemas for:\n\n## Core Payment Operations\n- Payment creation\n- Payment validation\n- Payment completion\n\n## Event Schemas\n- Payment events\n- Notification events"
  }'
```

### Scenario 6: Soft Delete Namespace (FR-007, FR-013)

**User Story**: As a schema registry client, I want to soft-delete a namespace, which should also soft-delete all contained schemas.

**Example**:

```bash
# Soft delete namespace
curl -X DELETE "http://localhost:5000/ns/payment-schemas"
```

**Expected Response** (200 OK):

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Processing Schemas",
    "description": "Updated description for payment processing related schemas",
    "documentation": "# Payment Processing Schemas\n\nThis namespace contains...",
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

### Scenario 7: List Including Deleted Namespaces (FR-011, FR-013)

**User Story**: As a schema registry client, I want to see both active and soft-deleted namespaces using a `deleted=true` filter.

**Example**:

```bash
# List all namespaces including deleted ones
curl -X GET "http://localhost:5000/ns?deleted=true"
```

**Expected Response** (200 OK):

```json
{
  "items": [
    {
      "name": "payment-schemas",
      "displayName": "Payment Processing Schemas",
      "description": "Updated description for payment processing related schemas",
      "documentation": "# Payment Processing Schemas\n\nThis namespace contains...",
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
    }
  ],
  "continuationToken": null,
  "totalCount": 1
}
```

### Scenario 8: Restore Soft-Deleted Namespace (FR-008, FR-014)

**User Story**: As a schema registry client, I want to restore a previously soft-deleted namespace, which should also restore all schemas that were soft-deleted with it.

**Example**:

```bash
# Restore soft-deleted namespace
curl -X PUT "http://localhost:5000/ns/payment-schemas/restore"
```

**Expected Response** (200 OK):

```json
{
  "success": true,
  "namespace": {
    "name": "payment-schemas",
    "displayName": "Payment Processing Schemas",
    "description": "Updated description for payment processing related schemas",
    "documentation": "# Payment Processing Schemas\n\nThis namespace contains...",
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

### Scenario 9: Batch Retrieve Namespaces (Constitutional Requirement)

**User Story**: As a schema registry client, I want to retrieve multiple namespaces by their names in a single request.

**Example**:

```bash
# Create additional namespaces for testing
curl -X POST "http://localhost:5000/ns/user-schemas" \
  -H "Content-Type: application/json" \
  -d '{"displayName": "User Schemas", "description": "User-related schemas"}'

curl -X POST "http://localhost:5000/ns/product-schemas" \
  -H "Content-Type: application/json" \
  -d '{"displayName": "Product Schemas", "description": "Product-related schemas"}'

# Batch retrieve multiple namespaces
curl -X POST "http://localhost:5000/ns/batch" \
  -H "Content-Type: application/json" \
  -d '{
    "ids": ["payment-schemas", "user-schemas", "nonexistent-schema"],
    "includeDeleted": false
  }'
```

**Expected Response** (200 OK):

```json
{
  "found": [
    {
      "name": "payment-schemas",
      "displayName": "Payment Processing Schemas",
      "status": "Active",
      "createdAt": "2025-09-25T10:00:00Z",
      "modifiedAt": "2025-09-25T12:30:00Z",
      "operations": {
        "canUpdateDescriptions": true,
        "canUpdateDocumentation": true,
        "canDelete": true,
        "canRestore": false
      }
    },
    {
      "name": "user-schemas",
      "displayName": "User Schemas",
      "status": "Active",
      "createdAt": "2025-09-25T13:00:00Z",
      "modifiedAt": "2025-09-25T13:00:00Z",
      "operations": {
        "canUpdateDescriptions": true,
        "canUpdateDocumentation": true,
        "canDelete": true,
        "canRestore": false
      }
    }
  ],
  "notFound": ["nonexistent-schema"]
}
```

### Scenario 10: Pagination with Continuation Token (Constitutional Requirement)

**User Story**: As a schema registry client, I want to paginate through large lists of namespaces using continuation tokens.

**Example**:

```bash
# Get first page of namespaces
curl -X GET "http://localhost:5000/ns?pageSize=2"

# Use continuation token from response for next page
curl -X GET "http://localhost:5000/ns?pageSize=2&continuationToken=eyJjcmVhdGVkQXQiOiIyMDI1LTA5LTI1VDEzOjAwOjAwWiJ9"
```

### Scenario 11: Error Handling Examples

**Duplicate Name** (409 Conflict):

```bash
# Try to create duplicate namespace
curl -X POST "http://localhost:5000/ns/payment-schemas" \
  -H "Content-Type: application/json" \
  -d '{"displayName": "Duplicate"}'
```

**Invalid State Transition** (422 Unprocessable Entity):

```bash
# Try to restore active namespace
curl -X PUT "http://localhost:5000/ns/payment-schemas/restore"

# Try to delete already deleted namespace
curl -X DELETE "http://localhost:5000/ns/payment-schemas"
```

**Not Found** (404 Not Found):

```bash
# Try to get nonexistent namespace
curl -X GET "http://localhost:5000/ns/nonexistent-namespace"
```

## Performance Validation

### Load Testing

Create 100 namespaces to test performance requirements (NFR-001, NFR-002):

```bash
# Script to create multiple namespaces
for i in {1..100}; do
  curl -X POST "http://localhost:5000/ns/test-namespace-$i" \
    -H "Content-Type: application/json" \
    -d "{\"displayName\": \"Test Namespace $i\", \"description\": \"Test namespace number $i\"}" \
    --silent --output /dev/null --write-out "%{http_code} %{time_total}s\n"
done
```

**Performance Expectations**:

- All operations complete in <100ms (NFR-002)
- System supports 10,000+ namespaces (NFR-001)
- List operations with pagination perform consistently

## Audit Trail Validation (Future - FR-025-027)

Once audit trail is implemented:

```bash
# Get audit trail for a namespace
curl -X GET "http://localhost:5000/ns/payment-schemas/audit"
```

## Success Criteria

After running all scenarios, verify:

1. ✅ **Namespace Creation**: Can create namespaces with unique names
2. ✅ **Validation**: Invalid names and data are rejected appropriately
3. ✅ **Read Operations**: Can retrieve individual and multiple namespaces
4. ✅ **Updates**: Can update descriptions and documentation
5. ✅ **Soft Deletion**: Can soft delete and restore namespaces
6. ✅ **Filtering**: Can list active-only or include deleted namespaces
7. ✅ **Pagination**: Continuation token pagination works correctly
8. ✅ **Error Handling**: Appropriate HTTP status codes and messages
9. ✅ **Performance**: Operations complete within required timeframes
10. ✅ **State Management**: Operations correctly update namespace state

## Next Steps

1. Implement the missing read operations in the codebase
2. Add proper response models with full namespace details
3. Implement continuation token pagination
4. Add comprehensive error handling with ProblemDetails
5. Implement domain events for audit trail
6. Add performance optimizations for large namespace counts

This quickstart serves as both a usage guide and acceptance test suite for the namespace management feature.
