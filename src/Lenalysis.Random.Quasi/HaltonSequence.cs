using Lenalysis.Random.Quasi.SharedInternal;

namespace Lenalysis.Random.Quasi;

/// <summary>
/// Implements the Halton sequence with support for skips, leaps, custom selection of bases, and the RR2 digit
/// shuffling recommended by the paper (TODO).
/// </summary>
public class HaltonSequence: IPointSequence
{
    private readonly int[] _bases;
    private readonly int[] _leaps;
    private readonly int[] _seeds;
    private readonly int[][] _permutation;
    private int _currentStep;

    /// <summary>
    /// The simplest constructor for the Halton sequence.
    ///
    /// Uses default settings for skip (0), leap (1), bases (2, 3, 5, 7, 11, ...), and no RR2 shuffling.
    /// </summary>
    /// <param name="dimensions">The number of dimensions desired</param>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="dimensions"/> argument must be greater than 0</exception>
    public HaltonSequence(int dimensions)
    {
        if (dimensions < 1)
            throw new ArgumentOutOfRangeException(nameof(dimensions), dimensions,
                "Dimensions should be greater or equal to 1");
        Dimensions = dimensions;
        _bases = _DefaultBases();
        _leaps = _DefaultLeaps();
        _seeds = _DefaultSeeds();
        _permutation = _IdentPermutation();
    }

    /// <summary>
    /// Constructs a Halton sequence with custom bases.  The bases should be relatively prime.
    /// </summary>
    /// <param name="dimensions">The number of dimensions desired</param>
    /// <param name="bases">The bases to use for the sequence (should be relatively prime)</param>
    /// <exception cref="ArgumentException">The <paramref name="bases"/> must be relatively prime (no multiples of each other)</exception>
    public HaltonSequence(int dimensions, int[] bases)
    {
        if (!PrimeNumbers.CheckRelativePrime(bases))
            throw new ArgumentException("The bases should be relatively prime.", nameof(bases));
        Dimensions = dimensions;
        _bases = bases;
        _leaps = _DefaultLeaps();
        _seeds = _DefaultSeeds();
        _permutation = _IdentPermutation();
    }

    /// <summary>
    /// Constructs a Halton sequence with support for scrambling the digits using the RR2 method described in (TODO).
    /// </summary>
    /// <param name="dimensions">The number of dimensions desired</param>
    /// <param name="scrambleRR2">Whether to use the scramble or not (true to use, false to use normal unscrambled sequence)</param>
    public HaltonSequence(int dimensions, bool scrambleRR2)
    {
        Dimensions = dimensions;
        _bases = _DefaultBases();
        _leaps = _DefaultLeaps();
        _seeds = _DefaultSeeds();
        _permutation = scrambleRR2
            ? _RR2Permutation()
            : _IdentPermutation();
    }

    /// <summary>
    /// Constructs a Halton sequence with support for scrambling using the RR2 method described in paper (TODO).
    ///
    /// Allows specification of single skip and leap parameters that are used uniformly across all dimensions.
    ///
    /// Skip causes the first <paramref name="skip"/> points in the sequence to be skipped, while Leap causes the sequence to return every <paramref name="leap"/>th element.
    /// </summary>
    /// <param name="dimensions">The number of dimensions desired</param>
    /// <param name="skip">The number of initial points to skip</param>
    /// <param name="leap">The number of steps forward to skip each time a point is returned in the sequence</param>
    /// <param name="scrambleRR2">Whether to scramble digits using RR2</param>
    /// <exception cref="ArgumentOutOfRangeException">The skip argument must be greater or equal to 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The leap argument must be greater or equal to 1.</exception>
    public HaltonSequence(int dimensions, int skip, int leap, bool scrambleRR2)
    {
        if (skip < 0)
            throw new ArgumentOutOfRangeException(nameof(skip), skip, "Skip should be greater or equal to zero");
        if (leap < 1)
            throw new ArgumentOutOfRangeException(nameof(leap), leap, "Leap should be greater or equal to 1");
        Dimensions = dimensions;
        _bases = _DefaultBases();
        _leaps = Enumerable.Repeat(leap, Dimensions).ToArray();
        _seeds = Enumerable.Repeat(skip, Dimensions).ToArray();
        _permutation = scrambleRR2
            ? _RR2Permutation()
            : _IdentPermutation();
    }

    public HaltonSequence(int dimensions, int[] bases, int[] seeds, int[] leaps, bool scrambleRR2)
    {
        if (!PrimeNumbers.CheckRelativePrime(bases))
            throw new ArgumentException("The bases should be relatively prime.", nameof(bases));
        Dimensions = dimensions;
        _bases = bases;
        _leaps = leaps;
        _seeds = seeds;
        _permutation = scrambleRR2
            ? _RR2Permutation()
            : _IdentPermutation();
    }

    public int Dimensions { get; }

    public double[] Next()
    {
        var dimensions = Dimensions;
        var result = new double[dimensions];
        for (var i = 0; i < dimensions; i++)
        {
            var baseValue = (ulong)_bases[i];
            var thisIndex = _seeds[i] + _currentStep * _leaps[i];
            ulong denom = 1;
            ulong numer = 0;
            while (thisIndex != 0)
            {
                denom *= baseValue;
                var digit = thisIndex % _bases[i];
                var permutedDigit = _permutation[i][digit];
                numer *= baseValue;
                numer += (ulong)permutedDigit;
                thisIndex /= _bases[i];
            }

            result[i] = numer / (double)denom;
        }

        _currentStep++;

        return result;
    }

    private int[][] _RR2Permutation() => RR2Permutation.MakePermutation(_bases);

    private int[][] _IdentPermutation() => _bases.Select(x => Enumerable.Range(0, x).ToArray()).ToArray();

    private int[] _DefaultBases() => PrimeNumbers.FirstNStartingWith2((ushort)Dimensions);

    private int[] _DefaultSeeds() => Enumerable.Repeat(0, Dimensions).ToArray();

    private int[] _DefaultLeaps() => Enumerable.Repeat(1, Dimensions).ToArray();
}
