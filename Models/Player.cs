namespace TicTacToe.Models;

public class Player
{
    public int Id { get; set; }
    public int[][] Cells { get; set; } = null!;
    public string Name { get; set; } = null!;
    
    public Player(){}
}
