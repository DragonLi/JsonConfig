using System;
using System.IO;
using Newtonsoft.Json;
using JsonC = Newtonsoft.Json.JsonConvert;

namespace ReadJson
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConvertOldJsonFormat();
            TestNewJsonFormat();
        }

        private static void IndentOldJsonFormat()
        {
            string path = "BattleConfig.bytes";
            var json = JsonC.DeserializeObject<JsonBattleConfigInfo>(File.ReadAllText(path));
            
            var serializeObject = JsonC.SerializeObject(json, Formatting.Indented);
            File.WriteAllText("BattleConfig.json", serializeObject);
        }

        private static void ConvertOldJsonFormat()
        {
            string path = "BattleConfig.bytes";
            var json = JsonC.DeserializeObject<JsonBattleConfigInfo>(File.ReadAllText(path));
            var newjson = new NewJsonBattleConfigInfo {time = json.time};
            foreach (var jsonSkillConfigInfo in json.list)
            {
                newjson.list.Add(jsonSkillConfigInfo.ToSkillConfigInfo());
            }

            var serializeObject = JsonC.SerializeObject(newjson, Formatting.Indented);
            File.WriteAllText("NewBattleConfig.json", serializeObject);
        }

        private static void TestNewJsonFormat()
        {
            string path = "NewBattleConfig.json";
            var newjson = JsonC.DeserializeObject<NewJsonBattleConfigInfo>(File.ReadAllText(path));
            Console.ReadLine();
        }
    }
}