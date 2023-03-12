using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TicTacToe.DatabaseContext;
using TicTacToe.Models;

namespace TicTacToe.Controllers.PlayerControllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PlayerController: ControllerBase, IPlayerController
{
    private readonly ILogger<PlayerController> _logger;
    private readonly AppDbContext _context;

    public PlayerController(ILogger<PlayerController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("get/{id}")]
    public async Task<IResult> GetAsync(int id)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            _logger.LogWarning($"Player with id = {id} not found");
            return Results.NotFound();
        }
        
        _logger.LogInformation($"Player with id = {id} received");
        return Results.Json(player);
    }

    [HttpGet("get")]
    public async Task<IResult> GetAsync()
    {
        var players = await _context.Players.ToListAsync();

        if (players.Count == 0)
        {
            _logger.LogWarning($"There is no player data in db");
            return Results.NotFound();
        }
        
        _logger.LogCritical($"All players received");
        return Results.Json(players);
    }

    [HttpPost("create")]
    public async Task<IResult> CreateAsync(Player player)
    {
        if (!AreCellsValuesCorrect(player))
        {
            _logger.LogWarning("Cells values are not in range 0-2");
            return Results.BadRequest();
        }

        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();

        return Results.Ok();
    }
    
    [HttpPut("update")]
    public async Task<IResult> UpdateAsync(Player player)
    {
        if (!AreCellsValuesCorrect(player))
            return Results.BadRequest();
        
        var oldPlayer = await _context.Players.FirstOrDefaultAsync(p => p.Id == player.Id);

        if (oldPlayer == null)
        {
            _logger.LogWarning($"Player with id = {player.Id} not found");
            return Results.NotFound();
        }

        var newPlayer = _context.Players.Update(oldPlayer);
        newPlayer.Entity.Id = player.Id;
        newPlayer.Entity.Cells = player.Cells;
        newPlayer.Entity.Name = player.Name;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Player with id = {player.Id} updated");
        return Results.Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IResult> DeleteAsync(int id)
    {
        var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            _logger.LogWarning($"Player with id = {id} not found");
            return Results.NotFound();
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Player with id = {id} deleted");
        return Results.Ok();
    }

    private bool AreCellsValuesCorrect(Player player)
    {
        foreach (var cell in player.Cells)
        {
            if (cell[0] > 2 || cell[0] < 0
                               || cell[1] > 2 || cell[1] < 0)
            {
                _logger.LogError("Invalid cells values were given");
                return false;
            }
        }

        return true;
    }
}