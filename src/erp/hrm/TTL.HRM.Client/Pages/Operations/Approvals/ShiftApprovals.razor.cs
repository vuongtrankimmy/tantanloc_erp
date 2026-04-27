using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TTL.HRM.Client.Pages.Operations.Approvals
{
    public class ShiftApprovalRequest
    {
        public string id { get; set; } = "";
        public string employeeName { get; set; } = "";
        public string employeeId { get; set; } = "";
        public string department { get; set; } = "";
        public string avatarColor { get; set; } = "primary";
        public string currentShift { get; set; } = "";
        public string currentShiftColor { get; set; } = "primary";
        public string requestedShift { get; set; } = "";
        public string requestedShiftColor { get; set; } = "warning";
        public string reason { get; set; } = "";
        public string applyDate { get; set; } = "";
        public string submitDate { get; set; } = "24/10/2023 08:30";
        public string oldShiftDate { get; set; } = "Thứ 2, 25/10/2023";
        public string oldShiftTime { get; set; } = "08:00 - 17:00";
        public string newShiftDate { get; set; } = "Thứ 4, 27/10/2023";
        public string newShiftTime { get; set; } = "14:00 - 22:00";
        public string status { get; set; } = "pending";
    }

    public partial class ShiftApprovals : ComponentBase
    {
        [Inject]
        public IJSRuntime JS { get; set; } = default!;

        public List<ShiftApprovalRequest> Requests { get; set; } = new List<ShiftApprovalRequest>();
        public string CurrentTab { get; set; } = "pending";
        public ShiftApprovalRequest? SelectedRequest { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(100);
                await JS.InvokeVoidAsync("KTComponents.init");
                await JS.InvokeVoidAsync("KTMenu.createInstances");
            }
        }

        protected override void OnInitialized()
        {
            // Mock data based on the screenshot
            Requests = new List<ShiftApprovalRequest>
            {
                new ShiftApprovalRequest
                {
                    id = "REQ-001",
                    employeeName = "Nguyễn Văn An",
                    employeeId = "NV-0284",
                    department = "KT",
                    avatarColor = "info",
                    currentShift = "CA SÁNG",
                    currentShiftColor = "primary",
                    requestedShift = "CA ĐÊM",
                    requestedShiftColor = "warning",
                    reason = "Việc gia đình đột xuất, cần t...",
                    applyDate = "12/10/2023",
                    status = "pending"
                },
                new ShiftApprovalRequest
                {
                    id = "REQ-002",
                    employeeName = "Lê Thị Bình",
                    employeeId = "NV-0912",
                    department = "HR",
                    avatarColor = "success",
                    currentShift = "HÀNH CHÍNH",
                    currentShiftColor = "secondary",
                    requestedShift = "CA SÁNG",
                    requestedShiftColor = "primary",
                    reason = "Đi học thêm chứng chỉ tiếng ...",
                    applyDate = "15/10/2023",
                    status = "pending"
                },
                new ShiftApprovalRequest
                {
                    id = "REQ-003",
                    employeeName = "Phạm Văn Cường",
                    employeeId = "NV-1044",
                    department = "KD",
                    avatarColor = "danger",
                    currentShift = "CA ĐÊM",
                    currentShiftColor = "warning",
                    requestedShift = "CA SÁNG",
                    requestedShiftColor = "primary",
                    reason = "Đổi ca với anh An để giải qu...",
                    applyDate = "12/10/2023",
                    status = "pending"
                },
                new ShiftApprovalRequest
                {
                    id = "REQ-004",
                    employeeName = "Hoàng Thu Dung",
                    employeeId = "NV-0411",
                    department = "MKT",
                    avatarColor = "primary",
                    currentShift = "CA CHIỀU",
                    currentShiftColor = "primary",
                    requestedShift = "HÀNH CHÍNH",
                    requestedShiftColor = "secondary",
                    reason = "Chuyển sang làm hành chính...",
                    applyDate = "18/10/2023",
                    status = "pending"
                }
            };
        }

        public void SetTab(string tab)
        {
            CurrentTab = tab;
            StateHasChanged();
        }

        public void ShowDetail(ShiftApprovalRequest req)
        {
            SelectedRequest = req;
            StateHasChanged();
        }

        public void ApproveRequest(string id)
        {
            var req = Requests.FirstOrDefault(r => r.id == id);
            if (req != null)
            {
                req.status = "approved";
                StateHasChanged();
            }
        }

        public void RejectRequest(string id)
        {
            var req = Requests.FirstOrDefault(r => r.id == id);
            if (req != null)
            {
                req.status = "rejected";
                StateHasChanged();
            }
        }
    }
}
