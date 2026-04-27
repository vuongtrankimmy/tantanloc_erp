using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace TTL.HRM.Client.Pages.Operations.Work_shifts
{
    public enum ViewMode
    {
        Day,
        Week,
        Month
    }

    public class DayInfo
    {
        public string Title { get; set; } = string.Empty;
        public string DateString { get; set; } = string.Empty;
        public bool IsWeekend { get; set; }
    }

    public partial class ShiftSchedule : ComponentBase
    {
        public List<ShiftSummary> ShiftSummaries { get; set; } = new();
        public List<EmployeeShift> EmployeeShifts { get; set; } = new();
        public List<string> TimeSlots { get; set; } = new();
        public ViewMode CurrentViewMode { get; set; } = ViewMode.Day;
        public List<DayInfo> WeekDays { get; set; } = new();
        public List<DayInfo> MonthDays { get; set; } = new();

        public string CurrentDateText => CurrentViewMode switch
        {
            ViewMode.Day => "01/04/2026",
            ViewMode.Week => "30/03 - 05/04/2026",
            ViewMode.Month => "04 / 2026",
            _ => ""
        };

        public EmployeeShift? SelectedEmployeeShift { get; set; }
        public bool IsOffcanvasOpen { get; set; }
        public ShiftSummary? SelectedShiftDetails => ShiftSummaries.FirstOrDefault(s => s.Code == SelectedEmployeeShift?.ShiftCode);

        public void SetViewMode(ViewMode mode)
        {
            CurrentViewMode = mode;
        }

        public void OpenShiftDetails(EmployeeShift emp)
        {
            SelectedEmployeeShift = emp;
            IsOffcanvasOpen = true;
        }

        public void CloseShiftDetails()
        {
            IsOffcanvasOpen = false;
            SelectedEmployeeShift = null;
        }

        protected override void OnInitialized()
        {
            // Initialize time slots from 6:00 to 23:00
            for (int i = 6; i <= 23; i++)
            {
                TimeSlots.Add($"{i}:00");
                //TimeSlots.Add($"{i}:30"); // Let's just do hourly columns for simplicity
            }

            // Initialize Week Days
            WeekDays = new List<DayInfo>
            {
                new DayInfo { Title = "THỨ HAI", DateString = "30" },
                new DayInfo { Title = "THỨ BA", DateString = "31" },
                new DayInfo { Title = "THỨ TƯ", DateString = "01" },
                new DayInfo { Title = "THỨ NĂM", DateString = "02" },
                new DayInfo { Title = "THỨ SÁU", DateString = "03" },
                new DayInfo { Title = "THỨ BẢY", DateString = "04" },
                new DayInfo { Title = "CHỦ NHẬT", DateString = "05", IsWeekend = true }
            };

            // Initialize Month Days
            for (int i = 1; i <= 31; i++)
            {
                bool isWeekend = (i == 6 || i == 7 || i == 13 || i == 14 || i == 20 || i == 21 || i == 27 || i == 28);
                MonthDays.Add(new DayInfo { DateString = i.ToString(), IsWeekend = isWeekend });
            }

            // Mock Data for Shifts
            ShiftSummaries = new List<ShiftSummary>
            {
                new ShiftSummary { Code = "HC", Name = "CA HÀNH CHÍNH THỨ 2 - THỨ 6", TimeRange = "08:30 - 17:30", AssignedCount = 12, ColorClass = "primary", IsActive = true },
                new ShiftSummary { Code = "CC", Name = "CA CHIỀU (KHÔ BẮC)", TimeRange = "13:30 - 21:30", AssignedCount = 0, ColorClass = "warning", IsActive = false },
                new ShiftSummary { Code = "CS", Name = "CA SÁNG (KHÔ BẮC)", TimeRange = "06:00 - 14:00", AssignedCount = 0, ColorClass = "success", IsActive = false },
                new ShiftSummary { Code = "B7", Name = "CA BẢY SÁNG", TimeRange = "06:00 - 12:00", AssignedCount = 4, ColorClass = "danger", IsActive = false },
                new ShiftSummary { Code = "PT", Name = "CA PART-TIME", TimeRange = "Linh hoạt", AssignedCount = 15, ColorClass = "info", IsActive = false }
            };

            // Mock Data for Employees
            EmployeeShifts = new List<EmployeeShift>
            {
                new EmployeeShift { EmployeeCode = "NV-01", EmployeeName = "Phạm Minh Khang", ShiftCode = "HC", StartTime = 8.5, EndTime = 17.5, ColorClass = "primary" },
                new EmployeeShift { EmployeeCode = "NV-02", EmployeeName = "Lê Văn Tuấn", ShiftCode = "HC", StartTime = 8.5, EndTime = 17.5, ColorClass = "primary" },
                new EmployeeShift { EmployeeCode = "NV-03", EmployeeName = "Hoàng Thu Phương", ShiftCode = "HC", StartTime = 8.5, EndTime = 17.5, ColorClass = "primary" },
                new EmployeeShift { EmployeeCode = "NV-04", EmployeeName = "Trần Tuấn Đạt", ShiftCode = "HC", StartTime = 8.5, EndTime = 17.5, ColorClass = "primary" },
                new EmployeeShift { EmployeeCode = "NV-05", EmployeeName = "Nguyễn Thanh Bình", ShiftCode = "HC", StartTime = 8.5, EndTime = 17.5, ColorClass = "primary" }
            };
        }

        public class ShiftSummary
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string TimeRange { get; set; } = string.Empty;
            public int AssignedCount { get; set; }
            public string ColorClass { get; set; } = string.Empty;
            public bool IsActive { get; set; }
        }

        public class EmployeeShift
        {
            public string EmployeeCode { get; set; } = string.Empty;
            public string EmployeeName { get; set; } = string.Empty;
            public string ShiftCode { get; set; } = string.Empty;
            public double StartTime { get; set; }
            public double EndTime { get; set; }
            public string ColorClass { get; set; } = string.Empty;
            
            // Calculate width based on total hours from 6:00 to 24:00 (18 hours)
            public string GetLeftPercentage()
            {
                // Start of timeline is 6:00. End is 24:00. Total = 18 hours.
                double offset = StartTime - 6.0;
                return $"{(offset / 18.0) * 100}%";
            }

            public string GetWidthPercentage()
            {
                double duration = EndTime - StartTime;
                return $"{(duration / 18.0) * 100}%";
            }
        }
    }
}
