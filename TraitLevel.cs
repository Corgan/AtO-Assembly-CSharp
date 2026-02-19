using System.Text;
using TMPro;
using UnityEngine;

public class TraitLevel : MonoBehaviour
{
	public Transform boxInnate;

	public Transform textInnate;

	public Transform boxRegular;

	public Transform textRegular;

	private SpriteRenderer boxSpr;

	private TMP_Text boxText;

	private Color colorText;

	private int heroIndex;

	private int boxType;

	private TraitData traitData;

	private string heroColor = "";

	private Animator anim;

	private new bool enabled;

	private bool active;

	private BoxCollider2D collider;

	private int traitLevel;

	private CardItem CI;

	private void Awake()
	{
		collider = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
		anim.enabled = false;
		CI = GetComponent<CardItem>();
	}

	public void SetHeroIndex(int _heroIndex)
	{
		heroIndex = _heroIndex;
	}

	public void SetColor(string _color)
	{
		heroColor = _color;
	}

	public void SetPosition(int _boxType)
	{
		switch (_boxType)
		{
		case 1:
			boxRegular.gameObject.SetActive(value: true);
			textRegular.gameObject.SetActive(value: true);
			boxInnate.gameObject.SetActive(value: false);
			textInnate.gameObject.SetActive(value: false);
			boxSpr = boxRegular.GetComponent<SpriteRenderer>();
			boxText = textRegular.GetComponent<TMP_Text>();
			break;
		case 2:
			boxRegular.gameObject.SetActive(value: true);
			boxRegular.localScale = new Vector3(-1f, boxRegular.localScale.y, 1f);
			textRegular.gameObject.SetActive(value: true);
			boxInnate.gameObject.SetActive(value: false);
			textInnate.gameObject.SetActive(value: false);
			boxSpr = boxRegular.GetComponent<SpriteRenderer>();
			boxText = textRegular.GetComponent<TMP_Text>();
			break;
		default:
			boxRegular.gameObject.SetActive(value: false);
			textRegular.gameObject.SetActive(value: false);
			boxInnate.gameObject.SetActive(value: true);
			textInnate.gameObject.SetActive(value: true);
			boxSpr = boxInnate.GetComponent<SpriteRenderer>();
			boxText = textInnate.GetComponent<TMP_Text>();
			break;
		}
		boxType = _boxType;
		colorText = boxText.color;
	}

	public void SetTrait(TraitData _traitData, int _traitLevel = 0)
	{
		traitLevel = _traitLevel;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			if (GameManager.Instance.IsWeeklyChallenge())
			{
				traitLevel = 2;
			}
			else
			{
				int obeliskMadness = AtOManager.Instance.GetObeliskMadness();
				if (obeliskMadness >= 5)
				{
					if (obeliskMadness < 8)
					{
						traitLevel = 1;
					}
					else
					{
						traitLevel = 2;
					}
				}
			}
		}
		traitData = Globals.Instance.GetTraitData(_traitData.Id);
		if (traitData != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!enabled && !active)
			{
				stringBuilder.Append("<color=#666666>");
			}
			stringBuilder.Append("<size=+.25><color=");
			if (enabled || active)
			{
				stringBuilder.Append(heroColor);
			}
			else if (!active)
			{
				stringBuilder.Append("#999");
			}
			stringBuilder.Append(">");
			if (traitData.TraitCard == null)
			{
				stringBuilder.Append(traitData.TraitName);
			}
			else
			{
				stringBuilder.Append(Globals.Instance.GetCardData(traitData.TraitCard.Id, instantiate: false).CardName);
			}
			stringBuilder.Append("</color></size>\n");
			if (traitData.TraitCard == null)
			{
				stringBuilder.Append(traitData.Description);
			}
			else
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("traitAddCard"), Globals.Instance.GetCardData(traitData.TraitCard.Id, instantiate: false).CardName));
			}
			if (!enabled && !active)
			{
				stringBuilder.Append("</color>");
			}
			boxText.text = stringBuilder.ToString();
			if (active)
			{
				boxText.color = Functions.HexToColor("#D4AC5B");
			}
		}
	}

	public void SetEnable(bool _state)
	{
		if (_state)
		{
			boxSpr.color = Functions.HexToColor(heroColor);
		}
		else
		{
			boxSpr.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
		}
		enabled = _state;
	}

	public void SetActive(bool _state)
	{
		active = _state;
		if (active)
		{
			if (anim != null)
			{
				anim.enabled = true;
			}
			boxSpr.color = Functions.HexToColor("#FFCC00");
			heroColor = "#FFCC00";
		}
		else if (anim != null)
		{
			anim.enabled = false;
		}
	}

	private void OnMouseEnter()
	{
		if (AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive())
		{
			return;
		}
		if (traitData != null)
		{
			if (traitData.TraitCard == null && traitData.TraitCardForAllHeroes == null)
			{
				PopupManager.Instance.SetTrait(traitData, includeDescription: false);
				if (CI != null)
				{
					CI.CardData = null;
					CI.enabled = false;
				}
			}
			else
			{
				string text = "";
				if (traitData.TraitCard != null)
				{
					text = traitData.TraitCard.Id;
					if (traitLevel == 0)
					{
						CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(text), GetComponent<BoxCollider2D>());
					}
					else if (traitLevel == 1)
					{
						CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(Globals.Instance.GetCardData(text, instantiate: false).UpgradesTo1), GetComponent<BoxCollider2D>());
					}
					else if (traitLevel == 2)
					{
						CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(Globals.Instance.GetCardData(text, instantiate: false).UpgradesTo2), GetComponent<BoxCollider2D>());
					}
				}
				else if (traitData.TraitCardForAllHeroes != null)
				{
					text = traitData.TraitCardForAllHeroes.Id;
					CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(text), GetComponent<BoxCollider2D>());
				}
				if (CI != null)
				{
					CI.enabled = true;
				}
			}
		}
		if (!active && !enabled)
		{
			boxSpr.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
		}
		else if (active)
		{
			base.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
			GameManager.Instance.PlayLibraryAudio("ui_menu_popup_01");
		}
	}

	private void OnMouseExit()
	{
		GameManager.Instance.CleanTempContainer();
		PopupManager.Instance.ClosePopup();
		if (!active && !enabled)
		{
			boxSpr.color = new Color(0.5f, 0.5f, 0.5f, 0.4f);
		}
		else if (active)
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void OnMouseUp()
	{
		if (!active || !Functions.ClickedThisTransform(base.transform) || AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive())
		{
			return;
		}
		if ((bool)TownManager.Instance || (bool)MapManager.Instance)
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				AtOManager.Instance.HeroLevelUp(heroIndex, traitData.Id);
			}
			else
			{
				AtOManager.Instance.HeroLevelUpMP(heroIndex, traitData.Id);
			}
		}
		else
		{
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("cantLevelUp"));
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		PopupManager.Instance.ClosePopup();
	}
}
