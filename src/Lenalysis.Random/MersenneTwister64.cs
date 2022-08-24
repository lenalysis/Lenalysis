using System;
using System.Diagnostics.CodeAnalysis;

// Based on https://en.wikipedia.org/wiki/Mersenne_Twister pseudocode
namespace Lenalysis.Random;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class MersenneTwister64: IIntegerSequence64
{
    public MersenneTwister64(ulong seed)
    {
        MT[0] = seed;
        for (var i = 1; i < n; i++)
            MT[i] = f * (MT[i-1] ^ (MT[i - 1] >> (w - 2))) + (ulong)i;
        index = n;
    }

    public class State
    {
        public State(uint index, [NotNull] ulong[] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (matrix.Length != n)
                throw new InvalidOperationException("Incorrect length of matrix.  Must be 'n' = 312 for 64-bit MT");
            if (index > n)
                throw new InvalidOperationException("Incorrect index.  It should be between 0 and 'n' inclusive (n = 312) for 64-bit MT");

            Index = index;
            Matrix = matrix;
        }

        public readonly uint Index;
        public readonly ulong[] Matrix;
    }

    public MersenneTwister64(State stateToRestore)
    {
        index = stateToRestore.Index;
#if NETSTANDARD2_0
            Array.Copy(stateToRestore.Matrix, MT, n);
#else
        stateToRestore.Matrix.CopyTo(MT.AsSpan());
#endif
    }

    public State ExportState()
    {
        var clone = new ulong[n];
#if NETSTANDARD2_0
            Array.Copy(MT, clone, n);
#else
        MT.CopyTo(clone.AsSpan());
#endif
        return new State(index, clone);
    }

    public ulong Next()
    {
        if (index >= n)
            twist();

        var y = MT[index++];

        y ^= (y >> u) & d;
        y ^= (y << s) & b;
        y ^= (y << t) & c;
        y ^= y >> l;

        return y;
    }

    private void twist()
    {
        const ulong lMask = (1ul << r) - 1;
        const ulong uMask = ~lMask;

        for (var i = 0; i < n; i++)
        {
            var x = (MT[i] & uMask) | (MT[(i + 1) % n] & lMask);
            var xA = x >> 1;
            if (x % 2 != 0)
                xA ^= a;
            MT[i] = MT[(i + m) % n] ^ xA;
        }
        index = 0;
    }

    // MT19937-64 parameters per wikipedia.
    private const int w = 64, n = 312, m = 156, r = 31;
    private const ulong a = 0xB5026F5AA96619E9;

    private const int u = 29;
    private const ulong d = 0x5555555555555555ul;

    private const int s = 17;
    private const ulong b = 0x71D67FFFEDA60000ul;

    private const int t = 37;
    private const ulong c = 0xFFF7EEE000000000ul;

    private const int l = 43;

    // only for seeding.
    private const ulong f = 6364136223846793005ul;

    // matrix
    private readonly ulong[] MT = new ulong[n];
    private uint index;
}