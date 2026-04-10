
namespace SmartDocAnalyzer.Application.Interfaces;

public interface IDocumentParser
{
    Task<string> ExtractTextAsync(string filePath);
}
