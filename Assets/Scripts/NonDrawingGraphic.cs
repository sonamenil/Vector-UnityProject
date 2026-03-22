using UnityEngine.UI;

public class NonDrawingGraphic : MaskableGraphic
{
	public override void SetMaterialDirty()
	{
	}

	public override void SetVerticesDirty()
	{
	}

	protected override void OnPopulateMesh(VertexHelper p_vh)
	{
		p_vh.Clear();
	}
}
