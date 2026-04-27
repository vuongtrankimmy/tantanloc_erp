using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace TTL.WMS.Client.Pages.Suppliers
{
    public class SupplierModel
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public partial class SupplierList : ComponentBase
    {
        public List<SupplierModel> Suppliers { get; set; } = new();

        protected override void OnInitialized()
        {
            Suppliers = new List<SupplierModel>
            {
                new SupplierModel
                {
                    Id = "1",
                    Code = "NCC-PAPER-001",
                    Name = "Công ty Giấy Bình Minh",
                    Address = "KCN Tân Bình, TP.HCM",
                    ContactPerson = "Nguyễn Văn An",
                    PhoneNumber = "0901 234 567",
                    Category = "Giấy cuộn",
                    Status = "Đang hợp tác"
                },
                new SupplierModel
                {
                    Id = "2",
                    Code = "NCC-GLUE-024",
                    Name = "Hóa chất Keo Việt Đức",
                    Address = "Quận 12, TP.HCM",
                    ContactPerson = "Trần Thị Bích",
                    PhoneNumber = "0912 999 888",
                    Category = "Keo",
                    Status = "Chờ duyệt HĐ"
                },
                new SupplierModel
                {
                    Id = "3",
                    Code = "NCC-INK-009",
                    Name = "Mực in Công nghiệp ABC",
                    Address = "KCN Sóng Thần, Bình Dương",
                    ContactPerson = "Lê Hoàng Nam",
                    PhoneNumber = "0933 111 222",
                    Category = "Mực in",
                    Status = "Tạm dừng"
                },
                new SupplierModel
                {
                    Id = "4",
                    Code = "NCC-PAPER-012",
                    Name = "Tổng công ty Giấy Việt Nam",
                    Address = "Ba Đình, Hà Nội",
                    ContactPerson = "Phạm Văn Đồng",
                    PhoneNumber = "024 3823 4567",
                    Category = "Giấy cuộn",
                    Status = "Đang hợp tác"
                },
                new SupplierModel
                {
                    Id = "5",
                    Code = "NCC-GLUE-005",
                    Name = "Keo dán Miền Nam",
                    Address = "Long Thành, Đồng Nai",
                    ContactPerson = "Vũ Thị Sen",
                    PhoneNumber = "0944 555 666",
                    Category = "Keo",
                    Status = "Ngừng hợp tác"
                }
            };
        }

        protected string GetCategoryColor(string category)
        {
            return category switch
            {
                "Giấy cuộn" => "primary",
                "Keo" => "warning",
                "Mực in" => "info",
                _ => "secondary"
            };
        }

        protected string GetStatusColor(string status)
        {
            return status switch
            {
                "Đang hợp tác" => "success",
                "Chờ duyệt HĐ" => "primary",
                "Tạm dừng" => "secondary",
                "Ngừng hợp tác" => "danger",
                _ => "dark"
            };
        }
    }
}
