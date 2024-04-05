public class Journal
{
    public static String[] journal;

    public static void Load()
    {
        journal = File.ReadAllLines("journal.txt").Where(line => line != "").ToArray(); // filter out empty lines
        Log.Info("Loaded journal of length " + journal.Length);
    }

    public static string ReadDate(string date)
    {
        return string.Join("\n", journal.Where(line => line.StartsWith("[" + date)).ToArray());
    }
}