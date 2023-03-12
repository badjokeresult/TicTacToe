using Newtonsoft.Json.Linq;

namespace TicTacToe.Models;

public class Player
{
    public int Id { get; set; }
    public (int, int)[] Cells { get; set; } = null!;
    public string Name { get; set; } = null!;

    public Player(string json)
    {
        var jObject = JObject.Parse(json);
        var jPlayer = jObject["player"];
        Name = (string)jPlayer["name"];
        Cells = jPlayer["cells"].ToObject<(int, int)[]>();
    }
}
