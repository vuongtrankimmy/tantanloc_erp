using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Work_shifts
{
    public partial class ShiftDetail
    {
        [Parameter]
        public string Code { get; set; } = string.Empty;

        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        public ShiftModel? Shift { get; set; }
        public List<EmployeeShiftMock> AssignedEmployees { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var shiftsData = await Http.GetFromJsonAsync<List<ShiftModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/shifts.json");
                if (shiftsData != null)
                {
                    Shift = shiftsData.FirstOrDefault(s => s.Code == Code);
                }

                // Mock some assigned employees
                AssignedEmployees = new List<EmployeeShiftMock>
                {
                    new EmployeeShiftMock { EmployeeCode = "NV001", EmployeeName = "Trần Kim Mỹ", Position = "Giám đốc nhân sự", Department = "Phòng Hành chính Nhân sự", AvatarColor = "primary" },
                    new EmployeeShiftMock { EmployeeCode = "NV005", EmployeeName = "Lê Hoàng Tú", Position = "Trưởng phòng IT", Department = "Phòng Công nghệ", AvatarColor = "success" },
                    new EmployeeShiftMock { EmployeeCode = "NV012", EmployeeName = "Nguyễn Văn A", Position = "Chuyên viên MKT", Department = "Phòng Marketing", AvatarColor = "info" }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading shift detail: {ex.Message}");
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(50);
                await JS.InvokeVoidAsync("KTComponents.init");
                await JS.InvokeVoidAsync("KTMenu.createInstances");
            }
        }

        public string GetDaysText(List<int> days)
        {
            if (days == null || days.Count == 0) return "Chưa thiết lập";
            var dayNames = new Dictionary<int, string>
            {
                {1, "CN"}, {2, "T2"}, {3, "T3"}, {4, "T4"}, {5, "T5"}, {6, "T6"}, {7, "T7"}
            };
            return string.Join(", ", days.Select(d => dayNames.ContainsKey(d) ? dayNames[d] : d.ToString()));
        }
    }

    public class EmployeeShiftMock
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string AvatarColor { get; set; } = "primary";
    }
}
