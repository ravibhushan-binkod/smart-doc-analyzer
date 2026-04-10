using Microsoft.Extensions.DependencyInjection;
using SmartDocAnalyzer.Application.Interfaces;

namespace SmartDocAnalyzer.Infrastructure.AI;

public class AIServiceFactory
{
    private readonly IServiceProvider _provider;

    public AIServiceFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IAIService GetService(string type)
    {
        return type.ToLower() switch
        {
            "openai" => _provider.GetRequiredService<OpenAIService>(),
            //"ollama" => _provider.GetRequiredService<OllamaService>(),
            _ => throw new Exception("Invalid AI provider")
        };
    }
}
