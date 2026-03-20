using UnityEngine.UI;

public class NonDrawingGraphic : MaskableGraphic
{
	public override void SetMaterialDirty()
	{
		return;
	}

	public override void SetVerticesDirty()
	{
		return;
	}

	protected override void OnPopulateMesh(VertexHelper p_vh)
	{
		p_vh.Clear();
	}
}
