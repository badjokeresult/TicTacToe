using TicTacToe.Models;

namespace TicTacToe.Controllers.GameControllers;

public interface IGameController
{
    public Task<IResult> GetAsync(int id);
    public Task<IResult> GetAsync();
    public Task<IResult> CreateAsync(Game game);
    public Task<IResult> UpdateAsync(Game game);
    public Task<IResult> DeleteAsync(int id);
}