using System.Text;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace OllamaGptOss.Services;
/// <summary>
/// Encapsula a comunicação com o modelo via Ollama + Microsoft.Extensions.AI.
/// </summary>
public sealed class ChatService
{
    private readonly IChatClient _client;

    public ChatService(Uri endpoint, string model)
    {
        _client = new OllamaApiClient(endpoint, model);
    }

    /// <summary>
    /// Envia a conversa atual e a nova entrada do usuário, fazendo streaming dos tokens.
    /// </summary>
    /// <param name="history">Histórico atual (apenas leitura).</param>
    /// <param name="userInput">Mensagem do usuário.</param>
    /// <param name="onToken">Callback chamado a cada token recebido (opcional).</param>
    /// <returns>Texto completo da resposta do assistente.</returns>
    public async Task<string> SendAsync(
        System.Collections.Generic.IReadOnlyList<ChatMessage> history,
        string userInput,
        Action<string>? onToken = null)
    {
        // Cria uma cópia do histórico + a nova entrada do usuário
        var buffer = new System.Collections.Generic.List<ChatMessage>(history)
        {
            new ChatMessage(ChatRole.User, userInput)
        };

        var sb = new StringBuilder();

        await foreach (var update in _client.GetStreamingResponseAsync(buffer))
        {
            if (!string.IsNullOrEmpty(update.Text))
            {
                onToken?.Invoke(update.Text);
                sb.Append(update.Text);
            }
        }

        return sb.ToString();
    }
}