using TicTacToe.Models;

namespace TicTacToe.Controllers.PlayerControllers;

public interface IPlayerController
{
    public Task<IResult> GetAsync(int id);
    public Task<IResult> GetAsync();
    public Task<IResult> CreateAsync(Player player);
    public Task<IResult> UpdateAsync(Player player);
    public Task<IResult> DeleteAsync(int id);
}