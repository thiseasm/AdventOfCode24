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

                var operationScenarios = new List<Queue<string>>();
                var availableSpaces = rightPartNumbers.Length - 1;
                var possibleCombinationCount = availableSpaces * 2;

                for (var counter = 0; counter < possibleCombinationCount; counter++)
                {
                    var operationQueue = new Queue<string>();
                    for (var multiplicationScenarios = 0; multiplicationScenarios < availableSpaces / 2; multiplicationScenarios++)
                    {
                        operationQueue.Enqueue(availableOperators[0]);
                    }
                    
                    operationScenarios.Add(operationQueue);
                }

                foreach (var scenario in operationScenarios)
                {
                    var numbers = new Queue<int>();
                    foreach (var number in rightPartNumbers)
                    {
                        numbers.Enqueue(number);
                    }
                    
                    var sum = numbers.Dequeue();
                    
                    while (!string.IsNullOrEmpty(scenario.Peek()))
                    {
                        var nextNumber = numbers.Dequeue();
                        var nextOperation = scenario.Dequeue();
                        
                        if (nextOperation.Equals("*"))
                        {
                            sum = sum * nextNumber;
                        }
                        else
                        {
                            sum += nextNumber;
                        }
                    }

                    if (sum == targetAmount)
                    {
                        sumOfValidEquations += sum;
                        break;
                    }
                }
            }

            Console.WriteLine($"Sum of all correct calibrations: {sumOfValidEquations}");
        }
    }
}
