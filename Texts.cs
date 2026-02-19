using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class Texts : MonoBehaviour
{
	[SerializeField]
	private Dictionary<string, Dictionary<string, string>> TextStrings;

	private Dictionary<string, Dictionary<string, string>> TextKeynotes;

	private List<string> tipsList;

	private string lang = "";

	private bool countWords;

	private bool translationLoaded;

	private bool translationFinished;

	public static Texts Instance { get; private set; }

	public List<string> TipsList
	{
		get
		{
			return tipsList;
		}
		set
		{
			tipsList = value;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		TextStrings = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
		TextKeynotes = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
	}

	public bool GotTranslations()
	{
		return translationFinished;
	}

	public void LoadTranslation()
	{
		if (!GameManager.Instance.PrefsLoaded)
		{
			return;
		}
		if (translationLoaded)
		{
			Debug.LogError("LoadTranslation translationLoaded");
			return;
		}
		translationLoaded = true;
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("LoadTranslation");
		}
		lang = "en";
		TextStrings[lang] = new Dictionary<string, string>();
		TextKeynotes[lang] = new Dictionary<string, string>();
		LoadTranslationText("cards");
		LoadTranslationText("traits");
		LoadTranslationText("");
		LoadTranslationText("keynotes");
		LoadTranslationText("auracurse");
		LoadTranslationText("events");
		LoadTranslationText("nodes");
		LoadTranslationText("fluff");
		LoadTranslationText("class");
		LoadTranslationText("monsters");
		LoadTranslationText("requirements");
		LoadTranslationText("tips");
		if (Globals.Instance.CurrentLang != "en")
		{
			lang = Globals.Instance.CurrentLang;
			TextStrings[lang] = TextStrings["en"];
			TextKeynotes[lang] = TextKeynotes["en"];
			LoadTranslationText("cards");
			LoadTranslationText("traits");
			LoadTranslationText("");
			LoadTranslationText("keynotes");
			LoadTranslationText("auracurse");
			LoadTranslationText("events");
			LoadTranslationText("nodes");
			LoadTranslationText("fluff");
			LoadTranslationText("class");
			LoadTranslationText("monsters");
			LoadTranslationText("requirements");
			LoadTranslationText("tips");
		}
		translationFinished = true;
	}

	private void LoadTranslationText(string type)
	{
		TextAsset textAsset = new TextAsset();
		string text = "";
		type = type.ToLower();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Lang/");
		stringBuilder.Append(lang);
		stringBuilder.Append("/");
		stringBuilder.Append(lang);
		switch (type)
		{
		case "":
			text = lang + ".txt";
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "keynotes":
			text = lang + "_keynotes.txt";
			stringBuilder.Append("_keynotes");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "traits":
			text = lang + "_traits.txt";
			stringBuilder.Append("_traits");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "auracurse":
			text = lang + "_auracurse.txt";
			stringBuilder.Append("_auracurse");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "events":
			text = lang + "_events.txt";
			stringBuilder.Append("_events");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "nodes":
			text = lang + "_nodes.txt";
			stringBuilder.Append("_nodes");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "cards":
			text = lang + "_cards.txt";
			stringBuilder.Append("_cards");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "fluff":
			text = lang + "_cardsfluff.txt";
			stringBuilder.Append("_cardsfluff");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "class":
			text = lang + "_class.txt";
			stringBuilder.Append("_class");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "monsters":
			text = lang + "_monsters.txt";
			stringBuilder.Append("_monsters");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "requirements":
			text = lang + "_requirements.txt";
			stringBuilder.Append("_requirements");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			break;
		case "tips":
			text = lang + "_tips.txt";
			stringBuilder.Append("_tips");
			textAsset = Resources.Load(stringBuilder.ToString()) as TextAsset;
			tipsList = new List<string>();
			break;
		}
		if (textAsset == null)
		{
			return;
		}
		List<string> list = new List<string>(textAsset.text.Split('\n'));
		int num = 0;
		bool flag = true;
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		new StringBuilder();
		new StringBuilder();
		for (int i = 0; i < list.Count; i++)
		{
			string text2 = list[i];
			if (text2 == "" || text2[0] == '#')
			{
				continue;
			}
			string[] array = text2.Trim().Split(new char[1] { '=' }, 2);
			if (array == null || array.Length < 2)
			{
				continue;
			}
			array[0] = array[0].Trim().ToLower();
			array[1] = Functions.SplitString("//", array[1])[0].Trim();
			switch (type)
			{
			case "keynotes":
				stringBuilder2.Append("keynotes_");
				break;
			case "traits":
				stringBuilder2.Append("traits_");
				break;
			case "auracurse":
				stringBuilder2.Append("auracurse_");
				break;
			case "events":
				stringBuilder2.Append("events_");
				break;
			case "nodes":
				stringBuilder2.Append("nodes_");
				break;
			case "cards":
			case "fluff":
				stringBuilder2.Append("cards_");
				break;
			case "class":
				stringBuilder2.Append("class_");
				break;
			case "monsters":
				stringBuilder2.Append("monsters_");
				break;
			case "requirements":
				stringBuilder2.Append("requirements_");
				break;
			case "tips":
				stringBuilder2.Append("tips_");
				break;
			}
			stringBuilder2.Append(array[0]);
			if (TextStrings[lang].ContainsKey(stringBuilder2.ToString()))
			{
				TextStrings[lang][stringBuilder2.ToString()] = array[1];
			}
			else
			{
				TextStrings[lang].Add(stringBuilder2.ToString(), array[1]);
			}
			if (type == "tips")
			{
				tipsList.Add(array[1]);
			}
			flag = true;
			switch (type)
			{
			case "":
				if (array[1].StartsWith("rptd_", StringComparison.OrdinalIgnoreCase))
				{
					stringBuilder3.Append(array[1].Substring(5).ToLower());
					TextStrings[lang][stringBuilder2.ToString()] = TextStrings[lang][stringBuilder3.ToString()];
					flag = false;
					stringBuilder3.Clear();
				}
				break;
			case "events":
				if (array[1].StartsWith("rptd_", StringComparison.OrdinalIgnoreCase))
				{
					stringBuilder3.Append("events_");
					stringBuilder3.Append(array[1].Substring(5).ToLower());
					if (TextStrings[lang].ContainsKey(stringBuilder3.ToString()))
					{
						TextStrings[lang][stringBuilder2.ToString()] = TextStrings[lang][stringBuilder3.ToString()];
					}
					flag = false;
					stringBuilder3.Clear();
				}
				break;
			case "cards":
				if (array[1].StartsWith("rptd_", StringComparison.OrdinalIgnoreCase))
				{
					stringBuilder3.Append("cards_");
					stringBuilder3.Append(array[1].Substring(5).ToLower());
					TextStrings[lang][stringBuilder2.ToString()] = TextStrings[lang][stringBuilder3.ToString()];
					flag = false;
					stringBuilder3.Clear();
				}
				break;
			case "monsters":
				if (array[1].StartsWith("rptd_", StringComparison.OrdinalIgnoreCase))
				{
					stringBuilder3.Append("monsters_");
					stringBuilder3.Append(array[1].Substring(5).ToLower());
					TextStrings[lang][stringBuilder2.ToString()] = TextStrings[lang][stringBuilder3.ToString()];
					flag = false;
					stringBuilder3.Clear();
				}
				break;
			}
			if (flag && countWords)
			{
				string input = Regex.Replace(array[1], "<(.*?)>", "");
				input = Regex.Replace(input, "\\s+", " ");
				num += input.Split(" ").Length;
			}
			stringBuilder2.Clear();
		}
		if (countWords)
		{
			Debug.Log("Count words file -> " + text + " = " + num);
		}
		textAsset = null;
		stringBuilder2 = null;
		stringBuilder3 = null;
	}

	public string GetText(string _id, string _type = "")
	{
		if (Globals.Instance == null || !GameManager.Instance.PrefsLoaded || string.IsNullOrWhiteSpace(_id))
		{
			return "";
		}
		_id = _id.Replace(" ", "").ToLower();
		if (!string.IsNullOrEmpty(_type))
		{
			_id = _type.ToLower() + "_" + _id;
		}
		if (TextStrings.TryGetValue(Globals.Instance.CurrentLang, out var value) && value.TryGetValue(_id, out var value2))
		{
			return value2;
		}
		return "";
	}

	public void SetText(string _id, string _text)
	{
		_id = _id.Replace(" ", "").ToLower();
		if (_id != "" && !TextStrings[Globals.Instance.CurrentLang].ContainsKey(_id))
		{
			TextStrings[Globals.Instance.CurrentLang][_id] = _text;
		}
	}
}
