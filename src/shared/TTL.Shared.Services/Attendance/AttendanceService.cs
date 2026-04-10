using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TTL.Shared.Services.Attendance
{
    public interface IAttendanceService
    {
        Task<bool> ProcessScanAsync(string code);
        event Action? OnDataChanged;
        List<AttendanceRecord> GetLastScans();
    }

    public class Employee
    {
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string Department { get; set; } = "N/A";
        public string AvatarUrl { get; set; } = null!;
    }

    public class AttendanceRecord
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = "Nhân viên mới";
        public string EmployeeId { get; set; } = null!;
        public DateTime ScanTime { get; set; }
        public string Status { get; set; } = "VAO";
        public string AvatarUrl { get; set; } = "https://ui-avatars.com/api/?name=NV&background=28a745&color=fff";
    }

    public class AttendanceDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ttl_attendance.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
            });

            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Optional logical relationship without strict constraints for simpler migrations right now
                entity.HasOne<Employee>()
                      .WithMany()
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
        
        public void EnsureSeedData()
        {
            Database.EnsureCreated();
            if (!Employees.Any())
            {
                Employees.AddRange(
                    new Employee { EmployeeId = "1001", EmployeeName = "Trần Phú", Department = "Ban Giám Đốc", AvatarUrl = "https://ui-avatars.com/api/?name=Tran+Phu&background=28a745&color=fff" },
                    new Employee { EmployeeId = "1002", EmployeeName = "Lê Hoàng Yến", Department = "Hành chính - Nhân sự", AvatarUrl = "https://ui-avatars.com/api/?name=Le+Hoang+Yen&background=17a2b8&color=fff" },
                    new Employee { EmployeeId = "1003", EmployeeName = "Nguyễn Minh Khang", Department = "IT", AvatarUrl = "https://ui-avatars.com/api/?name=Nguyen+Minh+Khang&background=dc3545&color=fff" },
                    new Employee { EmployeeId = "1004", EmployeeName = "Đặng Thùy Trâm", Department = "Kế toán", AvatarUrl = "https://ui-avatars.com/api/?name=Dang+Thuy+Tram&background=ffc107&color=000" }
                );
                SaveChanges();
            }
        }
    }

    public class SqliteAttendanceService : IAttendanceService
    {
        public event Action? OnDataChanged;

        public SqliteAttendanceService()
        {
            using var db = new AttendanceDbContext();
            db.EnsureSeedData();
        }

        public async Task<bool> ProcessScanAsync(string code)
        {
            using var db = new AttendanceDbContext();
            
            // Fake small hardware transmission delay
            await Task.Delay(100);
            
            // Xử lý Check Employee trong csdl SQLite
            var employee = await db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == code);
            
            if (employee == null)
            {
                // Trả về false nếu không tìm thấy thẻ nhân viên trong database
                return false; 
            }

            // Sinh dữ liệu chấm công thật bằng SecureTimeProvider (Đồng hồ khóa bảo mật)
            var record = new AttendanceRecord 
            { 
                EmployeeId = employee.EmployeeId, 
                EmployeeName = employee.EmployeeName, 
                ScanTime = SecureTimeProvider.Instance.Now, 
                AvatarUrl = employee.AvatarUrl 
            };
            
            db.AttendanceRecords.Add(record);
            await db.SaveChangesAsync();
            
            OnDataChanged?.Invoke();
            return true;
        }

        public List<AttendanceRecord> GetLastScans() 
        {
            using var db = new AttendanceDbContext();
            return db.AttendanceRecords
                     .OrderByDescending(x => x.ScanTime)
                     .Take(10)
                     .ToList();
        }
    }
}
