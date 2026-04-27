using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Operations;
using TTL.Shared.Models.HRM.Organization;

namespace TTL.HRM.Client.Pages.Operations.Work_shifts
{
    public partial class ShiftAssignment
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        [Parameter]
        public string ShiftId { get; set; } = string.Empty;

        protected ShiftModel? Shift { get; set; }

        protected DateTime AllocationFromDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        protected DateTime AllocationToDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

        protected List<int> SelectedAllocationDays { get; set; } = new();

        protected string ActiveTab { get; set; } = "Department"; // "Department" or "Employee"

        protected List<string> Departments { get; set; } = new()
        {
            "Ban Giám Đốc",
            "Phòng Hành chính Nhân sự",
            "Phòng Công nghệ (IT)",
            "Phòng Tài chính Kế toán",
            "Phòng Kinh doanh (Sales)",
            "Phòng Marketing",
            "Kho Vận (Logistics)",
            "Phòng Mua Hàng",
            "Phòng R&D",
            "Tổ Bảo vệ"
        };
        protected HashSet<string> SelectedDepartments { get; set; } = new();

        protected List<EmployeeModel> Employees { get; set; } = new();
        protected HashSet<EmployeeModel> SelectedEmployees { get; set; } = new();
        protected string SearchEmployeeQuery { get; set; } = string.Empty;

        protected IEnumerable<EmployeeModel> FilteredEmployees => 
            string.IsNullOrWhiteSpace(SearchEmployeeQuery) 
                ? Employees 
                : Employees.Where(e => e.FullName.Contains(SearchEmployeeQuery, StringComparison.OrdinalIgnoreCase) || e.Code.Contains(SearchEmployeeQuery, StringComparison.OrdinalIgnoreCase));

        protected Dictionary<string, int> DaysOfWeek = new()
        {
            { "Thứ 2", 2 },
            { "Thứ 3", 3 },
            { "Thứ 4", 4 },
            { "Thứ 5", 5 },
            { "Thứ 6", 6 },
            { "Thứ 7", 7 },
            { "Chủ nhật", 1 }
        };

        protected List<ShiftStatusModel> ShiftStatuses { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            // Load shift details
            var shifts = await Http.GetFromJsonAsync<List<ShiftModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/shifts.json");
            if (shifts != null)
            {
                Shift = shifts.FirstOrDefault(s => s.Code == ShiftId) ?? shifts.FirstOrDefault();
                if (Shift != null)
                {
                    // Default to shift's applied days
                    SelectedAllocationDays = new List<int>(Shift.AppliedDays);
                }
            }

            // Load statuses
            var statuses = await Http.GetFromJsonAsync<List<ShiftStatusModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/shift_statuses.json");
            if (statuses != null)
            {
                ShiftStatuses = statuses;
            }

            // Load employees
            var emps = await Http.GetFromJsonAsync<List<EmployeeModel>>("_content/TTL.Shared.UI/mock-data/hrm/organization/employees.json");
            if (emps != null)
            {
                Employees = emps;
            }
        }

        protected void ToggleAllocationDay(int dayValue, object? isCheckedObj)
        {
            if (isCheckedObj is bool isChecked)
            {
                if (isChecked && !SelectedAllocationDays.Contains(dayValue))
                {
                    SelectedAllocationDays.Add(dayValue);
                }
                else if (!isChecked && SelectedAllocationDays.Contains(dayValue))
                {
                    SelectedAllocationDays.Remove(dayValue);
                }
            }
        }

        protected void SelectAllDays()
        {
            SelectedAllocationDays = DaysOfWeek.Values.ToList();
        }

        protected void ClearAllDays()
        {
            SelectedAllocationDays.Clear();
        }

        protected void ToggleDepartment(string dept)
        {
            if (SelectedDepartments.Contains(dept))
            {
                SelectedDepartments.Remove(dept);
            }
            else
            {
                SelectedDepartments.Add(dept);
            }
        }

        protected void ToggleEmployee(EmployeeModel emp)
        {
            if (SelectedEmployees.Contains(emp))
            {
                SelectedEmployees.Remove(emp);
            }
            else
            {
                SelectedEmployees.Add(emp);
            }
        }

        protected string GetRandomColor(string input)
        {
            var colors = new[] { "primary", "success", "info", "warning", "danger" };
            int index = Math.Abs(input.GetHashCode()) % colors.Length;
            return colors[index];
        }

        protected string GetStatusText(int statusId) 
        {
            var status = ShiftStatuses.FirstOrDefault(s => s.StatusId == statusId);
            return status?.StatusName ?? "Không xác định";
        }

        protected async Task SaveAllocation()
        {
            // Simulate saving
            await JS.InvokeVoidAsync("Swal.fire", "Thành công!", "Đã phân bổ ca làm việc thành công.", "success");
        }
    }
}
