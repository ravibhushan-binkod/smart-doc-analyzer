
namespace SmartDocAnalyzer.Domain.Entities;

public class ChatHistory
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DocumentId { get; set; }
    public string Role { get; set; } // "user" | "ai"
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
