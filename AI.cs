using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AI
{
	public static bool DoAI(NPC _npc, Hero[] _teamHero, NPC[] _teamNPC, ref float castDelay)
	{
		if (_npc.NPCItem == null || _npc.HpCurrent <= 0 || MatchManager.Instance.CheckMatchIsOver())
		{
			return false;
		}
		List<AICards> list = new List<AICards>();
		AICards aICards = null;
		int num = _npc.NpcData.AICards.Length;
		for (int i = 0; i < num; i++)
		{
			AICards aICards2 = null;
			int num2 = 1000;
			for (int j = 0; j < num; j++)
			{
				if (_npc.NpcData.AICards[j].Priority < num2 && !list.Contains(_npc.NpcData.AICards[j]))
				{
					aICards2 = _npc.NpcData.AICards[j];
					num2 = aICards2.Priority;
				}
			}
			list.Add(aICards2);
		}
		List<string> list2 = new List<string>();
		for (int k = 0; k < MatchManager.Instance.CountNPCHand(_npc.NPCIndex); k++)
		{
			string text = MatchManager.Instance.CardFromNPCHand(_npc.NPCIndex, k);
			if (text != "")
			{
				string[] array = text.Split('_');
				if (array != null && array[0] != null)
				{
					list2.Add(array[0]);
				}
			}
		}
		bool flag = false;
		for (int l = 0; l < list2.Count; l++)
		{
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m] != null && list[m].Card != null && list[m].Card.Id == list2[l])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				AICards aICards3 = new AICards();
				aICards3.Card = Globals.Instance.GetCardData(list2[l]);
				list.Add(aICards3);
			}
		}
		int energy = _npc.GetEnergy();
		num = list.Count;
		for (int num3 = num - 1; num3 >= 0; num3--)
		{
			if (list[num3].Card == null || !_npc.CanPlayCard(list[num3].Card) || !_npc.CanPlayCardSummon(list[num3].Card) || _npc.GetCardFinalCost(list[num3].Card) > energy || !list2.Contains(list[num3].Card.Id) || !_npc.CheckPlayableIfSpecialCard(list[num3].Card.Id))
			{
				list.RemoveAt(num3);
			}
		}
		num = list.Count;
		Dictionary<string, List<Hero>> dictionary = new Dictionary<string, List<Hero>>();
		Dictionary<string, List<NPC>> dictionary2 = new Dictionary<string, List<NPC>>();
		bool flag2 = false;
		for (int num4 = num - 1; num4 >= 0; num4--)
		{
			List<Hero> list3 = new List<Hero>();
			List<NPC> list4 = new List<NPC>();
			AICards aICards4 = list[num4];
			if (aICards4.Card.TargetType == Enums.CardTargetType.Global)
			{
				if (aICards4.Card.TargetSide == Enums.CardTargetSide.Enemy || aICards4.Card.TargetSide == Enums.CardTargetSide.Anyone)
				{
					foreach (Hero hero in _teamHero)
					{
						if (hero != null && hero.Alive)
						{
							list3.Add(hero);
						}
					}
				}
				if (aICards4.Card.TargetSide == Enums.CardTargetSide.Friend || aICards4.Card.TargetSide == Enums.CardTargetSide.Anyone)
				{
					foreach (NPC nPC in _teamNPC)
					{
						if (nPC != null && nPC.Alive)
						{
							list4.Add(_npc);
						}
					}
				}
			}
			else if (aICards4.Card.TargetSide == Enums.CardTargetSide.Enemy || aICards4.Card.TargetSide == Enums.CardTargetSide.Anyone)
			{
				foreach (Hero hero2 in _teamHero)
				{
					if (hero2 != null && hero2.Alive && hero2.HasEffect("taunt") && !hero2.HasEffect("stealth"))
					{
						list3.Add(hero2);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					foreach (Hero hero3 in _teamHero)
					{
						if (hero3 == null || !hero3.Alive || hero3.HasEffect("stealth") || (aICards4.Card.TargetPosition == Enums.CardTargetPosition.Front && !MatchManager.Instance.PositionIsFront(isHero: true, hero3.Position)) || (aICards4.Card.TargetPosition == Enums.CardTargetPosition.Back && !MatchManager.Instance.PositionIsBack(hero3)) || (aICards4.Card.TargetPosition == Enums.CardTargetPosition.Middle && !MatchManager.Instance.PositionIsMiddle(hero3)))
						{
							continue;
						}
						bool flag3 = false;
						if (aICards4.OnlyCastIf == Enums.OnlyCastIf.Always)
						{
							flag3 = true;
						}
						else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TeamNpcAllAlive)
						{
							if (MatchManager.Instance.NumNPCsAlive() == 4)
							{
								flag3 = true;
							}
						}
						else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetLifeLessThanPercent)
						{
							float valueCastIf = aICards4.ValueCastIf;
							if (hero3.GetHpPercent() <= valueCastIf)
							{
								flag3 = true;
							}
						}
						else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasNotAuraCurse)
						{
							string id = aICards4.AuracurseCastIf.Id;
							if (!hero3.HasEffect(id))
							{
								flag3 = true;
							}
						}
						else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetLifeHigherThanPercent)
						{
							float valueCastIf2 = aICards4.ValueCastIf;
							if (hero3.GetHpPercent() >= valueCastIf2)
							{
								flag3 = true;
							}
						}
						else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasAuraCurse)
						{
							string id2 = aICards4.AuracurseCastIf.Id;
							if (hero3.HasEffect(id2))
							{
								flag3 = true;
							}
						}
						else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasAnyAura)
						{
							if (hero3.HasAnyAura())
							{
								flag3 = true;
							}
						}
						else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasAnyCurse && hero3.HasAnyCurse())
						{
							flag3 = true;
						}
						if (flag3)
						{
							list3.Add(hero3);
						}
					}
				}
			}
			else
			{
				foreach (NPC nPC2 in _teamNPC)
				{
					if (nPC2 == null || !nPC2.Alive)
					{
						continue;
					}
					bool flag4 = true;
					if (aICards4.Card.TargetSide == Enums.CardTargetSide.Self && _npc.Id != nPC2.Id)
					{
						flag4 = false;
					}
					else if (aICards4.Card.TargetSide == Enums.CardTargetSide.FriendNotSelf && _npc.Id == nPC2.Id)
					{
						flag4 = false;
					}
					if (!flag4 || !nPC2.Alive || (nPC2.Position > 0 && aICards4.Card.TargetPosition == Enums.CardTargetPosition.Front))
					{
						continue;
					}
					bool flag5 = false;
					if (aICards4.OnlyCastIf == Enums.OnlyCastIf.Always)
					{
						flag5 = true;
					}
					else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TeamNpcAllAlive)
					{
						if (MatchManager.Instance.NumNPCsAlive() == 4)
						{
							flag5 = true;
						}
					}
					else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetLifeLessThanPercent)
					{
						float valueCastIf3 = aICards4.ValueCastIf;
						if (nPC2.GetHpPercent() <= valueCastIf3)
						{
							flag5 = true;
						}
					}
					else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasNotAuraCurse)
					{
						string id3 = aICards4.AuracurseCastIf.Id;
						if (!nPC2.HasEffect(id3))
						{
							flag5 = true;
						}
					}
					else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetLifeHigherThanPercent)
					{
						float valueCastIf4 = aICards4.ValueCastIf;
						if (nPC2.GetHpPercent() >= valueCastIf4)
						{
							flag5 = true;
						}
					}
					else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasAuraCurse)
					{
						string id4 = aICards4.AuracurseCastIf.Id;
						if (nPC2.HasEffect(id4))
						{
							flag5 = true;
						}
					}
					else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasAnyAura)
					{
						if (nPC2.HasAnyAura())
						{
							flag5 = true;
						}
					}
					else if (aICards4.OnlyCastIf == Enums.OnlyCastIf.TargetHasAnyCurse && nPC2.HasAnyCurse())
					{
						flag5 = true;
					}
					if (flag5)
					{
						list4.Add(nPC2);
					}
				}
			}
			if (!flag2 && list3.Count > 1)
			{
				int num9 = -1;
				float num10 = 1000f;
				float num11 = 10000f;
				float num12 = 0f;
				float num13 = 0f;
				int num14 = 0;
				int num15 = 10;
				int num16 = 100;
				int num17 = -1;
				float num18 = 0f;
				float num19 = 10000f;
				num14 = list3.Count;
				for (int num20 = 0; num20 < list3.Count; num20++)
				{
					if (list3[num20].Position > num9)
					{
						num9 = list3[num20].Position;
					}
					if (list3[num20].Position < num15)
					{
						num15 = list3[num20].Position;
					}
					int num21 = list3[num20].GetSpeed()[0];
					if (num21 < num16)
					{
						num16 = num21;
					}
					if (num21 > num17)
					{
						num17 = num21;
					}
					float num22 = list3[num20].GetHp();
					if (num22 < num11)
					{
						num11 = num22;
					}
					if (num22 > num13)
					{
						num13 = num22;
					}
					float hpPercent = list3[num20].GetHpPercent();
					if (hpPercent < num10)
					{
						num10 = hpPercent;
					}
					if (hpPercent > num12)
					{
						num12 = hpPercent;
					}
					float num23 = num22 + (float)list3[num20].GetBlock();
					if (num23 < num19)
					{
						num19 = num23;
					}
					if (num23 > num18)
					{
						num18 = num23;
					}
				}
				for (int num24 = list3.Count - 1; num24 >= 0; num24--)
				{
					Hero hero4 = list3[num24];
					if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.Slowest)
					{
						if (hero4.GetSpeed()[0] > num16)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.Fastest)
					{
						if (hero4.GetSpeed()[0] < num17)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.LeastHP)
					{
						if ((float)hero4.GetHp() > num11)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.MostHP)
					{
						if ((float)hero4.GetHp() < num13)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.Front)
					{
						if (hero4.Position > num15)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.Middle)
					{
						if (num14 > 2 && (hero4.Position == num15 || hero4.Position == num9))
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.Back)
					{
						if (hero4.Position < num9)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.AnyButFront)
					{
						if (hero4.Position == num15)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.AnyButBack)
					{
						if (hero4.Position == num9)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessHealthPercent)
					{
						if (hero4.GetHpPercent() > num10)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreHealthPercent)
					{
						if (hero4.GetHpPercent() < num12)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessHealthFlat)
					{
						if ((float)hero4.GetHp() > num11)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreHealthFlat)
					{
						if ((float)hero4.GetHp() < num13)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessHealthAbsolute)
					{
						if ((float)(hero4.GetHp() + hero4.GetBlock()) > num19)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreHealthAbsolute)
					{
						if ((float)(hero4.GetHp() + hero4.GetBlock()) < num18)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessInitiative)
					{
						if (hero4.GetSpeed()[0] > num16)
						{
							list3.RemoveAt(num24);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreInitiative && hero4.GetSpeed()[0] < num17)
					{
						list3.RemoveAt(num24);
					}
				}
			}
			if (list4.Count > 1)
			{
				int num9 = -1;
				float num10 = 1000f;
				float num11 = 10000f;
				float num12 = 0f;
				float num13 = 0f;
				int num14 = 0;
				int num15 = 10;
				int num16 = 100;
				int num17 = -1;
				float num18 = 0f;
				float num19 = 10000f;
				num14 = list4.Count;
				for (int num25 = 0; num25 < list4.Count; num25++)
				{
					if (list4[num25].Position > num9)
					{
						num9 = list4[num25].Position;
					}
					if (list4[num25].Position < num15)
					{
						num15 = list4[num25].Position;
					}
					int num26 = list4[num25].GetSpeed()[0];
					if (num26 < num16)
					{
						num16 = num26;
					}
					if (num26 > num17)
					{
						num17 = num26;
					}
					float num27 = list4[num25].GetHp();
					if (num27 < num11)
					{
						num11 = num27;
					}
					if (num27 > num13)
					{
						num13 = num27;
					}
					float hpPercent2 = list4[num25].GetHpPercent();
					if (hpPercent2 < num10)
					{
						num10 = hpPercent2;
					}
					if (hpPercent2 > num12)
					{
						num12 = hpPercent2;
					}
					float num28 = num27 + (float)list4[num25].GetBlock();
					if (num28 < num19)
					{
						num19 = num28;
					}
					if (num28 > num18)
					{
						num18 = num28;
					}
				}
				for (int num29 = list4.Count - 1; num29 >= 0; num29--)
				{
					NPC nPC3 = list4[num29];
					if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.Slowest)
					{
						if (nPC3.GetSpeed()[0] > num16)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.Fastest)
					{
						if (nPC3.GetSpeed()[0] < num17)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.LeastHP)
					{
						if ((float)nPC3.GetHp() > num11)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.Card.TargetPosition == Enums.CardTargetPosition.MostHP)
					{
						if ((float)nPC3.GetHp() < num13)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.Front)
					{
						if (nPC3.Position > num15)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.Middle)
					{
						if (num14 > 2 && (nPC3.Position == num15 || nPC3.Position == num9))
						{
							list4.RemoveAt(num29);
						}
						else if (num14 == 2 && nPC3.Position == num15)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.Back)
					{
						if (nPC3.Position < num9)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.AnyButFront)
					{
						if (nPC3.Position == num15)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.AnyButBack)
					{
						if (nPC3.Position == num9)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessHealthPercent)
					{
						if (nPC3.GetHpPercent() > num10)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreHealthPercent)
					{
						if (nPC3.GetHpPercent() < num12)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessHealthFlat)
					{
						if ((float)nPC3.GetHp() > num11)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreHealthFlat)
					{
						if ((float)nPC3.GetHp() < num13)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessHealthAbsolute)
					{
						if ((float)(nPC3.GetHp() + nPC3.GetBlock()) > num19)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreHealthAbsolute)
					{
						if ((float)(nPC3.GetHp() + nPC3.GetBlock()) < num18)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.LessInitiative)
					{
						if (nPC3.GetSpeed()[0] > num16)
						{
							list4.RemoveAt(num29);
						}
					}
					else if (aICards4.TargetCast == Enums.TargetCast.MoreInitiative && nPC3.GetSpeed()[0] < num17)
					{
						list4.RemoveAt(num29);
					}
				}
			}
			if (list3.Count == 0 && list4.Count == 0)
			{
				list.RemoveAt(num4);
			}
			else
			{
				if (!dictionary.ContainsKey(aICards4.Card.Id))
				{
					dictionary.Add(aICards4.Card.Id, list3);
				}
				if (!dictionary2.ContainsKey(aICards4.Card.Id))
				{
					dictionary2.Add(aICards4.Card.Id, list4);
				}
			}
		}
		string text2 = "";
		int num30 = -1;
		if (list.Count > 0)
		{
			int num31 = 1;
			int priority = list[0].Priority;
			for (int num32 = 1; num32 < list.Count && list[num32].Priority == priority; num32++)
			{
				num31++;
			}
			if (num31 > 1)
			{
				float[] array2 = new float[num31];
				float num33 = 0f;
				for (int num34 = 0; num34 < num31; num34++)
				{
					num33 += list[num34].PercentToCast;
				}
				for (int num35 = 0; num35 < num31; num35++)
				{
					array2[num35] = list[num35].PercentToCast * 100f / num33;
				}
				float num36 = 0f;
				aICards = null;
				int randomIntRange = MatchManager.Instance.GetRandomIntRange(1, 101);
				for (int num37 = 0; num37 < num31; num37++)
				{
					num36 += array2[num37];
					if ((float)randomIntRange <= num36)
					{
						aICards = list[num37];
						break;
					}
				}
			}
			List<AICards> list5 = new List<AICards>();
			if (aICards == null)
			{
				for (int num38 = list.Count - 1; num38 >= 0; num38--)
				{
					if (list[num38].AddCardRound <= MatchManager.Instance.GetCurrentRound() && !MatchManager.Instance.GetNPCDiscardDeck(_npc.NPCIndex).Contains(list[num38].Card.Id))
					{
						list5.Add(list[num38]);
					}
				}
			}
			if (list5.Count > 0)
			{
				aICards = (from card in list5
					orderby card.AddCardRound == MatchManager.Instance.GetCurrentRound() descending, card.AddCardRound, card.Priority
					select card).ToList()[0];
			}
			if (aICards == null)
			{
				aICards = list[0];
			}
			text2 = aICards.Card.Id;
			if (_npc.NPCIsBoss() && MatchManager.Instance.IsPhantomArmorSpecialCard(text2) && castDelay <= 0f)
			{
				castDelay = 2.1f;
				return false;
			}
			for (int num39 = 0; num39 < MatchManager.Instance.CountNPCHand(_npc.NPCIndex); num39++)
			{
				if (MatchManager.Instance.CardFromNPCHand(_npc.NPCIndex, num39) != null && MatchManager.Instance.CardFromNPCHand(_npc.NPCIndex, num39).Split(char.Parse("_"))[0] == text2)
				{
					num30 = num39;
					break;
				}
			}
		}
		if (num30 > -1)
		{
			for (int num40 = _npc.NPCItem.cardsCI.Length - 1; num40 >= 0; num40--)
			{
				if (_npc.NPCItem.cardsCI[num40] != null && _npc.NPCItem.cardsCI[num40].CardData != null && _npc.NPCItem.cardsCI[num40].CardData.Id == text2 && _npc.NPCItem.cardsCI[num40] != null && _npc.NPCItem.cardsCI[num40].IsRevealed())
				{
					num30 = num40;
					break;
				}
			}
			int num41 = -1;
			Hero hero5 = null;
			Transform transform = _npc.NPCItem.transform;
			if (_npc.NPCItem == null || _npc.NPCItem.cardsCI == null || num30 >= _npc.NPCItem.cardsCI.Length || _npc.NPCItem.cardsCI[num30] == null)
			{
				return false;
			}
			CardData cardData = _npc.NPCItem.cardsCI[num30].CardData;
			Transform transform2;
			if (dictionary[text2].Count == 0)
			{
				num41 = MatchManager.Instance.GetRandomIntRange(0, dictionary2[text2].Count);
				transform2 = dictionary2[text2][num41].NPCItem.transform;
			}
			else
			{
				num41 = MatchManager.Instance.GetRandomIntRange(0, dictionary[text2].Count);
				hero5 = dictionary[text2][num41];
				if (hero5 == null || !(hero5.HeroItem != null))
				{
					return false;
				}
				transform2 = hero5.HeroItem.transform;
			}
			if (_npc.NPCItem.cardsCI[num30] != null && _npc.NPCItem.cardsCI[num30].gameObject.activeSelf)
			{
				_npc.CastCardNPC(num30, transform2);
				MatchManager.Instance.CastAutomatic(cardData, transform, transform2);
				MatchManager.Instance.NPCDiscard(_npc.NPCIndex, num30, casted: true);
				return true;
			}
			return false;
		}
		return false;
	}
}
