using DotaDashboardAPI.Models;
using DotaDashboardAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HeroesController : ControllerBase
{
    private readonly HeroService _heroService;

    public HeroesController(HeroService heroService)
    {
        _heroService = heroService;
    }

    [HttpGet("top-by-tier")]
    public async Task<IActionResult> GetTopHeroesByTier()
    {
        try
        {
            var result = await _heroService.GetTopHeroesByTierAsync();
            return Ok(
                new ApiResponse<Dictionary<string, List<HeroDto>>>(
                    success: true,
                    message: "Top heroes by tier retrieved successfully.",
                    data: result
                )
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new ApiResponse<string>(
                    success: false,
                    message: $"An error occurred: {ex.Message}",
                    data: null
                )
            );
        }
    }

    [HttpGet("meta-by-pro-stats")]
    public async Task<IActionResult> GetMetaHeroesByProStats()
    {
        try
        {
            var result = await _heroService.GetMetaHeroesByProStatsAsync();
            return Ok(
                new ApiResponse<IEnumerable<HeroDto>>(
                    success: true,
                    message: "Meta heroes by pro stats retrieved successfully.",
                    data: result
                )
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new ApiResponse<string>(
                    success: false,
                    message: $"An error occurred: {ex.Message}",
                    data: null
                )
            );
        }
    }

    [HttpGet("recommend/{playerId}")]
    public async Task<IActionResult> GetRecommendedHeroes(int playerId)
    {
        if (playerId <= 0)
        {
            return BadRequest(
                new ApiResponse<string>(
                    success: false,
                    message: "Invalid player ID. Player ID must be a positive integer.",
                    data: null
                )
            );
        }

        try
        {
            // Log before calling the service to troubleshoot issues
            Console.WriteLine($"Fetching recommended heroes for player ID: {playerId}");
            var result = await _heroService.GetRecommendedHeroesForPlayerAsync(playerId);

            if (result == null || !result.Any())
            {
                return NotFound(
                    new ApiResponse<string>(
                        success: false,
                        message: "No heroes found for the specified player ID.",
                        data: null
                    )
                );
            }

            return Ok(
                new ApiResponse<IEnumerable<HeroDto>>(
                    success: true,
                    message: "Recommended heroes retrieved successfully.",
                    data: result
                )
            );
        }
        catch (Exception ex)
        {
            // Log the full exception for debugging purposes
            Console.Error.WriteLine($"Error in GetRecommendedHeroes: {ex}");
            return StatusCode(
                500,
                new ApiResponse<string>(
                    success: false,
                    message: $"An error occurred: {ex.Message}",
                    data: null
                )
            );
        }
    }
}
