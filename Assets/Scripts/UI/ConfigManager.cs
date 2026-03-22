using System.Collections;
using System.IO;
using Banzai.Routiner;
using UnityEngine;

namespace UI
{
	public class ConfigManager : AbstractManager<ConfigManager>
	{
		public ADConfig ADConfig
		{
			get;
			private set;
		}

		private string filePath => Application.persistentDataPath + "/config.xml";

		protected override void InitInternal()
		{
			ADConfig = new ADConfig();
			ParseSaved();
			Routiner.Go(StartDownloadConfig());

		}

		private void ParseSaved()
		{
			if (File.Exists(filePath))
			{
				ParseXML(FileUtils.ReadAllText(filePath, true));
			}
		}

		private IEnumerator StartDownloadConfig()
		{
			return null;
		}

		private void ParseXML(string content)
		{
			var doc = XmlUtils.OpenXMLDocumentFromString(content);
			ADConfig.Parse(doc["Root"]["AD"]);
			FileUtils.WriteAllText(filePath, content, true);
		}
	}
}
