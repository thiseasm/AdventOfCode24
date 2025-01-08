namespace AdventOfCode.Days;

public class Day9 : DayBase
{
    public override void Start()
    {
        var inputs = ReadText("Day9.txt");

        CalculateFirstPart(inputs);
        
        CalculateSecondPart(inputs);
    }

    private static void CalculateSecondPart(string inputs)
    {
        var filesystem = GetFilesystem(inputs);
        
        for (var pointer = filesystem.Count - 1; pointer >= 0; pointer--)
        {
            var currentBlock = filesystem[pointer];
            if (currentBlock.Type == FileType.FreeSpace)
            {
                continue;
            }
                
            var indexOfFirstEmptySpace = filesystem.FindIndex(x => x.Space >= currentBlock.Space && x.Type == FileType.FreeSpace);
            if (indexOfFirstEmptySpace < 0 || indexOfFirstEmptySpace > pointer)
            {
                continue;
            }

            var emptySpace = filesystem[indexOfFirstEmptySpace];
            var emptySpaceCapacity = emptySpace.Space;
            if (currentBlock.Space < emptySpaceCapacity)
            {
                emptySpace.Space = currentBlock.Space;
            }
            filesystem[pointer] = emptySpace;
            
            var firstPart = filesystem[..(indexOfFirstEmptySpace + 1)];
            var secondPart = filesystem[(indexOfFirstEmptySpace + 1)..];
                
            firstPart[^1] = currentBlock;

            if (currentBlock.Space < emptySpaceCapacity)
            {
                firstPart.Add(new FileBlock
                {
                    Space = emptySpaceCapacity - currentBlock.Space,
                    Type = FileType.FreeSpace
                });
            }

            firstPart.AddRange(secondPart);
            filesystem = firstPart;
            
            for (var index = 0; index < filesystem.Count - 1; index++)
            {
                if (filesystem[index].Type == FileType.File || filesystem[index + 1].Type == FileType.File)
                {
                    continue;
                }
            
                var spaceToMerge = filesystem[index];
                spaceToMerge.Space += filesystem[index + 1].Space;
                filesystem[index] = spaceToMerge;
                filesystem.RemoveAt(index + 1);
                index--;
            }
        }
      
        long checksum = 0;
        var fileSystemSorted = new List<string>();

        foreach (var element in filesystem)
        {
            for (var index = 0; index < element.Space; index++)
            {
                fileSystemSorted.Add( element.Type == FileType.File ? element.Id.ToString() : ".");
            }
        }

        for (var index = 0; index < fileSystemSorted.Count; index++)
        {
            if (fileSystemSorted[index].Equals("."))
            {
                continue;
            }
            checksum += int.Parse(fileSystemSorted[index]) * index;
        }
        
        Console.WriteLine($"The filesystem checksum is: {checksum}");
    }

    private static List<FileBlock> GetFilesystem(string inputs)
    {
        var fileId = 0;
        var isFile = true;

        var filesystem = new List<FileBlock>();
        foreach (var block in inputs)
        {
            var size = int.Parse(block.ToString());
            var currentBlock =new FileBlock
            {
                Id = isFile ? fileId : 0,
                Space = size,
                Type = isFile ? FileType.File : FileType.FreeSpace
            };

            filesystem.Add(currentBlock);

            if (isFile)
            {
                fileId++;
            }

            isFile = !isFile;
        }

        return filesystem;
    }

    private static void CalculateFirstPart(string inputs)
    {
        var fileId = 0;
        var isFile = true;

        var filesystem = new List<string>();
        foreach (var block in inputs)
        {
            var size = int.Parse(block.ToString());
            var currentBlock =new FileBlock
            {
                Id = isFile ? fileId : 0,
                Space = size,
                Type = isFile ? FileType.File : FileType.FreeSpace
            };

            for (var counter = 0; counter < size; counter++)
            {
                filesystem.Add(isFile ? currentBlock.Id.ToString() : ".");
            }

            if (isFile)
            {
                fileId++;
            }

            isFile = !isFile;
        }

        while (filesystem.Any(x => x.Equals(".")))
        {
            for (var pointer = filesystem.Count - 1; pointer >= 0; pointer-- )
            {
                var currentBlock = filesystem[pointer];
                var indexOfFirstEmptySpace = filesystem.FindIndex(x => x.Equals("."));
                if (indexOfFirstEmptySpace < 0)
                {
                    break;
                }

                filesystem[indexOfFirstEmptySpace] = currentBlock;
                filesystem.RemoveAt(pointer);
            }   
        }

        long checksum = 0;

        for (var index = 0; index < filesystem.Count; index++)
        {
            checksum += int.Parse(filesystem[index]) * index;
        }
        
        Console.WriteLine($"The filesystem checksum is: {checksum}");
    }

    enum FileType
    {
        File,
        FreeSpace
    }

    struct FileBlock
    {
        public int Id { get; set; }
        public int Space { get; set; }
        public FileType Type { get; set; }
    }
}