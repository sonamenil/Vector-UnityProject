using System;

namespace PlayerData
{
	[Serializable]
	public class Options
	{
		public float SoundLevel;

		public float MusicLevel;

		public float LastUsedSetSoundValue;

		public float LastUsedSetMusicValue;

		public bool Sound;

		public bool Music;

		public bool GDPR;

		public string Locale;

		public int CurrentLocaleIndex;
	}
}
