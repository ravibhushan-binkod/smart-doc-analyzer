
namespace SmartDocAnalyzer.Domain.Entities;

public class DocumentChunk
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DocumentId { get; set; }
    public string Content { get; set; }
    public float[] Embedding { get; set; }
}
