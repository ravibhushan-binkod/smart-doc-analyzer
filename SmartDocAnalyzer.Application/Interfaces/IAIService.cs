
namespace SmartDocAnalyzer.Application.Interfaces;

public interface IAIService
{
    Task<string> AskAsync(string prompt);
}
