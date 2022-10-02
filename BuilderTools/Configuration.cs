using Newtonsoft.Json;
using TShockAPI;

namespace buildertools;

public class Configuration
{
    private static readonly string _uri = Path.Combine(TShock.SavePath, "btoolsConfiguration.json");

    #region ConfigSettings

    public int Cooldown = 30; // default is 30 seconds

    public List<int> Items = new()
    {
        1543, 1544, 1545, 3611, 496, 1326, 3620, 510, 3061, 5010, 407, 1923, 2215, 3624, 4989, 4954, 4409, 4008, 410, 411, 4444
    };

    #endregion ConfigSettings

    public static Configuration Read() =>
        !File.Exists(_uri) ? new Configuration().Write()
        : JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(_uri));

    public Configuration Write()
    {
        if (!File.Exists(_uri))
            (new FileInfo(_uri)).Directory.Create();

        File.WriteAllText(_uri,
            JsonConvert.SerializeObject(this, Formatting.Indented));

        return this;
    }
}