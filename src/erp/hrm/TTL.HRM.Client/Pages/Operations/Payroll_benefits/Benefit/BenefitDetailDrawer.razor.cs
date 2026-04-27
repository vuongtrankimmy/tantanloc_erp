using Microsoft.AspNetCore.Components;

namespace TTL.HRM.Client.Pages.Operations.Payroll_benefits.Benefit
{
    public partial class BenefitDetailDrawer : ComponentBase
    {
        [Parameter]
        public BenefitData? BenefitItem { get; set; }
        
        [Parameter]
        public EventCallback<BenefitData> OnSave { get; set; }

        public void Save()
        {
            if (BenefitItem != null)
            {
                OnSave.InvokeAsync(BenefitItem);
            }
        }
    }
}
