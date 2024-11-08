using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DotaDashboardAPI.Models;

namespace DotaDashboardAPI.Services
{
    public class HeroService
    {
        private readonly HttpClient _httpClient;

        public HeroService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.opendota.com/api/");
        }

        public async Task<IEnumerable<Hero>> GetHeroStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("herostats");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<IEnumerable<Hero>>(content, options)
                    ?? new List<Hero>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to fetch hero stats", ex);
            }
        }

        public async Task<PlayerStats> GetPlayerStatsAsync(int playerId)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var responseProfile = await _httpClient.GetAsync($"players/{playerId}");

            responseProfile.EnsureSuccessStatusCode();
            var contentProfile = await responseProfile.Content.ReadAsStringAsync();

            var playerStats =
                JsonSerializer.Deserialize<PlayerStats>(contentProfile, options)
                ?? new PlayerStats();

            var responseHeroes = await _httpClient.GetAsync($"players/{playerId}/heroes");
            responseHeroes.EnsureSuccessStatusCode();
            var contentHeroes = await responseHeroes.Content.ReadAsStringAsync();

            var playerHeroes =
                JsonSerializer.Deserialize<List<PlayerHeroStats>>(contentHeroes, options)
                ?? new List<PlayerHeroStats>();

            var favoriteHeroes = playerHeroes
                .OrderByDescending(h => h.Games)
                .Take(5)
                .Select(h => h.HeroId)
                .ToList();

            playerStats.FavoriteHeroes = favoriteHeroes;

            return playerStats;
        }

        public async Task<Dictionary<string, List<HeroDto>>> GetTopHeroesByTierAsync()
        {
            var heroes = await GetHeroStatsAsync();

            var maxProPick = heroes.Max(h => h.ProPickRate);
            var maxProBan = heroes.Max(h => h.ProBanRate);

            var rankBracketMapping = new Dictionary<string, int>
            {
                { "Herald", 1 },
                { "Guardian", 2 },
                { "Crusader", 3 },
                { "Archon", 4 },
                { "Legend", 5 },
                { "Ancient", 6 },
                { "Divine", 7 },
                { "Immortal", 8 },
            };

            var topHeroesByRank = new Dictionary<string, List<HeroDto>>();

            foreach (var rank in rankBracketMapping.Keys)
            {
                int bracket = rankBracketMapping[rank];

                foreach (var hero in heroes)
                {
                    float bracketWinRate = hero.GetBracketWinRate(bracket);

                    var normalizedPickRate =
                        maxProPick > 0 ? (float)hero.ProPickRate / maxProPick : 0;
                    var normalizedBanRate = maxProBan > 0 ? (float)hero.ProBanRate / maxProBan : 0;

                    hero.CompositeScore =
                        (bracketWinRate * 0.4f)
                        + (normalizedPickRate * 0.2f)
                        + (normalizedBanRate * 0.2f)
                        + (hero.WinRate * 0.2f);
                }

                var topHeroes = heroes
                    .OrderByDescending(h => h.CompositeScore)
                    .Take(10)
                    .Select(hero => new HeroDto
                    {
                        Id = hero.Id,
                        Image = hero.Image,
                        LocalizedName = hero.LocalizedName,
                        PrimaryAttribute = hero.PrimaryAttribute,
                        TotalPick = hero.ProPickRate,
                        TotalBan = hero.ProBanRate,
                        WinRate = hero.WinRate.ToString("F2"),
                    })
                    .ToList();

                topHeroesByRank[rank] = topHeroes;
            }

            return topHeroesByRank;
        }

        public async Task<IEnumerable<HeroDto>> GetMetaHeroesByProStatsAsync()
        {
            var heroes = await GetHeroStatsAsync();

            foreach (var hero in heroes)
            {
                hero.CompositeScore =
                    hero.ProPickRate > 0 ? (float)hero.ProWinRate / hero.ProPickRate * 100 : 0;
            }

            var metaHeroes = heroes
                .OrderByDescending(h =>
                    (h.ProPickRate * 0.4) + (h.ProBanRate * 0.3) + (h.CompositeScore * 0.3)
                )
                .Take(10)
                .Select(hero => new HeroDto
                {
                    Id = hero.Id,
                    Image = hero.Image,
                    LocalizedName = hero.LocalizedName,
                    PrimaryAttribute = hero.PrimaryAttribute,
                    TotalPick = hero.ProPickRate,
                    TotalBan = hero.ProBanRate,
                    WinRate = hero.WinRate.ToString("F2"),
                })
                .ToList();

            return metaHeroes;
        }

        public async Task<List<PlayerHeroStats>> GetPlayerHeroStatsAsync(int playerId)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var responseHeroes = await _httpClient.GetAsync($"players/{playerId}/heroes");
            responseHeroes.EnsureSuccessStatusCode();
            var contentHeroes = await responseHeroes.Content.ReadAsStringAsync();

            var playerHeroes =
                JsonSerializer.Deserialize<List<PlayerHeroStats>>(contentHeroes, options)
                ?? new List<PlayerHeroStats>();

            return playerHeroes;
        }

        private void ComputeCompositeScores(IEnumerable<Hero> heroes)
        {
            var maxProPick = heroes.Max(h => h.ProPickRate);
            var maxProBan = heroes.Max(h => h.ProBanRate);

            foreach (var hero in heroes)
            {
                float winRate =
                    hero.ProPickRate > 0 ? (float)hero.ProWinRate / hero.ProPickRate * 100 : 0;
                var normalizedPickRate = maxProPick > 0 ? (float)hero.ProPickRate / maxProPick : 0;
                var normalizedBanRate = maxProBan > 0 ? (float)hero.ProBanRate / maxProBan : 0;

                hero.CompositeScore =
                    (hero.WinRate * 0.5f)
                    + (normalizedPickRate * 0.3f)
                    + (normalizedBanRate * 0.2f);
            }
        }

        public async Task<IEnumerable<HeroDto>> GetRecommendedHeroesForPlayerAsync(int playerId)
        {
            var playerStats = await GetPlayerStatsAsync(playerId);
            var heroes = await GetHeroStatsAsync();

            var playerHeroStats = await GetPlayerHeroStatsAsync(playerId);

            var playerHeroStatsDict = playerHeroStats.ToDictionary(h => h.HeroId, h => h);

            ComputeCompositeScores(heroes);

            var metaHeroes = await GetMetaHeroesByProStatsAsync();
            var metaHeroIds = new HashSet<int>(metaHeroes.Select(h => h.Id));

            var recommendedHeroes = heroes
                .Where(h => metaHeroIds.Contains(h.Id))
                .Where(h =>
                    !playerHeroStatsDict.ContainsKey(h.Id) || playerHeroStatsDict[h.Id].Games < 5
                )
                .OrderByDescending(h => h.CompositeScore)
                .Take(5)
                .Select(hero => new HeroDto
                {
                    Id = hero.Id,
                    Image = hero.Image,
                    LocalizedName = hero.LocalizedName,
                    PrimaryAttribute = hero.PrimaryAttribute,
                    TotalPick = hero.ProPickRate,
                    TotalBan = hero.ProBanRate,
                    WinRate = hero.WinRate.ToString("F2"),
                })
                .ToList();

            return recommendedHeroes;
        }
    }
}
