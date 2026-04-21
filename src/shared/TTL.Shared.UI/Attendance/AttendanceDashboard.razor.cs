using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTL.Shared.Services.Attendance;

namespace TTL.Shared.UI.Attendance
{
    public partial class AttendanceDashboard : ComponentBase, IDisposable
    {
        [Inject]
        public IAttendanceService AttendanceService { get; set; } = default!;

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        private List<AttendanceRecord> _scans = new();
        private DateTime _currentTime = SecureTimeProvider.Instance.Now;
        private bool _isProcessing = false;
        private bool _checkInMode = true;
        private bool? _lastScanSuccess = null;
        private string _lastEmployeeName = "";
        private string _lastRawCodeScanned = "";
        private System.Threading.Timer? _timer;
        private DotNetObjectReference<AttendanceDashboard>? _objRef;
        private bool _isFocused = true;

        private bool _isScannerConnected = false;
        private bool _isDbConnected = false;
        private bool _isNetworkActive = false;
        private long _networkLatencyMs = 0;
        private string _networkStatusMsg = "TESTING NETWORK...";

        private int _hardwareCheckCounter = 0;
        private string _scannerCheckStatus = "CHECKING SCANNER...";

        // Cấu hình Modal & Giao diện
        private bool _showSettings = false;
        private bool _isDarkTheme = false;
        private string _primaryColor = "#ff6f00"; 
        private string _memoryInfo = "0 MB";
        private string _diskInfo = "0 GB";

        private bool _disposed = false;
        
        // Timer tracking để đóng giao diện
        private int _scanUiSessionId = 0;
        
        // Cache chống trùng lặp dữ liệu (Debouncer)
        private Dictionary<string, DateTime> _lastScannedCodes = new();

        protected override void OnInitialized()
        {
            AttendanceService.OnDataChanged += HandleDataChanged;
            RefreshScans();
            
            _timer = new System.Threading.Timer(_ => 
            {
                InvokeAsync(() => {
                    _currentTime = SecureTimeProvider.Instance.Now;

                    if (_showExitConfirm)
                    {
                        if (_exitCountdown > 0)
                        {
                            _exitCountdown--;
                            if (_exitCountdown <= 0)
                            {
                                CloseApp();
                            }
                        }
                    }
                    
                    StateHasChanged();
                });
            }, null, 0, 1000);

            // Chạy luồng kiểm tra ngầm (Background Task) độc lập - Chống đứng giật UI và Đẩy nhanh tốc độ nhận diện
            _ = Task.Run(async () => {
                while (!_disposed)
                {
                    try
                    {
                        bool oldDb = _isDbConnected;
                        bool oldNet = _isNetworkActive;
                        bool oldScanner = _isScannerConnected;
                        string oldScannerMsg = _scannerCheckStatus;

                        _isDbConnected = CheckDatabaseConnection();
                        
                        var netCheck = await CheckNetworkConnectionAsync();
                        _isNetworkActive = netCheck.IsActive;
                        _networkLatencyMs = netCheck.Latency;

                        if (_isNetworkActive)
                        {
                            if (_networkLatencyMs <= 80) _networkStatusMsg = $"ONLINE ({_networkLatencyMs}ms - EXCELLENT)";
                            else if (_networkLatencyMs <= 250) _networkStatusMsg = $"ONLINE ({_networkLatencyMs}ms - FAIR)";
                            else _networkStatusMsg = $"ONLINE ({_networkLatencyMs}ms - UNSTABLE)";
                        }
                        else
                        {
                            _networkStatusMsg = "DISCONNECTED";
                        }

                        _isScannerConnected = await VerifyScannerHardwareAsync(true);
                        
                        if (_showSettings) 
                        {
                            UpdateSystemMetrics();
                        }

                        // Chỉ đâm ngắt giao diện nếu có sự thay đổi (Tối ưu hóa re-rendering)
                        if (oldDb != _isDbConnected || oldNet != _isNetworkActive || oldScanner != _isScannerConnected || oldScannerMsg != _scannerCheckStatus)
                        {
                            await InvokeAsync(StateHasChanged);
                        }
                    }
                    catch { }

                    await Task.Delay(2000); // Quét lại siêu tốc mỗi 2 giây
                }
            });
        }

        private bool CheckDatabaseConnection()
        {
            try
            {
                // Kiểm tra bằng cách thử read data
                AttendanceService.GetLastScans();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<(bool IsActive, long Latency)> CheckNetworkConnectionAsync()
        {
            try
            {
                // Bước 1: Xem trên hệ điều hành xem có bất kỳ Card mạng (Wifi/Lan) nào cắm cáp hay không
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    return (false, 0);

                // Bước 2: Đo độ trễ (Ping) đến trạm mạng toàn cầu (Google DNS) để kiểm chứng Internet & độ ổn định
                using var ping = new System.Net.NetworkInformation.Ping();
                var reply = await ping.SendPingAsync("8.8.8.8", 1500); // timeout 1500ms
                
                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return (true, reply.RoundtripTime);
                }
                return (false, 0);
            }
            catch
            {
                return (false, 0);
            }
        }

        private async Task<bool> VerifyScannerHardwareAsync(bool skipDelay = false)
        {
            if (!skipDelay) await Task.Delay(500); 

            try
            {
#pragma warning disable CA1416
                if (!OperatingSystem.IsWindows())
                {
                    _scannerCheckStatus = "OS NOT SUPPORTED";
                    return false;
                }

                _scannerCheckStatus = "SCANNING...";
                bool isScannerFound = false;
                using var searcher = new System.Management.ManagementObjectSearcher(@"Select * From Win32_PnPEntity");
                
                foreach (var device in searcher.Get())
                {
                    var name = device.GetPropertyValue("Name")?.ToString()?.ToLower() ?? "";
                    var desc = device.GetPropertyValue("Description")?.ToString()?.ToLower() ?? "";
                    var pnpClass = device.GetPropertyValue("PNPClass")?.ToString()?.ToUpper() ?? "";
                    var deviceId = device.GetPropertyValue("DeviceID")?.ToString()?.ToUpper() ?? "";

                    if (deviceId.StartsWith("ACPI") || deviceId.StartsWith("ROOT") || deviceId.StartsWith("PCI")) continue;

                    // 1. Nhận diện theo tên Hãng hoặc Model POS cụ thể
                    if (name.Contains("barcode") || desc.Contains("barcode") ||
                        name.Contains("qr scanner") || name.Contains("qr reader") || 
                        name.Contains("datalogic") || name.Contains("zebra") || 
                        name.Contains("honeywell") || name.Contains("symbol") || 
                        name.Contains("symcode") || name.Contains("netum") || 
                        name.Contains("eyoyo") || name.Contains("scanner") ||
                        name.Contains("ds4308") || name.Contains("ds9208") ||
                        name.Contains("1452g") || name.Contains("1900gsr") || name.Contains("1900g"))
                    {
                        isScannerFound = true;
                        _scannerCheckStatus = $"ON: {name.ToUpper()}";
                        break;
                    }

                    // 2. Nhận diện Scanner cài đặt dưới dạng Serial COM
                    if (pnpClass == "PORTS" && (deviceId.Contains("USB\\VID") || name.Contains("serial") || name.Contains("com")))
                    {
                        isScannerFound = true;
                        _scannerCheckStatus = $"ON: COM PORT SCANNER";
                        break;
                    }

                    // 3. Nhận diện theo VID phần cứng (Hardware ID) của dòng cao cấp
                    // Zebra/Symbol (05E0, 1D5B, 080C, 060E)
                    if (deviceId.Contains("VID_05E0") || deviceId.Contains("VID_0C2E") || 
                        deviceId.Contains("VID_0536") || deviceId.Contains("VID_05F9") || 
                        deviceId.Contains("VID_1EAB") || deviceId.Contains("VID_2DD6") || 
                        deviceId.Contains("VID_1D5B") || deviceId.Contains("VID_080C") ||
                        deviceId.Contains("VID_0483") || deviceId.Contains("VID_11CA") ||
                        deviceId.Contains("VID_16EE") || deviceId.Contains("VID_060E"))
                    {
                        isScannerFound = true;
                        // Phân tích tên nếu bị Windows gán thành USB Input Device
                        string brandText = (deviceId.Contains("05E0") || deviceId.Contains("060E")) ? "ZEBRA/SYMBOL" : 
                                          (deviceId.Contains("0C2E") || deviceId.Contains("16EE")) ? "HONEYWELL" : "POS SCANNER";
                                          
                        _scannerCheckStatus = $"ON: {brandText} WEDGE [{deviceId.Split('\\').LastOrDefault()}]";
                        break;
                    }
                }
                
                if (!isScannerFound)
                {
                    _scannerCheckStatus = "CẦN GẮN HOẶC KẾT NỐI ĐẦU QUÉT (ZEBRA/HONEYWELL)";
                }
                return isScannerFound;
#pragma warning restore CA1416
            }
            catch (Exception ex) 
            { 
                _scannerCheckStatus = "WMI ERR: " + ex.GetType().Name; 
            }
            
            return false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _objRef = DotNetObjectReference.Create(this);
                // Fire and forget or handle securely
                _ = JSRuntime.InvokeVoidAsync("attendanceScanner.init", _objRef).AsTask();
            }
        }

        private void RefreshScans()
        {
            _scans = AttendanceService.GetLastScans().OrderByDescending(r => r.ScanTime).Take(10).ToList();
        }

        private void HandleDataChanged()
        {
            InvokeAsync(() => {
                RefreshScans();
                StateHasChanged();
            });
        }

        private void SetCheckInMode(bool isCheckIn)
        {
            _checkInMode = isCheckIn;
            StateHasChanged();
        }

        private bool _showHistory = false; // NEW STATE VARIABLE

        private void ToggleSettings()
        {
            _showSettings = !_showSettings;
            if (_showSettings)
            {
                UpdateSystemMetrics();
            }
        }
        
        private void ToggleHistory()
        {
            _showHistory = !_showHistory;
            StateHasChanged();
        }

        private void SetTheme(bool isDark) { _isDarkTheme = isDark; }
        
        private void SetPrimaryColor(string color) { _primaryColor = color; }

        private void UpdateSystemMetrics()
        {
            try
            {
                var proc = System.Diagnostics.Process.GetCurrentProcess();
                _memoryInfo = $"{(proc.WorkingSet64 / 1048576.0):F1} MB";

                var dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ttl_attendance.db");
                if (System.IO.File.Exists(dbPath))
                {
                    var fileInfo = new System.IO.FileInfo(dbPath);
                    // Hiển thị KB hoặc MB tuỳ theo dung lượng lớn nhỏ
                    if (fileInfo.Length > 1048576) {
                        _diskInfo = $"{(fileInfo.Length / 1048576.0):F2} MB";
                    } else {
                        _diskInfo = $"{(fileInfo.Length / 1024.0):F1} KB";
                    }
                }
                else
                {
                    _diskInfo = "DB Not Found";
                }
            }
            catch { }
        }

        [JSInvokable]
        public async Task HandleScannerInput(string code)
        {
            if (_isProcessing) return;
            
            // XỬ LÝ CHỐNG QUÉT ĐÚP (DEBOUNCE): Ngăn người dùng quẹt liên tục 1 mã thẻ trong 15 giây
            var now = SecureTimeProvider.Instance.Now;
            if (_lastScannedCodes.TryGetValue(code, out var lastTime))
            {
                if ((now - lastTime).TotalSeconds < 15)
                {
                    return; // Lờ đi tín hiệu này vì vừa quét xong
                }
            }
            _lastScannedCodes[code] = now;
            
            _isProcessing = true;
            _scanUiSessionId++; // Đánh dấu ID phiên quét hiện tại
            var currentSessionId = _scanUiSessionId;
            
            _lastScanSuccess = null;
            _lastRawCodeScanned = code;
            StateHasChanged();
            
            await Task.Delay(300); // Simulate processing time
            
            var success = await AttendanceService.ProcessScanAsync(code, _checkInMode);
            
            // Lấy lại danh sách ngay lập tức để tránh lỗi bất đồng bộ với sự kiện OnDataChanged
            if (success) {
                RefreshScans();
                _lastEmployeeName = _scans.FirstOrDefault()?.EmployeeName ?? "Unknown";
            }
            
            _lastScanSuccess = success;
            _isProcessing = false;
            StateHasChanged();
            
            _ = Task.Delay(3500).ContinueWith(_ => {
                // Chỉ dọn dẹp màn hình nếu chưa có người nhân viên nào khác xen ngang
                if (_scanUiSessionId == currentSessionId)
                {
                    _lastScanSuccess = null;
                    _lastRawCodeScanned = "";
                    InvokeAsync(StateHasChanged);
                }
            });
        }

        [JSInvokable]
        public void UpdateFocusStatus(bool isFocused)
        {
            _isFocused = isFocused;
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _disposed = true;
            try {
                _ = JSRuntime.InvokeVoidAsync("attendanceScanner.dispose").AsTask();
            } catch {}
            
            if (AttendanceService != null)
            {
                AttendanceService.OnDataChanged -= HandleDataChanged;
            }
            _timer?.Dispose();
            _objRef?.Dispose();
        }
        
        private bool _showExitConfirm = false;
        private int _exitCountdown = 30;

        private void PromptCloseApp()
        {
            _showExitConfirm = true;
            _exitCountdown = 30;
            StateHasChanged();
        }

        private void CancelCloseApp()
        {
            _showExitConfirm = false;
            StateHasChanged();
        }

        private void CloseApp()
        {
            Environment.Exit(0);
        }
    }
}
