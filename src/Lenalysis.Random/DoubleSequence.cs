namespace Lenalysis.Random;

/// <summary>
/// Factory facade for creating double sequences with various ranges and domains from integer sequences.
/// </summary>
public static class DoubleSequence
{
    /// <summary>
    /// Create a double sequence that provides values from the interval [0,1).
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <returns>A double sequence with values in [0,1)</returns>
    public static IDoubleSequence ZeroClosedOneOpen(IIntegerSequence64 underlying)
    {
        const int shift = 11;
        const double delta = 0.0;
        const double p = 1ul << 53;
        const double scale = 1.0 / p;
        return new DoubleSequence64(shift, delta, scale, underlying);
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [0,1).
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="use53Bit">Determines whether to use 53-bit resolution or 32-bit resolution</param>
    /// <returns>A double sequence with values in [0,1)</returns>
    public static IDoubleSequence ZeroClosedOneOpen(IIntegerSequence32 underlying, bool use53Bit = false)
    {
        if (use53Bit)
        {
            const int shift = 0;
            const double delta = 0.0;
            const double p = 1ul << 53;
            const double scale = 1.0 / p;
            return new DoubleSequence32HighRes(shift, delta, scale, underlying);
        }
        else
        {
            const int shift = 0;
            const double delta = 0.0;
            const double p = 1ul << 32;
            const double scale = 1.0 / p;
            return new DoubleSequence32(shift, delta, scale, underlying);
        }
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [0,1].
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <returns>A double sequence with values in [0,1]</returns>
    public static IDoubleSequence ZeroOneClosed(IIntegerSequence64 underlying)
    {
        const int shift = 11;
        const double delta = 0.0;
        const double p = (1ul << 53) - 1;
        const double scale = 1.0 / p;
        return new DoubleSequence64(shift, delta, scale, underlying);
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [0,1].
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="use53Bit">Determines whether to use 53-bit resolution or 32-bit resolution</param>
    /// <returns>A double sequence with values in [0,1]</returns>
    public static IDoubleSequence ZeroOneClosed(IIntegerSequence32 underlying, bool use53Bit = false)
    {
        if (use53Bit)
        {
            const int shift = 0;
            const double delta = 0.0;
            const double p = (1ul << 53) - 1;
            const double scale = 1.0 / p;
            return new DoubleSequence32HighRes(shift, delta, scale, underlying);
        }
        else
        {
            const int shift = 0;
            const double delta = 0.0;
            const double p = (1ul << 32) - 1;
            const double scale = 1.0 / p;
            return new DoubleSequence32(shift, delta, scale, underlying);
        }
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval (0,1).
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <returns>A double sequence with values in (0,1)</returns>
    public static IDoubleSequence ZeroOneOpen(IIntegerSequence64 underlying)
    {
        const int shift = 12;
        const double delta = 0.5;
        const double p = 1ul << 52;
        const double scale = 1.0 / p;
        return new DoubleSequence64(shift, delta, scale, underlying);
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval (0,1).
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="use53Bit">Determines whether to use 53-bit resolution or 32-bit resolution</param>
    /// <returns>A double sequence with values in (0,1)</returns>
    public static IDoubleSequence ZeroOneOpen(IIntegerSequence32 underlying, bool use53Bit = false)
    {
        if (use53Bit)
        {
            const int shift = 1;
            const double delta = 0.5;
            const double p = 1ul << 52;
            const double scale = 1.0 / p;
            return new DoubleSequence32HighRes(shift, delta, scale, underlying);
        }
        else
        {
            const int shift = 0;
            const double delta = 0.5;
            const double p = 1ul << 32;
            const double scale = 1.0 / p;
            return new DoubleSequence32(shift, delta, scale, underlying);
        }
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [-0.5,0.5].
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <returns>A double sequence with values in [-0.5,0.5]</returns>
    public static IDoubleSequence CenteredClosed(IIntegerSequence64 underlying)
    {
        const int shift = 11;
        const double p = (1ul << 53) - 1;
        const double delta = -p / 2;
        const double scale = 1.0 / p;
        return new DoubleSequence64(shift, delta, scale, underlying);
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [-0.5,0.5].
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="use53Bit">Determines whether to use 53-bit resolution or 32-bit resolution</param>
    /// <returns>A double sequence with values in [-0.5,0.5]</returns>
    public static IDoubleSequence CenteredClosed(IIntegerSequence32 underlying, bool use53Bit = false)
    {
        if (use53Bit)
        {
            const int shift = 0;
            const double p = (1ul << 53) - 1;
            const double delta = -p / 2;
            const double scale = 1.0 / p;
            return new DoubleSequence32HighRes(shift, delta, scale, underlying);
        }
        else
        {
            const int shift = 0;
            const double p = (1ul << 32) - 1;
            const double delta = -p / 2;
            const double scale = 1.0 / p;
            return new DoubleSequence32(shift, delta, scale, underlying);
        }
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [a,b].
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="a">The lower bound (inclusive)</param>
    /// <param name="b">The upper bound (inclusive)</param>
    /// <returns>A double sequence with values in [a,b]</returns>
    public static IDoubleSequence RangeClosed(IIntegerSequence64 underlying, double a, double b)
    {
        const int shift = 11;
        const double p = (1ul << 53) - 1;
        var pAdj = p / (b - a);
        var deltaAdj = pAdj * a;
        var scaleAdj = 1.0 / pAdj;
        return new DoubleSequence64(shift, deltaAdj, scaleAdj, underlying);
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [a,b].
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="a">The lower bound (inclusive)</param>
    /// <param name="b">The upper bound (inclusive)</param>
    /// <param name="use53Bit">Determines whether to use 53-bit resolution or 32-bit resolution</param>
    /// <returns>A double sequence with values in [a,b]</returns>
    public static IDoubleSequence RangeClosed(IIntegerSequence32 underlying, double a, double b, bool use53Bit = false)
    {
        if (use53Bit)
        {
            const int shift = 0;
            const double p = (1ul << 53) - 1;
            var pAdj = p / (b - a);
            var deltaAdj = pAdj * a;
            var scaleAdj = 1.0 / pAdj;
            return new DoubleSequence32HighRes(shift, deltaAdj, scaleAdj, underlying);
        }
        else
        {
            const int shift = 0;
            const double p = (1ul << 32) - 1;
            var pAdj = p / (b - a);
            var deltaAdj = pAdj * a;
            var scaleAdj = 1.0 / pAdj;
            return new DoubleSequence32(shift, deltaAdj, scaleAdj, underlying);
        }
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [a,b).
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="a">The lower bound (inclusive)</param>
    /// <param name="b">The upper bound (exclusive)</param>
    /// <returns>A double sequence with values in [a,b)</returns>
    public static IDoubleSequence RangeClosedOpen(IIntegerSequence64 underlying, double a, double b)
    {
        const int shift = 11;
        const double p = 1ul << 53;
        var pAdj = p / (b - a);
        var deltaAdj = pAdj * a;
        var scaleAdj = 1.0 / pAdj;
        return new DoubleSequence64(shift, deltaAdj, scaleAdj, underlying);
    }

    /// <summary>
    /// Create a double sequence that provides values from the interval [a,b).
    /// </summary>
    /// <param name="underlying">An underlying integer sequence generator.</param>
    /// <param name="a">The lower bound (inclusive)</param>
    /// <param name="b">The upper bound (inclusive)</param>
    /// <param name="use53Bit">Determines whether to use 53-bit resolution or 32-bit resolution</param>
    /// <returns>A double sequence with values in [a,b)</returns>
    public static IDoubleSequence RangeClosedOpen(IIntegerSequence32 underlying, double a, double b, bool use53Bit = false)
    {
        if (use53Bit)
        {
            const int shift = 0;
            const double p = 1ul << 53;
            var pAdj = p / (b - a);
            var deltaAdj = pAdj * a;
            var scaleAdj = 1.0 / pAdj;
            return new DoubleSequence32HighRes(shift, deltaAdj, scaleAdj, underlying);
        }
        else
        {
            const int shift = 0;
            const double p = 1ul << 32;
            var pAdj = p / (b - a);
            var deltaAdj = pAdj * a;
            var scaleAdj = 1.0 / pAdj;
            return new DoubleSequence32(shift, deltaAdj, scaleAdj, underlying);
        }
    }
}
