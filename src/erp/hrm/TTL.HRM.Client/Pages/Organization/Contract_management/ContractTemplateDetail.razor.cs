using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.Organization;

namespace TTL.HRM.Client.Pages.Organization.Contract_management
{
    public partial class ContractTemplateDetail : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public string? Id { get; set; }

        protected ContractTemplateModel Template { get; set; } = new();

        protected bool IsEditMode => !string.IsNullOrEmpty(Id);

        protected override async Task OnInitializedAsync()
        {
            if (IsEditMode)
            {
                var data = await Http.GetFromJsonAsync<List<ContractTemplateModel>>("_content/TTL.Shared.UI/mock-data/hrm/organization/contract_templates.json");
                if (data != null)
                {
                    Template = data.FirstOrDefault(t => t.id == Id) ?? new ContractTemplateModel();
                }
            }
            else
            {
                Template = new ContractTemplateModel
                {
                    status = "Đang sử dụng",
                    type = "Hợp đồng thử việc"
                };
            }
        }

        protected void GoBack()
        {
            NavigationManager.NavigateTo("hrm/contract");
        }

        protected void SaveChanges()
        {
            // Handle save logic
            NavigationManager.NavigateTo("hrm/contract");
        }
    }
}
