using System;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Hero", menuName = "Hero Data", order = 52)]
public class HeroData : ScriptableObject
{
	[SerializeField]
	private string heroName;

	[SerializeField]
	private string id;

	[SerializeField]
	private Enums.HeroClass heroClass;

	[SerializeField]
	private SubClassData heroSubClass;

	public string HeroName
	{
		get
		{
			return heroName;
		}
		set
		{
			heroName = value;
		}
	}

	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public Enums.HeroClass HeroClass
	{
		get
		{
			return heroClass;
		}
		set
		{
			heroClass = value;
		}
	}

	public SubClassData HeroSubClass
	{
		get
		{
			return heroSubClass;
		}
		set
		{
			heroSubClass = value;
		}
	}

	private void Awake()
	{
		id = Regex.Replace(heroName, "\\s+", "").ToLower();
	}
}
