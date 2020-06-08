# Lenalysis.Random

This library provides functionality for generating and transforming
random numbers into various forms for the use of simulations.

It provides the following random number generators:

* [Mersenne Twister (MT19937)](https://en.wikipedia.org/wiki/Mersenne_Twister)
* [Mersenne Twister 64-bit (MT19937-64)](https://en.wikipedia.org/wiki/Mersenne_Twister)

All random number generators provided by Lenalysis.Random generate 
integral values in the range [0, 2^N-1] for some N (32 or 64).  In 
order to use these in practical applications, one typically needs to
convert these integers into one or more different ranges of values 
([0, 1), [0, 1], [-0.5, 0.5], etc.).  To facilitate this transformation
the Lenalysis.Random library provides various different transforms to
create what is called a Double Sequence by the library 
(`IDoubleSequence` interface) with the appropriate behavior.

The double sequences provided by this library are as follows:

* values between [0, 1] (inclusive)
* values between [0, 1) (inclusive lower, exclusive upper bound)
* values between (0, 1) (exclusive)
* values between [-0.5, +0.5] (inclusive)
* values between arbitrary bounds [a, b] (inclusive)
* values between arbitrary bounds [a, b) (inclusive lower, exclusive upper bound)

All of these sequences are provided through a static facade class called
`DoubleSequence`.  Use the static methods on this class to generate the desired
sequence from a particular input sequence.

Additionally, the double sequences listed above are all available
for values low-resolution or high-resolution when they are based
on a 32-bit RNG (`IIntegerSequence32`).  For high-resolution sequences
each double value uses two input 32-bit values from the underlying RNG
to build a single 53-bit result.  For low-resolution sequences, the 
32-bit range is used instead (and a single input value is used).  The
low-resolution approach is what is typically used in practice with 32-bit
MT 19937.

To use high-resolution double results with a 32-bit input sequence,
use the 'use53Bit' parameter on the factory functions.

An example of how to use the 32-bit MT implementation to provide 
high-resolution values from the range [0, 10] is below:

```c#
var seedValue = 12345;
var mt = new MersenneTwister32(seedValue);
var seq = DoubleSequence.RangeClosed(mt, 0, 10, true);

for(var i=0; i<100; i++)
    Console.Log($"[{i}] {seq.Next()}");
```