using System.IO;

public static class SaveText
{
	public static void SaveEvents(string str)
	{
		string text = "EventsText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveAuras(string str)
	{
		string text = "AurasText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveTraits(string str)
	{
		string text = "TraitsText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveClass(string str)
	{
		string text = "ClassText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveMonster(string str)
	{
		string text = "MonsterText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveCards(string str)
	{
		string text = "CardsText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveCardsFluff(string str)
	{
		string text = "CardsFluffText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveKeynotes(string str)
	{
		string text = "KeynotesText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveNodes(string str)
	{
		string text = "NodesText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveRequirements(string str)
	{
		string text = "RequirementsText.txt";
		string text2 = "Assets/TextsFromGame/";
		if (Directory.Exists(text2))
		{
			StreamWriter streamWriter = File.CreateText(text2 + text);
			streamWriter.WriteLine(str);
			streamWriter.Close();
		}
	}

	public static void SaveCards()
	{
	}
}
