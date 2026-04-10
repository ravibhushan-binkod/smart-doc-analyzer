using SmartDocAnalyzer.Application.Interfaces;

namespace SmartDocAnalyzer.Infrastructure.DocumentProcessing;

public class TextParser : IDocumentParser
{
    public async Task<string> ExtractTextAsync(string filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }
}
