using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;

namespace TicTacToe.Controllers.PlayerControllers;

public interface IPlayerController
{
    public Task<ActionResult<Player>> GetAsync(int id);
    public Task<ActionResult<IEnumerable<Player>>> GetAsync();
    public Task<ActionResult> CreateAsync(Player player);
    public Task<ActionResult> UpdateAsync(Player player);
    public Task<ActionResult> DeleteAsync(int id);
}