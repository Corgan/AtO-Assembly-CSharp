using System;
using System.Collections.Generic;
using UnityEngine;
using Zone;

public static class CheatDrawer
{
	private class CheatDefinition
	{
		public string propertyName;

		public string label;

		public CheatDefinition(string propertyName, string label)
		{
			this.propertyName = propertyName;
			this.label = label;
		}
	}

	private static readonly List<CheatDefinition> cheats = new List<CheatDefinition>
	{
		new CheatDefinition("cheatMode", "Cheat Mode (+dlc)"),
		new CheatDefinition("winMatchOnStart", "Win match on start"),
		new CheatDefinition("skipTutorial", "Skip Tutorial"),
		new CheatDefinition("startFromMap", "Start from Map"),
		new CheatDefinition("useImmortal", "Use Immortal"),
		new CheatDefinition("useManyResources", "Use Many Resources"),
		new CheatDefinition("unlockAllHeroes", "Unlock All Heroes"),
		new CheatDefinition("unlockMadness", "Unlock Madness"),
		new CheatDefinition("disableSave", "Disable Save"),
		new CheatDefinition("disableSteamAuthorizationForPhoton", "Disable Steam Auth"),
		new CheatDefinition("useTestSteamID", "Use Test Steam ID")
	};

	public static void DrawCheatsRuntime(GameManager gameManager, GUIStyle headerStyle, GUIStyle toggleStyle, GUIStyle buttonStyle)
	{
		GUILayout.Label("Cheat Flags", headerStyle);
		gameManager.CheatMode = GUILayout.Toggle(gameManager.CheatMode, "Cheat Mode (+dlc)", toggleStyle);
		gameManager.WinMatchOnStart = GUILayout.Toggle(gameManager.WinMatchOnStart, "Win match on start", toggleStyle);
		gameManager.SkipTutorial = GUILayout.Toggle(gameManager.SkipTutorial, "Skip Tutorial", toggleStyle);
		gameManager.UseImmortal = GUILayout.Toggle(gameManager.UseImmortal, "Use Immortal", toggleStyle);
		gameManager.UseManyResources = GUILayout.Toggle(gameManager.UseManyResources, "Use Many Resources", toggleStyle);
		gameManager.UnlockAllHeroes = GUILayout.Toggle(gameManager.UnlockAllHeroes, "Unlock All Heroes", toggleStyle);
		gameManager.UnlockMadness = GUILayout.Toggle(gameManager.UnlockMadness, "Unlock Singularity Madness", toggleStyle);
		gameManager.IsSaveDisabled = GUILayout.Toggle(gameManager.IsSaveDisabled, "Disable Save", toggleStyle);
		gameManager.DisableSteamAuthorizationForPhoton = GUILayout.Toggle(gameManager.DisableSteamAuthorizationForPhoton, "Disable Steam Auth", toggleStyle);
		gameManager.UseTestSteamID = GUILayout.Toggle(gameManager.UseTestSteamID, "Use Test Steam ID", toggleStyle);
		GUILayout.Space(10f);
		GUILayout.Label("Start From Map:", toggleStyle);
		gameManager.StartFromMap = (MapType)GUILayout.SelectionGrid((int)gameManager.StartFromMap, Enum.GetNames(typeof(MapType)), 2, buttonStyle);
		GUILayout.Space(10f);
		if (GUILayout.Button("Clean Saved Data", buttonStyle))
		{
			SaveManager.CleanSavePlayerData();
			Debug.Log("Player data cleaned.");
		}
	}
}
