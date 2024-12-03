namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 03, CodeType.Original)]
public sealed partial class Day03Original : IPuzzle
{
    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex { get; }

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex InstructionRegex { get; }

    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = MulRegex.Matches(input.Text).Sum(m =>
            long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[2].Value)
        ).ToString();

        var part2 = InstructionRegex.Matches(input.Text).Aggregate((true, 0L), (current, m) =>
            m.Value switch
            {
                "do()" => (true, current.Item2),
                "don't()" => (false, current.Item2),
                _ when current.Item1 => (current.Item1, current.Item2 + long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[2].Value)),
                _ => current
            })
            .Item2.ToString();

        return (part1, part2);
    }
}