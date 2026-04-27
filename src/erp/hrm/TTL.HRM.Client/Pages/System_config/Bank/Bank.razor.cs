using Microsoft.AspNetCore.Components;
using TTL.Shared.Models.HRM.System_admin;

namespace TTL.HRM.Client.Pages.System_config.Bank
{
    public partial class Bank : ComponentBase
    {
        private List<BankModel>? banks;

        protected override void OnInitialized()
        {
            banks = new List<BankModel>
            {
                new BankModel
                {
                    Code = "970436",
                    ShortName = "Vietcombank",
                    FullName = "Ngân hàng TMCP Ngoại thương Việt Nam",
                    SwiftCode = "BFTVVNVX",
                    StatusId = 1,
                    Status = "Hoạt động"
                },
                new BankModel
                {
                    Code = "970415",
                    ShortName = "VietinBank",
                    FullName = "Ngân hàng TMCP Công thương Việt Nam",
                    SwiftCode = "ICBVVNX",
                    StatusId = 1,
                    Status = "Hoạt động"
                },
                new BankModel
                {
                    Code = "970418",
                    ShortName = "BIDV",
                    FullName = "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam",
                    SwiftCode = "BIDVVNVX",
                    StatusId = 1,
                    Status = "Hoạt động"
                },
                new BankModel
                {
                    Code = "970405",
                    ShortName = "Agribank",
                    FullName = "Ngân hàng NN & PTNT Việt Nam",
                    SwiftCode = "VBAAVNVX",
                    StatusId = 1,
                    Status = "Hoạt động"
                },
                new BankModel
                {
                    Code = "970422",
                    ShortName = "MB",
                    FullName = "Ngân hàng TMCP Quân đội",
                    SwiftCode = "MSCIVNVX",
                    StatusId = 1,
                    Status = "Hoạt động"
                },
                new BankModel
                {
                    Code = "970403",
                    ShortName = "Sacombank",
                    FullName = "Ngân hàng TMCP Sài Gòn Thương Tín",
                    SwiftCode = "SGTCVNVX",
                    StatusId = 0,
                    Status = "Ngưng hoạt động"
                }
            };
        }
    }
}
