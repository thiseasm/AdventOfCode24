namespace AdventOfCode.Days;

public abstract class DayBase
{
    public abstract void Start();

    private static readonly string Directory = Path.Combine(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, @"Inputs\");

    protected static string[] ReadFile(string file)
    {
        var inputPath = Path.Combine(Directory, file);
        return File.ReadAllLines(inputPath);
    }
    protected static string ReadText(string file)
    {
        var inputPath = Path.Combine(Directory, file);
        return File.ReadAllText(inputPath);
    }
    protected static int[] ReadIntegerFile(string file)
    {
        var inputPath = Path.Combine(Directory, file);
        var inputsRaw = File.ReadAllLines(inputPath);

        return inputsRaw.Select(input => int.Parse(input.Trim())).ToArray();
    }

    protected static long[] ReadLongFile(string file)
    {
        var inputPath = Path.Combine(Directory, file);
        var inputsRaw = File.ReadAllLines(inputPath);

        return inputsRaw.Select(input => long.Parse(input.Trim())).ToArray();
    }

    protected int[] ReadCsv(string file)
    {
        return ReadSv(file, ',');
    }

    protected static int[] ReadSv(string file, char separator)
    {
        var inputPath = Path.Combine(Directory, file);
        var inputRaw = File.ReadAllText(inputPath).Split(separator);

        return inputRaw.Select(input => int.Parse(input.Trim())).ToArray();
    }

    protected static int[] ReadSpaceSeparatedIntegers(string file)
    {
        var inputPath = Path.Combine(Directory, file);
        var inputsRaw = File.ReadAllLines(inputPath);

        return inputsRaw.Select(input => input.Equals(string.Empty)
            ? -1
            : int.Parse(input.Trim())
        ).ToArray();
    }
}