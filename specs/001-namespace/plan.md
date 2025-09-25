
# Implementation Plan: Namespace Management for Schema Registry

**Branch**: `001-namespace` | **Date**: September 25, 2025 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-namespace/spec.md`

## Execution Flow (/plan command scope)

```text
1. Load feature spec from Input path
   → If not found: ERROR "No feature spec at {path}"
2. Fill Technical Context (scan for NEEDS CLARIFICATION)
   → Detect Project Type from context (web=frontend+backend, mobile=app+api)
   → Set Structure Decision based on project type
3. Fill the Constitution Check section based on the content of the constitution document.
4. Evaluate Constitution Check section below
   → If violations exist: Document in Complexity Tracking
   → If no justification possible: ERROR "Simplify approach first"
   → Update Progress Tracking: Initial Constitution Check
5. Execute Phase 0 → research.md
   → If NEEDS CLARIFICATION remain: ERROR "Resolve unknowns"
6. Execute Phase 1 → contracts, data-model.md, quickstart.md, agent-specific template file (e.g., `CLAUDE.md` for Claude Code, `.github/copilot-instructions.md` for GitHub Copilot, `GEMINI.md` for Gemini CLI, `QWEN.md` for Qwen Code or `AGENTS.md` for opencode).
7. Re-evaluate Constitution Check section
   → If new violations: Refactor design, return to Phase 1
   → Update Progress Tracking: Post-Design Constitution Check
8. Plan Phase 2 → Describe task generation approach (DO NOT create tasks.md)
9. STOP - Ready for /tasks command
```

**IMPORTANT**: The /plan command STOPS at step 7. Phases 2-4 are executed by other commands:

- Phase 2: /tasks command creates tasks.md
- Phase 3-4: Implementation execution (manual or via tools)

## Summary

Implement namespace management functionality for the Schema Registry service to allow users to organize schemas into logical namespaces. Each namespace has a unique name, optional display name, description, and markdown documentation. The system supports full CRUD operations including soft deletion and restoration. The implementation will use .NET 9.0 with Clean Architecture, Marten for event sourcing, Orleans for distributed processing, and FluentValidation for domain validation.

## Technical Context

**Language/Version**: .NET 9.0 with C# 13
**Primary Dependencies**: ASP.NET Core, FluentValidation, Microsoft Orleans, OpenTelemetry, Marten
**Storage**: PostgreSQL with Marten for document storage and event sourcing
**Testing**: xUnit, Moq, Verify, Shouldly
**Target Platform**: Docker containers on Linux servers
**Project Type**: web (backend service with REST API)
**Performance Goals**: <100ms p95 for CRUD operations, support 10,000+ namespaces
**Constraints**: 99.9% availability, continuation token pagination, idempotent operations
**Scale/Scope**: Microservice architecture, clean architecture layers, RESTful API design

**User-Provided Context**: Review existing code on namespace feature with respect to the specs. Start from Domain layer, up to WebApi.

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Validate design against the Constitution:

- Version Control: Feature branch strategy used; no direct main commits; PR workflow with code review planned.
- Code Quality: Business logic test coverage strategy defined (100% with xUnit); Moq, Verify, and Shouldly used for testing; DDD terminology consistently applied throughout; infrastructure integration test approach specified; error handling with ProblemDetails planned; warnings treated as errors.
- Technology Stack: .NET/C# stack used; Clean Architecture with Domain/Application/Infrastructure/WebApi layers; ASP.NET Core for APIs; Marten library for event sourcing over PostgreSQL; Microsoft Orleans for scaling and caching; Temporal.io for long-running workflows; OpenTelemetry for observability; .NET Aspire for local development; Docker containerization planned; GitHub Actions CI/CD configured; MermaidJS diagrams used for technical documentation.
- Architecture: Layer separation maintained; dependency direction correct; FluentValidation used in Domain layer as only third-party dependency; soft deletion pattern used; RESTful API design with explicit data contracts; continuation token pagination for all list operations; standard entity query patterns (List, Get by ID, Get by IDs) implemented for all entities; event sourcing for mutations; idempotent operations.

## Project Structure

### Documentation (this feature)

```text
specs/001-namespace/
├── spec.md              # Specification (input to /plan command)
├── plan.md              # This file (/plan command output)
├── research.md          # Phase 0 output (/plan command)
├── data-model.md        # Phase 1 output (/plan command)
├── quickstart.md        # Phase 1 output (/plan command)
├── contracts/           # Phase 1 output (/plan command)
└── tasks.md             # Phase 2 output (/tasks command - NOT created by /plan)
```

### Source Code (repository root)

```text
# Option 1: Single project (DEFAULT)
src/
├── models/
├── services/
├── cli/
└── lib/

tests/
├── contract/
├── integration/
└── unit/

# Option 2: Web application (when "frontend" + "backend" detected)
backend/
├── src/
│   ├── models/
│   ├── services/
│   └── api/
└── tests/

frontend/
├── src/
│   ├── components/
│   ├── pages/
│   └── services/
└── tests/

# Option 3: Mobile + API (when "iOS/Android" detected)
api/
└── [same as backend above]

ios/ or android/
└── [platform-specific structure]
```

**Structure Decision**: Option 1 - Single project with existing Clean Architecture layers (Domain, Application, Infrastructure, WebApi)

## Phase 0: Outline & Research

1. **Extract unknowns from Technical Context** above:
   - For each NEEDS CLARIFICATION → research task
   - For each dependency → best practices task
   - For each integration → patterns task

2. **Generate and dispatch research agents**:

   ```text
   For each unknown in Technical Context:
     Task: "Research {unknown} for {feature context}"
   For each technology choice:
     Task: "Find best practices for {tech} in {domain}"
   ```

3. **Consolidate findings** in `research.md` using format:
   - Decision: [what was chosen]
   - Rationale: [why chosen]
   - Alternatives considered: [what else evaluated]

**Output**: research.md with all NEEDS CLARIFICATION resolved

## Phase 1: Design & Contracts

Prerequisites: research.md complete

1. **Extract entities from feature spec** → `data-model.md`:
   - Entity name, fields, relationships
   - Validation rules from requirements
   - State transitions if applicable

2. **Generate API contracts** from functional requirements:
   - For each user action → endpoint
   - Use standard REST/GraphQL patterns
   - Output OpenAPI/GraphQL schema to `/contracts/`

3. **Generate contract tests** from contracts:
   - One test file per endpoint
   - Assert request/response schemas
   - Tests must fail (no implementation yet)

4. **Extract test scenarios** from user stories:
   - Each story → integration test scenario
   - Quickstart test = story validation steps

5. **Update agent file incrementally** (O(1) operation):
   - Run `.specify/scripts/powershell/update-agent-context.ps1 -AgentType copilot`
     **IMPORTANT**: Execute it exactly as specified above. Do not add or remove any arguments.
   - If exists: Add only NEW tech from current plan
   - Preserve manual additions between markers
   - Update recent changes (keep last 3)
   - Keep under 150 lines for token efficiency
   - Output to repository root

**Output**: data-model.md, /contracts/*, failing tests, quickstart.md, agent-specific file

## Phase 2: Task Planning Approach

**Note**: This section describes what the /tasks command will do - DO NOT execute during /plan

**Task Generation Strategy**:

- Load `.specify/templates/tasks-template.md` as base
- Generate tasks from Phase 1 design docs (contracts, data model, quickstart)
- Domain Events: Create NamespaceCreated, NamespaceUpdated, NamespaceDeleted, NamespaceRestored events [P]
- Response Models: Create new response DTOs for read operations [P]
- Query Operations: Add List, GetById with detailed responses, GetByIds batch operations [P]
- REST Endpoints: Add missing GET endpoints for namespace retrieval
- Continuation Token Pagination: Implement pagination support
- Error Handling: Enhance with ProblemDetails format
- Integration Tests: Create comprehensive test scenarios from quickstart
- Contract Tests: Validate OpenAPI specification against implementation

**Ordering Strategy**:

- TDD order: Tests before implementation
- Layer order: Domain events → Application services → Infrastructure → REST endpoints
- Parallel tasks [P]: Independent model/event creation
- Sequential: API endpoint implementation after models complete

**Estimated Output**: 18-22 numbered, ordered tasks in tasks.md

**Key Task Categories**:

1. Domain Events (4 tasks) - NamespaceCreated, Updated, Deleted, Restored
2. Response Models (3 tasks) - DetailedResponse, ListResponse, BatchResponse
3. Query Services (3 tasks) - List, GetById, GetByIds operations
4. REST Endpoints (4 tasks) - Add GET operations to existing controller
5. Pagination (2 tasks) - Continuation token implementation
6. Error Handling (2 tasks) - ProblemDetails integration
7. Integration Tests (4 tasks) - Comprehensive test scenarios

**IMPORTANT**: This phase is executed by the /tasks command, NOT by /plan

## Phase 3+: Future Implementation

**Note**: These phases are beyond the scope of the /plan command

**Phase 3**: Task execution (/tasks command creates tasks.md)
**Phase 4**: Implementation (execute tasks.md following constitutional principles)
**Phase 5**: Validation (run tests, execute quickstart.md, performance validation)

## Complexity Tracking

**Note**: Fill ONLY if Constitution Check has violations that must be justified

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |

## Progress Tracking

**Note**: This checklist is updated during execution flow

**Phase Status**:

- [x] Phase 0: Research complete (/plan command)
- [x] Phase 1: Design complete (/plan command)
- [x] Phase 2: Task planning complete (/plan command - describe approach only)
- [ ] Phase 3: Tasks generated (/tasks command)
- [ ] Phase 4: Implementation complete
- [ ] Phase 5: Validation passed

**Gate Status**:

- [x] Initial Constitution Check: PASS
- [x] Post-Design Constitution Check: PASS
- [x] All NEEDS CLARIFICATION resolved
- [ ] Complexity deviations documented

---
*Based on Constitution v1.0.0 - See `.specify/memory/constitution.md`*
