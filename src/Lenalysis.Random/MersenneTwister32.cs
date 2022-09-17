namespace Lenalysis.Random;

/// <summary>
/// A 32-bit Mersenne Twister RNG implementation based on the pseudocode on the wikipedia page.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class MersenneTwister32: IIntegerSequence32
{
    /// <summary>
    /// Construct a MT 64-bit instance using the provided seed.
    /// </summary>
    /// <param name="seed">A seed value used to construct the sequence</param>
    public MersenneTwister32(uint seed)
    {
        MT[0] = seed;
        for (var i = 1; i < n; i++)
            MT[i] = f * (MT[i-1] ^ (MT[i - 1] >> (w - 2))) + (uint)i;
        index = n;
    }

    /// <summary>
    /// State representation for MT32 state, to allow save/restore/snapshotting behavior.
    /// </summary>
    public class State
    {
        /// <summary>
        /// Constructs the state from an index and matrix (the two items stored internally by the MT32 instance.
        /// </summary>
        /// <param name="index">The index into the state matrix for the current position</param>
        /// <param name="matrix">The state matrix of values</param>
        /// <exception cref="ArgumentNullException">Matrix cannot be null</exception>
        /// <exception cref="InvalidOperationException">The matrix was either of the wrong length (624 entries), or index was not between zero and the length of the matrix (inclusive)</exception>
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

        /// <summary>
        /// Index into the matrix to read the next value from
        /// </summary>
        public readonly uint Index;

        /// <summary>
        /// The matrix of currently constructed values
        /// </summary>
        public readonly uint[] Matrix;
    }

    /// <summary>
    /// Construct a MT 32-bit sequence from state that was previously exported by an object of this type
    /// </summary>
    /// <param name="stateToRestore">The previously exported state</param>
    public MersenneTwister32(State stateToRestore)
    {
        index = stateToRestore.Index;
#if NETSTANDARD2_0
        Array.Copy(stateToRestore.Matrix, MT, n);
#else
            stateToRestore.Matrix.CopyTo(MT.AsSpan());
#endif
    }

    /// <summary>
    /// Export the state for this MT32 object to allow restore later.
    /// </summary>
    /// <returns>An exported state suitable for restoring later</returns>
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

    /// <summary>
    /// Returns the next value in the MT32 sequence
    /// </summary>
    /// <returns>The next value</returns>
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
