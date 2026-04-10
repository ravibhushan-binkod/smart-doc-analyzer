using Microsoft.AspNetCore.Mvc;
using SmartDocAnalyzer.Domain.Entities;
using SmartDocAnalyzer.Infrastructure.AI;
using SmartDocAnalyzer.Infrastructure.Helpers;
using SmartDocAnalyzer.Infrastructure.Services;

namespace SmartDocAnalyzer.API.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly DocumentVectorService _vector;
    private readonly OllamaService _ai;
    private readonly AIServiceFactory _factory;
    private readonly ChatHistoryService _chatHistory;

    public ChatController(DocumentVectorService vector, OllamaService ai,
    AIServiceFactory factory, ChatHistoryService chatHistory)
    {
        _vector = vector;
        _ai = ai;
        _factory = factory;
        _chatHistory = chatHistory;
    }    

    [HttpPost("ask-ollama")]
    public async Task<IActionResult> AskOllama([FromBody] ChatRequest request)
    {
        // History: Save USER Question
        _chatHistory.Add(new ChatHistory
        {
            DocumentId = request.DocumentId,
            Role = "user",
            Message = request.Question
        });

        var chunks = await _vector.Search(request.Question, request.DocumentId);
        var prompt = new PromptBuilder().Build(chunks, request.Question);
        var answer = await _ai.AskAsync(prompt);

        // History: Save AI response
        _chatHistory.Add(new ChatHistory
        {
            DocumentId = request.DocumentId,
            Role = "ai",
            Message = string.IsNullOrEmpty(answer) ? "⚠️ No response from AI" : answer
        });

        return Ok(new
        {
            model = "ollama",
            response = answer,
            documentId = request.DocumentId
        });
    }

    [Obsolete]
    [HttpPost("ask-openai")]
    public async Task<IActionResult> AskOpenAI([FromBody] ChatRequest request)
    {
        var chunks = await _vector.Search(request.Question, request.DocumentId);

        var prompt = new PromptBuilder().Build(chunks, request.Question);

        var aiService = _factory.GetService("openai");

        var answer = await aiService.AskAsync(prompt);

        return Ok(new
        {
            model = "openai",
            response = answer,
            documentId = request.DocumentId
        });
    }

    [HttpGet("history/{docId}")]
    public IActionResult GetHistory(string docId)
    {
        var history = _chatHistory.GetByDocument(docId);
        return Ok(history);
    }

    [HttpDelete("history/{docId}")]
    public IActionResult Clear(string docId)
    {
        _chatHistory.ClearByDocument(docId);
        return Ok();
    }
}
