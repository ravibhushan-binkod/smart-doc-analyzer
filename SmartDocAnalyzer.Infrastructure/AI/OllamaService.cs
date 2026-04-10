using SmartDocAnalyzer.Application.Interfaces;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SmartDocAnalyzer.Infrastructure.AI;

public class OllamaService //: IAIService
{
    private readonly HttpClient _http;

    public OllamaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> AskAsync(string prompt)
    {
        var response = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", new
        {
            model = "llama3",
            prompt = prompt,
            stream = true // default, but explicit
        });

        var stream = await response.Content.ReadAsStreamAsync();
        var reader = new StreamReader(stream);

        var fullResponse = new StringBuilder();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                var json = JsonDocument.Parse(line);

                if (json.RootElement.TryGetProperty("response", out var chunk))
                {
                    fullResponse.Append(chunk.GetString());
                }
            }
            catch
            {
                // ignore malformed lines
            }
        }

        return fullResponse.ToString();
    }

    public async Task<string> SummarizeAsync(string text)
    {
        var prompt = $"Summarize this:\n{text}";

        return await AskAsync(prompt);
    }
}
