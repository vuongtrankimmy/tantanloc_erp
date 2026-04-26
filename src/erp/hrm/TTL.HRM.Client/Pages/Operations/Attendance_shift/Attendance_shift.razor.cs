using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Attendance_shift
{
    public partial class Attendance_shift
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        private List<AttendanceModel> Attendances = new();
        private AttendanceModel? SelectedAttendance { get; set; }

        private void SelectAttendance(AttendanceModel attendance)
        {
            SelectedAttendance = attendance;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var data = await Http.GetFromJsonAsync<List<AttendanceModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/attendances.json");
                if (data != null)
                {
                    Attendances = data;
                    SelectedAttendance = Attendances.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading attendances data: {ex.Message}");
            }
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
    }
}
