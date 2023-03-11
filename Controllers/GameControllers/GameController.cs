using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TicTacToe.Models;
using TicTacToe.DatabaseContext;

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

    [HttpGet(Name = "GetGameById")]
    [Route("/get/{id}")]
    public async Task<ActionResult<Game>> GetAsync(int id)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            _logger.LogWarning($"Game data with id = {id} not found");
            return NotFound();
        }

        _logger.LogInformation($"Game data with id = {id} received");
        return game;
    }

    [HttpGet(Name = "GetAllGames")]
    [Route("/get")]
    public async Task<ActionResult<IEnumerable<Game>>> GetAsync()
    {
        var games = await _context.Games.ToListAsync();

        if (games.Count == 0)
        {
            _logger.LogWarning("There is no game data in db");
            return NotFound();
        }
        
        _logger.LogInformation("All games data received");
        return games;
    }

    [HttpPost(Name = "CreateGame")]
    [Route("/create")]
    public async Task<ActionResult> CreateAsync(Game game)
    {
        if (!AreBothPlayersDifferent(game))
        {
            _logger.LogError("There are cannot be two similar players in one game");
            return BadRequest();
        }

        if (!AreCellsNotIntercepted(game))
        {
            _logger.LogError("Cells values of both players are intersected");
            return BadRequest();
        }
        
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Game data with id = {game.Id} created");
        
        return Ok();
    }

    [HttpPut(Name = "UpdateGame")]
    [Route("/update")]
    public async Task<ActionResult> UpdateAsync(Game game)
    {
        if (!AreBothPlayersDifferent(game))
        {
            _logger.LogError("There are cannot be two similar players in one game");
            return BadRequest();
        }

        if (!AreCellsNotIntercepted(game))
        {
            _logger.LogError("Cells values of both players are intersected");
            return BadRequest();
        }
        
        var oldGame = await _context.Games.FirstOrDefaultAsync(g => g.Id == game.Id);

        if (oldGame == null)
        {
            _logger.LogWarning($"Game data with id = {game.Id} not found");
            return NotFound();
        }

        var newGame = _context.Games.Update(oldGame);
        newGame.Entity.Id = game.Id;
        newGame.Entity.FirstPlayer.Id = game.FirstPlayer.Id;
        newGame.Entity.SecondPlayer.Id = game.SecondPlayer.Id;
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Game data with id = {game.Id} updated");
        return Ok();
    }

    [HttpDelete(Name = "DeleteGameById")]
    [Route("/delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            _logger.LogWarning($"Game data with id = {id} not found");
            return NotFound();
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Game data with id = {id} deleted");
        return Ok();
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