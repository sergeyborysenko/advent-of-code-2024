
using System.Diagnostics;
using System.Linq;
using Xunit;

string filePath = "input.txt";
const int minAdj = 1;
const int maxAdj = 3;

var reports= new List<List<int>>();
var lines = File.ReadAllLines(filePath);
lines.ToList().ForEach(line =>
{
    var levels = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
    reports.Add(levels);
});

var stopwatch = Stopwatch.StartNew();
var count = CountSafeReports(reports, minAdj, maxAdj, false);
stopwatch.Stop();
Console.WriteLine($"CountSafeReports. Result: {count}; Execution Time: {stopwatch.ElapsedTicks} ticks");

stopwatch.Restart();
count = CountSafeReports(reports, minAdj, maxAdj, true);
stopwatch.Stop();
Console.WriteLine($"CountSafeReports with Problem Dampener. Result: {count}; Execution Time: {stopwatch.ElapsedTicks} ticks");

static int CountSafeReports(List<List<int>> reports, int minAdj, int maxAdj, bool withDampener)
{
    return withDampener ?
        reports.Count(r => IsSafeReportWithDampener(r, minAdj, maxAdj)) :
        reports.Count( r => IsSafeReport(r, minAdj, maxAdj));
}

static bool IsSafeReport(List<int> r, int minAdj, int maxAdj)
{
    if (r.Count < 2)
    {
        return true;    
    }
    bool isAsc = r[0] < r[1];
    var i = 1;
    while (i < r.Count)
    {
        var diff = r[i] - r[i - 1];
        if (diff < 0 && isAsc || diff > 0 && !isAsc)
        {
            return false;
        }
        if (Math.Abs(diff) < minAdj || Math.Abs(diff) > maxAdj)
        {
            return false;
        }
        i++;
    }
    return true;
}

static bool IsSafeReportWithDampener(List<int> r, int minAdj, int maxAdj)
{
    if (r.Count < 2)
    {
        return true;
    }
    bool isAsc = r[0] <= r[1];
    var i = 1;
    while (i < r.Count)
    {
        var diff = r[i] - r[i - 1];
        if (diff < 0 && isAsc || diff > 0 && !isAsc)
        {
            return (CanBeMadeSafeByRemovingOne(r, i, minAdj, maxAdj) ||
                CanBeMadeSafeByRemovingOne(r, i - 1, minAdj, maxAdj) ||
                // Edge case when direction is wrong from the start
                (i == 2 && CanBeMadeSafeByRemovingOne(r, 0, minAdj, maxAdj)));            
        }
        if (Math.Abs(diff) < minAdj || Math.Abs(diff) > maxAdj)
        {
            return (CanBeMadeSafeByRemovingOne(r, i, minAdj, maxAdj) ||
                CanBeMadeSafeByRemovingOne(r, i - 1, minAdj, maxAdj));
        }
        i++;
    }
    return true;
}

static bool CanBeMadeSafeByRemovingOne(List<int> r, int index, int minAdj, int maxAdj)
{
    var copy = r.ToList();
    copy.RemoveAt(index);
    return IsSafeReport(copy, minAdj, maxAdj);
}