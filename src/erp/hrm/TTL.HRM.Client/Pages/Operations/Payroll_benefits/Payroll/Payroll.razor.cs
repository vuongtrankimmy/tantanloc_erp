using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Operations;

namespace TTL.HRM.Client.Pages.Operations.Payroll_benefits.Payroll
{
    public partial class Payroll
    {
        [Inject]
        protected HttpClient Http { get; set; } = default!;

        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        protected List<PayrollModel> Payrolls { get; set; } = new();

        protected string SearchQuery { get; set; } = string.Empty;
        protected string SelectedYear { get; set; } = "2026";
        protected string SelectedMonth { get; set; } = "Tất cả các tháng";

        protected decimal TotalPaymentSum { get; set; } = 325324750;
        protected decimal TotalTaxSum { get; set; } = 7062250;
        protected decimal TotalInsuranceSum { get; set; } = 26013000;
        protected int TotalEmployeeCount { get; set; } = 64;

        protected IEnumerable<PayrollModel> FilteredPayrolls =>
            Payrolls.Where(p =>
                (string.IsNullOrWhiteSpace(SearchQuery) || p.PayrollName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                (SelectedMonth == "Tất cả các tháng" || p.MonthYear.StartsWith(SelectedMonth)) &&
                p.MonthYear.EndsWith(SelectedYear)
            );

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var data = await Http.GetFromJsonAsync<List<PayrollModel>>("_content/TTL.Shared.UI/mock-data/hrm/operations/payrolls.json");
                if (data != null)
                {
                    Payrolls = data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading payrolls data: {ex.Message}");
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

        protected string GetStatusBadgeClass(int statusId)
        {
            return statusId switch
            {
                1 => "badge-light text-muted border-gray-300", // Chưa khởi tạo
                2 => "badge-light-primary border-primary", // Dự thảo
                3 => "badge-light-warning border-warning", // Chờ duyệt
                4 => "badge-light-info border-info", // Đã chốt (màu tím/xanh ngọc theo ảnh template, dùng info tạm thời hoặc dark/purple nếu có)
                _ => "badge-light"
            };
        }
        
        protected string GetStatusIconClass(int statusId)
        {
            return statusId switch
            {
                1 => "ki-information text-muted",
                2 => "ki-document text-primary",
                3 => "ki-time text-warning",
                4 => "ki-check-circle text-info",
                _ => "ki-question text-muted"
            };
        }
        
        protected string GetStatusBadgeColor(int statusId)
        {
            return statusId switch
            {
                1 => "secondary", 
                2 => "primary",
                3 => "warning", 
                4 => "info", 
                _ => "secondary"
            };
        }

        protected void CalculateMonthPayroll()
        {
            // Trigger calculation for the current month
        }
    }
}
