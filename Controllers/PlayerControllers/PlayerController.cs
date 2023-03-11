using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicTacToe.DatabaseContext;
using TicTacToe.Models;

namespace TicTacToe.Controllers.PlayerControllers;

[ApiController]
[Route("api/v1/player")]
public class PlayerController: ControllerBase, IPlayerController
{
    private readonly ILogger<PlayerController> _logger;
    private readonly AppDbContext _context;

    public PlayerController(ILogger<PlayerController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetPlayerById")]
    [Route("/get/{id}")]
    public async Task<ActionResult<Player>> GetAsync(int id)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            _logger.LogWarning($"Player with id = {id} not found");
            return NotFound();
        }
        
        _logger.LogInformation($"Player with id = {id} received");
        return player;
    }

    [HttpGet(Name = "GetAllPlayers")]
    [Route("/get")]
    public async Task<ActionResult<IEnumerable<Player>>> GetAsync()
    {
        var players = await _context.Players.ToListAsync();

        if (players.Count == 0)
        {
            _logger.LogWarning($"There is no player data in db");
            return NotFound();
        }
        
        _logger.LogCritical($"All players received");
        return players;
    }

    [HttpPost(Name = "CreatePlayer")]
    [Route("/create")]
    public async Task<ActionResult> CreateAsync(Player player)
    {
        if (!AreCellsValuesCorrect(player))
            return BadRequest();
        
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut(Name = "UpdatePlayer")]
    [Route("/update")]
    public async Task<ActionResult> UpdateAsync(Player player)
    {
        if (!AreCellsValuesCorrect(player))
            return BadRequest();
        
        var oldPlayer = await _context.Players.FirstOrDefaultAsync(p => p.Id == player.Id);

        if (oldPlayer == null)
        {
            _logger.LogWarning($"Player with id = {player.Id} not found");
            return NotFound();
        }

        var newPlayer = _context.Players.Update(oldPlayer);
        newPlayer.Entity.Id = player.Id;
        newPlayer.Entity.Cells = player.Cells;
        newPlayer.Entity.Name = player.Name;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Player with id = {player.Id} updated");
        return Ok();
    }

    [HttpDelete(Name = "DeletePlayerById")]
    [Route("/delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            _logger.LogWarning($"Player with id = {id} not found");
            return NotFound();
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Player with id = {id} deleted");
        return Ok();
    }

    private bool AreCellsValuesCorrect(Player player)
    {
        foreach (var cell in player.Cells)
        {
            if (cell.Item1 > 2 || cell.Item1 < 0
                               || cell.Item2 > 2 || cell.Item2 < 0)
            {
                _logger.LogError("Invalid cells values were given");
                return false;
            }
        }

        return true;
    }
}