namespace Lenalysis.Random.Quasi.SobolInternal;

public class SobolBitFlipGenerator
{
    private uint _seq;
    private int _bit;

    public void Next()
    {
        _seq++;
        _bit = _CalculateBit();
    }

    private int _CalculateBit()
    {
        // find the rightmost zero in the number.
        var bit = 0;
        for (var v = _seq; v % 2 == 1; v >>= 1)
            bit++;
        return bit;
    }

    public int Current => _bit;

    public uint Sequence => _seq;
}