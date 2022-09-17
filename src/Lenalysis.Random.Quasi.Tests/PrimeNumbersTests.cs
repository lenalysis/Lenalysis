using Lenalysis.Random.Quasi.SharedInternal;

namespace Lenalysis.Random.Quasi.Tests;

public class PrimeNumbersTests
{
    [Fact]
    public void SimpleTest()
    {
        PrimeNumbers.FirstNStartingWith2(5).ShouldBe(new[]{ 2, 3, 5, 7, 11 });
    }

    [Fact]
    public void ThrowsOnInputTooBig()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => PrimeNumbers.FirstNStartingWith2(1001))
            .ActualValue.ShouldBe(1001);
    }

    [Theory]
    [InlineData(new[]{ 2, 4 }, false)]
    [InlineData(new[]{ 2 }, true)]
    [InlineData(new[]{ 2, 5 }, true)]
    [InlineData(new[]{ 2, 5, 7 }, true)]
    [InlineData(new[]{ 10, 7, 5 }, false)]
    [InlineData(new[]{ 10, 7 }, true)] // ok for non-prime, just mutually prime!
    public void RelativePrime(int[] values, bool expectedResult)
    {
        PrimeNumbers.CheckRelativePrime(values).ShouldBe(expectedResult);
    }
}
