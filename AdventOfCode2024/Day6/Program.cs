using System.Diagnostics;

const string filePath = "input.txt";
const int Width = 130;
const int Height = 130;

var lines = File.ReadAllLines(filePath);


var x = 0;
var y = 0;
int[] direction = [-1, 0]; // up
var mat = ParseInputToMatrix(lines, out x, out y);

var stopwatch = Stopwatch.StartNew();
var result = GetDistinctPositionsCount(mat, x, y, direction);
stopwatch.Stop();
Console.WriteLine($"{nameof(GetDistinctPositionsCount)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

stopwatch.Restart();
result = GetDistinctLoopObstaclePositionsCount(mat, x, y, direction);
stopwatch.Stop();
Console.WriteLine($"{nameof(GetDistinctLoopObstaclePositionsCount)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

int GetDistinctPositionsCount(int[,] mat, int x, int y, int[] direction)
{
    int result = 0;
    while(true)
    {
        if (mat[x,y] == 0) // not visited
        {
            result++;
            mat[x,y] = 1;
            x += direction[0];
            y += direction[1];
        }
        else if (mat[x,y] >= 1) // visited
        {
            mat[x, y]++;
            x += direction[0];
            y += direction[1];
        }
        else if (mat[x,y] == -1) // obstacle
        {
            // get one step back
            x -= direction[0];
            y -= direction[1];
            // turn right
            direction = [direction[1], -1 * direction[0]];
        }
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            break;
        }
    }    
    return result;
}

int GetDistinctLoopObstaclePositionsCount(int[,] mat, int x, int y, int[] direction)
{
    int result = 0;
    for (int i = 0; i < Width; i++)
    {
        for (int j = 0; j < Height; j++)
        {
            if (mat[i, j] != -1 && !(i == x && j == y))
            {
                mat[i, j] = -1;
                if (IsLoopDetected(CopyMatrix(mat), x, y, [-1, 0]))
                {
                    result++;
                }
                mat[i, j] = 0;
            }
        }
    }

    return result;
}

bool IsLoopDetected(int[,] mat, int x, int y, int[] direction)
{
    while (true)
    {
        if (mat[x, y] == 0) // not visited
        {
            result++;
            mat[x, y] = 1;
            x += direction[0];
            y += direction[1];
        }
        else if (mat[x, y] >= 1) // visited
        {
            mat[x, y]++;
            if (mat[x, y] == 10) // Assume loop detected
            {
                return true;
            }
            x += direction[0];
            y += direction[1];            
        }
        else if (mat[x, y] == -1) // obstacle
        {
            // get one step back
            x -= direction[0];
            y -= direction[1];
            // turn right
            direction = [direction[1], -1 * direction[0]];
        }
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            break;
        }
    }
    return false;
}

int[,] CopyMatrix(int[,] mat)
{
    int[,] copy = new int[Width, Height];
    for (int i = 0; i < Width; i++)
    {
        for (int j = 0; j < Height; j++)
        {
            copy[i, j] = mat[i, j];
        }
    }
    return copy;
}

int[,] ParseInputToMatrix(string[] lines, out int x, out int y)
{
    int[,] mat = new int[Width, Height];
    x = 0;
    y = 0;
    for (int i = 0; i < lines.Length; i++)
    {
        for (int j = 0; j < lines[i].Length; j++)
        {
            switch (lines[i][j])
            {
                case '.':
                    mat[i, j] = 0;
                    break;
                case '#':
                    mat[i, j] = -1;
                    break;
                case '^':
                    mat[i, j] = 0;
                    x = i;
                    y = j; // starting point
                    break;
                default:
                    throw new Exception("Invalid character");
            }
        }
    }
    return mat;
}