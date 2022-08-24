namespace Lenalysis.Random;

public interface IPointSequence
{
    int Dimensions { get; }
    double[] Next();
}
