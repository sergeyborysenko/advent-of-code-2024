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
Console.WriteLine($"GetDistanceSum. Result: {sum}; Execution Time: {stopwatch.ElapsedTicks} ticks");

stopwatch.Restart();
var score = GetSimilarityScore(list1, list2);
stopwatch.Stop();
Console.WriteLine($"GetSimilarityScore. Result: {score}; Execution Time: {stopwatch.ElapsedTicks} ticks");


stopwatch.Restart();
score = GetSimilarityScoreOptimized(list1, list2);
stopwatch.Stop();
Console.WriteLine($"GetSimilarityScoreOptimized. Result: {score}; Execution Time: {stopwatch.ElapsedTicks} ticks");

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

static int GetSimilarityScore(List<int> list1, List<int> list2)
{
    var score = 0;
    for (int i = 0; i < list1.Count; i++)
    {
        score += list2.Count(l2 => l2 == list1[i]) * list1[i];
    }
    return score;
}

static int GetSimilarityScoreOptimized(List<int> list1, List<int> list2)
{
    var score = 0;
    Dictionary<int, int> dict = new();
    for (int i = 0; i < list1.Count; i++)
    {
        if (!dict.ContainsKey(list1[i]))
        {
            dict.Add(list1[i], list2.Count(l2 => l2 == list1[i]) * list1[i]);
        }
        score += dict[list1[i]];
    }
    return score;
}