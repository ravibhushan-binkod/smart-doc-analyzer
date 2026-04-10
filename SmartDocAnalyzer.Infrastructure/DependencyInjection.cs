using Microsoft.Extensions.DependencyInjection;
using SmartDocAnalyzer.Application.Interfaces;
using SmartDocAnalyzer.Infrastructure.AI;
using SmartDocAnalyzer.Infrastructure.DocumentProcessing;
using SmartDocAnalyzer.Infrastructure.Services;
using SmartDocAnalyzer.Infrastructure.Storage;

namespace SmartDocAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<LiteDbContext>();
        services.AddHttpClient<OllamaService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(180);
        });
        services.AddHttpClient<OpenAIService>();

        services.AddScoped<IStorageService, FileStorageService>();

        services.AddScoped<PdfParser>();
        services.AddScoped<WordParser>();
        services.AddScoped<TextParser>();
        services.AddScoped<ImageOcrService>();

        services.AddScoped<DocumentParserFactory>();
        services.AddScoped<DocumentService>();

        services.AddScoped<IChunkService, ChunkService>();
        services.AddScoped<IEmbeddingService, SimpleEmbeddingService>();
        services.AddScoped<DocumentVectorService>();

        services.AddScoped<AIServiceFactory>();

        return services;
    }
}
