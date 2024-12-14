using System.Diagnostics;

const string filePath = "input.txt";
const int Width = 140;
const int Height = 140;

var mat = new char[Width, Height];

var lines = File.ReadAllLines(filePath);
ParseMap(lines, mat);

var stopwatch = Stopwatch.StartNew();
var result = CountSumFencePrice(mat);

stopwatch.Stop();
Console.WriteLine($"{nameof(CountSumFencePrice)} by score. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

void ParseMap(string[] lines, char[,] mat)
{
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        for (int j = 0; j < line.Length; j++)
        {
            mat[i, j] = line[j];
        }
    }
}
long CountSumFencePrice(char[,] mat)
{
    var count = 0;
    bool[,] visited = new bool[Width, Height];
    for (int i = 0; i < Width; i++)
    {
        for (int j = 0; j < Height; j++)
        {
           if (!visited[i, j])
            {
                count += CountNewRegionPrice(mat, i, j, visited);
            }    
        }
    }
    return count;
}

int CountNewRegionPrice(char[,] mat, int i, int j, bool[,] visited)
{
    var area = 0;
    var perimeter = 0;
    var type = mat[i, j];
    var queue = new Queue<(int x, int y)>(new[] { (i, j) });
    while(queue.Count > 0)
    {
        var (x, y) = queue.Dequeue();
        if (visited[x, y])
            continue;
        visited[x, y] = true;
        area++;
        perimeter += 4;

        if (x-1 >= 0 && mat[x - 1, y] == type)
        {
            perimeter--;
            if (!visited[x - 1, y])
                queue.Enqueue((x - 1, y));
        }

        if (y-1 >= 0 && mat[x, y-1] == type)
        {
            perimeter--;
            if (!visited[x, y - 1] )
                queue.Enqueue((x, y-1));
        }

        if (x + 1 < Height && mat[x + 1, y] == type)
        {
            perimeter--;
            if (!visited[x + 1, y])
                queue.Enqueue((x + 1, y));
        }

        if (y + 1 < Width && mat[x, y + 1] == type)
        {
            perimeter--;
            if (!visited[x, y + 1])
                queue.Enqueue((x, y + 1));
        }
    }
    return area * perimeter;
}