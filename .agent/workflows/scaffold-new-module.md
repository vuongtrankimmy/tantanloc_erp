---
description: Automated E2E Module Scaffolding using the TTL Standard Skill Kit
---

# Workflow: Scaffold New Module

This workflow guides the automated creation of a new functional module (API + Frontend + MCP) following the TTL Premium Standards.

## 🚀 Pre-requisites
- Project Name (e.g., `AssetManagement`)
- Primary Entity Name (e.g., `Asset`)
- Required features (CRUD, Excel, Auth, MCP)

## 🛠️ Step-by-Step Execution

// turbo
### 1. Backend Scaffolding
Use the `ttl-project-scaffolding` skill to create the base projects.
- [ ] Create `{{ProjectName}}.Domain`, `Application`, `Infrastructure`, `API`.
- [ ] Setup namespaces and project references.

### 2. Implementation Lifecycle
Use the `ttl-api-feature-lifecycle` and `ttl-error-handling-logging` skills.
- [ ] Create `{{EntityName}}` entity and repository.
- [ ] Implement Create, Update, Delete, List commands/queries.
- [ ] Ensure `Transactional Outbox` is used for events.

### 3. Frontend Implementation
Use the `ttl-frontend-premium-page` skill.
- [ ] Create `{EntityName}}List.razor` with Skeleton Loading.
- [ ] Use `DeleteConfirmationModal` for deletions.
- [ ] Apply premium Metronic styling.

### 4. Advanced Integrations
- [ ] **Security**: Use `ttl-identity-integration` to add `[Authorize]` and permissions.
- [ ] **i18n**: Use `ttl-i18n-workflow` to add labels to `.resx` and create `_translate` collection.
- [ ] **Reporting**: Use `ttl-reporting-excel` if export is needed.
- [ ] **AI**: Use `ttl-mcp-dotnet-server` to expose tools for the agent.

## ✅ Verification
1. Run `dotnet build` on the solution.
2. verify the API endpoint via Swagger.
3. Verify the Blazor UI loads with skeletons and displays mock data.
