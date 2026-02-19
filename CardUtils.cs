using System;
using System.Linq;

public static class CardUtils
{
	private static readonly string[] Suffixes = new string[3] { "", "a", "b" };

	public static readonly string[] MasterReweaverKeys = Suffixes.Select((string suffix) => "masterreweaver" + suffix).ToArray();

	public static int GetMaxPlaceholderFormattedStringIndex(string text)
	{
		int num = -1;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] != '{')
			{
				continue;
			}
			int num2 = i + 1;
			int num3 = text.IndexOf('}', num2);
			if (num3 > num2)
			{
				if (int.TryParse(text.Substring(num2, num3 - num2), out var result))
				{
					num = Math.Max(num, result);
				}
				i = num3;
			}
		}
		return num;
	}
}
