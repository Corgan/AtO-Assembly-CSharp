using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardVertical : MonoBehaviour
{
	private Hero currentHero;

	public Transform button;

	public Transform iconlock;

	public new TMP_Text name;

	public TMP_Text nameOver;

	public Transform energyT;

	public TMP_Text energy;

	public Image bg;

	public Image bgClass;

	public Image bgRare;

	public Image skill;

	public Sprite bgPlain;

	public Sprite bgOver;

	public Sprite bgActive;

	public Transform rarityT;

	public ParticleSystem particleA;

	public ParticleSystem particleB;

	public ParticleSystem particleRare;

	private bool active;

	private CardItem CI;

	public CardData cardData;

	private string internalId;

	private int cardIndex;

	private Color colorA;

	private Color colorB;

	private Color colorRare;

	private Color colorActive;

	private void Awake()
	{
		CI = GetComponent<CardItem>();
		colorA = Functions.HexToColor("#67BCECFF");
		colorB = Functions.HexToColor("#FFB410FF");
		colorRare = Functions.HexToColor("#DA73FFFF");
		colorActive = Functions.HexToColor("#333333FF");
	}

	private void Start()
	{
		if (CardScreenManager.Instance.IsActive())
		{
			if (!button.gameObject.activeSelf)
			{
				button.gameObject.SetActive(value: true);
			}
			return;
		}
		if (button.gameObject.activeSelf)
		{
			button.gameObject.SetActive(value: false);
		}
		GetComponent<BoxCollider2D>().enabled = true;
	}

	public void ShowLock(bool state, bool paintItBlack = true)
	{
		iconlock.gameObject.SetActive(state);
		if (state && paintItBlack)
		{
			bgClass.color = new Color(0.2f, 0.2f, 0.2f);
		}
		else
		{
			bgClass.color = Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.CardClass), cardData.CardClass)]);
		}
	}

	public void SetBgColor(string _theColor)
	{
		bgClass.color = Functions.HexToColor(_theColor);
	}

	public void SetCard(string _cardId, int _cardType = 0, Hero _hero = null)
	{
		currentHero = _hero;
		internalId = _cardId;
		cardData = Globals.Instance.GetCardData(_cardId.Split('_')[0], instantiate: false);
		if (cardData == null)
		{
			return;
		}
		if (!_cardId.Contains("_"))
		{
			_cardId += "_0";
		}
		cardIndex = int.Parse(_cardId.Split('_')[1]);
		if (cardData.CardUpgraded == Enums.CardUpgraded.Rare)
		{
			name.color = colorRare;
			name.text = Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false).CardName;
		}
		else if (cardData.CardUpgraded == Enums.CardUpgraded.A)
		{
			name.color = colorA;
			name.text = Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false).CardName;
		}
		else if (cardData.CardUpgraded == Enums.CardUpgraded.B)
		{
			name.color = colorB;
			name.text = Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false).CardName;
		}
		else
		{
			name.text = cardData.CardName;
		}
		nameOver.text = name.text;
		if (AtOManager.Instance.GetHero(0) != null)
		{
			energy.text = AtOManager.Instance.GetHero(0).GetCardFinalCost(cardData).ToString();
		}
		else
		{
			energy.text = cardData.EnergyCost.ToString();
		}
		skill.sprite = cardData.Sprite;
		if (!cardData.FlipSprite)
		{
			if (skill.transform.localScale.x < 0f)
			{
				skill.transform.localScale = new Vector3(-1f * skill.transform.localScale.x, skill.transform.localScale.y, skill.transform.localScale.z);
			}
		}
		else if (cardData.FlipSprite && skill.transform.localScale.x > 0f)
		{
			skill.transform.localScale = new Vector3(-1f * skill.transform.localScale.x, skill.transform.localScale.y, skill.transform.localScale.z);
		}
		bgClass.color = Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.CardClass), cardData.CardClass)]);
		if (cardData.Playable)
		{
			if (!energyT.gameObject.activeSelf)
			{
				energyT.gameObject.SetActive(value: true);
			}
		}
		else if (energyT.gameObject.activeSelf)
		{
			energyT.gameObject.SetActive(value: false);
		}
		if (cardData.CardRarity == Enums.CardRarity.Uncommon)
		{
			if (!rarityT.gameObject.activeSelf)
			{
				rarityT.gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(0).gameObject.activeSelf)
			{
				rarityT.GetChild(0).gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(1).gameObject.activeSelf)
			{
				rarityT.GetChild(1).gameObject.SetActive(value: true);
			}
		}
		else if (cardData.CardRarity == Enums.CardRarity.Rare)
		{
			if (!rarityT.gameObject.activeSelf)
			{
				rarityT.gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(0).gameObject.activeSelf)
			{
				rarityT.GetChild(0).gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(2).gameObject.activeSelf)
			{
				rarityT.GetChild(2).gameObject.SetActive(value: true);
			}
		}
		else if (cardData.CardRarity == Enums.CardRarity.Epic)
		{
			if (!rarityT.gameObject.activeSelf)
			{
				rarityT.gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(0).gameObject.activeSelf)
			{
				rarityT.GetChild(0).gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(3).gameObject.activeSelf)
			{
				rarityT.GetChild(3).gameObject.SetActive(value: true);
			}
		}
		else if (cardData.CardRarity == Enums.CardRarity.Mythic)
		{
			if (!rarityT.gameObject.activeSelf)
			{
				rarityT.gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(0).gameObject.activeSelf)
			{
				rarityT.GetChild(0).gameObject.SetActive(value: true);
			}
			if (!rarityT.GetChild(4).gameObject.activeSelf)
			{
				rarityT.GetChild(4).gameObject.SetActive(value: true);
			}
		}
		else if (rarityT.gameObject.activeSelf)
		{
			rarityT.gameObject.SetActive(value: false);
		}
		if (bgRare.gameObject.activeSelf)
		{
			bgRare.gameObject.SetActive(value: false);
		}
	}

	public void SetCardData(CardData _cardData)
	{
		SetCard(_cardData.Id + "_" + cardIndex, 0, currentHero);
	}

	public void ReplaceWithCard(CardData _cardData, string type, bool showParticles = true)
	{
		SetCardData(_cardData);
		StartCoroutine(SetNameOverCo(type, showParticles));
	}

	public void PlayParticle(string type)
	{
		if (type == "A")
		{
			particleA.Play();
		}
		else
		{
			particleB.Play();
		}
	}

	private IEnumerator SetNameOverCo(string type, bool showParticles = true)
	{
		switch (type)
		{
		case "A":
			nameOver.color = colorA;
			if (showParticles)
			{
				particleA.Play();
			}
			break;
		case "B":
			nameOver.color = colorB;
			if (showParticles)
			{
				particleB.Play();
			}
			break;
		case "RARE":
			nameOver.color = colorRare;
			if (showParticles)
			{
				particleRare.Play();
			}
			break;
		}
		if (!nameOver.gameObject.activeSelf)
		{
			nameOver.gameObject.SetActive(value: true);
		}
		if (name.gameObject.activeSelf)
		{
			name.gameObject.SetActive(value: false);
		}
		yield return Globals.Instance.WaitForSeconds(1.2f);
		if (active)
		{
			ClearActive();
		}
	}

	private void SetActive()
	{
		CardCraftManager.Instance.ClearActiveCard();
		CardCraftManager.Instance.SetActiveCard(this);
		bg.sprite = bgActive;
		bgClass.color = colorActive;
		active = true;
		base.transform.localPosition = new Vector3(0.15f, base.transform.localPosition.y, base.transform.localPosition.z);
	}

	public void ClearActive()
	{
		active = false;
		bg.sprite = bgPlain;
		bgClass.color = Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.CardClass), cardData.CardClass)]);
		base.transform.localPosition = new Vector3(0f, base.transform.localPosition.y, base.transform.localPosition.z);
	}

	public bool IsLocked()
	{
		return iconlock.gameObject.activeSelf;
	}

	public void OnMouseUp()
	{
		if (!button.gameObject.activeSelf && !iconlock.gameObject.activeSelf)
		{
			fMouseUp();
		}
	}

	public void fMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && !TomeManager.Instance.IsActive() && !CardScreenManager.Instance.IsActive() && !CardCraftManager.Instance.blocked && (CardCraftManager.Instance.craftType == 0 || CardCraftManager.Instance.craftType == 1 || CardCraftManager.Instance.craftType == 6))
		{
			SetActive();
			CardCraftManager.Instance.SelectCard(internalId);
			CI.DestroyReveleadOutside();
			if (!GameManager.Instance.ConfigUseLegacySounds)
			{
				GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
			}
		}
	}

	public void OnMouseExit()
	{
		if (!button.gameObject.activeSelf)
		{
			fMouseExit();
		}
	}

	public void fMouseExit()
	{
		GameManager.Instance.CleanTempContainer();
		if (!active)
		{
			bg.sprite = bgPlain;
		}
	}

	public void OnMouseEnter()
	{
		if (!button.gameObject.activeSelf)
		{
			fMouseEnter();
		}
	}

	public void fMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!CardScreenManager.Instance.IsActive() || !(base.transform.parent.name != "VerticalContainer_Canvas")))
		{
			GameManager.Instance.CleanTempContainer();
			CI.cardoutsideverticallist = true;
			CI.CreateAmplifyOutsideCard(cardData, GetComponent<BoxCollider2D>(), currentHero);
			if (!active)
			{
				GameManager.Instance.PlayLibraryAudio("castnpccardfast");
				bg.sprite = bgOver;
			}
		}
	}
}
