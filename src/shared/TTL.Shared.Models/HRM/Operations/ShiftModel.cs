using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TTL.Shared.Models.HRM.Operations
{
    public class ShiftModel
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("shift_code")]
        public string ShiftCode { get; set; } = string.Empty;

        [JsonPropertyName("start_time")]
        public string StartTime { get; set; } = string.Empty;

        [JsonPropertyName("end_time")]
        public string EndTime { get; set; } = string.Empty;

        [JsonPropertyName("status_id")]
        public int StatusId { get; set; } = 1; // 1: Cố định, 2: Linh hoạt

        [JsonPropertyName("color")]
        public string Color { get; set; } = string.Empty; // primary, success, warning, etc.

        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; } = false;
        
        // Extended fields for Edit Modal
        [JsonPropertyName("is_flexible")]
        public bool IsFlexible { get; set; } = false;

        [JsonPropertyName("valid_from")]
        public DateTime? ValidFrom { get; set; }

        [JsonPropertyName("valid_to")]
        public DateTime? ValidTo { get; set; }

        [JsonPropertyName("applied_days")]
        public List<int> AppliedDays { get; set; } = new List<int> { 2, 3, 4, 5, 6 }; // T2, T3, T4, T5, T6
        
        [JsonIgnore]
        public string DisplayWorkingHours => $"{StartTime} - {EndTime}";
    }
}
