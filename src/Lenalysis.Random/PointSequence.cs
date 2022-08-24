namespace Lenalysis.Random;

public static class PointSequence
{
    public static IPointSequence FromDoubleSequence(IDoubleSequence sequence, int numDimensions)
    {
        return new PointSequenceFromDouble(sequence, numDimensions);
    }
}
