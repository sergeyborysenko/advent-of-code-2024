using System.Diagnostics;
using System.Text.RegularExpressions;

const string filePath = "input.txt";

char[,] mat = new char[140, 140];
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
Console.ReadKey();

int CalculatexXmasWords(char[,] mat)
{
    int result = 0;
    for (int i = 0; i < mat.GetLength(0); i++)
    {
        for (int j = 0; j < mat.GetLength(1); j++)
        {
            if (mat[i, j] == word[0])
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

    if (i + di >= 0 && i + di < 140 && j + dj >= 0 && j + dj < 140)
    {
        if (mat[i + di, j + dj] == word[charInd])
        {
            return FindXmasWordByDirection(mat, i + di, j + dj, charInd+1, di, dj);
        }
    }
    return 0;
}
