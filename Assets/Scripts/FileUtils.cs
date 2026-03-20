public static class FileUtils
{
	public static void WriteAllText(string path, string contents, bool useEncrypt)
	{
		System.IO.File.WriteAllText(path, contents);
	}

	public static string ReadAllText(string path, bool useEncrypt)
	{
		return System.IO.File.ReadAllText(path);
	}
}
