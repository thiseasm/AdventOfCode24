using Microsoft.Extensions.Caching.Memory;

namespace AdventOfCode.Days;

public class Day11 : DayBase
{
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions());
    public override void Start()
    {
        var input = ReadText("Day11.txt.");

        var stones = input.Split(" ").Select(long.Parse).ToArray();
        
        CalculateFor25Rounds(stones);

        var currentState = new Dictionary<long,long>();
        foreach (var stone in stones)
        {
            currentState.Add(stone,1);
        }
        
        var nextState = new Dictionary<long,long>
        {
            { 0, 0 },
            { 1, 0 }
        };
        for(var timesBlinked = 0; timesBlinked < 75; timesBlinked++)
        {
            foreach (var (engraving, count) in currentState)
            {
                if (engraving == 0)
                {
                    nextState[1] += count;
                }
                else if (engraving.ToString().Length % 2 == 0)
                {
                    var (firstEngraving, secondEngraving) = SplitEngraving(engraving);

                    nextState.TryAdd(firstEngraving, 0);
                    nextState[firstEngraving] += count;

                    nextState.TryAdd(secondEngraving, 0);
                    nextState[secondEngraving] += count;
                }
                else
                {
                    var result = MultiplyStone(engraving);
                    nextState.TryAdd(result, 0);
                    nextState[result] += count;
                }
            }

            currentState = nextState;
            nextState = new Dictionary<long,long>
            {
                { 0, 0 },
                { 1, 0 }
            };

        }
        
        Console.WriteLine($"After blinking even more there are {currentState.Values.Sum()}");
    }

    private static long MultiplyStone(long stone)
    {
        if (Cache.Keys.Contains($"m_{stone}"))
        {
            return (long)Cache.Get($"m_{stone}")!;
        }
        var result = stone * 2024;
        Cache.Set($"s_{stone}", result);
        return result;
    }

    private static (long firstEngraving, long secondEngraving) SplitEngraving(long stone)
    {
        if (Cache.Keys.Contains($"s_{stone}"))
        {
            return ((long, long))Cache.Get($"s_{stone}")!;
        }
        
        var engraving = stone.ToString();
        var firstEngraving = long.Parse(new string(engraving.Take(engraving.Length / 2).ToArray()));
        var secondEngraving = long.Parse(new string(engraving.Skip(engraving.Length / 2).ToArray()));
        Cache.Set($"s_{stone}", (firstEngraving, secondEngraving));
        return (firstEngraving, secondEngraving);
    }

    private static void CalculateFor25Rounds(long[] startingStones)
    {
        var stones = (long[])startingStones.Clone();
        var timesBlinked = 0;
        while (timesBlinked < 25)
        {
            var stonesFinalized = new List<long>();

            foreach (var stone in stones)
            {
                if (stone == 0)
                {
                    stonesFinalized.Add(1);
                }
                else if (stone.ToString().Length % 2 == 0)
                {
                    var engraving = stone.ToString();
                    var firstEngraving = long.Parse(new string(engraving.Take(engraving.Length / 2).ToArray()!));
                    var secondEngraving = long.Parse(engraving.Skip(engraving.Length / 2).ToArray()!);

                    stonesFinalized.AddRange([firstEngraving,secondEngraving]);
                }
                else
                {
                    stonesFinalized.Add(stone * 2024);
                }
            }

            stones = stonesFinalized.ToArray();
            timesBlinked++;
        }
        
        Console.WriteLine($"After blinking enough there are {stones.Length}");
    }
}