using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TicTacToe.DatabaseContext;
using TicTacToe.Models;

namespace TicTacToe.Controllers.GameControllers;

[ApiController]
[Route("api/v1/game")]
public class GameController: ControllerBase, IGameController
{
    private readonly ILogger<GameController> _logger;
    private readonly AppDbContext _context;

    public GameController(ILogger<GameController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet("get/{id}")]
    public async Task<IResult> GetAsync(int id)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            _logger.LogWarning($"Game data with id = {id} not found");
            return Results.NotFound();
        }

        _logger.LogInformation($"Game data with id = {id} received");
        return Results.Json(game);
    }
    
    [HttpGet("get-all")]
    public async Task<IResult> GetAsync()
    {
        var games = await _context.Games.ToListAsync();

        if (games.Count == 0)
        {
            _logger.LogWarning("There is no game data in db");
            return Results.NotFound();
        }
        
        _logger.LogInformation("All games data received");
        return Results.Json(games);
    }
    
    [HttpPost("create")]
    public async Task<IResult> CreateAsync(Game game)
    {
        if (!AreBothPlayersDifferent(game))
        {
            _logger.LogError("There are cannot be two similar players in one game");
            return Results.BadRequest();
        }

        if (!AreCellsNotIntercepted(game))
        {
            _logger.LogError("Cells values of both players are intersected");
            return Results.BadRequest();
        }
        
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Game data with id = {game.Id} created");
        
        return Results.Ok();
    }

    [HttpPut("update")]
    public async Task<IResult> UpdateAsync(Game game)
    {
        if (!AreBothPlayersDifferent(game))
        {
            _logger.LogError("There are cannot be two similar players in one game");
            return Results.BadRequest();
        }

        if (!AreCellsNotIntercepted(game))
        {
            _logger.LogError("Cells values of both players are intersected");
            return Results.BadRequest();
        }
        
        var oldGame = await _context.Games.FirstOrDefaultAsync(g => g.Id == game.Id);

        if (oldGame == null)
        {
            _logger.LogWarning($"Game data with id = {game.Id} not found");
            return Results.NotFound();
        }

        var newGame = _context.Games.Update(oldGame);
        newGame.Entity.Id = game.Id;
        newGame.Entity.FirstPlayer.Id = game.FirstPlayer.Id;
        newGame.Entity.SecondPlayer.Id = game.SecondPlayer.Id;
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Game data with id = {game.Id} updated");
        return Results.Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IResult> DeleteAsync(int id)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            _logger.LogWarning($"Game data with id = {id} not found");
            return Results.NotFound();
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Game data with id = {id} deleted");
        return Results.Ok();
    }

    private bool AreBothPlayersDifferent(Game game) => 
        game.FirstPlayer.Id != game.SecondPlayer.Id;

    private bool AreCellsNotIntercepted(Game game)
    {
        var firstPlayerCells = game.FirstPlayer.Cells.ToHashSet();
        var secondPlayerCells = game.SecondPlayer.Cells.ToHashSet();

        firstPlayerCells.IntersectWith(secondPlayerCells);
        return firstPlayerCells.Count == 0;
    }
}