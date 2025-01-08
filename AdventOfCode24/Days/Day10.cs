using System.Drawing;

namespace AdventOfCode.Days;

public class Day10 : DayBase
{
    public override void Start()
    {
        var inputs = ReadFile("Day10.txt");
        
        const int startingElevation = 0;
        const int peakElevation = 9;

        var xValueCount = inputs[0].Length;
        var yValueCount = inputs.Length;

        var map = new string[xValueCount, yValueCount];

        var trailHeadLocations = new List<Point>();

        for (var x = 0; x < xValueCount; x++)
        {
            for (var y = 0; y < yValueCount; y++)
            {
                map[x, y] = inputs[y][x].ToString();
                if (int.TryParse(map[x, y], out var value) && value == startingElevation)
                {
                    trailHeadLocations.Add(new Point(x,y));
                }
            }
        }

        CalculateFirstPart(trailHeadLocations, map, xValueCount, yValueCount, peakElevation);
        
        CalculateSecondPart(trailHeadLocations, map, xValueCount, yValueCount, peakElevation);
    }

    private static void CalculateSecondPart(List<Point> trailHeadLocations, string[,] map, int xValueCount, int yValueCount,
        int peakElevation)
    {
        var sumOfScores = 0;
        foreach (var startingLocation in trailHeadLocations)
        {
            var scoreOfTrail = 0;
            var currentLocation = startingLocation;
            
            var alreadyExploredPaths = new List<Point>();
            var branchingPoints = new Stack<Point>();
            
            var trailHasPathsToExplore = true;
            
            while (trailHasPathsToExplore)
            {
                var pointsToExplore = new List<Point>();
                var currentElevation = int.Parse(map[currentLocation.X, currentLocation.Y]);
                if (currentLocation.X < xValueCount - 1)
                {
                    var pointToExplore = currentLocation with { X = currentLocation.X + 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }

                if (currentLocation.X > 0)
                {
                    var pointToExplore = currentLocation with { X = currentLocation.X - 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }

                if (currentLocation.Y < yValueCount - 1)
                {
                    var pointToExplore = currentLocation with { Y = currentLocation.Y + 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }

                if (currentLocation.Y > 0)
                {
                    var pointToExplore = currentLocation with { Y = currentLocation.Y - 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }
                
                var availablePaths = pointsToExplore.Where(x => !alreadyExploredPaths.Contains(x)).ToList();

                if (availablePaths.Count > 1)
                {
                    branchingPoints.Push(currentLocation);
                    currentLocation = availablePaths[0];
                    alreadyExploredPaths.Add(currentLocation);
                }
                else if (availablePaths.Count == 1)
                {
                    currentLocation = availablePaths[0];
                }
                else
                {
                    if (currentElevation == peakElevation)
                    {
                        scoreOfTrail++;
                    }

                    if (branchingPoints.Count > 0)
                    {
                        currentLocation = branchingPoints.Pop();
                    }
                    else
                    {
                        trailHasPathsToExplore = false;
                    }
                }
            }

            sumOfScores += scoreOfTrail;
        }
        
        Console.WriteLine($"The sum of all trail scores is: {sumOfScores}");
    }

    private static void CalculateFirstPart(List<Point> trailHeadLocations, string[,] map, int xValueCount, int yValueCount, int peakElevation)
    {
        var sumOfScores = 0;
        foreach (var startingLocation in trailHeadLocations)
        {
            var peaksReachedFromCurrentTrail = new List<Point>();
            var currentLocation = startingLocation;
            
            var alreadyExploredPaths = new List<Point>();
            var branchingPoints = new Stack<Point>();
            
            var trailHasPathsToExplore = true;

            while (trailHasPathsToExplore)
            {
                var pointsToExplore = new List<Point>();
                var currentElevation = int.Parse(map[currentLocation.X, currentLocation.Y]);
                if (currentLocation.X < xValueCount - 1)
                {
                    var pointToExplore = currentLocation with { X = currentLocation.X + 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }

                if (currentLocation.X > 0)
                {
                    var pointToExplore = currentLocation with { X = currentLocation.X - 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }

                if (currentLocation.Y < yValueCount - 1)
                {
                    var pointToExplore = currentLocation with { Y = currentLocation.Y + 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }

                if (currentLocation.Y > 0)
                {
                    var pointToExplore = currentLocation with { Y = currentLocation.Y - 1 };
                    PeekAdjacentLocation(map, pointToExplore, currentElevation, pointsToExplore);
                }

                var availablePaths = pointsToExplore.Where(x => !alreadyExploredPaths.Contains(x)).ToList();

                if (availablePaths.Count > 1)
                {
                    branchingPoints.Push(currentLocation);
                    currentLocation = availablePaths[0];
                    alreadyExploredPaths.Add(currentLocation);
                }
                else if (availablePaths.Count == 1)
                {
                    currentLocation = availablePaths[0];
                }
                else
                {
                    if (currentElevation == peakElevation && !peaksReachedFromCurrentTrail.Contains(currentLocation))
                    {
                        peaksReachedFromCurrentTrail.Add(currentLocation);
                    }

                    if (branchingPoints.Count > 0)
                    {
                        currentLocation = branchingPoints.Pop();
                        alreadyExploredPaths.Add(currentLocation);
                    }
                    else
                    {
                        trailHasPathsToExplore = false;
                    }
                }
            }

            sumOfScores += peaksReachedFromCurrentTrail.Count;
        }
        
        Console.WriteLine($"The sum of all trail score is: {sumOfScores}");
    }

    private static void PeekAdjacentLocation(string[,] map, Point pointToExplore, int currentElevation, ICollection<Point> pointsToExplore)
    {
        var adjacentLocation = map[pointToExplore.X, pointToExplore.Y];
        if (adjacentLocation.Equals("."))
        {
            return;
        }
        
        if (int.Parse(adjacentLocation) - currentElevation == 1)
        {
            pointsToExplore.Add(new Point(pointToExplore.X, pointToExplore.Y));
        }
    }
}