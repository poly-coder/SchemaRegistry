# Tasks: [FEATURE NAME]

**Input**: Design documents from `/specs/[###-feature-name]/`
**Prerequisites**: plan.md (required), research.md, data-model.md, contracts/

## Execution Flow (main)

```text
1. Load plan.md from feature directory
   → If not found: ERROR "No implementation plan found"
   → Extract: tech stack, libraries, structure
2. Load optional design documents:
   → data-model.md: Extract entities → model tasks
   → contracts/: Each file → contract test task
   → research.md: Extract decisions → setup tasks
3. Generate tasks by category:
   → Setup: project init, dependencies, linting
   → Tests: contract tests, integration tests
   → Core: models, services, CLI commands
   → Integration: DB, middleware, logging
   → Polish: unit tests, performance, docs
4. Apply task rules:
   → Different files = mark [P] for parallel
   → Same file = sequential (no [P])
   → Tests before implementation (TDD)
5. Number tasks sequentially (T001, T002...)
6. Generate dependency graph
7. Create parallel execution examples
8. Validate task completeness:
   → All contracts have tests?
   → All entities have models?
   → All endpoints implemented?
9. Return: SUCCESS (tasks ready for execution)
```

## Format: `[ID] [P?] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- Include exact file paths in descriptions

## Path Conventions

- **Single project**: `src/`, `tests/` at repository root
- **Web app**: `backend/src/`, `frontend/src/`
- **Mobile**: `api/src/`, `ios/src/` or `android/src/`
- Paths shown below assume single project - adjust based on plan.md structure

## Phase 3.1: Setup

- [ ] T001 Create project structure per implementation plan
- [ ] T002 Initialize [language] project with [framework] dependencies
- [ ] T003 [P] Configure linting and formatting tools

## Phase 3.2: Tests First ⚠️ MUST COMPLETE BEFORE 3.3

CRITICAL: These tests MUST be written and MUST FAIL before ANY implementation

- [ ] T004 [P] Business logic test: [specific business rule] in tests/unit/test_[domain].py
- [ ] T005 [P] Business logic test: [specific business rule] in tests/unit/test_[domain].py
- [ ] T006 [P] Integration test: [infrastructure component] in tests/integration/test_[component].py
- [ ] T007 [P] Integration test: [service boundary] with ProblemDetails in tests/integration/test_api_errors.py

## Phase 3.3: Core Implementation (ONLY after tests are failing)

- [ ] T008 [P] [Domain] model with business logic in src/models/[domain].py
- [ ] T009 [P] [Domain]Service with business rules in src/services/[domain]_service.py
- [ ] T010 [P] CLI commands in src/cli/[domain]_commands.py
- [ ] T011 POST /api/[resource] endpoint
- [ ] T012 GET /api/[resource]/{id} endpoint
- [ ] T013 Input validation at API boundaries
- [ ] T014 Error handling with ProblemDetails conversion and structured logging

## Phase 3.4: Integration

- [ ] T015 Connect [Domain]Service to storage with proper transaction handling
- [ ] T016 Authentication/Authorization middleware
- [ ] T017 Request/response logging with metrics and traces
- [ ] T018 CORS and security headers

## Phase 3.5: Polish

- [ ] T019 [P] Unit tests for validation helpers in tests/unit/test_validation.py
- [ ] T020 Performance tests (validation p95 target per constitution)
- [ ] T021 [P] Update docs/api.md and schema change notes
- [ ] T022 Remove duplication
- [ ] T023 Run manual-testing.md

## Dependencies

- Tests (T004-T007) before implementation (T008-T014)
- T008 blocks T009, T015
- T016 blocks T018
- Implementation before polish (T019-T023)

## Parallel Example

```text
# Launch T004-T007 together:
Task: "Contract test schema create in tests/contract/test_schema_create.py"
Task: "Contract test schema get in tests/contract/test_schema_get.py"
Task: "Compatibility MINOR test in tests/contract/test_schema_compat_minor.py"
Task: "Compatibility MAJOR test in tests/contract/test_schema_compat_major.py"
```

## Notes

- [P] tasks = different files, no dependencies
- Verify tests fail before implementing
- Commit after each task
- Avoid: vague tasks, same file conflicts

## Task Generation Rules

Applied during main() execution

1. **From Contracts**:
   - Each contract file → contract test task [P]
   - Each endpoint → implementation task

2. **From Data Model**:
   - Each entity → model creation task [P]
   - Relationships → service layer tasks

3. **From User Stories**:
   - Each story → integration test [P]
   - Quickstart scenarios → validation tasks

4. **Ordering**:
   - Setup → Tests → Models → Services → Endpoints → Polish
   - Dependencies block parallel execution

## Validation Checklist

GATE: Checked by main() before returning

- [ ] All business logic has 100% test coverage with individual rule tests using xUnit
- [ ] Moq, Verify, and Shouldly used for test implementation (mocking, assertions, fluent assertions)
- [ ] Infrastructure components have integration tests
- [ ] All tests come before implementation
- [ ] Parallel tasks truly independent
- [ ] Each task specifies exact file path
- [ ] No task modifies same file as another [P] task
- [ ] Error handling includes ProblemDetails conversion
- [ ] Observability tasks include metrics, traces, and structured logs
- [ ] .NET/C# stack used with proper tooling (dotnet CLI, NuGet)
- [ ] Clean Architecture layers followed (Domain, Application, Infrastructure, WebApi)
- [ ] ASP.NET Core used for API endpoints
- [ ] FluentValidation used for Domain layer validation (only third-party dependency in Domain)
- [ ] Marten library configured for event sourcing over PostgreSQL
- [ ] Microsoft Orleans configured for service scaling and application caching
- [ ] Temporal.io configured for long-running workflows and durable execution
- [ ] OpenTelemetry configured for comprehensive observability (tracing, metrics, logging)
- [ ] .NET Aspire configured for local development orchestration
- [ ] Docker containerization configured
- [ ] Soft deletion pattern implemented where applicable
- [ ] Domain events emitted for mutations
- [ ] RESTful API design followed
- [ ] Operations are idempotent
