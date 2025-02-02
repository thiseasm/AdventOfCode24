﻿using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    internal class Day3 : DayBase
    {
        public override void Start()
        {
            var input = ReadText("Day3.txt");

            const string commandPattern = @"(mul[(]\d{1,3}[,]\d{1,3}[)])";

            var result = CalculateFirstPart(input, commandPattern);

            Console.WriteLine($"Sum of all results: {result}");

            CalculateSecondPart(input, commandPattern);
        }

        private static void CalculateSecondPart(string input, string commandPattern)
        {
            const string invalidCommandSectionPattern = @"((don't\(\))((.|\n)*?))(?=(don't\(\))|(do\(\))|$)";
            input = Regex.Replace(input, invalidCommandSectionPattern, string.Empty);

            var result = 0;
            foreach (Match command in Regex.Matches(input, commandPattern))
            {
                var extractedNumbers = command.Value
                    .Replace("mul(", "")
                    .Replace(")", "")
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();

                result += extractedNumbers[0] * extractedNumbers[1];
            }

            Console.WriteLine($"Sum of all valid command results: {result}");
        }

        private static int CalculateFirstPart(string input, string commandPattern)
        {
            var result = 0;

            foreach (Match command in Regex.Matches(input, commandPattern))
            {
                var extractedNumbers = command.Value
                    .Replace("mul(", "")
                    .Replace(")", "")
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();

                result += extractedNumbers[0] * extractedNumbers[1];
            }

            return result;
        }
    }
}
