using System;

namespace Xml2Prefab
{
	[Serializable]
	public class ChoiceContainer
	{
		public string Name;

		public string Variant;

		public ChoiceContainer(string name, string variant)
		{
			Name = name;
			Variant = variant;
		}

		public ChoiceContainer()
		{
		}
	}
}
