namespace TTL.Shared.Models.HRM.Organization
{
    public class ContractTemplateModel
    {
        public string id { get; set; } = string.Empty;
        public string template_name { get; set; } = string.Empty;
        public string template_code { get; set; } = string.Empty;
        public DateTime created_date { get; set; }
        public string last_updated_by { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty; // Đang sử dụng, Nháp
        public string type { get; set; } = string.Empty; // Loại mẫu hợp đồng
        public string description { get; set; } = string.Empty; // Mô tả chi tiết
        public string content { get; set; } = string.Empty; // Nội dung
        public int usage_count { get; set; } // Số lượng sử dụng
    }
}
