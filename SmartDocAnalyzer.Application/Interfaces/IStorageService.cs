using Microsoft.AspNetCore.Http;

namespace SmartDocAnalyzer.Application.Interfaces;

public interface IStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string fileName);
}
