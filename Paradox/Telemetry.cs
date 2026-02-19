using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using PDX.SDK.Contracts.Enums.Errors;
using PDX.SDK.Contracts.Service.Telemetry;
using PDX.SDK.Contracts.Service.Telemetry.Models;
using PDX.SDK.Contracts.Service.Telemetry.Result;
using UnityEngine;

namespace Paradox;

public class Telemetry
{
	private static DateTime? _startTime;

	private static bool debugTelemetry = true;

	public static async Task<bool> IsConsentAvailable()
	{
		return await Startup.PDXContext.Telemetry.IsTelemetryConsentPresentable();
	}

	public static async Task<bool> GetConsentChoice()
	{
		GetConsentChoiceResult getConsentChoiceResult = await Startup.PDXContext.Telemetry.GetTelemetryConsentChoice();
		if (getConsentChoiceResult.Success)
		{
			return getConsentChoiceResult.ConsentChoice;
		}
		return false;
	}

	public static async Task SetConsentChoice(bool value)
	{
		await Startup.PDXContext.Telemetry.SetTelemetryConsentChoice(value);
	}

	public static async void SendStartGame()
	{
		string text = Globals.Instance.CurrentLang.ToString();
		string text2 = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.ToString();
		List<string> list = new List<string>();
		for (int i = 0; i < Globals.Instance.SkuAvailable.Count; i++)
		{
			if (SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[i]))
			{
				switch (Globals.Instance.SkuAvailable[i])
				{
				case "2168960":
					list.Add("Spooky night in Senenthia");
					break;
				case "2325780":
					list.Add("The Wolf Wars");
					break;
				case "2511580":
					list.Add("Sands of Ulminin");
					break;
				case "2666340":
					list.Add("Amelia, the Queen");
					break;
				case "2879690":
					list.Add("The Obsidian Uprising");
					break;
				case "2879680":
					list.Add("Nenukil, the Engineer");
					break;
				case "3185630":
					list.Add("Shores of Sahti");
					break;
				case "3185650":
					list.Add("Sigrun, the Valkyrie");
					break;
				case "3185640":
					list.Add("Bernard, the Alchemist");
					break;
				case "3473720":
					list.Add("The Sunken Temple");
					break;
				case "3473700":
					list.Add("Tulah, the Spider Queen");
					break;
				case "3875470":
					list.Add("Project Necropolis");
					break;
				case "4013430":
					list.Add("Myths of the Nine Realms");
					break;
				case "4013420":
					list.Add("Golden Season Pack");
					break;
				default:
					list.Add(Globals.Instance.SkuAvailable[i]);
					break;
				}
			}
		}
		List<JsonEventBuilder> events = new JsonBuilder().AddEvent("language").SetPayload(new Dictionary<string, string>
		{
			{ "game_language", text },
			{ "os_language", text2 }
		}).AddEvent("dlc")
			.SetPayload(new Dictionary<string, string> { 
			{
				"dlc_owned",
				SerializeListForTelemetry(list)
			} })
			.Build();
		if (debugTelemetry)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <language> [game_language] -> " + text);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <language> [os_language] -> " + text2);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <dlc> [dlc_owned] -> " + SerializeListForTelemetry(list));
			}
		}
		HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events));
	}

	public static async void SendPlaysessionStart(bool isLoading = false)
	{
		if (Startup.PDXContext == null)
		{
			return;
		}
		string playthrough_id = AtOManager.Instance.GetGameUniqueId();
		string sessionflag = (isLoading ? "load" : "new");
		string gamemode = ((!GameManager.Instance.IsObeliskChallenge()) ? "adventure" : ((!GameManager.Instance.IsWeeklyChallenge()) ? "obelisk" : "weekly"));
		int multiplayer = (GameManager.Instance.IsMultiplayer() ? 1 : 0);
		List<string> heroesList = new List<string>();
		Hero[] array = new Hero[4];
		while (array[0] == null || array[0].HeroData == null)
		{
			await Task.Delay(100);
			array = AtOManager.Instance.GetTeamBackup();
			if (array == null || array.Length == 0 || array[0] == null || array[0].HeroData == null)
			{
				array = AtOManager.Instance.GetTeam();
			}
		}
		for (int num = 3; num >= 0; num--)
		{
			if (array[num] != null && array[num].HeroData != null)
			{
				heroesList.Add(array[num].HeroData.HeroSubClass.SourceCharacterName);
			}
		}
		string text = SerializeListForTelemetry(heroesList);
		int madnessDifficulty = AtOManager.Instance.GetMadnessDifficulty();
		string madnessCorruptors = AtOManager.Instance.GetMadnessCorruptors();
		string text2 = "";
		if (MadnessManager.Instance.GetMadnessCorruptorNumber(madnessCorruptors) > 0)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < madnessCorruptors.Length; i++)
			{
				if (madnessCorruptors[i] == '1')
				{
					switch (i)
					{
					case 0:
						list.Add("Impending Doom");
						break;
					case 1:
						list.Add("Decadence");
						break;
					case 2:
						list.Add("Restricted Power");
						break;
					case 3:
						list.Add("Resistant Monsters");
						break;
					case 4:
						list.Add("Poverty");
						break;
					case 5:
						list.Add("Overcharged Monsters");
						break;
					case 6:
						list.Add("Random Combats");
						break;
					case 7:
						list.Add("Despair");
						break;
					}
				}
			}
			text2 = SerializeListForTelemetry(list);
		}
		int num2 = (SandboxManager.Instance.IsEnabled() ? 1 : 0);
		string gameId = AtOManager.Instance.GetGameId();
		string text3 = SerializeArrayForTelemetry(AtOManager.Instance.teamAtO);
		string text4 = SerializeDictionaryForTelemetry(AtOManager.Instance.heroPerks);
		StartPlayTimer();
		List<JsonEventBuilder> events = new JsonBuilder().AddEvent("playsession_start").SetPayload(new Dictionary<string, string>
		{
			{ "playthrough_id", playthrough_id },
			{ "session_flag", sessionflag },
			{ "game_mode", gamemode },
			{
				"multiplayer",
				multiplayer.ToString()
			},
			{ "characters", text },
			{
				"madness_level",
				madnessDifficulty.ToString()
			},
			{ "corruptors", text2 },
			{
				"sandbox",
				num2.ToString()
			},
			{ "seed", gameId },
			{ "perks_slected", text4 },
			{ "hero_selected", text3 }
		}).Build();
		if (debugTelemetry)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [playthrough_id] -> " + playthrough_id);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [session_flag] -> " + sessionflag);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [game_mode] -> " + gamemode);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [multiplayer] -> " + multiplayer);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [characters] -> " + text);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [madness_level] -> " + madnessDifficulty);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [corruptors] -> " + text2);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [sandbox] -> " + num2);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_start> [seed] -> " + gameId);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.LogError("Telemetry <playsession_start> [perks_slected] -> " + text4);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.LogError("Telemetry <playsession_start> [hero_selected] -> " + text3);
			}
		}
		HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events));
	}

	public static async void SendPlaysessionEnd(GameData gameDataSave = null)
	{
		if (Startup.PDXContext == null)
		{
			return;
		}
		int score = 0;
		string playthrough_id;
		string currentMapNode;
		string zoneName;
		int zoneTier;
		if (gameDataSave == null)
		{
			playthrough_id = AtOManager.Instance.GetGameUniqueId();
			currentMapNode = AtOManager.Instance.currentMapNode;
			zoneName = GetZoneName(currentMapNode);
			zoneTier = GetZoneTier(zoneName, start: false);
			if (GameManager.Instance.IsWeeklyChallenge())
			{
				zoneName = "Weekly";
			}
			else if (GameManager.Instance.IsObeliskChallenge())
			{
				zoneName = "Obelisk Challenge";
			}
		}
		else
		{
			playthrough_id = gameDataSave.GameUniqueId;
			currentMapNode = gameDataSave.CurrentMapNode;
			zoneName = GetZoneName(currentMapNode);
			zoneTier = GetZoneTier(zoneName, start: false, gameDataSave.TownTier);
			if (gameDataSave.GameType == Enums.GameType.WeeklyChallenge)
			{
				zoneName = "Weekly";
			}
			else if (gameDataSave.GameType == Enums.GameType.Challenge)
			{
				zoneName = "Obelisk Challenge";
			}
		}
		string nodeName = "";
		if (currentMapNode != "")
		{
			NodeData nodeData = Globals.Instance.GetNodeData(currentMapNode);
			if (nodeData != null)
			{
				nodeName = nodeData.SourceNodeName + " (" + nodeData.NodeId + ")";
			}
		}
		string outcome;
		if (gameDataSave != null)
		{
			outcome = "abandoned";
		}
		else
		{
			outcome = ((!AtOManager.Instance.IsAdventureCompleted()) ? "failed" : "completed");
			if ((bool)FinishRunManager.Instance)
			{
				score = FinishRunManager.Instance.gameScore;
			}
		}
		if (gameDataSave == null)
		{
			string text = SerializeHeroesWithTraits(AtOManager.Instance.teamAtO);
			string text2 = SerializeListForTelemetry(AtOManager.Instance.mapVisitedNodes);
			string text3 = SerializeHeroesWithEquipment(AtOManager.Instance.teamAtO);
			string text4 = StopPlayTimer();
			List<JsonEventBuilder> events = new JsonBuilder().AddEvent("playsession_end").SetPayload(new Dictionary<string, string>
			{
				{ "playthrough_id", playthrough_id },
				{
					"act_number",
					zoneTier.ToString()
				},
				{ "zone", zoneName },
				{ "node", nodeName },
				{ "outcome", outcome },
				{
					"score",
					score.ToString()
				},
				{ "traits", text },
				{ "nodes", text2 },
				{ "equipment", text3 },
				{ "play_time", text4 }
			}).Build();
			if (debugTelemetry)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [traits] -> " + text);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [nodes] -> " + text2);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [equipment] -> " + text3);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [play_time] -> " + text4);
				}
			}
			HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events));
		}
		else
		{
			List<JsonEventBuilder> events2 = new JsonBuilder().AddEvent("playsession_end").SetPayload(new Dictionary<string, string>
			{
				{ "playthrough_id", playthrough_id },
				{
					"act_number",
					zoneTier.ToString()
				},
				{ "zone", zoneName },
				{ "node", nodeName },
				{ "outcome", outcome },
				{
					"score",
					score.ToString()
				}
			}).Build();
			HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events2));
		}
		if (debugTelemetry)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [playthrough_id] -> " + playthrough_id);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [act_number] -> " + zoneTier);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [zone] -> " + zoneName);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [node] -> " + nodeName);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [outcome] -> " + outcome);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [score] -> " + score);
			}
		}
	}

	public static async void SendActStart(int status = 0)
	{
		if (Startup.PDXContext == null)
		{
			return;
		}
		string gameUniqueId = AtOManager.Instance.GetGameUniqueId();
		string zoneName = GetZoneName(AtOManager.Instance.currentMapNode);
		bool flag = false;
		if (zoneName != "Senenthia" && zoneName != "Velkarath" && zoneName != "Aquarfall" && zoneName != "TheVoid" && zoneName != "Faeborg" && zoneName != "Ulminin" && zoneName != "Sahti" && zoneName != "Witch Woods" && zoneName != "Castle Courtyard" && zoneName != "Castle Spire")
		{
			flag = true;
		}
		string text = SerializeListForTelemetry(AtOManager.Instance.teamAtO[0].Cards);
		string value = SerializeListForTelemetry(AtOManager.Instance.teamAtO[1].Cards);
		string value2 = SerializeListForTelemetry(AtOManager.Instance.teamAtO[2].Cards);
		string value3 = SerializeListForTelemetry(AtOManager.Instance.teamAtO[3].Cards);
		if (!flag)
		{
			int zoneTier = GetZoneTier(zoneName, start: true);
			List<JsonEventBuilder> events = new JsonBuilder().AddEvent("act_start").SetPayload(new Dictionary<string, string>
			{
				{ "playthrough_id", gameUniqueId },
				{ "zone", zoneName },
				{
					"act_number",
					zoneTier.ToString()
				},
				{ "cards_deck0", text },
				{ "cards_deck1", value },
				{ "cards_deck2", value2 },
				{ "cards_deck3", value3 }
			}).Build();
			if (debugTelemetry)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.Log("Telemetry <act_start> [playthrough_id] -> " + gameUniqueId);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.Log("Telemetry <act_start> [zone] -> " + zoneName);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.Log("Telemetry <act_start> [act_number] -> " + zoneTier);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [cards] -> " + text);
				}
			}
			HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events));
			return;
		}
		int zoneTier2 = GetZoneTier(zoneName, start: false);
		string zoneNameByDungeon = GetZoneNameByDungeon(zoneName);
		List<JsonEventBuilder> events2 = new JsonBuilder().AddEvent("dungeon_start").SetPayload(new Dictionary<string, string>
		{
			{ "playthrough_id", gameUniqueId },
			{ "zone", zoneNameByDungeon },
			{ "dungeon", zoneName },
			{
				"act_number",
				zoneTier2.ToString()
			}
		}).Build();
		if (debugTelemetry)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <dungeon_start> [playthrough_id] -> " + gameUniqueId);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <dungeon_start> [zone] -> " + zoneNameByDungeon);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <dungeon_start> [dungeon] -> " + zoneName);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <dungeon_start> [act_number] -> " + zoneTier2);
			}
		}
		HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events2));
	}

	public static async void SendActEnd(GameData gameDataSave = null, bool actCompleted = false)
	{
		if (Startup.PDXContext == null)
		{
			return;
		}
		int score = 0;
		List<string> heroesList = new List<string>();
		Hero[] array = new Hero[4];
		while (array == null || array[0] == null || array[0].HeroData == null)
		{
			await Task.Delay(100);
			array = AtOManager.Instance.GetTeamBackup();
			if (array == null || array.Length == 0 || array[0] == null || array[0].HeroData == null)
			{
				array = AtOManager.Instance.GetTeam();
			}
		}
		for (int num = 3; num >= 0; num--)
		{
			if (array[num] != null && array[num].HeroData != null)
			{
				heroesList.Add(array[num].HeroData.HeroSubClass.SourceCharacterName);
			}
		}
		string value = SerializeListForTelemetry(heroesList);
		string playthrough_id;
		string currentMapNode;
		string zoneName;
		int zoneTier;
		if (gameDataSave == null)
		{
			playthrough_id = AtOManager.Instance.GetGameUniqueId();
			currentMapNode = AtOManager.Instance.currentMapNode;
			zoneName = GetZoneName(currentMapNode);
			zoneTier = GetZoneTier(zoneName, start: false);
			if (GameManager.Instance.IsWeeklyChallenge())
			{
				zoneName = "Weekly";
			}
			else if (GameManager.Instance.IsObeliskChallenge())
			{
				zoneName = "Obelisk Challenge";
			}
		}
		else
		{
			playthrough_id = gameDataSave.GameUniqueId;
			currentMapNode = gameDataSave.CurrentMapNode;
			zoneName = GetZoneName(currentMapNode);
			zoneTier = GetZoneTier(zoneName, start: false, gameDataSave.TownTier);
			if (gameDataSave.GameType == Enums.GameType.WeeklyChallenge)
			{
				zoneName = "Weekly";
			}
			else if (gameDataSave.GameType == Enums.GameType.Challenge)
			{
				zoneName = "Obelisk Challenge";
			}
		}
		string nodeName = "";
		if (currentMapNode != "")
		{
			NodeData nodeData = Globals.Instance.GetNodeData(currentMapNode);
			if (nodeData != null)
			{
				nodeName = nodeData.SourceNodeName + " (" + nodeData.NodeId + ")";
			}
		}
		string outcome;
		if (gameDataSave != null)
		{
			outcome = "abandoned";
		}
		else
		{
			outcome = (actCompleted ? "completed" : "failed");
			if ((bool)FinishRunManager.Instance)
			{
				score = FinishRunManager.Instance.gameScore;
			}
		}
		if (gameDataSave == null)
		{
			string text = SerializeHeroesWithTraits(AtOManager.Instance.teamAtO);
			string text2 = SerializeListForTelemetry(AtOManager.Instance.mapVisitedNodes);
			string text3 = SerializeHeroesWithEquipment(AtOManager.Instance.teamAtO);
			string text4 = StopPlayTimer();
			List<JsonEventBuilder> events = new JsonBuilder().AddEvent("playsession_end").SetPayload(new Dictionary<string, string>
			{
				{ "playthrough_id", playthrough_id },
				{
					"act_number",
					zoneTier.ToString()
				},
				{ "zone", zoneName },
				{ "outcome", outcome },
				{
					"score",
					score.ToString()
				},
				{ "characters", value },
				{ "visited_nodes", text2 }
			}).Build();
			if (debugTelemetry)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [traits] -> " + text);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [nodes] -> " + text2);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [equipment] -> " + text3);
				}
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("Telemetry <playsession_start> [play_time] -> " + text4);
				}
			}
			HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events));
		}
		else
		{
			List<JsonEventBuilder> events2 = new JsonBuilder().AddEvent("playsession_end").SetPayload(new Dictionary<string, string>
			{
				{ "playthrough_id", playthrough_id },
				{
					"act_number",
					zoneTier.ToString()
				},
				{ "zone", zoneName },
				{ "node", nodeName },
				{ "outcome", outcome },
				{
					"score",
					score.ToString()
				}
			}).Build();
			HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events2));
		}
		if (debugTelemetry)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [playthrough_id] -> " + playthrough_id);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [act_number] -> " + zoneTier);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [zone] -> " + zoneName);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [node] -> " + nodeName);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [outcome] -> " + outcome);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <playsession_end> [score] -> " + score);
			}
		}
	}

	public static async void SendUnlock(string type, string name)
	{
		if (Startup.PDXContext == null)
		{
			return;
		}
		string gameUniqueId = AtOManager.Instance.GetGameUniqueId();
		List<JsonEventBuilder> events = new JsonBuilder().AddEvent("unlock").SetPayload(new Dictionary<string, string>
		{
			{ "playthrough_id", gameUniqueId },
			{ "unlock_type", type },
			{ "unlock_name", name }
		}).Build();
		if (debugTelemetry)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <unlock> [playthrough_id] -> " + gameUniqueId);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <unlock> [unlock_type] -> " + type);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("Telemetry <unlock> [unlock_name] -> " + name);
			}
		}
		HandleTelemetryResult(await Startup.PDXContext.Telemetry.Send((IList<JsonEventBuilder>)events));
	}

	public static void HandleTelemetryResult(SendResult telemetryResult)
	{
		if (!debugTelemetry)
		{
			return;
		}
		if (telemetryResult.Success)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("[HandleTelemetryResult] Success");
			}
			if (telemetryResult.RejectedEvents.Count > 0 && GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("[HandleTelemetryResult] rejected from backend");
			}
			return;
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("[HandleTelemetryResult] Error => " + telemetryResult.Error);
		}
		if (telemetryResult.Error == InvalidState.TelemetryDisabled)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("[HandleTelemetryResult] Telemetry is disabled");
			}
		}
		else if (telemetryResult.Error == Forbidden.TelemetryNotCollected && GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("[HandleTelemetryResult] Telemetry is not collected");
		}
	}

	private static int GetZoneTier(string zoneName, bool start, int zoneTier = -1)
	{
		int num = 0;
		if (zoneName == "Castle Courtyard")
		{
			num = 1;
		}
		if (zoneName == "Castle Spire")
		{
			num = 2;
		}
		switch (zoneName)
		{
		case "Senenthia":
		case "The Hatch":
		case "Wolf Wars":
			return 1;
		default:
			if (start)
			{
				if (zoneTier == -1)
				{
					return AtOManager.Instance.GetTownTier() + 2 + num;
				}
				return zoneTier + 2 + num;
			}
			if (zoneTier == -1)
			{
				return AtOManager.Instance.GetTownTier() + 1 + num;
			}
			return zoneTier + 1 + num;
		}
	}

	private static string GetZoneName(string currentMapNode)
	{
		if (currentMapNode == "")
		{
			return "";
		}
		string result = "";
		switch (currentMapNode.Substring(0, 4))
		{
		case "sen_":
			result = "Senenthia";
			break;
		case "tuto":
			result = "Senenthia";
			break;
		case "sect":
			result = "The Hatch";
			break;
		case "wolf":
			result = "Wolf Wars";
			break;
		case "spid":
			result = "Spider Lair";
			break;
		case "forg":
			result = "Black Forge";
			break;
		case "sewe":
			result = "Frozen Sewers";
			break;
		case "pyr_":
			result = "The Ancient Pyramid";
			break;
		case "velk":
			result = "Velkarath";
			break;
		case "aqua":
			result = "Aquarfall";
			break;
		case "void":
			result = "TheVoid";
			break;
		case "faen":
			result = "Faeborg";
			break;
		case "ulmi":
			result = "Ulminin";
			break;
		case "drea":
			result = "The Dreadnought";
			break;
		case "saht":
			result = "Sahti";
			break;
		case "cast":
		case "cour":
		case "wood":
			result = "Necropolis";
			break;
		}
		return result;
	}

	private static string GetZoneNameByDungeon(string dungeon)
	{
		string result = "";
		switch (dungeon)
		{
		case "The Hatch":
			result = "Senenthia";
			break;
		case "Wolf Wars":
			result = "Senenthia";
			break;
		case "Spider Lair":
			result = "Aquarfall";
			break;
		case "Black Forge":
			result = "Velkarath";
			break;
		case "Frozen Sewers":
			result = "Faeborg";
			break;
		case "The Ancient Pyramid":
			result = "Ulminin";
			break;
		case "The Dreadnought":
			result = "Sahti";
			break;
		}
		return result;
	}

	private static string SerializeListForTelemetry(List<string> list)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		for (int i = 0; i < list.Count; i++)
		{
			stringBuilder.Append("\"");
			stringBuilder.Append(list[i]);
			stringBuilder.Append("\"");
			if (i < list.Count - 1)
			{
				stringBuilder.Append(",");
			}
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}

	private static string SerializeDictionaryForTelemetry(Dictionary<string, List<string>> dict)
	{
		if (dict == null)
		{
			return "{}";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("{");
		int num = 0;
		foreach (KeyValuePair<string, List<string>> item in dict)
		{
			string text = item.Key ?? "";
			stringBuilder.Append("\"");
			stringBuilder.Append(text.Replace("\"", "\\\""));
			stringBuilder.Append("\":");
			List<string> list = item.Value ?? new List<string>();
			stringBuilder.Append(SerializeListForTelemetry(list));
			if (num < dict.Count - 1)
			{
				stringBuilder.Append(",");
			}
			num++;
		}
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}

	private static string SerializeArrayForTelemetry(Hero[] array)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append("\"");
			stringBuilder.Append(array[i].SubclassName);
			stringBuilder.Append("\"");
			if (i < array.Length - 1)
			{
				stringBuilder.Append(",");
			}
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}

	private static string SerializeHeroesWithTraits(Hero[] heroes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		for (int i = 0; i < heroes.Length; i++)
		{
			string subclassName = heroes[i].SubclassName;
			string[] traits = heroes[i].Traits;
			stringBuilder.Append("{");
			stringBuilder.Append("\"HeroName\":\"");
			stringBuilder.Append(subclassName);
			stringBuilder.Append("\",");
			stringBuilder.Append("\"Traits\":[");
			bool flag = false;
			for (int j = 0; j < traits.Length; j++)
			{
				if (!string.IsNullOrEmpty(traits[j]))
				{
					if (flag)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append("\"");
					stringBuilder.Append(traits[j]);
					stringBuilder.Append("\"");
					flag = true;
				}
			}
			stringBuilder.Append("]}");
			if (i < heroes.Length - 1)
			{
				stringBuilder.Append(",");
			}
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}

	private static string SerializeHeroesWithEquipment(Hero[] heroes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		for (int i = 0; i < heroes.Length; i++)
		{
			string subclassName = heroes[i].SubclassName;
			stringBuilder.Append("{");
			stringBuilder.Append("\"HeroName\":\"");
			stringBuilder.Append(subclassName);
			stringBuilder.Append("\",");
			stringBuilder.Append("\"Pet\":\"");
			stringBuilder.Append(string.IsNullOrEmpty(heroes[i].Pet) ? "" : heroes[i].Pet);
			stringBuilder.Append("\",");
			stringBuilder.Append("\"Weapon\":\"");
			stringBuilder.Append(string.IsNullOrEmpty(heroes[i].Weapon) ? "" : heroes[i].Weapon);
			stringBuilder.Append("\",");
			stringBuilder.Append("\"Accesory\":\"");
			stringBuilder.Append(string.IsNullOrEmpty(heroes[i].Accesory) ? "" : heroes[i].Accesory);
			stringBuilder.Append("\",");
			stringBuilder.Append("\"Armor\":\"");
			stringBuilder.Append(string.IsNullOrEmpty(heroes[i].Armor) ? "" : heroes[i].Armor);
			stringBuilder.Append("\",");
			stringBuilder.Append("\"Jewelry\":\"");
			stringBuilder.Append(string.IsNullOrEmpty(heroes[i].Jewelry) ? "" : heroes[i].Jewelry);
			stringBuilder.Append("\"");
			stringBuilder.Append("}");
			if (i < heroes.Length - 1)
			{
				stringBuilder.Append(",");
			}
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}

	public static void StartPlayTimer()
	{
		_startTime = DateTime.Now;
	}

	public static string StopPlayTimer()
	{
		if (!_startTime.HasValue)
		{
			throw new InvalidOperationException("Play timer was never started.");
		}
		TimeSpan timeSpan = DateTime.Now - _startTime.Value;
		_startTime = null;
		return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
	}
}
