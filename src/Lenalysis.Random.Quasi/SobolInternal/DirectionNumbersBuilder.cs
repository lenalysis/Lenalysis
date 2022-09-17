using System.Diagnostics;
using System.IO;

namespace Lenalysis.Random.Quasi.SobolInternal;

public class DirectionNumbersBuilder : IDirectionNumbersBuilder
{
    private readonly int _maxDimension;
    private readonly DimensionRow[] _dimensionsFromData;

    private DirectionNumbersBuilder(Stream dimensionsSource, int maxDimension)
    {
        _maxDimension = maxDimension;

        var sr = new DimensionStreamReader(dimensionsSource);
        _dimensionsFromData = sr.ReadTo(maxDimension).ToArray();
    }

    public static IDirectionNumbersBuilder FromStream(Stream dimensionsSource, int maxDimension)
    {
        return new DirectionNumbersBuilder(dimensionsSource, maxDimension);
    }

    public uint[] ForDimensionNumber(int dimensionOneBased)
    {
        if (dimensionOneBased < 1)
            throw new ArgumentOutOfRangeException(nameof(dimensionOneBased), dimensionOneBased,
                "Value should be greater than or equal to 1.");
        if (dimensionOneBased > _maxDimension)
            throw new ArgumentOutOfRangeException(nameof(dimensionOneBased), dimensionOneBased,
                "Value should be less than or equal to the value of maxDimension passed when constructing this object.");

        if (dimensionOneBased == 1)
            return BuildDimension1();

        // all other dimensions use values from the input file
        ref var row = ref _dimensionsFromData[dimensionOneBased - 2];
        var v2 = BuildFromRow(dimensionOneBased, ref row);

        return v2;
    }

    public static uint[] BuildFromRow(int dimensionOneBased, ref DimensionRow row)
    {
        var (d, s, a, m2) = row;
        Debug.Assert(d == dimensionOneBased, $"d == dimensionOneBased ({d} == {dimensionOneBased})");

        var v2 = new uint[32];
        for (var i = 0; i < s; i++)
            v2[i] = m2[i] << (31 - i);
        for (var i = s; i < 32; i++)
        {
            v2[i] = v2[i - s] ^ (v2[i - s] >> s);
            for (var k = 0; k <= s - 2; k++)
                v2[i] ^= ((a >> s - 2 - k) & 1) * v2[i - k - 1];
        }

        return v2;
    }

    public static uint[] BuildDimension1()
    {
        // this dimension is special... no idea why but I couldn't figure out how to use the other branch for this (too lazy).
        var v1 = new uint[32];
        for (var i = 0; i < 32; i++)
            v1[i] = 1u << (31 - i);

        return v1;
    }
}