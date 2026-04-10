
using SmartDocAnalyzer.Application.Interfaces;
using SmartDocAnalyzer.Domain.Entities;
using SmartDocAnalyzer.Infrastructure.Helpers;

namespace SmartDocAnalyzer.Infrastructure.Services;

public class DocumentVectorService
{
    private readonly LiteDbContext _db;
    private readonly IEmbeddingService _embedding;

    public DocumentVectorService(LiteDbContext db, IEmbeddingService embedding)
    {
        _db = db;
        _embedding = embedding;
    }

    public async Task StoreChunks(string docId, List<string> chunks)
    {
        foreach (var chunk in chunks)
        {
            var vector = _embedding.GetEmbedding(chunk);

            _db.Chunks.Insert(new DocumentChunk
            {
                DocumentId = docId,
                Content = chunk,
                Embedding = vector
            });
        }
    }

    public async Task StoreDocInfo(Document doc)
    {
        _db.Documents.Insert(new Document
        {
            Id = doc.Id,
            FileName = doc.FileName,
            FileType = doc.FileType,
            UploadedAt = DateTime.UtcNow
        });
    }

    [Obsolete]
    public async Task<List<string>> SearchOld(string question, string docId)
    {
        var queryVector = _embedding.GetEmbedding(question);

        var chunks = _db.Chunks.Find(x => x.DocumentId == docId);

        //var results = chunks
        //    .Select(c => new
        //    {
        //        Text = c.Content,
        //        Score = SimilarityHelper.Cosine(queryVector, c.Embedding)
        //    })
        //    .OrderByDescending(x => x.Score)
        //    .Take(10)
        //    .Select(x => x.Text)
        //    .ToList();

        var results = chunks.Take(10)
        .Select(x => x.Content)
        .ToList();

        return results;
    }

    public async Task<List<string>> Search(string query, string docId)
    {
        var queryEmbedding = _embedding.GetEmbedding(query);
        var chunks = _db.Chunks.Find(x => x.DocumentId == docId);

        var results = chunks
            .Select(c => new
            {
                Text = c.Content,
                Score = SimilarityHelper.CosineSimilarity(queryEmbedding, c.Embedding)
            })
            .OrderByDescending(x => x.Score)
            .Take(5)
            .Select(x => x.Text)
            .ToList();

        return results;
    }

    
}
