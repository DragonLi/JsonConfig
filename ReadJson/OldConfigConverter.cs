using System;
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
            var jsonTxt = File.ReadAllText("ReformattedBattleConfig.json");
            var old = JsonC.DeserializeObject<BattleConfigInfo>(jsonTxt);
            var converted = new CorrectBattleConfigInfo {time = old.time};
            converted.list = FromOldNormalAttack(old.list);
            
            var serializeObject = JsonC.SerializeObject(
                converted, 
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
            File.WriteAllText("ConvertedNormalAttackConfig.json", serializeObject);
        }
        
        public static void ConvertOldAndSave()
        {
            var jsonTxt = File.ReadAllText("ReformattedBattleConfig.json");
            var old = JsonC.DeserializeObject<BattleConfigInfo>(jsonTxt);
            var converted = new CorrectBattleConfigInfo {time = old.time};
            converted.list = FromOld(old.list);
            
            var serializeObject = JsonC.SerializeObject(
                converted, 
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
            File.WriteAllText("ConvertedBattleConfig.json", serializeObject);
        }
        
        public static CorrectSkillConfig FromOld(SkillConfigInfo oldCfg)
        {
            var injuredPhrase = Convert(oldCfg.injurerActions);
            var attackPhrase = Convert(oldCfg.attackerActions,injuredPhrase);
            if (attackPhrase == null) return null;
            
            var result = new CorrectSkillConfig
            {
                id = oldCfg.id,
                name = oldCfg.name,
                battlePhrase = attackPhrase
            };
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
                    eff.type = null;
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
                FixTime(actInfo);
                lst[i]=ActionPhrase.Create(actInfo);
                if (removeIndex >= 0)
                {
                    lst[i] = lst[i].Parall(inserted);
                    inserted = null;
                }
            }

            return lst.ToSeq().Parall(inserted);
        }

        private static void FixTime(BaseActionInfo actInfo)
        {
            actInfo.type = null;
            var move = actInfo as MoveActionInfo;
            if (move != null && Math.Abs(move.time) < float.Epsilon)
            {
                move.time = 1f;
            }

            var moveback = actInfo as MoveBackActionInfo;
            if (moveback != null && Math.Abs(moveback.time) < float.Epsilon)
            {
                moveback.time = 1f;
            }
        }

        private static BattlePhraseBase Convert(List<BaseActionInfo> injuredList)
        {
            var lst = new BattlePhraseBase[injuredList.Count];
            for (var i = 0; i < injuredList.Count; i++)
            {
                var info = injuredList[i];
                info.initiator = ActionInitiator.Victim;
                foreach (var eff in info.effects)
                {
                    eff.type = null;
                }
                FixTime(info);
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
                var newCfg = FromOld(oldCfg);
                if (newCfg == null) continue;
                result.Add(newCfg);
            }

            return result;
        }
        
        public static List<CorrectSkillConfig> FromOld(List<SkillConfigInfo> oldLst)
        {
            var result=new List<CorrectSkillConfig>();
            foreach (var oldCfg in oldLst)
            {
                var newCfg = FromOld(oldCfg);
                if (newCfg == null) continue;
                result.Add(newCfg);
            }
            return result;
        }
    }
}