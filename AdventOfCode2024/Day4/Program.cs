using System.Diagnostics;

const string filePath = "input.txt";
const int Width = 140;
const int Height = 140;

char[,] mat = new char[Width, Height];
char[] word = ['X', 'M', 'A', 'S'];

var lines = File.ReadAllLines(filePath);
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        mat[i, j] = lines[i][j] switch
        {
            'A' => 'A',
            'M' => 'M',
            'S' => 'S',
            'X' => 'X',
            _ => '.'
        };
    }
}
var stopwatch = Stopwatch.StartNew();
var result = CalculatexXmasWords(mat);
stopwatch.Stop();
Console.WriteLine($"{nameof(CalculatexXmasWords)}. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");

stopwatch.Restart();
result = CalculatexMasXPatterns(mat);
stopwatch.Stop();
Console.WriteLine($"{nameof(CalculatexXmasWords)}. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");


Console.ReadKey();

int CalculatexXmasWords(char[,] mat)
{
    int result = 0;
    for (int i = 0; i < mat.GetLength(0); i++)
    {
        for (int j = 0; j < mat.GetLength(1); j++)
        {
            if (mat[i, j] == word[0]/*'X'*/)
            {
                result += FindPossibleXmasWord(mat, i, j);
            }
        }
    }
    return result;
}

int FindPossibleXmasWord(char[,] mat, int i, int j)
{
    int result = 0;
    result += FindXmasWordByDirection(mat, i, j, 1, -1, -1);
    result += FindXmasWordByDirection(mat, i, j, 1, 0, -1);
    result += FindXmasWordByDirection(mat, i, j, 1, 1, -1);
    result += FindXmasWordByDirection(mat, i, j, 1, 1, 0);
    result += FindXmasWordByDirection(mat, i, j, 1, 1, 1);
    result += FindXmasWordByDirection(mat, i, j, 1, 0, 1);
    result += FindXmasWordByDirection(mat, i, j, 1, -1, 1);
    result += FindXmasWordByDirection(mat, i, j, 1, -1, 0);
    return result;
}

int FindXmasWordByDirection(char[,] mat, int i, int j, int charInd, int di, int dj)
{
    if (charInd == word.Length)
    {
        return 1;
    }

    if (i + di >= 0 && i + di < Width && j + dj >= 0 && j + dj < Height)
    {
        if (mat[i + di, j + dj] == word[charInd])
        {
            return FindXmasWordByDirection(mat, i + di, j + dj, charInd+1, di, dj);
        }
    }
    return 0;
}

int CalculatexMasXPatterns(char[,] mat)
{
    int result = 0;
    for (int i = 0; i < mat.GetLength(0); i++)
    {
        for (int j = 0; j < mat.GetLength(1); j++)
        {
            if (mat[i, j] == word[2]/*'A'*/)
            {
                result += IsСenterOfMasXPattern(mat, i, j);
            }
        }
    }
    return result;
}

int IsСenterOfMasXPattern(char[,] mat, int i, int j)
{
    if (i == 0 || j == 0 || i == Width - 1 || j == Height -1)
        return 0;
    if ((mat[i-1,j-1] == word[1] && mat[i+1, j+1] == word[3]) ||
        (mat[i - 1, j - 1] == word[3] && mat[i + 1, j + 1] == word[1]))
    {
        if ((mat[i - 1, j + 1] == word[1] && mat[i + 1, j - 1] == word[3]) ||
        (mat[i - 1, j + 1] == word[3] && mat[i + 1, j - 1] == word[1]))
            return 1;
    }
    return 0;
}