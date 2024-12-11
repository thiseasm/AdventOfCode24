namespace AdventOfCode.Days;

public class Day11 : DayBase
{
    public override void Start()
    {
        var input = ReadText("Day11.txt.");

        var stones = input.Split(" ").Select(long.Parse).ToArray();

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