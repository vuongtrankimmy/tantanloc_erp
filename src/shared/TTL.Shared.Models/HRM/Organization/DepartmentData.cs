using System;

namespace TTL.Shared.Models.HRM.Organization
{
    public class DepartmentData
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string code { get; set; } = "";
        public string manager { get; set; } = "";
        public int headcount { get; set; }
        public string status { get; set; } = "";
        public DateTime created_at { get; set; }
    }
}
