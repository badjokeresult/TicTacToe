namespace TicTacToe.Controllers.GameControllers;

public interface IGameController
{
    public Task<IResult> GetAsync(int id);
    public Task<IResult> GetAsync();
    public Task<IResult> CreateAsync(string data);
    public Task<IResult> UpdateAsync(string data);
    public Task<IResult> DeleteAsync(int id);
}