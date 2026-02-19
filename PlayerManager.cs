using System;
using System.Collections.Generic;
using Paradox;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[SerializeField]
	private string playerName;

	[Header("Add cards to unlock by default")]
	[SerializeField]
	private List<string> defaultCardUnlocks = new List<string>();

	[SerializeField]
	private string[] lastUsedTeam;

	[SerializeField]
	private List<string> tutorialWatched;

	[SerializeField]
	private List<string> unlockedNodes;

	[SerializeField]
	private List<string> unlockedHeroes;

	[SerializeField]
	private List<string> unlockedCards;

	[SerializeField]
	private List<string> unlockedCardbacks;

	[SerializeField]
	private List<string> playerRuns;

	[SerializeField]
	private List<Hero> playerHeroes;

	[SerializeField]
	private int playerRankProgress;

	[SerializeField]
	private List<string> treasuresClaimed;

	[SerializeField]
	private bool ngUnlocked;

	[SerializeField]
	private int ngLevel;

	[SerializeField]
	private int maxAdventureMadnessLevel;

	[SerializeField]
	private int obeliskMadnessLevel;

	[SerializeField]
	private int bossesKilled;

	[SerializeField]
	private List<string> bossesKilledName;

	[SerializeField]
	private int monstersKilled;

	[SerializeField]
	private int expGained;

	[SerializeField]
	private int cardsCrafted;

	[SerializeField]
	private int cardsUpgraded;

	[SerializeField]
	private int goldGained;

	[SerializeField]
	private int dustGained;

	[SerializeField]
	private int bestScore;

	[SerializeField]
	private int supplyGained;

	[SerializeField]
	private int supplyActual;

	[SerializeField]
	private List<string> supplyBought;

	[SerializeField]
	private int purchasedItems;

	[SerializeField]
	private int corruptionsCompleted;

	[SerializeField]
	private Dictionary<string, string> skinUsed;

	[SerializeField]
	private Dictionary<string, string> cardbackUsed;

	[SerializeField]
	private Dictionary<string, List<string>> unlockedCardsByGame;

	[SerializeField]
	private string[] teamHeroes;

	[SerializeField]
	private string[] teamNPCs;

	[SerializeField]
	private Dictionary<string, int> heroProgress;

	[SerializeField]
	private Dictionary<string, List<string>> heroPerks;

	[SerializeField]
	private Hero[] gameTeamHeroes;

	[SerializeField]
	private PlayerDeck playerSavedDeck;

	[SerializeField]
	private PlayerPerk playerSavedPerk;

	private List<string> achievementsSent = new List<string>();

	[SerializeField]
	private int singularityMadnessLevel;

	public static PlayerManager Instance { get; private set; }

	public List<Hero> PlayerHeroes
	{
		get
		{
			return playerHeroes;
		}
		set
		{
			playerHeroes = value;
		}
	}

	public string[] TeamHeroes
	{
		get
		{
			return teamHeroes;
		}
		set
		{
			teamHeroes = value;
		}
	}

	public string[] TeamNPCs
	{
		get
		{
			return teamNPCs;
		}
		set
		{
			teamNPCs = value;
		}
	}

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

	public PlayerDeck PlayerSavedDeck
	{
		get
		{
			return playerSavedDeck;
		}
		set
		{
			playerSavedDeck = value;
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

	public PlayerPerk PlayerSavedPerk
	{
		get
		{
			return playerSavedPerk;
		}
		set
		{
			playerSavedPerk = value;
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

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitPlayerData();
	}

	public void InitPlayerData()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("Initplayerdata");
		}
		unlockedHeroes = new List<string>();
		unlockedNodes = new List<string>();
		unlockedCards = new List<string>();
		unlockedCardbacks = new List<string>();
		playerRuns = new List<string>();
		playerHeroes = new List<Hero>();
		treasuresClaimed = new List<string>();
		teamNPCs = new string[4];
		heroProgress = new Dictionary<string, int>();
		heroPerks = new Dictionary<string, List<string>>();
		bossesKilledName = new List<string>();
		supplyBought = new List<string>();
		playerSavedDeck = new PlayerDeck();
		bossesKilled = 0;
		playerRankProgress = 0;
		monstersKilled = 0;
		expGained = 0;
		cardsCrafted = 0;
		cardsUpgraded = 0;
		goldGained = 0;
		dustGained = 0;
		bestScore = 0;
		supplyGained = 0;
		supplyActual = 0;
		purchasedItems = 0;
		corruptionsCompleted = 0;
		ngLevel = 0;
		obeliskMadnessLevel = 0;
		maxAdventureMadnessLevel = 0;
		singularityMadnessLevel = 0;
	}

	public void ClearAdventurePerks()
	{
		foreach (KeyValuePair<string, List<string>> heroPerk in HeroPerks)
		{
			for (int num = heroPerk.Value.Count - 1; num >= 0; num--)
			{
				PerkData perkData = Globals.Instance.GetPerkData(heroPerk.Value[num]);
				if (perkData != null && perkData.CardClass == Enums.CardClass.Special)
				{
					Debug.Log("PERK REMOVED -> " + perkData.Id);
					heroPerk.Value.RemoveAt(num);
				}
			}
		}
	}

	public int GetCharacterTier(string _subclassId, string _type, int _rank = -1)
	{
		int num = ((_rank == -1) ? GetPerkRank(_subclassId) : _rank);
		switch (_type)
		{
		case "trait":
			if (num >= 44)
			{
				return 2;
			}
			if (num >= 20)
			{
				return 1;
			}
			break;
		case "card":
			if (num >= 36)
			{
				return 2;
			}
			if (num >= 12)
			{
				return 1;
			}
			break;
		case "item":
			if (num >= 28)
			{
				return 2;
			}
			if (num >= 4)
			{
				return 1;
			}
			break;
		}
		return 0;
	}

	public void SetPlayerRankProgress(int _rk)
	{
		int num = Globals.Instance.PerkLevel[Globals.Instance.PerkLevel.Count - 1];
		if (_rk > num)
		{
			_rk = num;
		}
		playerRankProgress = _rk;
	}

	public int GetPlayerRankProgress()
	{
		int num = Globals.Instance.PerkLevel[Globals.Instance.PerkLevel.Count - 1];
		if (playerRankProgress > num)
		{
			playerRankProgress = num;
		}
		return playerRankProgress;
	}

	public void ModifyPlayerRankProgress(int _value)
	{
		playerRankProgress += _value;
		int num = Globals.Instance.PerkLevel[Globals.Instance.PerkLevel.Count - 1];
		if (playerRankProgress > num)
		{
			playerRankProgress = num;
		}
	}

	public int GetHighestCharacterRank()
	{
		int num = 0;
		if (playerRankProgress > 0)
		{
			for (int i = 0; i < Globals.Instance.PerkLevel.Count && playerRankProgress >= Globals.Instance.PerkLevel[i]; i++)
			{
				num++;
			}
		}
		else if (heroProgress != null)
		{
			int num2 = 0;
			foreach (KeyValuePair<string, int> item in heroProgress)
			{
				if (item.Value > num2)
				{
					num2 = item.Value;
					num = GetPerkRank(item.Key);
				}
			}
			playerRankProgress = num2;
		}
		return num;
	}

	public void ResetPerks(string _hero)
	{
		_hero = _hero.ToLower();
		if (heroPerks != null && heroPerks.ContainsKey(_hero))
		{
			heroPerks[_hero].Clear();
			SaveManager.SavePlayerData();
			if (SceneStatic.GetSceneName() == "HeroSelection")
			{
				HeroSelectionManager.Instance.SendHeroPerksMP(_hero);
			}
		}
	}

	public int GetPerkCurrency(string _subclassId)
	{
		if (_subclassId == "" || _subclassId == null)
		{
			return 0;
		}
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetCurrencyBonus(list[i]);
			}
		}
		return num;
	}

	public int GetPerkShards(string _subclassId)
	{
		if (_subclassId == "" || _subclassId == null)
		{
			return 0;
		}
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetShardsBonus(list[i]);
			}
		}
		return num;
	}

	public int GetPerkMaxHealth(string _subclassId)
	{
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetMaxHealth(list[i]);
			}
		}
		return num;
	}

	public int GetPerkSpeed(string _subclassId)
	{
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetSpeed(list[i]);
			}
		}
		return num;
	}

	public int GetPerkEnergyBegin(string _subclassId)
	{
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetEnergyBegin(list[i]);
			}
		}
		return num;
	}

	public int GetPerkDamageBonus(string _subclassId, Enums.DamageType _dmgType)
	{
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetDamageBonus(list[i], _dmgType);
			}
		}
		return num;
	}

	public int GetPerkHealBonus(string _subclassId)
	{
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetHealBonus(list[i]);
			}
		}
		return num;
	}

	public int GetPerkResistBonus(string _subclassId, Enums.DamageType _resist)
	{
		List<string> list = GetHeroPerks(_subclassId);
		int num = 0;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				num += Perk.GetResistBonus(list[i], _resist);
			}
		}
		return num;
	}

	public int GetPerkNextLevelPoints(string _subclassId)
	{
		int num = 0;
		num = ((!(_subclassId != "")) ? playerRankProgress : GetProgress(_subclassId));
		for (int i = 0; i < Globals.Instance.PerkLevel.Count; i++)
		{
			if (num < Globals.Instance.PerkLevel[i])
			{
				return Globals.Instance.PerkLevel[i];
			}
		}
		return 0;
	}

	public int GetPerkPrevLevelPoints(string _subclassId)
	{
		int num = 0;
		num = ((!(_subclassId != "")) ? playerRankProgress : GetProgress(_subclassId));
		int result = 0;
		for (int i = 0; i < Globals.Instance.PerkLevel.Count && num >= Globals.Instance.PerkLevel[i]; i++)
		{
			result = Globals.Instance.PerkLevel[i];
		}
		return result;
	}

	public int GetPerkRank(string _subclassId)
	{
		_subclassId = _subclassId.Replace(" ", "").ToLower();
		int num = 0;
		int progress = GetProgress(_subclassId);
		for (int i = 0; i < Globals.Instance.PerkLevel.Count && progress >= Globals.Instance.PerkLevel[i]; i++)
		{
			num++;
		}
		return num;
	}

	public int GetPerkPointsAvailable(string _subclassId)
	{
		int num = GetHighestCharacterRank();
		if (num > Globals.MaxPerkPoints)
		{
			num = Globals.MaxPerkPoints;
		}
		List<string> list = GetHeroPerks(_subclassId);
		int num2 = 0;
		foreach (KeyValuePair<string, PerkNodeData> perkNodeData in PerkTree.Instance.GetPerkNodeDatas())
		{
			if (perkNodeData.Value != null && perkNodeData.Value.Perk != null && list != null && list.Contains(perkNodeData.Value.Perk.Id.ToLower()))
			{
				int pointsForNode = PerkTree.Instance.GetPointsForNode(perkNodeData.Value);
				num2 += pointsForNode;
			}
		}
		num -= num2;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public void ModifyProgress(string _subclassId, int _quantity, int _index = -1)
	{
		bool flag = true;
		SubClassData subClassData = Globals.Instance.GetSubClassData(_subclassId);
		if (subClassData == null)
		{
			return;
		}
		if (!subClassData.MainCharacter && _index > -1)
		{
			flag = false;
			Hero[] teamBackup = AtOManager.Instance.GetTeamBackup();
			if (teamBackup != null && teamBackup.Length > _index && teamBackup[_index] != null)
			{
				SubClassData subClassData2 = Globals.Instance.GetSubClassData(teamBackup[_index].SubclassName);
				if (subClassData2 != null && subClassData2.MainCharacter)
				{
					_subclassId = teamBackup[_index].SubclassName;
					flag = true;
				}
			}
		}
		if (flag)
		{
			if (heroProgress == null)
			{
				heroProgress = new Dictionary<string, int>();
			}
			if (heroProgress.ContainsKey(_subclassId))
			{
				heroProgress[_subclassId] += _quantity;
			}
			else
			{
				heroProgress.Add(_subclassId, _quantity);
			}
			if (heroProgress[_subclassId] > Globals.Instance.PerkLevel[Globals.Instance.PerkLevel.Count - 1])
			{
				heroProgress[_subclassId] = Globals.Instance.PerkLevel[Globals.Instance.PerkLevel.Count - 1];
			}
			int perkRank = GetPerkRank(_subclassId);
			if (perkRank > 4)
			{
				AchievementUnlock("MISC_RECRUIT");
			}
			if (perkRank > 19)
			{
				AchievementUnlock("MISC_SOLDIER");
			}
			if (perkRank > 34)
			{
				AchievementUnlock("MISC_VETERAN");
			}
		}
	}

	public int GetProgress(string _subclassId)
	{
		_subclassId = _subclassId.ToLower();
		if (heroProgress == null)
		{
			heroProgress = new Dictionary<string, int>();
		}
		if (heroProgress.ContainsKey(_subclassId))
		{
			return heroProgress[_subclassId];
		}
		return 0;
	}

	public void AssignPerkList(string _subclassId, List<string> _perks)
	{
		Debug.Log("AssignPerkList ->" + _subclassId);
		if (!heroPerks.ContainsKey(_subclassId))
		{
			heroPerks.Add(_subclassId, new List<string>());
		}
		heroPerks[_subclassId] = _perks;
		SaveManager.SavePlayerData();
		if (SceneStatic.GetSceneName() == "HeroSelection")
		{
			HeroSelectionManager.Instance.SendHeroPerksMP(_subclassId);
		}
	}

	public void AssignPerk(string _subclassId, string _perk, bool _addPerk = true)
	{
		Debug.Log("AssignPerk " + _subclassId + " " + _perk);
		_perk = _perk.ToLower();
		if (heroPerks.ContainsKey(_subclassId))
		{
			if (_addPerk && !heroPerks[_subclassId].Contains(_perk))
			{
				heroPerks[_subclassId].Add(_perk);
			}
			if (!_addPerk && heroPerks[_subclassId].Contains(_perk))
			{
				heroPerks[_subclassId].Remove(_perk);
			}
			PopupManager.Instance.ClosePopup();
		}
		else
		{
			heroPerks.Add(_subclassId, new List<string>());
			if (_addPerk)
			{
				heroPerks[_subclassId].Add(_perk);
			}
		}
		SaveManager.SavePlayerData();
		if (SceneStatic.GetSceneName() == "HeroSelection")
		{
			HeroSelectionManager.Instance.SendHeroPerksMP(_subclassId);
		}
	}

	public List<string> GetHeroPerks(string _hero, bool forceFromPlayerManager = false)
	{
		if (_hero == "")
		{
			return null;
		}
		_hero = _hero.ToLower().Split("_")[0];
		Hero[] team = AtOManager.Instance.GetTeam();
		Dictionary<string, List<string>> dictionary = ((!forceFromPlayerManager && team != null && team.Length >= 4 && !(SceneStatic.GetSceneName() == "FinishRun")) ? AtOManager.Instance.heroPerks : heroPerks);
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, List<string>>();
		}
		if (dictionary.ContainsKey(_hero))
		{
			return dictionary[_hero];
		}
		return null;
	}

	public bool HeroHavePerk(string _hero, string _perk)
	{
		_hero = _hero.ToLower();
		_perk = _perk.ToLower();
		if (heroPerks == null)
		{
			heroPerks = new Dictionary<string, List<string>>();
		}
		if (heroPerks.ContainsKey(_hero) && heroPerks[_hero].Contains(_perk))
		{
			return true;
		}
		return false;
	}

	public Dictionary<string, List<string>> GetHeroPerksDictionary()
	{
		return heroPerks;
	}

	public void PreBeginGame()
	{
		CreateSkinDictionary();
		CreateCardbackDictionary();
	}

	public void BeginGame()
	{
		CardUnlock("bunny");
		CardUnlock("scaraby");
		CardUnlock("rifty");
		CardUnlock("floaty");
		CardUnlock("inky");
		CardUnlock("jelly");
		UnlockCardsByDefault();
		CardUnlock("fireball");
		CreatePlayer();
		SaveManager.LoadPlayerPerkConfig();
		SaveManager.LoadMutedPlayers();
		if (unlockedHeroes != null)
		{
			for (int i = 0; i < unlockedHeroes.Count; i++)
			{
				string theAchievement = "UNLOCK_" + unlockedHeroes[i].ToUpper();
				AchievementUnlock(theAchievement);
			}
		}
		if (unlockedCards == null || !Globals.Instance.CardItemByType.ContainsKey(Enums.CardType.Pet))
		{
			return;
		}
		for (int j = 0; j < Globals.Instance.CardItemByType[Enums.CardType.Pet].Count; j++)
		{
			string text = Globals.Instance.CardItemByType[Enums.CardType.Pet][j];
			if (unlockedCards.Contains(text))
			{
				string theAchievement2 = "EVENT_UNLOCK_" + text.ToUpper();
				if (text == "liante")
				{
					theAchievement2 = "EVENT_UNLOCK_LIANTA";
				}
				AchievementUnlock(theAchievement2);
			}
		}
	}

	private void UnlockCardsByDefault()
	{
		for (int i = 0; i < defaultCardUnlocks.Count; i++)
		{
			CardUnlock(defaultCardUnlocks[i]);
		}
	}

	public void CreatePlayer()
	{
		playerName = Functions.RandomString(6f);
	}

	public void CreateSkinDictionary()
	{
		skinUsed = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		foreach (KeyValuePair<string, SubClassData> item in Globals.Instance.SubClass)
		{
			string skinBaseIdBySubclass = Globals.Instance.GetSkinBaseIdBySubclass(item.Key);
			if (!(skinBaseIdBySubclass == ""))
			{
				skinUsed.Add(item.Key, skinBaseIdBySubclass);
				AtOManager.Instance.SetSkinIntoSubclassData(item.Key, skinBaseIdBySubclass);
			}
		}
	}

	public void RecreateSkins()
	{
		if (skinUsed == null)
		{
			return;
		}
		foreach (KeyValuePair<string, SubClassData> item in Globals.Instance.SubClass)
		{
			if (skinUsed.ContainsKey(item.Key))
			{
				string skinId = skinUsed[item.Key];
				AtOManager.Instance.SetSkinIntoSubclassData(item.Key, skinId);
			}
		}
	}

	public void SetSkin(string _subclass, string _skinId)
	{
		_subclass = _subclass.ToLower().Replace(" ", "");
		if (skinUsed == null)
		{
			skinUsed = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}
		if (skinUsed.ContainsKey(_subclass))
		{
			skinUsed[_subclass] = _skinId;
		}
		else
		{
			skinUsed.Add(_subclass, _skinId);
		}
		SaveManager.SavePlayerData();
	}

	public void CreateCardbackDictionary()
	{
		cardbackUsed = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		foreach (KeyValuePair<string, SubClassData> item in Globals.Instance.SubClass)
		{
			string cardbackBaseIdBySubclass = Globals.Instance.GetCardbackBaseIdBySubclass(item.Key);
			if (!(cardbackBaseIdBySubclass == ""))
			{
				cardbackUsed.Add(item.Key, cardbackBaseIdBySubclass);
			}
		}
	}

	public void SetCardback(string _subclass, string _cardbackId)
	{
		_subclass = _subclass.ToLower();
		_cardbackId = _cardbackId.ToLower();
		if (cardbackUsed == null)
		{
			cardbackUsed = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}
		if (cardbackUsed.ContainsKey(_subclass))
		{
			cardbackUsed[_subclass] = _cardbackId;
		}
		else
		{
			cardbackUsed.Add(_subclass, _cardbackId);
		}
		SaveManager.SavePlayerData();
	}

	public string GetActiveCardback(string _subclass)
	{
		_subclass = _subclass.ToLower();
		if (cardbackUsed != null && cardbackUsed.ContainsKey(_subclass))
		{
			return cardbackUsed[_subclass].ToLower();
		}
		return Globals.Instance.GetCardbackBaseIdBySubclass(_subclass);
	}

	public void SetPlayerName(string _name)
	{
		playerName = _name;
		SaveManager.SaveIntoPrefsString("LobbyNick", _name);
	}

	public string GetActiveSkin(string _subclass)
	{
		_subclass = _subclass.ToLower();
		if (skinUsed != null && skinUsed.ContainsKey(_subclass) && skinUsed[_subclass] != "")
		{
			return skinUsed[_subclass];
		}
		return Globals.Instance.GetSkinBaseIdBySubclass(_subclass);
	}

	public string GetPlayerName()
	{
		return playerName;
	}

	public void UnlockInitialHeroes()
	{
		foreach (KeyValuePair<string, SubClassData> item in Globals.Instance.SubClass)
		{
			if (item.Value.InitialUnlock)
			{
				HeroUnlock(item.Value.Id, save: false, achievement: false);
			}
		}
	}

	public void NodeUnlock(string nodeName)
	{
		if (unlockedNodes == null)
		{
			unlockedNodes = new List<string>();
		}
		if (!unlockedNodes.Contains(nodeName))
		{
			unlockedNodes.Add(nodeName);
			SaveManager.SavePlayerData();
		}
	}

	public bool IsNodeUnlocked(string nodeName)
	{
		if (unlockedNodes == null)
		{
			return false;
		}
		if (!unlockedNodes.Contains(nodeName))
		{
			return false;
		}
		return true;
	}

	public void HeroUnlock(string subclass, bool save = true, bool achievement = true)
	{
		if (unlockedHeroes == null)
		{
			unlockedHeroes = new List<string>();
		}
		if (!unlockedHeroes.Contains(subclass))
		{
			unlockedHeroes.Add(subclass);
			CardsUnlockHero(subclass);
			ItemsUnlockHero(subclass);
			if (save)
			{
				SaveManager.SavePlayerData();
			}
			SubClassData subClassData = Globals.Instance.GetSubClassData(subclass);
			if (subClassData != null && !subClassData.InitialUnlock)
			{
				Telemetry.SendUnlock("character", subClassData.CharacterName);
			}
		}
		if (achievement)
		{
			string theAchievement = "UNLOCK_" + subclass.ToUpper();
			AchievementUnlock(theAchievement);
		}
	}

	public bool IsHeroUnlocked(string subclass)
	{
		if (unlockedHeroes == null)
		{
			return false;
		}
		if (!unlockedHeroes.Contains(subclass.ToLower()))
		{
			return false;
		}
		return true;
	}

	private void CardsUnlockHero(string subclass)
	{
		List<string> cardsId = Globals.Instance.GetSubClassData(subclass).GetCardsId();
		for (int i = 0; i < cardsId.Count; i++)
		{
			CardUnlock(cardsId[i]);
		}
	}

	private void ItemsUnlockHero(string subclass)
	{
		SubClassData subClassData = Globals.Instance.GetSubClassData(subclass);
		if (subClassData.Item != null)
		{
			CardUnlock(subClassData.Item.Id);
		}
	}

	public void UnlockHeroes(bool save = true)
	{
		foreach (KeyValuePair<string, SubClassData> item in Globals.Instance.SubClass)
		{
			HeroUnlock(item.Key, save);
		}
		if (save)
		{
			SaveManager.SavePlayerData();
		}
	}

	public void UnlockCards(bool save = true)
	{
		for (int i = 0; i < Globals.Instance.CardListNotUpgraded.Count; i++)
		{
			CardUnlock(Globals.Instance.CardListNotUpgraded[i], save);
		}
		if (save)
		{
			SaveManager.SavePlayerData();
		}
	}

	public void UnlockAllExceptHeroes()
	{
		UnlockCards(save: false);
		SingularityMadnessLevel = 10;
	}

	public void CardUnlock(string cardId, bool save = false, CardItem cardItem = null)
	{
		if (GameManager.Instance.IsObeliskChallenge())
		{
			return;
		}
		if (unlockedCards == null)
		{
			unlockedCards = new List<string>();
		}
		if (unlockedCardsByGame == null)
		{
			unlockedCardsByGame = new Dictionary<string, List<string>>();
		}
		string text = "";
		int num = 0;
		CardData cardData = null;
		while (text == "")
		{
			cardData = Globals.Instance.GetCardData(cardId, instantiate: false);
			if (cardData != null)
			{
				if (cardData.UpgradedFrom.Trim() == "")
				{
					text = cardData.Id;
				}
				else
				{
					cardId = cardData.UpgradedFrom.Trim();
				}
				num++;
				if (num > 5)
				{
					return;
				}
				continue;
			}
			return;
		}
		if (unlockedCards.Contains(text))
		{
			return;
		}
		unlockedCards.Add(text);
		cardData = Globals.Instance.GetCardData(text, instantiate: false);
		if (cardData != null && cardData.CardType == Enums.CardType.Pet)
		{
			Telemetry.SendUnlock("pet", text);
		}
		if (!AtOManager.Instance.unlockedCards.Contains(text))
		{
			AtOManager.Instance.unlockedCards.Add(text);
		}
		string text2 = AtOManager.Instance.GetGameId().Trim();
		if (!(text2 != ""))
		{
			return;
		}
		if (!unlockedCardsByGame.ContainsKey(text2))
		{
			unlockedCardsByGame.Add(text2, new List<string>());
		}
		if (!unlockedCardsByGame[text2].Contains(text))
		{
			unlockedCardsByGame[text2].Add(text);
		}
		if (save)
		{
			SaveManager.SavePlayerData();
		}
		if (cardItem != null)
		{
			cardItem.ShowUnlocked();
		}
		List<string> list = Globals.Instance.CardListNotUpgradedByClass[cardData.CardClass];
		int count = list.Count;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			if (IsCardUnlocked(list[i]))
			{
				num2++;
			}
		}
		string text3 = Enum.GetName(typeof(Enums.CardClass), cardData.CardClass).ToUpper();
		string theAchievement = text3 + "_CARDS_30";
		if (num2 >= 30)
		{
			AchievementUnlock(theAchievement);
		}
		theAchievement = text3 + "_CARDS_60";
		if (num2 >= 60)
		{
			AchievementUnlock(theAchievement);
		}
		theAchievement = text3 + "_CARDS_90";
		if (num2 >= 90)
		{
			AchievementUnlock(theAchievement);
		}
	}

	public void AchievementUnlock(string theAchievement)
	{
		if (!achievementsSent.Contains(theAchievement) && !AtOManager.Instance.IsCombatTool)
		{
			SteamManager.Instance.AchievementUnlock(theAchievement);
			achievementsSent.Add(theAchievement);
		}
	}

	public void BossKilled(string bossName, string bossNameInGame, string scriptableObjectName)
	{
		bossesKilled++;
		if (bossesKilledName == null)
		{
			bossesKilledName = new List<string>();
		}
		if (!bossesKilledName.Contains(bossName))
		{
			bossesKilledName.Add(bossName);
		}
		if (bossesKilled > 9)
		{
			AchievementUnlock("MISC_PUNISHER");
		}
		if (bossesKilled > 49)
		{
			AchievementUnlock("MISC_EXECUTIONER");
		}
		string text = "BOSS_" + bossNameInGame.Replace(" ", "").ToUpper();
		AchievementUnlock(text);
		if (AtOManager.Instance.GetNgPlus() > 0)
		{
			text += "_NG";
		}
		AchievementUnlock(text);
		if (scriptableObjectName != "")
		{
			text = "BOSS_" + scriptableObjectName.Replace(" ", "").ToUpper();
			AchievementUnlock(text);
			if (AtOManager.Instance.GetNgPlus() > 0)
			{
				text += "_NG";
			}
			AchievementUnlock(text);
		}
	}

	public void MonsterKilled()
	{
		monstersKilled++;
		if (monstersKilled > 19)
		{
			AchievementUnlock("MISC_BLOODSHED");
		}
		if (monstersKilled > 199)
		{
			AchievementUnlock("MISC_MASSACRE");
		}
		if (monstersKilled > 499)
		{
			AchievementUnlock("MISC_EXTERMINATION");
		}
	}

	public void ExpGainedSum(int quantity)
	{
		expGained += quantity;
	}

	public void GainSupply(int quantity)
	{
		if (supplyActual < 500)
		{
			if (supplyActual + quantity > 500)
			{
				quantity = 500 - supplyActual;
			}
			supplyActual += quantity;
		}
		supplyGained += quantity;
		PlayerUIManager.Instance.SetSupply(animation: true);
	}

	public void SpendSupply(int quantity)
	{
		if (supplyActual >= quantity)
		{
			supplyActual -= quantity;
			PlayerUIManager.Instance.SetSupply(animation: true);
		}
	}

	public int GetPlayerSupplyActual()
	{
		return supplyActual;
	}

	public bool PlayerHaveSupply(string supplyId)
	{
		if (GameManager.Instance.IsGameAdventure() && supplyBought != null)
		{
			return supplyBought.Contains(supplyId);
		}
		return false;
	}

	public void PlayerBuySupply(string supplyId)
	{
		if (supplyBought == null)
		{
			supplyBought = new List<string>();
		}
		if (supplyBought.Contains(supplyId))
		{
			return;
		}
		int num = PointsRequiredForSupply(supplyId);
		if (supplyActual < num)
		{
			return;
		}
		string text = SupplyRequiredForSupply(supplyId);
		if (text != "" && !supplyBought.Contains(text))
		{
			return;
		}
		SpendSupply(num);
		supplyBought.Add(supplyId);
		if (supplyId == "townUpgrade_6_4")
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("caravan"));
			}
			else if (NetworkManager.Instance.IsMaster())
			{
				AtOManager.Instance.AddPlayerRequirement(Globals.Instance.GetRequirementData("caravan"));
				AtOManager.Instance.AddPlayerRequirementOthers("caravan");
			}
		}
		if (TotalPointsSpentInSupplys() > 0)
		{
			AchievementUnlock("TOWN_TAXPAYER");
		}
		if (TotalPointsSpentInSupplys() > 29)
		{
			AchievementUnlock("TOWN_MAYOR");
		}
		if (TotalPointsSpentInSupplys() > 249)
		{
			AchievementUnlock("TOWN_GOVERNOR");
		}
		SaveManager.SavePlayerData();
	}

	public void ResetTownUpgrade()
	{
		supplyBought = new List<string>();
		SaveManager.SavePlayerData();
	}

	public string SupplyRequiredForSupply(string supplyId)
	{
		string[] array = supplyId.Split('_');
		if (array.Length > 1)
		{
			int num = int.Parse(array[2]);
			if (num > 1)
			{
				return array[0] + "_" + array[1] + "_" + (num - 1);
			}
		}
		return "";
	}

	public int PointsRequiredForSupply(string supplyId)
	{
		string[] array = supplyId.Split('_');
		if (array.Length > 1)
		{
			int num = int.Parse(array[2]);
			int num2 = 0;
			switch (num)
			{
			case 1:
				num2 = 1;
				break;
			case 2:
				num2 = 3;
				break;
			case 3:
				num2 = 6;
				break;
			case 4:
				num2 = 9;
				break;
			case 5:
				num2 += 12;
				break;
			case 6:
				num2 = 15;
				break;
			}
			return num2;
		}
		return 0;
	}

	public int TotalPointsSpentInSupplys()
	{
		int num = 0;
		if (supplyBought != null)
		{
			for (int i = 0; i < supplyBought.Count; i++)
			{
				num += PointsRequiredForSupply(supplyBought[i]);
			}
		}
		return num;
	}

	public void PurchasedItem()
	{
		purchasedItems++;
		if (purchasedItems > 4)
		{
			AchievementUnlock("TOWN_CASUALSHOOPING");
		}
		if (purchasedItems > 99)
		{
			AchievementUnlock("TOWN_CAPITALISM");
		}
		SaveManager.SavePlayerData();
	}

	public void CorruptionCompleted()
	{
		corruptionsCompleted++;
		SaveManager.SavePlayerData();
	}

	public void CardCrafted()
	{
		cardsCrafted++;
		if (cardsCrafted > 4)
		{
			AchievementUnlock("TOWN_CRAFTMANASSISTANT");
		}
		if (cardsCrafted > 99)
		{
			AchievementUnlock("TOWN_CRAFTMAN");
		}
		if (cardsCrafted > 249)
		{
			AchievementUnlock("TOWN_MASTERCRAFTMAN");
		}
		SaveManager.SavePlayerData();
	}

	public void CardUpgraded()
	{
		cardsUpgraded++;
		if (cardsUpgraded > 4)
		{
			AchievementUnlock("TOWN_INITIATE");
		}
		if (cardsUpgraded > 99)
		{
			AchievementUnlock("TOWN_ADEPT");
		}
		if (cardsUpgraded > 499)
		{
			AchievementUnlock("TOWN_SCHOLAR");
		}
		SaveManager.SavePlayerData();
	}

	public void GoldGainedSum(int quantity, bool save = true)
	{
		goldGained += quantity;
		if (goldGained > 4999)
		{
			AchievementUnlock("MISC_ENTREPRENEUR");
		}
		if (goldGained > 99999)
		{
			AchievementUnlock("MISC_FILTHYRICH");
		}
		if (save)
		{
			SaveManager.SavePlayerData();
		}
	}

	public void DustGainedSum(int quantity, bool save = true)
	{
		dustGained += quantity;
		if (dustGained > 4999)
		{
			AchievementUnlock("MISC_ALCHEMIST");
		}
		if (dustGained > 99999)
		{
			AchievementUnlock("MISC_SPELLCRAFTER");
		}
		if (save)
		{
			SaveManager.SavePlayerData();
		}
	}

	public void SetBestScore(int score)
	{
		bestScore = score;
	}

	public bool IsCardUnlocked(string _cardId)
	{
		if ((bool)CardCraftManager.Instance && !TomeManager.Instance.IsActive() && SandboxManager.Instance.IsEnabled() && AtOManager.Instance.Sandbox_craftUnlocked)
		{
			return true;
		}
		string id = _cardId.ToLower().Split('_')[0];
		string text = "";
		while (text == "")
		{
			CardData cardData = Globals.Instance.GetCardData(id, instantiate: false);
			if (cardData != null)
			{
				if (cardData.CardClass == Enums.CardClass.Monster || cardData.CardClass == Enums.CardClass.Special)
				{
					return true;
				}
				if (cardData.UpgradedFrom.Trim() == "")
				{
					text = cardData.Id;
				}
				else
				{
					id = cardData.UpgradedFrom.Trim();
				}
				continue;
			}
			return true;
		}
		if (unlockedCards == null)
		{
			return false;
		}
		return unlockedCards.Contains(text);
	}

	public bool IsCardbackUnlocked(string _cardbackId)
	{
		_cardbackId = _cardbackId.ToLower();
		if (unlockedCardbacks != null && unlockedCardbacks.Contains(_cardbackId))
		{
			return true;
		}
		return false;
	}

	public void ClaimTreasure(string id, bool save = true)
	{
		if (treasuresClaimed == null)
		{
			treasuresClaimed = new List<string>();
		}
		if (treasuresClaimed.Contains(id))
		{
			return;
		}
		treasuresClaimed.Add(id);
		AchievementUnlock("MISC_FIRSTREWARD");
		if (treasuresClaimed.Count > 24)
		{
			AchievementUnlock("MISC_TREASUREHOARDER");
		}
		if (save)
		{
			SaveManager.SavePlayerData();
			if (!GameManager.Instance.IsMultiplayer())
			{
				AtOManager.Instance.SaveGame();
			}
		}
	}

	public bool IsTreasureClaimed(string treasureId)
	{
		if (treasuresClaimed == null)
		{
			return false;
		}
		if (!treasuresClaimed.Contains(treasureId))
		{
			return false;
		}
		return true;
	}

	public void SetMaxAdventureMadnessLevel()
	{
		int madnessDifficulty = AtOManager.Instance.GetMadnessDifficulty();
		if (maxAdventureMadnessLevel < madnessDifficulty)
		{
			maxAdventureMadnessLevel = madnessDifficulty;
		}
	}

	public void IncreaseNg()
	{
		if (!SandboxManager.Instance.IsEnabled())
		{
			ngUnlocked = true;
			int ngPlus = AtOManager.Instance.GetNgPlus();
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("current NG " + ngPlus, "trace");
			}
			ngPlus++;
			if (ngPlus > 9)
			{
				ngPlus = 9;
			}
			if (ngPlus > ngLevel)
			{
				ngLevel = ngPlus;
			}
			if (ngLevel < 3)
			{
				ngLevel = 3;
			}
			SetMaxAdventureMadnessLevel();
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("final NG " + ngLevel, "general");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("final MaxMadness " + maxAdventureMadnessLevel, "general");
			}
			SaveManager.SavePlayerData();
		}
	}

	public void IncreaseObeliskMadness()
	{
		int obeliskMadness = AtOManager.Instance.GetObeliskMadness();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("current ObeliskLevel " + obeliskMadness, "trace");
		}
		obeliskMadness++;
		if (obeliskMadness > 10)
		{
			obeliskMadness = 10;
		}
		if (obeliskMadness > obeliskMadnessLevel)
		{
			obeliskMadnessLevel = obeliskMadness;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("final Obelisk " + obeliskMadnessLevel, "trace");
		}
		SaveManager.SavePlayerData();
	}

	public void IncreaseSingularityMadness()
	{
		if (!SandboxManager.Instance.IsEnabled())
		{
			int singularityMadness = AtOManager.Instance.GetSingularityMadness();
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("current SingularityLevel " + singularityMadness, "trace");
			}
			singularityMadness++;
			if (singularityMadness > 10)
			{
				singularityMadness = 10;
			}
			if (singularityMadness > singularityMadnessLevel)
			{
				singularityMadnessLevel = singularityMadness;
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("final Singularity " + singularityMadnessLevel, "trace");
			}
			SaveManager.SavePlayerData();
		}
	}

	public bool IsNgUnlocked()
	{
		if (ngUnlocked && ngLevel == 0)
		{
			ngLevel = 1;
		}
		return ngUnlocked;
	}

	public void TeamAddNPC(string id, int position)
	{
		teamNPCs[position] = id;
	}

	public bool PlayerHaveHero(string subclass)
	{
		return false;
	}

	public Hero[] GetTeamHero()
	{
		return gameTeamHeroes;
	}

	public void TeamAddHero(string subclass, int arrayPosition)
	{
		Hero hero = GameManager.Instance.CreateHero(subclass);
		if (gameTeamHeroes == null)
		{
			CreateTeamForGame();
		}
		gameTeamHeroes[arrayPosition] = hero;
	}

	private void CreateTeamForGame()
	{
		gameTeamHeroes = new Hero[4];
	}

	public void ClearUnlockedCardsByGame()
	{
		UnlockedCardsByGame = new Dictionary<string, List<string>>();
	}
}
