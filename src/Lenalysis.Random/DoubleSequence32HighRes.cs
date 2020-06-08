namespace Lenalysis.Random
{
    /// <summary>
    /// Implements a high-resolution parameterized double sequence transform for 32-bit integer sequences.  This is
    /// used to transform pairs of integers from integer sequences in the range [0,2^32-1] into double values in a
    /// particular range (using 53-bit precision).  <see cref="DoubleSequence"/>
    ///
    /// Generally, users shouldn't construct their own <see cref="DoubleSequence32HighRes"/>, but rather use one of
    /// the factory methods from <see cref="DoubleSequence"/>
    /// </summary>
    public class DoubleSequence32HighRes : IDoubleSequence
    {
        private readonly int _shift;
        private readonly double _delta;
        private readonly double _scale;
        private readonly IIntegerSequence32 _underlying;

        public DoubleSequence32HighRes(in int shift, in double delta, in double scale, IIntegerSequence32 underlying)
        {
            _shift = shift;
            _delta = delta;
            _scale = scale;
            _underlying = underlying;
        }

        public double Next()
        {
            var r1 = _underlying.Next() >> 5; // top 27 bits
            var r2 = _underlying.Next() >> 6; // top 26 bits
            var r = ((ulong)r1 << 26) | r2;
            return ((r >> _shift) + _delta) * _scale;
        }
    }
}