
using LiteDB;
using SmartDocAnalyzer.Domain.Entities;

namespace SmartDocAnalyzer.Infrastructure.Services;

public class ChatHistoryService
{
    private readonly LiteDbContext _db;

    public ChatHistoryService(LiteDbContext db)
    {
        _db = db;
    }

    public void Add(ChatHistory chat)
    {
        _db.ChatHistory.Insert(chat);
    }

    public List<ChatHistory> GetByDocument(string docId)
    {
        return _db.ChatHistory
            .Find(x => x.DocumentId == docId)
            .OrderBy(x => x.CreatedAt)
            .ToList();
    }

    public void ClearByDocument(string docId)
    {
        _db.ChatHistory.DeleteMany(x => x.DocumentId == docId);
    }
}