namespace TTL.Shared.Models.HRM.System_admin
{
    public class RoleModel
    {
        public string id { get; set; } = string.Empty;
        public string role_name { get; set; } = string.Empty;
        public string role_code { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string status { get; set; } = "Hoạt động";
        public List<RoleMemberModel> members { get; set; } = new();
    }

    public class RoleMemberModel
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string avatar_url { get; set; } = string.Empty;
    }
}
