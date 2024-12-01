using System.Runtime.CompilerServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 01, CodeType.Fastest)]
public sealed class Day01Fast : IPuzzle
{
    [SkipLocalsInit]
    public (string, string) Solve(PuzzleInput input)
    {
        var span = input.Span;

        var len = (nuint)input.Lines.Length;
        uint[] leftArr = GC.AllocateUninitializedArray<uint>((int)len), rightArr = GC.AllocateUninitializedArray<uint>((int)len);
        ref var leftRef = ref leftArr[0];
        ref var rightRef = ref rightArr[0];
        
        nuint index = 0;
        foreach (var lineRange in input.Span.Split((byte)'\n'))
        {
            var line = span[lineRange];
            if (line.IsEmpty) break;
            var endFirst = line.IndexOf((byte)' ');
            _ = uint.TryParse(line[..endFirst], out Unsafe.Add(ref leftRef, index));
            _ = uint.TryParse(line[(endFirst + 3)..], out Unsafe.Add(ref rightRef, index++));
        }
        
        Array.Sort(leftArr);
        Array.Sort(rightArr);

        uint part1 = 0;
        uint part2 = 0;
        nuint rightIndex = 0;
        for (nuint i = 0; i < len; i++)
        {
            var left = Unsafe.Add(ref leftRef, i);
            var right = Unsafe.Add(ref rightRef, i);
            part1 += left > right ? left - right : right - left;

            if (rightIndex >= len) continue;

            rightIndex = (nuint)rightArr.AsSpan((int)rightIndex..).IndexOfAnyExceptInRange(0u, left - 1) + rightIndex;

            var val = Unsafe.Add(ref rightRef, rightIndex);
            if (val != left)
                continue;
            
            var newRightIndexSigned = rightArr.AsSpan((int)rightIndex..).IndexOfAnyExcept(left);

            var newRightIndex = newRightIndexSigned == -1 ? len : (nuint)newRightIndexSigned + rightIndex;

            part2 += left * (uint)(newRightIndex - rightIndex);

            rightIndex = newRightIndex;
        }

        return (part1.ToString(), part2.ToString());
    }
}
