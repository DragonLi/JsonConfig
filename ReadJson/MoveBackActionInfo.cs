public class MoveBackActionInfo : BaseActionInfo
{
	public const string TYPE = "moveBack";
	
	public float time; //move'speed

	//bug should move the following code into the virtual fucntion FillInfo
	static public BaseActionInfo ToBaseActionInfo(JsonActionInfo json)
	{
		MoveBackActionInfo info = new MoveBackActionInfo ();
		info.FillInfo (json);
		info.time = json.time;
		
		return info;
	}

	public override void FillInfo(JsonActionInfo info)
	{
		base.FillInfo(info);
		time = info.time;
	}
}

