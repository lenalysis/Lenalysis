namespace Lenalysis.Random;

public class PointSequenceFromDouble: IPointSequence
{
    private readonly IDoubleSequence _sequence;

    public PointSequenceFromDouble(IDoubleSequence sequence, int numDimensions)
    {
        _sequence = sequence;
        Dimensions = numDimensions;
    }

    public int Dimensions { get; }

    public double[] Next()
    {
        var dimensions = Dimensions;
        var result = new double[dimensions];
        for (var i = 0; i < dimensions; i++)
            result[i] = _sequence.Next();
        return result;
    }
}
