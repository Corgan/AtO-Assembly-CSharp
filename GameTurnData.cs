using System;

[Serializable]
public class GameTurnData
{
	private string turnId = "";

	private string turnCombatData = "";

	private string turnData = "";

	private string turnCombatDictionaryKeys = "";

	private string turnCombatDictionaryValues = "";

	private string turnHeroItems = "";

	private string turnNPCParams = "";

	private string turnCombatStatsEffects = "";

	private string turnCombatStatsCurrent = "";

	private string turnHeroLifeArr = "";

	private string itemExecutionInCombat = "";

	private int currentSpecialCardsUsedInMatch;

	private bool draculaDoSwap = true;

	private int draculaConsecutiveHits;

	public void FillData()
	{
		turnId = AtOManager.Instance.GetGameId() + "_" + AtOManager.Instance.mapVisitedNodes.Count + "_" + AtOManager.Instance.currentMapNode;
		CombatData currentCombatData = AtOManager.Instance.GetCurrentCombatData();
		if (currentCombatData == null)
		{
			turnCombatData = "";
		}
		else
		{
			turnCombatData = currentCombatData.CombatId;
		}
		turnData = MatchManager.Instance.CurrentGameCodeForReload;
		turnCombatDictionaryKeys = MatchManager.Instance.GetCardDictionaryKeys();
		turnCombatDictionaryValues = MatchManager.Instance.GetCardDictionaryValues();
		turnHeroItems = MatchManager.Instance.GetHeroItemsForTurnSave();
		turnCombatStatsEffects = MatchManager.Instance.GetCombatStatsForTurnSave();
		turnCombatStatsCurrent = MatchManager.Instance.GetCombatStatsCurrentForTurnSave();
		turnHeroLifeArr = MatchManager.Instance.GetHeroLifeArrForTurnSave();
		itemExecutionInCombat = MatchManager.Instance.GetItemExecutionInCombatForTurnSave();
		currentSpecialCardsUsedInMatch = MatchManager.Instance.MindSpikeAbility.CurrentSpecialCardsUsedInMatch;
		turnNPCParams = MatchManager.Instance.GetNPCParamsForTurnSave();
	}

	public void LoadData()
	{
		if (!(AtOManager.Instance.GetGameId() + "_" + AtOManager.Instance.mapVisitedNodes.Count + "_" + AtOManager.Instance.currentMapNode != turnId))
		{
			CombatData currentCombatData = AtOManager.Instance.GetCurrentCombatData();
			if ((!(currentCombatData == null) || !(turnCombatData != "")) && (!(currentCombatData != null) || !(currentCombatData.CombatId != turnCombatData)))
			{
				MatchManager.Instance.SetLoadTurn(turnData, turnCombatDictionaryKeys, turnCombatDictionaryValues, turnHeroItems, turnCombatStatsEffects, turnCombatStatsCurrent, turnHeroLifeArr, itemExecutionInCombat, currentSpecialCardsUsedInMatch.ToString(), turnNPCParams);
			}
		}
	}
}
