# Constitution

## Purpose

This document provides constitutional instructions to guide the specification, design, and development of the Message Registry repository. It establishes foundational principles, responsibilities, and processes to ensure clarity, consistency, and quality throughout the project lifecycle.

## Core Principles

### Git Version Control

- The repository will use Git for version control, following a trunk-based development model with a `main` branch as the primary integration branch.
- Feature branches will be created for new features or significant changes, named using the format `feature/<feature-name>`.
- Pull requests will be used for code reviews and merging changes into the `main` branch.
- Commit messages should be clear, concise, and follow a consistent format, ideally referencing relevant issues or feature requests.

### Markdown for Documentation and Specifications

- Comprehensive documentation will be maintained in the `specs` directory, covering architecture, design decisions, API references, and user guides.
- All specifications should be written in Markdown format to ensure readability and consistency.

### .NET and C# as Primary Technology Stack

- The project will primarily utilize the .NET framework and C# programming language for development, leveraging their robust ecosystem and tooling.
- Adherence to .NET best practices and coding standards will be enforced to ensure code quality and maintainability.
- Proper use of the .NET tooling must be used, including `dotnet` CLI commands, NuGet package management, etc.
- Use of .NET Aspire for local development and testing.
- Use of xUnit for unit testing and integration testing.
- Use of ASP.NET Core for building web APIs and services.

### Docker for Containerization

- Docker will be used for containerizing applications and services to ensure consistency across different environments (development, testing, production).

### Code Style and Quality

- A consistent code style will be enforced across the codebase, using tools like EditorConfig and Code Analyzers.
- Regular code reviews will be conducted to maintain code quality and share knowledge among team members.
- All warnings should be treated as errors to ensure high code quality.
- Automated testing will be implemented to ensure code reliability and facilitate continuous integration and deployment (CI/CD).

### Continuous Integration

- A CI/CD pipeline will be established to automate the building, testing, and deployment of the application.
- Automated tests will be run as part of the CI process to catch issues early in the development cycle.
- Use of GitHub Actions for CI/CD workflows.
- Code coverage tools will be used to monitor and improve test coverage over time.
