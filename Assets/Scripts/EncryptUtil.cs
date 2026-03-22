using System;

public class EncryptUtil
{
	private static bool _encryptUserData;

	public static bool EncryptUserData
	{
		get => false;
		set
		{
		}
	}

	private static byte[] Key => null;

	private static byte[] GetUnicByteArray(int length)
	{
		return null;
	}

	public static byte[] Encrypt(byte[] input, int index, int count, byte[] initVector = null)
	{
		return null;
	}

	public static byte[] EncryptWithoutStartIV(byte[] input, int index, int count, byte[] key = null, byte[] initVector = null)
	{
		return null;
	}

	public static byte[] Encrypt(byte[] input, byte[] initVector = null)
	{
		return null;
	}

	public static byte[] Encrypt(string text, byte[] initVector)
	{
		return null;
	}

	public static byte[] Encrypt(string text)
	{
		return null;
	}

	public static ArraySegment<byte> DecryptToBinary(ArraySegment<byte> input, int aesLength = 256, byte[] initVector = null)
	{
		return default(ArraySegment<byte>);
	}

	public static ArraySegment<byte> DecryptToBinary(byte[] input, int aesLength = 256, byte[] initVector = null)
	{
		return default(ArraySegment<byte>);
	}

	public static string Decrypt(byte[] cipherBytes)
	{
		return null;
	}

	public static string Decrypt(byte[] cipherBytes, byte[] key)
	{
		return null;
	}
}
