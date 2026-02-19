using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DeckEnergy : MonoBehaviour
{
	public TMP_Text cardChallengeEnergy;

	public TMP_Text[] cardChallengeBarSup;

	public Transform[] cardChallengeBar;

	private Dictionary<int, int> dictEnergyCost = new Dictionary<int, int>();

	public void WriteEnergy(int _heroIndex, int _type)
	{
		Hero hero = AtOManager.Instance.GetHero(_heroIndex);
		if (hero == null)
		{
			return;
		}
		List<string> list = new List<string>();
		dictEnergyCost.Clear();
		switch (_type)
		{
		case 0:
			list = hero.Cards;
			break;
		case 1:
			list = MatchManager.Instance.GetHeroDeck(_heroIndex);
			break;
		case 2:
			list = MatchManager.Instance.GetHeroDiscard(_heroIndex);
			break;
		case 3:
			list = MatchManager.Instance.GetHeroVanish(_heroIndex);
			break;
		}
		float num = 0f;
		CardData cardData = null;
		int num2 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != "")
			{
				int num3 = 0;
				if ((bool)MatchManager.Instance)
				{
					cardData = MatchManager.Instance.GetCardData(list[i], createInDict: false);
					num3 = hero.GetCardFinalCost(cardData);
				}
				else
				{
					cardData = Globals.Instance.GetCardData(list[i], instantiate: false);
					num3 = cardData.EnergyCost;
				}
				if (cardData != null && cardData.Playable)
				{
					num += (float)num3;
					AddToDictEnergy(num3);
					num2++;
				}
			}
		}
		if (num2 == 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		num /= (float)num2;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("challengeEnergy"));
		stringBuilder.Append("<br><color=#FF9F2A><size=+2>");
		stringBuilder.Append(num.ToString("0.00"));
		stringBuilder.Append("  <sprite name=energy>");
		cardChallengeEnergy.text = stringBuilder.ToString();
		WriteEnergyCostBars();
	}

	private void AddToDictEnergy(int value)
	{
		if (value > 5)
		{
			value = 5;
		}
		if (dictEnergyCost.ContainsKey(value))
		{
			dictEnergyCost[value]++;
		}
		else
		{
			dictEnergyCost.Add(value, 1);
		}
	}

	private void WriteEnergyCostBars()
	{
		float num = -1f;
		foreach (KeyValuePair<int, int> item in dictEnergyCost)
		{
			if ((float)item.Value >= num)
			{
				num = item.Value;
			}
		}
		float num2 = 0.8f;
		for (int i = 0; i < 6; i++)
		{
			int num3 = 0;
			if (dictEnergyCost.ContainsKey(i))
			{
				num3 = dictEnergyCost[i];
			}
			cardChallengeBar[i].transform.localScale = new Vector3(0.25f, num2 * (float)num3 / num, 1f);
			if (dictEnergyCost.ContainsKey(i))
			{
				cardChallengeBarSup[i].text = dictEnergyCost[i].ToString();
				cardChallengeBarSup[i].transform.localPosition = new Vector3(cardChallengeBarSup[i].transform.localPosition.x, cardChallengeBar[i].transform.localScale.y + 0.15f, cardChallengeBarSup[i].transform.localPosition.z);
			}
			else
			{
				cardChallengeBarSup[i].text = "";
			}
		}
	}
}
