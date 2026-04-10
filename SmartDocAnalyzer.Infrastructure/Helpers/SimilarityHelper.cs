
namespace SmartDocAnalyzer.Infrastructure.Helpers;

public static class SimilarityHelper
{
    public static float Cosine(float[] v1, float[] v2)
    {
        float dot = 0, mag1 = 0, mag2 = 0;

        for (int i = 0; i < Math.Min(v1.Length, v2.Length); i++)
        {
            dot += v1[i] * v2[i];
            mag1 += v1[i] * v1[i];
            mag2 += v2[i] * v2[i];
        }

        return dot / ((float)(Math.Sqrt(mag1) * Math.Sqrt(mag2)) + 1e-5f);
    }

    public static float CosineSimilarity(float[] a, float[] b)
    {
        var dot = a.Zip(b, (x, y) => x * y).Sum();
        var magA = Math.Sqrt(a.Sum(x => x * x));
        var magB = Math.Sqrt(b.Sum(x => x * x));

        return (float)(dot / (magA * magB + 1e-5));
    }
}
