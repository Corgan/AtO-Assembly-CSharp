using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zone;

public static class CheatDrawer
{
	private class CheatDefinition
	{
		public string PropertyName { get; }

		public string Label { get; }

		public CheatDefinition(string propertyName, string label)
		{
			PropertyName = propertyName;
			Label = label;
		}
	}

	private const string ZonesResourcesPath = "World/Zones";

	private const string CombatsResourcesRoot = "World/Combats";

	private const string RandomToken = "__RANDOM__";

	private const string NoneLabel = "None";

	private static readonly List<CheatDefinition> cheats = new List<CheatDefinition>
	{
		new CheatDefinition("cheatMode", "Cheat Mode (+dlc)"),
		new CheatDefinition("winMatchOnStart", "Win battle"),
		new CheatDefinition("skipTutorial", "Skip Tutorial"),
		new CheatDefinition("useImmortal", "Immortal"),
		new CheatDefinition("useManyResources", "Many Resources"),
		new CheatDefinition("enableButtons", "Rank, levelUp, shop.. buttons"),
		new CheatDefinition("unlockAllExceptHeroes", "Unlock cards, pets, madness"),
		new CheatDefinition("unlockHeroes", "Unlock heroes"),
		new CheatDefinition("disableSave", "Disable Save"),
		new CheatDefinition("disableSteamAuthorizationForPhoton", "Disable multiplayer auth"),
		new CheatDefinition("useTestSteamID", "Test Steam ID")
	};

	private static string[] GetCombatNamesForZone(string zoneName)
	{
		if (string.IsNullOrEmpty(zoneName))
		{
			return Array.Empty<string>();
		}
		CombatData[] array = Resources.LoadAll<CombatData>("World/Combats/" + zoneName);
		if (array == null)
		{
			return Array.Empty<string>();
		}
		return (from n in (from c in array
				where c != null
				select c.name).Distinct()
			orderby n
			select n).ToArray();
	}

	private static string[] BuildCombatDropdownOptions(string zoneName)
	{
		string[] combatNamesForZone = GetCombatNamesForZone(zoneName);
		return new string[1] { "Random" }.Concat(combatNamesForZone).ToArray();
	}

	public static void DrawCheatsRuntime(GameManager gameManager, GUIStyle headerStyle, GUIStyle toggleStyle, GUIStyle buttonStyle)
	{
		GUILayout.Label("Cheat Flags", headerStyle);
		GUIStyle gUIStyle = new GUIStyle(toggleStyle);
		gUIStyle.padding.left += 10;
		foreach (CheatDefinition cheat in cheats)
		{
			FieldInfo field = gameManager.GetType().GetField(cheat.PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (!(field == null) && !(field.FieldType != typeof(bool)))
			{
				bool flag = (bool)field.GetValue(gameManager);
				bool flag2 = GUILayout.Toggle(flag, cheat.Label, gUIStyle);
				if (flag2 != flag)
				{
					field.SetValue(gameManager, flag2);
				}
			}
		}
		GUILayout.Space(10f);
		GUILayout.Label("Start From Map:", toggleStyle);
		gameManager.StartFromMap = (MapType)GUILayout.SelectionGrid((int)gameManager.StartFromMap, Enum.GetNames(typeof(MapType)), 2, buttonStyle);
		if (gameManager.CheatMode)
		{
			GUILayout.Space(12f);
			GUILayout.Label("Practice Mode Combat", headerStyle);
			ZoneData[] array = Resources.LoadAll<ZoneData>("World/Zones");
			if (array == null || array.Length == 0)
			{
				GUILayout.Label("No ZoneData found in Resources/World/Zones", toggleStyle);
				return;
			}
			string[] array2 = (from n in (from z in array
					where z != null
					select z.name).Distinct()
				orderby n
				select n).ToArray();
			string[] texts = new string[1] { "None" }.Concat(array2).ToArray();
			int num = 0;
			if (!string.IsNullOrEmpty(gameManager.CheatZoneName))
			{
				int num2 = Array.IndexOf(array2, gameManager.CheatZoneName);
				num = ((num2 >= 0) ? (num2 + 1) : 0);
			}
			int num3 = GUILayout.SelectionGrid(num, texts, 2, buttonStyle);
			if (num3 != num)
			{
				gameManager.CheatZoneName = ((num3 == 0) ? string.Empty : array2[num3 - 1]);
				gameManager.CheatCombatName = "__RANDOM__";
			}
			if (string.IsNullOrEmpty(gameManager.CheatZoneName))
			{
				GUILayout.Label("Override disabled (Zone = None)", toggleStyle);
				return;
			}
			string[] array3 = BuildCombatDropdownOptions(gameManager.CheatZoneName);
			int selected = 0;
			if (!string.IsNullOrEmpty(gameManager.CheatCombatName) && gameManager.CheatCombatName != "__RANDOM__")
			{
				int b = Array.IndexOf(array3, gameManager.CheatCombatName);
				selected = Mathf.Max(0, b);
			}
			int num4 = GUILayout.SelectionGrid(selected, array3, 1, buttonStyle);
			gameManager.CheatCombatName = ((num4 == 0) ? "__RANDOM__" : array3[num4]);
			if (array3.Length <= 1)
			{
				GUILayout.Label("No combats in Resources/World/Combats/" + gameManager.CheatZoneName, toggleStyle);
			}
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("DELETE ALL SAVES", buttonStyle))
		{
			SaveManager.CleanSavePlayerData();
			Debug.Log("Player data cleaned.");
		}
	}
}
