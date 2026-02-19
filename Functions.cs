using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Functions
{
	private static string[] serversMaster = new string[24]
	{
		"time-a-g.nist.gov", "time-b-g.nist.gov", "time-c-g.nist.gov", "time-d-g.nist.gov", "time-d-g.nist.gov", "time-e-g.nist.gov", "time-e-g.nist.gov", "time-a-wwv.nist.gov", "time-b-wwv.nist.gov", "time-c-wwv.nist.gov",
		"time-d-wwv.nist.gov", "time-d-wwv.nist.gov", "time-e-wwv.nist.gov", "time-e-wwv.nist.gov", "time-a-b.nist.gov", "time-b-b.nist.gov", "time-c-b.nist.gov", "time-d-b.nist.gov", "time-d-b.nist.gov", "time-e-b.nist.gov",
		"time-e-b.nist.gov", "time.nist.gov", "utcnist.colorado.edu", "utcnist2.colorado.edu"
	};

	private static int serverIndex = 0;

	private static bool serversRandomized = false;

	private static Dictionary<string, string> replacements = new Dictionary<string, string>
	{
		{ "<y>", "<color=#fc0>" },
		{ "</y>", "</color>" },
		{ "<r>", "<color=#FF8181>" },
		{ "</r>", "</color>" },
		{ "<re>", "<color=#8C0800>" },
		{ "</re>", "</color>" },
		{ "<g>", "<color=#41CD41>" },
		{ "</g>", "</color>" },
		{ "<gr>", "<color=#444>" },
		{ "</gr>", "</color>" },
		{ "<bl>", "<color=#1A83D9>" },
		{ "</bl>", "</color>" },
		{ "<w>", "<color=#fff>" },
		{ "</w>", "</color>" },
		{ "<o>", "<color=#f90>" },
		{ "</o>", "</color>" },
		{ "<col>", "<color=#00e4ff>" },
		{ "</col>", "</color>" },
		{ "<lig>", "<color=#ffff11>" },
		{ "</lig>", "</color>" },
		{ "<sha>", "<color=#BF6DFF>" },
		{ "</sha>", "</color>" },
		{ "<min>", "<color=#FF69F6>" },
		{ "</min>", "</color>" },
		{ "<hol>", "<color=#fffbb9>" },
		{ "</hol>", "</color>" },
		{ "<pie>", "<color=#bafcc0>" },
		{ "</pie>", "</color>" },
		{ "<blu>", "<color=#ffe3ca>" },
		{ "</blu>", "</color>" },
		{ "<sla>", "<color=#bad5fc>" },
		{ "</sla>", "</color>" },
		{ "<sy>", "<color=#111111>" },
		{ "</sy>", "</color>" },
		{ "<br><br>", "<br><line-height=50%><br></line-height>" },
		{ "<br1>", "\n" },
		{ "<br2>", "\n" },
		{ "<br3>", "\n" },
		{ "\\n", "\n" }
	};

	public static string CompressString(string text)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
		{
			gZipStream.Write(bytes, 0, bytes.Length);
		}
		memoryStream.Position = 0L;
		byte[] array = new byte[memoryStream.Length];
		memoryStream.Read(array, 0, array.Length);
		byte[] array2 = new byte[array.Length + 4];
		Buffer.BlockCopy(array, 0, array2, 4, array.Length);
		Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, array2, 0, 4);
		return Convert.ToBase64String(array2);
	}

	public static string DecompressString(string compressedText)
	{
		byte[] array = Convert.FromBase64String(compressedText);
		using MemoryStream memoryStream = new MemoryStream();
		int num = BitConverter.ToInt32(array, 0);
		memoryStream.Write(array, 4, array.Length - 4);
		byte[] array2 = new byte[num];
		memoryStream.Position = 0L;
		using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
		{
			gZipStream.Read(array2, 0, array2.Length);
		}
		return Encoding.UTF8.GetString(array2);
	}

	public static string CheckIfSavegameIsCompatible(GameData gameData)
	{
		if (gameData == null)
		{
			return "";
		}
		int num = GameVersionToNumber(gameData.Version);
		foreach (KeyValuePair<string, string> item in Globals.Instance.IncompatibleVersion)
		{
			int num2 = GameVersionToNumber(item.Key);
			if (num < num2)
			{
				return item.Value;
			}
		}
		return "";
	}

	public static int GameVersionToNumber(string _str = "")
	{
		float num = 0f;
		_str = _str.Trim();
		if (_str == "")
		{
			return FuncRoundToInt(num);
		}
		string[] array = _str.Split('.');
		float num2 = 0f;
		if (array.Length != 0)
		{
			num2 = float.Parse(array[0]);
			for (int i = 1; i < array[0].Length; i++)
			{
				num2 *= 0.1f;
			}
			num += num2 * 10000000f;
		}
		if (array.Length > 1)
		{
			num2 = float.Parse(array[1]);
			for (int j = 1; j < array[1].Length; j++)
			{
				num2 *= 0.1f;
			}
			num += num2 * 10000f;
		}
		if (array.Length > 2)
		{
			array[2] = array[2].ToLower().Split(' ')[0];
			array[2] = array[2].Replace("a", "1");
			array[2] = array[2].Replace("b", "2");
			array[2] = array[2].Replace("c", "3");
			array[2] = array[2].Replace("d", "4");
			array[2] = array[2].Replace("e", "5");
			array[2] = array[2].Replace("f", "6");
			array[2] = array[2].Replace("g", "7");
			array[2] = array[2].Replace("h", "8");
			array[2] = array[2].Replace("i", "9");
			try
			{
				num2 = float.Parse(array[2]);
			}
			catch
			{
			}
			for (int k = 1; k < array[2].Length; k++)
			{
				num2 *= 0.1f;
			}
			num += num2 * 100f;
		}
		return FuncRoundToInt(num);
	}

	public static string StripTagsString(string _str)
	{
		return Regex.Replace(_str, "<[^>]*>", "");
	}

	public static TimeSpan TimeDifference(string dateInput)
	{
		DateTime.Parse(dateInput);
		return DateTime.Parse(dateInput) - GameManager.Instance.GetTime();
	}

	public static bool Expired(string dateInput)
	{
		if (dateInput == "")
		{
			return true;
		}
		DateTime dateTime = DateTime.Parse(dateInput);
		if (GameManager.Instance.GetTime() > dateTime)
		{
			return true;
		}
		return false;
	}

	public static string NormalizeTextForArchive(string str)
	{
		str = str.Trim().Replace("\t", "").Replace("\r", "")
			.Replace("\n", "<br>");
		return str;
	}

	public static string[] SplitString(string needle, string haystack)
	{
		return haystack.Split(new string[1] { needle }, StringSplitOptions.None);
	}

	public static string OnlyFirstCharToUpper(this string input)
	{
		if (input != null)
		{
			if (input == "")
			{
				throw new ArgumentException("input cannot be empty", "input");
			}
			return input[0].ToString().ToUpper() + input.Substring(1);
		}
		throw new ArgumentNullException("input");
	}

	public static string UppercaseFirst(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		return char.ToUpper(str[0]) + str.Substring(1).ToLower();
	}

	public static string LowercaseFirst(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		str = str.Trim();
		return char.ToLower(str[0]) + str.Substring(1);
	}

	public static string RemoveLastDoubleDot(string str)
	{
		if (!string.IsNullOrEmpty(str) && str[str.Length - 1] == ':')
		{
			str = str.TrimEnd(':');
		}
		return str;
	}

	public static string Substring(string str, int num)
	{
		if (str.Length > num)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(str.Substring(0, num));
			stringBuilder.Append(".");
			return stringBuilder.ToString();
		}
		return str;
	}

	public static int FuncRoundToInt(float num)
	{
		if (!(num % 1f >= 0.5f))
		{
			return Mathf.FloorToInt(num);
		}
		return Mathf.CeilToInt(num);
	}

	public static string Base64Encode(string plainText)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
	}

	public static string Base64Decode(string base64EncodedData)
	{
		byte[] bytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(bytes);
	}

	public static string FloatToTime(float theTime)
	{
		float num = Mathf.Floor(theTime / 3600f);
		float num2 = Mathf.Floor(theTime - num * 3600f);
		float num3 = Mathf.Floor(num2 / 60f);
		float num4 = Mathf.Floor(num2 - num3 * 60f);
		StringBuilder stringBuilder = new StringBuilder();
		if (num < 10f)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num);
		stringBuilder.Append(":");
		if (num3 < 10f)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num3);
		stringBuilder.Append(":");
		if (num4 < 10f)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num4);
		return stringBuilder.ToString();
	}

	public static int GetDeterministicHashCode(this string str)
	{
		int num = 352654597;
		int num2 = num;
		for (int i = 0; i < str.Length; i += 2)
		{
			num = ((num << 5) + num) ^ str[i];
			if (i == str.Length - 1)
			{
				break;
			}
			num2 = ((num2 << 5) + num2) ^ str[i + 1];
		}
		return num + num2 * 1566083941;
	}

	public static string RandomString(float minCharAmount, float maxCharAmount = 0f)
	{
		float num = 0f;
		num = ((maxCharAmount != 0f) ? UnityEngine.Random.Range(minCharAmount, maxCharAmount) : minCharAmount);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; (float)i < num; i++)
		{
			stringBuilder.Append("abcdefghkmnpqrstuvwxyz123456789"[UnityEngine.Random.Range(0, "abcdefghkmnpqrstuvwxyz123456789".Length)]);
		}
		return stringBuilder.ToString();
	}

	public static string RandomStringSafe(float minCharAmount, float maxCharAmount = 0f)
	{
		float num = 0f;
		num = ((maxCharAmount != 0f) ? UnityEngine.Random.Range(minCharAmount, maxCharAmount) : minCharAmount);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; (float)i < num; i++)
		{
			stringBuilder.Append("abcdefghkmnpqrstwxyz2345689"[UnityEngine.Random.Range(0, "abcdefghkmnpqrstwxyz2345689".Length)]);
		}
		return stringBuilder.ToString();
	}

	public static T[] ShuffleArray<T>(T[] arr)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int num = arr.Length - 1; num > 0; num--)
			{
				int num2 = UnityEngine.Random.Range(0, num);
				T val = arr[num];
				arr[num] = arr[num2];
				arr[num2] = val;
			}
		}
		return arr;
	}

	public static List<T> ShuffleList<T>(this List<T> ts)
	{
		int count = ts.Count;
		int num = count - 1;
		List<T> list = new List<T>();
		for (int i = 0; i < count; i++)
		{
			list.Add(ts[i]);
		}
		for (int j = 0; j < 2; j++)
		{
			for (int k = 0; k < num; k++)
			{
				int num2 = 0;
				num2 = ((!(MatchManager.Instance != null)) ? UnityEngine.Random.Range(k, count) : ((MatchManager.Instance.EventList == null || !MatchManager.Instance.EventList.Contains("ResetDeck")) ? MatchManager.Instance.GetRandomIntRange(k, count) : MatchManager.Instance.GetRandomIntRange(k, count, "shuffle")));
				T value = list[k];
				list[k] = list[num2];
				list[num2] = value;
			}
		}
		return list;
	}

	public static string ClassIconFromString(string _class)
	{
		_class = _class.ToLower().Replace(" ", "");
		return _class switch
		{
			"warrior" => "block", 
			"scout" => "piercing", 
			"mage" => "fire", 
			_ => "regeneration", 
		};
	}

	public static float DecimalPositions(float number, int positions)
	{
		if (positions > 0)
		{
			return Mathf.Floor(number * Mathf.Pow(10f, positions)) / Mathf.Pow(10f, positions);
		}
		return Mathf.Floor(number);
	}

	public static string Log(params object[] data)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < data.Length; i++)
		{
			stringBuilder.Append(data[i].ToString());
		}
		return stringBuilder.ToString();
	}

	public static long ASCIIWordSum(string str, long[] sumArr)
	{
		int length = str.Length;
		int num = 0;
		long num2 = 0L;
		long num3 = 0L;
		for (int i = 0; i < length; i++)
		{
			if (str[i] == ' ')
			{
				num3 += num2;
				sumArr[num++] = num2;
				num2 = 0L;
			}
			else
			{
				num2 += str[i];
			}
		}
		sumArr[num] = num2;
		return num3 + num2;
	}

	public static string ThermometerTextForRewards(ThermometerData _thermometerData)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("performanceBonus"));
		stringBuilder.Append(" [");
		stringBuilder.Append("<color=#");
		stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(_thermometerData.ThermometerColor));
		stringBuilder.Append(">");
		stringBuilder.Append(Texts.Instance.GetText(_thermometerData.ThermometerId).ToUpper());
		stringBuilder.Append("</color>");
		stringBuilder.Append("]");
		stringBuilder.Append(":\n");
		bool flag = false;
		flag = true;
		stringBuilder.Append("<sprite name=gold> ");
		if (_thermometerData.UiGold > -1)
		{
			stringBuilder.Append("+");
		}
		stringBuilder.Append(_thermometerData.UiGold);
		stringBuilder.Append("%    ");
		flag = true;
		stringBuilder.Append("<sprite name=experience> ");
		if (_thermometerData.UiExp > -1)
		{
			stringBuilder.Append("+");
		}
		stringBuilder.Append(_thermometerData.UiExp);
		stringBuilder.Append("%    ");
		if (_thermometerData.UiCard != 0)
		{
			flag = true;
			stringBuilder.Append("<sprite name=cards>");
			if (_thermometerData.UiCard > 0)
			{
				stringBuilder.Append("+");
			}
			stringBuilder.Append(_thermometerData.UiCard);
			stringBuilder.Append(" ");
			stringBuilder.Append(Texts.Instance.GetText("cardsTier"));
		}
		if (flag)
		{
			return stringBuilder.ToString();
		}
		return "";
	}

	public static string ThermometerTextForPopup(ThermometerData _thermometerData)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+2><color=#");
		stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(_thermometerData.ThermometerColor));
		stringBuilder.Append("><b>");
		stringBuilder.Append(Texts.Instance.GetText(_thermometerData.ThermometerId).ToUpper());
		stringBuilder.Append("</b></color></size>\n");
		if (AtOManager.Instance.currentMapNode == "voidhigh_13" || AtOManager.Instance.currentMapNode == "of1_10" || AtOManager.Instance.currentMapNode == "of2_10")
		{
			return stringBuilder.ToString();
		}
		stringBuilder.Append("<color=#FFFFFF>");
		stringBuilder.Append(Texts.Instance.GetText("performanceBonus"));
		stringBuilder.Append("</color>");
		stringBuilder.Append("\n<line-height=110%><size=+3>");
		stringBuilder.Append("<sprite name=gold> ");
		if (_thermometerData.UiGold > -1)
		{
			stringBuilder.Append("+");
		}
		stringBuilder.Append(_thermometerData.UiGold);
		stringBuilder.Append("%");
		stringBuilder.Append("     <sprite name=experience> ");
		if (_thermometerData.UiExp > -1)
		{
			stringBuilder.Append("+");
		}
		stringBuilder.Append(_thermometerData.UiExp);
		stringBuilder.Append("%");
		if (_thermometerData.UiCard != 0)
		{
			stringBuilder.Append("\n");
			stringBuilder.Append("<sprite name=cards> ");
			if (_thermometerData.UiCard > 0)
			{
				stringBuilder.Append("+");
			}
			stringBuilder.Append(_thermometerData.UiCard);
			stringBuilder.Append(" ");
			stringBuilder.Append(Texts.Instance.GetText("cardsTier"));
		}
		return stringBuilder.ToString();
	}

	public static int GetMadnessScoreMultiplier(int level, bool adventureMode = true)
	{
		if (adventureMode)
		{
			if (level == 1)
			{
				return 50;
			}
			if (level == 2)
			{
				return 75;
			}
			if (level == 3)
			{
				return 100;
			}
			if (level == 4)
			{
				return 125;
			}
			if (level == 5)
			{
				return 150;
			}
			if (level == 6)
			{
				return 175;
			}
			if (level == 7)
			{
				return 200;
			}
			if (level == 8)
			{
				return 225;
			}
			if (level == 9)
			{
				return 250;
			}
			if (level == 10)
			{
				return 275;
			}
			if (level == 11)
			{
				return 300;
			}
			if (level == 12)
			{
				return 325;
			}
			if (level == 13)
			{
				return 350;
			}
			if (level == 14)
			{
				return 400;
			}
			if (level == 15)
			{
				return 600;
			}
			if (level == 16)
			{
				return 800;
			}
			if (level == 17)
			{
				return 1000;
			}
			if (level >= 18)
			{
				return 1200;
			}
		}
		else
		{
			switch (level)
			{
			case 1:
				return 25;
			case 2:
				return 50;
			case 3:
				return 75;
			case 4:
				return 100;
			case 5:
				return 125;
			case 6:
				return 150;
			case 7:
				return 175;
			case 8:
				return 250;
			case 9:
				return 400;
			case 10:
				return 550;
			}
		}
		return 0;
	}

	public static string GetMadnessBonusText(int value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (value == 0)
		{
			stringBuilder.Append(Texts.Instance.GetText("madnessBasicInfo"));
			stringBuilder.Append("<br>");
		}
		else
		{
			stringBuilder.Append(Texts.Instance.GetText("madnessBasic"));
			stringBuilder.Append("<br>");
		}
		switch (value)
		{
		case 2:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+0% / +0% / +10% / +15%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>0 / 0 / +1 / +1</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-1 / -1 / -1 / -1</color> ", Texts.Instance.GetText("madnessPerAct")));
			break;
		case 3:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+5% / +10% / +15% / +20%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>+1 / +1 / +1 / +1</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-1 / -1 / -2 / -2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageDone"), " <color=#E5A44E>+1 / +1 / +1 / +1</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessSeriousInjuries"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNoChest"), "800"));
			break;
		case 4:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+15% / +25% / +30% / +35%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>+1 / +1 / +2 / +2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-1 / -2 / -2 / -2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageDone"), " <color=#E5A44E>+1 / +1 / +2 / +2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessSeriousInjuries"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNoChest"), "700"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessHeroEnergy"), " <color=#E5A44E>-1</color> "));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessReroll"));
			break;
		case 5:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+25% / +35% / +40% / +45%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>+2 / +2 / +2 / +2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-2 / -2 / -2 / -2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageDone"), " <color=#E5A44E>+1 / +2 / +2 / +2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessDeadlyInjuries"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNoChest"), "600"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessHeroEnergy"), " <color=#E5A44E>-1</color> "));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessReroll"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessHarderEvents"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessInfrequent"));
			break;
		case 6:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+30% / +45% / +50% / +55%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>+2 / +2 / +2 / +3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-2 / -2 / -2 / -3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageDone"), " <color=#E5A44E>+2 / +2 / +2 / +2</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessDeadlyInjuries"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNoChest"), "500"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessNoSupplyExchange"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessHeroEnergy"), " <color=#E5A44E>-1</color> "));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessReroll"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessHarderEvents"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessInfrequent"));
			break;
		case 7:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+35% / +50% / +55% / +60%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>+2 / +2 / +3 / +3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-2 / -2 / -3 / -3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageDone"), " <color=#E5A44E>+2 / +2 / +2 / +3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessDeadlyInjuries"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNoChest"), "400"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessNoSupplyExchange"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessHeroEnergy"), " <color=#E5A44E>-2</color> "));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessReroll"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessHarderEvents"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessInfrequent"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessRandomSeed"));
			break;
		case 8:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+40% / +55% / +60% / +65%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>+2 / +3 / +3 / +3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-2 / -3 / -3 / -3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageDone"), " <color=#E5A44E>+2 / +2 / +3 / +3</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessDeadlyInjuries"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNoChest"), "300"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessNoSupplyExchange"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessHeroEnergy"), " <color=#E5A44E>-2</color> "));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessReroll"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessHarderEvents"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessInfrequent"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessRandomSeed"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessDisabledCards"));
			break;
		case 9:
			stringBuilder.Append(Texts.Instance.GetText("madnessExhaust"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterHp"), " <color=#E5A44E>+45% / +60% / +65% / +70%</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterSpeed"), " <color=#E5A44E>+2 / +3 / +3 / +4</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageTaken"), " <color=#E5A44E>-2 / -3 / -3 / -4</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessMonsterDamageDone"), " <color=#E5A44E>+2 / +3 / +3 / +4</color> ", Texts.Instance.GetText("madnessPerAct")));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessDeadlyInjuries"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNoChest"), "250"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessNoSupplyExchange"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessHeroEnergy"), " <color=#E5A44E>-2</color> "));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessReroll"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessHarderEvents"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessInfrequent"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessRandomSeed"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessDisabledCards"));
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("madnessPermanentInjuries"));
			break;
		}
		string text = PregReplaceIcon(stringBuilder.ToString());
		if (SpaceBeforePercentSign())
		{
			text = Regex.Replace(text, "(?<!\\s)%", " %");
			text = Regex.Replace(text, " \\/ ", "<color=#AAA><size=-.3><voffset=.2> / </voffset></size></color>");
		}
		return text;
	}

	public static NPCData[] GetRandomCombat(Enums.CombatTier combatTier, int seed, string nodeSelectedId, bool forceIsThereRare = false)
	{
		NPCData[] array = new NPCData[4];
		UnityEngine.Random.InitState(seed);
		bool flag = false;
		bool flag2 = false;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			if (AtOManager.Instance.NodeHaveBossRare(nodeSelectedId) || forceIsThereRare)
			{
				flag = true;
			}
		}
		else if (MadnessManager.Instance.IsMadnessTraitActive("randomcombats") || AtOManager.Instance.IsChallengeTraitActive("randomcombats"))
		{
			flag = true;
		}
		NPCData nPCData = null;
		if (flag)
		{
			bool flag3 = false;
			int num = 0;
			while (!flag3 && num < 200)
			{
				nPCData = Globals.Instance.GetNPCForRandom(_rare: true, 0, combatTier, array);
				if (nPCData != null)
				{
					flag3 = true;
				}
				num++;
			}
			if (nPCData != null)
			{
				if (nPCData.PreferredPosition == Enums.CardTargetPosition.Front)
				{
					array[0] = nPCData;
					flag2 = true;
				}
				else if (nPCData.BigModel)
				{
					array[2] = nPCData;
				}
				else
				{
					array[3] = nPCData;
				}
			}
			if (AtOManager.Instance.Sandbox_doubleChampions)
			{
				flag3 = false;
				num = 0;
				while (!flag3 && num < 200)
				{
					nPCData = Globals.Instance.GetNPCForRandom(_rare: true, 0, combatTier, array);
					if (nPCData != null)
					{
						if (flag2 && nPCData.PreferredPosition != Enums.CardTargetPosition.Front)
						{
							flag3 = true;
						}
						else if (!flag2 && nPCData.PreferredPosition == Enums.CardTargetPosition.Front)
						{
							flag3 = true;
						}
					}
					num++;
				}
				if (nPCData != null)
				{
					if (nPCData.PreferredPosition == Enums.CardTargetPosition.Front)
					{
						array[0] = nPCData;
						flag2 = true;
					}
					else if (nPCData.BigModel)
					{
						array[2] = nPCData;
					}
					else
					{
						array[3] = nPCData;
					}
				}
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if ((nPCData != null && nPCData.IsBoss && ((!flag2 && i > 1) || (flag2 && i == 1))) || !(array[i] == null))
			{
				continue;
			}
			if (i == 0 || i == 1)
			{
				if (nPCData != null && nPCData.IsBoss && !flag2 && i == 1)
				{
					array[i] = Globals.Instance.GetNPCForRandom(_rare: false, -1, combatTier, array);
				}
				else
				{
					array[i] = Globals.Instance.GetNPCForRandom(_rare: false, 1, combatTier, array);
				}
			}
			else
			{
				array[i] = Globals.Instance.GetNPCForRandom(_rare: false, -1, combatTier, array);
			}
		}
		return array;
	}

	public static string ScoreFormat(int _score)
	{
		return $"{_score:#,0}";
	}

	public static string PregReplaceIcon(string str)
	{
		return Regex.Replace(str, "\\<i=(\\w+)\\>", "<size=+.5><voffset=-.3><sprite name=$1></voffset></size>");
	}

	public static string GetClassFromCards(List<string> _cards)
	{
		if (_cards.Contains("captainshowlstart"))
		{
			return "mercenary";
		}
		if (_cards.Contains("heatlaserstart"))
		{
			return "sentinel";
		}
		if (_cards.Contains("vigorousfurystart"))
		{
			return "berserker";
		}
		if (_cards.Contains("experttrackerstart"))
		{
			return "ranger";
		}
		if (_cards.Contains("killerinstincstart"))
		{
			return "assassin";
		}
		if (_cards.Contains("coldsparkstart"))
		{
			return "elementalist";
		}
		if (_cards.Contains("fieryshieldstart"))
		{
			return "pyromancer";
		}
		if (_cards.Contains("divinegracestart"))
		{
			return "cleric";
		}
		if (_cards.Contains("atonementstart"))
		{
			return "priest";
		}
		if (_cards.Contains("maledictionstart"))
		{
			return "voodoowitch";
		}
		return "";
	}

	public static void DebugLogGD(string str, string type = "")
	{
		if (!GameManager.Instance.GetDeveloperMode() || !Globals.Instance.ShowDebug)
		{
			return;
		}
		bool flag = false;
		if (type switch
		{
			"error" => true, 
			"cache" => false, 
			"gamebusy" => false, 
			"gamestatus" => false, 
			"general" => false, 
			"item" => false, 
			"map" => false, 
			"net" => false, 
			"ocmap" => false, 
			"randomindex" => false, 
			"reload" => false, 
			"save" => false, 
			"synccode" => false, 
			"trace" => true, 
			_ => false, 
		})
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (type != "")
			{
				stringBuilder.Append("[");
				stringBuilder.Append(type.ToUpper());
				stringBuilder.Append("] ");
			}
			stringBuilder.Append(str);
			if (type != "cache")
			{
				Debug.Log(stringBuilder.ToString());
			}
			else
			{
				Debug.LogWarning(stringBuilder.ToString());
			}
			stringBuilder = null;
		}
	}

	public static string FormatString(string text)
	{
		StringBuilder stringBuilder = new StringBuilder(text);
		foreach (KeyValuePair<string, string> replacement in replacements)
		{
			stringBuilder.Replace(replacement.Key, replacement.Value);
		}
		return stringBuilder.ToString();
	}

	public static string FormatStringCard(string text)
	{
		string input = new StringBuilder(text).Replace("<gl>", "<color=#5E3016>").Replace("</gl>", "</color>").Replace("<bl>", "<color=#263ABC>")
			.Replace("</bl>", "</color>")
			.Replace("<rd>", "<color=#720070>")
			.Replace("</rd>", "</color>")
			.Replace("<act>", "<size=-.15><color=#444>")
			.Replace("</act>", "</size></color>")
			.ToString();
		input = Regex.Replace(input, "\\<i=(\\w+)\\>", "<size=+.1><voffset=-.1><space=.2> <sprite name=$1></voffset></size>");
		string pattern = "<crd=(.*?),(.*?)>";
		foreach (Match item in Regex.Matches(input, pattern))
		{
			CardData cardData = Globals.Instance.GetCardData(item.Groups[1].ToString(), instantiate: false);
			if (cardData != null)
			{
				string text2 = item.Groups[2].ToString();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<color=#5E3016>");
				if (int.Parse(text2) > 1)
				{
					stringBuilder.Append(text2);
					stringBuilder.Append(" ");
				}
				stringBuilder.Append("  <sprite name=cards>");
				stringBuilder.Append(cardData.CardName);
				stringBuilder.Append("</color>");
				input = input.Replace(item.ToString(), stringBuilder.ToString());
				stringBuilder = null;
			}
		}
		return input;
	}

	public static string StripTags(string input)
	{
		return Regex.Replace(input, "<.*?>", string.Empty);
	}

	public static string OnlyAscii(string str)
	{
		if (str != "")
		{
			return Regex.Replace(str, "[^\\u0000-\\u007F]+", string.Empty);
		}
		return "";
	}

	public static string Sanitize(string str, bool doLowerCase = true)
	{
		if (str != "")
		{
			string text = Regex.Replace(str, "\\s+", "");
			if (text != "")
			{
				if (doLowerCase)
				{
					return text.ToLower().Trim().Replace("'", "");
				}
				return text.Trim().Replace("'", "");
			}
		}
		return "";
	}

	public static Vector3 GetCardPosition(Vector3 posZero, int index, int total)
	{
		Vector3 vector = posZero;
		float y = posZero.y;
		float num = 2.6f;
		float num2 = 3.2f;
		float num3 = posZero.y + num2 * 0.5f;
		float num4 = posZero.y - num2 * 0.5f;
		int num5 = 0;
		if (total > 10)
		{
			num5 = Mathf.FloorToInt((float)index / 10f);
			total = 10;
			index %= 10;
		}
		num4 -= (float)num5 * num2 * 2f;
		num3 -= (float)num5 * num2 * 2f;
		switch (total)
		{
		case 2:
			vector = ((index != 0) ? new Vector3(posZero.x + num * 0.5f, y) : new Vector3(posZero.x - num * 0.5f, y));
			break;
		case 3:
			vector = index switch
			{
				0 => new Vector3(posZero.x - num, y), 
				1 => posZero, 
				_ => new Vector3(posZero.x + num, y), 
			};
			break;
		case 4:
			vector = index switch
			{
				0 => new Vector3(posZero.x - num * 0.5f, num3), 
				1 => new Vector3(posZero.x + num * 0.5f, num3), 
				2 => new Vector3(posZero.x - num * 0.5f, num4), 
				_ => new Vector3(posZero.x + num * 0.5f, num4), 
			};
			break;
		case 5:
			vector = index switch
			{
				0 => new Vector3(posZero.x - num, num3), 
				1 => new Vector3(posZero.x, num3), 
				2 => new Vector3(posZero.x + num, num3), 
				3 => new Vector3(posZero.x - num * 0.5f, num4), 
				_ => new Vector3(posZero.x + num * 0.5f, num4), 
			};
			break;
		case 6:
			vector = index switch
			{
				0 => new Vector3(posZero.x - 2.2f, num3), 
				1 => new Vector3(posZero.x, num3), 
				2 => new Vector3(posZero.x + 2.2f, num3), 
				3 => new Vector3(posZero.x - 2.2f, num4), 
				4 => new Vector3(posZero.x, num4), 
				_ => new Vector3(posZero.x + 2.2f, num4), 
			};
			break;
		case 7:
			vector = index switch
			{
				0 => new Vector3(posZero.x - (num * 0.5f + num), num3), 
				1 => new Vector3(posZero.x - num * 0.5f, num3), 
				2 => new Vector3(posZero.x + num * 0.5f, num3), 
				3 => new Vector3(posZero.x + num * 0.5f + num, num3), 
				4 => new Vector3(posZero.x - num, num4), 
				5 => new Vector3(posZero.x, num4), 
				_ => new Vector3(posZero.x + num, num4), 
			};
			break;
		case 8:
			vector = index switch
			{
				0 => new Vector3(posZero.x - (num * 0.5f + num), num3), 
				1 => new Vector3(posZero.x - num * 0.5f, num3), 
				2 => new Vector3(posZero.x + num * 0.5f, num3), 
				3 => new Vector3(posZero.x + num * 0.5f + num, num3), 
				4 => new Vector3(posZero.x - (num * 0.5f + num), num4), 
				5 => new Vector3(posZero.x - num * 0.5f, num4), 
				6 => new Vector3(posZero.x + num * 0.5f, num4), 
				_ => new Vector3(posZero.x + num * 0.5f + num, num4), 
			};
			break;
		case 9:
			vector = index switch
			{
				0 => new Vector3(posZero.x - num * 2f, num3), 
				1 => new Vector3(posZero.x - num, num3), 
				2 => new Vector3(posZero.x, num3), 
				3 => new Vector3(posZero.x + num, num3), 
				4 => new Vector3(posZero.x + num * 2f, num3), 
				5 => new Vector3(posZero.x - (num * 0.5f + num), num4), 
				6 => new Vector3(posZero.x - num * 0.5f, num4), 
				7 => new Vector3(posZero.x + num * 0.5f, num4), 
				_ => new Vector3(posZero.x + num * 0.5f + num, num4), 
			};
			break;
		case 10:
			vector = index switch
			{
				0 => new Vector3(posZero.x - num * 2f, num3), 
				1 => new Vector3(posZero.x - num, num3), 
				2 => new Vector3(posZero.x, num3), 
				3 => new Vector3(posZero.x + num, num3), 
				4 => new Vector3(posZero.x + num * 2f, num3), 
				5 => new Vector3(posZero.x - num * 2f, num4), 
				6 => new Vector3(posZero.x - num, num4), 
				7 => new Vector3(posZero.x, num4), 
				8 => new Vector3(posZero.x + num, num4), 
				_ => new Vector3(posZero.x + num * 2f, num4), 
			};
			break;
		}
		return new Vector3(vector.x, vector.y, -3f + (float)index * -0.01f);
	}

	public static MonoBehaviour GetSpriteSkinComponent(this GameObject gameObject)
	{
		MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in components)
		{
			if (monoBehaviour.GetType().FullName == "UnityEngine.Experimental.U2D.Animation.SpriteSkin")
			{
				return monoBehaviour;
			}
		}
		return null;
	}

	public static string GetRandomCardIdByTypeAndRandomRarity(Enums.CardType _cardType)
	{
		int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, Globals.Instance.CardListByType[Enums.CardType.Small_Weapon].Count);
		string id = Globals.Instance.CardListByType[Enums.CardType.Small_Weapon][randomIntRange];
		CardData cardData = Globals.Instance.GetCardData(id, instantiate: false);
		return GetCardByRarity(MatchManager.Instance.GetRandomIntRange(0, 100), cardData);
	}

	public static List<string> GetIdListVarsFromCard(string _card)
	{
		CardData cardData = Globals.Instance.GetCardData(_card, instantiate: false);
		if (cardData != null)
		{
			List<string> list = new List<string>();
			if (cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				list.Add(_card.ToLower());
			}
			else
			{
				cardData = Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false);
				if (cardData != null)
				{
					list.Add(cardData.Id.ToLower());
				}
			}
			if (cardData != null && list.Count == 1)
			{
				list.Add(cardData.UpgradesTo1.ToLower());
				list.Add(cardData.UpgradesTo2.ToLower());
				if (cardData.UpgradesToRare != null)
				{
					list.Add(cardData.UpgradesToRare.Id.ToLower());
				}
				return list;
			}
		}
		return null;
	}

	public static CardData GetCardDataFromCardData(CardData _cardData, string type)
	{
		if (_cardData == null)
		{
			return null;
		}
		CardData cardData = _cardData;
		if (type != "")
		{
			type = type.ToUpper();
		}
		switch (type)
		{
		case "A":
			cardData = ((cardData.CardUpgraded != Enums.CardUpgraded.No) ? Globals.Instance.GetCardData(_cardData.UpgradedFrom + "A", instantiate: false) : Globals.Instance.GetCardData(_cardData.UpgradesTo1, instantiate: false));
			break;
		case "B":
			cardData = ((cardData.CardUpgraded != Enums.CardUpgraded.No) ? Globals.Instance.GetCardData(_cardData.UpgradedFrom + "B", instantiate: false) : Globals.Instance.GetCardData(_cardData.UpgradesTo2, instantiate: false));
			break;
		case "RARE":
			if (cardData.CardUpgraded != Enums.CardUpgraded.No)
			{
				cardData = Globals.Instance.GetCardData(_cardData.UpgradedFrom, instantiate: false);
			}
			if (cardData.UpgradesToRare != null)
			{
				cardData = cardData.UpgradesToRare;
				break;
			}
			return null;
		case "":
			if (cardData.CardUpgraded != Enums.CardUpgraded.No)
			{
				cardData = Globals.Instance.GetCardData(_cardData.UpgradedFrom, instantiate: false);
			}
			break;
		}
		return cardData;
	}

	public static bool ListHaveThisBaseCard(string cardId, List<string> cardList)
	{
		CardData cardDataFromCardData = GetCardDataFromCardData(Globals.Instance.GetCardData(cardId.ToLower(), instantiate: false), "");
		for (int i = 0; i < cardList.Count; i++)
		{
			if (GetCardDataFromCardData(Globals.Instance.GetCardData(cardList[i], instantiate: false), "").Id == cardDataFromCardData.Id)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CardIsPercentRarity(int rarity, CardData _cardData)
	{
		if (rarity < 15 && _cardData.CardRarity == Enums.CardRarity.Common)
		{
			return true;
		}
		if (rarity >= 15 && rarity < 45 && _cardData.CardRarity == Enums.CardRarity.Uncommon)
		{
			return true;
		}
		if (rarity >= 45 && rarity < 80 && _cardData.CardRarity == Enums.CardRarity.Rare)
		{
			return true;
		}
		if (rarity >= 80 && rarity < 95 && _cardData.CardRarity == Enums.CardRarity.Epic)
		{
			return true;
		}
		if (rarity >= 95 && rarity < 100 && _cardData.CardRarity == Enums.CardRarity.Mythic)
		{
			return true;
		}
		return false;
	}

	public static string GetCardByRarity(int rarity, CardData _cardData, bool isChallenge = false)
	{
		int num = 78;
		int num2 = 88;
		int num3 = 98;
		int num4 = 0;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			if (GameManager.Instance.IsSingularity())
			{
				num4 = AtOManager.Instance.GetSingularityMadness();
				num = 26;
				num2 = 58;
				num3 = 90;
			}
			else
			{
				num4 = AtOManager.Instance.GetMadnessDifficulty();
			}
			if (num4 > 0)
			{
				num -= 3;
				num2 -= 3;
				num3 -= 3;
			}
		}
		else
		{
			num4 = AtOManager.Instance.GetObeliskMadness();
		}
		if (num4 > 0)
		{
			num -= num4;
			num2 -= FuncRoundToInt((float)num4 * 0.5f);
		}
		if (rarity < num)
		{
			if (_cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				return _cardData.Id.ToLower();
			}
			if (_cardData.UpgradedFrom != "")
			{
				return _cardData.UpgradedFrom.ToLower();
			}
			return _cardData.Id.ToLower();
		}
		if (rarity >= num && rarity < num2)
		{
			if (_cardData.CardUpgraded == Enums.CardUpgraded.A)
			{
				return _cardData.Id.ToLower();
			}
			if (_cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				return _cardData.UpgradesTo1.ToLower();
			}
			if (_cardData.CardUpgraded == Enums.CardUpgraded.B)
			{
				return (_cardData.UpgradedFrom + "A").ToLower();
			}
		}
		else
		{
			if (rarity >= num2 && rarity < num3)
			{
				if (_cardData.CardUpgraded == Enums.CardUpgraded.B)
				{
					return _cardData.Id.ToLower();
				}
				if (_cardData.CardUpgraded == Enums.CardUpgraded.No)
				{
					return _cardData.UpgradesTo2.ToLower();
				}
				return (_cardData.UpgradedFrom + "B").ToLower();
			}
			if (_cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				if (_cardData.UpgradesToRare != null)
				{
					return _cardData.UpgradesToRare.Id.ToLower();
				}
				return _cardData.Id.ToLower();
			}
			CardData cardData = Globals.Instance.GetCardData(_cardData.UpgradedFrom, instantiate: false);
			if (cardData != null)
			{
				if (cardData.UpgradesToRare != null)
				{
					return cardData.UpgradesToRare.Id.ToLower();
				}
				return _cardData.Id.ToLower();
			}
		}
		return _cardData.Id.ToLower();
	}

	public static string GetTimestamp()
	{
		return GameManager.Instance.GetTime().ToString("yyyyMMddHHmmss");
	}

	public static string GetTimestampString()
	{
		return DateTime.Now.ToString("HH:mm:ss");
	}

	public static string Md5Sum(string strToEncrypt)
	{
		byte[] bytes = new UTF8Encoding().GetBytes(strToEncrypt);
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(bytes);
		string text = "";
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	public static Color HexToColor(string hex)
	{
		hex = hex.Replace("0x", "");
		hex = hex.Replace("#", "");
		byte a = byte.MaxValue;
		byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		if (hex.Length == 8)
		{
			a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
		}
		return new Color32(r, g, b, a);
	}

	public static IEnumerator FollowArc(Transform mover, Vector2 start, Vector2 end, float radius, float duration)
	{
		Vector2 vector = end - start;
		float magnitude = vector.magnitude;
		float absRadius = Mathf.Abs(radius);
		if (magnitude > 2f * absRadius)
		{
			float num;
			absRadius = (num = magnitude / 2f);
			radius = num;
		}
		Vector2 vector2 = new Vector2(vector.y, 0f - vector.x) / magnitude;
		vector2 *= Mathf.Sign(radius) * Mathf.Sqrt(radius * radius - magnitude * magnitude / 4f);
		Vector2 center = start + vector / 2f + vector2;
		Vector2 vector3 = start - center;
		float startAngle = Mathf.Atan2(vector3.y, vector3.x);
		Vector2 vector4 = end - center;
		float num2 = Mathf.Atan2(vector4.y, vector4.x);
		float travel = (num2 - startAngle + MathF.PI * 5f) % (MathF.PI * 2f) - MathF.PI;
		float progress = 0f;
		do
		{
			float f = startAngle + progress * travel;
			mover.position = center + new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * absRadius;
			progress += Time.deltaTime / duration;
			yield return null;
		}
		while (progress < 1f);
		mover.position = end;
	}

	public static int GetCurrentWeeklyWeek()
	{
		if (AtOManager.Instance.weeklyForcedId != "")
		{
			return int.Parse(AtOManager.Instance.weeklyForcedId.Replace("week", ""));
		}
		DateTime time = GameManager.Instance.GetTime();
		foreach (KeyValuePair<string, ChallengeData> item in Globals.Instance.WeeklyDataSource)
		{
			if (item.Value.GetDateFrom() < time && item.Value.GetDateTo() > time)
			{
				return item.Value.Week;
			}
		}
		if (time < Globals.Instance.InitialWeeklyDate)
		{
			return 1;
		}
		return (int)((time - Globals.Instance.InitialWeeklyDate).TotalDays / 7.0) + 1;
	}

	public static DateTime LastDateForWeek()
	{
		DateTime dateTo = Globals.Instance.GetWeeklyData(GetCurrentWeeklyWeek()).GetDateTo();
		DateTime time = GameManager.Instance.GetTime();
		if (dateTo > time)
		{
			return dateTo;
		}
		return Globals.Instance.InitialWeeklyDate.AddDays(GetCurrentWeeklyWeek() * 7);
	}

	public static TimeSpan TimeSpanLeftWeekly()
	{
		DateTime time = GameManager.Instance.GetTime();
		return LastDateForWeek() - time;
	}

	public static DateTime GetDummyDate()
	{
		return new DateTime(1000, 1, 1);
	}

	public static DateTime GetServerTime()
	{
		Debug.Log("GetServerTime <----------------");
		new System.Random(DateTime.Now.Millisecond);
		DateTime result = GetDummyDate();
		string empty = string.Empty;
		if (!serversRandomized)
		{
			ShuffleArray(serversMaster);
		}
		for (int i = 0; i < 5; i++)
		{
			try
			{
				string hostname = serversMaster[serverIndex];
				serverIndex++;
				if (serverIndex >= serversMaster.Length)
				{
					serverIndex = 0;
				}
				TcpClient tcpClient = new TcpClient();
				tcpClient.Connect(hostname, 13);
				if (!tcpClient.Connected)
				{
					Debug.Log("Socket not connected");
					continue;
				}
				StreamReader streamReader = new StreamReader(tcpClient.GetStream());
				empty = streamReader.ReadToEnd();
				streamReader.Close();
				if (empty.Length > 47 && empty.Substring(38, 9).Equals("UTC(NIST)"))
				{
					int num = int.Parse(empty.Substring(1, 5));
					int num2 = int.Parse(empty.Substring(7, 2));
					int month = int.Parse(empty.Substring(10, 2));
					int day = int.Parse(empty.Substring(13, 2));
					int hour = int.Parse(empty.Substring(16, 2));
					int minute = int.Parse(empty.Substring(19, 2));
					int second = int.Parse(empty.Substring(22, 2));
					num2 = ((num <= 51544) ? (num2 + 1999) : (num2 + 2000));
					result = new DateTime(num2, month, day, hour, minute, second);
					break;
				}
			}
			catch (Exception ex)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogWarning("AddCharges exception-> " + ex);
				}
			}
		}
		return result;
	}

	public static int StringToAsciiInt32(string str)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(AtOManager.Instance.GetGameId());
		int total = 0;
		Array.ForEach(bytes, delegate(byte i)
		{
			total += i;
		});
		return Convert.ToInt32(total);
	}

	public static double ConvertBytesToMegabytes(long bytes)
	{
		return (float)bytes / 1024f / 1024f;
	}

	public static int AsciiSumFromString(string _str)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(_str);
		int total = 0;
		Array.ForEach(bytes, delegate(byte i)
		{
			total += i;
		});
		return total;
	}

	public static string GetAuraCurseImmune(NPCData _npcData, string _mapNode = "")
	{
		List<string> list = new List<string>();
		list.Add("bleed");
		list.Add("burn");
		list.Add("chill");
		list.Add("crack");
		list.Add("dark");
		list.Add("decay");
		list.Add("insane");
		list.Add("mark");
		list.Add("poison");
		list.Add("sanctify");
		list.Add("sight");
		list.Add("slow");
		list.Add("spark");
		list.Add("vulnerable");
		list.Add("wet");
		list.Add("sleep");
		StringBuilder stringBuilder = new StringBuilder();
		if (_mapNode == "")
		{
			_mapNode = AtOManager.Instance.currentMapNode;
		}
		stringBuilder.Append(_mapNode);
		stringBuilder.Append(AtOManager.Instance.GetGameId());
		stringBuilder.Append(AtOManager.Instance.GetObeliskMadness());
		stringBuilder.Append(AtOManager.Instance.GetMadnessDifficulty());
		int num = (int)((float)AsciiSumFromString(stringBuilder.ToString()) % (float)list.Count);
		string text = "";
		bool flag = false;
		while (!flag)
		{
			text = list[num];
			if (_npcData != null && _npcData.AuracurseImmune != null && _npcData.AuracurseImmune.Contains(text))
			{
				if (AtOManager.Instance.Sandbox_doubleChampions)
				{
					flag = true;
					continue;
				}
				num++;
				if (num >= list.Count)
				{
					num = 0;
				}
			}
			else
			{
				flag = true;
			}
		}
		return text;
	}

	public static bool TransformIsVisible(Transform _obj)
	{
		if (_obj == null)
		{
			return false;
		}
		while (_obj != null)
		{
			if (!_obj.gameObject.activeSelf)
			{
				return false;
			}
			_obj = _obj.parent;
		}
		return true;
	}

	public static int GetTransformIndexInList(List<Transform> theList, string theName)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			if (theList[i] != null && theList[i].gameObject.name == theName)
			{
				return i;
			}
		}
		return 0;
	}

	public static int GetListClosestIndexToMousePosition(List<Transform> theList, bool checkUiItems = false)
	{
		Vector3 b = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, 10f);
		if (KeyboardManager.Instance.IsActive() || checkUiItems)
		{
			b = Input.mousePosition + new Vector3(0f, 0f, 10f);
		}
		Vector3 zero = Vector3.zero;
		float num = 10000f;
		int result = 0;
		for (int i = 0; i < theList.Count; i++)
		{
			if (theList[i] != null)
			{
				zero.x = theList[i].position.x;
				zero.y = theList[i].position.y;
				if (Vector3.Distance(zero, b) < num)
				{
					result = i;
					num = Vector3.Distance(zero, b);
				}
			}
		}
		return result;
	}

	public static int GetClosestIndexFromList(Transform theTransform, List<Transform> theList, int excludeIndex = -1, Vector3 offset = default(Vector3))
	{
		Vector3 vector = theTransform.position + offset;
		Vector3 zero = Vector3.zero;
		zero.x = vector.x;
		zero.y = vector.y;
		Vector3 zero2 = Vector3.zero;
		float num = float.MaxValue;
		int result = -1;
		for (int i = 0; i < theList.Count; i++)
		{
			if (i != excludeIndex && theList[i] != null)
			{
				zero2.x = theList[i].position.x;
				zero2.y = theList[i].position.y;
				if (Vector3.Distance(zero2, zero) < num)
				{
					num = Vector3.Distance(zero2, zero);
					result = i;
				}
			}
		}
		return result;
	}

	public static int GetClosestIndexBasedOnDirection(List<Transform> theList, int index, bool goingUp, bool goingRight, bool goingDown, bool goingLeft, bool checkUiItems = false)
	{
		if (theList == null || theList.Count == 0 || index < 0 || index > theList.Count - 1)
		{
			return 0;
		}
		Vector3 position = theList[index].position;
		List<Transform> list = new List<Transform>();
		List<Transform> list2 = new List<Transform>();
		list.Add(theList[index]);
		list2.Add(theList[index]);
		for (int i = 0; i < theList.Count; i++)
		{
			if (i == index || !(theList[i] != null))
			{
				continue;
			}
			if (theList[i].position.y == position.y)
			{
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					if (theList[i].position.x < list[j].position.x)
					{
						list.Insert(j, theList[i]);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(theList[i]);
				}
			}
			else
			{
				if (theList[i].position.x != position.x)
				{
					continue;
				}
				bool flag2 = false;
				for (int k = 0; k < list2.Count; k++)
				{
					if (theList[i].position.y < list2[k].position.y)
					{
						list2.Insert(k, theList[i]);
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					list2.Add(theList[i]);
				}
			}
		}
		if ((goingLeft || goingRight) && list.Count > 1)
		{
			for (int l = 0; l < list.Count; l++)
			{
				if (!(list[l] == theList[index]))
				{
					continue;
				}
				if (goingRight)
				{
					if (l < list.Count - 1)
					{
						_ = list[l + 1];
						break;
					}
				}
				else if (l > 0)
				{
					_ = list[l - 1];
					break;
				}
			}
		}
		bool flag3 = false;
		float num = 0f;
		int num2 = -1;
		int num3 = index;
		int num4 = 100;
		while (!flag3 && num2 < num4)
		{
			num2++;
			num = ((num2 >= num4 / 2) ? ((float)(num2 - num4 / 2 + 1) * 0.3f) : ((!(goingLeft || goingRight)) ? ((float)(num2 + 1) * 0.035f) : ((float)(num2 + 1) * 0.04f)));
			if (checkUiItems)
			{
				num = ((!(goingUp || goingDown)) ? (num * 1000f) : (num * 200f));
			}
			index = num3;
			if (goingDown)
			{
				index = GetClosestIndexFromList(theList[index], theList, index, new Vector3(0f, 0f - num, 0f));
				if (num2 < num4 / 2)
				{
					if (index > -1 && position.y - theList[index].position.y > 0.25f && Mathf.Abs(position.x - theList[index].position.x) < 0.1f)
					{
						flag3 = true;
					}
				}
				else if (index > -1 && position.y - theList[index].position.y > 0.25f)
				{
					flag3 = true;
				}
			}
			else if (goingUp)
			{
				index = GetClosestIndexFromList(theList[index], theList, index, new Vector3(0f, num, 0f));
				if (num2 < num4 / 2)
				{
					if (index > -1 && theList[index].position.y - position.y > 0.25f && Mathf.Abs(position.x - theList[index].position.x) < 0.1f)
					{
						flag3 = true;
					}
				}
				else if (index > -1 && theList[index].position.y - position.y > 0.25f)
				{
					flag3 = true;
				}
			}
			else if (goingRight)
			{
				index = GetClosestIndexFromList(theList[index], theList, index, new Vector3(num, 0f, 0f));
				if (index > -1 && list2.Contains(theList[index]))
				{
					continue;
				}
				if (num2 < num4 / 2)
				{
					if (index > -1 && theList[index].position.x - position.x > 0.25f && Mathf.Abs(position.y - theList[index].position.y) < 0.3f)
					{
						flag3 = true;
					}
				}
				else if (index > -1 && theList[index].position.x - position.x > 0.25f)
				{
					flag3 = true;
				}
			}
			else
			{
				if (!goingLeft)
				{
					continue;
				}
				index = GetClosestIndexFromList(theList[index], theList, index, new Vector3(0f - num, 0f, 0f));
				if (index > -1 && list2.Contains(theList[index]))
				{
					continue;
				}
				if (num2 < num4 / 2)
				{
					if (index > -1 && position.x - theList[index].position.x > 0.25f && Mathf.Abs(position.y - theList[index].position.y) < 0.3f)
					{
						flag3 = true;
					}
				}
				else if (index > -1 && position.x - theList[index].position.x > 0.25f)
				{
					flag3 = true;
				}
			}
		}
		if (!flag3)
		{
			index = num3;
		}
		if (index > theList.Count - 1)
		{
			index = theList.Count - 1;
		}
		else if (index < 0)
		{
			index = 0;
		}
		return index;
	}

	public static List<Transform> FindChildrenByName(Transform transform, string[] namesToFind)
	{
		List<Transform> list = new List<Transform>();
		FindChildrenByNameRecursive(transform, namesToFind, list);
		return list;
	}

	private static void FindChildrenByNameRecursive(Transform parent, string[] namesToFind, List<Transform> foundChildren)
	{
		foreach (Transform item in parent)
		{
			if (ArrayContainsString(namesToFind, item.name))
			{
				foundChildren.Add(item);
			}
			FindChildrenByNameRecursive(item, namesToFind, foundChildren);
		}
	}

	private static bool ArrayContainsString(string[] array, string target)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == target)
			{
				return true;
			}
		}
		return false;
	}

	public static bool ClickedThisTransform(Transform _transform)
	{
		RaycastHit2D[] array = Physics2D.RaycastAll(GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 10f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].collider != null && (array[i].transform == _transform || array[i].transform.gameObject.name == _transform.gameObject.name))
			{
				return true;
			}
		}
		return false;
	}

	public static string GetNodeGroundSprite(Enums.NodeGround _ground)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<sprite name=");
		switch (_ground)
		{
		case Enums.NodeGround.HeavyRain:
			stringBuilder.Append("wet");
			break;
		case Enums.NodeGround.ExtremeHeat:
			stringBuilder.Append("burn");
			break;
		case Enums.NodeGround.FreezingCold:
			stringBuilder.Append("chill");
			break;
		case Enums.NodeGround.HolyGround:
			stringBuilder.Append("sanctify");
			break;
		case Enums.NodeGround.Graveyard:
			stringBuilder.Append("dark");
			break;
		case Enums.NodeGround.PoisonousAir:
			stringBuilder.Append("poison");
			break;
		case Enums.NodeGround.StaticElectricity:
			stringBuilder.Append("spark");
			break;
		case Enums.NodeGround.EerieAtmosphere:
			stringBuilder.Append("insane");
			break;
		case Enums.NodeGround.DampCavern:
			stringBuilder.Append("wet");
			break;
		case Enums.NodeGround.BloodMist:
			stringBuilder.Append("bleed");
			break;
		case Enums.NodeGround.Infestation:
			stringBuilder.Append("leech");
			break;
		default:
			return "";
		}
		stringBuilder.Append(">");
		return stringBuilder.ToString();
	}

	public static int Random(int min, int max, string seed)
	{
		if (min == max)
		{
			return min;
		}
		int num = 0;
		int length = seed.Length;
		for (int i = 0; i < length; i++)
		{
			num += seed[i];
			num += num << 10;
			num ^= num >> 6;
		}
		num += num << 3;
		num ^= num >> 11;
		num += num << 15;
		int num2 = max - min;
		return min + Mathf.Abs(num) % num2;
	}

	public static bool IsValidEmail(string email)
	{
		return new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").IsMatch(email);
	}

	public static string HideEmail(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return email;
		}
		int num = email.IndexOf('@');
		if (num <= 0)
		{
			return email;
		}
		string text = email.Substring(0, 1);
		string text2 = new string('*', num - 1);
		return text + text2 + email.Substring(num);
	}

	public static string TextChargesLeft(int currentCharges, int chargesTotal)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<br><color=#FFF>");
		stringBuilder.Append(currentCharges.ToString());
		stringBuilder.Append("/");
		stringBuilder.Append(chargesTotal.ToString());
		return stringBuilder.ToString();
	}

	public static bool SpaceBeforePercentSign()
	{
		if (Globals.Instance.CurrentLang == "es" || Globals.Instance.CurrentLang == "fr" || Globals.Instance.CurrentLang == "sv")
		{
			return true;
		}
		return false;
	}

	public static bool CompatibleVersion(string version1, string version2)
	{
		string[] array = version1.Split('.');
		string[] array2 = version2.Split('.');
		for (int i = 0; i < 3; i++)
		{
			string text = ((array.Length <= i) ? "0" : array[i]);
			string text2 = ((array2.Length <= i) ? "0" : array2[i]);
			if (text != text2)
			{
				return false;
			}
		}
		return true;
	}

	public static string ConcatStrings(string[] args)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < args.Length; i++)
		{
			stringBuilder.Append(args[i]);
		}
		return stringBuilder.ToString();
	}

	public static CultureInfo GetCultureInfoByTwoLetterCode(string twoLetterCode)
	{
		if (twoLetterCode == "jp")
		{
			twoLetterCode = "ja";
		}
		try
		{
			return CultureInfo.GetCultureInfo(twoLetterCode);
		}
		catch (CultureNotFoundException)
		{
			return null;
		}
	}

	public static int GetConsistentRandom(int value, int minLimit, int maxLimit)
	{
		int stableHash = GetStableHash(value);
		return minLimit + Math.Abs(stableHash) % (maxLimit - minLimit + 1);
	}

	public static int GetStableHash(int value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		using MD5 mD = MD5.Create();
		return BitConverter.ToInt32(mD.ComputeHash(bytes), 0);
	}

	public static string RemoveWhitespace(string str, bool toLower = false)
	{
		string text = Regex.Replace(str, "[\\s']+", "");
		if (toLower)
		{
			return text.ToLower();
		}
		return text;
	}
}
