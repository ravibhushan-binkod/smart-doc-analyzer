namespace SmartDocAnalyzer.Application.Interfaces;

public interface IChunkService
{
    List<string> Split(string text, int chunkSize = 500);
}
