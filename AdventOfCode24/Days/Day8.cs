using AdventOfCode24.Days;

namespace AdventOfCode.Days
{
    public class Day8 : DayBase
    {
        public override void Start()
        {
            var inputs = ReadFile("Day8.txt");

            var xValueCount = inputs[0].Length;
            var yValueCount = inputs.Count();

            char[,] map = new char[xValueCount, yValueCount];

            Dictionary<char, List<(int, int)>> antennaLocations = [];

            for (int x = 0; x < xValueCount; x++)
            {
                for (int y = 0; y < yValueCount; y++)
                {
                    map[x, y] = inputs[y][x];
                    if(map[x, y].Equals('.'))
                    {
                        continue;
                    }

                    if (antennaLocations.TryGetValue(map[x, y], out List<(int, int)>? registeredLocations))
                    {
                        registeredLocations.Add((x, y));
                    }
                    else 
                    {
                        antennaLocations.Add(map[x, y], [(x, y)]);
                    }
                }
            }

            HashSet<(int,int)> antinodeLocations = [];

            foreach (var location in antennaLocations) 
            {
                for(int current = 0; current < location.Value.Count; current++)
                {
                    for(int other = current + 1; other < location.Value.Count; other++)
                    {
                        var firstAntennaLocation = location.Value[current];
                        var secondAntennaLocation = location.Value[other];

                        var xDistance = Math.Abs(firstAntennaLocation.Item1 - secondAntennaLocation.Item1);
                        var yDistance = Math.Abs(firstAntennaLocation.Item2 - secondAntennaLocation.Item2);
                       var firstAntinode = FindFirstAntinode(firstAntennaLocation, secondAntennaLocation, xDistance, yDistance);

                        if (PositionIsInsideBounds(xValueCount, yValueCount, firstAntinode))
                        {
                            antinodeLocations.Add(firstAntinode);
                        }

                        var secondAntinode = FindSecondAntinode(firstAntennaLocation, secondAntennaLocation, xDistance, yDistance);
                        if (PositionIsInsideBounds(xValueCount, yValueCount, secondAntinode))
                        {
                            antinodeLocations.Add(secondAntinode);
                        }
                    }
                }
            }

            Console.WriteLine($"All unique antinode locations are: {antinodeLocations.Count}");
        }

        private static (int , int ) FindFirstAntinode((int, int) firstAntennaLocation, (int, int) secondAntennaLocation, int xDistance, int yDistance)
        {
            var firstAntinodeXPosition = firstAntennaLocation.Item1 - secondAntennaLocation.Item1 > 0
                                        ? firstAntennaLocation.Item1 + xDistance
                                        : firstAntennaLocation.Item1 - xDistance;

            var firstAntinodeYPosition = firstAntennaLocation.Item2 - secondAntennaLocation.Item2 > 0
                                        ? firstAntennaLocation.Item2 + yDistance
                                        : firstAntennaLocation.Item2 - yDistance;

            var firstAntinode = (firstAntinodeXPosition, firstAntinodeYPosition);
            return firstAntinode;
        }

        private static (int , int ) FindSecondAntinode((int, int) firstAntennaLocation, (int, int) secondAntennaLocation, int xDistance, int yDistance)
        {
            var secondAntinodeXPosition = secondAntennaLocation.Item1 - firstAntennaLocation.Item1 > 0
                                        ? secondAntennaLocation.Item1 + xDistance
                                        : secondAntennaLocation.Item1 - xDistance;

            var secondAntinodeYPosition = secondAntennaLocation.Item2 - firstAntennaLocation.Item2 > 0
                                        ? secondAntennaLocation.Item2 + yDistance
                                        : secondAntennaLocation.Item2 - yDistance;

            var secondAntinode = (secondAntinodeXPosition, secondAntinodeYPosition);
            return secondAntinode;
        }

        private static bool PositionIsInsideBounds(int xValueCount, int yValueCount, (int, int) position)
        {
            return position.Item1 >= 0 && position.Item1 < xValueCount && position.Item2 >= 0 && position.Item2 < yValueCount;
        }
    }
}
