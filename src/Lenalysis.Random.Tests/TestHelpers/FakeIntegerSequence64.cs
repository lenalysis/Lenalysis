namespace Lenalysis.Random.Tests.TestHelpers
{
    /// <summary>
    /// Used for testing classes that use IIntegerSequence64 as their input.
    /// </summary>
    public class FakeIntegerSequence64 : IIntegerSequence64
    {
        private readonly ulong[] _values;
        private int _position;

        public FakeIntegerSequence64(ulong[] values)
        {
            _values = values;
            _position = 0;
        }

        public ulong Next()
        {
            return _values[_position++];
        }
    }
}