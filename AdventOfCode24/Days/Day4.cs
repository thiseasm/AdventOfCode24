using AdventOfCode24.Days;

namespace AdventOfCode.Days
{
    public class Day4 : DayBase
    {
        public override void Start()
        {
            var inputs = ReadFile("Day4.txt");

            var xValueCount = inputs[0].Length;
            var yValueCount = inputs.Count();

            char[,] puzzle = new char[xValueCount, yValueCount];

            for (int x = 0; x < xValueCount; x++) 
            {
                for (int y = 0; y < yValueCount; y++)
                {
                    puzzle[x,y] = inputs[x][y];
                }
            }

            const string wordLookup = "XMAS";
            var occurencesFound = 0;

            for (int x = 0; x < xValueCount; x++)
            {
                for (int y = 0; y < yValueCount; y++)
                {
                    var word = string.Empty;

                    if (puzzle[x, y] != 'X') 
                    { 
                        continue;
                    }

                    //Search Horizontally

                    if(xValueCount - x > 3)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x + 1, y], puzzle[x + 2, y], puzzle[x + 3, y] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                    if (x >= 3)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x - 1, y], puzzle[x - 2, y], puzzle[x - 3, y] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                    //Search Vertically

                    if (yValueCount - y > 3)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x, y + 1], puzzle[x, y + 2], puzzle[x, y + 3] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                    if (y >= 3)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x, y - 1], puzzle[x, y - 2], puzzle[x, y - 3] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                    //Search Diagonally


                    if( x + 3 < xValueCount && y + 3 < yValueCount)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x + 1, y + 1], puzzle[x + 2, y + 2], puzzle[x + 3, y + 3] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                    if (x + 3 < xValueCount && y >= 3)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x + 1, y - 1], puzzle[x + 2, y - 2], puzzle[x + 3, y - 3] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                    if (x >= 3 && y >= 3)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x - 1, y - 1], puzzle[x - 2, y - 2], puzzle[x - 3, y - 3] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                    if (x >= 3 && y + 3 < yValueCount)
                    {
                        char[] chars = { puzzle[x, y], puzzle[x - 1, y + 1], puzzle[x - 2, y + 2], puzzle[x - 3, y + 3] };
                        word = new string(chars);

                        if (word.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            occurencesFound++;
                        }
                    }

                }
            }

            Console.WriteLine($"Total occurences found: {occurencesFound}");
        }
    }
}
