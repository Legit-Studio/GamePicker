using Microsoft.AspNetCore.Mvc;
using Backend.Services;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SteamController : ControllerBase
    {
        private readonly SteamService _steamService;

        public SteamController(SteamService steamService)
        {
            _steamService = steamService;
        }

        [HttpGet("games")]
        public async Task<IActionResult> GetAllSteamGames([FromQuery] int limit = 10)
        {
            var allGames = await _steamService.GetAllSteamGamesAsync(limit);
            return Ok(allGames);
        }
    }
}