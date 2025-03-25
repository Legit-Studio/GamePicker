using Microsoft.AspNetCore.Mvc;
using Backend.Services;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SteamController(SteamService steamService) : ControllerBase
{
    [HttpGet("games")]
    public async Task<IActionResult> FetchAndStoreGames([FromQuery] int limit = 10)
    {
        var games = await steamService.FetchAndStoreSteamGamesAsync(limit);
        return Ok(games);
    }

    [HttpGet("stored")]
    public async Task<IActionResult> GetStoredGames([FromQuery] int limit = 10)
    {
        var games = await steamService.GetStoredGamesAsync(limit);
        return Ok(games);
    }

}
