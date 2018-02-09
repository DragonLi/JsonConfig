public class MoveActionInfo : BaseActionInfo
{
	public const string TYPE = "move";

	public float time;
	public float distance;
	public bool center;

	//bug should move the following code into the virtual fucntion FillInfo
	static public BaseActionInfo ToBaseActionInfo(JsonActionInfo json)
	{
		MoveActionInfo info = new MoveActionInfo ();
		info.FillInfo (json);
		info.time = json.time;
		info.distance = json.distance;
		info.center = json.center;

		return info;
	}

	public override void FillInfo(JsonActionInfo json)
	{
		base.FillInfo(json);
		time = json.time;
		distance = json.distance;
		center = json.center;		
	}
}