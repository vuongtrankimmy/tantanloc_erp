using System.Text.Json.Serialization;

namespace TTL.Shared.Models.HRM.Operations
{
    public class PayrollDetailModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("payroll_code")]
        public string PayrollCode { get; set; } = string.Empty; // e.g. "PR012026"

        [JsonPropertyName("employee_id")]
        public string EmployeeId { get; set; } = string.Empty;

        [JsonPropertyName("employee_code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [JsonPropertyName("employee_name")]
        public string EmployeeName { get; set; } = string.Empty;

        [JsonPropertyName("department")]
        public string Department { get; set; } = string.Empty;

        [JsonPropertyName("avatar_color")]
        public string AvatarColor { get; set; } = string.Empty;

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [JsonPropertyName("basic_salary")]
        public decimal BasicSalary { get; set; }

        [JsonPropertyName("allowance")]
        public decimal Allowance { get; set; }

        [JsonPropertyName("working_days")]
        public double WorkingDays { get; set; }

        [JsonPropertyName("deduction")]
        public decimal Deduction { get; set; }

        [JsonPropertyName("net_salary")]
        public decimal NetSalary { get; set; }

        [JsonPropertyName("status_id")]
        public int StatusId { get; set; } // e.g. 1: Đã duyệt, 2: Chưa duyệt

        [JsonPropertyName("status_name")]
        public string StatusName { get; set; } = string.Empty;
    }
}
