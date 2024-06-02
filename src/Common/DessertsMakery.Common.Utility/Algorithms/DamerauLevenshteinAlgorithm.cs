namespace DessertsMakery.Common.Utility.Algorithms;

public sealed class DamerauLevenshteinAlgorithm
{
    /// <summary>
    /// Computes the Damerau-Levenshtein Distance between two strings, represented as arrays of
    /// integers, where each integer represents the code point of a character in the source string.
    /// Includes an optional threshold which can be used to indicate the maximum allowable distance.
    /// </summary>
    /// <param name="source">The first string</param>
    /// <param name="target">The second string</param>
    /// <param name="threshold">Maximum allowable distance</param>
    /// <returns>Int.MaxValue if threshold exceeded; otherwise the Damerau-Levenshtein distance between the strings</returns>
    public static int Distance(string source, string target, int threshold = int.MaxValue)
    {
        var length1 = source.Length;
        var length2 = target.Length;

        // Return trivial case - difference in string lengths exceeds threshold
        if (Math.Abs(length1 - length2) > threshold)
        {
            return int.MaxValue;
        }

        // Ensure arrays [i] / length1 use shorter length
        if (length1 > length2)
        {
            Swap(ref target, ref source);
            Swap(ref length1, ref length2);
        }

        var maxI = length1;
        var maxJ = length2;

        var dCurrent = new int[maxI + 1];
        var dMinus1 = new int[maxI + 1];
        var dMinus2 = new int[maxI + 1];

        for (var i = 0; i <= maxI; i++)
        {
            dCurrent[i] = i;
        }

        var jm1 = 0;

        for (var j = 1; j <= maxJ; j++)
        {
            // Rotate
            var dSwap = dMinus2;
            dMinus2 = dMinus1;
            dMinus1 = dCurrent;
            dCurrent = dSwap;

            // Initialize
            var minDistance = int.MaxValue;
            dCurrent[0] = j;
            var im1 = 0;
            var im2 = -1;

            for (var i = 1; i <= maxI; i++)
            {
                var cost = source[im1] == target[jm1] ? 0 : 1;

                var del = dCurrent[im1] + 1;
                var ins = dMinus1[i] + 1;
                var sub = dMinus1[im1] + cost;

                // Fastest execution for min value of 3 integers
                var min =
                    del > ins
                        ? ins > sub
                            ? sub
                            : ins
                        : del > sub
                            ? sub
                            : del;

                if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                    min = Math.Min(min, dMinus2[im2] + cost);

                dCurrent[i] = min;
                if (min < minDistance)
                {
                    minDistance = min;
                }

                im1++;
                im2++;
            }

            jm1++;
            if (minDistance > threshold)
            {
                return int.MaxValue;
            }
        }

        var result = dCurrent[maxI];
        return result > threshold ? int.MaxValue : result;
    }

    private static void Swap<T>(ref T arg1, ref T arg2) => (arg1, arg2) = (arg2, arg1);
}
