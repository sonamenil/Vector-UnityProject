using System.IO;

public static class FileUtils
{
	public static void WriteAllText(string path, string contents, bool useEncrypt)
	{
		File.WriteAllText(path, contents);
	}

	public static string ReadAllText(string path, bool useEncrypt)
	{
		return File.ReadAllText(path);
	}
}
