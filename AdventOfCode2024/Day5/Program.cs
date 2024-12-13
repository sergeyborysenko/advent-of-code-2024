using System.Diagnostics;

const string filePath = "input.txt";

var lines = File.ReadAllLines(filePath);

(var orderRules, var updates) = ParseInput(lines);

var stopwatch = Stopwatch.StartNew();
var result = CalculatexSum(orderRules, updates);
stopwatch.Stop();
Console.WriteLine($"{nameof(CalculatexSum)}. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");

stopwatch.Restart();
result = CalculatexSumForCorrectedUpdates(orderRules, updates.Where(update => !IsCorrectlyOrdered(update, orderRules)).ToArray());
stopwatch.Stop();
Console.WriteLine($"{nameof(CalculatexSumForCorrectedUpdates)}. Result: {result}; Execution Time: {stopwatch.ElapsedTicks} ticks");

int CalculatexSum(Dictionary<int, HashSet<int>> orderRules, int[][] updates)
{
    return updates.Where(update => IsCorrectlyOrdered(update, orderRules)).Sum(update => update[update.Length / 2]);
}
int CalculatexSumForCorrectedUpdates(Dictionary<int, HashSet<int>> orderRules, int[][] updates)
{
    return updates.Select(update => CorrectUpdate(update, orderRules)).Sum(corrected => corrected[corrected.Length / 2]);
}
bool IsCorrectlyOrdered(int[] update, Dictionary<int, HashSet<int>> rules)
{
    var visited = new HashSet<int>();
    foreach (var pageNumber in update)
    {
        if (!rules.ContainsKey(pageNumber))
        {
            visited.Add(pageNumber);
            continue;
        }
        var rule = rules[pageNumber];
        if (rule.Any(r => visited.Contains(r)))
        {
            return false;
        }
        else
        {
            visited.Add(pageNumber);
        }
    }
    return true;
}

int[] CorrectUpdate(int[] update, Dictionary<int, HashSet<int>> rules)
{
    var visited = new HashSet<int>();
    var corrected = new List<int>();
    foreach (var pageNumber in update)
    {
        if (!rules.ContainsKey(pageNumber))
        {
            corrected.Add(pageNumber);
            visited.Add(pageNumber);
            continue;
        }
        var rule = rules[pageNumber];
        if (rule.Any(r => visited.Contains(r)))
        {
            var index = corrected.FindIndex(c => rule.Contains(c));
            corrected.Insert(index, pageNumber);
        }
        else
        {
            corrected.Add(pageNumber);
            visited.Add(pageNumber);
        }
    }
    return [.. corrected];
}

(Dictionary<int, HashSet<int>>, int[][]) ParseInput(string[] lines)
{
    var orderRules = new Dictionary<int, HashSet<int>>();
    int i = 0;
    while (i < lines.Length)
    {
        if (string.IsNullOrWhiteSpace(lines[i]))
        {
            break;
        }
        int[] rule = lines[i].Split('|').Select(int.Parse).ToArray();
        if (!orderRules.ContainsKey(rule[0]))
        {
            orderRules[rule[0]] = new HashSet<int>();
        }
        orderRules[rule[0]].Add(rule[1]);
        i++;
    }
    var updates = lines.Skip(i + 1).Select(line => line.Split(',').Select(int.Parse).ToArray()).ToArray();

    return (orderRules, updates);
}