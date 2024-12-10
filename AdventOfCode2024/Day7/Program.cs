using System.Diagnostics;
using System.Globalization;

class Operator
{
    public Func<long, long, long>? Operation { get; set; }

    public static Operator Sum = new() { Operation = (a, b) => a + b };
    public static Operator Mul = new() { Operation = (a, b) => a * b };
}

class Program
{
    const string filePath = "input.txt";
    static void Main()
    {
        var lines = File.ReadAllLines(filePath);

        var rows = ParseInputToRows(lines);

        var stopwatch = Stopwatch.StartNew();
        var result = GetSumOfCalibratedRows(rows);
        stopwatch.Stop();
        Console.WriteLine($"{nameof(GetSumOfCalibratedRows)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
    }

    static long GetSumOfCalibratedRows(IEnumerable<(long Result, List<int> Numbers)> rows)
    {
        long result = 0;
        foreach (var row in rows)
        {
            if (CanBeCalibrated(row))
            {
                result += row.Result;
            }
        }
        return result;
    }

    static bool CanBeCalibrated((long Result, List<int> Numbers) row)
    {
        var result = false;
        var combinations = GetMatrixOfOperatorCombinations(row.Numbers.Count);
        foreach (var combination in combinations)
        {
            long currentResult = row.Numbers[0];
            for (int i = 1; i < row.Numbers.Count; i++)
            {
                currentResult = combination[i - 1].Operation!(currentResult, row.Numbers[i]);
            }
            if (currentResult == row.Result)
            {
                result = true;
                break;
            }
        }
        return result;
    }

    static IEnumerable<(long, List<int>)> ParseInputToRows(string[] lines)
    {
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            var result = long.Parse(parts[0]);
            var numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            yield return (result, numbers);
        }
    }

    static List<List<Operator>> GetMatrixOfOperatorCombinations(int countOfNumbers)
    {
        var results = new List<List<Operator>>();
        GenerateCombinationsRecursive([Operator.Sum, Operator.Mul], [], countOfNumbers -1, results);
        return results;
    }

    static void GenerateCombinationsRecursive(Operator[] operators, List<Operator> current, int positions, List<List<Operator>> results)
    {
        if (current.Count == positions)
        {
            results.Add(new List<Operator>(current));
            return;
        }

        foreach (var op in operators)
        {
            current.Add(op);
            GenerateCombinationsRecursive(operators, current, positions, results);
            current.RemoveAt(current.Count - 1); // Backtrack
        }
    }
}
