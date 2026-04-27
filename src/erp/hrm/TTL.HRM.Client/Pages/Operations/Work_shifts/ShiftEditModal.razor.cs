using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Work_shifts
{
    public partial class ShiftEditModal
    {
        [Parameter]
        public ShiftModel Model { get; set; } = new();

        [Parameter]
        public EventCallback<ShiftModel> OnSave { get; set; }

        private List<string> Colors = new() { "primary", "success", "info", "warning", "danger", "dark" };

        private Dictionary<string, int> DaysOfWeek = new()
        {
            { "T2", 2 },
            { "T3", 3 },
            { "T4", 4 },
            { "T5", 5 },
            { "T6", 6 },
            { "T7", 7 },
            { "CN", 1 }
        };

        private void ToggleDay(int dayValue, object? isCheckedObj)
        {
            if (isCheckedObj is bool isChecked)
            {
                if (isChecked && !Model.AppliedDays.Contains(dayValue))
                {
                    Model.AppliedDays.Add(dayValue);
                }
                else if (!isChecked && Model.AppliedDays.Contains(dayValue))
                {
                    Model.AppliedDays.Remove(dayValue);
                }
            }
        }

        private async Task Save()
        {
            // Add basic validation logic if needed
            if (OnSave.HasDelegate)
            {
                await OnSave.InvokeAsync(Model);
            }
        }
    }
}
