
namespace SmartDocAnalyzer.Application.Interfaces;

public interface IEmbeddingService
{
    float[] GetEmbedding(string text);
}
