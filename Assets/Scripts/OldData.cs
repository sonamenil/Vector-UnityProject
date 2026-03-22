using System.Collections.Generic;
using PlayerData;

public class OldData : BaseUserHolder<OldData>
{
	public Dictionary<string, string> _oldData;

	protected override string JSONName => "OldData";

	public override void ParseData()
	{
	}

	public override void SaveData()
	{
	}

	public void SetOldData(Dictionary<string, string> oldData)
	{
	}

	public void Migrate(PlayerDataComponent playerData)
	{
	}
}
