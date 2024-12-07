using System.Text;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 06, CodeType.Original)]
public sealed class Day06Original : IPuzzle
{
    private static (int XOffset, int YOffset) GetDirectionOffsets(char d) => d switch
    {
        '^' => (-1, 0),
        'v' => (1, 0),
        '<' => (0, -1),
        '>' => (0, 1),
        _ => throw new ArgumentOutOfRangeException(nameof(d))
    };

    private static char GetNextDirection(char d) => d switch
    {
        '^' => '>',
        '>' => 'v',
        'v' => '<',
        '<' => '^',
        _ => throw new ArgumentOutOfRangeException(nameof(d))
    };

    private static bool RunSim(StringBuilder[] lines, int x, int y, char direction, HashSet<(int X, int Y, char Direction)> visitedSet)
    {
        while (x != 0 && x != lines.Length - 1 && y != 0 && y != lines[0].Length - 1)
        {
            if (!visitedSet.Add((x, y, direction)))
            {
                return false;
            }
            var (xOffset, yOffset) = GetDirectionOffsets(direction);
            if (lines[x + xOffset][y + yOffset] == '#')
            {
                direction = GetNextDirection(direction);
                continue;
            }
            x += xOffset;
            y += yOffset;
        }

        visitedSet.Add((x, y, direction));
        return true;
    }
    
    public (string, string) Solve(PuzzleInput input)
    {
        var visitedSet = new HashSet<(int X, int Y, char Direction)>();

        var mutableLines = input.Lines.Select(s => new StringBuilder(s)).ToArray();

        int startX = -1;
        int startY = -1;
        char startDirection = '\0';
        for (int i = 0; i < input.Lines.Length; i++)
        {
            if (input.Lines[i].AsSpan().IndexOfAny(['^', '>', '<', 'v']) is var index and not -1)
            {
                startX = i;
                startY = index;
                startDirection = input.Lines[i][index];
                break;
            }
        }

        RunSim(mutableLines, startX, startY, startDirection, visitedSet);
        var part1 = visitedSet.DistinctBy(v => (v.X, v.Y)).Count().ToString();

        var loopCount = 0;
        
        for (int i = 0; i < input.Lines.Length; i++)
        {
            for (int j = 0; j < input.Lines[0].Length; j++)
            {
                if (i == startX && j == startY)
                {
                    continue;
                }
                var oldValue = mutableLines[i][j];
                if (oldValue == '#')
                {
                    continue;
                }
                visitedSet.Clear();
                mutableLines[i][j] = '#';
                if (!RunSim(mutableLines, startX, startY, startDirection, visitedSet))
                {
                    loopCount += 1;
                }
                mutableLines[i][j] = oldValue;
            }
        }

        var part2 = loopCount.ToString();

        return (part1, part2);
    }
}