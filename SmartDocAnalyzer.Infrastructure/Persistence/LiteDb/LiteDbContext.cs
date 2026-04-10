using LiteDB;
using SmartDocAnalyzer.Domain.Entities;

public class LiteDbContext
{
    private readonly LiteDatabase _db;

    public LiteDbContext()
    {
        _db = new LiteDatabase("smartdoc.db");
    }

    public ILiteCollection<DocumentChunk> Chunks => _db.GetCollection<DocumentChunk>("chunks");
    public ILiteCollection<Document> Documents => _db.GetCollection<Document>("documents");
    public ILiteCollection<ChatHistory> ChatHistory => _db.GetCollection<ChatHistory>("chat_history");
}