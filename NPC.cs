using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NPC : Character
{
	private CardItem currentCardItem;

	private int priorityOffsetter;

	public CardItem GetCurrentCardItem()
	{
		return currentCardItem;
	}

	public override void InitData()
	{
		if (!(base.NpcData != null))
		{
			return;
		}
		base.GameName = base.NpcData.Id;
		base.ScriptableObjectName = base.NpcData.ScriptableObjectName;
		base.NpcData.NPCName = Texts.Instance.GetText(base.GameName + "_name", "monsters");
		base.SourceName = base.NpcData.NPCName;
		string text = (base.SubclassName = "Monster");
		base.ClassName = text;
		base.SpriteSpeed = base.NpcData.SpriteSpeed;
		base.SpritePortrait = base.NpcData.SpritePortrait;
		base.Id = base.NpcData.Id + "_" + base.InternalId;
		int num = (base.HpCurrent = base.NpcData.Hp);
		base.Hp = num;
		num = (base.EnergyCurrent = 10000);
		base.Energy = num;
		base.EnergyTurn = 0;
		base.Speed = base.NpcData.Speed;
		base.IsHero = false;
		base.ResistSlashing = base.NpcData.ResistSlashing;
		if (base.ResistSlashing >= 100)
		{
			base.ImmuneSlashing = true;
		}
		base.ResistBlunt = base.NpcData.ResistBlunt;
		if (base.ResistBlunt >= 100)
		{
			base.ImmuneBlunt = true;
		}
		base.ResistPiercing = base.NpcData.ResistPiercing;
		if (base.ResistPiercing >= 100)
		{
			base.ImmunePiercing = true;
		}
		base.ResistFire = base.NpcData.ResistFire;
		if (base.ResistFire >= 100)
		{
			base.ImmuneFire = true;
		}
		base.ResistCold = base.NpcData.ResistCold;
		if (base.ResistCold >= 100)
		{
			base.ImmuneCold = true;
		}
		base.ResistLightning = base.NpcData.ResistLightning;
		if (base.ResistLightning >= 100)
		{
			base.ImmuneLightning = true;
		}
		base.ResistMind = base.NpcData.ResistMind;
		if (base.ResistMind >= 100)
		{
			base.ImmuneMind = true;
		}
		base.ResistHoly = base.NpcData.ResistHoly;
		if (base.ResistHoly >= 100)
		{
			base.ImmuneHoly = true;
		}
		base.ResistShadow = base.NpcData.ResistShadow;
		if (base.ResistShadow >= 100)
		{
			base.ImmuneShadow = true;
		}
		bool flag = true;
		string text3 = "";
		for (int i = 0; i < base.NpcData.AuracurseImmune.Count; i++)
		{
			text3 = base.NpcData.AuracurseImmune[i].Trim().ToLower();
			flag = true;
			if (text3 == "bleed" && AtOManager.Instance.TeamHavePerk("mainperkbleed2c"))
			{
				flag = false;
			}
			if (text3 == "sight" && AtOManager.Instance.TeamHavePerk("mainperksight1c"))
			{
				flag = false;
			}
			if (flag && !base.AuracurseImmune.Contains(text3))
			{
				base.AuracurseImmune.Add(text3);
			}
		}
		base.Alive = true;
		base.AuraList = new List<Aura>();
		if (base.NpcData.AICards != null)
		{
			SetInitalCards(base.NpcData);
		}
		switch (AtOManager.Instance.GetNgPlus())
		{
		case 1:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp - (float)base.Hp * 0.1f));
				base.Hp = num;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp - (float)base.Hp * 0.05f));
				base.Hp = num;
			}
			else if (AtOManager.Instance.GetTownTier() != 2 && AtOManager.Instance.GetTownTier() != 3)
			{
			}
			break;
		case 2:
			if (AtOManager.Instance.GetTownTier() != 0 && AtOManager.Instance.GetTownTier() != 1)
			{
				if (AtOManager.Instance.GetTownTier() == 2)
				{
					num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.1f));
					base.Hp = num;
					base.Speed++;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.15f));
					base.Hp = num;
					base.Speed++;
				}
			}
			break;
		case 3:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.05f));
				base.Hp = num;
				base.Speed++;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.1f));
				base.Hp = num;
				base.Speed++;
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.15f));
				base.Hp = num;
				base.Speed++;
			}
			else if (AtOManager.Instance.GetTownTier() == 3)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.2f));
				base.Hp = num;
				base.Speed++;
			}
			break;
		case 4:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.15f));
				base.Hp = num;
				base.Speed++;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.25f));
				base.Hp = num;
				base.Speed++;
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.3f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 3)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.35f));
				base.Hp = num;
				base.Speed += 2;
			}
			break;
		case 5:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.25f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.35f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.4f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 3)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.45f));
				base.Hp = num;
				base.Speed += 2;
			}
			break;
		case 6:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.3f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.45f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.5f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 3)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.55f));
				base.Hp = num;
				base.Speed += 3;
			}
			break;
		case 7:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.35f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.5f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.55f));
				base.Hp = num;
				base.Speed += 3;
			}
			else if (AtOManager.Instance.GetTownTier() == 3)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.6f));
				base.Hp = num;
				base.Speed += 3;
			}
			break;
		case 8:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.4f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.55f));
				base.Hp = num;
				base.Speed += 3;
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.6f));
				base.Hp = num;
				base.Speed += 3;
			}
			else if (AtOManager.Instance.GetTownTier() == 3)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.65f));
				base.Hp = num;
				base.Speed += 3;
			}
			break;
		case 9:
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.45f));
				base.Hp = num;
				base.Speed += 2;
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.6f));
				base.Hp = num;
				base.Speed += 3;
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.65f));
				base.Hp = num;
				base.Speed += 3;
			}
			else if (AtOManager.Instance.GetTownTier() == 3)
			{
				num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.7f));
				base.Hp = num;
				base.Speed += 4;
			}
			break;
		}
		if (AtOManager.Instance.IsChallengeTraitActive("fastmonsters"))
		{
			base.Speed++;
		}
		if (AtOManager.Instance.IsChallengeTraitActive("slowmonsters"))
		{
			base.Speed--;
		}
		if (AtOManager.Instance.IsChallengeTraitActive("ludicrousspeed"))
		{
			base.Speed += 3;
		}
		if (AtOManager.Instance.IsChallengeTraitActive("vigorousmonsters"))
		{
			num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.25f));
			base.Hp = num;
		}
		if (AtOManager.Instance.IsChallengeTraitActive("gargantuanmonsters"))
		{
			num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * 0.5f));
			base.Hp = num;
		}
		if (AtOManager.Instance.IsChallengeTraitActive("punymonsters"))
		{
			num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp - (float)base.Hp * 0.25f));
			base.Hp = num;
		}
		if (AtOManager.Instance.Sandbox_additionalMonsterHP != 0)
		{
			num = (base.HpCurrent = Functions.FuncRoundToInt((float)base.Hp + (float)base.Hp * ((float)AtOManager.Instance.Sandbox_additionalMonsterHP * 0.01f)));
			base.Hp = num;
		}
		if (base.Hp <= 0)
		{
			num = (base.HpCurrent = 1);
			base.Hp = num;
		}
	}

	public bool NPCIsBoss()
	{
		if (base.NpcData != null)
		{
			return base.NpcData.IsBoss;
		}
		return false;
	}

	public bool IsBigModel()
	{
		if (base.NpcData != null)
		{
			return base.NpcData.BigModel;
		}
		return false;
	}

	private void SetInitalCards(NPCData npcData)
	{
		base.Cards = new List<string>();
		if (npcData.AICards == null)
		{
			return;
		}
		for (int i = 0; i < npcData.AICards.Length; i++)
		{
			for (int j = 0; j < npcData.AICards[i].UnitsInDeck; j++)
			{
				base.Cards.Add(npcData.AICards[i].Card.Id);
			}
		}
	}

	private float GetCardPriorityValue(string cardName)
	{
		int currentRound = MatchManager.Instance.GetCurrentRound();
		for (int i = 0; i < base.NpcData.AICards.Length; i++)
		{
			if (cardName == base.NpcData.AICards[i].Card.Id && base.NpcData.AICards[i].AddCardRound == currentRound)
			{
				return (float)base.NpcData.AICards[i].Priority + 0.001f * base.NpcData.AICards[i].PercentToCast;
			}
		}
		priorityOffsetter++;
		return 10000 + priorityOffsetter;
	}

	public string GetCardPriorityText(string _cardName)
	{
		return "";
	}

	public void CheckRevealCardsCurse()
	{
		if (MatchManager.Instance.CountNPCHand(base.NPCIndex) < 1)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < base.AuraList.Count; i++)
		{
			if (base.AuraList[i] != null && base.AuraList[i].ACData != null && base.AuraList[i].ACData.RevealCardsPerCharge > 0)
			{
				num += base.AuraList[i].ACData.RevealCardsPerCharge * base.AuraList[i].AuraCharges;
			}
		}
		if (num > 0)
		{
			RevealCards(num);
		}
	}

	public void RevealCards(int numCards)
	{
		if (base.NPCItem.cardsCI.Length == 0)
		{
			return;
		}
		int num = MatchManager.Instance.CountNPCHand(base.NPCIndex);
		if (num < 1 || numCards < 1)
		{
			return;
		}
		if (num <= numCards)
		{
			for (int i = 0; i < base.NPCItem.cardsCI.Length; i++)
			{
				if (base.NPCItem.cardsCI[i] != null)
				{
					base.NPCItem.cardsCI[i].RevealCard();
				}
			}
			return;
		}
		int num2 = 0;
		for (int j = 0; j < base.NPCItem.cardsCI.Length; j++)
		{
			if (base.NPCItem.cardsCI[j] != null && base.NPCItem.cardsCI[j].IsRevealed())
			{
				num2++;
			}
		}
		if (num2 >= numCards)
		{
			return;
		}
		for (int k = 0; k < numCards - num2; k++)
		{
			int num3 = -1;
			bool flag = false;
			int num4 = 0;
			while (num4 < 50 && !flag)
			{
				num3 = MatchManager.Instance.GetRandomIntRange(0, base.NPCItem.cardsCI.Length);
				if (base.NPCItem.cardsCI[num3] != null && !base.NPCItem.cardsCI[num3].IsRevealed())
				{
					base.NPCItem.cardsCI[num3].RevealCard();
					flag = true;
				}
				else
				{
					num4++;
				}
			}
		}
	}

	public void UnrevealAllCards()
	{
		if (base.NPCItem.cardsCI.Length == 0)
		{
			return;
		}
		MatchManager.Instance.CountNPCHand(base.NPCIndex);
		for (int i = 0; i < base.NPCItem.cardsCI.Length; i++)
		{
			if (base.NPCItem.cardsCI[i] != null)
			{
				base.NPCItem.cardsCI[i].UnrevealCard();
			}
		}
	}

	public void RedrawRevealedCards()
	{
		for (int i = 0; i < base.NPCItem.cardsCI.Length; i++)
		{
			if (base.NPCItem.cardsCI[i] != null && base.NPCItem.cardsCI[i].cardrevealed)
			{
				base.NPCItem.cardsCI[i].RedrawDescriptionPrecalculatedNPC(this);
			}
		}
	}

	public override void BeginTurn()
	{
		base.BeginTurn();
	}

	public override void BeginTurnPerks()
	{
		base.BeginTurnPerks();
	}

	public override void BeginRound()
	{
		base.BeginRound();
		if (base.NpcData != null && base.NpcData.AICards != null)
		{
			for (int i = 0; i < base.NpcData.AICards.Length; i++)
			{
				if (base.NpcData.AICards[i] != null && base.NpcData.AICards[i].AddCardRound == MatchManager.Instance.GetCurrentRound())
				{
					for (int j = 0; j < base.NpcData.AICards[i].UnitsInDeck; j++)
					{
						MatchManager.Instance.AddCardToNPCDeck(base.NPCIndex, base.NpcData.AICards[i].Card.Id, base.InternalId);
					}
				}
			}
		}
		CreateOverDeck(getCardFromDeck: true);
	}

	public override void CreateOverDeck(bool getCardFromDeck, bool maxOneCard = false)
	{
		priorityOffsetter = 0;
		if (base.NpcData == null)
		{
			return;
		}
		int num = (maxOneCard ? 1 : base.NpcData.CardsInHand);
		if (num < 1)
		{
			num = 1;
		}
		List<CardItem> list = new List<CardItem>();
		List<Transform> list2 = new List<Transform>();
		int num2 = 0;
		GameObject gameObject;
		for (int i = 0; i < num; i++)
		{
			if (getCardFromDeck)
			{
				MatchManager.Instance.GetCardFromDeckToHandNPC(base.NPCIndex);
			}
			gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, base.NPCItem.cardsGOT);
			gameObject.gameObject.SetActive(value: false);
			gameObject.transform.localScale = Vector3.zero;
			list2.Add(gameObject.transform);
			CardItem component = gameObject.GetComponent<CardItem>();
			component.SetCard(MatchManager.Instance.CardFromNPCHand(base.NPCIndex, i), deckScale: false, null, this);
			gameObject.name = component.InternalId;
			if (component.CardData != null && component.CardData.Corrupted)
			{
				num2++;
			}
			list.Add(component);
		}
		if (num2 > 0)
		{
			for (int j = num; j < num + num2; j++)
			{
				gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, base.NPCItem.cardsGOT);
				gameObject.gameObject.SetActive(value: false);
				gameObject.transform.localScale = Vector3.zero;
				if (getCardFromDeck)
				{
					MatchManager.Instance.GetCardFromDeckToHandNPC(base.NPCIndex);
				}
				list2.Add(gameObject.transform);
				CardItem component2 = gameObject.GetComponent<CardItem>();
				component2.SetCard(MatchManager.Instance.CardFromNPCHand(base.NPCIndex, j), deckScale: false, null, this);
				list.Add(component2);
			}
		}
		gameObject = null;
		for (int num3 = list.Count - 1; num3 >= 0; num3--)
		{
			if (list[num3] == null || list[num3].CardData == null)
			{
				list.RemoveAt(num3);
				list2.RemoveAt(num3);
			}
		}
		base.NPCItem.cardsCI = new CardItem[list.Count];
		base.NPCItem.cardsT = new Transform[list.Count];
		base.NPCItem.cardsCI = list.ToArray();
		base.NPCItem.cardsT = list2.ToArray();
		List<AICards> list3 = new List<AICards>();
		for (int num4 = base.NpcData.AICards.Count() - 1; num4 >= 0; num4--)
		{
			if (base.NpcData.AICards[num4].AddCardRound <= MatchManager.Instance.GetCurrentRound() && !MatchManager.Instance.GetNPCDiscardDeck(base.NPCIndex).Contains(base.NpcData.AICards[num4].Card.Id))
			{
				list3.Add(base.NpcData.AICards[num4]);
			}
		}
		if (list3.Count > 0)
		{
			list3 = (from card in list3
				orderby card.AddCardRound == MatchManager.Instance.GetCurrentRound() descending, card.AddCardRound, card.Priority
				select card).ToList();
		}
		SortedList<double, CardItem> sortedList = new SortedList<double, CardItem>();
		for (int num5 = 0; num5 < base.NPCItem.cardsCI.Length; num5++)
		{
			if (base.NPCItem.cardsCI[num5] == null || base.NPCItem.cardsCI[num5].CardData == null)
			{
				continue;
			}
			string text = base.NPCItem.cardsCI[num5].CardData.Id.Split("_")[0];
			for (int num6 = 0; num6 < list3.Count; num6++)
			{
				if (!sortedList.ContainsKey(num6 + num2) && text == list3[num6].Card.Id)
				{
					sortedList.Add(num6 + num2, base.NPCItem.cardsCI[num5]);
					break;
				}
			}
		}
		if (num2 > 0)
		{
			int num7 = 0;
			int j2;
			for (j2 = 0; j2 < base.NPCItem.cardsCI.Length; j2++)
			{
				if (!sortedList.Any((KeyValuePair<double, CardItem> p) => p.Value.CardData.Id == base.NPCItem.cardsCI[j2].CardData.Id))
				{
					sortedList.Add(num7, base.NPCItem.cardsCI[j2]);
					num7++;
				}
			}
		}
		int num8 = 0;
		foreach (KeyValuePair<double, CardItem> item in sortedList)
		{
			CardItem value = item.Value;
			value.PositionCardInNPC(sortedList.Count - 1 - num8, sortedList.Count);
			value.DefaultElementsLayeringOrder(100 * num8);
			value.CreateColliderAdjusted();
			value.ShowBackImage(state: true);
			value.ShowCardNPC(num8);
			if (value.CardData.Corrupted)
			{
				value.RevealCard();
			}
			num8++;
		}
		MatchManager.Instance.RemoveCardsFromNPCHand(base.NPCIndex, num8);
		CheckRevealCardsCurse();
	}

	public void UpdateOverDeck()
	{
		if (base.NPCItem?.cardsCI != null)
		{
			CardItem[] cardsCI = base.NPCItem.cardsCI;
			for (int i = 0; i < cardsCI.Length; i++)
			{
				cardsCI[i]?.IsDivineExecutionPlayable();
			}
		}
	}

	public void CastCardNPC(int theCard, Transform targetT)
	{
		if (base.NPCItem.cardsCI[theCard] != null)
		{
			currentCardItem = base.NPCItem.cardsCI[theCard];
			currentCardItem.CastCardNPC(targetT);
			currentCardItem.RemoveAmplifyNewCard();
			base.NPCItem.cardsCI[theCard] = null;
		}
	}

	public void CastCardNPCEnd()
	{
		if (currentCardItem != null)
		{
			currentCardItem.DiscardCardNPC(0);
			currentCardItem = null;
		}
	}

	public void DiscardHand()
	{
		if (!(base.NPCItem != null))
		{
			return;
		}
		for (int i = 0; i < base.NPCItem.cardsCI.Length; i++)
		{
			if (base.NPCItem.cardsCI[i] != null)
			{
				MatchManager.Instance.NPCDiscard(base.NPCIndex, i, casted: false);
				base.NPCItem.cardsCI[i].DiscardCardNPC(base.NPCItem.cardsCI.Length - 1 - i);
			}
		}
		MatchManager.Instance.ResetDeckHandNPC(base.NPCIndex);
	}

	public bool CheckPlayableIfSpecialCard(string cardId)
	{
		if (!MatchManager.Instance.IsPhantomArmorSpecialCard(cardId))
		{
			return true;
		}
		if (IsParalyzed() || IsSleep())
		{
			return false;
		}
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		foreach (NPC nPC in teamNPC)
		{
			if (nPC == null || (nPC != null && !nPC.Alive) || nPC.IsParalyzed())
			{
				return false;
			}
		}
		return true;
	}
}
