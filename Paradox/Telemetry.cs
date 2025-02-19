// Decompiled with JetBrains decompiler
// Type: Paradox.Telemetry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using PDX.SDK.Contracts.Enums.Errors;
using PDX.SDK.Contracts.Service.Telemetry;
using PDX.SDK.Contracts.Service.Telemetry.Models;
using PDX.SDK.Contracts.Service.Telemetry.Result;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Paradox
{
  public class Telemetry
  {
    private static bool debugTelemetry;

    public static async void SendStartGame()
    {
      string str1 = Globals.Instance.CurrentLang.ToString();
      string str2 = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.ToString();
      List<string> list = new List<string>();
      for (int index = 0; index < Globals.Instance.SkuAvailable.Count; ++index)
      {
        if (SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[index]))
        {
          switch (Globals.Instance.SkuAvailable[index])
          {
            case "2168960":
              list.Add("Spooky night in Senenthia");
              continue;
            case "2325780":
              list.Add("The Wolf Wars");
              continue;
            case "2511580":
              list.Add("Sands of Ulminin");
              continue;
            case "2666340":
              list.Add("Amelia, the Queen");
              continue;
            default:
              continue;
          }
        }
      }
      List<JsonEventBuilder> events = new JsonBuilder().AddEvent("language").SetPayload((object) new Dictionary<string, string>()
      {
        {
          "game_language",
          str1
        },
        {
          "os_language",
          str2
        }
      }).AddEvent("dlc").SetPayload((object) new Dictionary<string, string>()
      {
        {
          "dlc_owned",
          Paradox.Telemetry.SerializeListForTelemetry(list)
        }
      }).Build();
      if (Paradox.Telemetry.debugTelemetry)
      {
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <language> [game_language] -> " + str1));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <language> [os_language] -> " + str2));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <dlc> [dlc_owned] -> " + Paradox.Telemetry.SerializeListForTelemetry(list)));
      }
      Paradox.Telemetry.HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send<JsonEventBuilder>((IList<JsonEventBuilder>) events));
    }

    public static async void SendPlaysessionStart(bool isLoading = false)
    {
      string playthrough_id;
      string sessionflag;
      string gamemode;
      List<string> heroesList;
      if (Startup.PDXContext == null)
      {
        playthrough_id = (string) null;
        sessionflag = (string) null;
        gamemode = (string) null;
        heroesList = (List<string>) null;
      }
      else
      {
        playthrough_id = AtOManager.Instance.GetGameUniqueId();
        sessionflag = isLoading ? "load" : "new";
        gamemode = !GameManager.Instance.IsObeliskChallenge() ? "adventure" : (!GameManager.Instance.IsWeeklyChallenge() ? "obelisk" : "weekly");
        int multiplayer = !GameManager.Instance.IsMultiplayer() ? 0 : 1;
        heroesList = new List<string>();
        Hero[] heroArray = new Hero[4];
        while (heroArray[0] == null || (UnityEngine.Object) heroArray[0].HeroData == (UnityEngine.Object) null)
        {
          await Task.Delay(100);
          heroArray = AtOManager.Instance.GetTeamBackup();
          if (heroArray == null || heroArray.Length == 0 || heroArray[0] == null || (UnityEngine.Object) heroArray[0].HeroData == (UnityEngine.Object) null)
            heroArray = AtOManager.Instance.GetTeam();
        }
        for (int index = 3; index >= 0; --index)
        {
          if (heroArray[index] != null && (UnityEngine.Object) heroArray[index].HeroData != (UnityEngine.Object) null)
            heroesList.Add(heroArray[index].HeroData.HeroSubClass.SourceCharacterName);
        }
        string str1 = Paradox.Telemetry.SerializeListForTelemetry(heroesList);
        int madnessDifficulty = AtOManager.Instance.GetMadnessDifficulty();
        string madnessCorruptors = AtOManager.Instance.GetMadnessCorruptors();
        string str2 = "";
        if (MadnessManager.Instance.GetMadnessCorruptorNumber(madnessCorruptors) > 0)
        {
          List<string> list = new List<string>();
          for (int index = 0; index < madnessCorruptors.Length; ++index)
          {
            if (madnessCorruptors[index] == '1')
            {
              switch (index)
              {
                case 0:
                  list.Add("Impending Doom");
                  continue;
                case 1:
                  list.Add("Decadence");
                  continue;
                case 2:
                  list.Add("Restricted Power");
                  continue;
                case 3:
                  list.Add("Resistant Monsters");
                  continue;
                case 4:
                  list.Add("Poverty");
                  continue;
                case 5:
                  list.Add("Overcharged Monsters");
                  continue;
                case 6:
                  list.Add("Random Combats");
                  continue;
                case 7:
                  list.Add("Despair");
                  continue;
                default:
                  continue;
              }
            }
          }
          str2 = Paradox.Telemetry.SerializeListForTelemetry(list);
        }
        int num = !SandboxManager.Instance.IsEnabled() ? 0 : 1;
        string gameId = AtOManager.Instance.GetGameId();
        List<JsonEventBuilder> events = new JsonBuilder().AddEvent("playsession_start").SetPayload((object) new Dictionary<string, string>()
        {
          {
            "playthrough_id",
            playthrough_id
          },
          {
            "session_flag",
            sessionflag
          },
          {
            "game_mode",
            gamemode
          },
          {
            "multiplayer",
            multiplayer.ToString()
          },
          {
            "characters",
            str1
          },
          {
            "madness_level",
            madnessDifficulty.ToString()
          },
          {
            "corruptors",
            str2
          },
          {
            "sandbox",
            num.ToString()
          },
          {
            "seed",
            gameId
          }
        }).Build();
        if (Paradox.Telemetry.debugTelemetry)
        {
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [playthrough_id] -> " + playthrough_id));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [session_flag] -> " + sessionflag));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [game_mode] -> " + gamemode));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [multiplayer] -> " + multiplayer.ToString()));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [characters] -> " + str1));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [madness_level] -> " + madnessDifficulty.ToString()));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [corruptors] -> " + str2));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [sandbox] -> " + num.ToString()));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <playsession_start> [seed] -> " + gameId));
        }
        Paradox.Telemetry.HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send<JsonEventBuilder>((IList<JsonEventBuilder>) events));
        playthrough_id = (string) null;
        sessionflag = (string) null;
        gamemode = (string) null;
        heroesList = (List<string>) null;
      }
    }

    public static async void SendPlaysessionEnd(GameData gameDataSave = null)
    {
      if (Startup.PDXContext == null)
        return;
      int num = 0;
      string gameUniqueId;
      string currentMapNode;
      string zoneName;
      int zoneTier;
      if (gameDataSave == null)
      {
        gameUniqueId = AtOManager.Instance.GetGameUniqueId();
        currentMapNode = AtOManager.Instance.currentMapNode;
        zoneName = Paradox.Telemetry.GetZoneName(currentMapNode);
        zoneTier = Paradox.Telemetry.GetZoneTier(zoneName, false);
        if (GameManager.Instance.IsWeeklyChallenge())
          zoneName = "Weekly";
        else if (GameManager.Instance.IsObeliskChallenge())
          zoneName = "Obelisk Challenge";
      }
      else
      {
        gameUniqueId = gameDataSave.GameUniqueId;
        currentMapNode = gameDataSave.CurrentMapNode;
        zoneName = Paradox.Telemetry.GetZoneName(currentMapNode);
        zoneTier = Paradox.Telemetry.GetZoneTier(zoneName, false, gameDataSave.TownTier);
        if (gameDataSave.GameType == global::Enums.GameType.WeeklyChallenge)
          zoneName = "Weekly";
        else if (gameDataSave.GameType == global::Enums.GameType.Challenge)
          zoneName = "Obelisk Challenge";
      }
      string str1 = "";
      if (currentMapNode != "")
      {
        NodeData nodeData = Globals.Instance.GetNodeData(currentMapNode);
        if ((UnityEngine.Object) nodeData != (UnityEngine.Object) null)
          str1 = nodeData.SourceNodeName + " (" + nodeData.NodeId + ")";
      }
      string str2;
      if (gameDataSave != null)
      {
        str2 = "abandoned";
      }
      else
      {
        str2 = !AtOManager.Instance.IsAdventureCompleted() ? "failed" : "completed";
        if ((bool) (UnityEngine.Object) FinishRunManager.Instance)
          num = FinishRunManager.Instance.gameScore;
      }
      List<JsonEventBuilder> events = new JsonBuilder().AddEvent("playsession_end").SetPayload((object) new Dictionary<string, string>()
      {
        {
          "playthrough_id",
          gameUniqueId
        },
        {
          "act_number",
          zoneTier.ToString()
        },
        {
          "zone",
          zoneName
        },
        {
          "node",
          str1
        },
        {
          "outcome",
          str2
        },
        {
          "score",
          num.ToString()
        }
      }).Build();
      if (Paradox.Telemetry.debugTelemetry)
      {
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <playsession_end> [playthrough_id] -> " + gameUniqueId));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <playsession_end> [act_number] -> " + zoneTier.ToString()));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <playsession_end> [zone] -> " + zoneName));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <playsession_end> [node] -> " + str1));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <playsession_end> [outcome] -> " + str2));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <playsession_end> [score] -> " + num.ToString()));
      }
      Paradox.Telemetry.HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send<JsonEventBuilder>((IList<JsonEventBuilder>) events));
    }

    public static async void SendActStart(int status = 0)
    {
      if (Startup.PDXContext == null)
        return;
      string gameUniqueId = AtOManager.Instance.GetGameUniqueId();
      string zoneName = Paradox.Telemetry.GetZoneName(AtOManager.Instance.currentMapNode);
      bool flag = false;
      if (zoneName != "Senenthia" && zoneName != "Velkarath" && zoneName != "Aquarfall" && zoneName != "TheVoid" && zoneName != "Faeborg" && zoneName != "Ulminin")
        flag = true;
      if (!flag)
      {
        int zoneTier = Paradox.Telemetry.GetZoneTier(zoneName, true);
        List<JsonEventBuilder> events = new JsonBuilder().AddEvent("act_start").SetPayload((object) new Dictionary<string, string>()
        {
          {
            "playthrough_id",
            gameUniqueId
          },
          {
            "zone",
            zoneName
          },
          {
            "act_number",
            zoneTier.ToString()
          }
        }).Build();
        if (Paradox.Telemetry.debugTelemetry)
        {
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <act_start> [playthrough_id] -> " + gameUniqueId));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <act_start> [zone] -> " + zoneName));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <act_start> [act_number] -> " + zoneTier.ToString()));
        }
        Paradox.Telemetry.HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send<JsonEventBuilder>((IList<JsonEventBuilder>) events));
      }
      else
      {
        int zoneTier = Paradox.Telemetry.GetZoneTier(zoneName, false);
        string zoneNameByDungeon = Paradox.Telemetry.GetZoneNameByDungeon(zoneName);
        List<JsonEventBuilder> events = new JsonBuilder().AddEvent("dungeon_start").SetPayload((object) new Dictionary<string, string>()
        {
          {
            "playthrough_id",
            gameUniqueId
          },
          {
            "zone",
            zoneNameByDungeon
          },
          {
            "dungeon",
            zoneName
          },
          {
            "act_number",
            zoneTier.ToString()
          }
        }).Build();
        if (Paradox.Telemetry.debugTelemetry)
        {
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <dungeon_start> [playthrough_id] -> " + gameUniqueId));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <dungeon_start> [zone] -> " + zoneNameByDungeon));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <dungeon_start> [dungeon] -> " + zoneName));
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) ("Telemetry <dungeon_start> [act_number] -> " + zoneTier.ToString()));
        }
        Paradox.Telemetry.HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send<JsonEventBuilder>((IList<JsonEventBuilder>) events));
      }
    }

    public static async void SendUnlock(string type, string name)
    {
      if (Startup.PDXContext == null)
        return;
      string gameUniqueId = AtOManager.Instance.GetGameUniqueId();
      List<JsonEventBuilder> events = new JsonBuilder().AddEvent("unlock").SetPayload((object) new Dictionary<string, string>()
      {
        {
          "playthrough_id",
          gameUniqueId
        },
        {
          "unlock_type",
          type
        },
        {
          "unlock_name",
          name
        }
      }).Build();
      if (Paradox.Telemetry.debugTelemetry)
      {
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <unlock> [playthrough_id] -> " + gameUniqueId));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <unlock> [unlock_type] -> " + type));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("Telemetry <unlock> [unlock_name] -> " + name));
      }
      Paradox.Telemetry.HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send<JsonEventBuilder>((IList<JsonEventBuilder>) events));
    }

    public static void HandleTelemetryResult(SendResult telemetryResult)
    {
      if (!Paradox.Telemetry.debugTelemetry)
        return;
      if (telemetryResult.Success)
      {
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) "[HandleTelemetryResult] Success");
        if (telemetryResult.RejectedEvents.Count <= 0 || !GameManager.Instance.GetDeveloperMode())
          return;
        Debug.Log((object) "[HandleTelemetryResult] rejected from backend");
      }
      else
      {
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("[HandleTelemetryResult] Error => " + telemetryResult.Error?.ToString()));
        if (telemetryResult.Error == (Enum) InvalidState.TelemetryDisabled)
        {
          if (!GameManager.Instance.GetDeveloperMode())
            return;
          Debug.Log((object) "[HandleTelemetryResult] Telemetry is disabled");
        }
        else
        {
          if (!(telemetryResult.Error == (Enum) Forbidden.TelemetryNotCollected) || !GameManager.Instance.GetDeveloperMode())
            return;
          Debug.Log((object) "[HandleTelemetryResult] Telemetry is not collected");
        }
      }
    }

    private static int GetZoneTier(string zoneName, bool start, int zoneTier = -1)
    {
      if (zoneName == "Senenthia" || zoneName == "The Hatch" || zoneName == "Wolf Wars")
        return 1;
      return start ? (zoneTier == -1 ? AtOManager.Instance.GetTownTier() + 2 : zoneTier + 2) : (zoneTier == -1 ? AtOManager.Instance.GetTownTier() + 1 : zoneTier + 1);
    }

    private static string GetZoneName(string currentMapNode)
    {
      if (currentMapNode == "")
        return "";
      string zoneName = "";
      switch (currentMapNode.Substring(0, 4))
      {
        case "aqua":
          zoneName = "Aquarfall";
          break;
        case "drea":
          zoneName = "The Dreadnought";
          break;
        case "faen":
          zoneName = "Faeborg";
          break;
        case "forg":
          zoneName = "Black Forge";
          break;
        case "pyr_":
          zoneName = "The Ancient Pyramid";
          break;
        case "saht":
          zoneName = "Sahti";
          break;
        case "sect":
          zoneName = "The Hatch";
          break;
        case "sen_":
          zoneName = "Senenthia";
          break;
        case "sewe":
          zoneName = "Frozen Sewers";
          break;
        case "spid":
          zoneName = "Spider Lair";
          break;
        case "tuto":
          zoneName = "Senenthia";
          break;
        case "ulmi":
          zoneName = "Ulminin";
          break;
        case "velk":
          zoneName = "Velkarath";
          break;
        case "void":
          zoneName = "TheVoid";
          break;
        case "wolf":
          zoneName = "Wolf Wars";
          break;
      }
      return zoneName;
    }

    private static string GetZoneNameByDungeon(string dungeon)
    {
      string zoneNameByDungeon = "";
      switch (dungeon)
      {
        case "Black Forge":
          zoneNameByDungeon = "Velkarath";
          break;
        case "Frozen Sewers":
          zoneNameByDungeon = "Faeborg";
          break;
        case "Spider Lair":
          zoneNameByDungeon = "Aquarfall";
          break;
        case "The Ancient Pyramid":
          zoneNameByDungeon = "Ulminin";
          break;
        case "The Dreadnought":
          zoneNameByDungeon = "Sahti";
          break;
        case "The Hatch":
          zoneNameByDungeon = "Senenthia";
          break;
        case "Wolf Wars":
          zoneNameByDungeon = "Senenthia";
          break;
      }
      return zoneNameByDungeon;
    }

    private static string SerializeListForTelemetry(List<string> list)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[");
      for (int index = 0; index < list.Count; ++index)
      {
        stringBuilder.Append("\"");
        stringBuilder.Append(list[index]);
        stringBuilder.Append("\"");
        if (index < list.Count - 1)
          stringBuilder.Append(",");
      }
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }
  }
}
