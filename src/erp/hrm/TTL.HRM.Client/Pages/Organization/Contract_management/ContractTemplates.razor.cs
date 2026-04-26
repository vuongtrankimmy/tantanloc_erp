using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.Organization;

namespace TTL.HRM.Client.Pages.Organization.Contract_management
{
    public partial class ContractTemplates : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        protected List<ContractTemplateModel> Templates { get; set; } = new();
        protected string ActiveTab { get; set; } = "all";

        protected override async Task OnInitializedAsync()
        {
            var data = await Http.GetFromJsonAsync<List<ContractTemplateModel>>("_content/TTL.Shared.UI/mock-data/hrm/organization/contract_templates.json");
            if (data != null)
            {
                Templates = data;
            }
        }

        protected IEnumerable<ContractTemplateModel> GetFilteredTemplates()
        {
            if (ActiveTab == "active")
                return Templates.Where(t => t.status == "Đang sử dụng");
            if (ActiveTab == "draft")
                return Templates.Where(t => t.status == "Nháp");
            return Templates;
        }

        protected void SetTab(string tab)
        {
            ActiveTab = tab;
        }

        protected string GetStatusBadgeClass(string status)
        {
            return status == "Đang sử dụng" ? "badge-light-success text-success" : "badge-light text-gray-700";
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
            if (initial == "A" || initial == "B" || initial == "C") return "bg-light-primary text-primary";
            if (initial == "N" || initial == "M" || initial == "L") return "bg-light-info text-info";
            if (initial == "T" || initial == "S" || initial == "R") return "bg-light-danger text-danger";
            return "bg-light-warning text-warning";
        }
    }
}
