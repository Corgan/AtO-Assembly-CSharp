using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneStatic
{
	public static string CrossScene { get; set; }

	public static string GetSceneName()
	{
		return SceneManager.GetActiveScene().name;
	}

	public static void LoadByName(string scene)
	{
		if (AlertManager.Instance != null)
		{
			AlertManager.buttonClickDelegate = null;
			AlertManager.Instance.HideAlert();
		}
		if (MadnessManager.Instance != null)
		{
			MadnessManager.Instance.CloseMadness();
		}
		if (CardScreenManager.Instance != null && CardScreenManager.Instance.IsActive())
		{
			CardScreenManager.Instance.ShowCardScreen(_state: false);
		}
		Cursor.visible = true;
		switch (scene)
		{
		case "MainMenu":
			LoadMainMenu();
			break;
		case "Lobby":
			LoadLobby();
			break;
		case "Combat":
			LoadCombat();
			break;
		case "IntroNewGame":
			LoadIntroNewGame();
			break;
		case "Town":
			LoadTown();
			break;
		case "Map":
			LoadMap();
			break;
		case "HeroSelection":
			LoadHeroSelection();
			break;
		case "TomeOfKnowledge":
			LoadTome();
			break;
		case "TeamManagement":
			LoadTeamManagement();
			break;
		case "Rewards":
			LoadRewards();
			break;
		case "Loot":
			LoadLoot();
			break;
		case "FinishRun":
			LoadFinishRun();
			break;
		case "ChallengeSelection":
			LoadChallengeSelection();
			break;
		case "TrailerEnd":
			if (GameManager.Instance == null)
			{
				CrossScene = "TrailerEnd";
				SceneManager.LoadScene("Game");
			}
			else
			{
				GameManager.Instance.ChangeScene("TrailerEnd");
			}
			break;
		case "TrailerPoster":
			if (GameManager.Instance == null)
			{
				CrossScene = "TrailerPoster";
				SceneManager.LoadScene("Game");
			}
			else
			{
				GameManager.Instance.ChangeScene("TrailerPoster");
			}
			break;
		case "CardPlayer":
			if (GameManager.Instance == null)
			{
				CrossScene = "CardPlayer";
				SceneManager.LoadScene("Game");
			}
			else
			{
				GameManager.Instance.ChangeScene("CardPlayer");
			}
			break;
		case "CardPlayerPairs":
			LoadCardPlayerPairs();
			break;
		case "Cinematic":
			LoadCinematic();
			break;
		}
	}

	public static void LoadMainMenu()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "MainMenu";
			SceneManager.LoadScene("Game");
			return;
		}
		if (AtOManager.Instance.IsCombatTool)
		{
			PlayerManager.Instance.InitPlayerData();
			PlayerManager.Instance.UnlockInitialHeroes();
			AtOManager.Instance.ClearGame();
			GameManager.Instance.LoadPlayerData();
		}
		GameManager.Instance.ChangeScene("MainMenu");
	}

	public static void LoadLobby()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "Lobby";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("Lobby");
		}
	}

	public static void LoadCombat()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "Combat";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("Combat");
		}
	}

	public static void LoadIntroNewGame()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "IntroNewGame";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("IntroNewGame");
		}
	}

	public static void LoadTown()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "Town";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("Town");
		}
	}

	public static void LoadCinematic()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "Cinematic";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("Cinematic");
		}
	}

	public static void LoadMap()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "Map";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("Map");
		}
	}

	public static void LoadHeroSelection()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "HeroSelection";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("HeroSelection");
		}
	}

	public static void LoadTome()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "TomeOfKnowledge";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("TomeOfKnowledge");
		}
	}

	public static void LoadTeamManagement()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "TeamManagement";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("TeamManagement");
		}
	}

	public static void LoadRewards()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "Rewards";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("Rewards");
		}
	}

	public static void LoadLoot()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "Loot";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("Loot");
		}
	}

	public static void LoadFinishRun()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "FinishRun";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("FinishRun");
		}
	}

	public static void LoadChallengeSelection()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "ChallengeSelection";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("ChallengeSelection");
		}
	}

	public static void LoadCardPlayerPairs()
	{
		if (GameManager.Instance == null)
		{
			CrossScene = "CardPlayerPairs";
			SceneManager.LoadScene("Game");
		}
		else
		{
			GameManager.Instance.ChangeScene("CardPlayerPairs");
		}
	}
}
