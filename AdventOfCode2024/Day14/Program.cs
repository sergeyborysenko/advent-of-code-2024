using System.Diagnostics;

const string filePath = "input.txt";
const int Width = 101;
const int Height = 103;

var mat = new int[Height, Width];


var lines = File.ReadAllLines(filePath);
var robots = ParseInput(lines);

var stopwatch = Stopwatch.StartNew();
var result = CountSafetyFactor(robots, 100);
stopwatch.Stop();
Console.WriteLine($"{nameof(CountSafetyFactor)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

IEnumerable<Robot?> ParseInput(string[] lines)
{
    foreach(var line in lines)
    {
        yield return Robot.TryParse(line, out var robot) ? robot : null;
    }
}

long CountSafetyFactor(IEnumerable<Robot?> robots, int seconds)
{
    foreach (var robot in robots)
    {
        if (robot == null)
        {
            continue;
        }

        robot.Move(Height, Width, seconds);
        mat[robot.Position.X, robot.Position.Y]++;
    }
    var kv1 = CountNumberOfRobots(mat, 0, Height/2, 0, Width / 2);
    var kv2 = CountNumberOfRobots(mat, Height / 2 + 1, Height, 0, Width / 2);
    var kv3 = CountNumberOfRobots(mat, 0, Height/2, Width / 2 + 1, Width );
    var kv4 = CountNumberOfRobots(mat, Height / 2 + 1, Height, Width / 2 + 1, Width);

    return kv1 * kv2 * kv3 * kv4;
}

int CountNumberOfRobots(int[,] mat, int startX, int endX, int startY, int endY)
{
    var count = 0;
    for (int x = startX; x < endX; x++)
    {
        for (int y = startY; y < endY; y++)
        {
            count += mat[x, y];
        }
    }
    return count;
}