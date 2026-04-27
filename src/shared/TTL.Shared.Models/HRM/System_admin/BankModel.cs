namespace TTL.Shared.Models.HRM.System_admin
{
    public class BankModel
    {
        public string Code { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string SwiftCode { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public int StatusId { get; set; } = 1;
        public string Status { get; set; } = "Hoạt động";
    }
}
