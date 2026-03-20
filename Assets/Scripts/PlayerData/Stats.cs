using System;

namespace PlayerData
{
	[Serializable]
	public class Stats
	{
		public float TotalRunningDistance;

		public int JumpsCount;

		public int SlidesCount;

		public int BonusCollected;

		public int CoinsCollected;

		public int TricksPerformed;

		public int TracksPassed;

		public int TracksPassedWith3Stars;

		public int Death;

		public int DeathByHunter;

		public int HuntersKilled;

		public int GlassBroken;
	}
}
