using AdventOfCode24.Days;

namespace AdventOfCode.Days
{
    internal class Day7 : DayBase
    {
        public override void Start()
        {
            var input = ReadFile("Day7.txt");

            var availableOperators = new string[] { "*", "+" };

            var sumOfValidEquations = 0;

            foreach (var equation in input) 
            {
                var line = equation.Split(':');
                var target = int.Parse(line[0]);
                var rightPartNumbers = line[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();

                for (var i = 0; i < rightPartNumbers.Length + 1; i++)
                {
                    var firstSet = rightPartNumbers.Take(i);
                    var secondSet = rightPartNumbers.Skip(i);

                    var result = secondSet.Any() 
                        ? firstSet.Sum(x => x) + secondSet.Aggregate((x, y) => x * y)
                        : firstSet.Sum(x => x);
                    if(result == target)
                    {
                        sumOfValidEquations += result;
                        break;
                    }
                }
            }

            Console.WriteLine($"Sum of all correct calibrations: {sumOfValidEquations}");
        }
    }
}
