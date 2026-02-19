using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PerksWindowUI : MonoBehaviour
{
	public TMP_Text[] perkTextBlock;

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	public void DoPerks(Hero currentHero)
	{
		for (int i = 0; i < perkTextBlock.Length; i++)
		{
			perkTextBlock[i].text = "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		List<string> heroPerks = PlayerManager.Instance.GetHeroPerks(currentHero.Id);
		for (int j = 0; j < perkTextBlock.Length; j++)
		{
			int num = 0;
			for (int k = 0; k < 6; k++)
			{
				string text = currentHero.ClassName + (j + 1);
				switch (k)
				{
				case 0:
					text += "a";
					break;
				case 1:
					text += "b";
					break;
				case 2:
					text += "c";
					break;
				case 3:
					text += "d";
					break;
				case 4:
					text += "e";
					break;
				case 5:
					text += "f";
					break;
				}
				PerkData perkData = Globals.Instance.GetPerkData(text);
				if (perkData != null)
				{
					bool flag = false;
					if (heroPerks != null && heroPerks.Contains(text))
					{
						flag = true;
						stringBuilder2.Append("<color=#E0A44E><b>");
						num++;
					}
					stringBuilder2.Append("<sprite name=" + perkData.Icon.name + ">");
					stringBuilder2.Append(Perk.PerkDescription(perkData));
					if (flag)
					{
						stringBuilder2.Append("</b></color>");
					}
					stringBuilder2.Append("<br>");
					stringBuilder.Append(stringBuilder2.ToString());
					stringBuilder2.Clear();
				}
			}
			stringBuilder2.Append("<size=+1.2><sprite name=perk><color=#CC81FF><b>");
			stringBuilder2.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("rankTier")), Perk.RomanLevel(j + 1)).ToUpper());
			stringBuilder2.Append("</b></color></size>  <color=#FFF><size=+.3>(");
			stringBuilder2.Append(num);
			stringBuilder2.Append("/6)</size></color>");
			stringBuilder2.Append("<br><line-height=20%><br></line-height><color=#777>");
			stringBuilder2.Append(stringBuilder.ToString());
			perkTextBlock[j].text = stringBuilder2.ToString();
			stringBuilder.Clear();
			stringBuilder2.Clear();
		}
	}

	public void DoPerksOld(Hero currentHero)
	{
		for (int i = 0; i < perkTextBlock.Length; i++)
		{
			perkTextBlock[i].gameObject.SetActive(value: false);
			perkTextBlock[i].text = "";
		}
		List<string> heroPerks = PlayerManager.Instance.GetHeroPerks(currentHero.Id);
		if (heroPerks != null && heroPerks.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = 0; j < heroPerks.Count; j++)
			{
				for (int k = 0; k < 6; k++)
				{
					string text = currentHero.ClassName + (j + 1);
					switch (k)
					{
					case 0:
						text += "a";
						break;
					case 1:
						text += "b";
						break;
					case 2:
						text += "c";
						break;
					case 3:
						text += "d";
						break;
					case 4:
						text += "e";
						break;
					case 5:
						text += "f";
						break;
					}
					PerkData perkData = Globals.Instance.GetPerkData(text);
					if (perkData != null)
					{
						stringBuilder2.Append("<sprite name=" + perkData.IconTextValue + ">");
						stringBuilder2.Append(Perk.PerkDescription(perkData));
						stringBuilder.Append(stringBuilder2.ToString());
					}
				}
				perkTextBlock[j].text += stringBuilder.ToString();
				stringBuilder.Clear();
			}
			for (int l = 0; l < perkTextBlock.Length; l++)
			{
				if (perkTextBlock[l].text != "")
				{
					stringBuilder.Append("<size=+.8><sprite name=perk><color=#CC81FF>");
					stringBuilder.Append(string.Format(Texts.Instance.GetText("rankTier"), Perk.RomanLevel(l + 1)));
					stringBuilder.Append("</color></size><br><br>");
					perkTextBlock[l].text = stringBuilder.ToString() + perkTextBlock[l].text;
					stringBuilder.Clear();
					perkTextBlock[l].gameObject.SetActive(value: true);
				}
			}
		}
		else
		{
			perkTextBlock[0].gameObject.SetActive(value: true);
			perkTextBlock[0].text = "<size=+2>" + Texts.Instance.GetText("perkNone");
		}
	}
}
