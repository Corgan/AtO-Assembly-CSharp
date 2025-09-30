// Decompiled with JetBrains decompiler
// Type: CheatDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zone;

#nullable disable
public static class CheatDrawer
{
  private static readonly List<CheatDrawer.CheatDefinition> cheats = new List<CheatDrawer.CheatDefinition>()
  {
    new CheatDrawer.CheatDefinition("cheatMode", "Cheat Mode (+dlc)"),
    new CheatDrawer.CheatDefinition("winMatchOnStart", "Win match on start"),
    new CheatDrawer.CheatDefinition("skipTutorial", "Skip Tutorial"),
    new CheatDrawer.CheatDefinition("startFromMap", "Start from Map"),
    new CheatDrawer.CheatDefinition("useImmortal", "Use Immortal"),
    new CheatDrawer.CheatDefinition("useManyResources", "Use Many Resources"),
    new CheatDrawer.CheatDefinition("unlockAllHeroes", "Unlock All Heroes"),
    new CheatDrawer.CheatDefinition("unlockMadness", "Unlock Madness"),
    new CheatDrawer.CheatDefinition("disableSave", "Disable Save"),
    new CheatDrawer.CheatDefinition("disableSteamAuthorizationForPhoton", "Disable Steam Auth"),
    new CheatDrawer.CheatDefinition("useTestSteamID", "Use Test Steam ID")
  };

  public static void DrawCheatsRuntime(
    GameManager gameManager,
    GUIStyle headerStyle,
    GUIStyle toggleStyle,
    GUIStyle buttonStyle)
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
    gameManager.StartFromMap = (MapType) GUILayout.SelectionGrid((int) gameManager.StartFromMap, Enum.GetNames(typeof (MapType)), 2, buttonStyle);
    GUILayout.Space(10f);
    if (!GUILayout.Button("Clean Saved Data", buttonStyle))
      return;
    SaveManager.CleanSavePlayerData();
    Debug.Log((object) "Player data cleaned.");
  }

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
}
