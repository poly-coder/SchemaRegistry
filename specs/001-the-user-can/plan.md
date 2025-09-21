
# Implementation Plan: Namespace Management for Schema Registry

**Branch**: `001-namespace` | **Date**: September 21, 2025 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-the-user-**Phase Status**:

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
- [x] Complexity deviations documented (none required)# Execution Flow (/plan command scope)

```text
1. Load feature spec from Input path
   → ✅ Feature spec loaded successfully
2. Fill Technical Context (scan for NEEDS CLARIFICATION)
   → ✅ Project Type: web (Clean Architecture with multiple layers)
   → ✅ Structure Decision: Clean Architecture (.NET solution)
3. Fill the Constitution Check section based on the content of the constitution document.
   → ✅ Constitution principles identified and mapped
4. Evaluate Constitution Check section below
   → ✅ No violations detected - design aligns with constitutional requirements
   → ✅ Update Progress Tracking: Initial Constitution Check
5. Execute Phase 0 → research.md
   → ✅ All technical decisions resolved and documented
6. Execute Phase 1 → contracts, data-model.md, quickstart.md, agent context update
   → ✅ Data model with DDD patterns documented
   → ✅ OpenAPI contracts generated for all endpoints
   → ✅ Quickstart scenarios covering all user stories
   → ✅ GitHub Copilot context updated
7. Re-evaluate Constitution Check section
   → ✅ Post-design validation confirms constitutional compliance
   → ✅ Update Progress Tracking: Post-Design Constitution Check
8. Plan Phase 2 → Describe task generation approach (DO NOT create tasks.md)
   → ✅ Task planning strategy documented
9. STOP - Ready for /tasks command
   → ✅ COMPLETE - All planning phases finished
```

**IMPORTANT**: The /plan command STOPS at step 7. Phases 2-4 are executed by other commands:

- Phase 2: /tasks command creates tasks.md
- Phase 3-4: Implementation execution (manual or via tools)

## Summary

Implement namespace management functionality for Schema Registry allowing users to create, update, soft-delete, restore, and permanently delete namespaces with cascade operations for contained schemas. Technical approach uses .NET Clean Architecture with Entity Framework, JWT authorization, event sourcing via Marten, and RESTful API design following DDD principles.

## Technical Context

**Language/Version**: .NET 8.0 with C# 12
**Primary Dependencies**: ASP.NET Core, Entity Framework Core, Marten (event sourcing), FluentValidation, Microsoft Orleans, OpenTelemetry
**Storage**: PostgreSQL with Marten for event sourcing
**Testing**: xUnit, Moq, Verify, Shouldly for comprehensive test coverage
**Target Platform**: Linux containers (Docker)
**Project Type**: web - Clean Architecture with Domain/Application/Infrastructure/WebApi layers
**Performance Goals**: 1000 req/s, <200ms p95 response time
**Constraints**: JWT-based authorization with namespace-read/namespace-write permissions
**Scale/Scope**: Enterprise-level schema registry supporting multiple teams and thousands of schemas

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Validate design against the Constitution:

- Version Control: Feature branch strategy used; no direct main commits; PR workflow with code review planned.
- Code Quality: Business logic test coverage strategy defined (100% with xUnit); Moq, Verify, and Shouldly used for testing; DDD terminology consistently applied throughout; infrastructure integration test approach specified; error handling with ProblemDetails planned; warnings treated as errors.
- Technology Stack: .NET/C# stack used; Clean Architecture with Domain/Application/Infrastructure/WebApi layers; ASP.NET Core for APIs; Marten library for event sourcing over PostgreSQL; Microsoft Orleans for scaling and caching; Temporal.io for long-running workflows; OpenTelemetry for observability; .NET Aspire for local development; Docker containerization planned; GitHub Actions CI/CD configured; MermaidJS diagrams used for technical documentation.
- Architecture: Layer separation maintained; dependency direction correct; FluentValidation used in Domain layer as only third-party dependency; soft deletion pattern used; RESTful API design with explicit data contracts; event sourcing for mutations; idempotent operations.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
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

**Structure Decision**: [DEFAULT to Option 1 unless Technical Context indicates web/mobile app]

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

Prerequisite: research.md complete

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

This section describes what the /tasks command will do - DO NOT execute during /plan

**Task Generation Strategy**:

- Load `.specify/templates/tasks-template.md` as base
- Generate tasks from Phase 1 design docs (contracts, data model, quickstart)
- Each contract → contract test task [P]
- Each entity → model creation task [P]
- Each user story → integration test task
- Implementation tasks to make tests pass

**Ordering Strategy**:

- TDD order: Tests before implementation
- Dependency order: Models before services before UI
- Mark [P] for parallel execution (independent files)

**Estimated Output**: 25-30 numbered, ordered tasks in tasks.md

**IMPORTANT**: This phase is executed by the /tasks command, NOT by /plan

## Phase 3+: Future Implementation

These phases are beyond the scope of the /plan command

**Phase 3**: Task execution (/tasks command creates tasks.md)
**Phase 4**: Implementation (execute tasks.md following constitutional principles)
**Phase 5**: Validation (run tests, execute quickstart.md, performance validation)

## Complexity Tracking

Fill ONLY if Constitution Check has violations that must be justified

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., 4th project] | [current need] | [why 3 projects insufficient] |
| [e.g., Repository pattern] | [specific problem] | [why direct DB access insufficient] |


## Progress Tracking

This checklist is updated during execution flow

**Phase Status**:

- [ ] Phase 0: Research complete (/plan command)
- [ ] Phase 1: Design complete (/plan command)
- [ ] Phase 2: Task planning complete (/plan command - describe approach only)
- [ ] Phase 3: Tasks generated (/tasks command)
- [ ] Phase 4: Implementation complete
- [ ] Phase 5: Validation passed

**Gate Status**:

- [ ] Initial Constitution Check: PASS
- [ ] Post-Design Constitution Check: PASS
- [ ] All NEEDS CLARIFICATION resolved
- [ ] Complexity deviations documented

---
*Based on Constitution v1.0.0 - See `.specify/memory/constitution.md`*
