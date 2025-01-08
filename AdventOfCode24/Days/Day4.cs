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
                    puzzle[x, y] = inputs[x][y];
                }
            }

            CalculateFirstPart(xValueCount, yValueCount, puzzle);
               
            CalculateSecondPart(xValueCount, yValueCount, puzzle);
        }

        private static void CalculateSecondPart(int xValueCount, int yValueCount, char[,] puzzle)
        {
            var occurencesFound = 0;
            const string wordLookup = "MAS";
            const string wordLookupInverted = "SAM";

            for (var x = 0; x < xValueCount; x++)
            {
                for (var y = 0; y < yValueCount; y++)
                {
                    if (puzzle[x, y] != 'A')
                    {
                        continue;
                    }

                    if (xValueCount - x > 1 && x > 0 && yValueCount - y > 1 && y > 0)
                    {
                        char[] charsFirst = { puzzle[x - 1, y - 1], puzzle[x, y], puzzle[x + 1, y + 1] };
                        var firstWord = new string(charsFirst);

                        var firstWordMatches = firstWord.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase)
                            || firstWord.Equals(wordLookupInverted, StringComparison.InvariantCultureIgnoreCase);

                        char[] charsVertical = { puzzle[x - 1, y + 1], puzzle[x, y], puzzle[x + 1, y - 1] };
                        var secondWord = new string(charsVertical);

                        var secondWordMatches = secondWord.Equals(wordLookup, StringComparison.InvariantCultureIgnoreCase)
                            || secondWord.Equals(wordLookupInverted, StringComparison.InvariantCultureIgnoreCase);

                        if (firstWordMatches && secondWordMatches)
                        {
                            occurencesFound++;
                        }
                    }
                }
            }

            Console.WriteLine($"Total occurences found: {occurencesFound}");
        }

        private static void CalculateFirstPart(int xValueCount, int yValueCount, char[,] puzzle)
        {
            const string wordLookup = "XMAS";
            var occurencesFound = 0;

            for (var x = 0; x < xValueCount; x++)
            {
                for (var y = 0; y < yValueCount; y++)
                {
                    string word;

                    if (puzzle[x, y] != 'X')
                    {
                        continue;
                    }

                    //Search Horizontally

                    if (xValueCount - x > 3)
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


                    if (x + 3 < xValueCount && y + 3 < yValueCount)
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

            Console.WriteLine($"Occurences of X-MAS found: {occurencesFound}");
        }
    }
}
