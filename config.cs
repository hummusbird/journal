using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json;

public class Settings
{
    public bool JournalEnabled;
    public bool LoggingEnabled;
    public string? LogPath;
    public string? apikey;
}

public static class Config
{
    private const string path = "config.json";

    public static bool JournalEnabled { get; private set; } = true;
    public static bool LoggingEnabled { get; private set; } = true;
    public static string? LogPath { get; private set; } = "logs";
    private static string? apikey { get; set; } = "";


    public static void Load()
    {
        try
        {
            using (StreamReader SR = new StreamReader(path))
            {
                string json = SR.ReadToEnd();

                Settings? config = JsonConvert.DeserializeObject<Settings>(json);

                JournalEnabled = config!.JournalEnabled;
                LoggingEnabled = config!.LoggingEnabled;
                LogPath = config!.LogPath;
                apikey = config!.apikey;
            }
            if (!LoggingEnabled) { Log.Warning("Logging disabled! No logs will be written"); }
            Log.Info($"Loaded configuration file");
        }
        catch
        {
            Log.Error($"Unable to load {path}");
        }

        if (!File.Exists(path))
        {
            Save();
            Log.Info("Generated new configuration file");
        }
    }

    private static Settings Serialize()
    {
        Settings? config = new Settings
        {
            JournalEnabled = JournalEnabled,
            LoggingEnabled = LoggingEnabled,
            LogPath = LogPath,
            apikey = apikey
        };

        return config;
    }

    public static void Save()
    {
        try
        {
            using (StreamWriter SW = new StreamWriter(path, false))
            {
                SW.WriteLine(JsonConvert.SerializeObject(Serialize(), Formatting.Indented));
                Log.Info($"{path} saved!");
            }
        }
        catch
        {
            Log.Critical($"Unable to save {path}!");
        }
    }
}