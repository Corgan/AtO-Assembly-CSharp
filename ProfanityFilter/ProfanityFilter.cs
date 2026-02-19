using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProfanityFilter.Interfaces;

namespace ProfanityFilter;

public class ProfanityFilter : ProfanityBase, IProfanityFilter
{
	public IAllowList AllowList { get; }

	public ProfanityFilter()
	{
		AllowList = new AllowList();
	}

	public ProfanityFilter(string[] profanityList)
		: base(profanityList)
	{
		AllowList = new AllowList();
	}

	public ProfanityFilter(List<string> profanityList)
		: base(profanityList)
	{
		AllowList = new AllowList();
	}

	public bool IsProfanity(string word)
	{
		if (string.IsNullOrEmpty(word))
		{
			return false;
		}
		if (AllowList.Contains(word.ToLower(CultureInfo.InvariantCulture)))
		{
			return false;
		}
		return _profanities.Contains(word.ToLower(CultureInfo.InvariantCulture));
	}

	public ReadOnlyCollection<string> DetectAllProfanities(string sentence)
	{
		return DetectAllProfanities(sentence, removePartialMatches: false);
	}

	public ReadOnlyCollection<string> DetectAllProfanities(string sentence, bool removePartialMatches)
	{
		if (string.IsNullOrEmpty(sentence))
		{
			return new ReadOnlyCollection<string>(new List<string>());
		}
		sentence = sentence.ToLower();
		sentence = sentence.Replace(".", "");
		sentence = sentence.Replace(",", "");
		string[] words = sentence.Split(' ');
		List<string> postAllowList = FilterWordListByAllowList(words);
		List<string> swearList = new List<string>();
		AddMultiWordProfanities(swearList, ConvertWordListToSentence(postAllowList));
		if (removePartialMatches)
		{
			swearList.RemoveAll((string x) => swearList.Any((string y) => x != y && y.Contains(x)));
		}
		return new ReadOnlyCollection<string>(FilterSwearListForCompleteWordsOnly(sentence, swearList).Distinct().ToList());
	}

	public string CensorString(string sentence)
	{
		return CensorString(sentence, '*');
	}

	public string CensorString(string sentence, char censorCharacter)
	{
		return CensorString(sentence, censorCharacter, ignoreNumbers: false);
	}

	public string CensorString(string sentence, char censorCharacter, bool ignoreNumbers)
	{
		if (string.IsNullOrEmpty(sentence))
		{
			return string.Empty;
		}
		string[] words = Regex.Replace(sentence.Trim().ToLower(), "[^\\w\\s]", "").Split(' ');
		List<string> postAllowList = FilterWordListByAllowList(words);
		List<string> swearList = new List<string>();
		AddMultiWordProfanities(swearList, ConvertWordListToSentence(postAllowList));
		StringBuilder censored = new StringBuilder(sentence);
		StringBuilder tracker = new StringBuilder(sentence);
		return CensorStringByProfanityList(censorCharacter, swearList, censored, tracker, ignoreNumbers).ToString();
	}

	public (int, int, string)? GetCompleteWord(string toCheck, string profanity)
	{
		if (string.IsNullOrEmpty(toCheck))
		{
			return null;
		}
		string value = profanity.ToLower(CultureInfo.InvariantCulture);
		string text = toCheck.ToLower(CultureInfo.InvariantCulture);
		if (text.Contains(value))
		{
			int num = text.IndexOf(value, StringComparison.Ordinal);
			int i = num;
			while (num > 0 && toCheck[num - 1] != ' ' && !char.IsPunctuation(toCheck[num - 1]))
			{
				num--;
			}
			for (; i < toCheck.Length && toCheck[i] != ' ' && !char.IsPunctuation(toCheck[i]); i++)
			{
			}
			return (num, i, text.Substring(num, i - num).ToLower(CultureInfo.InvariantCulture));
		}
		return null;
	}

	public bool ContainsProfanity(string term)
	{
		if (string.IsNullOrWhiteSpace(term))
		{
			return false;
		}
		List<string> list = _profanities.Where((string word) => word.Length <= term.Length).ToList();
		if (list.Count == 0)
		{
			return false;
		}
		foreach (Match item in new Regex(string.Format("(?:{0})", string.Join("|", list).Replace("$", "\\$"), RegexOptions.IgnoreCase)).Matches(term))
		{
			if (!AllowList.Contains(item.Value.ToLower(CultureInfo.InvariantCulture)))
			{
				return true;
			}
		}
		return false;
	}

	private StringBuilder CensorStringByProfanityList(char censorCharacter, List<string> swearList, StringBuilder censored, StringBuilder tracker, bool ignoreNumeric)
	{
		foreach (string item in swearList.OrderByDescending((string x) => x.Length))
		{
			(int, int, string)? tuple = (0, 0, "");
			if (item.Split(' ').Length == 1)
			{
				do
				{
					tuple = GetCompleteWord(tracker.ToString(), item);
					if (!tuple.HasValue)
					{
						continue;
					}
					string text = tuple.Value.Item3;
					if (ignoreNumeric)
					{
						text = Regex.Replace(tuple.Value.Item3, "[\\d-]", string.Empty);
					}
					if (text == item)
					{
						for (int num = tuple.Value.Item1; num < tuple.Value.Item2; num++)
						{
							censored[num] = censorCharacter;
							tracker[num] = censorCharacter;
						}
					}
					else
					{
						for (int num2 = tuple.Value.Item1; num2 < tuple.Value.Item2; num2++)
						{
							tracker[num2] = censorCharacter;
						}
					}
				}
				while (tuple.HasValue);
			}
			else
			{
				censored = censored.Replace(item, CreateCensoredString(item, censorCharacter));
			}
		}
		return censored;
	}

	private List<string> FilterSwearListForCompleteWordsOnly(string sentence, List<string> swearList)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder(sentence);
		foreach (string item in swearList.OrderByDescending((string x) => x.Length))
		{
			(int, int, string)? tuple = (0, 0, "");
			if (item.Split(' ').Length == 1)
			{
				do
				{
					tuple = GetCompleteWord(stringBuilder.ToString(), item);
					if (!tuple.HasValue)
					{
						continue;
					}
					if (tuple.Value.Item3 == item)
					{
						list.Add(item);
						for (int num = tuple.Value.Item1; num < tuple.Value.Item2; num++)
						{
							stringBuilder[num] = '*';
						}
						break;
					}
					for (int num2 = tuple.Value.Item1; num2 < tuple.Value.Item2; num2++)
					{
						stringBuilder[num2] = '*';
					}
				}
				while (tuple.HasValue);
			}
			else
			{
				list.Add(item);
				stringBuilder.Replace(item, " ");
			}
		}
		return list;
	}

	private List<string> FilterWordListByAllowList(string[] words)
	{
		List<string> list = new List<string>();
		foreach (string text in words)
		{
			if (!string.IsNullOrEmpty(text) && !AllowList.Contains(text.ToLower(CultureInfo.InvariantCulture)))
			{
				list.Add(text);
			}
		}
		return list;
	}

	private static string ConvertWordListToSentence(List<string> postAllowList)
	{
		string text = string.Empty;
		foreach (string postAllow in postAllowList)
		{
			text = text + postAllow + " ";
		}
		return text;
	}

	private void AddMultiWordProfanities(List<string> swearList, string postAllowListSentence)
	{
		swearList.AddRange(from string profanity in _profanities
			where postAllowListSentence.ToLower(CultureInfo.InvariantCulture).Contains(profanity)
			select profanity);
	}

	private static string CreateCensoredString(string word, char censorCharacter)
	{
		string text = string.Empty;
		for (int i = 0; i < word.Length; i++)
		{
			text = ((word[i] == ' ') ? (text + " ") : (text + censorCharacter));
		}
		return text;
	}
}
