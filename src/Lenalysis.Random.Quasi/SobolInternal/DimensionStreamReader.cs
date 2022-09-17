using System.IO;
using System.Text;

namespace Lenalysis.Random.Quasi.SobolInternal;

public class DimensionStreamReader: IDisposable
{
    private readonly Stream _source;

    public DimensionStreamReader(Stream source)
    {
        _source = source;
    }

    public void Dispose()
    {
        _source.Dispose();
    }

    private class SubBinaryReader : BinaryReader
    {
        public SubBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public new int Read7BitEncodedInt()
        {
            return base.Read7BitEncodedInt();
        }
    }

    public IEnumerable<DimensionRow> ReadTo(int maxDimension)
    {
        // reset source stream
        _source.Position = 0;
        using var br = new SubBinaryReader(_source, Encoding.Default, true);

        // file starts with dimension 2 all the time
        var d = 2;

        // get the max dimension
        var lastD = br.Read7BitEncodedInt();
        if (lastD < maxDimension)
            throw new InvalidOperationException(
                $"The data file only has dimensions up to {lastD} but you requested dimensions up to {maxDimension}");

        // first, read the table of s changes
        var sChanges = new List<int>();
        var sChangeCount = br.Read7BitEncodedInt();
        for(var i=0; i<sChangeCount; i++)
            sChanges.Add(br.Read7BitEncodedInt());

        // now, read the rows...
        var s = 0;
        var sChangeIndex = 0;
        var sChange = sChanges.First();
        while (d <= maxDimension)
        {
            // is this the next place s goes up by 1?
            if (sChange == d)
            {
                s++;
                sChangeIndex++;
                if (sChangeIndex < sChanges.Count)
                    sChange = sChanges[sChangeIndex];
            }

            // read the data (a and m) (d and s are implied and not stored)
            var a = (uint)br.Read7BitEncodedInt();
            var m = new uint[s];
            for (var i = 0; i < m.Length; i++)
                m[i] = (uint)br.Read7BitEncodedInt();

            yield return new DimensionRow(d, s, a, m);
            d++;
        }
    }
}
