namespace TTL.Shared.Models.HRM.Organization
{
    public class EmployeeModel
    {
        public string Code { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string DepartmentBadgeClass { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = string.Empty;
        public string AvatarColor { get; set; } = "0D8ABC";
        
        public string CitizenId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BasicSalary { get; set; } = string.Empty;
        public string ContractType { get; set; } = string.Empty;
        public string JoinDate { get; set; } = string.Empty;
        public bool IsActiveAccount { get; set; } = true;
        public string RoleId { get; set; } = "1";

        // Financial & Bank Info
        public string BankName { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string SocialInsuranceNumber { get; set; } = string.Empty;

        // Emergency Contact
        public List<EmergencyContactModel> EmergencyContacts { get; set; } = new List<EmergencyContactModel> 
        {
            new EmergencyContactModel { IsPrimary = true } // Khởi tạo sẵn 1 dòng chính
        };
    }

    public class EmergencyContactModel
    {
        public string Name { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
    }
}
