# SchemaRegistry Development Guidelines

Auto-generated from all feature plans. Last updated: 2025-09-21

## Active Technologies
- .NET 9.0 with C# 13 + ASP.NET Core
- FluentValidation for validation
- Microsoft Orleans for distributed applications
- OpenTelemetry for tracing, metrics, and logging
- PostgreSQL with Marten for document storage and event sourcing
- .NET Aspire for project scaffolding and conventions

## Project Structure
```
/AppHost/ # .NET Aspire application host
/ServiceDefaults/ # .NET Aspire service defaults
/SchemaRegistry.Domain/ # Domain layer
/SchemaRegistry.Application/ # Application layer
/SchemaRegistry.Infrastructure/ # Infrastructure layer
/SchemaRegistry.WebApi/ # API layer
/test/ # Unit and integration tests
```

## Commands

## Code Style
.NET 9.0 with C# 13: Follow standard conventions

## Recent Changes
- 001-namespace: Added .NET 9.0 with C# 13 + ASP.NET Core, Marten (document storage & event sourcing), FluentValidation, Microsoft Orleans, OpenTelemetry, .NET Aspire

<!-- MANUAL ADDITIONS START -->
<!-- MANUAL ADDITIONS END -->
