using System.Text.Json.Serialization;

namespace TTL.Shared.Models.HRM.Operations
{
    public class PayrollModel
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("payroll_name")]
        public string PayrollName { get; set; } = string.Empty;

        [JsonPropertyName("month_year")]
        public string MonthYear { get; set; } = string.Empty;

        [JsonPropertyName("employee_count")]
        public int EmployeeCount { get; set; }

        [JsonPropertyName("total_payment")]
        public decimal TotalPayment { get; set; }

        [JsonPropertyName("status_id")]
        public int StatusId { get; set; } // 1: Chưa khởi tạo, 2: Dự thảo, 3: Chờ duyệt, 4: Đã chốt

        [JsonPropertyName("status_name")]
        public string StatusName { get; set; } = string.Empty;
    }
}
