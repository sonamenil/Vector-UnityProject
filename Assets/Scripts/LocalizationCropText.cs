using System.Text;

public class LocalizationCropText : LocalizationText
{
	private int _len_chars = 20;

	protected override void SetText(StringBuilder value)
	{
		if (_len_chars < value.Length)
		{
			var builder = new StringBuilder();
			for (int i = 0; i < _len_chars; i++)
			{
				builder.Append(value[i]);
			}
			builder.Append("...");
			_text.text = builder.ToString();
			return;
		}
		_text.text = value.ToString();
	}
}
