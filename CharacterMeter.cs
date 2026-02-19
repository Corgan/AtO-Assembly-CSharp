using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMeter : MonoBehaviour
{
	public TMP_Text[] stats;

	public Transform[] icons;

	public Transform detailedData;

	public Image image;

	public Transform content;

	public void DoStats(int _index)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder();
		if (AtOManager.Instance.combatStats == null)
		{
			AtOManager.Instance.InitCombatStats();
		}
		if (AtOManager.Instance.combatStatsCurrent == null)
		{
			AtOManager.Instance.InitCombatStatsCurrent();
		}
		int num5 = 5;
		if (AtOManager.Instance.combatStats.GetLength(1) == 12)
		{
			num5 = 12;
			detailedData.gameObject.SetActive(value: true);
		}
		else
		{
			detailedData.gameObject.SetActive(value: false);
		}
		int num6 = 0;
		int num7 = 0;
		for (int i = 0; i < num5; i++)
		{
			num = 0;
			flag = true;
			if (AtOManager.Instance.combatStatsCurrent != null && i < AtOManager.Instance.combatStatsCurrent.GetLength(1))
			{
				num4 = AtOManager.Instance.combatStatsCurrent[_index, i];
			}
			num3 = AtOManager.Instance.combatStats[_index, i];
			if (i != 0 && i > 4)
			{
				num6 += num4;
				num7 += num3;
			}
			image.sprite = null;
			if (!TomeManager.Instance.IsActive())
			{
				if (AtOManager.Instance.GetHero(_index) != null && AtOManager.Instance.GetHero(_index).HeroData != null)
				{
					image.sprite = AtOManager.Instance.GetHero(_index).SpriteSpeed;
				}
			}
			else
			{
				GameObject gameObject = GameObject.Find("char" + (3 - _index));
				if (gameObject != null)
				{
					image.sprite = gameObject.transform.GetComponent<SpriteRenderer>().sprite;
				}
			}
			for (int j = 0; j < 4; j++)
			{
				_ = AtOManager.Instance.combatStats[j, i];
				num += AtOManager.Instance.combatStats[j, i];
				if (AtOManager.Instance.combatStatsCurrent != null && i < AtOManager.Instance.combatStatsCurrent.GetLength(1))
				{
					num2 += AtOManager.Instance.combatStatsCurrent[j, i];
				}
				if (!TomeManager.Instance.IsActive() && ((bool)MatchManager.Instance || (bool)RewardsManager.Instance))
				{
					if (num4 == 0 || num4 < AtOManager.Instance.combatStatsCurrent[j, i])
					{
						flag = false;
					}
				}
				else if (num3 == 0 || num3 < AtOManager.Instance.combatStats[j, i])
				{
					flag = false;
				}
			}
			DamageMeterManager.Instance.SetTotal(i, num);
			stringBuilder.Clear();
			if (flag && i < 5)
			{
				stringBuilder.Append("<color=#FFCC00>");
			}
			if (!TomeManager.Instance.IsActive() && ((bool)MatchManager.Instance || (bool)RewardsManager.Instance))
			{
				if (i > 4 && num4 > 0)
				{
					stringBuilder.Append("<color=#D9D9D9>");
				}
				if (num4 > 0)
				{
					stringBuilder.Append(num4);
				}
				else
				{
					stringBuilder.Append("-");
				}
			}
			else
			{
				if (i > 4 && num3 > 0)
				{
					stringBuilder.Append("<color=#D9D9D9>");
				}
				stringBuilder.Append(num3);
			}
			if (!TomeManager.Instance.IsActive() && ((bool)MatchManager.Instance || (bool)RewardsManager.Instance))
			{
				if (i < 5)
				{
					stringBuilder.Append("\n<size=-12><color=#CCC>(");
				}
				else
				{
					stringBuilder.Append("\n<size=-12><color=#888>(");
				}
				stringBuilder.Append(num3);
				stringBuilder.Append(")</color></size>");
			}
			if (flag && i < 5)
			{
				stringBuilder.Append("</color>");
			}
			stats[i].text = stringBuilder.ToString();
		}
		if (image.sprite == null)
		{
			content.gameObject.SetActive(value: false);
		}
		else
		{
			content.gameObject.SetActive(value: true);
		}
	}
}
