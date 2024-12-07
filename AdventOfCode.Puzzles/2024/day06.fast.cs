using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 06, CodeType.Fastest)]
public sealed class Day06Fast : IPuzzle
{
    [Flags]
    private enum Direction : byte
    {
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
        // "unused" as literals, but are used as these values
        UpFirst = 16,
        RightFirst = 32,
        DownFirst = 64,
        LeftFirst = 128
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Direction GetNextDirection(Direction d) => d == Direction.Left ? Direction.Up : (Direction)((byte)d << 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (int XOffset, int YOffset) GetDirectionOffsets(Direction d)
    {
        switch (d)
        {
            case Direction.Up:
                return (-1, 0);
            case Direction.Right:
                return (0, 1);
            case Direction.Down:
                return (1, 0);
            case Direction.Left:
                return (0, -1);
            default:
                throw new ArgumentOutOfRangeException(nameof(d));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref byte UnsafeIndex(Span<byte> set, int x, int y, int yLen)
    {
        var index = x * yLen + y;
        if (index == 11310) Debugger.Break();
        return ref Unsafe.Add(ref MemoryMarshal.GetReference(set), index);
    }
    
    private static bool RunSim(Span<byte> input, int x, int xLen, int y, int yLen, Direction direction, Span<byte> visitedSet, out uint visitCountOut)
    {
        var (xOffset, yOffset) = GetDirectionOffsets(direction);
        uint visitCount = 0;
        while (x != 0 && x != xLen - 1 && y != 0 && y != yLen - 1)
        {
            ref var visited = ref UnsafeIndex(visitedSet, x, y, yLen);
            var visitValue = visited;
            byte toWrite = (byte)direction;
            if (visitValue == 0)
            {
                visitCount += 1;
                toWrite |= (byte)(toWrite << 4);
            }
            else if ((visitValue & (byte)direction) != 0)
            {
                visitCountOut = 0;
                return false;
            }
            visited = (byte)(visitValue | toWrite);
            if (UnsafeIndex(input, x + xOffset, y + yOffset, yLen) == (byte)'#')
            {
                direction = GetNextDirection(direction);
                (xOffset, yOffset) = GetDirectionOffsets(direction);
                continue;
            }
            x += xOffset;
            y += yOffset;
        }

        UnsafeIndex(visitedSet, x, y, yLen) |= (byte)(((byte)direction << 4) | (byte)direction);
        visitCountOut = visitCount + 1;
        return true;
    }
    
    public (string, string) Solve(PuzzleInput input)
    {
        int xLen = input.Lines.Length;
        int yLen = input.Lines[0].Length;
        
        var firstVisitedSet = new byte[xLen * yLen].AsSpan();

        var mutableInput = input.Bytes.Where(x => x != '\n').ToArray().AsSpan();

        var startIndex = mutableInput.IndexOf((byte)'^');
        int startX = startIndex / yLen;
        int startY = startIndex % yLen;
        var startDirection = Direction.Up;

        RunSim(mutableInput, startX, xLen, startY, yLen, startDirection, firstVisitedSet, out var visitCount);

        var part1 = visitCount.ToString();

        var loopCount = 0u;
        var part2VisitedSet = new byte[xLen * yLen].AsSpan();

        firstVisitedSet[startIndex] = 0;

        bool doneFirst = false;
        int i = 0;
        while (i < firstVisitedSet.Length)
        {
            var toAdd = firstVisitedSet[(i + 1)..].IndexOfAnyExcept((byte)0);
            if (toAdd == -1)
            {
                break;
            }

            i += toAdd + 1;

            startX = i / yLen;
            startY = i % yLen;
            startDirection = (Direction)(Unsafe.Add(ref MemoryMarshal.GetReference(firstVisitedSet), i) >> 4);
            var (xOffset, yOffset) = GetDirectionOffsets(startDirection);

            ref var c = ref Unsafe.Add(ref MemoryMarshal.GetReference(mutableInput), i);
            var oldValue = c;
            if (!doneFirst)
            {
                part2VisitedSet.Clear();
            }
            else
            {
                doneFirst = true;
            }
            c = (byte)'#';
            if (!RunSim(mutableInput, i / yLen - xOffset, xLen, i % yLen - yOffset, yLen, startDirection, part2VisitedSet, out _))
            {
                loopCount += 1;
            }
            c = oldValue;
        }

        var part2 = loopCount.ToString();

        return (part1, part2);
    }
}