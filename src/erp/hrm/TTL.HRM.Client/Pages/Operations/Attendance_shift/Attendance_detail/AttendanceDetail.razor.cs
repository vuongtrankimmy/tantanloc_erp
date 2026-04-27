using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Attendance_shift.Attendance_detail
{
    public partial class AttendanceDetail
    {
        [Parameter]
        public string EmployeeCode { get; set; } = default!;

        [Inject]
        protected System.Net.Http.HttpClient Http { get; set; } = default!;

        public AttendanceModel? Attendance { get; set; }
        private List<DailyLogModel> DailyLogs { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                // Load attendance summary
                var attendances = await Http.GetFromJsonAsync<List<AttendanceModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/attendances.json");
                if (attendances != null)
                {
                    Attendance = attendances.FirstOrDefault(a => a.EmployeeCode == EmployeeCode) ?? attendances.FirstOrDefault();
                }

                // Load daily logs
                var logs = await Http.GetFromJsonAsync<List<DailyLogModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/daily_logs.json");
                if (logs != null)
                {
                    DailyLogs = logs;
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Error loading attendance data: {ex.Message}");
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
