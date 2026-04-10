using DocumentFormat.OpenXml.Packaging;
using SmartDocAnalyzer.Application.Interfaces;

namespace SmartDocAnalyzer.Infrastructure.DocumentProcessing;

public class WordParser : IDocumentParser
{
    public Task<string> ExtractTextAsync(string filePath)
    {
        using var doc = WordprocessingDocument.Open(filePath, false);
        var body = doc.MainDocumentPart.Document.Body;
        return Task.FromResult(body.InnerText);
    }
}
