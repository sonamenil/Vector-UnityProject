using System;

namespace UI
{
	public class VideoScreenPayloadData
	{
		public readonly string Video;

		public readonly Action Action;

		public bool IsPlaying;

		public VideoScreenPayloadData(string videoType, Action action)
		{
			Video = videoType;
			Action = action;
		}
	}
}
