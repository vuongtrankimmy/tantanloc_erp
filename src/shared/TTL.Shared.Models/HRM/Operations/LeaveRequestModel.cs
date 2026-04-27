using System;

namespace TTL.Shared.Models.HRM.Operations
{
    public class LeaveRequestModel
    {
        public string id { get; set; } = string.Empty;
        public string employee_id { get; set; } = string.Empty;
        public string employee_name { get; set; } = string.Empty;
        public string department { get; set; } = string.Empty;
        public string position { get; set; } = string.Empty;
        public string request_category { get; set; } = "Nghỉ phép"; // Nghỉ phép, Công tác
        public string leave_type { get; set; } = string.Empty;
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public decimal total_days { get; set; }
        public decimal remaining_leave_days { get; set; }
        public string reason { get; set; } = string.Empty;
        public string status { get; set; } = "Chờ duyệt"; // Chờ duyệt, Đã duyệt, Từ chối
        public string created_by { get; set; } = string.Empty;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
