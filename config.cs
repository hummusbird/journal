using Newtonsoft.Json;

public class Settings
{
    public bool JournalEnabled;
    public string? apikey;
}

public static class Config
{
    public static bool? JournalEnabled { get; private set; } = true;
    private static string apikey { get; set; } = "";

}