using Microsoft.AspNetCore.Http;
using SmartDocAnalyzer.Application.Interfaces;
using SmartDocAnalyzer.Infrastructure.DocumentProcessing;

namespace SmartDocAnalyzer.Infrastructure.Services;

public class DocumentService
{
    private readonly IStorageService _storage;
    private readonly DocumentParserFactory _factory;
    private readonly IChunkService _chunkService;
    private readonly DocumentVectorService _vectorService;

    public DocumentService(IStorageService storage, DocumentParserFactory factory,
        IChunkService chunkService, DocumentVectorService vectorService)
    {
        _storage = storage;
        _factory = factory;
        _chunkService = chunkService;
        _vectorService = vectorService;
    }

    public async Task<string> ProcessDocument(IFormFile file)
    {
        // 1. Generate Document ID FIRST
        var docId = Guid.NewGuid().ToString();

        // 2. Append docId to file name
        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        var ext = Path.GetExtension(file.FileName);

        //var newFileName = $"{fileName}_{docId}{ext}";

        // 3. Save file with new name
        var filePath = await _storage.SaveFileAsync(file, file.FileName);

        // 4. Extract text
        var parser = _factory.GetParser(ext);
        var text = await parser.ExtractTextAsync(filePath);

        // DEBUG LOGS
        Console.WriteLine($"DocumentId: {docId}");
        Console.WriteLine($"File: {file.FileName}");
        Console.WriteLine($"Text Length: {text?.Length}");
        Console.WriteLine($"Text: {text}");

        // Store document info
        await _vectorService.StoreDocInfo(new Domain.Entities.Document() { 
            Id = docId,
            FileName = file.FileName,
            FileType = ext,
        });

        // 5. Validate text
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new Exception("Text extraction failed or empty document.");
        }

        // 6. Clean text (important for AI)
        text = text.Replace("\n", " ")
                   .Replace("\r", " ")
                   .Trim();

        // 7. Split into chunks
        var chunks = _chunkService.Split(text);

        Console.WriteLine($"Chunks Created: {chunks.Count}");

        if (chunks.Count == 0)
        {
            throw new Exception("Chunking failed.");
        }

        // 8. Store chunks + embeddings
        await _vectorService.StoreChunks(docId, chunks);

        Console.WriteLine("Chunks stored successfully.");

        // 9. Return docId
        return docId;
    }
}
