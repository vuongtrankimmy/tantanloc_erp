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
        private System.Threading.Timer? _timer;
        private DotNetObjectReference<AttendanceDashboard>? _objRef;

        protected override void OnInitialized()
        {
            AttendanceService.OnDataChanged += HandleDataChanged;
            RefreshScans();
            
            _timer = new System.Threading.Timer(_ => 
            {
                InvokeAsync(() => {
                    _currentTime = SecureTimeProvider.Instance.Now;
                    StateHasChanged();
                });
            }, null, 0, 1000);
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

        [JSInvokable]
        public async Task HandleScannerInput(string code)
        {
            if (_isProcessing) return;
            _isProcessing = true;
            _lastScanSuccess = null;
            StateHasChanged();
            
            await Task.Delay(300); // Simulate processing time
            
            var success = await AttendanceService.ProcessScanAsync(code);
            _lastScanSuccess = success;
            if (success) {
                _lastEmployeeName = _scans.FirstOrDefault()?.EmployeeName ?? "Unknown";
            }
            
            _isProcessing = false;
            StateHasChanged();
            
            _ = Task.Delay(3000).ContinueWith(_ => {
                _lastScanSuccess = null;
                InvokeAsync(StateHasChanged);
            });
        }

        public void Dispose()
        {
            if (AttendanceService != null)
            {
                AttendanceService.OnDataChanged -= HandleDataChanged;
            }
            _timer?.Dispose();
            _objRef?.Dispose();
        }
        
        private void CloseApp()
        {
            Environment.Exit(0);
        }
    }
}
