// DotaDashboardAPI/Models/PlayerHeroStats.cs
using System.Text.Json.Serialization;

namespace DotaDashboardAPI.Models
{
    public class PlayerHeroStats
    {
        [JsonPropertyName("hero_id")]
        public int HeroId { get; set; }

        [JsonPropertyName("last_played")]
        public int LastPlayed { get; set; }

        [JsonPropertyName("games")]
        public int Games { get; set; }

        [JsonPropertyName("win")]
        public int Win { get; set; }

        // Add other properties if needed
    }
}
