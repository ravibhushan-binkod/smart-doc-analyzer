
namespace SmartDocAnalyzer.Infrastructure.AI;

using Microsoft.Extensions.Configuration;
using SmartDocAnalyzer.Application.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class OpenAIService : IAIService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public OpenAIService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["OpenAI:ApiKey"];
    }

    public async Task<string> AskAsync(string prompt)
    {
        var requestBody = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _http.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseContent);

        var root = doc.RootElement;

        // Check if error exists
        if (root.TryGetProperty("error", out var error))
        {
            var msg = error.GetProperty("message").GetString();
            return $"OpenAI Error: {msg}";
        }

        // Safe extraction
        if (root.TryGetProperty("choices", out var choices) &&
            choices.GetArrayLength() > 0)
        {
            var content = choices[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content;
        }

        return "No response from OpenAI";
    }
}