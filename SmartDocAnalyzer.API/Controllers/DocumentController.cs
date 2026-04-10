using Microsoft.AspNetCore.Mvc;
using SmartDocAnalyzer.Infrastructure.AI;
using SmartDocAnalyzer.Infrastructure.Helpers;
using SmartDocAnalyzer.Infrastructure.Services;

namespace SmartDocAnalyzer.API.Controllers;

[ApiController]
[Route("api/document")]
public class DocumentController : ControllerBase
{
    private readonly DocumentService _service;
    private readonly DocumentVectorService _vector;
    private readonly OllamaService _ollama;
    private readonly LiteDbContext _db;

    public DocumentController(DocumentService service, DocumentVectorService vector,
        OllamaService ollama, LiteDbContext db)
    {
        _service = service;
        _ollama = ollama;
        _db = db;
        _vector = vector;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var docId = await _service.ProcessDocument(file);

        return Ok(new
        {
            message = "Document processed successfully",
            documentId = docId
        });
    }

    [HttpPost("summarize")]
    public async Task<IActionResult> Summarize(IFormFile file)
    {
        var docId = await _service.ProcessDocument(file);
        var question = "generate a document or context summary";

        var chunks = await _vector.Search(question, docId);
        var prompt = new PromptBuilder().Build(chunks, question);
        var summary = await _ollama.AskAsync(prompt);

        return Ok(new
        {
            response = summary,
            documentId = docId
        });
    }

    [HttpGet("list")]
    public IActionResult GetAllDocuments()
    {
        var docs = _db.Documents.FindAll()
            .Select(d => new
            {
                documentId = d.Id,
                fileName = d.FileName,
                fileType = d.FileType,
                uploadedOn = d.UploadedAt
            })
            .OrderByDescending(x => x.uploadedOn)
            .ToList();

        return Ok(docs);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _db.Documents.Delete(id);
        _db.Chunks.DeleteMany(x => x.DocumentId == id);
        _db.ChatHistory.DeleteMany(x => x.DocumentId == id);

        return Ok(new { success = true, message = "Document deleted successfully" });
    }
}
