namespace Lenalysis.Random.Quasi;

public class HaltonSequence: IPointSequence
{
    private readonly int[] _bases;
    private readonly int[] _leaps;
    private readonly int[] _seeds;
    private int _currentStep;

    public HaltonSequence(int dimensions)
    {
        Dimensions = dimensions;
        _bases = PrimeNumbers.FirstNStartingWith2(Dimensions);
        _leaps = Enumerable.Repeat(1, Dimensions).ToArray();
        _seeds = Enumerable.Repeat(0, Dimensions).ToArray();
        _currentStep = 0;
    }

    public HaltonSequence(int dimensions, int[] bases)
    {
        // todo: check bases are mutually prime
        Dimensions = dimensions;
        _bases = bases;
        _leaps = Enumerable.Repeat(1, Dimensions).ToArray();
        _seeds = Enumerable.Repeat(0, Dimensions).ToArray();
    }

    public int Dimensions { get; }

    public double[] Next()
    {
        var dimensions = Dimensions;
        var result = new double[dimensions];
        for (var i = 0; i < dimensions; i++)
        {
            var thisIndex = _seeds[i] + _currentStep * _leaps[i];
            var baseInv = 1.0 / _bases[i];
            while (thisIndex != 0)
            {
                var digit = thisIndex % _bases[i];
                result[i] += digit * baseInv;
                baseInv /= _bases[i];
                thisIndex /= _bases[i];
            }
        }

        _currentStep++;

        return result;
    }
}
