using System.Globalization;
using System.Threading;
using UnityEngine;

public static class FixCultureRuntime
{
	[RuntimeInitializeOnLoadMethod]
	private static void FixCulture()
	{
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
    }
}
