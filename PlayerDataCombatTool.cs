using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerDataCombatTool
{
	public int profileId;

	public string[] LastUsedTeam;

	public Dictionary<string, List<string>> HeroPerks;

	private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "playerCombatTool.json");

	public PlayerDataCombatTool()
	{
		profileId = -1;
		LastUsedTeam = Array.Empty<string>();
		HeroPerks = new Dictionary<string, List<string>>();
	}

	public void Save()
	{
		try
		{
			string contents = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(SavePath, contents);
			Debug.Log("Saved PlayerDataCombatTool to: " + SavePath);
		}
		catch (Exception arg)
		{
			Debug.LogError($"Failed to save PlayerDataCombatTool: {arg}");
		}
	}

	public static PlayerDataCombatTool Load()
	{
		try
		{
			if (!File.Exists(SavePath))
			{
				Debug.Log("Save not found, returning new PlayerDataCombatTool()");
				return new PlayerDataCombatTool();
			}
			PlayerDataCombatTool playerDataCombatTool = JsonConvert.DeserializeObject<PlayerDataCombatTool>(File.ReadAllText(SavePath));
			if (playerDataCombatTool == null)
			{
				Debug.LogWarning("Deserialization returned null. Returning new instance.");
				return new PlayerDataCombatTool();
			}
			if (playerDataCombatTool.LastUsedTeam == null)
			{
				playerDataCombatTool.LastUsedTeam = Array.Empty<string>();
			}
			if (playerDataCombatTool.HeroPerks == null)
			{
				playerDataCombatTool.HeroPerks = new Dictionary<string, List<string>>();
			}
			return playerDataCombatTool;
		}
		catch (Exception arg)
		{
			Debug.LogError($"Failed to load PlayerDataCombatTool: {arg}. Returning new instance.");
			return new PlayerDataCombatTool();
		}
	}
}
