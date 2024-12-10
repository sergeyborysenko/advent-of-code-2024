using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

const string filePath = "input.txt";
const int Width = 50;
const int Height = 50;

var lines = File.ReadAllLines(filePath);
var mat = new int[Width, Height];

var coordinates = ParseAntennasCoordinates(lines);

var stopwatch = Stopwatch.StartNew();
var result = GetDistinctAntinodesCount([.. coordinates.Values], mat);

stopwatch.Stop();
Console.WriteLine($"{nameof(GetDistinctAntinodesCount)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

Dictionary<char, List<(int, int)>> ParseAntennasCoordinates(string[] lines)
{
    var coordinates = new Dictionary<char, List<(int, int)>>();
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        for (int j = 0; j < line.Length; j++)
        {
            if (line[j] != '.')
            {
                if (!coordinates.ContainsKey(line[j]))
                {
                    coordinates[line[j]] = [];
                }
                coordinates[line[j]].Add((i, j));
            }
        }
    }
    return coordinates;
}
int GetDistinctAntinodesCount(List<List<(int, int)>> coordinatesLists, int[,] mat)
{
    var result = 0;
    foreach (var coordinatesList in coordinatesLists)
    {
        var pairs = GetAllPairs(coordinatesList);
        foreach (var (ant1, ant2) in pairs)
        {
            var (diffX, diffY) = (ant2.X - ant1.X, ant2.Y - ant1.Y);
            if (IsNewAntinode(ref mat, ant1.X - diffX, ant1.Y - diffY))
                result++;
            if (IsNewAntinode(ref mat, ant2.X + diffX, ant2.Y + diffY))
                result++;
        }
    }
    return result;
}

bool IsNewAntinode(ref int[,] mat, int x, int y)
{
    if (x >= 0 && x < Width && y >= 0 && y < Height)
    {
        if (mat[x, y] != 1)
        {
            mat[x, y] = 1;
            return true;
        }
    }
    return false;
}

List<((int X, int Y) Ant1, (int X, int Y) Ant2)> GetAllPairs(List<(int, int)> coordinatesList)
{
    var pairs = new List<((int X, int Y) Ant1, (int X, int Y) Ant2)>();
    for (int i = 0; i < coordinatesList.Count; i++)
    {
        for (int j = i + 1; j < coordinatesList.Count; j++)
        {
            pairs.Add((coordinatesList[i], coordinatesList[j]));
        }
    }
    return pairs;
}