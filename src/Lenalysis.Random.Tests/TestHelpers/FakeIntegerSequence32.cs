namespace Lenalysis.Random.Tests.TestHelpers
{
    /// <summary>
    /// Used for testing classes that use IIntegerSequence32 as their input.
    /// </summary>
    public class FakeIntegerSequence32 : IIntegerSequence32
    {
        private readonly uint[] _values;
        private int _position;

        public FakeIntegerSequence32(uint[] values)
        {
            _values = values;
            _position = 0;
        }

        public uint Next()
        {
            return _values[_position++];
        }
    }
}