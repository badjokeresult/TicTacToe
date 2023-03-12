namespace TicTacToe.Models;

public class Game
{
    public int Id { get; set; }
    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;
    public int? WinnerId { get; set; } = null!;
    
    public Game(){}
}
