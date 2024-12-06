
using System.Diagnostics;
using System.Text.RegularExpressions;

const string filePath = "input.txt";
const string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

var reports = new List<List<int>>();
var lines = File.ReadAllLines(filePath);

var stopwatch = Stopwatch.StartNew();
var result = lines.Sum(line => CalculateInstructionsResult(line));
stopwatch.Stop();
Console.WriteLine($"CalculateInstructionsResult. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");
Console.ReadKey();

long CalculateInstructionsResult(string line)
{
    long result = 0;
    var matches = Regex.Matches(line, pattern);
    foreach (Match match in matches)
    {
        int num1 = int.Parse(match.Groups[1].Value); 
        int num2 = int.Parse(match.Groups[2].Value);
        result += num1 * num2;
    }
    return result;
}