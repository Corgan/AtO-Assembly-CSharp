using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Paradox;
using UnityEngine;

public static class SaveManager
{
	[Serializable]
	public class ContinueData
	{
		public string title;

		public string desc;

		public string date;

		public string rawGameVersion;

		public int slot;
	}

	[Serializable]
	public class JSONData
	{
		public int slot;

		public int number;
	}

	private static string savePlayerName = "player";

	private static string saveGameName = "gamedata_";

	private static string saveGameTurnExtension = "_turn";

	private static string profileFileName = "profile_name";

	private static string saveRunsName = "runs";

	private static string saveDecksName = "decks";

	private static string saveDecksSingularityName = "decks_s";

	private static string savePerksName = "perks";

	private static string saveMutedName = "muted";

	private static string saveGameExtension = ".ato";

	private static string saveGameExtensionBK = ".bak";

	private static string saveGameContinueName = "continue_game.json";

	private static string backupName = "_backup";

	private static byte[] key = new byte[8] { 18, 54, 100, 160, 190, 148, 136, 3 };

	private static byte[] iv = new byte[8] { 82, 242, 164, 132, 119, 197, 179, 20 };

	private static PlayerData playerDataStatic;

	private static int backupLimitFiles = 50;

	public static PlayerDataCombatTool playerDataCombatTool = new PlayerDataCombatTool();

	public static bool ExistsProfileFolder(int _slot)
	{
		if (_slot > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Application.persistentDataPath);
			stringBuilder.Append("/");
			stringBuilder.Append(SteamManager.Instance.steamId);
			stringBuilder.Append("/");
			stringBuilder.Append("profile");
			stringBuilder.Append(_slot);
			if (!Directory.Exists(stringBuilder.ToString()))
			{
				return false;
			}
		}
		return true;
	}

	public static void CreateProfileFolder(int _slot, string _name)
	{
		if (_slot >= 1 && _slot <= 4 && !(_name == ""))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Application.persistentDataPath);
			stringBuilder.Append("/");
			stringBuilder.Append(SteamManager.Instance.steamId);
			stringBuilder.Append("/");
			stringBuilder.Append("profile");
			stringBuilder.Append(_slot);
			if (Directory.Exists(stringBuilder.ToString()))
			{
				DeleteDirectory(stringBuilder.ToString());
			}
			Directory.CreateDirectory(stringBuilder.ToString());
			if (Directory.Exists(stringBuilder.ToString()))
			{
				stringBuilder.Append("/");
				stringBuilder.Append(profileFileName);
				stringBuilder.Append(saveGameExtension);
				File.WriteAllText(stringBuilder.ToString(), _name);
				MainMenuManager.Instance.LoadProfiles();
				MainMenuManager.Instance.UseProfile(_slot);
			}
		}
	}

	public static void DeleteProfileFolder(int _slot)
	{
		if (_slot >= 1 && _slot <= 4)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Application.persistentDataPath);
			stringBuilder.Append("/");
			stringBuilder.Append(SteamManager.Instance.steamId);
			stringBuilder.Append("/");
			stringBuilder.Append("profile");
			stringBuilder.Append(_slot);
			if (Directory.Exists(stringBuilder.ToString()))
			{
				DeleteDirectory(stringBuilder.ToString());
			}
		}
	}

	public static string[] GetProfileNames()
	{
		string[] array = new string[5];
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		for (int i = 0; i < 5; i++)
		{
			if (i == 0)
			{
				array[i] = Texts.Instance.GetText("default");
				continue;
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(stringBuilder.ToString());
			stringBuilder2.Append("profile");
			stringBuilder2.Append(i);
			if (!Directory.Exists(stringBuilder2.ToString()))
			{
				array[i] = "";
				continue;
			}
			stringBuilder2.Append("/");
			stringBuilder2.Append(profileFileName);
			stringBuilder2.Append(saveGameExtension);
			if (!File.Exists(stringBuilder2.ToString()))
			{
				array[i] = "";
				continue;
			}
			FileStream fileStream = new FileStream(stringBuilder2.ToString(), FileMode.Open);
			if (fileStream.Length == 0L)
			{
				fileStream.Close();
				array[i] = "";
			}
			else
			{
				string text = new StreamReader(fileStream, Encoding.UTF8).ReadToEnd();
				array[i] = text;
				fileStream.Close();
			}
		}
		return array;
	}

	public static bool GameSaveSlotExists(int slot, string profileFolder = "")
	{
		if (profileFolder == "")
		{
			profileFolder = GameManager.Instance.ProfileFolder;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(profileFolder);
		stringBuilder.Append(saveGameName);
		stringBuilder.Append(slot);
		stringBuilder.Append(saveGameExtension);
		return File.Exists(stringBuilder.ToString());
	}

	public static void SaveGame(int slot, bool backUp = false)
	{
		if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
		{
			slot = -1;
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("******* SAVE GAME (" + slot + ") *******");
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveGameName);
		stringBuilder.Append(slot);
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(stringBuilder.ToString());
		stringBuilder2.Append(saveGameExtensionBK);
		stringBuilder.Append(saveGameExtension);
		string text = stringBuilder.ToString();
		string destFileName = stringBuilder2.ToString();
		if (backUp && File.Exists(text))
		{
			File.Copy(text, destFileName, overwrite: true);
		}
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		FileStream fileStream = null;
		bool newGame = false;
		try
		{
			fileStream = new FileStream(text, FileMode.Create, FileAccess.Write);
			using (CryptoStream cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				GameData gameData = new GameData();
				gameData.FillData(newGame);
				binaryFormatter.Serialize(cryptoStream, gameData);
				cryptoStream.Close();
			}
			fileStream.Close();
			SaveGameContinue(slot);
		}
		catch
		{
			GameManager.Instance.AbortGameSave();
		}
	}

	public static void RemoveGameContinue(int slot = -1)
	{
		if (slot <= -1 || GetSlotFromContinue() == slot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Application.persistentDataPath);
			stringBuilder.Append("/");
			stringBuilder.Append(saveGameContinueName);
			if (File.Exists(stringBuilder.ToString()))
			{
				File.Delete(stringBuilder.ToString());
			}
		}
	}

	private static void SaveGameContinue(int slot)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			return;
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("******* SAVE CONTINUE GAME (" + slot + ") *******");
		}
		if (!(Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode) != null))
		{
			return;
		}
		string nodeName = Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeName;
		string text;
		if (slot >= 0 && slot <= 11)
		{
			text = Texts.Instance.GetText("modeAdventure");
		}
		else if (slot >= 12 && slot <= 23)
		{
			text = Texts.Instance.GetText("modeObelisk");
		}
		else if (slot >= 24 && slot <= 35)
		{
			text = Texts.Instance.GetText("modeWeekly");
		}
		else
		{
			if (slot < 36 || slot > 47)
			{
				return;
			}
			text = Texts.Instance.GetText("singularity");
		}
		ContinueData continueData = new ContinueData();
		continueData.title = nodeName;
		continueData.desc = text;
		continueData.date = DateTime.Now.ToString();
		continueData.rawGameVersion = GameManager.Instance.gameVersion;
		continueData.slot = slot;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(saveGameContinueName);
		File.WriteAllText(stringBuilder.ToString(), JsonUtility.ToJson(continueData));
	}

	public static int GetSlotFromContinue()
	{
		int result = -1;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(saveGameContinueName);
		if (File.Exists(stringBuilder.ToString()))
		{
			ContinueData continueData = JsonUtility.FromJson<ContinueData>(File.ReadAllText(stringBuilder.ToString()));
			if (continueData != null)
			{
				result = continueData.slot;
			}
		}
		return result;
	}

	public static void LoadGame(int slot, bool comingFromReloadCombat = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveGameName);
		stringBuilder.Append(slot);
		stringBuilder.Append(saveGameExtension);
		string path = stringBuilder.ToString();
		if (!File.Exists(path))
		{
			Debug.Log("ERROR File does not exists");
			return;
		}
		FileStream fileStream = new FileStream(path, FileMode.Open);
		if (fileStream.Length == 0L)
		{
			fileStream.Close();
			return;
		}
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		CryptoStream cryptoStream = null;
		try
		{
			cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
			(new BinaryFormatter().Deserialize(cryptoStream) as GameData).LoadData(comingFromReloadCombat);
			cryptoStream.Close();
		}
		catch (SerializationException ex)
		{
			Debug.Log("Failed to deserialize LoadGame. Reason: " + ex.Message);
		}
		fileStream.Close();
	}

	public static GameData GetGameData(int slot)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveGameName);
		stringBuilder.Append(slot);
		stringBuilder.Append(saveGameExtension);
		string path = stringBuilder.ToString();
		if (!File.Exists(path))
		{
			Debug.Log("ERROR File does not exists");
			return null;
		}
		FileStream fileStream = new FileStream(path, FileMode.Open);
		if (fileStream.Length == 0L)
		{
			fileStream.Close();
			return null;
		}
		GameData result = null;
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		CryptoStream cryptoStream = null;
		try
		{
			cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
			result = new BinaryFormatter().Deserialize(cryptoStream) as GameData;
			cryptoStream.Close();
		}
		catch (SerializationException ex)
		{
			Debug.Log("Failed to deserialize LoadGame. Reason: " + ex.Message);
		}
		fileStream.Close();
		return result;
	}

	public static void DeleteGame(int slot, bool sendTelemetry = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveGameName);
		stringBuilder.Append(slot);
		string path = stringBuilder.ToString() + saveGameExtension;
		if (File.Exists(path))
		{
			FileStream fileStream = new FileStream(path, FileMode.Open);
			if (fileStream.Length != 0L)
			{
				GameData gameData = null;
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = null;
				try
				{
					cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
					gameData = new BinaryFormatter().Deserialize(cryptoStream) as GameData;
					if (gameData != null && gameData.GameUniqueId != null && gameData.GameUniqueId != "" && sendTelemetry)
					{
						Telemetry.SendPlaysessionEnd(gameData);
						Telemetry.SendActEnd(gameData);
					}
					cryptoStream.Close();
				}
				catch (SerializationException ex)
				{
					Debug.Log("Failed to deserialize LoadGame. Reason: " + ex.Message);
				}
			}
			fileStream.Close();
			File.Delete(path);
		}
		path = stringBuilder.ToString() + saveGameExtensionBK;
		Debug.Log("REMOVED file " + path);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		DeleteSaveGameTurn(slot);
		RemoveGameContinue(slot);
	}

	public static GameData[] SaveGamesList()
	{
		GameData[] array = new GameData[48];
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Clear();
			stringBuilder.Append(Application.persistentDataPath);
			stringBuilder.Append("/");
			stringBuilder.Append(SteamManager.Instance.steamId);
			stringBuilder.Append("/");
			stringBuilder.Append(GameManager.Instance.ProfileFolder);
			stringBuilder.Append(saveGameName);
			stringBuilder.Append(i);
			bool flag = false;
			string text = "";
			for (int j = 0; j < 2; j++)
			{
				text = ((j != 0) ? (stringBuilder.ToString() + saveGameExtensionBK) : (stringBuilder.ToString() + saveGameExtension));
				if (!File.Exists(text))
				{
					continue;
				}
				FileStream fileStream;
				try
				{
					fileStream = new FileStream(text, FileMode.Open);
					if (fileStream.Length == 0L)
					{
						fileStream.Close();
						continue;
					}
				}
				catch
				{
					continue;
				}
				GameData gameData = null;
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = null;
				try
				{
					cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					try
					{
						gameData = binaryFormatter.Deserialize(cryptoStream) as GameData;
					}
					catch
					{
						fileStream.Close();
						goto end_IL_00f7;
					}
					if (gameData != null)
					{
						array[i] = gameData;
						flag = true;
					}
					cryptoStream.Close();
					goto IL_0163;
					end_IL_00f7:;
				}
				catch (SerializationException ex)
				{
					Debug.Log("Failed to deserialize LoadGame. Reason: " + ex.Message);
					goto IL_0163;
				}
				continue;
				IL_0163:
				fileStream.Close();
				if (flag)
				{
					if (j == 0)
					{
						break;
					}
					File.Copy(text, stringBuilder.ToString() + saveGameExtension, overwrite: true);
				}
			}
		}
		return array;
	}

	public static void DeleteSaveGameTurn(int slot)
	{
		string path = PathSaveGameTurn(slot);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static void SaveGameTurn(int slot)
	{
		string path = PathSaveGameTurn(slot);
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
		using CryptoStream cryptoStream = new CryptoStream(stream, dESCryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		GameTurnData gameTurnData = new GameTurnData();
		gameTurnData.FillData();
		binaryFormatter.Serialize(cryptoStream, gameTurnData);
		cryptoStream.FlushFinalBlock();
	}

	public static string LoadGameTurn(int slot)
	{
		string path = PathSaveGameTurn(slot);
		string result = "";
		if (File.Exists(path))
		{
			FileStream fileStream = new FileStream(path, FileMode.Open);
			if (fileStream.Length != 0L)
			{
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = null;
				try
				{
					cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
					(new BinaryFormatter().Deserialize(cryptoStream) as GameTurnData).LoadData();
					cryptoStream.Close();
				}
				catch (SerializationException ex)
				{
					if (GameManager.Instance.GetDeveloperMode())
					{
						Debug.Log("Failed to deserialize LoadGame. Reason: " + ex.Message);
					}
				}
			}
			fileStream.Close();
		}
		return result;
	}

	public static void CleanSavePlayerData()
	{
		SaveIntoPrefsInt("madnessLevel", 0);
		SaveIntoPrefsString("madnessCorruptors", "");
		SaveIntoPrefsInt("obeliskMadness", 0);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		string[] files = Directory.GetFiles(stringBuilder.ToString());
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(profileFileName);
		stringBuilder2.Append(saveGameExtension);
		string[] array = files;
		foreach (string text in array)
		{
			if (!text.Contains(stringBuilder2.ToString()))
			{
				File.Delete(text);
			}
		}
		GameManager.Instance.ReloadProfile();
		SavePlayerData();
	}

	public static void SavePlayerData(bool cleanThePlayerData = false, bool asBackup = false)
	{
		if (AtOManager.Instance.IsCombatTool || (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled))
		{
			return;
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("SavePlayerData");
		}
		PlayerData playerData;
		if (!cleanThePlayerData)
		{
			playerDataStatic = LoadPlayerData();
			playerData = ReBuildPlayerData(playerDataStatic);
		}
		else
		{
			playerData = new PlayerData();
			RestorePlayerData(playerData);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		if (!Directory.Exists(stringBuilder.ToString()))
		{
			Directory.CreateDirectory(stringBuilder.ToString());
		}
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		if (!Directory.Exists(stringBuilder.ToString()))
		{
			Directory.CreateDirectory(stringBuilder.ToString());
		}
		stringBuilder.Append(savePlayerName);
		if (asBackup)
		{
			stringBuilder.Append(backupName);
		}
		stringBuilder.Append(saveGameExtension);
		string path = stringBuilder.ToString();
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		using (CryptoStream cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
		{
			new BinaryFormatter().Serialize(cryptoStream, playerData);
			cryptoStream.Close();
		}
		fileStream.Close();
		Functions.DebugLogGD("SAVING PLAYER DATA", "save");
	}

	public static void SavePlayerDataBackup()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("SavePlayerDataBackup");
		}
		SavePlayerData(cleanThePlayerData: false, asBackup: true);
	}

	public static void BackupFromBackup()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("BackupFromBackup");
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(savePlayerName);
		stringBuilder.Append(backupName);
		string text = stringBuilder.ToString() + saveGameExtension;
		if (!File.Exists(text))
		{
			return;
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		int value = 0;
		for (int num = backupLimitFiles; num >= 0; num--)
		{
			stringBuilder2.Clear();
			stringBuilder2.Append(stringBuilder.ToString());
			stringBuilder2.Append("_");
			stringBuilder2.Append(num);
			stringBuilder2.Append(saveGameExtension);
			if (File.Exists(stringBuilder2.ToString()))
			{
				break;
			}
			value = num;
		}
		stringBuilder2.Clear();
		stringBuilder2.Append(stringBuilder.ToString());
		stringBuilder2.Append("_");
		stringBuilder2.Append(value);
		stringBuilder2.Append(saveGameExtension);
		File.Copy(text, stringBuilder2.ToString(), overwrite: true);
	}

	public static PlayerData LoadPlayerData(bool fromBackup = false)
	{
		if (fromBackup)
		{
			BackupFromBackup();
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("LoadPlayerData frombackup=>" + fromBackup);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(savePlayerName);
		if (fromBackup)
		{
			stringBuilder.Append(backupName);
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		PlayerData result = null;
		int num = -1;
		int num2 = -1;
		if (fromBackup)
		{
			num = backupLimitFiles;
		}
		for (int num3 = num; num3 >= num2; num3--)
		{
			stringBuilder2.Clear();
			stringBuilder2.Append(stringBuilder.ToString());
			if (num3 > -1)
			{
				stringBuilder2.Append("_");
				stringBuilder2.Append(num3);
			}
			stringBuilder2.Append(saveGameExtension);
			string text = stringBuilder2.ToString();
			if (File.Exists(text))
			{
				using FileStream fileStream = new FileStream(text, FileMode.Open);
				if (fileStream.Length != 0L)
				{
					PlayerData playerData = null;
					DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
					CryptoStream cryptoStream = null;
					try
					{
						cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						try
						{
							playerData = binaryFormatter.Deserialize(cryptoStream) as PlayerData;
						}
						catch (Exception ex)
						{
							Debug.LogWarning("AddCharges exception-> " + ex);
							if (GameManager.Instance.GetDeveloperMode())
							{
								Debug.Log("Corrupted Exception caught loading PlayerData from => " + text);
							}
							fileStream.Close();
							File.Delete(text);
							goto end_IL_010c;
						}
					}
					catch (SerializationException ex2)
					{
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.Log("Failed to deserialize PlayerData. Reason: " + ex2.Message);
						}
						fileStream.Close();
						goto end_IL_010c;
					}
					catch (DecoderFallbackException ex3)
					{
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.Log("DecoderFallbackException. Reason: " + ex3.Message);
						}
						fileStream.Close();
						goto end_IL_010c;
					}
					fileStream.Close();
					result = playerData;
					break;
				}
				fileStream.Close();
				end_IL_010c:;
			}
		}
		return result;
	}

	public static void SaveRuns(bool doBackup = false)
	{
		if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveRunsName);
		if (doBackup)
		{
			stringBuilder.Append(backupName);
		}
		stringBuilder.Append(saveGameExtension);
		string path = stringBuilder.ToString();
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		using (CryptoStream cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
		{
			new BinaryFormatter().Serialize(cryptoStream, PlayerManager.Instance.PlayerRuns);
			cryptoStream.Close();
		}
		fileStream.Close();
	}

	public static void LoadRuns(bool _useBackup = false)
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("LoadRuns " + _useBackup);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveRunsName);
		if (!_useBackup)
		{
			stringBuilder.Append(saveGameExtension);
		}
		else
		{
			stringBuilder.Append(backupName);
			stringBuilder.Append(saveGameExtension);
		}
		string text = stringBuilder.ToString();
		if (File.Exists(text))
		{
			bool flag = false;
			using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
			{
				if (fileStream.Length == 0L)
				{
					fileStream.Close();
					return;
				}
				DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
				CryptoStream cryptoStream = null;
				try
				{
					cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					try
					{
						PlayerManager.Instance.PlayerRuns = binaryFormatter.Deserialize(cryptoStream) as List<string>;
						cryptoStream.Close();
						flag = true;
					}
					catch (Exception ex)
					{
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.LogWarning("AddCharges exception-> " + ex);
						}
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.Log("Corrupted Exception caught loading Runs from => " + text);
						}
						if (!_useBackup)
						{
							fileStream.Close();
							LoadRuns(_useBackup: true);
						}
						else
						{
							fileStream.Close();
							SaveRuns();
						}
						return;
					}
				}
				catch (SerializationException ex2)
				{
					if (GameManager.Instance.GetDeveloperMode())
					{
						Debug.Log("Failed to deserialize Runs. Reason: " + ex2.Message);
					}
				}
				fileStream.Close();
			}
			if (flag)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.Log("We got Runs!");
				}
				if (_useBackup)
				{
					SaveRuns();
				}
				else
				{
					SaveRuns(doBackup: true);
				}
			}
		}
		else if (!_useBackup)
		{
			LoadRuns(_useBackup: true);
		}
		else
		{
			SaveRuns();
		}
	}

	public static void RemoveRuns()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveRunsName);
		if (File.Exists(stringBuilder.ToString()))
		{
			File.Delete(stringBuilder.ToString());
		}
	}

	public static void SavePlayerDeck()
	{
		if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		if (GameManager.Instance.IsSingularity())
		{
			stringBuilder.Append(saveDecksSingularityName);
		}
		else
		{
			stringBuilder.Append(saveDecksName);
		}
		stringBuilder.Append(saveGameExtension);
		string path = stringBuilder.ToString();
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		using (CryptoStream cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
		{
			new BinaryFormatter().Serialize(cryptoStream, PlayerManager.Instance.PlayerSavedDeck);
			cryptoStream.Close();
		}
		fileStream.Close();
	}

	public static void LoadPlayerDeck()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		if (GameManager.Instance.IsSingularity())
		{
			stringBuilder.Append(saveDecksSingularityName);
		}
		else
		{
			stringBuilder.Append(saveDecksName);
		}
		stringBuilder.Append(saveGameExtension);
		string text = stringBuilder.ToString();
		if (File.Exists(text))
		{
			using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
			{
				if (fileStream.Length == 0L)
				{
					fileStream.Close();
				}
				else
				{
					DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
					CryptoStream cryptoStream = null;
					try
					{
						cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						PlayerManager.Instance.PlayerSavedDeck = binaryFormatter.Deserialize(cryptoStream) as PlayerDeck;
						cryptoStream.Close();
					}
					catch (SerializationException ex)
					{
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.Log("Failed to deserialize Runs. Reason: " + ex.Message);
						}
					}
					fileStream.Close();
				}
				return;
			}
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("RunFile not found in " + text);
		}
	}

	public static void SavePlayerPerkConfig()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(savePerksName);
		stringBuilder.Append(saveGameExtension);
		string path = stringBuilder.ToString();
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		using (CryptoStream cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
		{
			new BinaryFormatter().Serialize(cryptoStream, PlayerManager.Instance.PlayerSavedPerk);
			cryptoStream.Close();
		}
		fileStream.Close();
	}

	public static void LoadPlayerPerkConfig()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(savePerksName);
		stringBuilder.Append(saveGameExtension);
		string text = stringBuilder.ToString();
		if (File.Exists(text))
		{
			using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
			{
				if (fileStream.Length == 0L)
				{
					fileStream.Close();
				}
				else
				{
					DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
					CryptoStream cryptoStream = null;
					try
					{
						cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						PlayerManager.Instance.PlayerSavedPerk = binaryFormatter.Deserialize(cryptoStream) as PlayerPerk;
						cryptoStream.Close();
					}
					catch (SerializationException ex)
					{
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.Log("Failed to deserialize PlayerPerks. Reason: " + ex.Message);
						}
					}
					fileStream.Close();
				}
				return;
			}
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("PlayerPerks not found in " + text);
		}
	}

	public static void SaveMutedPlayers()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(saveMutedName);
		stringBuilder.Append(saveGameExtension);
		string path = stringBuilder.ToString();
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		using (CryptoStream cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
		{
			new BinaryFormatter().Serialize(cryptoStream, NetworkManager.Instance.PlayerMuteList);
			cryptoStream.Close();
		}
		fileStream.Close();
	}

	public static void LoadMutedPlayers()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(saveMutedName);
		stringBuilder.Append(saveGameExtension);
		string text = stringBuilder.ToString();
		if (File.Exists(text))
		{
			using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
			{
				if (fileStream.Length == 0L)
				{
					fileStream.Close();
				}
				else
				{
					DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
					CryptoStream cryptoStream = null;
					try
					{
						cryptoStream = new CryptoStream(fileStream, dESCryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						NetworkManager.Instance.PlayerMuteList = binaryFormatter.Deserialize(cryptoStream) as List<string>;
						cryptoStream.Close();
					}
					catch (SerializationException ex)
					{
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.Log("Failed to deserialize MutedPlayers. Reason: " + ex.Message);
						}
					}
					fileStream.Close();
				}
				return;
			}
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("MutedPlayers not found in " + text);
		}
	}

	public static void ResetTutorial()
	{
		PlayerManager.Instance.TutorialWatched = new List<string>();
		SavePlayerData();
	}

	public static void RestorePlayerData(PlayerData playerData)
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("RestorePlayerData");
		}
		string text = SteamManager.Instance.steamId.ToString();
		if (playerData.SteamUserId == null)
		{
			playerData.SteamUserId = text;
		}
		if (playerData.SteamUserId != text && !GameManager.Instance.GetDeveloperMode())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Application.persistentDataPath);
			stringBuilder.Append("/");
			stringBuilder.Append(SteamManager.Instance.steamId);
			stringBuilder.Append("/");
			stringBuilder.Append(GameManager.Instance.ProfileFolder);
			stringBuilder.Append(savePlayerName);
			string text2 = stringBuilder.ToString();
			File.Move(text2 + saveGameExtension, text2 + "-error-" + Functions.GetTimestamp() + saveGameExtension);
			return;
		}
		PlayerManager.Instance.LastUsedTeam = playerData.LastUsedTeam;
		PlayerManager.Instance.TutorialWatched = playerData.TutorialWatched;
		if (playerData.UnlockedHeroes != null)
		{
			PlayerManager.Instance.UnlockedHeroes = playerData.UnlockedHeroes;
		}
		else
		{
			PlayerManager.Instance.UnlockedHeroes = new List<string>();
		}
		if (playerData.UnlockedCards != null)
		{
			PlayerManager.Instance.UnlockedCards = playerData.UnlockedCards;
		}
		else
		{
			PlayerManager.Instance.UnlockedCards = new List<string>();
		}
		if (playerData.UnlockedNodes != null)
		{
			PlayerManager.Instance.UnlockedNodes = playerData.UnlockedNodes;
		}
		else
		{
			PlayerManager.Instance.UnlockedNodes = new List<string>();
		}
		if (playerData.TreasuresClaimed != null)
		{
			PlayerManager.Instance.TreasuresClaimed = playerData.TreasuresClaimed;
		}
		else
		{
			PlayerManager.Instance.TreasuresClaimed = new List<string>();
		}
		if (playerData.UnlockedCardsByGame != null)
		{
			PlayerManager.Instance.UnlockedCardsByGame = playerData.UnlockedCardsByGame;
		}
		else
		{
			PlayerManager.Instance.UnlockedCardsByGame = new Dictionary<string, List<string>>();
		}
		_ = playerData.NgUnlocked;
		PlayerManager.Instance.NgUnlocked = playerData.NgUnlocked;
		_ = playerData.NgLevel;
		PlayerManager.Instance.NgLevel = playerData.NgLevel;
		_ = playerData.PlayerRankProgress;
		PlayerManager.Instance.SetPlayerRankProgress(playerData.PlayerRankProgress);
		_ = playerData.MaxAdventureMadnessLevel;
		PlayerManager.Instance.MaxAdventureMadnessLevel = playerData.MaxAdventureMadnessLevel;
		if (PlayerManager.Instance.MaxAdventureMadnessLevel == 0)
		{
			PlayerManager.Instance.MaxAdventureMadnessLevel = PlayerManager.Instance.NgLevel;
		}
		_ = playerData.ObeliskMadnessLevel;
		PlayerManager.Instance.ObeliskMadnessLevel = playerData.ObeliskMadnessLevel;
		_ = playerData.SingularityMadnessLevel;
		PlayerManager.Instance.SingularityMadnessLevel = playerData.SingularityMadnessLevel;
		PlayerManager.Instance.BossesKilled = playerData.BossesKilled;
		PlayerManager.Instance.BossesKilledName = playerData.BossesKilledName;
		PlayerManager.Instance.MonstersKilled = playerData.MonstersKilled;
		PlayerManager.Instance.ExpGained = playerData.ExpGained;
		PlayerManager.Instance.CardsCrafted = playerData.CardsCrafted;
		PlayerManager.Instance.CardsUpgraded = playerData.CardsUpgraded;
		PlayerManager.Instance.GoldGained = playerData.GoldGained;
		PlayerManager.Instance.DustGained = playerData.DustGained;
		PlayerManager.Instance.BestScore = playerData.BestScore;
		PlayerManager.Instance.PurchasedItems = playerData.PurchasedItems;
		PlayerManager.Instance.CorruptionsCompleted = playerData.CorruptionsCompleted;
		PlayerManager.Instance.SupplyGained = playerData.SupplyGained;
		PlayerManager.Instance.SupplyActual = playerData.SupplyActual;
		if (playerData.HeroProgress != null)
		{
			foreach (KeyValuePair<string, int> item in playerData.HeroProgress)
			{
				PlayerManager.Instance.HeroProgress.Add(item.Key, item.Value);
			}
		}
		PlayerManager.Instance.HeroPerks = playerData.HeroPerks;
		if (PlayerManager.Instance.HeroPerks != null)
		{
			foreach (KeyValuePair<string, List<string>> heroPerk in PlayerManager.Instance.HeroPerks)
			{
				for (int num = heroPerk.Value.Count - 1; num >= 0; num--)
				{
					PerkData perkData = Globals.Instance.GetPerkData(heroPerk.Value[num]);
					if (!(perkData == null))
					{
						_ = perkData.MainPerk;
						if (perkData.MainPerk)
						{
							continue;
						}
					}
					heroPerk.Value.RemoveAt(num);
				}
			}
		}
		PlayerManager.Instance.SupplyBought = playerData.SupplyBought;
		if (playerData.SkinUsed != null)
		{
			PlayerManager.Instance.SkinUsed = playerData.SkinUsed;
		}
		else
		{
			PlayerManager.Instance.SkinUsed = new Dictionary<string, string>();
		}
		if (playerData.CardbackUsed != null)
		{
			PlayerManager.Instance.CardbackUsed = playerData.CardbackUsed;
		}
		else
		{
			PlayerManager.Instance.CardbackUsed = new Dictionary<string, string>();
		}
		if (GameManager.Instance.GetDeveloperMode() || GameManager.Instance.UnlockMadness)
		{
			PlayerManager.Instance.NgLevel = 9;
			PlayerManager.Instance.ObeliskMadnessLevel = 10;
		}
		LoadRuns();
	}

	private static PlayerData ReBuildPlayerData(PlayerData _playerData)
	{
		if (_playerData == null)
		{
			_playerData = new PlayerData();
		}
		_playerData.LastUsedTeam = PlayerManager.Instance.LastUsedTeam;
		_playerData.TutorialWatched = PlayerManager.Instance.TutorialWatched;
		_playerData.UnlockedHeroes = PlayerManager.Instance.UnlockedHeroes;
		_playerData.UnlockedCards = PlayerManager.Instance.UnlockedCards;
		_playerData.UnlockedNodes = PlayerManager.Instance.UnlockedNodes;
		_playerData.PlayerRuns = PlayerManager.Instance.PlayerRuns;
		_playerData.TreasuresClaimed = PlayerManager.Instance.TreasuresClaimed;
		_playerData.UnlockedCardsByGame = PlayerManager.Instance.UnlockedCardsByGame;
		_playerData.NgUnlocked = PlayerManager.Instance.NgUnlocked;
		_playerData.NgLevel = PlayerManager.Instance.NgLevel;
		_playerData.PlayerRankProgress = PlayerManager.Instance.GetPlayerRankProgress();
		_playerData.MaxAdventureMadnessLevel = PlayerManager.Instance.MaxAdventureMadnessLevel;
		_playerData.ObeliskMadnessLevel = PlayerManager.Instance.ObeliskMadnessLevel;
		_playerData.SingularityMadnessLevel = PlayerManager.Instance.SingularityMadnessLevel;
		_playerData.BossesKilled = PlayerManager.Instance.BossesKilled;
		_playerData.BossesKilledName = PlayerManager.Instance.BossesKilledName;
		_playerData.MonstersKilled = PlayerManager.Instance.MonstersKilled;
		_playerData.ExpGained = PlayerManager.Instance.ExpGained;
		_playerData.CardsCrafted = PlayerManager.Instance.CardsCrafted;
		_playerData.CardsUpgraded = PlayerManager.Instance.CardsUpgraded;
		_playerData.GoldGained = PlayerManager.Instance.GoldGained;
		_playerData.DustGained = PlayerManager.Instance.DustGained;
		_playerData.BestScore = PlayerManager.Instance.BestScore;
		_playerData.PurchasedItems = PlayerManager.Instance.PurchasedItems;
		_playerData.CorruptionsCompleted = PlayerManager.Instance.CorruptionsCompleted;
		_playerData.SupplyGained = PlayerManager.Instance.SupplyGained;
		_playerData.SupplyActual = PlayerManager.Instance.SupplyActual;
		_playerData.HeroProgress = PlayerManager.Instance.HeroProgress;
		_playerData.HeroPerks = PlayerManager.Instance.HeroPerks;
		_playerData.SupplyBought = PlayerManager.Instance.SupplyBought;
		_playerData.SkinUsed = PlayerManager.Instance.SkinUsed;
		_playerData.CardbackUsed = PlayerManager.Instance.CardbackUsed;
		return _playerData;
	}

	public static void SaveRun(PlayerRun _playerRun)
	{
		List<string> list = PlayerManager.Instance.PlayerRuns;
		if (list == null)
		{
			list = new List<string>();
		}
		string item = JsonUtility.ToJson(_playerRun);
		list.Add(item);
		PlayerManager.Instance.PlayerRuns = list;
		if (list.Count > 4)
		{
			PlayerManager.Instance.AchievementUnlock("MISC_ADVENTURER");
		}
		if (_playerRun.TotalPlayers > 1)
		{
			PlayerManager.Instance.AchievementUnlock("MISC_ADVENTURERGUILD");
		}
		SaveRuns();
	}

	public static void DeleteDirectory(string target_dir)
	{
		string[] files = Directory.GetFiles(target_dir);
		string[] directories = Directory.GetDirectories(target_dir);
		string[] array = files;
		foreach (string path in array)
		{
			File.SetAttributes(path, FileAttributes.Normal);
			File.Delete(path);
		}
		array = directories;
		for (int i = 0; i < array.Length; i++)
		{
			DeleteDirectory(array[i]);
		}
		Directory.Delete(target_dir, recursive: false);
	}

	public static string PathSaveGameTurn(int slot)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append(SteamManager.Instance.steamId);
		stringBuilder.Append("/");
		stringBuilder.Append(GameManager.Instance.ProfileFolder);
		stringBuilder.Append(saveGameName);
		stringBuilder.Append(slot);
		stringBuilder.Append(saveGameTurnExtension);
		stringBuilder.Append(saveGameExtension);
		return stringBuilder.ToString();
	}

	public static bool PrefsHasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void PrefsRemoveKey(string key)
	{
		PlayerPrefs.DeleteKey(key);
	}

	public static void SavePrefs()
	{
		PlayerPrefs.Save();
	}

	public static void SaveIntoPrefsInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	public static int LoadPrefsInt(string key)
	{
		return PlayerPrefs.GetInt(key);
	}

	public static void SaveIntoPrefsFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	public static float LoadPrefsFloat(string key)
	{
		return PlayerPrefs.GetFloat(key);
	}

	public static void SaveIntoPrefsBool(string key, bool value)
	{
		PlayerPrefs.SetInt(key, value ? 1 : 0);
	}

	public static bool LoadPrefsBool(string key)
	{
		return PlayerPrefs.GetInt(key) == 1;
	}

	public static void SaveIntoPrefsString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	public static string LoadPrefsString(string key)
	{
		return PlayerPrefs.GetString(key);
	}
}
