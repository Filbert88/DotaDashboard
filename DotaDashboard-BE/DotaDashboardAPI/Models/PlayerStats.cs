// DotaDashboardAPI/Models/PlayerStats.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DotaDashboardAPI.Models
{
    public class PlayerStats
    {
        [JsonPropertyName("profile")]
        public PlayerProfile? Profile { get; set; }

        [JsonPropertyName("favorite_heroes")]
        public List<int> FavoriteHeroes { get; set; } = new List<int>();
    }

    public class PlayerProfile
    {
        [JsonPropertyName("account_id")]
        public int AccountId { get; set; }

        [JsonPropertyName("personaname")]
        public string? Personaname { get; set; }

        [JsonPropertyName("profileurl")]
        public string? ProfileUrl { get; set; }

        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

    }
}
