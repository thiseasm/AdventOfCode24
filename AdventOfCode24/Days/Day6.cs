namespace AdventOfCode.Days
{
    public class Day6 : DayBase
    {
        public override void Start()
        {
            var inputs = ReadFile("Day6.txt");

            var xValueCount = inputs[0].Length;
            var yValueCount = inputs.Count();

            char[,] map = new char[xValueCount, yValueCount];

            var positionOfGuard = (0, 0);

            for (int x = 0; x < xValueCount; x++)
            {
                for (int y = 0; y < yValueCount; y++)
                {
                    map[x, y] = inputs[y][x];
                    if (map[x, y] == '^')
                    {
                        positionOfGuard = (x, y);
                    }
                }
            }

            var startingMap = (char[,])map.Clone();

            var startingPosition = positionOfGuard;

            CalculateFirstPart(xValueCount, yValueCount, map, positionOfGuard);

            var locationsVisited = new List<(int, int)>();
            for (var x = 0; x < xValueCount; x++)
            {
                for (var y = 0; y < yValueCount; y++)
                {
                    if (map[x, y] == 'X' && (x, y) != startingPosition)
                    {
                        locationsVisited.Add((x, y));
                    }
                }
            }

            var possibleLocations = 0;

            foreach (var location in locationsVisited)
            {
                var hypotheticalMap = (char[,])startingMap.Clone();

                hypotheticalMap[location.Item1, location.Item2] = '#';

                var facingDirection = '^';
                positionOfGuard = startingPosition;

                var positionsVisited = new HashSet<(int, int, char)>();

                while (positionOfGuard.Item1 >= 0 && positionOfGuard.Item1 < xValueCount && positionOfGuard.Item2 >= 0 && positionOfGuard.Item2 < yValueCount)
                {
                    
                    if (facingDirection.Equals('^'))
                    {
                        if (positionOfGuard.Item2 == 0)
                        {
                            break;
                        }

                        var tileInFrontIsBlocked = hypotheticalMap[positionOfGuard.Item1, positionOfGuard.Item2 - 1].Equals('#');
                        if (tileInFrontIsBlocked)
                        {
                            facingDirection = '>';
                        }
                        else
                        {
                            positionOfGuard.Item2--;
                        }
                    }
                    else if (facingDirection.Equals('v'))
                    {
                        if (positionOfGuard.Item2 == yValueCount - 1)
                        {
                            break;
                        }

                        var tileInFrontIsBlocked = hypotheticalMap[positionOfGuard.Item1, positionOfGuard.Item2 + 1].Equals('#');
                        if (tileInFrontIsBlocked)
                        {
                            facingDirection = '<';
                        }
                        else
                        {
                            positionOfGuard.Item2++;
                        }
                    }
                    else if (facingDirection.Equals('>'))
                    {
                        if (positionOfGuard.Item1 == xValueCount - 1)
                        {
                            break;
                        }

                        var tileInFrontIsBlocked = hypotheticalMap[positionOfGuard.Item1 + 1, positionOfGuard.Item2].Equals('#');
                        if (tileInFrontIsBlocked)
                        {
                            facingDirection = 'v';
                        }
                        else
                        {
                            positionOfGuard.Item1++;
                        }
                    }
                    else if (facingDirection.Equals('<'))
                    {
                        if (positionOfGuard.Item1 == 0)
                        {
                            break;
                        }

                        var tileInFrontIsBlocked = hypotheticalMap[positionOfGuard.Item1 - 1, positionOfGuard.Item2].Equals('#');
                        if (tileInFrontIsBlocked)
                        {
                            facingDirection = '^';
                        }
                        else
                        {
                            positionOfGuard.Item1--;
                        }
                        
                    }

                    var currentPosition = (positionOfGuard.Item1, positionOfGuard.Item2, facingDirection);

                    if(!positionsVisited.Add(currentPosition))
                    {
                        possibleLocations++;
                        break;
                    }
                }
            }

            Console.WriteLine($"Possible locations causing infinite loops:{possibleLocations}");
        }


        private static void CalculateFirstPart(int xValueCount, int yValueCount, char[,] map, (int, int) positionOfGuard)
        {
            var positionsVisited = 1;
            var facingDirection = '^';
            map[positionOfGuard.Item1, positionOfGuard.Item2] = 'X';

            while (positionOfGuard.Item1 >= 0 && positionOfGuard.Item1 < xValueCount && positionOfGuard.Item2 >= 0 && positionOfGuard.Item2 < yValueCount)
            {
                if (facingDirection.Equals('^'))
                {
                    if (positionOfGuard.Item2 == 0)
                    {
                        positionOfGuard.Item2--;
                        continue;
                    }

                    var tileInFrontIsBlocked = map[positionOfGuard.Item1, positionOfGuard.Item2 - 1].Equals('#');
                    if (tileInFrontIsBlocked)
                    {
                        facingDirection = '>';
                        continue;
                    }

                    var tileInFrontIsVisitedAlready = map[positionOfGuard.Item1, positionOfGuard.Item2 - 1].Equals('X');
                    if (!tileInFrontIsVisitedAlready)
                    {
                        positionsVisited++;
                    }

                    positionOfGuard.Item2--;

                    map[positionOfGuard.Item1, positionOfGuard.Item2] = 'X';
                }

                if (facingDirection.Equals('v'))
                {
                    if (positionOfGuard.Item2 == yValueCount - 1)
                    {
                        positionOfGuard.Item2++;
                        continue;
                    }

                    var tileInFrontIsBlocked = map[positionOfGuard.Item1, positionOfGuard.Item2 + 1].Equals('#');
                    if (tileInFrontIsBlocked)
                    {
                        facingDirection = '<';
                        continue;
                    }

                    var tileInFrontIsVisitedAlready = map[positionOfGuard.Item1, positionOfGuard.Item2 + 1].Equals('X');
                    if (!tileInFrontIsVisitedAlready)
                    {
                        positionsVisited++;
                    }

                    positionOfGuard.Item2++;

                    map[positionOfGuard.Item1, positionOfGuard.Item2] = 'X';
                }

                if (facingDirection.Equals('>'))
                {
                    if (positionOfGuard.Item1 == xValueCount - 1)
                    {
                        positionOfGuard.Item1++;
                        continue;
                    }

                    var tileInFrontIsBlocked = map[positionOfGuard.Item1 + 1, positionOfGuard.Item2].Equals('#');
                    if (tileInFrontIsBlocked)
                    {
                        facingDirection = 'v';
                        continue;
                    }

                    var tileInFrontIsVisitedAlready = map[positionOfGuard.Item1 + 1, positionOfGuard.Item2].Equals('X');
                    if (!tileInFrontIsVisitedAlready)
                    {
                        positionsVisited++;
                    }

                    positionOfGuard.Item1++;

                    map[positionOfGuard.Item1, positionOfGuard.Item2] = 'X';
                }

                if (facingDirection.Equals('<'))
                {
                    if (positionOfGuard.Item1 == 0)
                    {
                        positionOfGuard.Item1--;
                        continue;
                    }

                    var tileInFrontIsBlocked = map[positionOfGuard.Item1 - 1, positionOfGuard.Item2].Equals('#');
                    if (tileInFrontIsBlocked)
                    {
                        facingDirection = '^';
                        continue;
                    }

                    var tileInFrontIsVisitedAlready = map[positionOfGuard.Item1 - 1, positionOfGuard.Item2].Equals('X');
                    if (!tileInFrontIsVisitedAlready)
                    {
                        positionsVisited++;
                    }

                    positionOfGuard.Item1--;

                    map[positionOfGuard.Item1, positionOfGuard.Item2] = 'X';
                }
            }

            Console.WriteLine($"Total positions visited: {positionsVisited}");
        }
    }
}
