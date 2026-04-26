using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.System_admin;

namespace TTL.HRM.Client.Pages.System_admin.Settings.Permission
{
    public partial class PermissionList : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        protected List<RoleModel> Roles { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var data = await Http.GetFromJsonAsync<List<RoleModel>>("_content/TTL.Shared.UI/mock-data/hrm/system_admin/roles.json");
            if (data != null)
            {
                Roles = data;
            }
        }
        
        protected string GetInitial(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "A";
            var parts = name.Trim().Split(' ');
            var lastName = parts.LastOrDefault() ?? "A";
            return lastName.Substring(0, 1).ToUpper();
        }
        
        protected string GetAvatarClass(string name)
        {
            var initial = GetInitial(name);
            if (initial == "A" || initial == "B" || initial == "C" || initial == "E") return "bg-light-primary text-primary";
            if (initial == "N" || initial == "M" || initial == "L" || initial == "G") return "bg-light-info text-info";
            if (initial == "T" || initial == "S" || initial == "R" || initial == "D") return "bg-light-danger text-danger";
            return "bg-light-warning text-warning";
        }
        
        protected (string icon, string color) GetRoleIcon(string code)
        {
            if (code == "FULL ACCESS") return ("ki-shield-tick", "primary");
            if (code == "STOCK CONTROL") return ("ki-shop", "info");
            if (code == "FINANCE") return ("ki-bill", "warning");
            return ("ki-shield", "primary");
        }
    }
}
