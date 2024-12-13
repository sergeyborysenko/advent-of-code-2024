using System.Diagnostics;

const string filePath = "input.txt";

var lines = File.ReadAllLines(filePath);

var stopwatch = Stopwatch.StartNew();

var result = CalculateNumberOfStones(lines.First(), 25);

stopwatch.Stop();
Console.WriteLine($"{nameof(CalculateNumberOfStones)} 25 times. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
Console.ReadKey();

int CalculateNumberOfStones(string line, int times)
{
    var stones = line.Split(' ').Select(long.Parse).ToList();
    for(int i = 0; i < times; i++)
    {
        stones = GetStonesAfterChange(stones);
    }
    return stones.Count;
}

List<long> GetStonesAfterChange(List<long> stones)
{
    int i = 0;
    while (i < stones.Count)
    {
        var stoneStr = stones[i].ToString();
        if (stoneStr.Length % 2 == 0)
        {
            var number1 = long.Parse(stoneStr.Substring(0, stoneStr.Length / 2));
            var number2 = long.Parse(stoneStr.Substring(stoneStr.Length / 2));
            stones[i] = number1;
            stones.Insert(i + 1, number2);
            i += 2;
        }
        else if (stones[i] == 0)
        {
            stones[i] = 1;
            i++;
        }
        else
        {
            stones[i] *= 2024;
            i++;
        }
    }
    return stones;
}