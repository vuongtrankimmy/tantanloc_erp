using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Operations;
using TTL.Shared.Models.HRM.Organization;

namespace TTL.HRM.Client.Pages.Operations.Payroll_benefits.PayrollDetail
{
    public partial class PayrollDetail
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        [Parameter]
        public string PayrollCode { get; set; } = "PR082026"; // Default parameter

        protected List<PayrollDetailModel> PayrollDetails { get; set; } = new();
        protected PayrollModel CurrentPayroll { get; set; } = new();

        protected string SearchQuery { get; set; } = string.Empty;
        protected string SelectedDepartment { get; set; } = "Tất cả";

        protected decimal TotalNetSalary => PayrollDetails.Sum(p => p.NetSalary);
        protected decimal TotalTax => 0; // Not detailed in mock, default to 0
        protected decimal TotalInsurance => 8671000; // Mocked aggregate 
        protected int TotalEmployees => PayrollDetails.Count;

        protected decimal TotalBasicSalary => PayrollDetails.Sum(p => p.BasicSalary);
        protected decimal TotalAllowance => PayrollDetails.Sum(p => p.Allowance);
        protected decimal TotalDeduction => PayrollDetails.Sum(p => p.Deduction);

        protected List<string> Departments => new List<string> 
        { 
            "Tất cả", "Ban Giám Đốc", "Phòng Nhân Sự (HR)", "Phòng Công Nghệ (IT)", "Phòng Tài Chính Kế Toán" 
        };

        protected IEnumerable<PayrollDetailModel> FilteredDetails =>
            PayrollDetails.Where(p =>
                (string.IsNullOrWhiteSpace(SearchQuery) || p.EmployeeName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || p.EmployeeCode.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                (SelectedDepartment == "Tất cả" || p.Department == SelectedDepartment)
            );

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Fetch the list of payrolls to get context for the header
                var payrolls = await Http.GetFromJsonAsync<List<PayrollModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/payrolls.json");
                if (payrolls != null)
                {
                    CurrentPayroll = payrolls.FirstOrDefault(p => p.Code == PayrollCode) ?? payrolls.FirstOrDefault() ?? new PayrollModel();
                }

                // Fetch details
                var data = await Http.GetFromJsonAsync<List<PayrollDetailModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/payroll_details.json");
                if (data != null)
                {
                    PayrollDetails = data.Where(d => d.PayrollCode == PayrollCode).ToList();
                    
                    if (!PayrollDetails.Any())
                    {
                        PayrollDetails = data;
                    }
                    SelectedDetail = PayrollDetails.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading payroll details data: {ex.Message}");
            }
        }

        protected PayrollDetailModel? SelectedDetail { get; set; }

        protected void SelectPayrollDetail(PayrollDetailModel detail)
        {
            SelectedDetail = detail;
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
