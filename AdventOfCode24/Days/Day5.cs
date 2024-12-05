using AdventOfCode24.Days;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day5 : DayBase
    {
        public override void Start()
        {
            var inputs = ReadFile("Day5.txt");

            var positionOfSeparator = inputs.ToList().FindIndex(x => x.Equals(string.Empty));

            var orderingRules = inputs.Take(positionOfSeparator).ToArray();
            var rulesGrouped = new Dictionary<int, List<int>>();

            foreach (var rule in orderingRules)
            { 
                var ruleParsed = rule.Split('|').Select(int.Parse).ToArray();
                if (rulesGrouped.Keys.Contains(ruleParsed[0]))
                {
                    rulesGrouped[ruleParsed[0]].Add(ruleParsed[1]);
                }
                else
                {
                    rulesGrouped.Add(ruleParsed[0], [ruleParsed[1]]);
                }
            }

            var updates = inputs.Skip(positionOfSeparator + 1)
                .Select(x => x.Split(',')
                    .Select(int.Parse)
                    .ToList())
                .ToArray();

            var sumOfMiddles = 0;
            var unsafeUpdates = new List<List<int>>();

            foreach (var update in updates)
            {
                var isSafe = true;
                for (var i = 0; i < update.Count(); i++)
                {
                    if(!rulesGrouped.ContainsKey(update[i]))
                    {
                        continue; 
                    }

                    foreach (var orderingRule in rulesGrouped[update[i]])
                    {
                        var indexOfRule = update.FindIndex(x => x == orderingRule);

                        if(indexOfRule >= 0 && indexOfRule < i)
                        {
                            isSafe = false;
                            unsafeUpdates.Add(update);
                            break;
                        }
                    }

                    if (!isSafe)
                    {
                        break;
                    }
                }

                if (isSafe)
                {
                    sumOfMiddles += update[update.Count / 2];
                }
            }

            Console.WriteLine($"The sum of all middles is:{sumOfMiddles}");

            sumOfMiddles = 0;

            foreach (var update in unsafeUpdates)
            {
                var rulesInUpdate = update.Intersect(rulesGrouped.Keys);
                foreach(var orderingRule in rulesGrouped.Where(x => rulesInUpdate.Contains(x.Key)))
                {
                   
                    foreach (var rule in orderingRule.Value)
                    {
                        var indexOfKey = update.FindIndex(x => x == orderingRule.Key);
                        var indexOfRule = update.FindIndex(x => x == rule);

                        if(indexOfRule < 0 || indexOfRule > indexOfKey)
                        {
                            continue;
                        }

                        (update[indexOfKey], update[indexOfRule]) = (update[indexOfRule], update[indexOfKey]);
                    }
                }

                sumOfMiddles += update[update.Count / 2];
            }

            Console.WriteLine($"The sum of all middles in unsafe updates is:{sumOfMiddles}");
        }
    }
}
