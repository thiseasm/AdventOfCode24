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

        var stonesToExamine = new Queue<long>();
        foreach (var stone in stones)
        {
            stonesToExamine.Enqueue(stone);
        }

        var numberOfStones = stonesToExamine.Count;
        var timesBlinked = 0;
        while (timesBlinked < 75 )
        {
            Console.WriteLine(timesBlinked);
            var generatedStones = new List<long>();
            while (stonesToExamine.Count != 0)
            {
                var stone = stonesToExamine.Dequeue();
                
                if (Cache.Keys.Contains($"{stone}"))
                {
                    var result = (List<long>)Cache.Get($"{stone}")!;
                    if (result.Count > 1)
                    {
                        numberOfStones++;
                    }
                    generatedStones.AddRange(result);
                }
                else
                {
                    var key = $"{stone}";
                    if (stone == 0)
                    {
                        stone++;
                        Cache.Set(key, new List<long>{stone});
                    }
                    else if (stone.ToString().Length % 2 == 0)
                    {
                        var (firstEngraving, secondEngraving) = SplitEngraving(stone);

                        stone = firstEngraving;
                        Cache.Set(key, new List<long>{firstEngraving, secondEngraving});
                        generatedStones.Add(secondEngraving);
                        numberOfStones++;
                    }
                    else
                    {
                        stone = MultiplyStone(stone);
                        Cache.Set(key, new List<long>{stone});
                    }
                
                    generatedStones.Add(stone);
                }
            }
            
            foreach (var generated in generatedStones)
            {
                stonesToExamine.Enqueue(generated);
            }

            timesBlinked++;
        }
        
        Console.WriteLine($"After blinking even more there are {numberOfStones}");
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