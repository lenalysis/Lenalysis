using Shouldly;
using Xunit;

namespace Lenalysis.Random.Tests
{
    public class MersenneTwister64Tests
    {
        [Fact]
        public void TestIntegralValues()
        {
            var mt = new MersenneTwister64(5489);
            ulong x = 0;
            for (var i = 0; i < 10000; i++)
                x = mt.Next();
            x.ShouldBe(9981545732273789042); // according to https://en.cppreference.com/w/cpp/numeric/random/mersenne_twister_engine
        }

        [Fact]
        public void TestState()
        {
            var mt = new MersenneTwister64(5489);
            mt.Next();
            var savedState = mt.ExportState();
            var before = mt.Next();
            var after = new MersenneTwister64(savedState).Next();
            after.ShouldBe(before);
        }
    }
}