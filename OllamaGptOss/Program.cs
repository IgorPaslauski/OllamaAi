using OllamaGptOss.Config;
using OllamaGptOss.Services;
using OllamaGptOss.UI;

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine($"Chat {AppSettings.Modelo} — conectado a {AppSettings.Endpoint}");
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
        };

        var history = new HistoryService(AppSettings.LimiteHistorico);
        var chat = new ChatService(AppSettings.GetEndpointUri(), AppSettings.Modelo);
        var console = new ConsoleChat(history, chat);

        using var cts = new CancellationTokenSource();
        await console.RunAsync(cts.Token);
    }
}