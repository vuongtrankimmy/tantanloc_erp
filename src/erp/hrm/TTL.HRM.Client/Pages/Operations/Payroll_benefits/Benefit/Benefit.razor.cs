using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TTL.HRM.Client.Pages.Operations.Payroll_benefits.Benefit
{
    public class BenefitData
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public string type { get; set; } = "";
        public decimal amount { get; set; }
        public string target { get; set; } = "";
        public string status { get; set; } = "";
    }

    public partial class Benefit : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = default!;

        [Inject]
        public IJSRuntime JS { get; set; } = default!;

        protected List<BenefitData>? benefits;
        public BenefitData? SelectedBenefit { get; set; }
        private bool _dataLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                benefits = await HttpClient.GetFromJsonAsync<List<BenefitData>>("_content/TTL.Shared.UI/mock-data/hrm/operations/benefits.json");
                _dataLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading benefits data: {ex.Message}");
            }
        }

        public void SelectBenefit(string id, string name, string description, string type, decimal amount, string target)
        {
            SelectedBenefit = new BenefitData
            {
                id = id,
                name = name,
                description = description,
                type = type,
                amount = amount,
                target = target,
                status = "Active"
            };
            StateHasChanged();
        }

        public void SaveBenefit(BenefitData data)
        {
            // Here you would typically save via API. For mock, we'll just close or update the list if it matches.
            SelectedBenefit = null;
            StateHasChanged();
            // Need to close drawer via JS or data-kt-drawer-dismiss
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_dataLoaded)
            {
                _dataLoaded = false;
                await Task.Delay(50);
                await JS.InvokeVoidAsync("KTComponents.init");
                await JS.InvokeVoidAsync("KTMenu.createInstances");
            }
        }
    }
}
