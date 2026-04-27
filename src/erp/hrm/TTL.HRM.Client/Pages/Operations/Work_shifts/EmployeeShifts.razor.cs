using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TTL.HRM.Client.Pages.Operations.Work_shifts
{
    public partial class EmployeeShifts : ComponentBase
    {
        [Inject]
        public IJSRuntime JS { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(100);
                await JS.InvokeVoidAsync("KTComponents.init");
                await JS.InvokeVoidAsync("KTMenu.createInstances");
            }
        }
    }
}
