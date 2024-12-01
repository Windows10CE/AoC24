namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 01, CodeType.Original)]
public sealed class Day01Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        List<long> left = [], right = [];

        foreach (var line in input.Lines)
        {
            var split = line.Split("   ");
            left.Add(long.Parse(split[0]));
            right.Add(long.Parse(split[1]));
        }

        var part1 = left.Order().Zip(right.Order()).Select(x => long.Abs(x.First - x.Second)).Sum().ToString();

        var counts = right.CountBy(x => x).ToDictionary();

        var part2 = left.Select(x => x * counts.GetValueOrDefault(x, 0)).Sum().ToString();

        return (part1, part2);
    }
}
