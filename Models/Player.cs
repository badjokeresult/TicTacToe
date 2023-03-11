namespace TicTacToe.Models;

public class Player
{
    public int Id { get; set; }
    public (int, int)[] Cells { get; set; }
    public string Name { get; set; }
}
