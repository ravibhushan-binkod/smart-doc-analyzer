using SmartDocAnalyzer.Application.Interfaces;
using Tesseract;

namespace SmartDocAnalyzer.Infrastructure.DocumentProcessing;

public class ImageOcrService : IDocumentParser
{
    public Task<string> ExtractTextAsync(string filePath)
    {
        //using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);

        var tessPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/tessdata");
        using var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default);

        using var img = Pix.LoadFromFile(filePath);
        using var page = engine.Process(img);

        return Task.FromResult(page.GetText());
    }
}
