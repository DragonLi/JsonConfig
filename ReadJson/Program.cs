using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using JsonC = Newtonsoft.Json.JsonConvert;

namespace ReadJson
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IndentOldJsonFormat();
            //ConvertOldJsonFormat();
            //TestShortTypeName();
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
            var newjson = new NewJsonBattleConfigInfo {time = json.time,list = new List<SkillConfigInfo>(json.list.Count)};
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

        private static void TestShortTypeName()
        {
            string path = "NormalAttack.json";
            var newjson = JsonC.DeserializeObject<SkillConfigInfo>(File.ReadAllText(path));
            Console.ReadLine();            
        }
    }
}