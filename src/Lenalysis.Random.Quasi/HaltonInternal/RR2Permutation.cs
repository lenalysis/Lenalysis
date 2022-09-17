namespace Lenalysis.Random.Quasi;

/// <summary>
/// Implements the HaltonRR2 permutation of digits in the Halton sequence
/// per the improvements recommended by the paper (TODO).
/// </summary>
public static class RR2Permutation
{
    /// <summary>
    /// Make the permutation of digits applicable to the beses supplied by the caller.
    ///
    /// The permutation is based on a reversed bit ordering, see tests for some example sequences.
    /// </summary>
    /// <param name="bases">The bases to be used in the Halton sequence (generally recommended to be mutually prime numbers, typically 2, 3, 5, 7, 11, etc.)</param>
    /// <returns>The shuffled digits for each base.  If the base is N there should be one entry for each [0,N) integer, with the value in position x of the array representing the shuffled value for the digit value x.</returns>
    public static int[][] MakePermutation(int[] bases)
    {
        var ret = bases.Select(x => new int[x]).ToArray();
        var counts = new int[bases.Length];
        var maxBits = _CountBits(bases.Max());
        var shiftBits = 32 - maxBits;
        var maxValue = 1 << maxBits;
        for (var i = 0; i < maxValue; i++)
        {
            var rev = _BitReverse(i, shiftBits);
            for (var j = 0; j < bases.Length; j++)
            {
                if (rev < bases[j])
                    ret[j][counts[j]++] = rev;
            }
        }
        return ret;
    }

    private static int _CountBits(int value)
    {
        var bits = 1;
        while ((value >>= 1) != 0)
            bits++;
        return bits;
    }

    private static int _BitReverse(int source, int shift)
    {
        var x = (uint)source;
        x = (x >> 16) | (x << 16);
        x = ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        x = ((x & 0xF0F0F0F0) >> 4) | ((x & 0x0F0F0F0F) << 4);
        x = ((x & 0xCCCCCCCC) >> 2) | ((x & 0x33333333) << 2);
        var ret = ((x & 0xAAAAAAAA) >> 1) | ((x & 0x55555555) << 1);
        ret >>= shift;
        return (int)ret;
    }
}
