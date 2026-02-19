using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
	private string steamUserId;

	private string[] lastUsedTeam;

	private int townTutorialStep;

	private List<string> tutorialWatched;

	private List<string> unlockedHeroes;

	private List<string> unlockedCards;

	private List<string> unlockedNodes;

	private List<string> playerRuns;

	private List<string> bossesKilledName;

	private List<string> supplyBought;

	private bool ngUnlocked;

	private int ngLevel;

	private int playerRankProgress;

	private int maxAdventureMadnessLevel;

	private int obeliskMadnessLevel;

	private int singularityMadnessLevel;

	private int bossesKilled;

	private int monstersKilled;

	private int expGained;

	private int cardsCrafted;

	private int cardsUpgraded;

	private int goldGained;

	private int dustGained;

	private int bestScore;

	private int purchasedItems;

	private int supplyGained;

	private int supplyActual;

	private int corruptionsCompleted;

	private List<string> treasuresClaimed;

	private Dictionary<string, List<string>> unlockedCardsByGame;

	private Dictionary<string, int> heroProgress;

	private Dictionary<string, List<string>> heroPerks;

	private Dictionary<string, string> skinUsed;

	private Dictionary<string, string> cardbackUsed;

	private string sandboxSettings = "";

	public List<string> UnlockedHeroes
	{
		get
		{
			return unlockedHeroes;
		}
		set
		{
			unlockedHeroes = value;
		}
	}

	public List<string> UnlockedCards
	{
		get
		{
			return unlockedCards;
		}
		set
		{
			unlockedCards = value;
		}
	}

	public List<string> UnlockedNodes
	{
		get
		{
			return unlockedNodes;
		}
		set
		{
			unlockedNodes = value;
		}
	}

	public List<string> PlayerRuns
	{
		get
		{
			return playerRuns;
		}
		set
		{
			playerRuns = value;
		}
	}

	public List<string> TreasuresClaimed
	{
		get
		{
			return treasuresClaimed;
		}
		set
		{
			treasuresClaimed = value;
		}
	}

	public Dictionary<string, List<string>> UnlockedCardsByGame
	{
		get
		{
			return unlockedCardsByGame;
		}
		set
		{
			unlockedCardsByGame = value;
		}
	}

	public List<string> TutorialWatched
	{
		get
		{
			return tutorialWatched;
		}
		set
		{
			tutorialWatched = value;
		}
	}

	public string[] LastUsedTeam
	{
		get
		{
			return lastUsedTeam;
		}
		set
		{
			lastUsedTeam = value;
		}
	}

	public int BossesKilled
	{
		get
		{
			return bossesKilled;
		}
		set
		{
			bossesKilled = value;
		}
	}

	public int MonstersKilled
	{
		get
		{
			return monstersKilled;
		}
		set
		{
			monstersKilled = value;
		}
	}

	public int CardsCrafted
	{
		get
		{
			return cardsCrafted;
		}
		set
		{
			cardsCrafted = value;
		}
	}

	public int CardsUpgraded
	{
		get
		{
			return cardsUpgraded;
		}
		set
		{
			cardsUpgraded = value;
		}
	}

	public int GoldGained
	{
		get
		{
			return goldGained;
		}
		set
		{
			goldGained = value;
		}
	}

	public int DustGained
	{
		get
		{
			return dustGained;
		}
		set
		{
			dustGained = value;
		}
	}

	public int BestScore
	{
		get
		{
			return bestScore;
		}
		set
		{
			bestScore = value;
		}
	}

	public int ExpGained
	{
		get
		{
			return expGained;
		}
		set
		{
			expGained = value;
		}
	}

	public int PurchasedItems
	{
		get
		{
			return purchasedItems;
		}
		set
		{
			purchasedItems = value;
		}
	}

	public Dictionary<string, int> HeroProgress
	{
		get
		{
			return heroProgress;
		}
		set
		{
			heroProgress = value;
		}
	}

	public Dictionary<string, List<string>> HeroPerks
	{
		get
		{
			return heroPerks;
		}
		set
		{
			heroPerks = value;
		}
	}

	public List<string> BossesKilledName
	{
		get
		{
			return bossesKilledName;
		}
		set
		{
			bossesKilledName = value;
		}
	}

	public int SupplyGained
	{
		get
		{
			return supplyGained;
		}
		set
		{
			supplyGained = value;
		}
	}

	public int SupplyActual
	{
		get
		{
			return supplyActual;
		}
		set
		{
			supplyActual = value;
		}
	}

	public List<string> SupplyBought
	{
		get
		{
			return supplyBought;
		}
		set
		{
			supplyBought = value;
		}
	}

	public bool NgUnlocked
	{
		get
		{
			return ngUnlocked;
		}
		set
		{
			ngUnlocked = value;
		}
	}

	public int CorruptionsCompleted
	{
		get
		{
			return corruptionsCompleted;
		}
		set
		{
			corruptionsCompleted = value;
		}
	}

	public string SteamUserId
	{
		get
		{
			return steamUserId;
		}
		set
		{
			steamUserId = value;
		}
	}

	public int NgLevel
	{
		get
		{
			return ngLevel;
		}
		set
		{
			ngLevel = value;
		}
	}

	public Dictionary<string, string> SkinUsed
	{
		get
		{
			return skinUsed;
		}
		set
		{
			skinUsed = value;
		}
	}

	public Dictionary<string, string> CardbackUsed
	{
		get
		{
			return cardbackUsed;
		}
		set
		{
			cardbackUsed = value;
		}
	}

	public int ObeliskMadnessLevel
	{
		get
		{
			return obeliskMadnessLevel;
		}
		set
		{
			obeliskMadnessLevel = value;
		}
	}

	public int MaxAdventureMadnessLevel
	{
		get
		{
			return maxAdventureMadnessLevel;
		}
		set
		{
			maxAdventureMadnessLevel = value;
		}
	}

	public int SingularityMadnessLevel
	{
		get
		{
			return singularityMadnessLevel;
		}
		set
		{
			singularityMadnessLevel = value;
		}
	}

	public int PlayerRankProgress
	{
		get
		{
			return playerRankProgress;
		}
		set
		{
			playerRankProgress = value;
		}
	}

	public int TownTutorialStep
	{
		get
		{
			return townTutorialStep;
		}
		set
		{
			townTutorialStep = value;
		}
	}

	public string SandboxSettings
	{
		get
		{
			return sandboxSettings;
		}
		set
		{
			sandboxSettings = value;
		}
	}
}
