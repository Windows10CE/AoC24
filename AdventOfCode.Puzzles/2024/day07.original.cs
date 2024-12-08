using System.Globalization;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 07, CodeType.Original)]
public sealed class Day07Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var lines = input.Lines.Select(l =>
        {
            var parts = l.Split(": ");
            return (Target: ulong.Parse(parts[0]), Numbers: parts[1].Split(' ').Select(ulong.Parse).ToArray());
        });

        var (oldPart1, currentPart1) = (new HashSet<ulong>(), new HashSet<ulong>());
        var (oldPart2, currentPart2) = (new HashSet<ulong>(), new HashSet<ulong>());

        ulong sumPart1 = 0;
        ulong sumPart2 = 0;

        Span<char> numScratch = stackalloc char[20];
        
        foreach (var line in lines)
        {
            oldPart1.Add(line.Numbers[0]);
            oldPart2.Add(line.Numbers[0]);
            foreach (var next in line.Numbers.AsSpan(1..))
            {
                foreach (var previous in oldPart1)
                {
                    var add = previous + next;
                    var mul = previous * next;
                    if (add <= line.Target)
                    {
                        currentPart1.Add(add);
                    }
                    if (mul <= line.Target)
                    {
                        currentPart1.Add(mul);
                    }
                }
                foreach (var previous in oldPart2)
                {
                    var add = previous + next;
                    var mul = previous * next;
                    _ = previous.TryFormat(numScratch, out var firstWritten, "", NumberFormatInfo.InvariantInfo);
                    var concatSuccess = next.TryFormat(numScratch[firstWritten..], out var secondWritten, "", NumberFormatInfo.InvariantInfo);
                    if (add <= line.Target)
                    {
                        currentPart2.Add(add);
                    }
                    if (mul <= line.Target)
                    {
                        currentPart2.Add(mul);
                    }
                    if (concatSuccess)
                    {
                        var concat = ulong.Parse(numScratch[..(firstWritten + secondWritten)]);
                        if (concat <= line.Target)
                        {
                            currentPart2.Add(concat);
                        }
                    }
                }
                oldPart1.Clear();
                oldPart2.Clear();
                (oldPart1, currentPart1, oldPart2, currentPart2) = (currentPart1, oldPart1, currentPart2, oldPart2);
            }

            if (oldPart1.Contains(line.Target))
            {
                sumPart1 += line.Target;
            }
            if (oldPart2.Contains(line.Target))
            {
                sumPart2 += line.Target;
            }
            oldPart1.Clear();
            oldPart2.Clear();
        }
        
        var part1 = sumPart1.ToString();
        var part2 = sumPart2.ToString();

        return (part1, part2);
    }
}
