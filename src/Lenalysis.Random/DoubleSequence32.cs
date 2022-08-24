namespace Lenalysis.Random;

/// <summary>
/// Implements a parameterized double sequence transform for 32-bit integer sequences.  This is used to transform
/// integer sequences in the range [0,2^32-1] into double values in a particular range.  <see cref="DoubleSequence"/>
///
/// Generally, users shouldn't construct their own <see cref="DoubleSequence32"/>, but rather use one of
/// the factory methods from <see cref="DoubleSequence"/>
/// </summary>
public class DoubleSequence32 : IDoubleSequence
{
    private readonly int _shift;
    private readonly double _delta;
    private readonly double _scale;
    private readonly IIntegerSequence32 _underlying;

    public DoubleSequence32(in int shift, in double delta, in double scale, IIntegerSequence32 underlying)
    {
        _shift = shift;
        _delta = delta;
        _scale = scale;
        _underlying = underlying;
    }

    public double Next()
    {
        return ((_underlying.Next() >> _shift) + _delta) * _scale;
    }
}