using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TTL.Shared.Services.Attendance
{
    /// <summary>
    /// Cung cấp một nguồn thời gian an toàn, không bị ảnh hưởng bởi việc thay đổi giờ hệ thống (OS Clock Tampering).
    /// Tự động đồng bộ với máy chủ khi có mạng và duy trì tính tiến thời gian liên tục.
    /// </summary>
    public class SecureTimeProvider
    {
        private static SecureTimeProvider? _instance;
        public static SecureTimeProvider Instance => _instance ??= new SecureTimeProvider();

        private DateTime _trustedBaseTimeUtc;
        private long _baseTickCount;
        private readonly string _cacheFile;
        private Timer? _syncTimer;
        private Timer? _saveTimer;
        private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);

        private SecureTimeProvider()
        {
            _cacheFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ttl_secure_time_v2.json");
            LoadFromCache();
            
            // Try network sync immediately in background
            Task.Run(SyncNetworkTimeAsync);

            // Check sync occasionally (every 5 minutes)
            _syncTimer = new Timer(async _ => await SyncNetworkTimeAsync(), null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15));
            
            // Save state every 1 minute to preserve progress in case of crash/power loss
            _saveTimer = new Timer(_ => SaveToCache(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        public DateTime Now
        {
            get
            {
                long currentTick = Environment.TickCount64;
                long elapsedMs = currentTick - _baseTickCount;
                
                if (elapsedMs < 0) 
                {
                    // Fallback in case counter wraps
                    elapsedMs = 0;
                    _baseTickCount = currentTick; 
                }

                return _trustedBaseTimeUtc.AddMilliseconds(elapsedMs).ToLocalTime();
            }
        }

        private async Task SyncNetworkTimeAsync()
        {
            await _syncLock.WaitAsync();
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                
                // Using Google's header as a highly reliable, low-latency NTP alternative
                var request = new HttpRequestMessage(HttpMethod.Head, "https://www.google.com");
                var response = await client.SendAsync(request);
                
                if (response.Headers.Date.HasValue)
                {
                    var utcTime = response.Headers.Date.Value.UtcDateTime;
                    
                    _trustedBaseTimeUtc = utcTime;
                    _baseTickCount = Environment.TickCount64;
                    
                    SaveToCache();
                }
            }
            catch
            {
                // Network unavailable. Soft fail. Local execution continues safely via TickCount64.
            }
            finally
            {
                _syncLock.Release();
            }
        }

        private void LoadFromCache()
        {
            bool loaded = false;
            try
            {
                if (File.Exists(_cacheFile))
                {
                    var json = File.ReadAllText(_cacheFile);
                    var state = JsonSerializer.Deserialize<TimeState>(json);
                    if (state != null)
                    {
                        var lastSavedTrustedUtc = state.LastTrustedUtc;
                        var lastSavedOsUtc = state.LastOsUtc;
                        var currentOsUtc = DateTime.UtcNow;

                        TimeSpan osDelta = currentOsUtc - lastSavedOsUtc;
                        
                        // Security Rule: If OS time went backward, assume 0 offline time passed (Tampering Detected)
                        if (osDelta.TotalSeconds < 0)
                        {
                            osDelta = TimeSpan.Zero;
                        }

                        _trustedBaseTimeUtc = lastSavedTrustedUtc.Add(osDelta);
                        _baseTickCount = Environment.TickCount64;
                        loaded = true;
                    }
                }
            }
            catch { }
            
            if (!loaded)
            {
                _trustedBaseTimeUtc = DateTime.UtcNow;
                _baseTickCount = Environment.TickCount64;
            }
        }

        private void SaveToCache()
        {
            try
            {
                var state = new TimeState 
                { 
                    // Calculate exact UTC time right now using our trusted logic
                    LastTrustedUtc = _trustedBaseTimeUtc.AddMilliseconds(Environment.TickCount64 - _baseTickCount),
                    LastOsUtc = DateTime.UtcNow 
                };
                File.WriteAllText(_cacheFile, JsonSerializer.Serialize(state));
            }
            catch { }
        }

        private class TimeState
        {
            public DateTime LastTrustedUtc { get; set; }
            public DateTime LastOsUtc { get; set; }
        }
    }
}
