using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Work_shifts
{
    public partial class Work_shifts
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        private List<ShiftModel> Shifts { get; set; } = new();
        private List<ShiftStatusModel> ShiftStatuses { get; set; } = new();
        private ShiftModel CurrentShift { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var shiftsData = await Http.GetFromJsonAsync<List<ShiftModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/shifts.json");
                if (shiftsData != null)
                {
                    Shifts = shiftsData;
                }

                var statuses = await Http.GetFromJsonAsync<List<ShiftStatusModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/shift_statuses.json");
                if (statuses != null)
                {
                    ShiftStatuses = statuses;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading shifts data: {ex.Message}");
            }
        }

        private void CreateNew()
        {
            CurrentShift = new ShiftModel { Color = "primary" }; // Reset form
        }

        private void EditShift(ShiftModel shift)
        {
            // Clone object to avoid two-way binding affecting the list immediately before save
            CurrentShift = new ShiftModel
            {
                Code = shift.Code,
                Name = shift.Name,
                ShiftCode = shift.ShiftCode,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                StatusId = shift.StatusId,
                Color = shift.Color,
                IsDefault = shift.IsDefault,
                IsFlexible = shift.IsFlexible,
                ValidFrom = shift.ValidFrom,
                ValidTo = shift.ValidTo,
                AppliedDays = new List<int>(shift.AppliedDays)
            };
        }

        private void SaveShift(ShiftModel shift)
        {
            if (string.IsNullOrEmpty(shift.Code))
            {
                // Adding new shift
                shift.Code = (Shifts.Count + 1).ToString();
                shift.StatusId = shift.IsFlexible ? 2 : 1;
                Shifts.Add(shift);
            }
            else
            {
                // Updating existing shift
                var existing = Shifts.FirstOrDefault(s => s.Code == shift.Code);
                if (existing != null)
                {
                    existing.Name = shift.Name;
                    existing.ShiftCode = shift.ShiftCode;
                    existing.StartTime = shift.StartTime;
                    existing.EndTime = shift.EndTime;
                    existing.Color = shift.Color;
                    existing.IsFlexible = shift.IsFlexible;
                    existing.StatusId = shift.IsFlexible ? 2 : 1;
                    existing.ValidFrom = shift.ValidFrom;
                    existing.ValidTo = shift.ValidTo;
                    existing.AppliedDays = shift.AppliedDays;
                }
            }
            
            // Notify state has changed since data source changed
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Delay(50);
                await JS.InvokeVoidAsync("KTComponents.init");
                await JS.InvokeVoidAsync("KTMenu.createInstances");
            }
        }

        protected int GetShiftCountByStatus(int statusId) => Shifts.Count(s => s.StatusId == statusId);
        
        protected string GetStatusText(int statusId) 
        {
            var status = ShiftStatuses.FirstOrDefault(s => s.StatusId == statusId);
            return status?.StatusName ?? "Không xác định";
        }
    }
}
