namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 04, CodeType.Original)]
public sealed partial class Day04Original : IPuzzle
{
    private static bool TryCopyToWithStride(string[] arr, Span<char> buffer, int x, int y, int strideX, int strideY)
    {
        var len = arr.Length;
        Span<char> span = buffer;
        for (int i = 0; i < span.Length; i++)
        {
            if (x < 0 || x >= len || y < 0 || y >= len)
            {
                return false;
            }
            span[i] = arr[x][y];
            x += strideX;
            y += strideY;
        }
        return true;
    }
    
    public (string, string) Solve(PuzzleInput input)
    {
        Span<char> buf = stackalloc char[4];

        ReadOnlySpan<(int X, int Y)> strides = [(0, 1), (0, -1), (1, 0), (1, 1), (1, -1), (-1, 0), (-1, 1), (-1, -1)];

        var lines = input.Lines;
        var len = lines.Length;
        int count = 0;
        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < len; j++)
            {
                foreach (var (strideX, strideY) in strides)
                {
                    if (TryCopyToWithStride(lines, buf, i, j, strideX, strideY) && buf is "XMAS")
                    {
                        count += 1;
                        buf.Clear();
                    }
                }
            }
        }

        var part1 = count.ToString();

        count = 0;
        buf = buf[..^1];
        
        for (int i = 1; i < len - 1; i++)
        {
            for (int j = 1; j < len - 1; j++)
            {
                if (TryCopyToWithStride(lines, buf, i - 1, j - 1, 1, 1) && buf is "MAS" or "SAM" && TryCopyToWithStride(lines, buf, i + 1, j - 1, -1, 1) && buf is "MAS" or "SAM")
                {
                    count += 1;
                    buf.Clear();
                }
            }
        }

        var part2 = count.ToString();

        return (part1, part2);
    }
}