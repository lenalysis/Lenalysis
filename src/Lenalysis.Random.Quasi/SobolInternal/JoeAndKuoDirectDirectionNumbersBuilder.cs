using System.IO;

namespace Lenalysis.Random.Quasi.SobolInternal;

public class JoeAndKuoDirectDirectionNumbersBuilder : IDirectionNumbersBuilder
{
    private readonly int _maxDimensions;
    private readonly DimensionRow[] _rows;

    public JoeAndKuoDirectDirectionNumbersBuilder(string textFileName, int maxDimensions)
    {
        _maxDimensions = maxDimensions;
        _rows = JoeAndKuoDirectionNumbersFile.ReadLines(File.ReadLines(textFileName))
            .Take(maxDimensions - 1)
            .ToArray();
    }

    public uint[] ForDimensionNumber(int dimensionOneBased)
    {
        if (dimensionOneBased < 1)
            throw new ArgumentOutOfRangeException(nameof(dimensionOneBased), "dimensionOneBased must be >= 1");
        if (dimensionOneBased > _maxDimensions)
            throw new ArgumentOutOfRangeException(nameof(dimensionOneBased),
                "dimensionOneBased must be less than max dimensions passed when constructing this object.");

        if (dimensionOneBased == 1)
            return DirectionNumbersBuilder.BuildDimension1();

        ref var row = ref _rows[dimensionOneBased - 2];
        return DirectionNumbersBuilder.BuildFromRow(dimensionOneBased, ref row);
    }
}