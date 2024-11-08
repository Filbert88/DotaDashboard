using System.Text.Json.Serialization;

namespace DotaDashboardAPI.Models
{
    public class HeroDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("localized_name")]
        public string? LocalizedName { get; set; }

        [JsonPropertyName("attr")]
        public string? PrimaryAttribute { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("total_pick")]
        public int TotalPick { get; set; }

        [JsonPropertyName("total_ban")]
        public int TotalBan { get; set; }

        [JsonPropertyName("winrate")]
        public string? WinRate { get; set; } 
    }
}
