using System.Diagnostics;
using System.Text.RegularExpressions;

const string filePath = "input.txt";
const string patternMul = @"mul\((\d{1,3}),(\d{1,3})\)";
const string patternMulOrDos = @"(mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\))";

var reports = new List<List<int>>();
var lines = File.ReadAllLines(filePath);

var stopwatch = Stopwatch.StartNew();
var result = lines.Sum(line => CalculateInstructionsResult(line));
stopwatch.Stop();
Console.WriteLine($"CalculateInstructionsResult. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");

stopwatch.Restart();
result = CalculateInstructionsResultWithDos(lines);
stopwatch.Stop();
Console.WriteLine($"CalculateInstructionsResultWithDos. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");
Console.ReadKey();  

long CalculateInstructionsResult(string line)
{
    long result = 0;
    var matches = Regex.Matches(line, patternMul);
    foreach (Match match in matches)
    {
        int num1 = int.Parse(match.Groups[1].Value); 
        int num2 = int.Parse(match.Groups[2].Value);
        result += num1 * num2;
    }
    return result;
}

long CalculateInstructionsResultWithDos(string[] lines)
{
    long result = 0;
    bool doIt = true;
    foreach (string line in lines)
    {
        var matches = Regex.Matches(line, patternMulOrDos);
        foreach (Match match in matches)
        {
            if (match.Groups[0].Value.Equals("do()"))
            {
                doIt = true;
                continue;
            }
            if (match.Groups[0].Value.Equals("don't()"))
            {
                doIt = false;
                continue;
            }
            if (doIt)
            {
                int num1 = int.Parse(match.Groups[2].Value);
                int num2 = int.Parse(match.Groups[3].Value);
                result += num1 * num2;
            }
        }
    }       
    return result;
}