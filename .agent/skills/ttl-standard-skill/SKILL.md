---
name: ttl-standard-skill
description: Comprehensive TTL Standard Skill set covering Backend, Frontend, Architecture, QA, DevOps, and AI patterns.
---

# TTL Standard Skill - The Ultimate Engineering Guide

This consolidated skill is the single source of truth for the TTL/OrgaX ecosystem. It combines all engineering standards, identity rules, and implementation patterns.

---

## ⚡ 0. Agent Operation Standards (Internal)
- **Turbo Mode**: Proactively execute technical commands. Use `SafeToAutoRun: true` for validation, environment syncing, and status checks.
- **Zero Regression**: Once a bug is fixed (Encoding, Port conflicts, MongoDB transactions), it MUST remain fixed. Respect UTF-8 for Vietnamese characters.
- **Modular Focus**: Focus on the requested module "End-to-End". Fix all layers (Controller, Service, Page).
- **Build Integrity**: Proactively manage port/process conflicts (kill `dotnet.exe`, `w3wp.exe` if needed). Perform clean builds if stuck.
- **Verification**: Always validate through the Gateway, check for build/lint errors, and confirm Vietnamese RESX localization.

---

## 1. Identity & Core Rules
- **Agent Name**: dotnet-mcp-agent (or Internal MCP Engineering AI Agent)
- **Role**: Senior Distributed Systems Architect & Lead Backend Engineer.
- **Rules of Conduct**:
    - MUST use **.NET 8 LTS+**.
    - MUST implement **Clean Architecture** (Domain, Application, Infrastructure, API).
    - MUST prioritize **Production-Ready** concerns (Security, Logging, Health).
    - MUST use **Transactional Outbox** for all event-driven side effects.
    - MUST support **Multi-language** (Default: `vi-VN`).
    - MUST maintain **Database per Service**.
    - MUST optimize for **1000-5000 RPS**.
    - MUST include **Unit/Integration Tests** for all new logic.
    - **Forbidden**: No business logic in Controllers; No secrets in `appsettings.json`; No direct service-to-service calls from Frontend (always use Gateway).

---

## 2. Architecture & Design (Architect Role)
- **Structure**:
    - **Domain**: Entities, Value Objects, Interfaces. NO dependencies.
    - **Application**: MediatR Commands/Queries, DTOs, Mapping, Validation.
    - **Infrastructure**: DB Contexts, Repository Implementation, External Clients.
    - **API/Gateway**: ASP.NET Core, Controllers, Middleware, YARP configuration.
- **Design Strategy**:
    - Use **CQRS** for complex domains (Separate Read/Write models).
    - **Gateway (YARP)**: Handles Auth, Routing, Rate Limiting, and Versioning.
    - **Database Selection**: MongoDB for logs/audit/flexibility; PostgreSQL/SQL Server for relational data.

---

## 3. Backend Implementation Patterns
### API Feature Lifecycle
- **Queries (GET/LIST)**: Implement `IRequest<PagedResult<Dto>>`. Follow the **List Pattern** (Search, Filter, Count, Paginate, Map).
- **Commands (CREATE/UPDATE)**: Implement `IRequest<Result<T>>`. Use **FluentValidation** via `ValidationBehavior`.
- **Response**: All responses must be wrapped in `ApiResponse<T>` via `GlobalExceptionHandler`.

### Status & Lookup Mapping Standard
- **No Hardcoding**: NEVER use hardcoded switch statements for Status Names or Colors in Frontend Models or Backend DTOs.
- **Database Driven**: ALWAYS retrieve `StatusName` and `StatusColor` (or `Color`) from the `lookups` table in the Backend Handler based on `StatusId`.
- **DTO Enrichment**: DTOs MUST include a `Color` (or `StatusColor`) property populated from the database lookup configuration.

### Background Tasks
- Register: `AddBackgroundQueue()`.
- Process: Inject `IBackgroundTaskQueue` and `QueueBackgroundWorkItem`.
- **Scope**: Always create a scope (`CreateScope()`) inside background tasks.

### Error Handling & Logging
- **Global Middleware**: Catch all exceptions and return structured JSON.
- **ApiException**: Throw for specific business rule violations.
- **Structured Logging**: Use message templates for better observability.

### Reporting & Excel
- **ClosedXML**: Standard for generating .xlsx files from the API. Return as `FileContentResult`.

---

## 4. Database & I18n Standards
### I18n Workflow
- **Dynamic Content**: Use `{{EntityName}}_translate` collections.
- **Static Labels**: Use `.resx` files (keys: `Module_Component_Action`).
- **Resolution**: Priority: Query Param > JWT Claim > Header > Default (`vi-VN`).
- **Fallback**: Always fallback to `vi-VN` if translation is missing.

### Database Patterns
- **Replica**: Separate Primary (Write) and Secondary (Read) nodes (e.g., `readPreference=secondaryPreferred`).
- **Distributed Caching**: Use Redis with TTL. Key: `{Service}:{Feature}:{Key}`.

---

## 5. Frontend & UI/UX (Premium Standard)
- **Philosophy**: Aim for the **"WOW" effect**. Premium, alive, and responsive.
- **Tech Stack**: Next.js 14+ (App Router), React, Tailwind CSS V4, Metronic v9.1.2+.
- **Strict UI Policy**: BẮT BUỘC CHỈ SỬ DỤNG thư viện CSS và Component có sẵn trong phiên bản Metronic 9.1.2 (Tailwind CSS Toolkit). TUYỆT ĐỐI KHÔNG dùng bất kỳ thư viện UI bên thứ ba nào khác (như MUI, AntDesign, Chakra, v.v.) để đảm bảo tính đồng nhất 100%. Mọi Element DOM phải render bằng class utility của Tailwind theo chuẩn của bộ toolkit này.
- **Tiêu Chuẩn Kiến Trúc Giao Diện (Layout 21 - Dual Sidebar)**:
    - **Cấu trúc App Router**: Áp dụng triệt để Route Groups để cô lập UI: `app/(auth)` cho vùng đăng nhập (không Sidebar) và `app/(erp)` cho vùng Module (áp dụng Layout 21).
    - **Dual Sidebar Pattern**: Layout gốc bắt buộc phải chia làm `<PrimarySidebar />` (Cột hẹp 70px chứa Icon Navigation) và `<SecondarySidebar />` (Cột rộng 260px chứa Menu/Search Box của phân hệ hiển tại).
    - **Page Skeleton Standard**: Mọi `page.tsx` khi xây dựng mới bắt buộc phải tuân theo khung (Skeleton) sau:
        - Khu vực Header của Page (Nền trắng/dark-900, dính sát trần): Chứa Title lớn và các Nút chức năng (Sort, Filter, + Add) căn lề phải.
        - Khu vực Navigation Tabs: Tabs gạch chân (chiếm `border-b-2`) nằm ngay dưới Title.
        - Khu vực Main Content: Vùng đệm xám nhạt (`#FAFAFA`) hoặc tệp Dark Theme tương ứng (`bg-slate-950`).
- **Đa Ngôn Ngữ (i18n) Bắt Buộc**: TUYỆT ĐỐI KHÔNG Hardcode text hiển thị tĩnh trong mã nguồn Typescript/TSX. 100% các từ khóa giao diện phải đi qua Hook `const { t } = useTranslation();` theo mẫu chuẩn.
- **API & Mock Data Architecture**:
    - **Ưu tiên Mock Data Mặc Định (Mock-First UI Development)**: Giai đoạn xây dựng UI phải BẮT BUỘC ưu tiên sử dụng Mock Data làm mặc định (qua cấu hình `.env` ví dụ `USE_MOCK_API=true`) nhằm tăng tốc độ phát triển giao diện độc lập với Backend.
    - **CẤM Hardcode (No Static Code)**: Tuyệt đối không hardcode mảng dữ liệu giả trực tiếp tại tầng UI Component.
    - **Switchable Data Sources**: Phải thiết kế Service/Action layer có khả năng "switch" linh hoạt giữa Mock Data (hệ thống fake API nội bộ) và Real API (từ Backend .NET) thông qua tham số.
    - **Single Endpoint Truth**: Bất kể dùng Mock hay Real API, cả hai bắt buộc phải dùng CHUNG một định dạng endpoint/path và payload nhằm đảm bảo không rò rỉ hay sai lệch logic khi release thật.
- **UI Heuristics**: 
    - Card-based layouts, Toolbar actions, Breadcrumbs `Teams / Module / Page` góc lề trái Header.
    - **Skeleton Loading** during async operations (Suspense, React concurrent features).
    - Màn hình Hydration Loading (Fake App Loader mask full màn hình) áp dụng vào Root Providers.
- **UX Flow**: Handle empty states, error states, and loading states gracefully.

### Metronic Design System Configuration (Tailwind V4)
- **Typography & Font**:
    - **Family**: `Inter` (Primary).
    - **Sizes**: `xs` (12px) for badge/caption, `sm` (13px) for Table/List (mật độ cao), `base` (14px) for card body/form, `lg` (16px) for sub-headers, `xl` (18px) for titles.
    - **Weights**: `400` (Regular), `500` (Medium for data), `600` (Semibold for headers), `700` (Bold).
- **Colors (Elite / Slate / Emerald concept)**:
    - **Page Background**: Light `#F5F8FA` / Dark `#151521`.
    - **Surface/Card**: Light `#FFFFFF` / Dark `#1E1E2D`.
    - **Primary**: `#009EF7` (Classic) hoặc `#10B981` (Emerald).
    - **Semantic Colors**: Success `#50CD89`, Warning `#FFC700`, Danger `#F1416C`, Info `#7239EA`.
    - **Soft Colors**: Dùng bg 10-15% opacity của màu chính cho Notify, Alert, Badge (VD: `bg-green-50 text-success`).
- **Borders & Radii**:
    - **Color**: `#EFF2F5` (Light), `#2B2B40` (Dark).
    - **Radii**: `sm` (4px), `base` (7.6px), `lg` (12px), `xl` (16px - chuẩn cho khối Card chính).
- **Core Components**:
    - **Table**: Mật độ cao. Dòng cao 44px-48px. Nền trong/xám nhạt (`hover:bg-slate-50`). Header: `uppercase`, `font-semibold`, `text-xs`, `text-slate-400`. Chỉ dùng `border-b`, không dùng viền dọc.
    - **Card**: KHÔNG dùng shadow bẩn ở Light mode (dùng `box-shadow: 0px 0px 20px 0px rgba(76, 87, 125, 0.02)`). Viền mảnh 1px hoặc không viền. Nền trắng hoàn toàn. Padding body `p-6`.
    - **List**: Phân cách bằng `border-b border-dashed border-gray-200`. Layout flex, gap 16px hoặc 24px.
    - **Notify/Alert**: Phong cách "Soft colors", bo góc 7.6px (`rounded-lg`), padding `p-4`.
- **Master-Detail View (Kiến trúc xử lý Quản lý chi tiết)**:
    - **Slide-over / Drawer (Được ưu tiên tuyệt đối)**: Sử dụng ngăn trượt từ phải (Right Slide-over) đối với các hành động View/Edit Chi tiết để không làm gián đoạn ngữ cảnh của Table Data. Hạn chế dùng Modal.
    - **Header của Slide-over**: Dính liền (`sticky top-0`). Chứa Title (`text-lg font-semibold`), nút Close form chéo (`hover:text-gray-600`), có viền dưới `border-b border-gray-100` và đệm chuẩn `px-6 py-4`.
    - **Body Display Mode (Chế độ Xem)**: Style hiển thị theo cặp `Label - Value`. Label sử dụng `text-xs font-semibold text-gray-500 uppercase tracking-widest`. Value dùng `text-[13px] text-gray-800 font-medium`. Sử dụng Grid để phân tách các Data Fields.
    - **Footer Actions**: Ghim ở chân panel (`sticky bottom-0 bg-white border-t p-4 px-6`), đẩy các nút hành động (Save, Cancel, Delete...) về bên phải góc (`justify-end`). `Cancel` ưu tiên dùng giao diện light gray (`bg-gray-100 hover:bg-gray-200 text-gray-600`).
    - **UX Transitions**: BẮT BUỘC sử dụng hiệu ứng `Skeleton Loading` êm ái (`animate-pulse` trên các khối `bg-gray-200 rounded`) mô tả cấu trúc thật khi tải dữ liệu chi tiết, tuyệt đối KHÔNG dùng spinner loading xoay ở giữa màn hình.

---

## 6. Automation & Infrastructure (DevOps Role)
- **Containerization**: Use multi-stage **Dockerfiles** (.NET 8 SDK -> ASP.NET Runtime).
- **Orchestration**: `docker-compose.yaml` for local development (RabbitMQ, Redis, MongoDB).
- **CI/CD**: GitHub Actions for automated Build and Test.
- **Security**: 
    - **Rate Limiting**: 100 req/min/IP default.
    - **Headers**: HSTS, CSP, X-Frame-Options, X-Content-Type-Options.
    - **Secrets**: Use `.env` or Vault; never hardcode in source files.

---

## 7. Quality Assurance & Recovery
### QA Engineering
- **Testing Layers**: Unit (xUnit), API (Postman/Swagger), Load (k6), E2E (Playwright).
- **Assertions**: Focus on edge cases and vulnerabilities.

### Recovery Tracker (Vietnamese Context)
- **Mục đích**: Khôi phục bối cảnh sau sự cố (crash, token limit).
- **Process**: 
    1. Quét file `task.md` và `e2e_real_data_checklist.md`.
    2. Xác định điểm dừng cuối cùng (`[x]`).
    3. Thông báo trạng thái và kế hoạch tiếp theo (Next Action).
    4. Kế thừa Code/DB hiện tại, tuyệt đối không viết lại từ đầu.

---

## 8. AI Policy & MCP Server
- **MCP Server**: Clean Architecture. Expose tools via MediatR Commands.
- **Gateway Integration**: MCP tools routed via YARP with `X-Mcp-Key` validation.
- **Review Policy**: Block PRs if business logic is in Controllers or if tests are missing.

---
*Consolidated by Antigravity AI - TTL Engineering Source of Truth.*
