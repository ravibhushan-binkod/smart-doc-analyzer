namespace SmartDocAnalyzer.Domain.Entities;

public class ChatRequest
{
    public string DocumentId { get; set; }
    public string Question { get; set; }
    //public string Provider { get; set; } // "openai" or "ollama"
}
