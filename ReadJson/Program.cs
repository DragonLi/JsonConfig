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
            OldConfigConverter.ConvertOldAndSave();
            LoadConvertedBattleConfig();
            //OldConfigConverter.ConvertAndSaveNormalAttack();
            //SimulateCorrectSkillConfig.SaveSimulatedNormalAttack();
            //SimulateCorrectSkillConfig.TestSimulatedConfig();
            //IndentOldJsonFormat();
            //ConvertOldJsonFormat();
            //TestShortTypeName();
        }

        private static Dictionary<long,CorrectSkillConfig> LoadConvertedBattleConfig()
        {
            var jsonTxt = File.ReadAllText("ConvertedBattleConfig.json");
            var newCfgList = JsonC.DeserializeObject<CorrectBattleConfigInfo>(jsonTxt);
            var result = new Dictionary<long,CorrectSkillConfig>(newCfgList.list.Count+1);
            var hasDefault = false;
            foreach (var cfg in newCfgList.list)
            {
                result.Add(cfg.id,cfg);
                if (cfg.id == 0)
                    hasDefault = true;
            }

            if (!hasDefault)
            {
                result.Add(0,new CorrectSkillConfig());
            }

            return result;
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
            var newjson = new BattleConfigInfo {time = json.time,list = new List<SkillConfigInfo>(json.list.Count)};
            foreach (var jsonSkillConfigInfo in json.list)
            {
                newjson.list.Add(jsonSkillConfigInfo.ToSkillConfigInfo());
            }

            var serializeObject = JsonC.SerializeObject(newjson, Formatting.Indented,new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            File.WriteAllText("ReformattedBattleConfig.json", serializeObject);
        }

        private static void TestNewJsonFormat()
        {
            string path = "ReformattedBattleConfig.json";
            var newjson = JsonC.DeserializeObject<BattleConfigInfo>(File.ReadAllText(path));
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