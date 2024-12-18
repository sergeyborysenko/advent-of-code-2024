using Day13;
using System.Diagnostics;

const string filePath = "input.txt";
const int Width = 140;
const int Height = 140;

var mat = new char[Width, Height];

var lines = File.ReadAllLines(filePath);
var arcades = ParseInput(lines);

var stopwatch = Stopwatch.StartNew();
var result = CountMinAmountTokens(arcades);

stopwatch.Stop();
Console.WriteLine($"{nameof(CountMinAmountTokens)} by score. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

var arcadesWithOffset = ParseInput(lines, 10000000000000);
stopwatch.Restart();
result = CountMinAmountTokens(arcadesWithOffset, 0);

stopwatch.Stop();
Console.WriteLine($"{nameof(CountMinAmountTokens)} by score. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
Console.ReadKey();

long CountMinAmountTokens(List<Arcade> arcades, int maxPressAmounts = 100)
{
    long minAmountTokens = 0;
    foreach (var arcade in arcades)
    {
        minAmountTokens += arcade.CalculateNumberOfTokens(maxPressAmounts);
    }
    return minAmountTokens;
}

List<Arcade> ParseInput(string[] lines, long prizeOffset = 0)
{
    var arcades = new List<Arcade>();
    Arcade? currentArcade = null;

    foreach (var line in lines)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            if (currentArcade != null)
            {
                arcades.Add(currentArcade);
                currentArcade = null;
            }
            continue;
        }

        if (line.StartsWith("Button A:"))
        {
            var coordinates = line[9..].Split(',');
            var x = int.Parse(coordinates[0].Split('+')[1]);
            var y = int.Parse(coordinates[1].Split('+')[1]);
            currentArcade ??= new Arcade();
            currentArcade.A = (x, y);
        }
        else if (line.StartsWith("Button B:"))
        {
            var coordinates = line[9..].Split(',');
            var x = int.Parse(coordinates[0].Split('+')[1]);
            var y = int.Parse(coordinates[1].Split('+')[1]);
            currentArcade ??= new Arcade();
            currentArcade.B = (x, y);
        }
        else if (line.StartsWith("Prize:"))
        {
            var coordinates = line[6..].Split(',');
            var x = int.Parse(coordinates[0].Split('=')[1]);
            var y = int.Parse(coordinates[1].Split('=')[1]);
            currentArcade ??= new Arcade();
            currentArcade.Prize = (prizeOffset + x, prizeOffset + y);
        }
    }

    if (currentArcade != null)
    {
        arcades.Add(currentArcade);
    }

    return arcades;
}
