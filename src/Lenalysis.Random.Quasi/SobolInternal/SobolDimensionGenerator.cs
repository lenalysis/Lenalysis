namespace Lenalysis.Random.Quasi.SobolInternal;

public class SobolDimensionGenerator
{
    private readonly uint[] _directionVector;
    private uint _numerator;

    public SobolDimensionGenerator(int dimension, uint[] directionVector)
    {
        Dimension = dimension;
        _directionVector = directionVector;
        _numerator = 0;
    }

    public double Next(int bitToFlip)
    {
        const double pow32 = 0x100000000;
        var res = _numerator / pow32;
        _numerator ^= _directionVector[bitToFlip];
        return res;
    }

    public int Dimension { get; }
}