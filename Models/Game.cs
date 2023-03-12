using Newtonsoft.Json.Linq;

namespace TicTacToe.Models;

public class Game
{
    public int Id { get; set; }
    public Player FirstPlayer { get; set; } = null!;
    public Player SecondPlayer { get; set; } = null!;

    public Game(string json)
    {
        var jObject = JObject.Parse(json);
        var jGame = jObject["game"];
        var players = jGame["players"].ToArray();
        FirstPlayer = new Player(players[0].ToString());
        SecondPlayer = new Player(players[1].ToString());
    }
}
