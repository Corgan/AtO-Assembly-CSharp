using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class CombatTextInstance : MonoBehaviour
{
	public Transform textTMT;

	private Animator animator;

	private TMP_Text textTM;

	private CastResolutionForCombatText _cast;

	private bool inversed;

	private void Awake()
	{
		textTM = textTMT.GetComponent<TMP_Text>();
		base.transform.localPosition = new Vector3(0f, 2.6f, 0f);
		textTMT.gameObject.SetActive(value: false);
		animator = GetComponent<Animator>();
	}

	public void ShowDamage(CombatText CT, CharacterItem characterItem, CastResolutionForCombatText _castObj)
	{
		_cast = _castObj;
		int num = _cast.damage + _cast.damage2;
		int heal = _cast.heal;
		string effect = _cast.effect;
		if (num > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (effect != "")
			{
				stringBuilder.Append("<sprite name=");
				stringBuilder.Append(effect.ToLower());
				stringBuilder.Append(">");
			}
			stringBuilder.Append("-");
			stringBuilder.Append(num);
			stringBuilder.Append(" Hp");
			ShowText(CT, characterItem, stringBuilder.ToString(), Enums.CombatScrollEffectType.Damage);
			return;
		}
		if (heal > 0)
		{
			ShowText(CT, characterItem, heal.ToString(), Enums.CombatScrollEffectType.Heal);
			return;
		}
		int blocked = _cast.blocked;
		bool immune = _cast.immune;
		bool invulnerable = _cast.invulnerable;
		bool evaded = _cast.evaded;
		bool fullblocked = _cast.fullblocked;
		StringBuilder stringBuilder2 = new StringBuilder();
		if (invulnerable)
		{
			if (effect != "")
			{
				stringBuilder2.Append(effect);
				stringBuilder2.Append("\n*");
				stringBuilder2.Append(Texts.Instance.GetText("invulnerable"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Damage);
			}
			else
			{
				stringBuilder2.Append("*");
				stringBuilder2.Append(Texts.Instance.GetText("invulnerable"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Aura);
			}
			if (!GameManager.Instance.ConfigUseLegacySounds)
			{
				GameManager.Instance.PlayLibraryAudio("sx_combat_card_Drag_defensemagical_03");
			}
			else
			{
				GameManager.Instance.PlayLibraryAudio("invulnerable_sound");
			}
			if (characterItem.IsHero)
			{
				EffectsManager.Instance.PlayEffectAC("invulnerable", isHero: true, characterItem.CharImageT, flip: false);
			}
			else
			{
				EffectsManager.Instance.PlayEffectAC("invulnerable", isHero: false, characterItem.CharImageT, flip: true);
			}
		}
		else if (evaded)
		{
			if (effect != "")
			{
				stringBuilder2.Append(effect);
				stringBuilder2.Append("\n*");
				stringBuilder2.Append(Texts.Instance.GetText("evaded"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Damage);
			}
			else
			{
				stringBuilder2.Append("*");
				stringBuilder2.Append(Texts.Instance.GetText("evaded"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Aura);
			}
			if (characterItem.IsHero)
			{
				EffectsManager.Instance.PlayEffectAC("evasion", isHero: true, characterItem.CharImageT, flip: false);
			}
			else
			{
				EffectsManager.Instance.PlayEffectAC("evasion", isHero: false, characterItem.CharImageT, flip: true);
			}
		}
		else if (fullblocked)
		{
			if (effect != "")
			{
				stringBuilder2.Append(Texts.Instance.GetText(effect));
				stringBuilder2.Append("\n*");
				stringBuilder2.Append(Texts.Instance.GetText("blocked"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Damage);
			}
			else
			{
				stringBuilder2.Append("*");
				stringBuilder2.Append(Texts.Instance.GetText("blocked"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Aura);
			}
			GameManager.Instance.PlayLibraryAudio("block_attack_sound");
			if (characterItem.IsHero)
			{
				EffectsManager.Instance.PlayEffectAC("blocked", isHero: true, characterItem.CharImageT, flip: false);
			}
			else
			{
				EffectsManager.Instance.PlayEffectAC("blocked", isHero: false, characterItem.CharImageT, flip: true);
			}
		}
		else if (immune)
		{
			if (effect != "")
			{
				stringBuilder2.Append(Texts.Instance.GetText(effect));
				stringBuilder2.Append("\n*");
				stringBuilder2.Append(Texts.Instance.GetText("immune"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Damage);
			}
			else
			{
				stringBuilder2.Append("*");
				stringBuilder2.Append(Texts.Instance.GetText("immune"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Damage);
			}
		}
		else if (blocked > 0)
		{
			if (effect != "")
			{
				stringBuilder2.Append(Texts.Instance.GetText(effect));
				stringBuilder2.Append("\n*");
				stringBuilder2.Append(Texts.Instance.GetText("blocked"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Damage);
			}
			else
			{
				stringBuilder2.Append("*");
				stringBuilder2.Append(Texts.Instance.GetText("blocked"));
				stringBuilder2.Append("*");
				ShowText(CT, characterItem, stringBuilder2.ToString(), Enums.CombatScrollEffectType.Aura);
			}
			GameManager.Instance.PlayLibraryAudio("block_attack_sound");
			if (characterItem.IsHero)
			{
				EffectsManager.Instance.PlayEffectAC("blocked", isHero: true, characterItem.CharImageT, flip: false);
			}
			else
			{
				EffectsManager.Instance.PlayEffectAC("blocked", isHero: false, characterItem.CharImageT, flip: true);
			}
		}
		else if (_cast.mitigated)
		{
			ShowText(CT, characterItem, "0", Enums.CombatScrollEffectType.Damage);
		}
	}

	public void ShowText(CombatText CT, CharacterItem characterItem, string text, Enums.CombatScrollEffectType type)
	{
		Color color;
		switch (type)
		{
		case Enums.CombatScrollEffectType.Damage:
			color = new Color(0.87f, 0.16f, 0.07f, 1f);
			break;
		case Enums.CombatScrollEffectType.Heal:
			color = new Color(0.13f, 0.56f, 0.17f, 1f);
			break;
		case Enums.CombatScrollEffectType.Aura:
			color = new Color(0.2f, 0.76f, 0.94f, 1f);
			break;
		case Enums.CombatScrollEffectType.Curse:
			color = new Color(0.61f, 0.61f, 0.61f, 1f);
			break;
		case Enums.CombatScrollEffectType.Energy:
			color = new Color(1f, 0.66f, 0f, 1f);
			break;
		case Enums.CombatScrollEffectType.Block:
			color = new Color(0.45f, 0.45f, 0.45f, 1f);
			break;
		case Enums.CombatScrollEffectType.Trait:
			color = new Color(0.74f, 0.43f, 0.21f, 1f);
			break;
		case Enums.CombatScrollEffectType.Weapon:
		case Enums.CombatScrollEffectType.Armor:
		case Enums.CombatScrollEffectType.Jewelry:
		case Enums.CombatScrollEffectType.Accesory:
			color = new Color(1f, 0.78f, 0.4f, 1f);
			break;
		case Enums.CombatScrollEffectType.Corruption:
			color = new Color(0.64f, 0.18f, 0.57f, 1f);
			break;
		default:
			color = new Color(0.41f, 0.41f, 0.41f, 1f);
			break;
		}
		Color color2 = color + new Color(0.2f, 0.2f, 0.2f);
		Color color3 = color;
		if (type != Enums.CombatScrollEffectType.Damage && type != Enums.CombatScrollEffectType.Heal)
		{
			textTM.colorGradient = new VertexGradient(color2, color2, color3, color3);
		}
		switch (type)
		{
		case Enums.CombatScrollEffectType.Trait:
			text = "<sprite name=experience> " + text;
			break;
		case Enums.CombatScrollEffectType.Weapon:
			text = "<sprite name=weapon> " + text;
			break;
		case Enums.CombatScrollEffectType.Armor:
			text = "<sprite name=armor> " + text;
			break;
		case Enums.CombatScrollEffectType.Jewelry:
			text = "<sprite name=jewelry> " + text;
			break;
		case Enums.CombatScrollEffectType.Accesory:
			text = "<sprite name=accesory> " + text;
			break;
		case Enums.CombatScrollEffectType.Corruption:
			text = "<sprite name=corruption> " + text;
			break;
		}
		textTM.text = text;
		StartCoroutine(ScrollCombatText());
	}

	private IEnumerator ScrollCombatText()
	{
		textTMT.gameObject.SetActive(value: true);
		new Color(0f, 0f, 0f, 0.03f);
		MatchManager.Instance.CombatTextIterations++;
		if (_cast != null && (_cast.damage != 0 || _cast.damage2 != 0 || _cast.heal != 0 || _cast.mitigated))
		{
			if (inversed)
			{
				base.transform.localScale = new Vector3(-1f * base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
			}
			textTM.GetComponent<MeshRenderer>().sortingOrder = 20000 + MatchManager.Instance.CombatTextIterations;
			StringBuilder stringBuilder = new StringBuilder();
			if (_cast.damage != 0 || _cast.mitigated)
			{
				stringBuilder.Append(_cast.damage);
				if (_cast.damageType != Enums.DamageType.None)
				{
					stringBuilder.Append(" <size=2.4><sprite name=");
					stringBuilder.Append(Enum.GetName(typeof(Enums.DamageType), _cast.damageType).ToLower());
					stringBuilder.Append("></size>");
				}
				else if (_cast.effect != "")
				{
					stringBuilder.Append(" <size=2.4><sprite name=");
					stringBuilder.Append(_cast.effect.ToLower());
					stringBuilder.Append("></size>");
				}
				else if (_cast.damage > 0)
				{
					stringBuilder.Append(" <size=2.4><sprite name=");
					stringBuilder.Append("heart");
					stringBuilder.Append("></size>");
				}
				if (_cast.damage2 > 0)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append(_cast.damage2);
					if (_cast.damageType2 != Enums.DamageType.None)
					{
						stringBuilder.Append(" <size=2.4><sprite name=");
						stringBuilder.Append(Enum.GetName(typeof(Enums.DamageType), _cast.damageType2).ToLower());
						stringBuilder.Append("></size>");
					}
					else if (_cast.effect != "")
					{
						stringBuilder.Append(" <size=2.4><sprite name=");
						stringBuilder.Append(_cast.effect.ToLower());
						stringBuilder.Append("></size>");
					}
					else if (_cast.damage2 > 0)
					{
						stringBuilder.Append(" <size=2.4><sprite name=");
						stringBuilder.Append("heart");
						stringBuilder.Append("></size>");
					}
				}
			}
			else if (_cast.damage2 > 0)
			{
				stringBuilder.Append(_cast.damage2);
				stringBuilder.Append(" <size=2.4><sprite name=");
				if (_cast.damageType2 != Enums.DamageType.None)
				{
					stringBuilder.Append(Enum.GetName(typeof(Enums.DamageType), _cast.damageType2).ToLower());
				}
				else if (_cast.effect != "")
				{
					stringBuilder.Append(_cast.effect.ToLower());
				}
				else
				{
					stringBuilder.Append("heart");
				}
				stringBuilder.Append("></size>");
			}
			else
			{
				stringBuilder.Append(_cast.heal);
				stringBuilder.Append(" <size=2.4><sprite name=heal>");
			}
			textTM.text = stringBuilder.ToString();
			if (base.transform.position.x > 0f)
			{
				animator.SetTrigger("popL");
			}
			else
			{
				animator.SetTrigger("popR");
			}
		}
		else
		{
			textTM.GetComponent<MeshRenderer>().sortingOrder = 10000 + MatchManager.Instance.CombatTextIterations;
			if (!inversed)
			{
				base.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(-1.5f, 1.5f, 1f);
			}
			animator.SetTrigger("scroll");
		}
		animator.enabled = true;
		yield return Globals.Instance.WaitForSeconds(3f);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
