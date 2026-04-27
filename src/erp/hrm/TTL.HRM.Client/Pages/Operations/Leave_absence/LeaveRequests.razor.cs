using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Leave_absence
{
    public partial class LeaveRequests : ComponentBase
    {
        public List<LeaveRequestModel> Requests { get; set; } = new();
        public LeaveRequestModel? SelectedRequest { get; set; }
        
        public enum ViewMode { Card, Table }
        public ViewMode ActiveView { get; set; } = ViewMode.Table;

        protected override void OnInitialized()
        {
            // Initialize with mock data
            Requests = new List<LeaveRequestModel>
            {
                new LeaveRequestModel
                {
                    id = "REQ-001",
                    employee_id = "EMP-001",
                    employee_name = "Nguyễn Văn A",
                    department = "Phòng Kỹ thuật",
                    position = "Chuyên viên thiết kế",
                    request_category = "Nghỉ phép",
                    leave_type = "Nghỉ phép năm",
                    start_date = new DateTime(2024, 10, 12),
                    end_date = new DateTime(2024, 10, 14),
                    total_days = 2,
                    remaining_leave_days = 5,
                    reason = "Giải quyết việc gia đình",
                    status = "Chờ phê duyệt",
                    created_at = DateTime.Today.AddDays(-1)
                },
                new LeaveRequestModel
                {
                    id = "REQ-002",
                    employee_id = "EMP-005",
                    employee_name = "Trần Thị B",
                    department = "Phòng Marketing",
                    position = "Nhân viên Marketing",
                    request_category = "Công tác",
                    leave_type = "Công tác",
                    start_date = new DateTime(2024, 10, 15),
                    end_date = new DateTime(2024, 10, 18),
                    total_days = 4,
                    remaining_leave_days = 12,
                    reason = "Hội thảo đối tác chiến lược",
                    status = "Chờ phê duyệt",
                    created_at = DateTime.Today.AddDays(-6)
                },
                new LeaveRequestModel
                {
                    id = "REQ-003",
                    employee_id = "EMP-012",
                    employee_name = "Lê Văn C",
                    department = "Phòng Tổ chức",
                    position = "Trưởng phòng",
                    request_category = "Nghỉ phép",
                    leave_type = "Nghỉ ốm",
                    start_date = new DateTime(2024, 10, 10),
                    end_date = new DateTime(2024, 10, 10),
                    total_days = 1,
                    remaining_leave_days = 10,
                    reason = "Đi khám sức khỏe định kỳ",
                    status = "Đã phê duyệt",
                    created_at = DateTime.Today.AddDays(-2)
                },
                new LeaveRequestModel
                {
                    id = "REQ-004",
                    employee_id = "EMP-015",
                    employee_name = "Phạm Minh D",
                    department = "Phòng Kinh doanh",
                    position = "Nhân viên Sales",
                    request_category = "Công tác",
                    leave_type = "Công tác",
                    start_date = new DateTime(2024, 10, 19),
                    end_date = new DateTime(2024, 10, 20),
                    total_days = 2,
                    remaining_leave_days = 15,
                    reason = "Hỗ trợ triển khai dự án mới",
                    status = "Chờ phê duyệt",
                    created_at = DateTime.Today.AddDays(-2)
                }
            };
        }

        public void SetActiveView(ViewMode mode)
        {
            ActiveView = mode;
        }

        public string GetActiveCssClass(ViewMode mode) => ActiveView == mode ? "active" : "";
        public string GetShowActiveCssClass(ViewMode mode) => ActiveView == mode ? "show active" : "";

        public void SelectRequest(LeaveRequestModel req)
        {
            SelectedRequest = req;
            StateHasChanged();
        }

        public string GetStatusBadgeClass(string status)
        {
            return status switch
            {
                "Chờ phê duyệt" => "badge-light-warning",
                "Đã phê duyệt" => "badge-light-success",
                "Từ chối" => "badge-light-danger",
                _ => "badge-light-primary"
            };
        }

        [Inject]
        protected Microsoft.JSInterop.IJSRuntime JS { get; set; } = default!;

        protected override async System.Threading.Tasks.Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await System.Threading.Tasks.Task.Delay(50);
                await JS.InvokeVoidAsync("KTComponents.init");
                await JS.InvokeVoidAsync("KTMenu.createInstances");
            }
        }
    }
}
