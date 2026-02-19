using System.Collections.Generic;
using UnityEngine;

public static class Loot
{
	public static List<string> GetLootItems(string _itemListId, string _idAux = "")
	{
		if (AtOManager.Instance.GetGameId() == "")
		{
			return null;
		}
		if (GameManager.Instance.GetDeveloperMode() && _itemListId == "")
		{
			return Globals.Instance.CardListByClass[Enums.CardClass.Item];
		}
		LootData lootData = Globals.Instance.GetLootData(_itemListId);
		if (lootData == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		for (int i = 0; i < Globals.Instance.CardListByClass[Enums.CardClass.Item].Count; i++)
		{
			list2.Add(Globals.Instance.CardListByClass[Enums.CardClass.Item][i]);
		}
		int deterministicHashCode = (AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId() + _idAux).GetDeterministicHashCode();
		Random.InitState(deterministicHashCode);
		list2.Shuffle(deterministicHashCode);
		int num = 0;
		CardData cardData = null;
		int num2 = 0;
		num2 = ((!GameManager.Instance.IsObeliskChallenge()) ? lootData.NumItems : lootData.LootItemTable.Length);
		for (int j = 0; j < num2; j++)
		{
			if (list.Count >= lootData.NumItems)
			{
				break;
			}
			if (j >= lootData.LootItemTable.Length)
			{
				continue;
			}
			LootItem lootItem = lootData.LootItemTable[j];
			if (!((float)Random.Range(0, 100) < lootItem.LootPercent))
			{
				continue;
			}
			if (lootItem.LootMisc != "")
			{
				string text = StablishLootMisc(lootItem);
				if (text != "")
				{
					list.Add(text);
				}
				else if (lootItem.LootCard != null)
				{
					list.Add(lootItem.LootCard.Id);
				}
				continue;
			}
			if (lootItem.LootCard != null)
			{
				list.Add(lootItem.LootCard.Id);
				continue;
			}
			bool flag = false;
			int num3 = 0;
			cardData = null;
			while (!flag && num3 < 10000)
			{
				if (num >= list2.Count)
				{
					num = 0;
				}
				string text2 = list2[num];
				if (!list.Contains(text2) && ((!AtOManager.Instance.ItemBoughtOnThisShop(_itemListId, text2) && AtOManager.Instance.HowManyTownRerolls() > 0) || AtOManager.Instance.HowManyTownRerolls() == 0))
				{
					cardData = Globals.Instance.GetCardData(text2, instantiate: false);
					if (cardData.Item != null && ((!cardData.Item.DropOnly && !lootData.AllowDropOnlyItems) || lootData.AllowDropOnlyItems))
					{
						if (lootItem.LootType == Enums.CardType.Petrare)
						{
							if (cardData.CardType == Enums.CardType.Pet && cardData.CardUpgraded != Enums.CardUpgraded.No)
							{
								flag = true;
							}
						}
						else if (cardData.CardUpgraded == Enums.CardUpgraded.Rare)
						{
							flag = false;
						}
						else if (cardData.Item.PercentRetentionEndGame > 0 && (AtOManager.Instance.GetNgPlus() > 2 || GameManager.Instance.IsObeliskChallenge() || GameManager.Instance.IsSingularity()))
						{
							flag = false;
						}
						else if (cardData.Item.PercentDiscountShop > 0 && (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty")))
						{
							flag = false;
						}
						else if (lootItem.LootType == Enums.CardType.None && cardData.CardRarity == lootItem.LootRarity)
						{
							flag = true;
						}
						else if (cardData.CardType == lootItem.LootType && cardData.CardRarity == lootItem.LootRarity)
						{
							flag = true;
						}
					}
				}
				num++;
				num3++;
			}
			if (flag && cardData != null)
			{
				list.Add(cardData.Id);
			}
		}
		for (int k = list.Count; k < lootData.NumItems; k++)
		{
			bool flag2 = false;
			int num4 = 0;
			cardData = null;
			int num5 = Random.Range(0, 100);
			while (!flag2 && num4 < 10000)
			{
				if (num >= list2.Count)
				{
					num = 0;
				}
				string text3 = list2[num];
				if (!list.Contains(text3) && ((!AtOManager.Instance.ItemBoughtOnThisShop(_itemListId, text3) && AtOManager.Instance.HowManyTownRerolls() > 0) || AtOManager.Instance.HowManyTownRerolls() == 0))
				{
					cardData = Globals.Instance.GetCardData(text3, instantiate: false);
					if (cardData.Item != null && !cardData.Item.DropOnly)
					{
						if (cardData.CardUpgraded == Enums.CardUpgraded.Rare)
						{
							flag2 = false;
						}
						else if (cardData.Item.PercentRetentionEndGame > 0 && (AtOManager.Instance.GetNgPlus() > 2 || GameManager.Instance.IsObeliskChallenge()))
						{
							flag2 = false;
						}
						else if (cardData.Item.PercentDiscountShop > 0 && (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty")))
						{
							flag2 = false;
						}
						else if ((float)num5 < lootData.DefaultPercentMythic)
						{
							if (cardData.CardRarity == Enums.CardRarity.Mythic)
							{
								flag2 = true;
							}
						}
						else if ((float)num5 < lootData.DefaultPercentEpic + lootData.DefaultPercentMythic)
						{
							if (cardData.CardRarity == Enums.CardRarity.Epic)
							{
								flag2 = true;
							}
						}
						else if ((float)num5 < lootData.DefaultPercentRare + lootData.DefaultPercentEpic + lootData.DefaultPercentMythic)
						{
							if (cardData.CardRarity == Enums.CardRarity.Rare)
							{
								flag2 = true;
							}
						}
						else if ((float)num5 < lootData.DefaultPercentUncommon + lootData.DefaultPercentRare + lootData.DefaultPercentEpic + lootData.DefaultPercentMythic)
						{
							if (cardData.CardRarity == Enums.CardRarity.Uncommon)
							{
								flag2 = true;
							}
						}
						else if (cardData.CardRarity == Enums.CardRarity.Common)
						{
							flag2 = true;
						}
					}
				}
				num++;
				num4++;
				if (!flag2 && num4 % 100 == 0)
				{
					num5 += 10;
				}
			}
			if (!flag2 || !(cardData != null))
			{
				break;
			}
			list.Add(cardData.Id);
		}
		list.Shuffle(deterministicHashCode);
		if (!AtOManager.Instance.CharInTown() && ((!GameManager.Instance.IsObeliskChallenge() && AtOManager.Instance.GetMadnessDifficulty() > 0) || GameManager.Instance.IsObeliskChallenge()))
		{
			int num6 = 0;
			if (AtOManager.Instance.corruptionId == "exoticshop")
			{
				num6 += 8;
			}
			else if (AtOManager.Instance.corruptionId == "rareshop")
			{
				num6 += 4;
			}
			if (GameManager.Instance.IsObeliskChallenge())
			{
				if (AtOManager.Instance.GetObeliskMadness() > 8)
				{
					num6 += 4;
				}
				else if (AtOManager.Instance.GetObeliskMadness() > 4)
				{
					num6 += 2;
				}
			}
			else
			{
				num6 += Functions.FuncRoundToInt(0.2f * (float)AtOManager.Instance.GetMadnessDifficulty());
			}
			for (int l = 0; l < list.Count; l++)
			{
				int num7 = Random.Range(0, 100);
				CardData cardData2 = Globals.Instance.GetCardData(list[l], instantiate: false);
				if (!(cardData2 == null))
				{
					bool flag3 = false;
					if ((cardData2.CardRarity == Enums.CardRarity.Mythic || cardData2.CardRarity == Enums.CardRarity.Epic) && num7 < 3 + num6)
					{
						flag3 = true;
					}
					else if (cardData2.CardRarity == Enums.CardRarity.Rare && num7 < 5 + num6)
					{
						flag3 = true;
					}
					else if (cardData2.CardRarity == Enums.CardRarity.Uncommon && num7 < 7 + num6)
					{
						flag3 = true;
					}
					else if (cardData2.CardRarity == Enums.CardRarity.Common && num7 < 9 + num6)
					{
						flag3 = true;
					}
					if (flag3 && cardData2.UpgradesToRare != null)
					{
						list[l] = cardData2.UpgradesToRare.Id;
					}
				}
			}
		}
		if (GameManager.Instance.IsTutorialGame() && list != null && list.Count > 0)
		{
			bool flag4 = true;
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m] == "spyglass")
				{
					flag4 = false;
					break;
				}
			}
			if (flag4)
			{
				list[list.Count - 1] = "spyglass";
			}
		}
		return list;
	}

	private static string StablishLootMisc(LootItem lootItem)
	{
		string text = lootItem.LootMisc.ToLower();
		string result = "";
		if (text == "scrappyrobot")
		{
			result = ScrappyRobotResolver.GetScrappyRobot(AtOManager.Instance.GetPlayerRequeriments());
		}
		return result;
	}
}
