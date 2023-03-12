using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TicTacToe.DatabaseContext.Converters;

public class CellsArrayToStringConverter: ValueConverter<int[][], string>
{
    public CellsArrayToStringConverter() : base(
        a => CellsArrayToString(a),
        s => StringToCellsArray(s))
    { }

    private static string CellsArrayToString(int[][] array)
    {
        if (array.Length == 0)
            return null;

        var sb = new StringBuilder();

        foreach (var item in array)
        {
            var first = item[0];
            var second = item[1];
            sb.Append(first)
                .Append(',')
                .Append(second)
                .Append(';');
        }

        return sb.ToString();
    }

    private static int[][] StringToCellsArray(string text)
    {
        if (text.Length == 0)
            return null;

        var strArray = text.Split(';');
        var result = new int[strArray.Length][];

        string[] items;
        for (var i = 0; i < result.Length; i++)
        {
            items = strArray[0].Split(',');
            var first = int.Parse(items[0]);
            var second = int.Parse(items[1]);
            result[i] = new []{first, second};
        }

        return result;
    }
}