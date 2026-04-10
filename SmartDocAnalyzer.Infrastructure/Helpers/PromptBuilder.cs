
using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;

namespace SmartDocAnalyzer.Infrastructure.Helpers;

public class PromptBuilder
{
    public string Build(List<string> chunks, string question)
    {
        var context = string.Join("\n\n", chunks);

        return $@"
        You are an intelligent document assistant.

        Answer ONLY from the provided context.
        If exact answer is not found, try to infer from context.
        Do NOT say 'Not found' unless absolutely nothing is relevant.

        Always give structured and clear answers.

        Context:
        ----------------
        {context}
        ----------------

        Question:
        {question}

        Answer:
        ";
    }
}
