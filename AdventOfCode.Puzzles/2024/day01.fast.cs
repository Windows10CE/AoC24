using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
            Unsafe.Add(ref leftRef, index) = ParseInt(line[..endFirst]);
            Unsafe.Add(ref rightRef, index++) = ParseInt(line[(endFirst + 3)..]);
        }
        
        Array.Sort(leftArr);
        Array.Sort(rightArr);

        uint part1 = 0;
        uint part2 = 0;
        var rightSpan = rightArr.AsSpan(); 
        for (nuint i = 0; i < len; i++)
        {
            var left = Unsafe.Add(ref leftRef, i);
            var right = Unsafe.Add(ref rightRef, i);
            part1 += left > right ? left - right : right - left;

            if (rightSpan.IsEmpty) continue;

            var foundStart = rightSpan.IndexOfAnyExceptInRange(0u, left - 1);
            if (foundStart == -1)
            {
                rightSpan = [];
                continue;
            }

            rightSpan = rightSpan[foundStart..];

            if (MemoryMarshal.GetReference(rightSpan) != left)
                continue;
            
            var newRightIndexSigned = rightSpan.IndexOfAnyExcept(left);

            if (newRightIndexSigned == -1)
            {
                part2 += left * (uint)rightSpan.Length;
                rightSpan = [];
            }
            else
            {
                part2 += left * (uint)newRightIndexSigned;
                rightSpan = rightSpan[newRightIndexSigned..];
            }
        }

        return (part1.ToString(), part2.ToString());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ParseInt(ReadOnlySpan<byte> bytes)
    {
        uint result = 0;
        foreach (var c in bytes)
        {
            result = result * 10 + c - (byte)'0';
        }
        return result;
    }
}
