using System;
using System.Diagnostics.CodeAnalysis;

// Based on https://en.wikipedia.org/wiki/Mersenne_Twister pseudocode
namespace Lenalysis.Random;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class MersenneTwister32: IIntegerSequence32
{
    public MersenneTwister32(uint seed)
    {
        MT[0] = seed;
        for (var i = 1; i < n; i++)
            MT[i] = f * (MT[i-1] ^ (MT[i - 1] >> (w - 2))) + (uint)i;
        index = n;
    }

    public class State
    {
        public State(uint index, [NotNull] uint[] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (matrix.Length != n)
                throw new InvalidOperationException("Incorrect length of matrix.  Must be 'n' = 624 for 32-bit MT");
            if (index > n)
                throw new InvalidOperationException("Incorrect index.  It should be between 0 and 'n' inclusive (n = 624) for 32-bit MT");

            Index = index;
            Matrix = matrix;
        }

        public readonly uint Index;
        public readonly uint[] Matrix;
    }

    public MersenneTwister32(State stateToRestore)
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
        var clone = new uint[n];
#if NETSTANDARD2_0
        Array.Copy(MT, clone, n);
#else
            MT.CopyTo(clone.AsSpan());
#endif
        return new State(index, clone);
    }

    public uint Next()
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
        const uint lMask = (1u << r) - 1;
        const uint uMask = ~lMask;

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

    // MT19937 (32) parameters per wikipedia.
    private const int w = 32, n = 624, m = 397, r = 31;
    private const uint a = 0x9908B0DF;

    private const int u = 11;
    private const uint d = 0xFFFFFFFF;

    private const int s = 7;
    private const uint b = 0x9D2C5680;

    private const int t = 15;
    private const uint c = 0xEFC60000;

    private const int l = 18;

    // only for seeding.
    private const uint f = 1812433253;

    // matrix
    private readonly uint[] MT = new uint[n];
    private uint index;
}
