namespace AdventOfCode24.Days
{
    public class Day1 : DayBase
    {
        public override void Start()
        {
            var inputs = ReadFile("Day1.txt");
            var firstList = inputs
                .Select(x => int.Parse(x.Substring(0, x.Length / 2)
                    .Trim()))
                .Order()
                .ToArray();

            var secondList = inputs
                .Select(x => int.Parse(x.Substring(x.Length / 2)
                    .Trim()))
                .Order()
                .ToArray();

            var totalDistance = 0;

            for (var counter = 0; counter < firstList.Count(); counter++)
            {

                if (firstList[counter] > secondList[counter])
                {
                    totalDistance += firstList[counter] - secondList[counter];
                }
                else if (firstList[counter] < secondList[counter])
                {
                    totalDistance += secondList[counter] - firstList[counter];
                }
            }

            Console.WriteLine($"Total Distance: {totalDistance} ");

            var similarityScore = firstList.Sum(num => secondList.Count(x => x == num) * num);
            Console.WriteLine($"Similarity Score: {similarityScore} ");
        }
    }
}
