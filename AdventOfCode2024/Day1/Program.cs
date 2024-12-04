using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

string filePath = "input.txt";
var list1 = new List<int>();
var list2 = new List<int>();
var lines = File.ReadAllLines(filePath);
lines.ToList().ForEach(line =>
{
    var parts = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
    list1.Add(int.Parse(parts[0]));
    list2.Add(int.Parse(parts[1]));
});

var stopwatch = Stopwatch.StartNew();
var sum = GetDistanceSum(list1, list2);
stopwatch.Stop();
Console.WriteLine($"Result: {sum}; Execution Time: {stopwatch.ElapsedTicks} ticks");

static int GetDistanceSum(List<int> list1, List<int> list2)
{
    list1.Sort();
    list2.Sort();
    var sum = 0;
    for (int i = 0; i < list1.Count; i++)
    {
        sum += Math.Abs(list1[i] - list2[i]);
    }
    return sum;
}