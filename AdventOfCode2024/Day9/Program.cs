using System;
using System.Diagnostics;
using System.Numerics;
using System.Linq;

const string filePath = "input.txt";

var lines = File.ReadAllLines(filePath);

var stopwatch = Stopwatch.StartNew();
var result = GetFilesystemChecksum(lines.First());
stopwatch.Stop();
Console.WriteLine($"{nameof(GetFilesystemChecksum)}. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
Console.ReadKey();

long GetFilesystemChecksum(string line)
{
    var fileSystem = ParseDiskmap(line);
    var optimizedFileSystem = OptimizeFileSystem(fileSystem);
    return CalculateChecksum(optimizedFileSystem);
}

long CalculateChecksum(List<int> optimizedFileSystem)
{
    long checksum = 0;
    for(int i = 0; i < optimizedFileSystem.Count; i++)
    {
        if (optimizedFileSystem[i] == -1)
            continue;
        checksum += i * optimizedFileSystem[i];
    }
    return checksum;
}

List<int> OptimizeFileSystem(List<int> fileSystem)
{
    var leftInd = 0;
    var rightInd = fileSystem.Count - 1;
    while (leftInd <= rightInd)
    {
        while(fileSystem[leftInd] != -1)
            leftInd++;
        while (fileSystem[rightInd] == -1)
            rightInd--;
        if (leftInd <= rightInd)
        {
            fileSystem[leftInd] = fileSystem[rightInd];
            fileSystem[rightInd] = -1;
        }
    }
    return fileSystem;
}

List<int> ParseDiskmap(string line)
{
    var diskmap = new List<int>();
    var fileId = 0;
    var fileBlock = true;
    for (var i = 0; i < line.Length; i ++)
    {
        diskmap.AddRange(Enumerable.Repeat(fileBlock ? fileId++ : -1, line[i] - '0'));        
        fileBlock = !fileBlock;
    }
    return diskmap;
}