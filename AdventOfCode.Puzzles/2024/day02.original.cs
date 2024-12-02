namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 02, CodeType.Original)]
public sealed class Day02Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var reports = input.Lines.Select(l => l.Split(' ').Select(int.Parse).ToArray()).ToArray();

        var part1 = reports.Count(r =>
        {
            bool increasing = r[0] < r[1];
            return r.Window(2).All(w => (increasing ? w[1] - w[0] : w[0] - w[1]) is >= 1 and <= 3);
        }).ToString();
        
        var part2 = reports.Count(r =>
            Enumerable.Range(0, r.Length)
                .Select(int[] (i) => [..r[..i], ..r[(i + 1)..]])
                .Prepend(r)
                .Any(r2 =>
                {
                    bool increasing = r2[0] < r2[1];
                    return r2.Window(2).All(w => (increasing ? w[1] - w[0] : w[0] - w[1]) is >= 1 and <= 3);
                })
        ).ToString();

        return (part1, part2);
    }
}
