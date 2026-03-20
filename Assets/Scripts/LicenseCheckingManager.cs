using UnityEngine;

public class LicenseCheckingManager : AbstractManager<LicenseCheckingManager>
{
	private LicenseCheckingAbstract _licenseChecking;

	private LicenseCheckResult _result;

	private const string AndroidLicensingKeyPaidVersion = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAj8ZEgV0vaf2gvhIlFRl8gDo8O4qSkSQSRWfydhh47VQp1qV4om2RBlJlab0W5AYp6IBaWTztiQYUhiYrrF/YplfWGj2nF9CUUfTUgmIiJmivxdcCmcmMBSrJ+eYegvaAhsUS/F/XtChEXw7buf8p/dlhQ2RtOLn/haKjVzwIDdtvWBKxMpr9CKPJlaKiPvDB6AWvbuEey1gRsjhX+kMyclP79Bb/J3JlfWdU4pm5geR1CkdCxV/l89ApE3bReCw4WdcVdRkHv31hUnv9xmtMMbMfO3c0idLghOr7w2dwFzSiJ63uYUfUYJ07Q2yyg846WjRyMJjFnvfnzjZv0Igz6QIDAQAB";

	private const string AndroidLicensingKeyFreeVersion = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkFEPylmuO3K1U8lx62R3Wz9s+daNE/ZjFBwVpsblx+A1fDTfOxcH3nC5lPUMbdBVotGG5pCMlGruFOitSkPytpSb41LnOmfLzwU2CwbrNu0C1p5p3We2I3KaVVT7QyxQrhT5LWCXUsVjgxhZsYVr1c2OqAgfBlrIKu2+ej2xkFjXKNjUh3xsH0lhR5824661n5cc0YWj/oejlQlfC6wcTBMpn+aVMur1TfJRhlXZuz99eHXrXTzV99xEktgSBkAsGALOLanxPgsMjaM6q/vmJ7/RELYc0w/ce8bSnY8nR0FW0qOJqPYvwPoxd195dyvY26JuQlQF9HVVwDvx95YSuQIDAQAB";

	public LicenseCheckResult Result => default(LicenseCheckResult);

	protected override void InitInternal()
	{
	}

	public void RunCheck()
	{
	}

	public void Allow()
	{
	}

	public void DontAllow()
	{
	}

	public void ApplicationError(string errorMessage)
	{
	}
}
