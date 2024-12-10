using System.Diagnostics;
using System.Globalization;
using System.Numerics;

class Operator
{
    public Func<long, long, long>? Operation { get; set; }

    public static Operator Sum = new() { Operation = (a, b) => a + b };
    public static Operator Mul = new() { Operation = (a, b) => a * b };
    public static Operator Conc = new() { Operation = (a, b) => long.Parse(a.ToString() + b.ToString()) };
}

class Program
{
    const string filePath = "input.txt";
    static void Main()
    {
        var lines = File.ReadAllLines(filePath);
        var rows = ParseInputToRows(lines);

        var stopwatch = Stopwatch.StartNew();
        var result = GetSumOfCalibratedRows(rows, [Operator.Sum, Operator.Mul]);
        stopwatch.Stop();
        Console.WriteLine($"{nameof(GetSumOfCalibratedRows)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
        
        stopwatch.Restart();
        result = GetSumOfCalibratedRows(rows, [Operator.Sum, Operator.Mul, Operator.Conc]);
        stopwatch.Stop();
        Console.WriteLine($"{nameof(GetSumOfCalibratedRows)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
    }

    static long GetSumOfCalibratedRows(IEnumerable<(long Result, List<int> Numbers)> rows, Operator[] operators)
    {
        long result = 0;
        foreach (var row in rows)
        {
            if (CanBeCalibrated(row, operators))
            {
                result += row.Result;
            }
        }
        return result;
    }

    static bool CanBeCalibrated((long Result, List<int> Numbers) row, Operator[] operators)
    {
        var result = false;
        var combinations = GetMatrixOfOperatorCombinations(row.Numbers.Count, operators);
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

    static List<List<Operator>> GetMatrixOfOperatorCombinations(int countOfNumbers, Operator[] operators)
    {
        var results = new List<List<Operator>>();
        GenerateCombinationsRecursive(operators, [], countOfNumbers -1, results);
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
