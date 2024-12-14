using System.Diagnostics;

const string filePath = "input.txt";

var lines = File.ReadAllLines(filePath);
var stopwatch = Stopwatch.StartNew();

var newStones = GetStonesAfter25Blinks(lines.First().Split(' ').Select(long.Parse).ToList());

stopwatch.Stop();
Console.WriteLine($"{nameof(GetStonesAfter25Blinks)}. Result: {newStones.Count}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

stopwatch.Restart();
Dictionary<(long, int), long> cache = [];
var count = CalculateNumberOfStones(lines.First().Split(' ').Select(long.Parse).ToList(), 75, cache);

stopwatch.Stop();
Console.WriteLine($"{nameof(CalculateNumberOfStones)}. Result: {count}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
Console.ReadKey();

// Bruteforce solution
List<long> GetStonesAfter25Blinks(List<long> stones)
{
    for(int i = 0; i < 25; i++)
    {
        stones = GetStonesAfterChange(stones);
    }

    return stones;
}

List<long> GetStonesAfterChange(List<long> stones)
{
    int i = 0;
    while (i < stones.Count)
    {
        var newStones = GetStonesAfterOneBlink(stones[i]);
        stones[i] = newStones[0];
        i++;
        if (newStones.Count == 2)
        {
            stones.InsertRange(i, newStones.Skip(1));
            i++;
        }
    }
    return stones;
}

List<long> GetStonesAfterOneBlink(long stone)
{
    int digits = stone == 0 ? 1 : (int)Math.Floor(Math.Log10(Math.Abs(stone)) + 1);
    if (digits % 2 == 0)
    {
        var stoneStr = stone.ToString();
        var number1 = long.Parse(stoneStr[..(stoneStr.Length / 2)]);
        var number2 = long.Parse(stoneStr[(stoneStr.Length / 2)..]);
        return [number1, number2];
    }
    else if (stone == 0)
    {
        return [1];
    }
    else
    {
        return[stone * 2024];
    }
}

long CalculateNumberOfStones(List<long> stones, int blinks, Dictionary<(long, int), long> cache)
{ 
    if (blinks == 0)
    {
        return stones.Count;
    }
    long count = 0;
    foreach(var stone in stones)
    {
        if (cache.TryGetValue((stone, blinks), out var cachedCount))
        {
            count += cachedCount;
            continue;
        }
        else
        {
            long singleStoneCount = CalculateNumberOfStones(GetStonesAfterOneBlink(stone), blinks - 1, cache);
            count += singleStoneCount;
            cache[(stone, blinks)] = singleStoneCount;
        }
    }
    return count;
}
