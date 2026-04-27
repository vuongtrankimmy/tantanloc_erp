using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.Operations;
using System.Net.Http.Json;

namespace TTL.HRM.Client.Pages.Operations.Attendance_shift.Attendance_detail
{
    public partial class AttendanceDetailDrawer
    {
        [Parameter]
        public AttendanceModel? Attendance { get; set; }

        [Inject]
        protected System.Net.Http.HttpClient Http { get; set; } = default!;

        private List<DailyLogModel> DailyLogs { get; set; } = new();

        protected override async System.Threading.Tasks.Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Attendance != null)
            {
                await LoadDailyLogs();
            }
        }

        private async System.Threading.Tasks.Task LoadDailyLogs()
        {
            try
            {
                var logs = await Http.GetFromJsonAsync<List<DailyLogModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/daily_logs.json");
                if (logs != null)
                {
                    DailyLogs = logs;
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Error loading daily logs: {ex.Message}");
            }
        }

        protected string GetLateClass(int lateMinutes) => lateMinutes > 0 ? "text-danger fw-bold" : "text-muted";
        protected string GetLateText(int lateMinutes) => lateMinutes > 0 ? $"{lateMinutes} p" : "-";

        protected string GetEarlyClass(int earlyMinutes) => earlyMinutes > 0 ? "text-warning fw-bold" : "text-muted";
        protected string GetEarlyText(int earlyMinutes) => earlyMinutes > 0 ? $"{earlyMinutes} p" : "-";

        protected string GetOtClass(double otHours) => otHours > 0 ? "text-success fw-bold" : "text-muted";
        protected string GetOtText(double otHours) => otHours > 0 ? $"{otHours} h" : "-";
    }
}
