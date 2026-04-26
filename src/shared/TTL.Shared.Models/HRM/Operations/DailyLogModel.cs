namespace TTL.Shared.Models.HRM.Operations
{
    public class DailyLogModel
    {
        public string Date { get; set; } = string.Empty;
        public string CheckIn { get; set; } = "-";
        public string CheckOut { get; set; } = "-";
        public int LateMinutes { get; set; } = 0;
        public int EarlyMinutes { get; set; } = 0;
        public double OtHours { get; set; } = 0;
    }
}
