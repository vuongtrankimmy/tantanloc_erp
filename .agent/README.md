# Agent Team Testing System

## Tổng quan

Hệ thống Agent Team này được thiết kế để tự động test và kiểm tra toàn bộ hệ thống ERP TTL. Bao gồm 7 agents chuyên biệt, mỗi agent đảm nhiệm một lĩnh vực testing cụ thể.

## Cấu trúc Agent Team

```
.agent/
├── config.json                      # Cấu hình tổng thể
├── test-coordinator.agent.json      # Agent điều phối chính
├── backend-tester.agent.json        # Test Backend API
├── frontend-tester.agent.json       # Test Frontend UI/UX
├── database-tester.agent.json       # Test Database & Data Integrity
├── security-tester.agent.json       # Test Security & Vulnerabilities
├── performance-tester.agent.json    # Test Performance & Load
├── integration-tester.agent.json    # Test Integration
└── report-generator.agent.json      # Tạo báo cáo tổng hợp
```

## Các Agent và Nhiệm vụ

### 1. Test Coordinator (Điều phối viên)
- **Vai trò**: Agent chính điều khiển toàn bộ quá trình testing
- **Nhiệm vụ**:
  - Khởi động và điều phối các agent con
  - Quản lý workflow testing
  - Tổng hợp kết quả từ tất cả agents
  - Ưu tiên các vấn đề cần xử lý

### 2. Backend API Tester
- **Vai trò**: Test các API services backend
- **Phạm vi**:
  - Identity Service (Authentication/Authorization)
  - Core Service (Business Logic)
  - Gateway (API Gateway)
- **Test cases**:
  - API endpoint testing
  - Authentication/Authorization flows
  - CRUD operations
  - Error handling
  - Response validation

### 3. Frontend UI/UX Tester
- **Vai trò**: Test giao diện và trải nghiệm người dùng
- **Phạm vi**:
  - Blazor Web Application
  - HTML Components
- **Test cases**:
  - UI component functionality
  - User flows
  - Form validation
  - Responsive design
  - Accessibility (A11y)
  - Cross-browser compatibility

### 4. Database & Data Integrity Tester
- **Vai trò**: Test database và tính toàn vẹn dữ liệu
- **Phạm vi**:
  - MongoDB databases
  - Data relationships
  - Query performance
- **Test cases**:
  - Connection & availability
  - Data integrity checks
  - CRUD operations
  - Query performance
  - Index optimization
  - Data consistency

### 5. Security & Vulnerability Tester
- **Vai trò**: Test bảo mật và phát hiện lỗ hổng
- **Phạm vi**:
  - API endpoints
  - Web application
  - Authentication/Authorization
- **Test cases**:
  - Authentication security
  - Authorization testing
  - Injection attacks (SQL/NoSQL)
  - XSS protection
  - CSRF protection
  - Sensitive data exposure
  - Security headers
  - Dependency vulnerabilities

### 6. Performance & Load Tester
- **Vai trò**: Test hiệu năng và khả năng chịu tải
- **Phạm vi**:
  - API services
  - Web application
  - Database
- **Test scenarios**:
  - Baseline performance
  - Load testing (50 concurrent users)
  - Stress testing (200+ users)
  - Spike testing
  - Endurance testing (4 hours)

### 7. Integration Tester
- **Vai trò**: Test tích hợp giữa các module
- **Phạm vi**:
  - Service-to-service communication
  - Frontend-to-backend integration
  - Cross-module workflows
- **Test scenarios**:
  - End-to-end user flows
  - Service communication
  - Data consistency across services
  - Event-driven workflows
  - Error propagation

### 8. Report Generator
- **Vai trò**: Tổng hợp và tạo báo cáo
- **Outputs**:
  - Executive Summary (cho management)
  - Technical Report (cho developers)
  - Test Coverage Report
- **Formats**: Markdown, HTML, JSON

## Quy trình Testing

### Phase 1: Infrastructure & Database
```
database-tester → Kiểm tra MongoDB, data integrity
```

### Phase 2: Backend Services
```
backend-tester   → Test API endpoints
security-tester  → Test security vulnerabilities
```

### Phase 3: Frontend & Performance
```
frontend-tester      → Test UI/UX
performance-tester   → Load & stress testing
```

### Phase 4: Integration
```
integration-tester → End-to-end testing
```

### Phase 5: Reporting
```
report-generator → Tạo báo cáo tổng hợp
```

## Cách sử dụng

### 1. Cấu hình môi trường

Chỉnh sửa [config.json](.agent/config.json):

```json
{
  "environment": {
    "api_base_url": "http://localhost:5000",
    "identity_service_url": "http://localhost:5001",
    "core_service_url": "http://localhost:5002",
    "web_app_url": "http://localhost:5003",
    "mongodb_connection": "mongodb://localhost:27017"
  }
}
```

### 2. Khởi động hệ thống cần test

```bash
# Start API services
cd TTL_API/src/Gateway/TTL.Gateway
dotnet run

# Start Web application
cd TTL_ERP/TTL.HR/TTL.HR.Web
dotnet run

# MongoDB should be running
```

### 3. Chạy toàn bộ test suite

```bash
# Cách 1: Chạy coordinator (sẽ tự động chạy các agent con)
# (Implementation tùy thuộc vào framework bạn sử dụng)

# Cách 2: Chạy từng agent riêng lẻ
# (Để debug hoặc test cụ thể)
```

### 4. Xem kết quả

Báo cáo sẽ được tạo trong [docs/test-reports](../docs/test-reports):
- `test-report-{timestamp}.md` - Markdown report
- `test-report-{timestamp}.html` - HTML report
- `test-results-{timestamp}.json` - JSON data

## Cấu hình chi tiết

### Bật/Tắt agents

Trong [config.json](.agent/config.json):

```json
{
  "agents": [
    {
      "id": "backend-tester",
      "enabled": true  // false để tắt
    }
  ]
}
```

### Điều chỉnh thời gian timeout

```json
{
  "execution_strategy": {
    "timeout_minutes": 120,  // Tổng thời gian tối đa
    "max_retries": 2         // Số lần retry khi fail
  }
}
```

### Thay đổi chiến lược thực thi

```json
{
  "execution_strategy": {
    "mode": "parallel",          // "parallel" hoặc "sequential"
    "max_parallel_agents": 5,    // Số agents chạy đồng thời
    "continue_on_error": true    // Tiếp tục khi có lỗi
  }
}
```

## Thông số đánh giá

### Performance Thresholds

| Metric | Excellent | Good | Acceptable | Poor |
|--------|-----------|------|------------|------|
| API Response Time | <200ms | <500ms | <1000ms | >2000ms |
| Error Rate | <0.1% | <1% | <5% | >5% |
| CPU Usage | <70% | <85% | <95% | >95% |
| Memory Usage | <70% | <85% | <95% | >95% |

### Security Severity Levels

| Level | Description | SLA |
|-------|-------------|-----|
| **Critical** | System crash, data loss, unauthorized access | 4 hours |
| **High** | Major feature failure, high security risk | 24 hours |
| **Medium** | Minor issues, UI problems | 7 days |
| **Low** | Cosmetic issues, enhancements | 30 days |

## Báo cáo và Metrics

### Executive Summary
- Overall system health score
- Top 5 critical issues
- Test coverage percentage
- Key performance metrics
- Recommendations

### Technical Report
- Detailed test results by module
- Performance analysis
- Security findings
- Database health
- Integration issues
- Complete bug list

### Test Coverage Report
- Coverage by module
- Untested areas
- Test case distribution

## Best Practices

1. **Chạy full test suite trước mỗi release**
2. **Chạy smoke tests hàng ngày**
3. **Monitor performance trends theo thời gian**
4. **Prioritize critical và high severity issues**
5. **Review security findings định kỳ**
6. **Keep test data updated**
7. **Archive test reports cho audit**

## Troubleshooting

### Agent không start được
- Kiểm tra config.json syntax
- Kiểm tra dependencies (các agent phụ thuộc)
- Xem logs trong console

### Test fail liên tục
- Kiểm tra services đang chạy
- Kiểm tra database connection
- Kiểm tra network/firewall

### Performance tests không accurate
- Đảm bảo không có background processes
- Chạy trên environment tương tự production
- Warm-up system trước khi test

## Mở rộng

### Thêm agent mới

1. Tạo file `{name}.agent.json`
2. Define capabilities và test scenarios
3. Thêm vào `config.json`
4. Implement test logic

### Thêm test scenarios

Edit file agent tương ứng, thêm vào section `test_scenarios`:

```json
{
  "test_scenarios": [
    {
      "name": "New Test Scenario",
      "tests": [...]
    }
  ]
}
```

## Liên hệ và Support

- Xem [docs/](../docs/) để biết thêm chi tiết về hệ thống
- Check [build_errors_*.txt](../) files để xem các lỗi hiện tại
- Xem [CLEANUP_SUMMARY.md](../docs/CLEANUP_SUMMARY.md) cho tổng quan dự án

---

**Version**: 1.0.0
**Last Updated**: 2026-03-19
**Maintained by**: TTL Development Team
