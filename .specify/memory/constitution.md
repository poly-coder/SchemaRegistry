<!--
Sync Impact Report
Version change: 1.0.0 → 1.0.0 (no version change requested)
Modified principles:
- II. Code Quality & Testing (expanded with Domain-Driven Development Terminology requirement)
- III. Technology Stack & Infrastructure (expanded with MermaidJS Diagram Requirements)
- IV. Architecture & Design (expanded with Data Contracts requirement)
Added sections:
- Domain-Driven Development Terminology requirement (DDD jargon for consistent domain modeling)
- MermaidJS Diagram Requirements (visual technical documentation standards)
- Data Contracts requirement (explicit API interface definitions)
Removed sections:
- None
Templates requiring updates:
- ✅ .specify/templates/plan-template.md (Constitution Check updated with DDD terminology, MermaidJS diagrams, and data contracts requirements)
- ✅ .specify/templates/spec-template.md (no changes needed - correctly focused on functional requirements)
- ✅ .specify/templates/tasks-template.md (validation checklist updated with DDD terminology, MermaidJS diagrams, and data contracts requirements)
Follow-up TODOs:
- None (all templates aligned with expanded Code Quality, Technology Stack, and Architecture requirements)
-->

# Schema Registry Service Constitution

## Core Principles

### I. Version Control & Change Management

**Trunk-Based Development**: Repository MUST use Git with `main` branch as primary integration branch following trunk-based development model.

**Feature Branch Strategy**: All development work MUST occur on feature branches named `feature/<feature-name>`, never directly on main branch.

**Pull Request Integration**: All changes to main branch MUST go through Pull Requests with mandatory code review before merging.

**History Preservation**: History rewriting is FORBIDDEN once pushed to remote. Local rebase before push is permitted and encouraged for clean history.

**Commit Standards**: Commit messages MUST be clear, concise, and follow consistent format, ideally referencing relevant issues or feature requests.

Rationale: Ensures code quality through review, maintains audit trail, enables collaborative development, and prevents accidental direct changes to production code.

### II. Code Quality & Testing

**Business Logic Coverage**: Business logic MUST achieve 100% test coverage with individual tests for each business rule using xUnit framework.

**Testing Libraries**: Test implementation MUST use Moq for mocking, Verify for assertion extensions, and Shouldly for fluent assertions to ensure consistent and readable test code.

**Domain-Driven Development Terminology**: All code, documentation, and communications MUST use established DDD terminology including but not limited to: Aggregate Root, Entity, Value Object, Domain Service, Application Service, Repository, Domain Event, Bounded Context, and Ubiquitous Language to ensure consistent domain modeling and team communication.

**Infrastructure Testing**: Infrastructure code MUST be validated through integration tests; full coverage not required.

**Quality Standards**: All warnings MUST be treated as errors to ensure high code quality. Consistent code style MUST be enforced using EditorConfig and Code Analyzers.

**Technical Debt Management**: Technical debt MUST be minimized but is acceptable when perfect solutions are prohibitively costly for current feature delivery.

**Error Handling Standard**: All errors MUST be logged and converted to ProblemDetails at service boundaries with no internal system information leaked.

**Observability Required**: All errors MUST include metrics, traces, and structured logs for diagnostic purposes.

**Continuous Integration**: Automated testing MUST be implemented as part of CI/CD pipeline to catch issues early. Code coverage tools MUST be used to monitor and improve test coverage over time.

Rationale: Ensures business logic reliability, maintains high code quality standards with consistent testing frameworks and domain-driven terminology, enables early issue detection, and maintains system security through proper error handling.

### III. Technology Stack & Infrastructure

**.NET Primary Stack**: Project MUST primarily utilize .NET framework and C# programming language, leveraging their robust ecosystem and tooling.

**Solution Architecture**: Project MUST follow Clean Architecture with distinct layers: Domain (business logic), Application (use cases), Infrastructure (data access and external services), and WebApi (presentation layer).

**ASP.NET Core Services**: Web APIs and services MUST be built using ASP.NET Core following .NET best practices and coding standards.

**.NET Tooling**: Proper use of .NET tooling is REQUIRED including `dotnet` CLI commands, NuGet package management, and .NET Aspire for local development and testing.

**Event Sourcing Implementation**: Event sourcing MUST be implemented using the Marten library over PostgreSQL database for domain event persistence, projection management, and event stream handling.

**Scaling and Distributed Computing**: Microsoft Orleans MUST be used for service scaling, use case management, application caching, and distributed actor-based processing to ensure horizontal scalability and resilient service coordination.

**Workflow Management**: Temporal.io MUST be used for long-running workflows, durable execution, and complex business process orchestration to ensure reliable and resumable workflow processing.

**Observability Implementation**: OpenTelemetry MUST be used for comprehensive observability including distributed tracing, metrics collection, and structured logging to ensure system monitoring and diagnostics.

**Local Development**: .NET Aspire MUST be used for local development orchestration, service discovery, and testing to ensure consistent development experience and simplified service coordination.

**Containerization**: Docker MUST be used for containerizing applications and services to ensure consistency across different environments (development, testing, production).

**CI/CD Pipeline**: GitHub Actions MUST be used for continuous integration and deployment workflows with automated building, testing, and deployment.

**Documentation Standard**: All specifications and documentation MUST be written in Markdown format and maintained in the `specs` directory to ensure readability and consistency.

**Diagram Requirements**: All architectural designs, system flows, and complex relationships MUST be documented using MermaidJS diagrams embedded in Markdown files to ensure visual clarity and maintainable technical documentation.

Rationale: Establishes consistent technology foundation with clear architectural boundaries, ensures reliable event sourcing through Marten/PostgreSQL, enables horizontal scaling through Orleans and durable workflows via Temporal.io, provides comprehensive observability through OpenTelemetry, enables simplified local development through .NET Aspire, maintains documentation standards, and ensures visual technical documentation through MermaidJS diagrams.

### IV. Architecture & Design

**Separation of Concerns**: Clear boundaries MUST be maintained between business logic, infrastructure, and presentation layers.

**Dependency Direction**: Dependencies MUST flow inward - infrastructure depends on business logic, not vice versa.

**Domain Layer Validation**: Domain layer MUST use FluentValidation library for all validation logic and SHALL NOT include any other third-party dependencies to maintain purity and testability.

**Soft Deletion Pattern**: Resources MUST implement soft deletion using timestamp fields (e.g., `DeletedAt`) rather than physical removal for data integrity and audit trails.

**Event Sourcing**: All significant mutations MUST emit domain events for traceability, audit, and integration purposes.

**RESTful API Design**: APIs MUST follow RESTful principles with standard HTTP verbs, appropriate status codes, and consistent resource naming.

**Data Contracts**: All API interfaces and data transfer objects MUST be explicitly defined with clear contracts including request/response schemas, validation rules, and versioning strategies to ensure API consistency and consumer clarity.

**Idempotent Operations**: All operations MUST be idempotent and resilient to transient failures for system reliability.

Rationale: Ensures maintainable architecture, reliable data management, pure domain layer with controlled dependencies through FluentValidation, comprehensive audit capabilities, consistent API design patterns, and clear data contract definitions for API reliability.

## Governance

**Constitutional Authority**: This constitution supersedes all other project practices and guidelines where conflicts exist.

**Amendment Process**: Constitutional changes require Pull Request with explicit rationale and impact analysis. MAJOR version for principle changes, MINOR for additions, PATCH for clarifications.

**Compliance Enforcement**: All Pull Requests MUST demonstrate adherence to constitutional principles through documented checks.

**Exception Handling**: Temporary exceptions to principles require explicit documentation with time-bound remediation plan and stakeholder approval.

**Version**: 1.0.0 | **Ratified**: 2025-09-21 | **Last Amended**: 2025-09-21
