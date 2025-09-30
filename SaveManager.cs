// Decompiled with JetBrains decompiler
// Type: SaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Paradox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

#nullable disable
public static class SaveManager
{
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
  private static byte[] key = new byte[8]
  {
    (byte) 18,
    (byte) 54,
    (byte) 100,
    (byte) 160,
    (byte) 190,
    (byte) 148,
    (byte) 136,
    (byte) 3
  };
  private static byte[] iv = new byte[8]
  {
    (byte) 82,
    (byte) 242,
    (byte) 164,
    (byte) 132,
    (byte) 119,
    (byte) 197,
    (byte) 179,
    (byte) 20
  };
  private static PlayerData playerDataStatic;
  private static int backupLimitFiles = 50;

  public static bool ExistsProfileFolder(int _slot)
  {
    if (_slot > 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(Application.persistentDataPath);
      stringBuilder.Append("/");
      stringBuilder.Append((ulong) SteamManager.Instance.steamId);
      stringBuilder.Append("/");
      stringBuilder.Append("profile");
      stringBuilder.Append(_slot);
      if (!Directory.Exists(stringBuilder.ToString()))
        return false;
    }
    return true;
  }

  public static void CreateProfileFolder(int _slot, string _name)
  {
    if (_slot < 1 || _slot > 4 || _name == "")
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append("profile");
    stringBuilder.Append(_slot);
    if (Directory.Exists(stringBuilder.ToString()))
      SaveManager.DeleteDirectory(stringBuilder.ToString());
    Directory.CreateDirectory(stringBuilder.ToString());
    if (!Directory.Exists(stringBuilder.ToString()))
      return;
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.profileFileName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    File.WriteAllText(stringBuilder.ToString(), _name);
    MainMenuManager.Instance.LoadProfiles();
    MainMenuManager.Instance.UseProfile(_slot);
  }

  public static void DeleteProfileFolder(int _slot)
  {
    if (_slot < 1 || _slot > 4)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append("profile");
    stringBuilder.Append(_slot);
    if (!Directory.Exists(stringBuilder.ToString()))
      return;
    SaveManager.DeleteDirectory(stringBuilder.ToString());
  }

  public static string[] GetProfileNames()
  {
    string[] profileNames = new string[5];
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append(Application.persistentDataPath);
    stringBuilder1.Append("/");
    stringBuilder1.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder1.Append("/");
    for (int index = 0; index < 5; ++index)
    {
      if (index == 0)
      {
        profileNames[index] = Texts.Instance.GetText("default");
      }
      else
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append(stringBuilder1.ToString());
        stringBuilder2.Append("profile");
        stringBuilder2.Append(index);
        if (!Directory.Exists(stringBuilder2.ToString()))
        {
          profileNames[index] = "";
        }
        else
        {
          stringBuilder2.Append("/");
          stringBuilder2.Append(SaveManager.profileFileName);
          stringBuilder2.Append(SaveManager.saveGameExtension);
          if (!File.Exists(stringBuilder2.ToString()))
          {
            profileNames[index] = "";
          }
          else
          {
            FileStream fileStream = new FileStream(stringBuilder2.ToString(), FileMode.Open);
            if (fileStream.Length == 0L)
            {
              fileStream.Close();
              profileNames[index] = "";
            }
            else
            {
              string end = new StreamReader((Stream) fileStream, Encoding.UTF8).ReadToEnd();
              profileNames[index] = end;
              fileStream.Close();
            }
          }
        }
      }
    }
    return profileNames;
  }

  public static bool GameSaveSlotExists(int slot, string profileFolder = "")
  {
    if (profileFolder == "")
      profileFolder = GameManager.Instance.ProfileFolder;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(profileFolder);
    stringBuilder.Append(SaveManager.saveGameName);
    stringBuilder.Append(slot);
    stringBuilder.Append(SaveManager.saveGameExtension);
    return File.Exists(stringBuilder.ToString());
  }

  public static void SaveGame(int slot, bool backUp = false)
  {
    if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
      slot = -1;
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) ("******* SAVE GAME (" + slot.ToString() + ") *******"));
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append(Application.persistentDataPath);
    stringBuilder1.Append("/");
    stringBuilder1.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder1.Append("/");
    stringBuilder1.Append(GameManager.Instance.ProfileFolder);
    stringBuilder1.Append(SaveManager.saveGameName);
    stringBuilder1.Append(slot);
    StringBuilder stringBuilder2 = new StringBuilder();
    stringBuilder2.Append(stringBuilder1.ToString());
    stringBuilder2.Append(SaveManager.saveGameExtensionBK);
    stringBuilder1.Append(SaveManager.saveGameExtension);
    string str = stringBuilder1.ToString();
    string destFileName = stringBuilder2.ToString();
    if (backUp && File.Exists(str))
      File.Copy(str, destFileName, true);
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    bool newGame = false;
    try
    {
      FileStream fileStream = new FileStream(str, FileMode.Create, FileAccess.Write);
      using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateEncryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Write))
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        GameData gameData = new GameData();
        gameData.FillData(newGame);
        CryptoStream serializationStream = cryptoStream;
        GameData graph = gameData;
        binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
        cryptoStream.Close();
      }
      fileStream.Close();
      SaveManager.SaveGameContinue(slot);
    }
    catch
    {
      GameManager.Instance.AbortGameSave();
    }
  }

  public static void RemoveGameContinue(int slot = -1)
  {
    if (slot > -1 && SaveManager.GetSlotFromContinue() != slot)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.saveGameContinueName);
    if (!File.Exists(stringBuilder.ToString()))
      return;
    File.Delete(stringBuilder.ToString());
  }

  private static void SaveGameContinue(int slot)
  {
    if (GameManager.Instance.IsMultiplayer())
      return;
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) ("******* SAVE CONTINUE GAME (" + slot.ToString() + ") *******"));
    if (!((UnityEngine.Object) Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode) != (UnityEngine.Object) null))
      return;
    string nodeName = Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeName;
    string text;
    if (slot >= 0 && slot <= 11)
      text = Texts.Instance.GetText("modeAdventure");
    else if (slot >= 12 && slot <= 23)
      text = Texts.Instance.GetText("modeObelisk");
    else if (slot >= 24 && slot <= 35)
    {
      text = Texts.Instance.GetText("modeWeekly");
    }
    else
    {
      if (slot < 36 || slot > 47)
        return;
      text = Texts.Instance.GetText("singularity");
    }
    SaveManager.ContinueData continueData = new SaveManager.ContinueData();
    continueData.title = nodeName;
    continueData.desc = text;
    continueData.date = DateTime.Now.ToString();
    continueData.rawGameVersion = GameManager.Instance.gameVersion;
    continueData.slot = slot;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.saveGameContinueName);
    File.WriteAllText(stringBuilder.ToString(), JsonUtility.ToJson((object) continueData));
  }

  public static int GetSlotFromContinue()
  {
    int slotFromContinue = -1;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.saveGameContinueName);
    if (File.Exists(stringBuilder.ToString()))
    {
      SaveManager.ContinueData continueData = JsonUtility.FromJson<SaveManager.ContinueData>(File.ReadAllText(stringBuilder.ToString()));
      if (continueData != null)
        slotFromContinue = continueData.slot;
    }
    return slotFromContinue;
  }

  public static void LoadGame(int slot, bool comingFromReloadCombat = false)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    stringBuilder.Append(SaveManager.saveGameName);
    stringBuilder.Append(slot);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    if (!File.Exists(path))
    {
      Debug.Log((object) "ERROR File does not exists");
    }
    else
    {
      FileStream fileStream = new FileStream(path, FileMode.Open);
      if (fileStream.Length == 0L)
      {
        fileStream.Close();
      }
      else
      {
        DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
        try
        {
          CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
          (new BinaryFormatter().Deserialize((Stream) serializationStream) as GameData).LoadData(comingFromReloadCombat);
          serializationStream.Close();
        }
        catch (SerializationException ex)
        {
          Debug.Log((object) ("Failed to deserialize LoadGame. Reason: " + ex.Message));
        }
        fileStream.Close();
      }
    }
  }

  public static GameData GetGameData(int slot)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    stringBuilder.Append(SaveManager.saveGameName);
    stringBuilder.Append(slot);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    if (!File.Exists(path))
    {
      Debug.Log((object) "ERROR File does not exists");
      return (GameData) null;
    }
    FileStream fileStream = new FileStream(path, FileMode.Open);
    if (fileStream.Length == 0L)
    {
      fileStream.Close();
      return (GameData) null;
    }
    GameData gameData = (GameData) null;
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    try
    {
      CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
      gameData = new BinaryFormatter().Deserialize((Stream) serializationStream) as GameData;
      serializationStream.Close();
    }
    catch (SerializationException ex)
    {
      Debug.Log((object) ("Failed to deserialize LoadGame. Reason: " + ex.Message));
    }
    fileStream.Close();
    return gameData;
  }

  public static void DeleteGame(int slot, bool sendTelemetry = false)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    stringBuilder.Append(SaveManager.saveGameName);
    stringBuilder.Append(slot);
    string path1 = stringBuilder.ToString() + SaveManager.saveGameExtension;
    if (File.Exists(path1))
    {
      FileStream fileStream = new FileStream(path1, FileMode.Open);
      if (fileStream.Length != 0L)
      {
        DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
        try
        {
          CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
          if (((!(new BinaryFormatter().Deserialize((Stream) serializationStream) is GameData gameDataSave) || gameDataSave.GameUniqueId == null ? 0 : (gameDataSave.GameUniqueId != "" ? 1 : 0)) & (sendTelemetry ? 1 : 0)) != 0)
            Telemetry.SendPlaysessionEnd(gameDataSave);
          serializationStream.Close();
        }
        catch (SerializationException ex)
        {
          Debug.Log((object) ("Failed to deserialize LoadGame. Reason: " + ex.Message));
        }
      }
      fileStream.Close();
      File.Delete(path1);
    }
    string path2 = stringBuilder.ToString() + SaveManager.saveGameExtensionBK;
    Debug.Log((object) ("REMOVED file " + path2));
    if (File.Exists(path2))
      File.Delete(path2);
    SaveManager.DeleteSaveGameTurn(slot);
    SaveManager.RemoveGameContinue(slot);
  }

  public static GameData[] SaveGamesList()
  {
    GameData[] gameDataArray = new GameData[48];
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = 0; index1 < gameDataArray.Length; ++index1)
    {
      stringBuilder.Clear();
      stringBuilder.Append(Application.persistentDataPath);
      stringBuilder.Append("/");
      stringBuilder.Append((ulong) SteamManager.Instance.steamId);
      stringBuilder.Append("/");
      stringBuilder.Append(GameManager.Instance.ProfileFolder);
      stringBuilder.Append(SaveManager.saveGameName);
      stringBuilder.Append(index1);
      bool flag = false;
      for (int index2 = 0; index2 < 2; ++index2)
      {
        string str = index2 != 0 ? stringBuilder.ToString() + SaveManager.saveGameExtensionBK : stringBuilder.ToString() + SaveManager.saveGameExtension;
        if (File.Exists(str))
        {
          FileStream fileStream;
          try
          {
            fileStream = new FileStream(str, FileMode.Open);
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
          DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
          try
          {
            CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GameData gameData;
            try
            {
              gameData = binaryFormatter.Deserialize((Stream) serializationStream) as GameData;
            }
            catch
            {
              fileStream.Close();
              continue;
            }
            if (gameData != null)
            {
              gameDataArray[index1] = gameData;
              flag = true;
            }
            serializationStream.Close();
          }
          catch (SerializationException ex)
          {
            Debug.Log((object) ("Failed to deserialize LoadGame. Reason: " + ex.Message));
          }
          fileStream.Close();
          if (flag)
          {
            if (index2 != 0)
              File.Copy(str, stringBuilder.ToString() + SaveManager.saveGameExtension, true);
            else
              break;
          }
        }
      }
    }
    return gameDataArray;
  }

  public static void DeleteSaveGameTurn(int slot)
  {
    string path = SaveManager.PathSaveGameTurn(slot);
    if (!File.Exists(path))
      return;
    File.Delete(path);
  }

  public static void SaveGameTurn(int slot)
  {
    if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
      return;
    string path = SaveManager.PathSaveGameTurn(slot);
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    try
    {
      FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
      using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateEncryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Write))
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        GameTurnData gameTurnData = new GameTurnData();
        gameTurnData.FillData();
        CryptoStream serializationStream = cryptoStream;
        GameTurnData graph = gameTurnData;
        binaryFormatter.Serialize((Stream) serializationStream, (object) graph);
        cryptoStream.Close();
      }
      fileStream.Close();
    }
    catch
    {
    }
  }

  public static string LoadGameTurn(int slot)
  {
    string path = SaveManager.PathSaveGameTurn(slot);
    string str = "";
    if (File.Exists(path))
    {
      FileStream fileStream = new FileStream(path, FileMode.Open);
      if (fileStream.Length != 0L)
      {
        DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
        try
        {
          CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
          (new BinaryFormatter().Deserialize((Stream) serializationStream) as GameTurnData).LoadData();
          serializationStream.Close();
        }
        catch (SerializationException ex)
        {
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Failed to deserialize LoadGame. Reason: " + ex.Message));
        }
      }
      fileStream.Close();
    }
    return str;
  }

  public static void CleanSavePlayerData()
  {
    SaveManager.SaveIntoPrefsInt("madnessLevel", 0);
    SaveManager.SaveIntoPrefsString("madnessCorruptors", "");
    SaveManager.SaveIntoPrefsInt("obeliskMadness", 0);
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append(Application.persistentDataPath);
    stringBuilder1.Append("/");
    stringBuilder1.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder1.Append("/");
    stringBuilder1.Append(GameManager.Instance.ProfileFolder);
    string[] files = Directory.GetFiles(stringBuilder1.ToString());
    StringBuilder stringBuilder2 = new StringBuilder();
    stringBuilder2.Append(SaveManager.profileFileName);
    stringBuilder2.Append(SaveManager.saveGameExtension);
    foreach (string path in files)
    {
      if (!path.Contains(stringBuilder2.ToString()))
        File.Delete(path);
    }
    GameManager.Instance.ReloadProfile();
    SaveManager.SavePlayerData();
  }

  public static void SavePlayerData(bool cleanThePlayerData = false, bool asBackup = false)
  {
    if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
      return;
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) nameof (SavePlayerData));
    PlayerData playerData;
    if (!cleanThePlayerData)
    {
      SaveManager.playerDataStatic = SaveManager.LoadPlayerData();
      playerData = SaveManager.ReBuildPlayerData(SaveManager.playerDataStatic);
    }
    else
    {
      playerData = new PlayerData();
      SaveManager.RestorePlayerData(playerData);
    }
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    if (!Directory.Exists(stringBuilder.ToString()))
      Directory.CreateDirectory(stringBuilder.ToString());
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    if (!Directory.Exists(stringBuilder.ToString()))
      Directory.CreateDirectory(stringBuilder.ToString());
    stringBuilder.Append(SaveManager.savePlayerName);
    if (asBackup)
      stringBuilder.Append(SaveManager.backupName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
      using (CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateEncryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Write))
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) playerData);
        serializationStream.Close();
      }
      fileStream.Close();
      Functions.DebugLogGD("SAVING PLAYER DATA", "save");
    }
  }

  public static void SavePlayerDataBackup()
  {
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) nameof (SavePlayerDataBackup));
    SaveManager.SavePlayerData(asBackup: true);
  }

  public static void BackupFromBackup()
  {
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) nameof (BackupFromBackup));
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append(Application.persistentDataPath);
    stringBuilder1.Append("/");
    stringBuilder1.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder1.Append("/");
    stringBuilder1.Append(GameManager.Instance.ProfileFolder);
    stringBuilder1.Append(SaveManager.savePlayerName);
    stringBuilder1.Append(SaveManager.backupName);
    string str = stringBuilder1.ToString() + SaveManager.saveGameExtension;
    if (!File.Exists(str))
      return;
    StringBuilder stringBuilder2 = new StringBuilder();
    int num = 0;
    for (int backupLimitFiles = SaveManager.backupLimitFiles; backupLimitFiles >= 0; --backupLimitFiles)
    {
      stringBuilder2.Clear();
      stringBuilder2.Append(stringBuilder1.ToString());
      stringBuilder2.Append("_");
      stringBuilder2.Append(backupLimitFiles);
      stringBuilder2.Append(SaveManager.saveGameExtension);
      if (!File.Exists(stringBuilder2.ToString()))
        num = backupLimitFiles;
      else
        break;
    }
    stringBuilder2.Clear();
    stringBuilder2.Append(stringBuilder1.ToString());
    stringBuilder2.Append("_");
    stringBuilder2.Append(num);
    stringBuilder2.Append(SaveManager.saveGameExtension);
    File.Copy(str, stringBuilder2.ToString(), true);
  }

  public static PlayerData LoadPlayerData(bool fromBackup = false)
  {
    if (fromBackup)
      SaveManager.BackupFromBackup();
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) ("LoadPlayerData frombackup=>" + fromBackup.ToString()));
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append(Application.persistentDataPath);
    stringBuilder1.Append("/");
    stringBuilder1.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder1.Append("/");
    stringBuilder1.Append(GameManager.Instance.ProfileFolder);
    stringBuilder1.Append(SaveManager.savePlayerName);
    if (fromBackup)
      stringBuilder1.Append(SaveManager.backupName);
    StringBuilder stringBuilder2 = new StringBuilder();
    PlayerData playerData1 = (PlayerData) null;
    int num1 = -1;
    int num2 = -1;
    if (fromBackup)
      num1 = SaveManager.backupLimitFiles;
    for (int index = num1; index >= num2; --index)
    {
      stringBuilder2.Clear();
      stringBuilder2.Append(stringBuilder1.ToString());
      if (index > -1)
      {
        stringBuilder2.Append("_");
        stringBuilder2.Append(index);
      }
      stringBuilder2.Append(SaveManager.saveGameExtension);
      string path = stringBuilder2.ToString();
      if (File.Exists(path))
      {
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
          if (fileStream.Length == 0L)
          {
            fileStream.Close();
          }
          else
          {
            DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
            PlayerData playerData2;
            try
            {
              CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
              BinaryFormatter binaryFormatter = new BinaryFormatter();
              try
              {
                playerData2 = binaryFormatter.Deserialize((Stream) serializationStream) as PlayerData;
              }
              catch (Exception ex)
              {
                Debug.LogWarning((object) ("AddCharges exception-> " + ex?.ToString()));
                if (GameManager.Instance.GetDeveloperMode())
                  Debug.Log((object) ("Corrupted Exception caught loading PlayerData from => " + path));
                fileStream.Close();
                File.Delete(path);
                continue;
              }
            }
            catch (SerializationException ex)
            {
              if (GameManager.Instance.GetDeveloperMode())
                Debug.Log((object) ("Failed to deserialize PlayerData. Reason: " + ex.Message));
              fileStream.Close();
              continue;
            }
            catch (DecoderFallbackException ex)
            {
              if (GameManager.Instance.GetDeveloperMode())
                Debug.Log((object) ("DecoderFallbackException. Reason: " + ex.Message));
              fileStream.Close();
              continue;
            }
            fileStream.Close();
            playerData1 = playerData2;
            break;
          }
        }
      }
    }
    return playerData1;
  }

  public static void SaveRuns(bool doBackup = false)
  {
    if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    stringBuilder.Append(SaveManager.saveRunsName);
    if (doBackup)
      stringBuilder.Append(SaveManager.backupName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
      using (CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateEncryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Write))
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) PlayerManager.Instance.PlayerRuns);
        serializationStream.Close();
      }
      fileStream.Close();
    }
  }

  public static void LoadRuns(bool _useBackup = false)
  {
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) ("LoadRuns " + _useBackup.ToString()));
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    stringBuilder.Append(SaveManager.saveRunsName);
    if (!_useBackup)
    {
      stringBuilder.Append(SaveManager.saveGameExtension);
    }
    else
    {
      stringBuilder.Append(SaveManager.backupName);
      stringBuilder.Append(SaveManager.saveGameExtension);
    }
    string path = stringBuilder.ToString();
    if (File.Exists(path))
    {
      bool flag = false;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        if (fileStream.Length == 0L)
        {
          fileStream.Close();
          return;
        }
        DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
        try
        {
          CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          try
          {
            PlayerManager.Instance.PlayerRuns = binaryFormatter.Deserialize((Stream) serializationStream) as List<string>;
            serializationStream.Close();
            flag = true;
          }
          catch (Exception ex)
          {
            if (GameManager.Instance.GetDeveloperMode())
              Debug.LogWarning((object) ("AddCharges exception-> " + ex?.ToString()));
            if (GameManager.Instance.GetDeveloperMode())
              Debug.Log((object) ("Corrupted Exception caught loading Runs from => " + path));
            if (!_useBackup)
            {
              fileStream.Close();
              SaveManager.LoadRuns(true);
              return;
            }
            fileStream.Close();
            SaveManager.SaveRuns();
            return;
          }
        }
        catch (SerializationException ex)
        {
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Failed to deserialize Runs. Reason: " + ex.Message));
        }
        fileStream.Close();
      }
      if (!flag)
        return;
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) "We got Runs!");
      if (_useBackup)
        SaveManager.SaveRuns();
      else
        SaveManager.SaveRuns(true);
    }
    else if (!_useBackup)
      SaveManager.LoadRuns(true);
    else
      SaveManager.SaveRuns();
  }

  public static void RemoveRuns()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    stringBuilder.Append(SaveManager.saveRunsName);
    if (!File.Exists(stringBuilder.ToString()))
      return;
    File.Delete(stringBuilder.ToString());
  }

  public static void SavePlayerDeck()
  {
    if (GameManager.Instance.CheatMode && GameManager.Instance.IsSaveDisabled)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    if (GameManager.Instance.IsSingularity())
      stringBuilder.Append(SaveManager.saveDecksSingularityName);
    else
      stringBuilder.Append(SaveManager.saveDecksName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
      using (CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateEncryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Write))
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) PlayerManager.Instance.PlayerSavedDeck);
        serializationStream.Close();
      }
      fileStream.Close();
    }
  }

  public static void LoadPlayerDeck()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    if (GameManager.Instance.IsSingularity())
      stringBuilder.Append(SaveManager.saveDecksSingularityName);
    else
      stringBuilder.Append(SaveManager.saveDecksName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    if (File.Exists(path))
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        if (fileStream.Length == 0L)
        {
          fileStream.Close();
        }
        else
        {
          DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
          try
          {
            CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
            PlayerManager.Instance.PlayerSavedDeck = new BinaryFormatter().Deserialize((Stream) serializationStream) as PlayerDeck;
            serializationStream.Close();
          }
          catch (SerializationException ex)
          {
            if (GameManager.Instance.GetDeveloperMode())
              Debug.Log((object) ("Failed to deserialize Runs. Reason: " + ex.Message));
          }
          fileStream.Close();
        }
      }
    }
    else
    {
      if (!GameManager.Instance.GetDeveloperMode())
        return;
      Debug.Log((object) ("RunFile not found in " + path));
    }
  }

  public static void SavePlayerPerkConfig()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.savePerksName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
      using (CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateEncryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Write))
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) PlayerManager.Instance.PlayerSavedPerk);
        serializationStream.Close();
      }
      fileStream.Close();
    }
  }

  public static void LoadPlayerPerkConfig()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.savePerksName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    if (File.Exists(path))
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        if (fileStream.Length == 0L)
        {
          fileStream.Close();
        }
        else
        {
          DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
          try
          {
            CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
            PlayerManager.Instance.PlayerSavedPerk = new BinaryFormatter().Deserialize((Stream) serializationStream) as PlayerPerk;
            serializationStream.Close();
          }
          catch (SerializationException ex)
          {
            if (GameManager.Instance.GetDeveloperMode())
              Debug.Log((object) ("Failed to deserialize PlayerPerks. Reason: " + ex.Message));
          }
          fileStream.Close();
        }
      }
    }
    else
    {
      if (!GameManager.Instance.GetDeveloperMode())
        return;
      Debug.Log((object) ("PlayerPerks not found in " + path));
    }
  }

  public static void SaveMutedPlayers()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.saveMutedName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
      using (CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateEncryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Write))
      {
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) NetworkManager.Instance.PlayerMuteList);
        serializationStream.Close();
      }
      fileStream.Close();
    }
  }

  public static void LoadMutedPlayers()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append(SaveManager.saveMutedName);
    stringBuilder.Append(SaveManager.saveGameExtension);
    string path = stringBuilder.ToString();
    if (File.Exists(path))
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        if (fileStream.Length == 0L)
        {
          fileStream.Close();
        }
        else
        {
          DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
          try
          {
            CryptoStream serializationStream = new CryptoStream((Stream) fileStream, cryptoServiceProvider.CreateDecryptor(SaveManager.key, SaveManager.iv), CryptoStreamMode.Read);
            NetworkManager.Instance.PlayerMuteList = new BinaryFormatter().Deserialize((Stream) serializationStream) as List<string>;
            serializationStream.Close();
          }
          catch (SerializationException ex)
          {
            if (GameManager.Instance.GetDeveloperMode())
              Debug.Log((object) ("Failed to deserialize MutedPlayers. Reason: " + ex.Message));
          }
          fileStream.Close();
        }
      }
    }
    else
    {
      if (!GameManager.Instance.GetDeveloperMode())
        return;
      Debug.Log((object) ("MutedPlayers not found in " + path));
    }
  }

  public static void ResetTutorial()
  {
    PlayerManager.Instance.TutorialWatched = new List<string>();
    SaveManager.SavePlayerData();
  }

  public static void RestorePlayerData(PlayerData playerData)
  {
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) nameof (RestorePlayerData));
    string str1 = SteamManager.Instance.steamId.ToString();
    if (playerData.SteamUserId == null)
      playerData.SteamUserId = str1;
    if (playerData.SteamUserId != str1 && !GameManager.Instance.GetDeveloperMode())
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(Application.persistentDataPath);
      stringBuilder.Append("/");
      stringBuilder.Append((ulong) SteamManager.Instance.steamId);
      stringBuilder.Append("/");
      stringBuilder.Append(GameManager.Instance.ProfileFolder);
      stringBuilder.Append(SaveManager.savePlayerName);
      string str2 = stringBuilder.ToString();
      File.Move(str2 + SaveManager.saveGameExtension, str2 + "-error-" + Functions.GetTimestamp() + SaveManager.saveGameExtension);
    }
    else
    {
      PlayerManager.Instance.LastUsedTeam = playerData.LastUsedTeam;
      PlayerManager.Instance.TutorialWatched = playerData.TutorialWatched;
      PlayerManager.Instance.UnlockedHeroes = playerData.UnlockedHeroes == null ? new List<string>() : playerData.UnlockedHeroes;
      PlayerManager.Instance.UnlockedCards = playerData.UnlockedCards == null ? new List<string>() : playerData.UnlockedCards;
      PlayerManager.Instance.UnlockedNodes = playerData.UnlockedNodes == null ? new List<string>() : playerData.UnlockedNodes;
      PlayerManager.Instance.TreasuresClaimed = playerData.TreasuresClaimed == null ? new List<string>() : playerData.TreasuresClaimed;
      PlayerManager.Instance.UnlockedCardsByGame = playerData.UnlockedCardsByGame == null ? new Dictionary<string, List<string>>() : playerData.UnlockedCardsByGame;
      int num1 = playerData.NgUnlocked ? 1 : 0;
      PlayerManager.Instance.NgUnlocked = playerData.NgUnlocked;
      int ngLevel = playerData.NgLevel;
      PlayerManager.Instance.NgLevel = playerData.NgLevel;
      int playerRankProgress = playerData.PlayerRankProgress;
      PlayerManager.Instance.SetPlayerRankProgress(playerData.PlayerRankProgress);
      int adventureMadnessLevel = playerData.MaxAdventureMadnessLevel;
      PlayerManager.Instance.MaxAdventureMadnessLevel = playerData.MaxAdventureMadnessLevel;
      if (PlayerManager.Instance.MaxAdventureMadnessLevel == 0)
        PlayerManager.Instance.MaxAdventureMadnessLevel = PlayerManager.Instance.NgLevel;
      int obeliskMadnessLevel = playerData.ObeliskMadnessLevel;
      PlayerManager.Instance.ObeliskMadnessLevel = playerData.ObeliskMadnessLevel;
      int singularityMadnessLevel = playerData.SingularityMadnessLevel;
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
        foreach (KeyValuePair<string, int> keyValuePair in playerData.HeroProgress)
          PlayerManager.Instance.HeroProgress.Add(keyValuePair.Key, keyValuePair.Value);
      }
      PlayerManager.Instance.HeroPerks = playerData.HeroPerks;
      if (PlayerManager.Instance.HeroPerks != null)
      {
        foreach (KeyValuePair<string, List<string>> heroPerk in PlayerManager.Instance.HeroPerks)
        {
          for (int index = heroPerk.Value.Count - 1; index >= 0; --index)
          {
            PerkData perkData = Globals.Instance.GetPerkData(heroPerk.Value[index]);
            if (!((UnityEngine.Object) perkData == (UnityEngine.Object) null))
            {
              int num2 = perkData.MainPerk ? 1 : 0;
              if (perkData.MainPerk)
                continue;
            }
            heroPerk.Value.RemoveAt(index);
          }
        }
      }
      PlayerManager.Instance.SupplyBought = playerData.SupplyBought;
      PlayerManager.Instance.SkinUsed = playerData.SkinUsed == null ? new Dictionary<string, string>() : playerData.SkinUsed;
      PlayerManager.Instance.CardbackUsed = playerData.CardbackUsed == null ? new Dictionary<string, string>() : playerData.CardbackUsed;
      if (GameManager.Instance.GetDeveloperMode() || GameManager.Instance.UnlockMadness)
      {
        PlayerManager.Instance.NgLevel = 9;
        PlayerManager.Instance.ObeliskMadnessLevel = 10;
      }
      SaveManager.LoadRuns();
    }
  }

  private static PlayerData ReBuildPlayerData(PlayerData _playerData)
  {
    if (_playerData == null)
      _playerData = new PlayerData();
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
    List<string> stringList = PlayerManager.Instance.PlayerRuns ?? new List<string>();
    string json = JsonUtility.ToJson((object) _playerRun);
    stringList.Add(json);
    PlayerManager.Instance.PlayerRuns = stringList;
    if (stringList.Count > 4)
      PlayerManager.Instance.AchievementUnlock("MISC_ADVENTURER");
    if (_playerRun.TotalPlayers > 1)
      PlayerManager.Instance.AchievementUnlock("MISC_ADVENTURERGUILD");
    SaveManager.SaveRuns();
  }

  public static void DeleteDirectory(string target_dir)
  {
    string[] files = Directory.GetFiles(target_dir);
    string[] directories = Directory.GetDirectories(target_dir);
    foreach (string path in files)
    {
      File.SetAttributes(path, FileAttributes.Normal);
      File.Delete(path);
    }
    foreach (string target_dir1 in directories)
      SaveManager.DeleteDirectory(target_dir1);
    Directory.Delete(target_dir, false);
  }

  public static string PathSaveGameTurn(int slot)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Application.persistentDataPath);
    stringBuilder.Append("/");
    stringBuilder.Append((ulong) SteamManager.Instance.steamId);
    stringBuilder.Append("/");
    stringBuilder.Append(GameManager.Instance.ProfileFolder);
    stringBuilder.Append(SaveManager.saveGameName);
    stringBuilder.Append(slot);
    stringBuilder.Append(SaveManager.saveGameTurnExtension);
    stringBuilder.Append(SaveManager.saveGameExtension);
    return stringBuilder.ToString();
  }

  public static bool PrefsHasKey(string key) => PlayerPrefs.HasKey(key);

  public static void PrefsRemoveKey(string key) => PlayerPrefs.DeleteKey(key);

  public static void SavePrefs() => PlayerPrefs.Save();

  public static void SaveIntoPrefsInt(string key, int value) => PlayerPrefs.SetInt(key, value);

  public static int LoadPrefsInt(string key) => PlayerPrefs.GetInt(key);

  public static void SaveIntoPrefsFloat(string key, float value)
  {
    PlayerPrefs.SetFloat(key, value);
  }

  public static float LoadPrefsFloat(string key) => PlayerPrefs.GetFloat(key);

  public static void SaveIntoPrefsBool(string key, bool value)
  {
    PlayerPrefs.SetInt(key, value ? 1 : 0);
  }

  public static bool LoadPrefsBool(string key) => PlayerPrefs.GetInt(key) == 1;

  public static void SaveIntoPrefsString(string key, string value)
  {
    PlayerPrefs.SetString(key, value);
  }

  public static string LoadPrefsString(string key) => PlayerPrefs.GetString(key);

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
}
