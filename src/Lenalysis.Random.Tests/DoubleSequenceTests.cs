using Lenalysis.Random.Tests.TestHelpers;
using Shouldly;
using Xunit;

namespace Lenalysis.Random.Tests
{
    public class DoubleSequenceTests
    {
        [Fact]
        public void TestZeroClosedOneOpen64()
        {
            const double epsilon = 1.0 / (1ul << 53);

            var intSeq = new FakeIntegerSequence64(new[]{ ulong.MinValue, ulong.MaxValue });
            var dblSeq = DoubleSequence.ZeroClosedOneOpen(intSeq);
            dblSeq.Next().ShouldBe(0.0);
            dblSeq.Next().ShouldBe(1.0 - epsilon);
        }

        [Fact]
        public void TestZeroClosedOneOpen32()
        {
            const double epsilon = 1.0 / (1ul << 32);
            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MaxValue });
            var dblSeq = DoubleSequence.ZeroClosedOneOpen(intSeq, false);
            dblSeq.Next().ShouldBe(0.0);
            dblSeq.Next().ShouldBe(1.0 - epsilon);
        }

        [Fact]
        public void TestZeroClosedOneOpen32HighRes()
        {
            const double epsilon = 1.0 / (1ul << 53);
            const uint max1 = ~((1u << 5) - 1); // make sure we don't "borrow bits" from elsewhere
            const uint max2 = ~((1u << 6) - 1);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MinValue, max1, max2 });
            var dblSeq = DoubleSequence.ZeroClosedOneOpen(intSeq, true);
            dblSeq.Next().ShouldBe(0.0);
            dblSeq.Next().ShouldBe(1.0 - epsilon);
        }

        [Fact]
        public void TestZeroOneClosed64()
        {
            var intSeq = new FakeIntegerSequence64(new[]{ ulong.MinValue, ulong.MaxValue });
            var dblSeq = DoubleSequence.ZeroOneClosed(intSeq);
            dblSeq.Next().ShouldBe(0.0);
            dblSeq.Next().ShouldBe(1.0);
        }

        [Fact]
        public void TestZeroOneClosed32()
        {
            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MaxValue });
            var dblSeq = DoubleSequence.ZeroOneClosed(intSeq, false);
            dblSeq.Next().ShouldBe(0.0);
            dblSeq.Next().ShouldBe(1.0);
        }

        [Fact]
        public void TestZeroOneClosed32HighRes()
        {
            const uint max1 = ~((1u << 5) - 1); // make sure we don't "borrow bits" from elsewhere
            const uint max2 = ~((1u << 6) - 1);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MinValue, max1, max2 });
            var dblSeq = DoubleSequence.ZeroOneClosed(intSeq, true);
            dblSeq.Next().ShouldBe(0.0);
            dblSeq.Next().ShouldBe(1.0);
        }

        [Fact]
        public void TestZeroOneOpen64()
        {
            const double epsilon = 1.0 / (1ul << 53);

            var intSeq = new FakeIntegerSequence64(new[]{ ulong.MinValue, ulong.MaxValue });
            var dblSeq = DoubleSequence.ZeroOneOpen(intSeq);
            dblSeq.Next().ShouldBe(0.0 + epsilon);
            dblSeq.Next().ShouldBe(1.0 - epsilon);
        }

        [Fact]
        public void TestZeroOneOpen32()
        {
            const double epsilon = 1.0 / (1ul << 33);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MaxValue });
            var dblSeq = DoubleSequence.ZeroOneOpen(intSeq, false);
            dblSeq.Next().ShouldBe(0.0 + epsilon);
            dblSeq.Next().ShouldBe(1.0 - epsilon);
        }

        [Fact]
        public void TestZeroOneOpen32HighRes()
        {
            const double epsilon = 1.0 / (1ul << 53);
            const uint max1 = ~((1u << 5) - 1); // make sure we don't "borrow bits" from elsewhere
            const uint max2 = ~((1u << 6) - 1);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MinValue, max1, max2 });
            var dblSeq = DoubleSequence.ZeroOneOpen(intSeq, true);
            dblSeq.Next().ShouldBe(0.0 + epsilon);
            dblSeq.Next().ShouldBe(1.0 - epsilon);
        }

        [Fact]
        public void TestCenteredClosed64()
        {
            var intSeq = new FakeIntegerSequence64(new[]{ ulong.MinValue, ulong.MaxValue });
            var dblSeq = DoubleSequence.CenteredClosed(intSeq);
            dblSeq.Next().ShouldBe(-0.5);
            dblSeq.Next().ShouldBe(0.5);
        }

        [Fact]
        public void TestCenteredClosed32()
        {
            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MaxValue });
            var dblSeq = DoubleSequence.CenteredClosed(intSeq, false);
            dblSeq.Next().ShouldBe(-0.5);
            dblSeq.Next().ShouldBe(+0.5);
        }

        [Fact]
        public void TestCenteredClosed32HighRes()
        {
            const uint max1 = ~((1u << 5) - 1); // make sure we don't "borrow bits" from elsewhere
            const uint max2 = ~((1u << 6) - 1);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MinValue, max1, max2 });
            var dblSeq = DoubleSequence.CenteredClosed(intSeq, true);
            dblSeq.Next().ShouldBe(-0.5);
            dblSeq.Next().ShouldBe(+0.5);
        }

        [Fact]
        public void TestRangeClosed64()
        {
            var intSeq = new FakeIntegerSequence64(new[]{ ulong.MinValue, ulong.MaxValue });
            var dblSeq = DoubleSequence.RangeClosed(intSeq, 3.5, 7.0);
            dblSeq.Next().ShouldBe(3.5);
            dblSeq.Next().ShouldBe(7.0);
        }

        [Fact]
        public void TestRangeClosed32()
        {
            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MaxValue });
            var dblSeq = DoubleSequence.RangeClosed(intSeq, 3.5, 7.0, false);
            dblSeq.Next().ShouldBe(3.5);
            dblSeq.Next().ShouldBe(7.0);
        }

        [Fact]
        public void TestRangeClosed32HighRes()
        {
            const uint max1 = ~((1u << 5) - 1); // make sure we don't "borrow bits" from elsewhere
            const uint max2 = ~((1u << 6) - 1);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MinValue, max1, max2 });
            var dblSeq = DoubleSequence.RangeClosed(intSeq, 3.5, 7.0, true);
            dblSeq.Next().ShouldBe(3.5);
            dblSeq.Next().ShouldBe(7.0);
        }

        [Fact]
        public void TestRangeClosedOpen64()
        {
            const double epsilon = 3.5 / (1ul << 53);

            var intSeq = new FakeIntegerSequence64(new[]{ ulong.MinValue, ulong.MaxValue });
            var dblSeq = DoubleSequence.RangeClosedOpen(intSeq, 3.5, 7.0);
            dblSeq.Next().ShouldBe(3.5);
            dblSeq.Next().ShouldBe(7.0 - epsilon);
        }

        [Fact]
        public void TestRangeClosedOpen32()
        {
            const double epsilon = 3.5 / (1ul << 32);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MaxValue });
            var dblSeq = DoubleSequence.RangeClosedOpen(intSeq, 3.5, 7.0, false);
            dblSeq.Next().ShouldBe(3.5);
            dblSeq.Next().ShouldBe(7.0 - epsilon);
        }

        [Fact]
        public void TestRangeClosedOpen32HighRes()
        {
            const uint max1 = ~((1u << 5) - 1); // make sure we don't "borrow bits" from elsewhere
            const uint max2 = ~((1u << 6) - 1);

            const double epsilon = 3.5 / (1ul << 53);

            var intSeq = new FakeIntegerSequence32(new[]{ uint.MinValue, uint.MinValue, max1, max2 });
            var dblSeq = DoubleSequence.RangeClosedOpen(intSeq, 3.5, 7.0, true);
            dblSeq.Next().ShouldBe(3.5);
            dblSeq.Next().ShouldBe(7.0 - epsilon);
        }
    }
}