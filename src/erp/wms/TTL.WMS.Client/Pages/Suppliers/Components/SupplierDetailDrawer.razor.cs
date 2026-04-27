using Microsoft.AspNetCore.Components;

namespace TTL.WMS.Client.Pages.Suppliers.Components
{
    public partial class SupplierDetailDrawer : ComponentBase
    {
        [Parameter] public string DrawerId { get; set; } = "kt_supplier_detail_drawer";

        // Mocks for internal state
        public bool IsVisible { get; set; }
        
        // Mocks for different material selections
        public List<string> MaterialTypes { get; set; } = new() { "Paper", "Chemical" }; // Options: "Paper", "Chemical", "Other"

        public void Show()
        {
            IsVisible = true;
            StateHasChanged();
        }

        public void Hide()
        {
            IsVisible = false;
            StateHasChanged();
        }
    }
}
