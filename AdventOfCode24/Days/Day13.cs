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
            var machineConfig = GetMachineConfiguration(machine, out var prize);
            long winningCombinationsCost = 0;
            
            for (var a = 0; a < 100; a++)
            {
                for (var b = 0; b < 100; b++)
                {
                    var xResult = a * machineConfig.A.Xmovement + b * machineConfig.B.Xmovement;
                    var yResult = a * machineConfig.A.Ymovement + b * machineConfig.B.Ymovement;

                    if (xResult == prize.X && yResult == prize.Y)
                    {
                        var cost = a * 3 + b;
                        if (winningCombinationsCost == 0 || winningCombinationsCost > cost)
                        {
                            winningCombinationsCost = cost;
                        }
                    }
                }
            }

            tokensSpent += winningCombinationsCost;

        }
        Console.WriteLine($"The cost for winning all machines is {tokensSpent}");

        const long newDistance = 10000000000000;
        
        
    }

    private static Machine GetMachineConfiguration(string machine, out Point prize)
    {
        var config = machine.Replace(",", string.Empty).Split("\r\n");
        var buttonA = GetButton(config[0]);
        var buttonB = GetButton(config[1]);
        var prizeLocation = config[2].Split(" ").Skip(1).ToArray();
        var prizeX = int.Parse(new string(prizeLocation[0].Skip(2).ToArray()));
        var prizeY = int.Parse(new string(prizeLocation[1].Skip(2).ToArray()));
        prize = new Point(prizeX, prizeY);

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