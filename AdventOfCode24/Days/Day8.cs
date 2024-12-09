namespace AdventOfCode.Days
{
    public class Day8 : DayBase
    {
        public override void Start()
        {
            var inputs = ReadFile("Day8.txt");

            var xValueCount = inputs[0].Length;
            var yValueCount = inputs.Length;

            var map = new char[xValueCount, yValueCount];

            Dictionary<char, List<(int, int)>> antennaLocations = [];

            for (var x = 0; x < xValueCount; x++)
            {
                for (var y = 0; y < yValueCount; y++)
                {
                    map[x, y] = inputs[y][x];
                    if(map[x, y].Equals('.'))
                    {
                        continue;
                    }

                    if (antennaLocations.TryGetValue(map[x, y], out var registeredLocations))
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
                for(var current = 0; current < location.Value.Count; current++)
                {
                    for(var other = current + 1; other < location.Value.Count; other++)
                    {
                        var firstAntennaLocation = location.Value[current];
                        var secondAntennaLocation = location.Value[other];

                        var xDistance = Math.Abs(firstAntennaLocation.Item1 - secondAntennaLocation.Item1);
                        var yDistance = Math.Abs(firstAntennaLocation.Item2 - secondAntennaLocation.Item2);
                        var firstAntinode = FindAntinode(firstAntennaLocation, secondAntennaLocation, xDistance, yDistance);

                        if (PositionIsInsideBounds(xValueCount, yValueCount, firstAntinode))
                        {
                            antinodeLocations.Add(firstAntinode);
                        }

                        var secondAntinode = FindAntinode(secondAntennaLocation, firstAntennaLocation, xDistance, yDistance);
                        if (PositionIsInsideBounds(xValueCount, yValueCount, secondAntinode))
                        {
                            antinodeLocations.Add(secondAntinode);
                        }
                    }
                }
            }

            Console.WriteLine($"All unique antinode locations are: {antinodeLocations.Count}");

            HashSet<(int, int)> antinodeLocationsAfterHarmonics = [];

            foreach (var location in antennaLocations)
            {
                if (location.Value.Count > 1)
                {
                    foreach (var point in location.Value)
                    {
                        antinodeLocationsAfterHarmonics.Add(point);
                    }
                }
                
                for (var current = 0; current < location.Value.Count; current++)
                {
                    for (var other = current + 1; other < location.Value.Count; other++)
                    {
                        var firstAntennaLocation = location.Value[current];
                        var secondAntennaLocation = location.Value[other];

                        var xDistance = Math.Abs(firstAntennaLocation.Item1 - secondAntennaLocation.Item1);
                        var yDistance = Math.Abs(firstAntennaLocation.Item2 - secondAntennaLocation.Item2);

                        var nextAntinode = FindAntinode(firstAntennaLocation, secondAntennaLocation, xDistance, yDistance);
                        var target = firstAntennaLocation;
                        while (PositionIsInsideBounds(xValueCount, yValueCount, nextAntinode))
                        {
                            antinodeLocationsAfterHarmonics.Add(nextAntinode);
                            var temp = nextAntinode;
                            nextAntinode = FindAntinode(nextAntinode, target, xDistance, yDistance);
                            target = temp;
                        }

                        nextAntinode = FindAntinode(secondAntennaLocation, firstAntennaLocation, xDistance, yDistance);
                        target = secondAntennaLocation;
                        while (PositionIsInsideBounds(xValueCount, yValueCount, nextAntinode))
                        {
                            antinodeLocationsAfterHarmonics.Add(nextAntinode);
                            var temp = nextAntinode;
                            nextAntinode = FindAntinode(nextAntinode, target, xDistance, yDistance);
                            target = temp;
                        }
                    }
                }
            }

            Console.WriteLine($"All unique antinode locations after harmonics are: {antinodeLocationsAfterHarmonics.Count}");
        }

        private static (int , int ) FindAntinode((int, int) fromPoint, (int, int) toPoint, int xDistance, int yDistance)
        {
            var antinodeXPosition = fromPoint.Item1 > toPoint.Item1
                                        ? fromPoint.Item1 + xDistance
                                        : fromPoint.Item1 - xDistance;

            var antinodeYPosition = fromPoint.Item2 > toPoint.Item2
                                        ? fromPoint.Item2 + yDistance
                                        : fromPoint.Item2 - yDistance;

            return ( antinodeXPosition,  antinodeYPosition);
        }

        private static bool PositionIsInsideBounds(int xValueCount, int yValueCount, (int, int) position)
        {
            return position.Item1 >= 0 && position.Item1 < xValueCount && position.Item2 >= 0 && position.Item2 < yValueCount;
        }
    }
}
