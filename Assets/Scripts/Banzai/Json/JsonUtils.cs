using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Banzai.Json
{
	public static class JsonUtils
	{
		public static bool GetBool(this JToken jsonClass, string name, bool defValue)
		{
            var jsonObject = GetNode(jsonClass, name);
            if (jsonObject == null)
            {
                return defValue;
            }
            var str = AsString(jsonObject);
			if (str == "1")
			{
				return true;
			}
			if (str.ToUpper() == "TRUE")
			{
				return true;
			}
			return false;
        }

		public static int GetInt(this JToken jsonClass, string name, int defValue = 0)
		{
            var jsonObject = GetNode(jsonClass, name);
            if (jsonObject == null)
            {
                return defValue;
            }
            return AsInt(jsonObject);
        }

		public static float GetFloat(this JToken jsonClass, string name, float defValue = 0f)
		{
			var jsonObject = GetNode(jsonClass, name);
			if (jsonObject == null)
			{
				return defValue;
			}
			return AsFloat(jsonObject);
		}

		public static string GetText(this JToken jsonClass, string name, string defValue = null)
		{
            var jsonObject = GetNode(jsonClass, name);
            if (jsonObject == null)
            {
                return defValue;
            }
            return AsString(jsonObject);
        }

		public static JToken GetNode(this JToken node, string name)
		{
			if (node != null)
			{
				if (node[name] != null)
				{
					return node[name];
				}
				foreach (var child in node)
				{
					return GetNode(child, name);
				}
			}
			return null;
		}

		public static int AsInt(this JToken jsonObject)
		{
			return Extensions.Value<int>(jsonObject);
		}

		public static float AsFloat(this JToken jsonObject)
		{
            return Extensions.Value<float>(jsonObject);
        }

		public static string AsString(this JToken jsonObject)
		{
            return Extensions.Value<string>(jsonObject);
        }
	}
}
