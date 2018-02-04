using System.IO;
using Newtonsoft.Json;
using JsonC = Newtonsoft.Json.JsonConvert;

namespace ReadJson
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string path = "BattleConfig.bytes";
            var json = JsonC.DeserializeObject<JsonBattleConfigInfo>(File.ReadAllText(path));
            var serializeObject = JsonC.SerializeObject(json, Formatting.Indented);
            File.WriteAllText("BattleConfig.json", serializeObject);
        }
    }
}