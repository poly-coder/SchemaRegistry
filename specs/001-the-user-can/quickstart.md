# Quickstart: Namespace Management

**Feature**: Namespace Management for Schema Registry
**Date**: September 21, 2025
**Status**: Ready for Implementation

## Overview

This quickstart guide demonstrates the complete namespace management workflow through practical examples. It serves as both documentation and acceptance testing for the feature.

## Prerequisites

- Schema Registry service running locally
- Valid JWT token with `namespace-read` and `namespace-write` permissions
- HTTP client (curl, Postman, or similar)

## Environment Setup

```bash
# Set base URL and authentication
export BASE_URL="http://localhost:5000/api"
export JWT_TOKEN="your-jwt-token-here"

# Helper function for authenticated requests
auth_header() {
    echo "Authorization: Bearer $JWT_TOKEN"
}
```

## Scenario 1: Create and Manage Namespace

### Step 1: Create a New Namespace

Create a namespace for payment-related schemas:

```bash
curl -X POST "$BASE_URL/namespaces" \
  -H "$(auth_header)" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "payment-schemas",
    "displayName": "Payment Processing Schemas",
    "description": "Schemas for payment processing workflows",
    "documentation": "# Payment Schemas\n\nThis namespace contains all schemas related to payment processing including:\n\n- Credit card transactions\n- Bank transfers\n- Payment validation rules"
  }'
```

**Expected Response** (201 Created):
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "payment-schemas",
  "displayName": "Payment Processing Schemas",
  "description": "Schemas for payment processing workflows",
  "documentation": "# Payment Schemas\n\nThis namespace contains all schemas related to payment processing including:\n\n- Credit card transactions\n- Bank transfers\n- Payment validation rules",
  "status": "Active",
  "createdAt": "2025-09-21T10:30:00Z",
  "modifiedAt": "2025-09-21T10:30:00Z",
  "deletedAt": null
}
```

### Step 2: Retrieve Namespace Details

```bash
curl -X GET "$BASE_URL/namespaces/123e4567-e89b-12d3-a456-426614174000" \
  -H "$(auth_header)"
```

**Expected Response** (200 OK):
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "payment-schemas",
  "displayName": "Payment Processing Schemas",
  "description": "Schemas for payment processing workflows",
  "documentation": "# Payment Schemas\n\nThis namespace contains all schemas related to payment processing including:\n\n- Credit card transactions\n- Bank transfers\n- Payment validation rules",
  "status": "Active",
  "createdAt": "2025-09-21T10:30:00Z",
  "modifiedAt": "2025-09-21T10:30:00Z",
  "deletedAt": null
}
```

### Step 3: Update Namespace Metadata

Update the description and documentation:

```bash
curl -X PUT "$BASE_URL/namespaces/123e4567-e89b-12d3-a456-426614174000" \
  -H "$(auth_header)" \
  -H "Content-Type: application/json" \
  -d '{
    "displayName": "Enhanced Payment Processing Schemas",
    "description": "Comprehensive schemas for all payment processing workflows including fraud detection",
    "documentation": "# Enhanced Payment Schemas\n\nThis namespace contains all schemas related to payment processing including:\n\n- Credit card transactions\n- Bank transfers\n- Payment validation rules\n- Fraud detection patterns\n\n## Usage Guidelines\n\n1. All payment schemas must follow naming convention\n2. Include validation rules for data integrity\n3. Document all field meanings"
  }'
```

**Expected Response** (200 OK):
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "payment-schemas",
  "displayName": "Enhanced Payment Processing Schemas",
  "description": "Comprehensive schemas for all payment processing workflows including fraud detection",
  "documentation": "# Enhanced Payment Schemas\n\nThis namespace contains all schemas related to payment processing including:\n\n- Credit card transactions\n- Bank transfers\n- Payment validation rules\n- Fraud detection patterns\n\n## Usage Guidelines\n\n1. All payment schemas must follow naming convention\n2. Include validation rules for data integrity\n3. Document all field meanings",
  "status": "Active",
  "createdAt": "2025-09-21T10:30:00Z",
  "modifiedAt": "2025-09-21T10:45:30Z",
  "deletedAt": null
}
```

## Scenario 2: List and Filter Namespaces

### Step 1: List All Active Namespaces

```bash
curl -X GET "$BASE_URL/namespaces" \
  -H "$(auth_header)"
```

**Expected Response** (200 OK):
```json
{
  "namespaces": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "payment-schemas",
      "displayName": "Enhanced Payment Processing Schemas",
      "description": "Comprehensive schemas for all payment processing workflows including fraud detection",
      "status": "Active",
      "createdAt": "2025-09-21T10:30:00Z",
      "modifiedAt": "2025-09-21T10:45:30Z",
      "deletedAt": null
    }
  ],
  "totalCount": 1,
  "pageSize": 20,
  "pageNumber": 1
}
```

### Step 2: Search Namespaces

Search for namespaces containing "payment":

```bash
curl -X GET "$BASE_URL/namespaces?search=payment" \
  -H "$(auth_header)"
```

**Expected Response** (200 OK):
```json
{
  "namespaces": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "payment-schemas",
      "displayName": "Enhanced Payment Processing Schemas",
      "description": "Comprehensive schemas for all payment processing workflows including fraud detection",
      "status": "Active",
      "createdAt": "2025-09-21T10:30:00Z",
      "modifiedAt": "2025-09-21T10:45:30Z",
      "deletedAt": null
    }
  ],
  "totalCount": 1,
  "pageSize": 20,
  "pageNumber": 1
}
```

## Scenario 3: Soft Delete and Restore Workflow

### Step 1: Soft Delete Namespace

Mark the namespace as deleted while preserving data:

```bash
curl -X DELETE "$BASE_URL/namespaces/123e4567-e89b-12d3-a456-426614174000" \
  -H "$(auth_header)"
```

**Expected Response** (204 No Content)

### Step 2: Verify Namespace is Hidden from Default Listing

```bash
curl -X GET "$BASE_URL/namespaces" \
  -H "$(auth_header)"
```

**Expected Response** (200 OK):
```json
{
  "namespaces": [],
  "totalCount": 0,
  "pageSize": 20,
  "pageNumber": 1
}
```

### Step 3: List Including Deleted Namespaces

```bash
curl -X GET "$BASE_URL/namespaces?includeDeleted=true" \
  -H "$(auth_header)"
```

**Expected Response** (200 OK):
```json
{
  "namespaces": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "payment-schemas",
      "displayName": "Enhanced Payment Processing Schemas",
      "description": "Comprehensive schemas for all payment processing workflows including fraud detection",
      "status": "SoftDeleted",
      "createdAt": "2025-09-21T10:30:00Z",
      "modifiedAt": "2025-09-21T11:00:00Z",
      "deletedAt": "2025-09-21T11:00:00Z"
    }
  ],
  "totalCount": 1,
  "pageSize": 20,
  "pageNumber": 1
}
```

### Step 4: Restore Namespace

Bring the namespace back to active status:

```bash
curl -X POST "$BASE_URL/namespaces/123e4567-e89b-12d3-a456-426614174000/restore" \
  -H "$(auth_header)"
```

**Expected Response** (200 OK):
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "payment-schemas",
  "displayName": "Enhanced Payment Processing Schemas",
  "description": "Comprehensive schemas for all payment processing workflows including fraud detection",
  "status": "Active",
  "createdAt": "2025-09-21T10:30:00Z",
  "modifiedAt": "2025-09-21T11:15:00Z",
  "deletedAt": null
}
```

### Step 5: Verify Namespace is Active Again

```bash
curl -X GET "$BASE_URL/namespaces" \
  -H "$(auth_header)"
```

**Expected Response** (200 OK):
```json
{
  "namespaces": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "payment-schemas",
      "displayName": "Enhanced Payment Processing Schemas",
      "description": "Comprehensive schemas for all payment processing workflows including fraud detection",
      "status": "Active",
      "createdAt": "2025-09-21T10:30:00Z",
      "modifiedAt": "2025-09-21T11:15:00Z",
      "deletedAt": null
    }
  ],
  "totalCount": 1,
  "pageSize": 20,
  "pageNumber": 1
}
```

## Scenario 4: Permanent Deletion

### Step 1: Soft Delete First

```bash
curl -X DELETE "$BASE_URL/namespaces/123e4567-e89b-12d3-a456-426614174000" \
  -H "$(auth_header)"
```

**Expected Response** (204 No Content)

### Step 2: Permanently Delete Namespace

**⚠️ WARNING**: This operation is irreversible and will permanently remove the namespace and all contained schemas.

```bash
curl -X DELETE "$BASE_URL/namespaces/123e4567-e89b-12d3-a456-426614174000/permanent" \
  -H "$(auth_header)"
```

**Expected Response** (204 No Content)

### Step 3: Verify Namespace is Completely Removed

```bash
curl -X GET "$BASE_URL/namespaces/123e4567-e89b-12d3-a456-426614174000" \
  -H "$(auth_header)"
```

**Expected Response** (404 Not Found):
```json
{
  "type": "https://api.schemaregistry.company.com/problems/namespace-not-found",
  "title": "Namespace not found",
  "status": 404,
  "detail": "Namespace with ID '123e4567-e89b-12d3-a456-426614174000' was not found",
  "instance": "/api/namespaces/123e4567-e89b-12d3-a456-426614174000"
}
```

## Scenario 5: Error Handling and Validation

### Step 1: Attempt to Create Duplicate Namespace

```bash
# First, create a namespace
curl -X POST "$BASE_URL/namespaces" \
  -H "$(auth_header)" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "user-schemas",
    "displayName": "User Management Schemas"
  }'

# Then try to create another with the same name
curl -X POST "$BASE_URL/namespaces" \
  -H "$(auth_header)" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "user-schemas",
    "displayName": "Duplicate User Schemas"
  }'
```

**Expected Response** (409 Conflict):
```json
{
  "type": "https://api.schemaregistry.company.com/problems/namespace-already-exists",
  "title": "Namespace already exists",
  "status": 409,
  "detail": "A namespace with the name 'user-schemas' already exists",
  "instance": "/api/namespaces",
  "errors": {
    "name": ["Name must be unique"]
  }
}
```

### Step 2: Invalid Namespace Name

```bash
curl -X POST "$BASE_URL/namespaces" \
  -H "$(auth_header)" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "invalid name with spaces!",
    "displayName": "Invalid Namespace"
  }'
```

**Expected Response** (400 Bad Request):
```json
{
  "type": "https://api.schemaregistry.company.com/problems/validation-failed",
  "title": "Validation failed",
  "status": 400,
  "detail": "One or more validation errors occurred",
  "instance": "/api/namespaces",
  "errors": {
    "name": ["Name must contain only alphanumeric characters, hyphens, and underscores"]
  }
}
```

### Step 3: Unauthorized Access

```bash
curl -X GET "$BASE_URL/namespaces" \
  -H "Authorization: Bearer invalid-token"
```

**Expected Response** (401 Unauthorized):
```json
{
  "type": "https://api.schemaregistry.company.com/problems/unauthorized",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid or expired JWT token",
  "instance": "/api/namespaces"
}
```

### Step 4: Insufficient Permissions

```bash
# Using a token with only namespace-read permission
curl -X POST "$BASE_URL/namespaces" \
  -H "Authorization: Bearer read-only-token" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "test-namespace",
    "displayName": "Test Namespace"
  }'
```

**Expected Response** (403 Forbidden):
```json
{
  "type": "https://api.schemaregistry.company.com/problems/insufficient-permissions",
  "title": "Insufficient permissions",
  "status": 403,
  "detail": "Operation requires 'namespace-write' permission",
  "instance": "/api/namespaces"
}
```

## Testing Checklist

Use this checklist to validate the implementation:

### Basic Operations
- [ ] ✅ Create namespace with valid data
- [ ] ✅ Retrieve namespace by ID
- [ ] ✅ Update namespace metadata
- [ ] ✅ List namespaces with pagination
- [ ] ✅ Search namespaces by text

### Lifecycle Management
- [ ] ✅ Soft delete namespace
- [ ] ✅ Restore soft-deleted namespace
- [ ] ✅ Permanently delete namespace
- [ ] ✅ Verify cascade operations work correctly

### Error Handling
- [ ] ✅ Handle duplicate namespace names
- [ ] ✅ Validate namespace name format
- [ ] ✅ Return proper error responses
- [ ] ✅ Handle unauthorized requests
- [ ] ✅ Enforce permission requirements

### Authorization
- [ ] ✅ Require JWT token for all operations
- [ ] ✅ Enforce namespace-read for read operations
- [ ] ✅ Enforce namespace-write for write operations
- [ ] ✅ Return appropriate HTTP status codes

### Performance
- [ ] ✅ Response times under 200ms for typical operations
- [ ] ✅ Handle concurrent requests correctly
- [ ] ✅ Pagination works efficiently
- [ ] ✅ Search operations are performant

## Integration with Schema Management

When schemas are added to the namespace, verify:

- [ ] ✅ Schemas are properly associated with namespace
- [ ] ✅ Soft deleting namespace soft deletes all schemas
- [ ] ✅ Restoring namespace restores all schemas
- [ ] ✅ Permanently deleting namespace removes all schemas
- [ ] ✅ Schemas can be accessed with `deleted=true` filter

---

This quickstart guide serves as both documentation and acceptance testing. All scenarios should pass before considering the feature complete.
