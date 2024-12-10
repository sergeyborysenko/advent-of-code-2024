using System.Diagnostics;

const string filePath = "input.txt";
const int Width = 130;
const int Height = 130;

int[,] mat = new int[Width, Height];

var lines = File.ReadAllLines(filePath);

var x = 0;
var y = 0;
int[] direction = [-1, 0]; // up

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
                mat[i, j] = 2;
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

var stopwatch = Stopwatch.StartNew();
var result = GetDistinctPositionsCount(mat, x, y, direction);
stopwatch.Stop();
Console.WriteLine($"{nameof(GetDistinctPositionsCount)}. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");

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
        else if (mat[x,y] == 1) // visited
        {
            x += direction[0];
            y += direction[1];
        }
        else if (mat[x,y] == 2) // obstacle
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