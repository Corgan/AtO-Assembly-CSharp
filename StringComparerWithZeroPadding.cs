using System;
using System.Collections.Generic;

public class StringComparerWithZeroPadding : IComparer<string>
{
	public int Compare(string x, string y)
	{
		string[] array = x.Split('_');
		string[] array2 = y.Split('_');
		for (int i = 0; i < Math.Min(array.Length, array2.Length); i++)
		{
			string text = array[i];
			string text2 = array2[i];
			text = FillWithZeros(text, Math.Max(text.Length, text2.Length));
			text2 = FillWithZeros(text2, Math.Max(text.Length, text2.Length));
			int num = text.CompareTo(text2);
			if (num != 0)
			{
				return num;
			}
		}
		return x.Length.CompareTo(y.Length);
	}

	private string FillWithZeros(string s, int maxLength)
	{
		if (s.Length == maxLength)
		{
			return s;
		}
		int num = -1;
		for (int i = 0; i < s.Length; i++)
		{
			if (char.IsDigit(s[i]))
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return s;
		}
		string text = s.Substring(0, num);
		string text2 = s.Substring(num);
		text2 = text2.PadLeft(maxLength - text.Length, '0');
		return text + text2;
	}

	private bool IsNumeric(string value)
	{
		int result;
		return int.TryParse(value, out result);
	}
}
