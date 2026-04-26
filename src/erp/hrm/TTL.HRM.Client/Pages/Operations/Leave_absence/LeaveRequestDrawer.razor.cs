using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Leave_absence
{
    public partial class LeaveRequestDrawer : ComponentBase
    {
        [Parameter]
        public LeaveRequestModel? Request { get; set; }

        public string GetStatusBadgeClass(string status)
        {
            return status switch
            {
                "Chờ duyệt" => "badge-light-warning",
                "Đã duyệt" => "badge-light-success",
                "Từ chối" => "badge-light-danger",
                _ => "badge-light-primary"
            };
        }
    }
}
