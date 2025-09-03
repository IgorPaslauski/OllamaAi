using Microsoft.Extensions.AI;

namespace OllamaGptOss.Services;

/// <summary>
/// Gerencia o histórico de mensagens com um limite máximo.
/// </summary>
public sealed class HistoryService
{
    private readonly List<ChatMessage> _historico = new();
    private readonly int _limite;

    public HistoryService(int limite)
    {
        _limite = limite;
    }

    public IReadOnlyList<ChatMessage> Snapshot() => _historico.AsReadOnly();

    public void AddUser(string content)
    {
        _historico.Add(new ChatMessage(ChatRole.User, content));
        EnforceLimit();
    }

    public void AddAssistant(string content)
    {
        _historico.Add(new ChatMessage(ChatRole.Assistant, content));
        EnforceLimit();
    }

    private void EnforceLimit()
    {
        if (_historico.Count <= _limite) return;
        var excedente = _historico.Count - _limite;
        _historico.RemoveRange(0, excedente);
    }
}