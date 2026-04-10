using Microsoft.Extensions.DependencyInjection;
using SmartDocAnalyzer.Application.Interfaces;

namespace SmartDocAnalyzer.Infrastructure.DocumentProcessing;

public class DocumentParserFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DocumentParserFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDocumentParser GetParser(string fileType)
    {
        return fileType.ToLower() switch
        {
            ".pdf" => _serviceProvider.GetService<PdfParser>(),
            ".docx" => _serviceProvider.GetService<WordParser>(),
            ".txt" => _serviceProvider.GetService<TextParser>(),
            ".jpg" or ".png" => _serviceProvider.GetService<ImageOcrService>(),
            _ => throw new NotSupportedException("Unsupported file type")
        };
    }
}
