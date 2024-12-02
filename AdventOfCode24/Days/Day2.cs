namespace AdventOfCode24.Days
{
    public class Day2 : DayBase
    {
        public override void Start()
        {
            var input = ReadFile("Day2.txt");

            CalculatePart1(input);

            CalculatePart2(input);
        }

        private static void CalculatePart2(string[] input)
        {
            var safeReportsCounter = 0;

            foreach (var report in input)
            {
                var reportUnpacked = report.Split(" ").Select(int.Parse).ToList();
                Direction direction = reportUnpacked[0] > reportUnpacked[1]
                    ? Direction.Descending
                    : Direction.Ascending;
                bool isProblematic = false;
                var isSafe = true;

                for (var counter = 0; counter < reportUnpacked.Count - 1; counter++)
                {
                    if (direction == Direction.Ascending)
                    {
                        if (reportUnpacked[counter + 1] - reportUnpacked[counter] > 3 || reportUnpacked[counter + 1] <= reportUnpacked[counter])
                        {
                            isProblematic = true;
                            break;
                        }
                    }
                    else
                    {
                        if (reportUnpacked[counter] - reportUnpacked[counter + 1] > 3 || reportUnpacked[counter + 1] >= reportUnpacked[counter])
                        {
                            isProblematic = true;
                            break;
                        }
                    }
                }


                if (isProblematic)
                {
                    for (var position = 0; position < reportUnpacked.Count; position++)
                    {
                        var reportWithRemoval = new List<int>(reportUnpacked);
                        reportWithRemoval.RemoveAt(position);

                        direction = reportWithRemoval[0] > reportWithRemoval[1]
                            ? Direction.Descending
                            : Direction.Ascending;

                        isSafe = true;
                        for (var counter = 0; counter < reportWithRemoval.Count() - 1; counter++)
                        {
                            if (direction == Direction.Ascending)
                            {
                                if (reportWithRemoval[counter + 1] - reportWithRemoval[counter] > 3 || reportWithRemoval[counter + 1] <= reportWithRemoval[counter])
                                {
                                    isSafe = false;
                                    break;
                                }
                            }
                            else
                            {
                                if (reportWithRemoval[counter] - reportWithRemoval[counter + 1] > 3 || reportWithRemoval[counter + 1] >= reportWithRemoval[counter])
                                {
                                    isSafe = false;
                                    break;
                                }
                            }
                        }

                        if (isSafe)
                        {
                            break;
                        }
                    }
                    
                }

                if (isSafe)
                {
                    safeReportsCounter++;
                }

            }

            Console.WriteLine($"Number of safe reports after dampening: {safeReportsCounter}");
        }

        private static void CalculatePart1(string[] input)
        {
            var safeReportsCounter = 0;

            foreach (var report in input) 
            { 
                var reportUnpacked = report.Split(" ").Select(int.Parse).ToArray();
                Direction direction = reportUnpacked[0] > reportUnpacked[1]
                    ? Direction.Descending
                    : Direction.Ascending;
                var isSafe = true;

                for (var counter = 0; counter < reportUnpacked.Count() - 1; counter++)
                {
                    if (direction == Direction.Ascending)
                    {
                        if (reportUnpacked[counter + 1] - reportUnpacked[counter] > 3 || reportUnpacked[counter + 1] <= reportUnpacked[counter])
                        {
                            isSafe = false;
                            break;
                        }
                    }
                    else
                    {
                        if (reportUnpacked[counter] - reportUnpacked[counter + 1] > 3 || reportUnpacked[counter + 1] >= reportUnpacked[counter])
                        {
                            isSafe = false;
                            break;
                        }
                    }
                }

                if (isSafe)
                {
                    safeReportsCounter++;
                }
                
            }

            Console.WriteLine($"Number of safe reports: {safeReportsCounter}");
        }

        private enum Direction
        {
            Ascending,
            Descending
        }
    }

}
