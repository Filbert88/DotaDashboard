using System.Text.Json.Serialization;

namespace DotaDashboardAPI.Models
{
    public class Hero
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("localized_name")]
        public string? LocalizedName { get; set; }

        [JsonPropertyName("primary_attr")]
        public string? PrimaryAttribute { get; set; }

        [JsonPropertyName("img")]
        public string? Image { get; set; }

        [JsonPropertyName("attack_type")]
        public string? AttackType { get; set; }

        [JsonPropertyName("base_health")]
        public float BaseHealth { get; set; }

        [JsonPropertyName("base_armor")]
        public float BaseArmor { get; set; }

        [JsonPropertyName("pro_win")]
        public int ProWinRate { get; set; }

        [JsonPropertyName("pro_pick")]
        public int ProPickRate { get; set; }

        [JsonPropertyName("pro_ban")]
        public int ProBanRate { get; set; }

        [JsonPropertyName("1_pick")]
        public int Bracket1Pick { get; set; }

        [JsonPropertyName("1_win")]
        public int Bracket1Win { get; set; }

        [JsonPropertyName("2_pick")]
        public int Bracket2Pick { get; set; }

        [JsonPropertyName("2_win")]
        public int Bracket2Win { get; set; }

        [JsonPropertyName("3_pick")]
        public int Bracket3Pick { get; set; }

        [JsonPropertyName("3_win")]
        public int Bracket3Win { get; set; }

        [JsonPropertyName("4_pick")]
        public int Bracket4Pick { get; set; }

        [JsonPropertyName("4_win")]
        public int Bracket4Win { get; set; }

        [JsonPropertyName("5_pick")]
        public int Bracket5Pick { get; set; }

        [JsonPropertyName("5_win")]
        public int Bracket5Win { get; set; }

        [JsonPropertyName("6_pick")]
        public int Bracket6Pick { get; set; }

        [JsonPropertyName("6_win")]
        public int Bracket6Win { get; set; }

        [JsonPropertyName("7_pick")]
        public int Bracket7Pick { get; set; }

        [JsonPropertyName("7_win")]
        public int Bracket7Win { get; set; }

        [JsonPropertyName("8_pick")]
        public int Bracket8Pick { get; set; }

        [JsonPropertyName("8_win")]
        public int Bracket8Win { get; set; }

        public float WinRate
        {
            get { return ProPickRate > 0 ? (float)ProWinRate / ProPickRate * 100 : 0; }
        }

        public float CompositeScore { get; set; }

        public float GetBracketWinRate(int bracket)
        {
            return bracket switch
            {
                1 => Bracket1Pick > 0 ? (float)Bracket1Win / Bracket1Pick * 100 : 0,
                2 => Bracket2Pick > 0 ? (float)Bracket2Win / Bracket2Pick * 100 : 0,
                3 => Bracket3Pick > 0 ? (float)Bracket3Win / Bracket3Pick * 100 : 0,
                4 => Bracket4Pick > 0 ? (float)Bracket4Win / Bracket4Pick * 100 : 0,
                5 => Bracket5Pick > 0 ? (float)Bracket5Win / Bracket5Pick * 100 : 0,
                6 => Bracket6Pick > 0 ? (float)Bracket6Win / Bracket6Pick * 100 : 0,
                7 => Bracket7Pick > 0 ? (float)Bracket7Win / Bracket7Pick * 100 : 0,
                8 => Bracket8Pick > 0 ? (float)Bracket8Win / Bracket8Pick * 100 : 0,
                _ => 0,
            };
        }
    }
}
