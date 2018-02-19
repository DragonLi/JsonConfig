//动作模板实例化模式
public enum ActionSchemeInstantiationMode
{
	Seq,//按服务器发送目标的顺序串联
	Par,//多目标并联
	//LastVictim,//上一个受击者
}

public class NormalActionInfo : BaseActionInfo
{
	public const string TYPE = "normal";

	public ActionSchemeInstantiationMode instMode;//动作模板实例化模式
	public float startTime; //action start time
	public float delayTime;//action delayed time

	//攻击动作是否可变化
	public bool AnimationChangeable = false;
	//（多段攻击的）攻击动作列表
	public string AttackerActions ;
	//（多段攻击的）攻击方向列表
	public string AttackerDirections;
	/**（多段攻击的）攻击时长列表*/
	public string AttackerDurations;
	
	//bug should move the following code into the virtual fucntion FillInfo
	static public BaseActionInfo ToBaseActionInfo(JsonActionInfo json)
	{
		NormalActionInfo info = new NormalActionInfo ();
		info.FillInfo (json);
		info.startTime = json.startTime;
		info.delayTime = json.delayTime;
		
		info.AnimationChangeable = json.AnimationChangeable;
		info.AttackerActions = json.AttackerActions;
		info.AttackerDirections = json.AttackerDirections;
		info.AttackerDurations = json.AttackerDurations;
		
		return info;
	}

	public override void FillInfo(JsonActionInfo json)
	{
		base.FillInfo(json);
		startTime = json.startTime;
		delayTime = json.delayTime;
		
		AnimationChangeable = json.AnimationChangeable;
		AttackerActions = json.AttackerActions;
		AttackerDirections = json.AttackerDirections;
		AttackerDurations = json.AttackerDurations;
	}
}

