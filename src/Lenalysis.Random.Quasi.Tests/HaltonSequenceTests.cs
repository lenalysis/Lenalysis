using Shouldly;

namespace Lenalysis.Random.Quasi.Tests;

public class HaltonSequenceTests
{
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
}
