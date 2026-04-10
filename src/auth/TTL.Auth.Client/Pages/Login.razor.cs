using Microsoft.AspNetCore.Components;

namespace TTL.Auth.Client.Pages;

public partial class Login : ComponentBase
{
    [Inject]
    public NavigationManager NavManager { get; set; } = default!;

    private async Task HandleLogin()
    {
        // TODO: Implement actual authentication API call logic here
        await Task.Delay(500); // Simulate network latency
        
        // Redirect to HRM module after login
        NavManager.NavigateTo("/hrm");
    }
}
