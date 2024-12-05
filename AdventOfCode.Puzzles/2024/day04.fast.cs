namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 04, CodeType.Fastest)]
public sealed class Day04Fast : IPuzzle
{
    private static bool CheckEqualsWithStride(string[] arr, string toCheck, int x, int y, int strideX, int strideY)
    {
        var len = arr.Length;
        foreach (var c in toCheck)
        {
            if (x < 0 || x >= len || y < 0 || y >= len)
            {
                return false;
            }
            if (c != arr[x][y])
            {
                return false;
            }
            x += strideX;
            y += strideY;
        }
        return true;
    }
    
    public (string, string) Solve(PuzzleInput input)
    {
        ReadOnlySpan<(int X, int Y)> strides = [(0, 1), (0, -1), (1, 0), (1, 1), (1, -1), (-1, 0), (-1, 1), (-1, -1)];

        var lines = input.Lines;
        var len = lines.Length;
        int count = 0;
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                if (lines[i][j] != 'X')
                {
                    continue;
                }
                foreach (var (strideX, strideY) in strides)
                {
                    if (CheckEqualsWithStride(lines, "XMAS", i, j, strideX, strideY))
                    {
                        count += 1;
                    }
                }
            }
        }

        var part1 = count.ToString();

        count = 0;
        
        for (int i = 1; i < len - 1; i++)
        {
            for (int j = 1; j < len - 1; j++)
            {
                if (lines[i][j] != 'A')
                {
                    continue;
                }
                if (
                    (CheckEqualsWithStride(lines, "MAS", i - 1, j - 1, 1, 1) || CheckEqualsWithStride(lines, "SAM", i - 1, j - 1, 1, 1))
                    && (CheckEqualsWithStride(lines, "MAS", i + 1, j - 1, -1, 1) || CheckEqualsWithStride(lines, "SAM", i + 1, j - 1, -1, 1))
                )
                {
                    count += 1;
                }
            }
        }

        var part2 = count.ToString();

        return (part1, part2);
    }
}