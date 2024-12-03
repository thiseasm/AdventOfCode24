using AdventOfCode24.Days;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    internal class Day3 : DayBase
    {
        public override void Start()
        {
            var input = ReadText("Day3.txt");

            const string pattern = @"(mul[(]\d{1,3}[,]\d{1,3}[)])";

            var result = 0;

            foreach (Match command in Regex.Matches(input, pattern))
            {
                var extractedNumbers = command.Value
                    .Replace("mul(", "")
                    .Replace(")", "")
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();

                result += extractedNumbers[0] * extractedNumbers[1];
            }

            Console.WriteLine($"Sum of all results: {result}");


        }
    }
}
