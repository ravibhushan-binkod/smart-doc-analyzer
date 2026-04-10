
using SmartDocAnalyzer.Application.Interfaces;

namespace SmartDocAnalyzer.Infrastructure.AI;

public class SimpleEmbeddingService : IEmbeddingService
{
    public float[] GetEmbedding(string text)
    {
        return text
            .ToLower()
            .Split(' ')
            .GroupBy(x => x)
            .Select(g => (float)g.Count())
            .ToArray();
    }
}
