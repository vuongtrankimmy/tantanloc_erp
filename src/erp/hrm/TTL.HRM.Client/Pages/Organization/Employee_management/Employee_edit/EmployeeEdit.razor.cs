using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TTL.Shared.Models.HRM.Organization;

namespace TTL.HRM.Client.Pages.Organization.Employee_management.Employee_edit
{
    public partial class EmployeeEdit
    {
        [Inject]
        protected IJSRuntime JS { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public string Code { get; set; } = string.Empty;

        private DateTime? DateOfBirthDate
        {
            get
            {
                if (string.IsNullOrEmpty(Model.DateOfBirth)) return null;
                if (DateTime.TryParseExact(Model.DateOfBirth, new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" }, null, System.Globalization.DateTimeStyles.None, out var dt))
                    return dt;
                return null;
            }
            set
            {
                Model.DateOfBirth = value?.ToString("yyyy-MM-dd") ?? string.Empty;
            }
        }

        private EmployeeModel Model { get; set; } = new EmployeeModel
        {
            ContractType = "Hợp đồng thử việc",
            StatusId = 2 // Thử việc
        };

        private EmployeeModel ScannedData { get; set; } = new EmployeeModel
        {
            FullName = "NGUYỄN VĂN A",
            CitizenId = "034092001XXX",
            DateOfBirth = "1992-04-15",
            Gender = "Nam",
            Address = "Số 123 Đường Kim Mã, Phường Kim Mã, Quận Ba Đình, Hà Nội"
        };

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Code))
            {
                // Fetch employee from API later
                // Model = await Http.GetFromJsonAsync<EmployeeModel>($"api/employees/{Code}");
                Model = new EmployeeModel
                {
                    Code = Code,
                    FullName = "Nguyễn Văn A",
                    Email = "nva@company.com",
                    Department = "Kỹ thuật",
                    Position = "Kỹ sư phần mềm",
                    StatusId = 1,
                    ContractType = "Chính thức"
                };
            }
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("KTComponents.init");
            }
        }

        private void ApplyScannedData()
        {
            Model.FullName = ScannedData.FullName;
            Model.CitizenId = ScannedData.CitizenId;
            Model.DateOfBirth = ScannedData.DateOfBirth;
            Model.Gender = ScannedData.Gender;
            Model.Address = ScannedData.Address;
        }

        private void AddEmergencyContact()
        {
            Model.EmergencyContacts.Add(new EmergencyContactModel());
        }

        private void RemoveEmergencyContact(EmergencyContactModel contact)
        {
            if (Model.EmergencyContacts.Count > 1)
            {
                Model.EmergencyContacts.Remove(contact);
                // Reassign primary if the removed one was primary
                if (contact.IsPrimary && Model.EmergencyContacts.Any())
                {
                    Model.EmergencyContacts.First().IsPrimary = true;
                }
            }
        }

        private void TogglePrimaryContact(EmergencyContactModel contact, ChangeEventArgs e)
        {
            bool isChecked = (bool)(e.Value ?? false);
            
            if (isChecked)
            {
                // Bật ON: Tắt tất cả các liên hệ khác, chỉ cho phép 1 liên hệ chính
                foreach (var c in Model.EmergencyContacts)
                {
                    c.IsPrimary = false;
                }
                contact.IsPrimary = true;
            }
            else
            {
                // Tắt OFF: Cho phép tắt
                contact.IsPrimary = false;
            }
        }

        private void SaveEmployee()
        {
            // TODO: Call API to save
            NavigationManager.NavigateTo("/hrm/employee");
        }
    }
}
