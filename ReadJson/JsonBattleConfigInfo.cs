using System.Collections.Generic;

public class NewJsonBattleConfigInfo
{
	public string time = "";
	
	public List<SkillConfigInfo> list;	
}

public class JsonBattleConfigInfo
{
	public string time = "";
	public List<JsonSkillConfigInfo> list = new List<JsonSkillConfigInfo>();
}
