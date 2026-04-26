using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Payroll_benefits.PayrollDetail
{
    public partial class EmployeePayrollDrawer
    {
        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        [Parameter]
        public PayrollDetailModel? PayrollDetail { get; set; }

        protected string ActiveTab { get; set; } = "overview";

        protected async Task SetActiveTab(string tab)
        {
            ActiveTab = tab;
            await InvokeAsync(StateHasChanged);
        }

        protected override void OnParametersSet()
        {
            if (PayrollDetail != null)
            {
                ActiveTab = "overview";
                GenerateMockAttendanceData();
            }
        }

        protected List<AttendanceRecordModel> AttendanceRecords { get; set; } = new();

        private void GenerateMockAttendanceData()
        {
            AttendanceRecords.Clear();
            var startDate = new DateTime(2026, 4, 1);
            for (int i = 0; i < 30; i++)
            {
                var currentDate = startDate.AddDays(i);
                var isWeekend = currentDate.DayOfWeek == DayOfWeek.Sunday;
                
                if (isWeekend)
                {
                    AttendanceRecords.Add(new AttendanceRecordModel
                    {
                        Date = currentDate,
                        CheckIn = "-",
                        CheckOut = "-",
                        WorkDays = 0,
                        Status = "Nghỉ tuần",
                        StatusColor = "secondary"
                    });
                }
                else
                {
                    bool isLate = i == 5 || i == 14;
                    bool isAbsent = i == 10;
                    
                    if (isAbsent)
                    {
                        AttendanceRecords.Add(new AttendanceRecordModel
                        {
                            Date = currentDate,
                            CheckIn = "-",
                            CheckOut = "-",
                            WorkDays = 0,
                            Status = "Vắng mặt",
                            StatusColor = "danger"
                        });
                    }
                    else
                    {
                        AttendanceRecords.Add(new AttendanceRecordModel
                        {
                            Date = currentDate,
                            CheckIn = isLate ? "08:45" : "07:55",
                            CheckOut = isLate ? "17:00" : "17:15",
                            WorkDays = isLate ? 0.5m : 1.0m,
                            Status = isLate ? "Đi muộn" : "Đúng giờ",
                            StatusColor = isLate ? "warning" : "success"
                        });
                    }
                }
            }
        }
    }

    public class AttendanceRecordModel
    {
        public DateTime Date { get; set; }
        public string CheckIn { get; set; } = string.Empty;
        public string CheckOut { get; set; } = string.Empty;
        public decimal WorkDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusColor { get; set; } = string.Empty;
    }
}
