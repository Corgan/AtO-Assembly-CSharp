using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UnlockedBar : MonoBehaviour
{
	public string type;

	public Transform maskTransform;

	public SpriteRenderer barSprite;

	public TMP_Text titleText;

	public TMP_Text cardsText;

	public SpriteRenderer sigil0;

	public SpriteRenderer sigil1;

	public SpriteRenderer sigil2;

	public SpriteRenderer sigil3;

	public SpriteRenderer sigil4;

	private float scale100 = 3.38f;

	private int cardsTotal = -1;

	private int cardsUnlocked;

	public void InitBar()
	{
		cardsTotal = -1;
		cardsUnlocked = 0;
		SetBasics();
		CalculateUnlock();
	}

	private void SetBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+.3>");
		if (type == "warriorCards")
		{
			TMP_Text tMP_Text = titleText;
			TMP_Text tMP_Text2 = cardsText;
			Color color = (barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]));
			Color color3 = (tMP_Text2.color = color);
			tMP_Text.color = color3;
			stringBuilder.Append("<sprite name=slashing>");
		}
		else if (type == "scoutCards")
		{
			TMP_Text tMP_Text3 = titleText;
			TMP_Text tMP_Text4 = cardsText;
			Color color = (barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]));
			Color color3 = (tMP_Text4.color = color);
			tMP_Text3.color = color3;
			stringBuilder.Append("<sprite name=piercing>");
		}
		else if (type == "mageCards")
		{
			TMP_Text tMP_Text5 = titleText;
			TMP_Text tMP_Text6 = cardsText;
			Color color = (barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["mage"]));
			Color color3 = (tMP_Text6.color = color);
			tMP_Text5.color = color3;
			stringBuilder.Append("<sprite name=fire>");
		}
		else if (type == "healerCards")
		{
			TMP_Text tMP_Text7 = titleText;
			TMP_Text tMP_Text8 = cardsText;
			Color color = (barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["healer"]));
			Color color3 = (tMP_Text8.color = color);
			tMP_Text7.color = color3;
			stringBuilder.Append("<sprite name=heal>");
		}
		else if (type == "equipment")
		{
			TMP_Text tMP_Text9 = titleText;
			TMP_Text tMP_Text10 = cardsText;
			Color color = (barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["item"]));
			Color color3 = (tMP_Text10.color = color);
			tMP_Text9.color = color3;
			stringBuilder.Append("<sprite name=jewelry>");
		}
		else if (type == "mapNodes")
		{
			TMP_Text tMP_Text11 = titleText;
			TMP_Text tMP_Text12 = cardsText;
			Color color = (barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["boon"]));
			Color color3 = (tMP_Text12.color = color);
			tMP_Text11.color = color3;
			stringBuilder.Append("<sprite name=node>");
		}
		else if (type == "uniqueBosses")
		{
			TMP_Text tMP_Text13 = titleText;
			TMP_Text tMP_Text14 = cardsText;
			Color color = (barSprite.color = Functions.HexToColor("#784FD4"));
			Color color3 = (tMP_Text14.color = color);
			tMP_Text13.color = color3;
			stringBuilder.Append("<sprite name=bossIcon>");
		}
		stringBuilder.Append("</size> ");
		stringBuilder.Append(Texts.Instance.GetText(type));
		titleText.text = stringBuilder.ToString();
	}

	private void CalculateUnlock()
	{
		if (cardsTotal == -1)
		{
			Enums.CardClass key = Enums.CardClass.Warrior;
			bool flag = false;
			bool flag2 = false;
			if (type == "warriorCards")
			{
				key = Enums.CardClass.Warrior;
			}
			else if (type == "scoutCards")
			{
				key = Enums.CardClass.Scout;
			}
			else if (type == "mageCards")
			{
				key = Enums.CardClass.Mage;
			}
			else if (type == "healerCards")
			{
				key = Enums.CardClass.Healer;
			}
			else if (type == "equipment")
			{
				key = Enums.CardClass.Item;
			}
			else if (type == "mapNodes")
			{
				flag = true;
			}
			else if (type == "uniqueBosses")
			{
				flag2 = true;
			}
			if (flag)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				cardsUnlocked = 0;
				foreach (KeyValuePair<string, NodeData> item2 in Globals.Instance.NodeDataSource)
				{
					try
					{
						if (!(item2.Key != "tutorial_0") || !(item2.Key != "tutorial_1") || !(item2.Key != "tutorial_2"))
						{
							continue;
						}
						switch (item2.Value.NodeZone.ZoneId)
						{
						case "Aquarfall":
						case "Sectarium":
						case "Senenthia":
						case "Spiderlair":
						case "Velkarath":
						case "Voidhigh":
						case "Voidlow":
						case "Faeborg":
						case "Frozensewers":
						case "Blackforge":
						case "Ulminin":
						case "Pyramid":
							if (!item2.Value.GoToTown && !item2.Value.TravelDestination)
							{
								list.Add(item2.Key);
							}
							break;
						}
					}
					catch (Exception)
					{
						Debug.Log(item2);
						throw;
					}
				}
				if (PlayerManager.Instance.UnlockedNodes != null)
				{
					for (int i = 0; i < PlayerManager.Instance.UnlockedNodes.Count; i++)
					{
						if (Globals.Instance.NodeCombatEventRelation.ContainsKey(PlayerManager.Instance.UnlockedNodes[i]))
						{
							string item = Globals.Instance.NodeCombatEventRelation[PlayerManager.Instance.UnlockedNodes[i]];
							if (list.Contains(item))
							{
								list2.Add(item);
							}
						}
					}
				}
				cardsTotal = list.Count;
				cardsUnlocked = list2.Count;
			}
			else if (flag2)
			{
				if (PlayerManager.Instance.BossesKilledName != null)
				{
					cardsUnlocked = PlayerManager.Instance.BossesKilledName.Count;
				}
				else
				{
					cardsUnlocked = 0;
				}
				cardsTotal = 0;
				List<string> list3 = new List<string>();
				foreach (KeyValuePair<string, NPCData> nPC in Globals.Instance.NPCs)
				{
					if (nPC.Value.IsBoss && !list3.Contains(nPC.Value.NPCName))
					{
						list3.Add(nPC.Value.NPCName);
						cardsTotal++;
					}
				}
			}
			else
			{
				List<string> list4 = Globals.Instance.CardListNotUpgradedByClass[key];
				cardsTotal = list4.Count;
				for (int j = 0; j < cardsTotal; j++)
				{
					if (PlayerManager.Instance.IsCardUnlocked(list4[j]))
					{
						cardsUnlocked++;
					}
				}
			}
		}
		if (cardsUnlocked > cardsTotal)
		{
			cardsUnlocked = cardsTotal;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+1.4>");
		stringBuilder.Append(cardsUnlocked);
		stringBuilder.Append("</size> <voffset=.4><color=#333>/");
		stringBuilder.Append(cardsTotal);
		stringBuilder.Append("</color>");
		cardsText.text = stringBuilder.ToString();
		float num = (float)cardsUnlocked / (float)cardsTotal;
		maskTransform.localScale = new Vector3(num * scale100, 2.03f, maskTransform.localScale.z);
	}
}
