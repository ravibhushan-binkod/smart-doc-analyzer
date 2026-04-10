
using SmartDocAnalyzer.Application.Interfaces;
using System.Text;

namespace SmartDocAnalyzer.Infrastructure.Services;

public class ChunkService : IChunkService
{
    public List<string> Split(string text, int chunkSize = 500)
    {
        var chunks = new List<string>();
        int overlap = 100;

        for (int i = 0; i < text.Length; i += (chunkSize - overlap))
        {
            var length = Math.Min(chunkSize, text.Length - i);
            var chunk = text.Substring(i, length);

            chunks.Add(chunk);
        }

        return chunks;
    }

    [Obsolete]
    public List<string> SplitOld(string text, int chunkSize = 500)
    {
        var chunks = new List<string>();

        var sentences = text.Split('.', StringSplitOptions.RemoveEmptyEntries);

        var current = new StringBuilder();

        foreach (var sentence in sentences)
        {
            if (current.Length + sentence.Length < chunkSize)
            {
                current.Append(sentence).Append(". ");
            }
            else
            {
                chunks.Add(current.ToString());
                current.Clear();
                current.Append(sentence).Append(". ");
            }
        }

        if (current.Length > 0)
            chunks.Add(current.ToString());

        return chunks;
    }
}
