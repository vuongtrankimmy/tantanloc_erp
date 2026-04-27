$file = "d:\MONEY\2026\TanTanLoc\version_300\tantanloc_erp\src\erp\hrm\TTL.HRM.Client\Pages\Organization\Company_structure\Department\Department.razor"
$content = Get-Content -Path $file -Raw

$newBody = @"
                <tbody>
                    @if (departments == null)
                    {
                        <tr>
                            <td colspan="6" class="text-center">Đang tải dữ liệu...</td>
                        </tr>
                    }
                    else if (!departments.Any())
                    {
                        <tr>
                            <td colspan="6" class="text-center">Không có dữ liệu.</td>
                        </tr>
                    }
                    else
                    {
                        foreach (var dept in departments)
                        {
                            <tr>
                                <td>
                                    <div class="d-flex align-items-center">
                                        <div class="symbol symbol-45px me-4">
                                            <span class="symbol-label bg-light-primary">
                                                <i class="ki-outline ki-briefcase fs-3 text-primary"></i>
                                            </span>
                                        </div>
                                        <div class="d-flex justify-content-start flex-column">
                                            <span class="text-dark fw-bolder text-hover-primary fs-6 mb-1">@dept.name</span>
                                            <span class="text-muted fw-bold fs-7">@((dept.name == "Ban Giám đốc") ? "Cơ quan quản lý cấp cao nhất" : "Phòng ban chức năng")</span>
                                        </div>
                                    </div>
                                </td>
                                <td class="text-center">
                                    <span class="badge badge-light fw-bolder px-3 py-2 border">@dept.code</span>
                                </td>
                                <td class="text-center">
                                    <span class="fw-bold ms-3">@dept.manager</span>
                                </td>
                                <td class="text-end">
                                    <div class="d-flex flex-column">
                                        <div class="d-flex align-items-center mb-1">
                                            <span class="text-gray-800 fw-bolder fs-7 me-1">@dept.headcount</span>
                                            <span class="text-muted fs-8">/ 20 vị trí</span>
                                        </div>
                                        <div class="progress h-4px w-100 bg-light-primary">
                                            <div class="progress-bar bg-primary" role="progressbar" style="width: @(dept.headcount * 5)%"></div>
                                        </div>
                                    </div>
                                </td>
                                <td class="text-end pe-0">
                                    <div class="badge badge-light-primary">Hoạt động</div>
                                </td>
                                <td class="text-end">
                                    <button type="button" class="btn btn-clean btn-sm btn-icon btn-icon-primary btn-active-light-primary me-n3 show menu-dropdown" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end">
                                        <i class="ki-duotone ki-element-plus fs-3 text-primary">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                            <span class="path3"></span>
                                            <span class="path4"></span>
                                            <span class="path5"></span>
                                        </i>
                                    </button>
                                </td>
                            </tr>
                        }
                    }
                </tbody>
"@

$content = $content -replace '(?s)                <tbody>.*?                </tbody>', $newBody
$content = $content -replace '@page "/hrm/department"', "@page `"/hrm/department`"`r`n@inject HttpClient Http"

$codeBlock = @"
</div>

@code {
    private List<DepartmentData>? departments;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            departments = await Http.GetFromJsonAsync<List<DepartmentData>>("_content/TTL.Shared.UI/mock-data/hrm/organization/departments.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading mock data: {ex.Message}");
        }
    }

    public class DepartmentData
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string code { get; set; } = "";
        public string manager { get; set; } = "";
        public int headcount { get; set; }
        public string status { get; set; } = "";
        public DateTime created_at { get; set; }
    }
}
"@

$content = $content -replace '</div>\s*$', $codeBlock
Set-Content -Path $file -Value $content -NoNewline
