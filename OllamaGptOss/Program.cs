using System.Text;
using Microsoft.Extensions.AI;
using OllamaSharp;

const string ENDPOINT = "http://localhost:11434/";
const string MODELO = "gpt-oss:20b";
const int LIMITE_HISTORICO = 40;

IChatClient chatClient = new OllamaApiClient(new Uri(ENDPOINT), MODELO);
List<ChatMessage> historico = new();

Console.WriteLine($"Chat {MODELO} — digite 'sair' para encerrar.\n");

while (true)
{
    Console.Write("Você: ");
    var entrada = Console.ReadLine();

    if (entrada is null) break;
    if (entrada.Trim().ToLower() == "sair") break;
    if (string.IsNullOrWhiteSpace(entrada)) continue;

    historico.Add(new ChatMessage(ChatRole.User, entrada));
    if (historico.Count > LIMITE_HISTORICO)
        historico.RemoveRange(0, historico.Count - LIMITE_HISTORICO);
    Console.Write("Assistente: ");
    var respostaSb = new StringBuilder();

    try
    {
        await foreach (var update in chatClient.GetStreamingResponseAsync(historico))
        {
            if (!string.IsNullOrEmpty(update.Text))
            {
                Console.Write(update.Text);
                respostaSb.Append(update.Text);
            }
        }
    }
    catch (Exception ex)
    {
        Console.Write($"\n[erro] {ex.Message}");
    }

    historico.Add(new ChatMessage(ChatRole.Assistant, respostaSb.ToString()));
    Console.WriteLine("\n");
}