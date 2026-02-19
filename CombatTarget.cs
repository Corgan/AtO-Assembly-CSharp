using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CombatTarget : MonoBehaviour
{
	public Transform elements;

	public SpriteRenderer image;

	public new TMP_Text name;

	public Transform bg;

	public Transform cards;

	public Transform cardsDeckT;

	public Transform cardsDiscardT;

	public TMP_Text cardsDeck;

	public TMP_Text cardsDiscard;

	public TMP_Text r0;

	public TMP_Text r1;

	public TMP_Text r2;

	public TMP_Text imm;

	private Character characterActive;

	private Character characterInStats;

	private string colorRed = "#FD7A76";

	private string colorGreen = "#50D75A";

	public GameObject GO_Buffs;

	public GameObject BuffPrefab;

	private Dictionary<string, List<string>> dictImmunityByItems = new Dictionary<string, List<string>>();

	private Vector3 bgSourcePosition;

	private Vector2 bgSourceSize;

	private int auraInTargetBox;

	private List<string> immuneList;

	private void Awake()
	{
		immuneList = new List<string>();
		bgSourcePosition = bg.localPosition;
		bgSourceSize = bg.GetComponent<SpriteRenderer>().size;
		imm.text = "";
		ShowElements(state: false);
	}

	public void SetTarget(Character character)
	{
		characterActive = character;
		ShowTarget();
	}

	public void SetTargetTMP(Character character)
	{
		ShowTarget(character);
	}

	private void ShowElements(bool state)
	{
		if (!state)
		{
			elements.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -100f);
			if ((bool)MatchManager.Instance)
			{
				MatchManager.Instance.ShowTraitInfo(state: true);
			}
		}
		else
		{
			elements.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f);
			if ((bool)MatchManager.Instance)
			{
				MatchManager.Instance.ShowTraitInfo(state: false);
			}
		}
	}

	public void ClearTarget()
	{
		ShowElements(state: false);
	}

	private void ShowTarget(Character character = null)
	{
		if (character == null)
		{
			character = characterActive;
		}
		if (character != null)
		{
			ShowElements(state: true);
			name.text = character.SourceName;
			image.sprite = character.SpriteSpeed;
			DoStats(character);
			DoCards(character);
			DoBuffs(character);
			ResizeBox();
		}
	}

	private void ResizeBox()
	{
		float num = (float)auraInTargetBox * 0.85f;
		float num2 = 0f;
		if (immuneList.Count > 0)
		{
			num2 = 0.7f;
		}
		bg.GetComponent<SpriteRenderer>().size = new Vector2(bgSourceSize.x + num, bgSourceSize.y + num2);
		bg.localPosition = bgSourcePosition + new Vector3(num * 0.5f * bg.transform.localScale.x, (0f - num2) * 0.5f * bg.transform.localScale.y, 0f);
	}

	private void DoStats(Character character)
	{
		if (character == null)
		{
			return;
		}
		string id = character.Id;
		characterInStats = character;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=2><sprite name=resist_slash></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Slashing), character.GetAuraResistModifiers(Enums.DamageType.Slashing)));
		stringBuilder.Append("\n");
		stringBuilder.Append("<size=2><sprite name=resist_fire></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Fire), character.GetAuraResistModifiers(Enums.DamageType.Fire)));
		stringBuilder.Append("\n");
		stringBuilder.Append("<size=2><sprite name=resist_holy></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Holy), character.GetAuraResistModifiers(Enums.DamageType.Holy)));
		stringBuilder.Append("\n");
		r0.text = stringBuilder.ToString();
		stringBuilder.Clear();
		stringBuilder.Append("<size=2><sprite name=resist_blunt></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Blunt), character.GetAuraResistModifiers(Enums.DamageType.Blunt)));
		stringBuilder.Append("\n");
		stringBuilder.Append("<size=2><sprite name=resist_cold></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Cold), character.GetAuraResistModifiers(Enums.DamageType.Cold)));
		stringBuilder.Append("\n");
		stringBuilder.Append("<size=2><sprite name=resist_shadow></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Shadow), character.GetAuraResistModifiers(Enums.DamageType.Shadow)));
		stringBuilder.Append("\n");
		r1.text = stringBuilder.ToString();
		stringBuilder.Clear();
		stringBuilder.Append("<size=2><sprite name=resist_piercing></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Piercing), character.GetAuraResistModifiers(Enums.DamageType.Piercing)));
		stringBuilder.Append("\n");
		stringBuilder.Append("<size=2><sprite name=resist_lightning></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Lightning), character.GetAuraResistModifiers(Enums.DamageType.Lightning)));
		stringBuilder.Append("\n");
		stringBuilder.Append("<size=2><sprite name=resist_mind></size> ");
		stringBuilder.Append(FormatColor(character.BonusResists(Enums.DamageType.Mind), character.GetAuraResistModifiers(Enums.DamageType.Mind)));
		stringBuilder.Append("\n");
		r2.text = stringBuilder.ToString();
		stringBuilder.Clear();
		immuneList.Clear();
		dictImmunityByItems.Clear();
		if (character != null && character.AuracurseImmune.Count > 0)
		{
			for (int i = 0; i < character.AuracurseImmune.Count; i++)
			{
				immuneList.Add(character.AuracurseImmune[i]);
			}
		}
		if (character != null && id != "" && !dictImmunityByItems.ContainsKey(id) && character.AuraCurseImmunitiesByItemsList() != null)
		{
			dictImmunityByItems.Add(id, character.AuraCurseImmunitiesByItemsList());
		}
		for (int j = 0; j < dictImmunityByItems[id].Count; j++)
		{
			if (!immuneList.Contains(dictImmunityByItems[id][j]))
			{
				immuneList.Add(dictImmunityByItems[id][j]);
			}
		}
		if (immuneList.Count > 0)
		{
			stringBuilder.Append(Texts.Instance.GetText("immune"));
			stringBuilder.Append(":  <voffset=-.2><size=+.3>");
			for (int k = 0; k < immuneList.Count; k++)
			{
				stringBuilder.Append("<sprite name=");
				stringBuilder.Append(immuneList[k]);
				stringBuilder.Append("> ");
			}
			stringBuilder.Append("</size>");
			imm.text = stringBuilder.ToString();
		}
		else
		{
			imm.text = "";
		}
	}

	public void DoCards(Character character)
	{
		if (character.HeroData != null)
		{
			cards.gameObject.SetActive(value: true);
			cardsDeck.text = MatchManager.Instance.CountHeroDeck(character.HeroIndex).ToString();
			cardsDiscard.text = MatchManager.Instance.CountHeroDiscard(character.HeroIndex).ToString();
			cardsDeckT.GetComponent<DeckInHero>().heroIndex = character.HeroIndex;
			cardsDiscardT.GetComponent<DeckInHero>().heroIndex = character.HeroIndex;
		}
		else
		{
			cards.gameObject.SetActive(value: false);
		}
	}

	public void Refresh()
	{
		if (elements.gameObject.activeSelf && elements.transform.position.z != -100f && characterInStats != null)
		{
			DoStats(characterInStats);
			DoBuffs(characterInStats);
			ResizeBox();
		}
	}

	public void RefreshCards()
	{
		if (characterInStats != null)
		{
			DoCards(characterInStats);
		}
	}

	private string FormatColor(int value, int mod)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (mod > 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorGreen);
			stringBuilder.Append(">");
		}
		else if (mod < 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorRed);
			stringBuilder.Append(">");
		}
		stringBuilder.Append(value);
		stringBuilder.Append("%");
		if (mod != 0)
		{
			stringBuilder.Append("</color>");
		}
		return stringBuilder.ToString();
	}

	public void DoBuffs(Character character)
	{
		_ = Vector3.zero;
		foreach (Transform item in GO_Buffs.transform)
		{
			Object.Destroy(item.gameObject);
		}
		int count = character.AuraList.Count;
		auraInTargetBox = 0;
		for (int i = 0; i < count; i++)
		{
			Aura aura = character.AuraList[i];
			if (aura != null && aura.ACData != null && (aura.ACData.Id == "energize" || aura.ACData.Id == "inspire" || aura.ACData.Id == "stress" || aura.ACData.Id == "fatigue"))
			{
				GameObject obj = Object.Instantiate(BuffPrefab, Vector3.zero, Quaternion.identity, GO_Buffs.transform);
				obj.GetComponentInChildren<Buff>()?.SetSortingInCombatHUD();
				string charId = ((character == null) ? "" : character.Id);
				obj.GetComponent<Buff>().SetBuff(aura.ACData, aura.GetCharges(), "", charId);
				obj.name = aura.ACData.ACName;
				auraInTargetBox++;
			}
		}
	}

	public void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && characterInStats != null)
		{
			GameManager.Instance.SetCursorPlain();
			int num = 0;
			num = ((!characterInStats.IsHero) ? characterInStats.NPCIndex : characterInStats.HeroIndex);
			MatchManager.Instance.ShowCharacterWindow("stats", characterInStats.IsHero, num);
		}
	}

	public void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			GameManager.Instance.SetCursorHover();
		}
	}

	public void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			GameManager.Instance.SetCursorPlain();
		}
	}
}
