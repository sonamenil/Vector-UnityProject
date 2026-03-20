using System.Collections;

public class GooglePlayPassManager : AbstractManager<GooglePlayPassManager>
{
	private bool _isGooglePlayPass;

	public bool IsGooglePlayPass => false;

	protected override void InitInternal()
	{
	}

	private IEnumerator WaitPaymentsInitialization()
	{
		return null;
	}

	private bool CheckGooglePlayPass()
	{
		return false;
	}
}
