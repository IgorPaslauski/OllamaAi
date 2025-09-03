namespace OllamaGptOss.Config;

public static class AppSettings
{
    public const string Endpoint = "http://localhost:11434/";
    public const string Modelo = "gpt-oss:20b";
    public const int LimiteHistorico = 40;

    public static Uri GetEndpointUri() => new(Endpoint);
}