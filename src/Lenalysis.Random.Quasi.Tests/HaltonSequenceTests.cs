using Lenalysis.Random.Quasi.SobolInternal;
using Xunit.Abstractions;

namespace Lenalysis.Random.Quasi.Tests;

public class HaltonSequenceTests
{
    private readonly ITestOutputHelper _helper;

    public HaltonSequenceTests(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    [Fact]
    public void SimpleTest()
    {
        var haltonSequence = new HaltonSequence(2);
        var elem0 = haltonSequence.Next();
        elem0.ShouldBe(new[] { 0.0, 0.0 });
        var elem1 = haltonSequence.Next();
        elem1.ShouldBe(new[] { 1.0 / 2.0, 1.0 / 3.0 });
        var elem2 = haltonSequence.Next();
        elem2.ShouldBe(new[] { 1.0 / 4.0, 2.0 / 3.0 });
        var elem3 = haltonSequence.Next();
        elem3.ShouldBe(new[] { 3.0 / 4.0, 1.0 / 9.0 });
    }

    [Fact]
    public void MathworksTest()
    {
        var haltonSequence = new HaltonSequence(3, new[]{ 5, 3, 7}, new[]{ 0, 1, 2 }, new[]{ 1, 2, 3 }, false);
        for (var i = 0; i < 100; i++)
        {
            var elem = haltonSequence.Next();
            _helper.WriteLine($"{elem[0],10:F6}{elem[1],10:F6}{elem[2],10:F6}");
        }
    }

    [Theory(Skip = "Skip unless building new binary data input")]
    [InlineData("/home/kleahy/Downloads/new-joe-kuo-6.21201", "/home/kleahy/Downloads/new-joe-kuo-6.21201.bin")]
    public void BuildDirectionVectorFile(string inputFile, string binaryFile)
    {
        JoeAndKuoDirectionNumbersFile.ConvertToBinary(binaryFile, inputFile);

        // for verification, rebuild the source file from the data, so we can externally compare.
        using var dimStreamReader = new DimensionStreamReader(File.OpenRead(binaryFile));
        using var text = File.CreateText(binaryFile + ".txt");
        foreach (var (d, s, a, m) in dimStreamReader.ReadTo(21201))
            text.WriteLine($"{d,-7} {s,-7} {a,-7} {string.Join(" ", m)} ");
    }

    [Fact]
    public void SobolTest3d()
    {
        // another verification...  print the first 10 sobol points in 3D.
        var sequence = SobolSequence.CreateDefault(3);
        var expected = new[]
        {
            new[] { 0.0, 0.0, 0.0 },
            new[] { 0.5, 0.5, 0.5 },
            new[] { 0.75, 0.25, 0.25 },
            new[] { 0.25, 0.75, 0.75 },
            new[] { 0.375, 0.375, 0.625 },
            new[] { 0.875, 0.875, 0.125 },
            new[] { 0.625, 0.125, 0.875 },
            new[] { 0.125, 0.625, 0.375 },
            new[] { 0.1875, 0.3125, 0.9375 },
            new[] { 0.6875, 0.8125, 0.4375 }
        };
        var actual = new double[10][];
        for (var i = 0; i < 10; i++)
        {
            actual[i] = sequence.Next();
        }

        actual.ShouldBe(expected);
    }

    [Fact]
    public void SobolTest1()
    {
        var v1 = new uint[32];
        for (var i = 0; i < 32; i++)
            v1[i] = 1u << (31 - i);

        const double pow32 = 0x100000000;
        var seq = 0;
        uint x = 0;
        double p = x;
        _helper.WriteLine($"{seq:D3} {p:F6}");
        var c = 0;
        while (seq++ < 100)
        {
            x ^= v1[c];
            p = x / pow32;
            _helper.WriteLine($"{seq:D3} {p:F6}");
            c = 0;
            var value = seq;
            while (value % 2 == 1)
            {
                value >>= 1;
                c++;
            }
        }
    }

    [Fact]
    public void SobolTest2()
    {
        // info from input file
        const uint s = 2;
        var m2 = new uint[] { 0, 1, 3 };
        const uint a = 1;

        // build direction vector for this dimension
        var v2 = new uint[32];
        for (var i = 1; i <= s; i++)
            v2[i] = m2[i] << (32 - i);
        for (var i = s + 1; i < 32; i++)
        {
            v2[i] = v2[i - s] ^ (v2[i - s] >> (int)s);
            for (var k = 1; k <= s - 1; k++)
                v2[i] ^= ((a >> (int)(s - 1 - k)) & 1) * v2[i - k];
        }

        const double pow32 = 0x100000000;
        var seq = 0;
        uint x = 0;
        var p0 = x;
        _helper.WriteLine($"{seq:D3} {p0:F6}");
        var c = 1;
        while (seq++ < 100)
        {
            //_helper.WriteLine($"{seq}: x:{x} c:{c} v2[c]:{v2[c]}");
            x ^= v2[c];
            var p = x / pow32;
            _helper.WriteLine($"{seq:D3} {p:F6}");
            c = 1;
            var value = seq;
            while (value % 2 == 1)
            {
                value >>= 1;
                c++;
            }
        }
    }

    [Fact]
    public void SobolTest3()
    {
        // info from input file
        const uint s = 2;
        var m2 = new uint[] { 1, 3 };
        const uint a = 1;

        // build direction vector for this dimension
        var v2 = new uint[32];
        for (var i = 0; i < s; i++)
            v2[i] = m2[i] << (31 - i);
        for (var i = s; i < 32; i++)
        {
            v2[i] = v2[i - s] ^ (v2[i - s] >> (int)s);
//            _helper.WriteLine("b: " + string.Join(":", v2));
            for (var k = 0; k <= s - 2; k++)
                v2[i] ^= ((a >> (int)(s - 2 - k)) & 1) * v2[i - k - 1];
//            _helper.WriteLine("a: " + string.Join(":", v2));
        }

        _helper.WriteLine(string.Join(", ", v2));

        const double pow32 = 0x100000000;
        var seq = 0;
        uint x = 0;
        var p0 = x;
        _helper.WriteLine($"{seq:D3} {p0:F6}");
        var c = 0;
        while (seq++ < 100)
        {
            x ^= v2[c];
            var p = x / pow32;
            _helper.WriteLine($"{seq:D3} {p:F6}");
            c = 0;
            var value = seq;
            while (value % 2 == 1)
            {
                value >>= 1;
                c++;
            }
        }
    }
}
