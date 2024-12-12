using System;
using System.Diagnostics;
using System.Numerics;
using System.Linq;

const string filePath = "input.txt";

var lines = File.ReadAllLines(filePath);

var stopwatch = Stopwatch.StartNew();
var result = GetFilesystemChecksum(lines.First(), true);
stopwatch.Stop();
Console.WriteLine($"Optimize by moving blocks. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");

stopwatch.Restart();
result = GetFilesystemChecksum(lines.First(), false);
stopwatch.Stop();
Console.WriteLine($"Optimize by moving files. Result: {result}; Execution Time: {stopwatch.ElapsedMilliseconds} miliseconds");
Console.ReadKey();

long GetFilesystemChecksum(string line, bool moveByBlocks)
{
    var fileSystem = ParseDiskmap(line);
    var optimizedFileSystem = moveByBlocks ? OptimizeByBlocks(fileSystem) : OptimizeByWholeFiles(fileSystem);
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

List<int> OptimizeByBlocks(List<int> fileSystem)
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

List<int> OptimizeByWholeFiles(List<int> fileSystem)
{
    var rightInd = fileSystem.Count - 1;
    while (rightInd > 0)
    {
        rightInd = SeekNextFileWithHighestId(fileSystem, rightInd);
        var fileSize = GetNextFileSize(fileSystem, rightInd);
        var freeSpaceIndex = SeekSuitedFreeSpaceIndex(fileSystem, rightInd, fileSize);
        if (freeSpaceIndex != -1)
        {
            for (int i = 0; i < fileSize; i++)
            {
                fileSystem[freeSpaceIndex + i] = fileSystem[rightInd - i];
                fileSystem[rightInd - i] = -1;
            }
        }
        rightInd -= fileSize;
    }
    return fileSystem;
}
int SeekSuitedFreeSpaceIndex(List<int> fileSystem, int rightIndex, int fileSize)
{
    var leftIndex = 0;
    while (leftIndex < rightIndex)
    {
        var nextFreeSpaceIndex = SeekNextFreeSpace(fileSystem, leftIndex);
        var freeSpaceSize = GetFreeSpaceSize(fileSystem, nextFreeSpaceIndex);
        if (freeSpaceSize >= fileSize && nextFreeSpaceIndex < rightIndex)
        {
            return nextFreeSpaceIndex;
        }
        leftIndex = nextFreeSpaceIndex + freeSpaceSize;
    }
    return -1;
}
int SeekNextFreeSpace(List<int> fileSystem, int index)
{
    while (index < fileSystem.Count && fileSystem[index] != -1)
        index++;
    return index;
}
int SeekNextFileWithHighestId(List<int> fileSystem, int index)
{
    while (index < fileSystem.Count && fileSystem[index] == -1)
        index--;
    return index;
}

int GetFreeSpaceSize(List<int> fileSystem, int index)
{
    var freeSpace = 0;
    while (index < fileSystem.Count && fileSystem[index] == -1)
    {
        freeSpace++;
        index++;
    }
    return freeSpace;
}
int GetNextFileSize(List<int> fileSystem, int index)
{
    var fileSize = 0;
    var fileId = fileSystem[index];
    while (index >= 0 && fileSystem[index] == fileId)
    {
        fileSize++;
        index--;
    }
    return fileSize;
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