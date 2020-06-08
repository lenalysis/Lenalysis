using Shouldly;
using Xunit;

namespace Lenalysis.Random.Tests
{
    public class MersenneTwister32Tests
    {
        [Fact]
        public void TestIntegralValues()
        {
            var mt = new MersenneTwister32(5489);
            uint x = 0;
            for (var i = 0; i < 10000; i++)
                x = mt.Next();
            x.ShouldBe(4123659995); // according to https://en.cppreference.com/w/cpp/numeric/random/mersenne_twister_engine
        }

        [Fact]
        public void TestState()
        {
            var mt = new MersenneTwister32(5489);
            mt.Next();
            var savedState = mt.ExportState();
            var before = mt.Next();
            var after = new MersenneTwister32(savedState).Next();
            after.ShouldBe(before);
        }
    }
}