using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;

namespace TicTacToe.Controllers.GameControllers;

public interface IGameController
{
    public Task<ActionResult<Game>> GetAsync(int id);
    public Task<ActionResult<IEnumerable<Game>>> GetAsync();
    public Task<ActionResult> CreateAsync(Game game);
    public Task<ActionResult> UpdateAsync(Game game);
    public Task<ActionResult> DeleteAsync(int id);
}