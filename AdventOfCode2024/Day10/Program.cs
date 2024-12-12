using System.Diagnostics;
using System.Linq;

const string filePath = "input.txt";
const int Width = 52;
const int Height = 52;

var mat = new int[Width, Height];

var lines = File.ReadAllLines(filePath);
ParseMap(lines, mat);

var stopwatch = Stopwatch.StartNew();
var result = CountTrailheadSum(mat, true);
stopwatch.Stop();
Console.WriteLine($"{nameof(CountTrailheadSum)} by score. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

stopwatch.Restart();
result = CountTrailheadSum(mat, false);
stopwatch.Stop();
Console.WriteLine($"{nameof(CountTrailheadSum)} by rating. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

Console.ReadKey();


void ParseMap(string[] lines, int[,] mat)
{
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        for (int j = 0; j < line.Length; j++)
        {
            mat[i, j] = line[j] - '0';
        }
    }
}

int CountTrailheadSum(int[,] mat, bool byScore)
{
    int sum = 0;
    for (int i = 0; i < Width; i++)
    {
        for (int j = 0; j < Height; j++)
        {
            if (mat[i, j] == 0)
            {
                sum += byScore ? 
                    CountTrailheadScore(mat, i, j) :
                    CountTrailheadRaiting(mat, i, j);
            }
        }
    }
    return sum;
}

int CountTrailheadScore(int[,] mat, int i, int j)
{
    int score = 0;
    var currentHeight = 0;
    var visitedTops = new HashSet<(int,int)>();
    var paths = new Queue<(int x, int y)>([(i, j)]);
    while (paths.Count > 0)
    {
        var (x, y) = paths.Dequeue();
        currentHeight = mat[x, y];
        if (currentHeight == 9 && !visitedTops.Contains((x, y)))
        {
            visitedTops.Add((x, y));
            score++;
            continue;
        }
        TryEnqueue(paths, mat, x-1, y, currentHeight);
        TryEnqueue(paths, mat, x+1, y, currentHeight);
        TryEnqueue(paths, mat, x, y-1, currentHeight);
        TryEnqueue(paths, mat, x, y+1, currentHeight);
    }

    return score;
}

int CountTrailheadRaiting(int[,] mat, int i, int j)
{
    int rating = 0;
    var currentHeight = 0;
    var paths = new Queue<(int x, int y)>([(i, j)]);
    while (paths.Count > 0)
    {
        var (x, y) = paths.Dequeue();
        currentHeight = mat[x, y];
        if (currentHeight == 9)
        {
            rating++;
            continue;
        }
        TryEnqueue(paths, mat, x - 1, y, currentHeight);
        TryEnqueue(paths, mat, x + 1, y, currentHeight);
        TryEnqueue(paths, mat, x, y - 1, currentHeight);
        TryEnqueue(paths, mat, x, y + 1, currentHeight);
    }

    return rating;
}


bool IsNextStep(int[,] mat, int i, int j, int currentHeight)
{
    if (i < 0 || i >= Width || j < 0 || j >= Height)
    {
        return false;
    }
    return (mat[i, j] == currentHeight + 1);
}

void TryEnqueue(Queue<(int, int)> paths, int[,] mat, int i, int j, int currentHeight)
{
    if (IsNextStep(mat, i, j, currentHeight))
    {
        paths.Enqueue((i, j));
    }
}