using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ProfanityFilter.Interfaces;

namespace ProfanityFilter;

public class AllowList : IAllowList
{
	private List<string> _allowList;

	public ReadOnlyCollection<string> ToList => new ReadOnlyCollection<string>(_allowList);

	public int Count => _allowList.Count;

	public AllowList()
	{
		_allowList = new List<string>();
	}

	public void Add(string wordToAllowlist)
	{
		if (string.IsNullOrEmpty(wordToAllowlist))
		{
			throw new ArgumentNullException("wordToAllowlist");
		}
		if (!_allowList.Contains(wordToAllowlist.ToLower(CultureInfo.InvariantCulture)))
		{
			_allowList.Add(wordToAllowlist.ToLower(CultureInfo.InvariantCulture));
		}
	}

	public bool Contains(string wordToCheck)
	{
		if (string.IsNullOrEmpty(wordToCheck))
		{
			throw new ArgumentNullException("wordToCheck");
		}
		return _allowList.Contains(wordToCheck.ToLower(CultureInfo.InvariantCulture));
	}

	public void Clear()
	{
		_allowList.Clear();
	}

	public bool Remove(string wordToRemove)
	{
		if (string.IsNullOrEmpty(wordToRemove))
		{
			throw new ArgumentNullException("wordToRemove");
		}
		return _allowList.Remove(wordToRemove.ToLower(CultureInfo.InvariantCulture));
	}
}
