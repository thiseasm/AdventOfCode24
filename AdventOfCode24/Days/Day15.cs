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

        CalculateFirstPart(inputs, xValueCount, yValueCount);
        
        var map = new string[xValueCount * 2, yValueCount];

        var positionOfRobot = new Point(0, 0);

        for (var x = 0; x < xValueCount * 2; x += 2)
        {
            for (var y = 0; y < yValueCount; y ++)
            {
                var currentObject = inputs[y][x/2].ToString();
                switch (currentObject)
                {
                    case ".":
                        map[ x , y ] = ".";
                        map[ x + 1, y ] = ".";
                        break;
                    case "#":
                        map[ x , y ] = "#";
                        map[ x + 1, y ] = "#";
                        break;
                    case "O":
                        map[ x , y ] = "[";
                        map[ x + 1, y ] = "]";
                        break;
                    case "@":
                        map[ x , y ] = "@";
                        map[ x + 1, y] = ".";
                        positionOfRobot = new Point( x , y);
                        break;
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

            if (nextMovement.Equals("<") || nextMovement.Equals(">"))
            {
                positionOfRobot = MoveSideways(map, nextMovement, ref positionOfRobot);
                continue;
            }
            
            var canMove= nextMovement switch
            {
                "^" => CheckForEmptySpacesUp(map, positionOfRobot),
                "v" => CheckForEmptySpacesDown(map, positionOfRobot),
            };

            if (!canMove)
            {
                continue;
            }

            var targetPoint = nextMovement switch
            {
                "^" => positionOfRobot with {Y = positionOfRobot.Y - 1},
                "v" => positionOfRobot with {Y = positionOfRobot.Y + 1},
            };
            
            
            switch (nextMovement)
            {
                case "^":
                    MoveUpInWideSpace(map, ref positionOfRobot);
                    break;
                case "v":
                    MoveDownInWideSpace(map, ref positionOfRobot);
                    break;
            }

            positionOfRobot = targetPoint;
            
        }

        var finalGoodPositions = new HashSet<Point>();

        for (var x = 0; x < xValueCount * 2; x++)
        {
            for (var y = 0; y < yValueCount; y++)
            {
                if (map[x, y].Equals("["))
                {
                    finalGoodPositions.Add(new Point(x, y));
                }
            }
        }

        var sumOfAllCoordinates = finalGoodPositions.Sum(position => 100 * position.Y + position.X);
        Console.WriteLine($"The sum of all GPS positions is :{sumOfAllCoordinates}");
    }

    private static Point MoveSideways(string[,] map, string nextMovement,ref Point positionOfRobot)
    {
        var emptyPoint= nextMovement switch
        {
            "<" => CheckForEmptySpaceLeft(map, positionOfRobot),
            ">" => CheckForEmptySpaceRight(map, positionOfRobot),
        };

        if (emptyPoint == null)
        {
            return positionOfRobot;
        }

        var targetPoint = nextMovement switch
        {
            "<" => positionOfRobot with {X = positionOfRobot.X - 1},
            ">" => positionOfRobot with {X = positionOfRobot.X + 1},
        };
            
            
        switch (nextMovement)
        {
            case "<":
                MoveLeft(map, positionOfRobot, targetPoint, emptyPoint.Value);
                break;
            case ">":
                MoveRight(map, positionOfRobot, targetPoint, emptyPoint.Value);
                break;
        }

        positionOfRobot = targetPoint;
        return positionOfRobot;
    }

    private static void CalculateFirstPart(List<string> inputs, int xValueCount, int yValueCount)
    {
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
    
    private static void MoveDownInWideSpace(string[,] map, ref Point positionOfRobot)
    {
        var targetPoint = positionOfRobot with { Y = positionOfRobot.Y + 1 };
        
        if (map[targetPoint.X, targetPoint.Y].Equals("."))
        {
            positionOfRobot = targetPoint;
            map[targetPoint.X, targetPoint.Y] = "@";
            map[targetPoint.X, targetPoint.Y - 1] = ".";
            return;
        }
        
        var crate = map[targetPoint.X, targetPoint.Y].Equals("[") 
            ? (new Point(targetPoint.X, targetPoint.Y), targetPoint with { X = targetPoint.X + 1 }) 
            : (targetPoint with { X = targetPoint.X - 1 }, new Point(targetPoint.X, targetPoint.Y));

        MoveCrateDown(map, crate);
        
        positionOfRobot = targetPoint;
        map[targetPoint.X, targetPoint.Y] = "@";
        map[targetPoint.X, targetPoint.Y - 1] = ".";

    }

    private static void MoveCrateDown(string[,] map, (Point, Point) crate)
    {
        if (map[crate.Item1.X, crate.Item1.Y + 1].Equals(".") && map[crate.Item2.X, crate.Item2.Y + 1].Equals("."))
        {
            map[crate.Item1.X, crate.Item1.Y + 1] = "[";
            map[crate.Item2.X, crate.Item2.Y + 1] = "]";
            map[crate.Item1.X, crate.Item1.Y] = ".";
            map[crate.Item2.X, crate.Item2.Y] = ".";
            
            return;
        }

        List<(Point, Point)> cratesToMove = [];
        
        if (map[crate.Item1.X, crate.Item1.Y + 1].Equals("[") && map[crate.Item2.X, crate.Item2.Y + 1].Equals("]"))
        {
            cratesToMove.Add((crate.Item1 with {Y = crate.Item1.Y + 1}, crate.Item2 with {Y = crate.Item2.Y + 1}));
        }

        if (map[crate.Item1.X, crate.Item1.Y + 1].Equals("]"))
        {
            cratesToMove.Add((new Point(crate.Item1.X - 1, crate.Item1.Y + 1), crate.Item1 with {Y = crate.Item1.Y + 1}));
        }
            
        if (map[crate.Item2.X, crate.Item2.Y + 1].Equals("["))
        {
            cratesToMove.Add((crate.Item2 with {Y = crate.Item2.Y + 1},new Point(crate.Item2.X + 1, crate.Item1.Y + 1)));
        }

        foreach (var crateToMove in cratesToMove)
        {
            MoveCrateDown(map, crateToMove);
        }
        
        map[crate.Item1.X, crate.Item1.Y + 1] = "[";
        map[crate.Item2.X, crate.Item2.Y + 1] = "]";
        map[crate.Item1.X, crate.Item1.Y] = ".";
        map[crate.Item2.X, crate.Item2.Y] = ".";
    }
    
    private static void MoveUpInWideSpace(string[,] map, ref Point positionOfRobot)
    {
        var targetPoint = positionOfRobot with { Y = positionOfRobot.Y - 1 };
        
        if (map[targetPoint.X, targetPoint.Y].Equals("."))
        {
            positionOfRobot = targetPoint;
            map[targetPoint.X, targetPoint.Y] = "@";
            map[targetPoint.X, targetPoint.Y + 1] = ".";
            return;
        }
        
        var crate = map[targetPoint.X, targetPoint.Y].Equals("[") 
            ? (new Point(targetPoint.X, targetPoint.Y), targetPoint with { X = targetPoint.X + 1 }) 
            : (targetPoint with { X = targetPoint.X - 1 }, new Point(targetPoint.X, targetPoint.Y));

        MoveCrateUp(map, crate);
        
        positionOfRobot = targetPoint;
        map[targetPoint.X, targetPoint.Y] = "@";
        map[targetPoint.X, targetPoint.Y + 1] = ".";
    }
    
    private static void MoveCrateUp(string[,] map, (Point, Point) crate)
    {
        if (map[crate.Item1.X, crate.Item1.Y - 1].Equals(".") && map[crate.Item2.X, crate.Item2.Y - 1].Equals("."))
        {
            map[crate.Item1.X, crate.Item1.Y - 1] = "[";
            map[crate.Item2.X, crate.Item2.Y - 1] = "]";
            map[crate.Item1.X, crate.Item1.Y] = ".";
            map[crate.Item2.X, crate.Item2.Y] = ".";
            
            return;
        }

        List<(Point, Point)> cratesToMove = [];
        
        if (map[crate.Item1.X, crate.Item1.Y - 1].Equals("[") && map[crate.Item2.X, crate.Item2.Y - 1].Equals("]"))
        {
            cratesToMove.Add((crate.Item1 with {Y = crate.Item1.Y - 1}, crate.Item2 with {Y = crate.Item2.Y - 1}));
        }

        if (map[crate.Item1.X, crate.Item1.Y - 1].Equals("]"))
        {
            cratesToMove.Add((new Point(crate.Item1.X - 1, crate.Item1.Y - 1), crate.Item1 with {Y = crate.Item1.Y - 1}));
        }
            
        if (map[crate.Item2.X, crate.Item2.Y - 1].Equals("["))
        {
            cratesToMove.Add((crate.Item2 with {Y = crate.Item2.Y - 1},new Point(crate.Item2.X + 1, crate.Item1.Y - 1)));
        }

        foreach (var crateToMove in cratesToMove)
        {
            MoveCrateUp(map, crateToMove);
        }
        
        map[crate.Item1.X, crate.Item1.Y - 1] = "[";
        map[crate.Item2.X, crate.Item2.Y - 1] = "]";
        map[crate.Item1.X, crate.Item1.Y] = ".";
        map[crate.Item2.X, crate.Item2.Y] = ".";
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
    
    private static bool CheckForEmptySpacesDown(string[,] map, Point currentPoint)
    {
        var adjacentPoint = currentPoint with { Y = currentPoint.Y + 1};
        switch (map[adjacentPoint.X, adjacentPoint.Y])
        {
            case ".":
                return true;
            case "#":
                return false;
        }

        var crate = map[adjacentPoint.X, adjacentPoint.Y].Equals("[") 
            ? (new Point(adjacentPoint.X, adjacentPoint.Y), adjacentPoint with { X = adjacentPoint.X + 1 }) 
            : (adjacentPoint with { X = adjacentPoint.X - 1 }, new Point(adjacentPoint.X, adjacentPoint.Y));
        
        List<Point> adjacentPoints = [crate.Item1 with { Y = currentPoint.Y + 1}, crate.Item2 with { Y = currentPoint.Y + 1}];

        return adjacentPoints.Select(point => CheckForEmptySpacesDown(map, point)).All(canMoveDown => canMoveDown);
    }
    
    private static bool CheckForEmptySpacesUp(string[,] map, Point currentPoint)
    {
        var adjacentPoint = currentPoint with { Y = currentPoint.Y - 1};
        switch (map[adjacentPoint.X, adjacentPoint.Y])
        {
            case ".":
                return true;
            case "#":
                return false;
        }

        var crate = map[adjacentPoint.X, adjacentPoint.Y].Equals("[") 
            ? (new Point(adjacentPoint.X, adjacentPoint.Y), adjacentPoint with { X = adjacentPoint.X + 1 }) 
            : (adjacentPoint with { X = adjacentPoint.X - 1 }, new Point(adjacentPoint.X, adjacentPoint.Y));
        
        List<Point> adjacentPoints = [crate.Item1 with { Y = currentPoint.Y - 1}, crate.Item2 with { Y = currentPoint.Y - 1}];

        return adjacentPoints.Select(point => CheckForEmptySpacesUp(map, point)).All(canMoveUp => canMoveUp);
    }
}