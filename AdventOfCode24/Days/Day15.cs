using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AdventOfCode.Days;

public class Day15 : DayBase
{
    public override void Start()
    {
        var inputs = ReadFile("Day15.txt").ToList();
        
        var xValueCount = inputs[0].Length;
        var yValueCount = inputs.FindIndex(x => x.Equals(string.Empty));

        var map = new string[xValueCount, yValueCount];

        var positionOfRobot = new Point(0, 0);

        for (var x = 0; x < xValueCount; x++)
        {
            for (var y = 0; y < yValueCount; y++)
            {
                map[x, y] = inputs[y][x].ToString();
                if (map[x, y].Equals("@"))
                {
                    positionOfRobot = new Point(x, y);
                }
            }
        }

        var robotSequence = new Queue<string>();
        foreach (var movement in inputs[yValueCount..].SelectMany(sequence => sequence))
        {
            robotSequence.Enqueue(movement.ToString());
        }

        while (robotSequence.Count > 0)
        {
            var nextMovement = robotSequence.Dequeue();
            
            var emptyPoint = nextMovement switch
            {
                "<" => CheckForEmptySpaceLeft(map, positionOfRobot),
                "v" => CheckForEmptySpaceDown(map, positionOfRobot),
                ">" => CheckForEmptySpaceRight(map, positionOfRobot),
                _ => CheckForEmptySpaceUp(map, positionOfRobot)
            };

            if (emptyPoint == null)
            {
                continue;
            }

            var targetPoint = nextMovement switch
            {
                "<" => positionOfRobot with {X = positionOfRobot.X - 1},
                "v" => positionOfRobot with {Y = positionOfRobot.Y + 1},
                ">" => positionOfRobot with {X = positionOfRobot.X + 1},
                _ => positionOfRobot with {Y = positionOfRobot.Y - 1}
            };
            
            
            switch (nextMovement)
            {
                case "<":
                    MoveLeft(map, positionOfRobot, targetPoint, emptyPoint.Value);
                    break;
                case ">":
                    MoveRight(map, positionOfRobot, targetPoint, emptyPoint.Value);
                    break;
                case "^":
                    MoveUp(map, positionOfRobot, targetPoint, emptyPoint.Value);
                    break;
                case "v":
                    MoveDown(map, positionOfRobot, targetPoint, emptyPoint.Value);
                    break;
            }

            positionOfRobot = targetPoint;
        }

        var finalGoodPositions = new HashSet<Point>();

        for (var x = 0; x < xValueCount; x++)
        {
            for (var y = 0; y < yValueCount; y++)
            {
                if (map[x, y].Equals("O"))
                {
                    finalGoodPositions.Add(new Point(x, y));
                }
            }
        }

        var sumOfAllCoordinates = finalGoodPositions.Sum(position => 100 * position.Y + position.X);
        Console.WriteLine($"The sum of all GPS positions is :{sumOfAllCoordinates}");
    }

    private static void MoveLeft(string[,] map, Point positionOfRobot, Point targetPoint, Point emptyPoint)
    {
        while (positionOfRobot != targetPoint)
        {
            var fromPoint = emptyPoint with { X = emptyPoint.X + 1 };
            map[emptyPoint.X, emptyPoint.Y] = map[fromPoint.X, fromPoint.Y];
            map[fromPoint.X, fromPoint.Y] = ".";

            if (map[emptyPoint.X, emptyPoint.Y].Equals("@"))
            {
                positionOfRobot = emptyPoint;
                continue;
            }

            emptyPoint = fromPoint;
        }
    }
    
    private static void MoveRight(string[,] map, Point positionOfRobot, Point targetPoint, Point emptyPoint)
    {
        while (positionOfRobot != targetPoint)
        {
            var fromPoint = emptyPoint with { X = emptyPoint.X - 1 };
            map[emptyPoint.X, emptyPoint.Y] = map[fromPoint.X, fromPoint.Y];
            map[fromPoint.X, fromPoint.Y] = ".";

            if (map[emptyPoint.X, emptyPoint.Y].Equals("@"))
            {
                positionOfRobot = emptyPoint;
                continue;
            }

            emptyPoint = fromPoint;
        }
    }
    
    private static void MoveUp(string[,] map, Point positionOfRobot, Point targetPoint, Point emptyPoint)
    {
        while (positionOfRobot != targetPoint)
        {
            var fromPoint = emptyPoint with { Y = emptyPoint.Y + 1 };
            map[emptyPoint.X, emptyPoint.Y] = map[fromPoint.X, fromPoint.Y];
            map[fromPoint.X, fromPoint.Y] = ".";

            if (map[emptyPoint.X, emptyPoint.Y].Equals("@"))
            {
                positionOfRobot = emptyPoint;
                continue;
            }

            emptyPoint = fromPoint;
        }
    }
    
    private static void MoveDown(string[,] map, Point positionOfRobot, Point targetPoint, Point emptyPoint)
    {
        while (positionOfRobot != targetPoint)
        {
            var fromPoint = emptyPoint with { Y = emptyPoint.Y - 1 };
            map[emptyPoint.X, emptyPoint.Y] = map[fromPoint.X, fromPoint.Y];
            map[fromPoint.X, fromPoint.Y] = ".";

            if (map[emptyPoint.X, emptyPoint.Y].Equals("@"))
            {
                positionOfRobot = emptyPoint;
                continue;
            }

            emptyPoint = fromPoint;
        }
    }


    private static Point? CheckForEmptySpaceLeft(string[,] map, Point currentPoint)
    {
        var adjacentPoint = currentPoint with { X = currentPoint.X - 1 };
        do
        {
            switch (map[adjacentPoint.X, adjacentPoint.Y])
            {
                case ".":
                    return adjacentPoint;
                case "#":
                    return null;
                default:
                    adjacentPoint = adjacentPoint with { X = adjacentPoint.X - 1 };
                    break;
            }
        } while (true);
    }
    
    private static Point? CheckForEmptySpaceRight(string[,] map, Point currentPoint)
    {
        var adjacentPoint = currentPoint with { X = currentPoint.X + 1 };
        do
        {
            switch (map[adjacentPoint.X, adjacentPoint.Y])
            {
                case ".":
                    return adjacentPoint;
                case "#":
                    return null;
                default:
                    adjacentPoint = adjacentPoint with { X = adjacentPoint.X + 1 };
                    break;
            }
        } while (true);
    }
    
    private static Point? CheckForEmptySpaceUp(string[,] map, Point currentPoint)
    {
        var adjacentPoint = currentPoint with { Y = currentPoint.Y - 1};
        do
        {
            switch (map[adjacentPoint.X, adjacentPoint.Y])
            {
                case ".":
                    return adjacentPoint;
                case "#":
                    return null;
                default:
                    adjacentPoint = adjacentPoint with { Y = adjacentPoint.Y - 1 };
                    break;
            }
        } while (true);
    }
    
    private static Point? CheckForEmptySpaceDown(string[,] map, Point currentPoint)
    {
        var adjacentPoint = currentPoint with { Y = currentPoint.Y + 1};
        do
        {
            switch (map[adjacentPoint.X, adjacentPoint.Y])
            {
                case ".":
                    return adjacentPoint;
                case "#":
                    return null;
                default:
                    adjacentPoint = adjacentPoint with { Y = adjacentPoint.Y + 1 };
                    break;
            }
        } while (true);
    }
}