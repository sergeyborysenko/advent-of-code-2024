using System.Diagnostics;

const string filePath = "input.txt";
const int Width = 50;
const int Height = 50;

var mat = new char[Height, Width];
var moves = new Queue<char>();

var lines = File.ReadAllLines(filePath);
var robotPosition = ParseInput(lines, mat, moves);

var stopwatch = Stopwatch.StartNew();

DoAllMoves(mat, robotPosition, moves);
var result = SumOfGPSCoordinates(mat);

stopwatch.Stop();
Console.WriteLine($"{nameof(SumOfGPSCoordinates)}. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");

Console.ReadKey();

(int X, int Y) ParseInput(string[] lines, char[,] mat, Queue<char> moves)
{
    (int X, int Y) robotPosition = (0, 0);
    for (int i = 0; i < Height; i++)
    {
        for (int j = 0; j < Width; j++)
        {
            mat[i, j] = lines[i][j];
            if (mat[i, j] == '@')
            {
                robotPosition = (i, j);
            }
        }
    }

    // skip empty line
    for (int i = Height + 1; i < lines.Length; i++)
    {
        foreach (var move in lines[i])
        {
            moves.Enqueue(move);
        }
    }

    return robotPosition;
}


long SumOfGPSCoordinates(char[,] mat)
{
    long sum = 0;
    for (int i = 0; i < Height; i++)
    {
        for (int j = 0; j < Width; j++)
        {
            if (mat[i, j] == 'O')
            {
                sum += CalculateGpsCoordinates(i, j);
            }
        }
    }
    return sum;
}

long CalculateGpsCoordinates(int i, int j)
{
    return 100 * i + j;
}

void DoAllMoves(char[,] mat, (int X, int Y) robotPosition, Queue<char> moves)
{
    while (moves.Count > 0)
    {
        var move = moves.Dequeue();
        var direction = move switch
        {
            '^' => (-1, 0),
            'v' => (1, 0),
            '<' => (0, -1),
            '>' => (0, 1),
            _ => (0, 0)
        };
        robotPosition = MoveRobot(mat, robotPosition, direction);
        //PrintMatrix(mat, move);
    }
}

void PrintMatrix(char[,] mat, char move)
{
    Console.WriteLine($"Move '{move}':");
    for (int i = 0; i < Height; i++)
    {
        for (int j = 0; j < Width; j++)
        {
            Console.Write(mat[i, j]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

(int X, int Y) MoveRobot(char[,] mat, (int X, int Y) robotPosition, (int Dx, int Dy) direction)
{
    var (X, Y) = robotPosition;
    var nextX = X + direction.Dx;
    var nextY = Y + direction.Dy;
    if (nextX < 0 || nextX >= Height || nextY < 0 || nextY >= Width)
    {
        return robotPosition;
    }    
    else if (mat[nextX, nextY] == '#')
    {
        return robotPosition;
    }
    else if (mat[nextX, nextY] == '.')
    {
        mat[X, Y] = '.';
        mat[nextX, nextY] = '@';
        return (nextX, nextY);
    }
    else if (mat[nextX, nextY] == 'O')
    {
        // find if we can move all boxes in the direction
        while(mat[nextX, nextY] == 'O')
        {
            nextX += direction.Dx;
            nextY += direction.Dy;
        }

        // can't move, return current position
        if (mat[nextX, nextY] == '#')
            return robotPosition;

        //move all boxes in the direction
        do
        {
            mat[nextX, nextY] = 'O';
            nextX -= direction.Dx;
            nextY -= direction.Dy;
        } while (nextX != robotPosition.X || nextY != robotPosition.Y);

        // move robot
        mat[X, Y] = '.';
        robotPosition = (X + direction.Dx, Y + direction.Dy);
        mat[robotPosition.X, robotPosition.Y] = '@';
    }

    return robotPosition;
}