using System.Drawing;

namespace AdventOfCode.Days
{
    public class Day16 : DayBase
    {
        public override void Start()
        {
            var inputs = ReadFile("Day16.txt").ToList();

            var xValueCount = inputs[0].Length;
            var yValueCount = inputs.Count;

            var map = new string[xValueCount, yValueCount];

            var startPosition = new Point(0, 0);
            var endPosition = new Point(0, 0);

            for (var x = 0; x < xValueCount; x++)
            {
                for (var y = 0; y < yValueCount; y++)
                {
                    map[x, y] = inputs[y][x].ToString();
                    if (map[x, y].Equals("E"))
                    {
                        endPosition = new Point(x, y);
                    }
                    else if (map[x, y].Equals("S"))
                    {
                        startPosition = new Point(x, y);
                    }   
                }
            }

            var allPathsExplored = false;
            var lowestScore = long.MaxValue;

            var pathsAlreadyTraveled = new Stack<KeyValuePair<PositionWithDirection, List<PositionWithDirection>>>();

            while (!allPathsExplored)
            {
                var currentPosition = new PositionWithDirection
                {
                    Position = endPosition,
                    Direction = 0
                };

                long score = 0;

                while (score < lowestScore)
                {
                    var adjustcentPositions = SurveyAdjustcentPositions(currentPosition, map);

                    if (adjustcentPositions.Any(x => x.Position.Equals(startPosition)))
                    {
                        var finalPosition = adjustcentPositions.First(x => x.IsFinal);
                        score += finalPosition.Direction == currentPosition.Direction ? 1 : 1000;
                       
                        if (score < lowestScore)
                        {
                            lowestScore = score;
                        }

                        currentPosition = new PositionWithDirection
                        {
                            Position = endPosition,
                            Direction = 0
                        };

                        if (!pathsAlreadyTraveled.Any())
                        {
                            allPathsExplored = true;
                        }
                        break;
                    }

                    if (adjustcentPositions.Count > 1)
                    {
                        if (!pathsAlreadyTraveled.Any(x => x.Key.Equals(currentPosition)))
                        {
                            pathsAlreadyTraveled.Push(new KeyValuePair<PositionWithDirection, List<PositionWithDirection>>(currentPosition, []));
                        }

                        if (pathsAlreadyTraveled.Any() && pathsAlreadyTraveled.Peek().Key.Equals(currentPosition))
                        {
                            var nextPosition = adjustcentPositions.FirstOrDefault(x => !pathsAlreadyTraveled.Peek().Value.Contains(x));
                            if (nextPosition.Equals((PositionWithDirection)default))
                            {
                                pathsAlreadyTraveled.Pop();
                                currentPosition = new PositionWithDirection
                                {
                                    Position = endPosition,
                                    Direction = 0
                                };

                                if (!pathsAlreadyTraveled.Any())
                                {
                                    allPathsExplored = true;
                                }
                                break;
                            }

                            pathsAlreadyTraveled.Peek().Value.Add(nextPosition);

                            score += currentPosition.Direction == nextPosition.Direction ? 1 : 1001;
                            currentPosition = nextPosition;
                        }
                        else
                        {
                            var nextPosition = pathsAlreadyTraveled.FirstOrDefault(x => x.Key.Equals(currentPosition)).Value.Last();
                            score += currentPosition.Direction == nextPosition.Direction ? 1 : 1001;
                            currentPosition = nextPosition;
                        }

                        continue;
                    }

                    if (adjustcentPositions.Count == 1)
                    {
                        var nextPosition = adjustcentPositions.First();
                        score += currentPosition.Direction == nextPosition.Direction ? 1 : 1001;
                        currentPosition = nextPosition;
                        continue;
                    }

                    currentPosition = new PositionWithDirection
                    {
                        Position = endPosition,
                        Direction = 0
                    };

                    if (!pathsAlreadyTraveled.Any())
                    {
                        allPathsExplored = true;
                    }

                }
            }

            Console.WriteLine($"The path with the lowest score costs: {lowestScore}");
        }

        private static List<PositionWithDirection> SurveyAdjustcentPositions(PositionWithDirection currentPosition, string[,] map)
        {
            var possibleNextPositions = new List<PositionWithDirection>();
            var xValueCount = map.GetLength(0);
            var yValueCount = map.GetLength(1);

            if (currentPosition.Position.X < xValueCount - 1 && currentPosition.Direction != Direction.North)
            {
                var nextPoint = currentPosition.Position with { X = currentPosition.Position.X + 1 };
                if (map[nextPoint.X, nextPoint.Y].Equals(".") || map[nextPoint.X, nextPoint.Y].Equals("S"))
                {
                    var nextPosition = new PositionWithDirection
                    {
                        Position = nextPoint,
                        Direction = Direction.South,
                        IsFinal = map[nextPoint.X, nextPoint.Y].Equals("S")
                    };
                    possibleNextPositions.Add(nextPosition);
                }
            }

            if (currentPosition.Position.X > 0 && currentPosition.Direction != Direction.South)
            {
                var nextPoint = currentPosition.Position with { X = currentPosition.Position.X - 1 };
                if (map[nextPoint.X, nextPoint.Y].Equals(".") || map[nextPoint.X, nextPoint.Y].Equals("S"))
                {
                    var nextPosition = new PositionWithDirection
                    {
                        Position = nextPoint,
                        Direction = Direction.North,
                        IsFinal = map[nextPoint.X, nextPoint.Y].Equals("S")
                    };
                    possibleNextPositions.Add(nextPosition);
                }
            }

            if (currentPosition.Position.Y > 0 && currentPosition.Direction != Direction.East)
            {
                var nextPoint = currentPosition.Position with { Y = currentPosition.Position.Y - 1 };
                if (map[nextPoint.X, nextPoint.Y].Equals(".") || map[nextPoint.X, nextPoint.Y].Equals("S"))
                {
                    var nextPosition = new PositionWithDirection
                    {
                        Position = nextPoint,
                        Direction = Direction.West,
                        IsFinal = map[nextPoint.X, nextPoint.Y].Equals("S")
                    };
                    possibleNextPositions.Add(nextPosition);
                }
            }

            if (currentPosition.Position.Y < yValueCount - 1 && currentPosition.Direction != Direction.West)
            {
                var nextPoint = currentPosition.Position with { Y = currentPosition.Position.Y + 1 };
                if (map[nextPoint.X, nextPoint.Y].Equals(".") || map[nextPoint.X, nextPoint.Y].Equals("S"))
                {
                    var nextPosition = new PositionWithDirection
                    {
                        Position = nextPoint,
                        Direction = Direction.East,
                        IsFinal = map[nextPoint.X, nextPoint.Y].Equals("S")
                    };
                    possibleNextPositions.Add(nextPosition);
                }
            }

            return possibleNextPositions;
        }


        private struct PositionWithDirection
        {
            public Point Position { get; set; }
            public Direction Direction { get; set; }
            public bool IsFinal { get; set; }
        }



        private enum Direction
        {
            North,
            South,
            East,
            West
        }
    }


}
