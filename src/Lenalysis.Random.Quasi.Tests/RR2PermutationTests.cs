namespace Lenalysis.Random.Quasi.Tests;

public class RR2PermutationTests
{
    [Fact]
    public void SimpleTest()
    {
        // example from page 273 of Computational Investigations of Low-Discrepancy Sequences article
        var perm = RR2Permutation.MakePermutation(new[] { 31, 29, 23, 19, 17, 13 });

        perm.Length.ShouldBe(6);

        perm[0].Length.ShouldBe(31);
        perm[1].Length.ShouldBe(29);
        perm[2].Length.ShouldBe(23);
        perm[3].Length.ShouldBe(19);
        perm[4].Length.ShouldBe(17);
        perm[5].Length.ShouldBe(13);

        perm[0].ShouldBe(new[]{ 0, 16, 8, 24, 4, 20, 12, 28, 2, 18, 10, 26, 6, 22, 14, 30, 1, 17, 9, 25, 5, 21, 13, 29, 3, 19, 11, 27, 7, 23, 15 });
        perm[1].ShouldBe(new[]{ 0, 16, 8, 24, 4, 20, 12, 28, 2, 18, 10, 26, 6, 22, 14, 1, 17, 9, 25, 5, 21, 13, 3, 19, 11, 27, 7, 23, 15 });
        perm[2].ShouldBe(new[]{ 0, 16, 8, 4, 20, 12, 2, 18, 10, 6, 22, 14, 1, 17, 9, 5, 21, 13, 3, 19, 11, 7, 15 });
        perm[3].ShouldBe(new[]{ 0, 16, 8, 4, 12, 2, 18, 10, 6, 14, 1, 17, 9, 5, 13, 3, 11, 7, 15 });
        perm[4].ShouldBe(new[]{ 0, 16, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15 });
        perm[5].ShouldBe(new[]{ 0, 8, 4, 12, 2, 10, 6, 1, 9, 5, 3, 11, 7 });
    }
}
