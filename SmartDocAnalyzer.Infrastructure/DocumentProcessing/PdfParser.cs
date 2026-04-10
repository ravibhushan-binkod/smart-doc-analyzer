using SmartDocAnalyzer.Application.Interfaces;
using UglyToad.PdfPig;

namespace SmartDocAnalyzer.Infrastructure.DocumentProcessing;

public class PdfParser : IDocumentParser
{
    public Task<string> ExtractTextAsync(string filePath)
    {
        var text = "";

        using (var document = PdfDocument.Open(filePath))
        {
            foreach (var page in document.GetPages())
            {
                text += page.Text;
            }
        }

        return Task.FromResult(text);
    }
}
