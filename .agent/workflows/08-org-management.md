---
description: Enterprise Organizational Management Workflow
---

# Enterprise Organizational Management Workflow

This workflow represents the macro view of task distributions for TTL ERP driven by the Chief Technology Officer (CTO).

## Stages for Organization-wide Directives

1. **CTO Intake & Requirements Structuring**
   - Read user input and cross-reference with `.agent/org_library/`.
   - Update `.agent/tasks/backlog.md` with structured tickets/modules.
2. **Product Requirement Document Generation**
   - Delegate to `01_product\product_manager` to build functional specifications.
3. **Architecture Check**
   - Pass PRD to `02_engineering\lead_architect` to create or update architectural blueprints, DB schema plans.
4. **Development Cycles**
   - Send technical specs to `backend_developer` and `frontend_developer` respectively.
   - Enforce coding standards set in `.agent/rules/`.
5. **Quality Gate**
   - Hand off to `03_qa\qa_engineer` for automated test generation and validation `/06-qa-test-suite`.
6. **Infrastructure Ops**
   - If tests pass, call `04_operations\devops_engineer` to wrap, build Docker, and deploy in `/05-deploy-production`.

## Invocation

Run this workflow by using the Slash Command `/08-org-management` when a feature touches multiple layers of the technology stack or involves strategic decision-making.
