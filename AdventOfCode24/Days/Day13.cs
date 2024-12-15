using System.Drawing;

namespace AdventOfCode.Days;

public class Day13 : DayBase
{
    public override void Start()
    {
        var inputs = ReadText("Day13.txt").Split(["\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries);

        long tokensSpent = 0;
        foreach (var machine in inputs)
        {
            var machineConfig = GetMachineConfiguration(machine);
            long winningCombinationCost = 0;
            
            for (var a = 0; a < 100; a++)
            {
                for (var b = 0; b < 100; b++)
                {
                    var xResult = a * machineConfig.A.Xmovement + b * machineConfig.B.Xmovement;
                    var yResult = a * machineConfig.A.Ymovement + b * machineConfig.B.Ymovement;

                    if (xResult == machineConfig.Prize.X && yResult == machineConfig.Prize.Y)
                    {
                        var cost = a * 3 + b;
                        if (winningCombinationCost == 0 || winningCombinationCost > cost)
                        {
                            winningCombinationCost = cost;
                        }
                    }
                }
            }

            tokensSpent += winningCombinationCost;
        }
        Console.WriteLine($"The cost for winning all machines is {tokensSpent}");

        const long newDistance = 10000000000000;
        
        var machinesThatCanBeWon = new Dictionary<Machine, long>();
        foreach (var machine in inputs)
        {
            var machineConfig = GetMachineConfiguration(machine);
            
            // Cramer's Law
            // machineConfig.A.Xmovement * a + machineConfig.B.Xmovement * b = machineConfig.Prize.X + newDistance
            // machineConfig.A.Ymovement * a + machineConfig.B.Ymovement * b = machineConfig.Prize.Y + newDistance

            var denominator = machineConfig.A.Xmovement * machineConfig.B.Ymovement - machineConfig.A.Ymovement * machineConfig.B.Xmovement;
            var aNumerator = machineConfig.B.Ymovement * (machineConfig.Prize.X + newDistance) - machineConfig.B.Xmovement * (machineConfig.Prize.Y + newDistance);

            var bNumerator = machineConfig.A.Xmovement * (machineConfig.Prize.Y + newDistance) - machineConfig.A.Ymovement * (machineConfig.Prize.X + newDistance);

            if (aNumerator % denominator != 0 || bNumerator % denominator != 0)
            {
                continue;
            }

            var a = aNumerator / denominator;
            var b = bNumerator / denominator;
            
            machinesThatCanBeWon.Add(machineConfig, a * 3 + b);
        }
        
        
        Console.WriteLine($"The cost for winning all machines with the new distances is {machinesThatCanBeWon.Values.Sum()}");
    }
        
        


    private static Machine GetMachineConfiguration(string machine)
    {
        var config = machine.Replace(",", string.Empty).Split("\r\n");
        var buttonA = GetButton(config[0]);
        var buttonB = GetButton(config[1]);
        var prizeLocation = config[2].Split(" ").Skip(1).ToArray();
        var prizeX = int.Parse(new string(prizeLocation[0].Skip(2).ToArray()));
        var prizeY = int.Parse(new string(prizeLocation[1].Skip(2).ToArray()));
        var prize = new Point(prizeX, prizeY);

        var machineConfig = new Machine(buttonA, buttonB, prize);
        return machineConfig;
    }

    private static Button GetButton(string config)
    {
        var secondButton = config.Split(" ").Skip(2).ToArray();
        return new Button
        {
            Xmovement = int.Parse(new string(secondButton[0].Skip(2).ToArray())),
            Ymovement = int.Parse(new string(secondButton[1].Skip(2).ToArray()))
        };
    }

    public struct Button
    {
        public int Xmovement { get; set; }
        public int Ymovement { get; set; }
    }

    public record Machine(Button A, Button B, Point Prize);
}