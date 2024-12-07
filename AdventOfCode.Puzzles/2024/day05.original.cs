using System.Diagnostics;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 05, CodeType.Original)]
public sealed class Day05Original : IPuzzle
{
    private static bool CheckUpdate((int Before, int After)[] rules, int[] update)
    {
        for (int i = 0; i < update.Length - 1; i++)
        {
            for (int j = i + 1; j < update.Length; j++)
            {
                if (rules.Contains((update[j], update[i])))
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    public (string, string) Solve(PuzzleInput input)
    {
        if (input.Lines.Split("").ToArray() is not [var ruleStrings, var updateStrings]) throw new UnreachableException();

        var rules = ruleStrings.Select(r => r.Split('|')).Select(r => (Before: int.Parse(r[0]), After: int.Parse(r[1]))).ToArray();

        var updates = updateStrings.Select(u => u.Split(',').Select(int.Parse).ToArray()).ToArray();
        
        var part1 = updates.Where(u => CheckUpdate(rules, u)).Sum(u => u[u.Length / 2]).ToString();

        var badUpdates = updates.Where(u => !CheckUpdate(rules, u));

        var sum = 0;

        foreach (var update in badUpdates)
        {
            var sorted = new List<int>(update.Length);
            var tempRules = rules.Where(r => update.Contains(r.Before) && update.Contains(r.After)).ToList();
            var nodeQueue = new Queue<int>(update.Where(p => tempRules.All(r => r.After != p)));
            while (nodeQueue.TryDequeue(out var n))
            {
                sorted.Add(n);
                while (tempRules.FindIndex(r => r.Before == n) is var index and not -1)
                {
                    var next = tempRules[index].After;
                    tempRules.RemoveAt(index);
                    if (tempRules.All(r => r.After != next))
                    {
                        nodeQueue.Enqueue(next);
                    }
                }
            }

            sum += sorted[sorted.Count / 2];
        }

        var part2 = sum.ToString();

        return (part1, part2);
    }
}