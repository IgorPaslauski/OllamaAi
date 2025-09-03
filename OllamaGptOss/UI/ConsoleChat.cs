using OllamaGptOss.Services;

namespace OllamaGptOss.UI;

/// <summary>
/// Camada de UI para interação via console.
/// </summary>
public sealed class ConsoleChat
{
    private readonly HistoryService _history;
    private readonly ChatService _chat;

    public ConsoleChat(HistoryService history, ChatService chat)
    {
        _history = history;
        _chat = chat;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Digite 'sair' para encerrar.\n");

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("Você: ");
            var entrada = Console.ReadLine();

            if (entrada is null) break; // EOF
            if (string.Equals(entrada.Trim(), "sair", StringComparison.OrdinalIgnoreCase)) break;
            if (string.IsNullOrWhiteSpace(entrada)) continue;

            // Guarda a mensagem do usuário
            _history.AddUser(entrada);

            Console.Write("Assistente: ");

            try
            {
                var resposta = await _chat.SendAsync(
                    _history.Snapshot(),
                    entrada,
                    onToken: token => Console.Write(token)
                );

                _history.AddAssistant(resposta);
                Console.WriteLine("\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[erro] {ex.Message}\n");
            }
        }
    }
}