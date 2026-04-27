using System;

namespace TTL.Shared.Models.HRM.Organization
{
    public class PositionData
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string job_group { get; set; } = "";
        public string level { get; set; } = "";
        public int headcount { get; set; }
        public string salary_range { get; set; } = "";
        public int requests { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
