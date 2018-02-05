using System.Collections.Generic;

public class NewJsonBattleConfigInfo
{
	public string time = "";
	
	public List<SkillConfigInfo> list = new List<SkillConfigInfo>();	
}

public class JsonBattleConfigInfo
{
	public string time = "";
	public List<JsonSkillConfigInfo> list = new List<JsonSkillConfigInfo>();
}
