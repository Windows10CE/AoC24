namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 08, CodeType.Original)]
public sealed class Day08Original : IPuzzle
{
    private static void HandleSignal(string[] lines, int x, int y, HashSet<(int X, int Y)> seenSet, HashSet<(int X, int Y)> seenSetPart2)
    {
        char signal = lines[x][y];
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] == signal && i != x && j != y)
                {
                    seenSetPart2.Add((i, j));
                    var xDiff = x - i;
                    var yDiff = y - j;
                    var antiX = x + xDiff;
                    var antiY = y + yDiff;
                    if (antiX >= 0 && antiX < lines.Length && antiY >= 0 && antiY < line.Length)
                    {
                        seenSet.Add((antiX, antiY));
                    }
                    else
                    {
                        continue;
                    }
                    do
                    {
                        seenSetPart2.Add((antiX, antiY));
                        antiX += xDiff;
                        antiY += yDiff;
                    } while (antiX >= 0 && antiX < lines.Length && antiY >= 0 && antiY < line.Length);

                    (antiX, antiY) = (x - xDiff, y - yDiff);
                    while (antiX >= 0 && antiX < lines.Length && antiY >= 0 && antiY < line.Length)
                    {
                        seenSetPart2.Add((antiX, antiY));
                        antiX -= xDiff;
                        antiY -= yDiff;
                    }
                }
            }
        }
    }
    
    public (string, string) Solve(PuzzleInput input)
    {
        var seenSet = new HashSet<(int X, int Y)>();
        var seenSetPart2 = new HashSet<(int X, int Y)>();

        var lines = input.Lines;
        
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j] != '.')
                {
                    HandleSignal(lines, i, j, seenSet, seenSetPart2);
                }
            }
        }

        var part1 = seenSet.Count.ToString();
        var part2 = seenSetPart2.Count.ToString();

        return (part1, part2);
    }
}
