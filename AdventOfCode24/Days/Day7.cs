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
                var targetAmount = int.Parse(line[0]);
                var rightPartNumbers = line[1].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToArray();

                var operationScenarios = new List<Queue<char>>();
                
                
            }

            Console.WriteLine($"Sum of all correct calibrations: {sumOfValidEquations}");
        }
    }
}
