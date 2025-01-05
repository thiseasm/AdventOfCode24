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

            var lowestScore = long.MaxValue;

            var priorityQueue = new PriorityQueue<(PositionWithDirection position, int score), int>();
            var visited = new HashSet<PositionWithDirection>();

            var startingPosition = new PositionWithDirection { Position = startPosition, Direction = Direction.East };
            priorityQueue.Enqueue((startingPosition, 0), 0);

            while (priorityQueue.Count > 0)
            {
                var (currentPosition, score) = priorityQueue.Dequeue();

                if (currentPosition.Position == endPosition && score < lowestScore)
                { 
                    lowestScore = score; 
                }

                if (!visited.Add(currentPosition))
                {
                    continue;
                }

                foreach (var (dx, dy, direction) in Directions)
                {
                    var nextPoint = currentPosition.Position with { X = currentPosition.Position.X + dx, Y = currentPosition.Position.Y + dy };   

                    if (nextPoint.X < 0 || nextPoint.X >= xValueCount || nextPoint.Y < 0  || nextPoint.Y >= yValueCount || map[nextPoint.X, nextPoint.Y].Equals("#"))
                    {
                        continue;
                    }

                    int turnCost = (currentPosition.Direction == direction) ? 1 : 1001;
                    int newScore = score + turnCost;

                    var nextPosition = new PositionWithDirection { Position = nextPoint, Direction = direction };

                    priorityQueue.Enqueue((nextPosition, newScore), newScore);
                }
            }

            Console.WriteLine($"The path with the lowest score costs: {lowestScore}");
        }


        private struct PositionWithDirection
        {
            public Point Position { get; set; }
            public Direction Direction { get; set; }
        }

        private static readonly (int dx, int dy, Direction direction)[] Directions = [
            (0, 1, Direction.South),
            (1, 0, Direction.East),
            (0, -1, Direction.North),
            (-1, 0, Direction.West)
        ];

        private enum Direction
        {
            North,
            South,
            East,
            West
        }
    }


}
