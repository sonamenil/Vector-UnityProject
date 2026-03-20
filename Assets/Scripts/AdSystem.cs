using System;

public class AdSystem
{
	public enum Decision
	{
		NONE,
		SHOW_INTERSTITIAL,
		SHOW_REWARD_VIDEO_DIALOG,
		SHOW_UPGRADE_DIALOG,
		SHOW_BOOST_DIALOG
	}

	private static AdSystem _current;

	private bool _lastTimeUserSeeRewardVideo;

	private bool _lastTimeUserBoostCoins;

	private Decision _currentDecision;

	private Action _onDecisionEnd;

	public static AdSystem current => null;

	public Decision currentDecision => default(Decision);

	public bool isDecisionNone => false;

	private AdSystem()
	{
	}

	public void ForceDecisionRewardDialog()
	{
	}

	public void ForceDecisionInterstitial()
	{
	}

	public void MakeDecision()
	{
	}

	private bool MakeDecisionShowInterstitial()
	{
		return false;
	}

	private bool isChance(int chance)
	{
		return false;
	}

	public void ClearDecision()
	{
	}

	public void ExecuteDecision(Action actionOnEnd)
	{
	}

	private void ShowRewardVideoDialog()
	{
	}

	private void ShowRewardVideoDialogResult(bool result)
	{
	}

	private void OnDecisionEnd()
	{
	}

	public void TryShowBoostDialog(Action actionOnEnd, bool force = false)
	{
	}

	private void ShowBoostDialogResult(bool result)
	{
	}

	private void ShowDialogAdError()
	{
	}

	public void TryShowInterstitialOnRun(Action actionOnEnd)
	{
	}

	private void RewardedVideoShown(RewardedVideoHandler.OnVideoRewardFailsReasons result)
	{
	}

	private void InterstitialShown(bool obj)
	{
	}
}
