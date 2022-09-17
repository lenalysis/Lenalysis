using System.IO;
using System.Text;

namespace Lenalysis.Random.Quasi.SobolInternal;

public static class JoeAndKuoDirectionNumbersFile
{
    public static IEnumerable<DimensionRow> ReadLines(IEnumerable<string> inputLines)
    {
        foreach (var line in inputLines)
        {
            if (line.StartsWith("d"))
                continue;
            if (string.IsNullOrWhiteSpace(line))
                break;

            var parts = line.Split(new[]{" "}, 4, StringSplitOptions.RemoveEmptyEntries);

            var d = int.Parse(parts[0]);
            var s = int.Parse(parts[1]);
            var a = uint.Parse(parts[2]);
            var ms = parts[3].Split(new[]{" "}, StringSplitOptions.RemoveEmptyEntries);
            var m = ms.Select(uint.Parse).ToArray();

            yield return new DimensionRow(d, s, a, m);
        }
    }

    private class SubBinaryWriter : BinaryWriter
    {
        public SubBinaryWriter(Stream target) :
            base(target, Encoding.Default, leaveOpen: true)
        {
        }

        public new void Write7BitEncodedInt(int value)
        {
            base.Write7BitEncodedInt(value);
        }
    }

    public static void ConvertToBinary(Stream file, IEnumerable<string> inputLines)
    {
        var lastD = 1;
        var lastS = 0;
        var sChanges = new List<int>();
        using var mem = new MemoryStream();
        using var bwMem = new SubBinaryWriter(mem);
        using var bwFile = new SubBinaryWriter(file);
        foreach (var (d, s, a, m) in ReadLines(inputLines))
        {

            if (d != lastD + 1)
                throw new Exception(
                    "Invalid file - first dimension should be 2 and each dimension after should be sequential");

            if (m.Length != s)
                throw new Exception(
                    $"Invalid file - length of 'm' for dimension {d} is not equal to value of 's'");

            bwMem.Write7BitEncodedInt((int)a);
            foreach (var i in m)
                bwMem.Write7BitEncodedInt((int)i);

            if (s != lastS)
            {
                sChanges.Add(d);
                lastS = s;
            }

            lastD = d;
        }

        bwFile.Write7BitEncodedInt(lastD);
        bwFile.Write7BitEncodedInt(sChanges.Count);
        foreach (var ch in sChanges)
            bwFile.Write7BitEncodedInt(ch);
        bwFile.Flush();

        bwMem.Flush();
        mem.Position = 0;
        mem.CopyTo(file);

        file.Flush();
    }

    public static void ConvertToBinary(string targetFile, string sourceFile)
    {
        using var file = File.Create(targetFile);
        var inputLines = File.ReadLines(sourceFile);
        ConvertToBinary(file, inputLines);
    }
}
