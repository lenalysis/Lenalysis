using System.Reflection;
using Lenalysis.Random.Quasi.SobolInternal;

namespace Lenalysis.Random.Quasi;

public class SobolSequence : IPointSequence
{
    private readonly SobolBitFlipGenerator _bitFlipGenerator = new();
    private readonly SobolDimensionGenerator[] _generators;

    private SobolSequence(IDirectionNumbersBuilder directionNumbers, int dimensions)
    {
        Dimensions = dimensions;
        _generators = new SobolDimensionGenerator[dimensions];
        for (var i = 1; i <= dimensions; i++)
            _generators[i - 1] = new SobolDimensionGenerator(i, directionNumbers.ForDimensionNumber(i));
    }

    public static SobolSequence CreateDefault(int dimensions)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("Lenalysis.Random.Quasi.SobolInternal.new-joe-kuo-6.21201.bin");
        return new SobolSequence(DirectionNumbersBuilder.FromStream(stream, dimensions), dimensions);
    }

    public int Dimensions { get; }

    public double[] Next()
    {
        var currentBitToFlip = _bitFlipGenerator.Current;
        var result = new double[_generators.Length];
        for (var i = 0; i < result.Length; i++)
            result[i] = _generators[i].Next(currentBitToFlip);
        _bitFlipGenerator.Next();
        return result;
    }
}
