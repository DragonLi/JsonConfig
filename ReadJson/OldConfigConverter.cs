using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using JsonC = Newtonsoft.Json.JsonConvert;

namespace ReadJson
{
    public class OldConfigConverter
    {
        public static void ConvertAndSaveNormalAttack()
        {
            var old = JsonC.DeserializeObject<BattleConfigInfo>(
                File.ReadAllText("NewBattleConfig.json"));
            var converted = new CorrectBattleConfigInfo {time = old.time};
            converted.list = FromOldNormalAttack(old.list);
            
            var serializeObject = JsonC.SerializeObject(
                converted, 
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
            File.WriteAllText("NewNormalAttackConfig.json", serializeObject);
        }
        
        public static CorrectSkillConfig FromOldNormalAttack(SkillConfigInfo oldCfg)
        {
            var result = new CorrectSkillConfig
            {
                id = oldCfg.id,
                name = oldCfg.name
            };
            var injuredPhrase = Convert(oldCfg.injurerActions);
            var attackPhrase = Convert(oldCfg.attackerActions,injuredPhrase);
            result.battlePhrase = attackPhrase;
            return result;
        }

        private static BattlePhraseBase Convert(List<BaseActionInfo> attackerList, BattlePhraseBase injuredPhrase)
        {
            var inserted = injuredPhrase;
            var lst = new BattlePhraseBase[attackerList.Count];
            for (var i = 0; i < attackerList.Count; i++)
            {
                var actInfo = attackerList[i];
                var removeIndex = -1;
                for (var index = 0; index < actInfo.effects.Count; index++)
                {
                    var eff = actInfo.effects[index];
                    if (eff is TakeDamageEffectInfo)
                    {
                        removeIndex = index;
                    }
                }

                if (removeIndex >= 0)
                {
                    actInfo.effects.RemoveAt(removeIndex);
                }

                actInfo.initiator = ActionInitiator.Attacker;
                lst[i]=ActionPhrase.Create(actInfo);
                if (removeIndex > 0)
                {
                    lst[i] = lst[i].Parall(inserted);
                    inserted = null;
                }
            }

            return lst.ToSeq().Parall(inserted);
        }

        private static BattlePhraseBase Convert(List<BaseActionInfo> injuredList)
        {
            var lst = new BattlePhraseBase[injuredList.Count];
            for (var i = 0; i < injuredList.Count; i++)
            {
                var info = injuredList[i];
                info.initiator = ActionInitiator.Victim;
                lst[i]=ActionPhrase.Create(info);
            }
            return lst.ToSeq();
        }

        public static List<CorrectSkillConfig> FromOldNormalAttack(List<SkillConfigInfo> oldLst)
        {
            var result=new List<CorrectSkillConfig>();
            foreach (var oldCfg in oldLst)
            {
                if (oldCfg.name!="普攻") continue;
                var newCfg = FromOldNormalAttack(oldCfg);
                result.Add(newCfg);
            }

            return result;
        }
    }
}