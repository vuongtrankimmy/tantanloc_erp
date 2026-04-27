using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TTL.HRM.Client.Pages.User
{
    public partial class Profile
    {
        [Parameter]
        public string? Id { get; set; }

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        public string ActiveTab { get; set; } = "Overview";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(50);
                await JS.InvokeVoidAsync("KTComponents.init");
                await JS.InvokeVoidAsync("KTMenu.createInstances");
            }
        }

        public void SetTab(string tab)
        {
            ActiveTab = tab;
            StateHasChanged();
        }
    }
}
