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

    [HttpGet(Name = "GetAllPlayers")]
    [Route("/get")]
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

    [HttpPost(Name = "CreatePlayer")]
    [Route("/create")]
    public async Task<IResult> CreateAsync(string data)
    {
        var player = ConvertFromJson(data);
        if (player == null)
            return Results.BadRequest();
        
        if (!AreCellsValuesCorrect(player))
            return Results.BadRequest();
        
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();

        return Results.Ok();
    }

    [HttpPut(Name = "UpdatePlayer")]
    [Route("/update")]
    public async Task<IResult> UpdateAsync(string data)
    {
        var player = ConvertFromJson(data);
        if (player == null)
            return Results.BadRequest();
        
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

    [HttpDelete(Name = "DeletePlayerById")]
    [Route("/delete/{id}")]
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
            if (cell.Item1 > 2 || cell.Item1 < 0
                               || cell.Item2 > 2 || cell.Item2 < 0)
            {
                _logger.LogError("Invalid cells values were given");
                return false;
            }
        }

        return true;
    }

    private Player? ConvertFromJson(string data)
    {
        try
        {
            return new Player(data);
        }
        catch (NullReferenceException)
        {
            _logger.LogCritical("Necessary fields not found");
            return null;
        }
    }
}