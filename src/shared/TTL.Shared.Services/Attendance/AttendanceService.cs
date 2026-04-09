using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTL.Shared.Services.Attendance
{
    public interface IAttendanceService
    {
        Task<bool> ProcessScanAsync(string code);
        event Action OnDataChanged;
        List<AttendanceRecord> GetLastScans();
    }

    public class AttendanceRecord
    {
        public string EmployeeName { get; set; } = "Nhân viên m?i";
        public string EmployeeId { get; set; }
        public DateTime ScanTime { get; set; }
        public string Status { get; set; } = "VÀO";
        public string AvatarUrl { get; set; } = "https://ui-avatars.com/api/?name=NV&background=28a745&color=fff";
    }

    public class MockAttendanceService : IAttendanceService
    {
        private List<AttendanceRecord> _scans = new();
        public event Action OnDataChanged;
        private readonly string[] _names = { "Nguy?n Van A", "Tr?n Th? B", "Lê Van C" };

        public async Task<bool> ProcessScanAsync(string code)
        {
            await Task.Delay(100);
            var name = _names[new Random().Next(_names.Length)];
            _scans.Insert(0, new AttendanceRecord { EmployeeId = code, EmployeeName = name, ScanTime = DateTime.Now, AvatarUrl = "https://ui-avatars.com/api/?name=" + name });
            if (_scans.Count > 10) _scans.RemoveAt(10);
            OnDataChanged?.Invoke();
            return true;
        }

        public List<AttendanceRecord> GetLastScans() => _scans;
    }
}
