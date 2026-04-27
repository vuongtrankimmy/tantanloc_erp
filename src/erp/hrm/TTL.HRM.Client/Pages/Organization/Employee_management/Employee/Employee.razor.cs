using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Organization;

namespace TTL.HRM.Client.Pages.Organization.Employee_management.Employee
{
    public partial class Employee
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        public enum ViewMode 
        {
            Table,
            Card
        }

        private List<EmployeeModel> Employees { get; set; } = new();
        private EmployeeModel? SelectedEmployee { get; set; }
        private ViewMode ActiveView { get; set; } = ViewMode.Table;

        private void SetActiveView(ViewMode view)
        {
            ActiveView = view;
        }

        private string GetActiveCssClass(ViewMode mode) => ActiveView == mode ? "active" : string.Empty;
        private string GetShowActiveCssClass(ViewMode mode) => ActiveView == mode ? "show active" : string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Employees = await Http.GetFromJsonAsync<List<EmployeeModel>>("_content/TTL.Shared.UI/mock-data/hrm/organization/employees.json") ?? new();
                SelectedEmployee = Employees.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading employee data: {ex.Message}");
            }
        }

        private void SelectEmployee(EmployeeModel emp)
        {
            SelectedEmployee = emp;
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
