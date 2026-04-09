---
description: Standardized Project Development Guidelines for TTL ERP
---

# TTL ERP - Standardized Project Guidelines

This workflow defines the structural and architectural standardization for developing the TTL ERP system. When starting a new service, feature, or refactoring an existing one, strictly adhere to these guidelines.

## 1. Directory Structure Standards
- `src/`: All source code (C# Backend, Blazor/JS Frontend).
- `tests/`: Unit, Integration, and E2E tests corresponding to the `src` directory.
- `infrastructure/`: Infrastructure as Code (Docker, Terraform, scripts).
- `.agent/`: AI Agent configurations, workflows, and prompts.

## 2. Microservice/Module Structure
Each microservice or logical bounded context MUST follow Clean Architecture principles:
- **Domain**: Entities, Enums, Exceptions, Interfaces, Types and Logic specific to the domain layer.
- **Application**: CQRS (Commands/Queries), MediatR handlers, DTOs, and Interfaces.
- **Infrastructure**: Database context (EF Core), Repositories, Caching, and External Services.
- **API/Presentation**: Controllers, Minimal APIs, or GraphQL endpoints.

## 3. Technology Stack & Best Practices
- **Backend**: .NET 8/9 C#, EF Core, MediatR.
- **Database**: PostgreSQL (relational) / MongoDB (documents), following the `/templates` schemas.
- **Caching**: Redis for distributed caching.
- **Event Bus**: Kafka for cross-service asynchronous messaging.
- **Frontend**: Blazor / JS with modern UI guidelines.

## 4. How to Create a New Feature
1. Define the requirement using `Product Manager` skill.
2. Determine architecture updates using `Architect` skill.
3. Run slash command `/01-scaffold-new-service` to generate boilerplate.
4. Implement application logic following CQRS.
5. Provide tests using `QA Engineer` skill via `/06-qa-test-suite`.
