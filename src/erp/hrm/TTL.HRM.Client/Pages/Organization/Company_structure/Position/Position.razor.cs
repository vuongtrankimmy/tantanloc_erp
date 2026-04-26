using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Organization;

namespace TTL.HRM.Client.Pages.Organization.Company_structure.Position
{
    public partial class Position
    {
        [Inject]
        public HttpClient Http { get; set; } = default!;

        [Inject]
        public IJSRuntime JS { get; set; } = default!;

        protected List<PositionData>? positions;
        private bool _dataLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                positions = await Http.GetFromJsonAsync<List<PositionData>>("_content/TTL.Shared.UI/mock-data/hrm/organization/positions.json");
                _dataLoaded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading mock data: {ex.Message}");
            }
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
