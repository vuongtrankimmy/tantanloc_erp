namespace TTL.WMS.Client.Pages.Suppliers
{
    public class SupplierCreateModel
    {
        public string TaxCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = "NCC-2023-0852";
        public bool SuppliesPaper { get; set; }
        public bool SuppliesOther { get; set; }

        public string ContactPerson { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string OfficeAddress { get; set; } = string.Empty;

        public string WarehouseName { get; set; } = string.Empty;
        public string WarehouseManager { get; set; } = string.Empty;
        public string WarehouseAddress { get; set; } = string.Empty;
        
        public string Notes { get; set; } = string.Empty;
    }

    public partial class SupplierCreate
    {
        public SupplierCreateModel Model { get; set; } = new();

        protected void OnSubmit()
        {
            // Submit logic
        }

        protected void OnCancel()
        {
            // Navigation logic or reset
        }
    }
}
