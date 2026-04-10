using Microsoft.AspNetCore.Http;
using SmartDocAnalyzer.Application.Interfaces;

namespace SmartDocAnalyzer.Infrastructure.Storage;

public class FileStorageService : IStorageService
{
    private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

    public async Task<string> SaveFileAsync(IFormFile file, string fileName)
    {
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);

        var filePath = Path.Combine(_basePath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return filePath;
    }
}
