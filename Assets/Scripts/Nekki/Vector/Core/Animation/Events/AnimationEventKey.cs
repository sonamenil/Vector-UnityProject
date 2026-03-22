using System.Xml;
using Nekki.Vector.Core.Controllers;

namespace Nekki.Vector.Core.Animation.Events
{
	public class AnimationEventKey : AnimationEvent
	{
		private readonly Key _key;

		public AnimationEventKey(AnimationEventParam param, XmlNode node)
			: base(param)
		{
			_key = KeyVariables.Parse(node.Attributes["Key"].Value);
		}

		public bool IsKey(KeyVariables keysVariables, int sign)
		{
            if (sign == -1 && keysVariables.Key == Key.Right)
            {
                return _key == Key.Left;
            }
            if (sign == -1 && keysVariables.Key == Key.Left)
            {
                return _key == Key.Right;
            }
            return _key == keysVariables.Key;
        }
	}
}
