using System.Text.Json.Serialization;

namespace TTL.Shared.Models.HRM.Operations
{
    public class ShiftStatusModel
    {
        [JsonPropertyName("status_id")]
        public int StatusId { get; set; }

        [JsonPropertyName("status_name")]
        public string StatusName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
