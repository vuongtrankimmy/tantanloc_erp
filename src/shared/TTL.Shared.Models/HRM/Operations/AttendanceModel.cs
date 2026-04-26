namespace TTL.Shared.Models.HRM.Operations
{
    public class AttendanceModel
    {
        public string Code { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string AvatarColor { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        
        public double StandardDays { get; set; }
        public double ActualDays { get; set; }
        public double LateHours { get; set; }
        public double EarlyLeaveHours { get; set; }
        public double PenaltyAmount { get; set; }
        public double HolidayDays { get; set; }
        public double TotalOTHours { get; set; }
        
        public string Status { get; set; } = "Đang làm việc"; // Đang làm việc, Thử việc
        public string StatusBadgeClass { get; set; } = "badge-light-success";
    }
}
