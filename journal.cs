public class Journal
{
    public static List<string>? journal;
    private static string path = "journal.txt";

    public static void Load()
    {
        if (!File.Exists(path)) { File.Create(path); }
        journal = File.ReadAllLines(path).Where(line => line != "").ToList(); // filter out empty lines
        Log.Info("Loaded journal of length " + journal.Count);
    }

    public static string ReadDate(string date)
    { // todo: return between 3am and 3am the next day, not the whole day
        return string.Join("\n", journal!.Where(line => line.StartsWith("[" + date)).ToList());
    }

    public static void AddEntry(string entry)
    {
        string timestamp = "[" + DateTime.Now.ToString("yyyy-MM-dd hh:mmtt") + "] ";
        journal!.Add(timestamp + entry);
    }
}