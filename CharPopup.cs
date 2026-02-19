using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using WebSocketSharp;

public class CharPopup : MonoBehaviour
{
	private SubClassData SCD;

	private SubClassData oldSCD;

	public Transform groupCharacter;

	public Transform groupStats;

	public BotonGeneric buttonStats;

	public Transform groupRank;

	public BotonGeneric buttonRank;

	public Transform groupPerks;

	public BotonGeneric buttonPerks;

	public BotonGeneric buttonResetPerks;

	public Transform groupSkins;

	public BotonGeneric buttonSkins;

	public Transform groupCardback;

	public Transform groupCardbackGOsCharacter;

	public Transform groupCardbackGOsGeneral;

	public Transform groupCardbackGOsMisc;

	public BotonGeneric buttonCardback;

	public GameObject goCardback;

	public Transform groupSingularityCards;

	public BotonGeneric buttonSingularityCards;

	public Transform groupSingularityCardsContainer;

	public Transform buttonClose;

	public TMP_Text warningPerk;

	public Transform warningPerkT;

	public TMP_Text rankProgress;

	public TMP_Text maxProgress;

	public TMP_Text availablePerksPoints;

	public Transform perkBarMask;

	public SpriteRenderer perkBar;

	public Transform perksNotAvailable;

	public Transform dlcCharacter;

	public TMP_Text _Name;

	public TMP_Text _Class;

	public TMP_Text _Fluff;

	public TMP_Text _HP;

	public TMP_Text _Energy;

	public TMP_Text _Speed;

	public TMP_Text _Slashing;

	public TMP_Text _Blunt;

	public TMP_Text _Piercing;

	public TMP_Text _Fire;

	public TMP_Text _Cold;

	public TMP_Text _Lightning;

	public TMP_Text _Mind;

	public TMP_Text _Holy;

	public TMP_Text _Shadow;

	public TMP_Text _Trait0;

	public TMP_Text _Trait1A;

	public TMP_Text _Trait1B;

	public TMP_Text _Trait2A;

	public TMP_Text _Trait2B;

	public TMP_Text _Trait3A;

	public TMP_Text _Trait3B;

	public TMP_Text _Trait4A;

	public TMP_Text _Trait4B;

	public TMP_Text traitListTxt;

	private TraitRollOver _Trait0RO;

	private TraitRollOver _Trait1ARO;

	private TraitRollOver _Trait1BRO;

	private TraitRollOver _Trait2ARO;

	private TraitRollOver _Trait2BRO;

	private TraitRollOver _Trait3ARO;

	private TraitRollOver _Trait3BRO;

	private TraitRollOver _Trait4ARO;

	private TraitRollOver _Trait4BRO;

	public SpriteRenderer _OverAnimated;

	public TMP_Text _CardN1;

	public TMP_Text _CardN2;

	public TMP_Text _CardN3;

	public TMP_Text _CardN4;

	public TMP_Text _CardN5;

	public TMP_Text _CardN6;

	public TMP_Text _CardN7;

	public GameObject _CardPrefab;

	public Transform _CardParent;

	public Transform _HeroParent;

	public SpriteRenderer _SpriteSubstitution;

	private Vector3 destination = Vector3.zero;

	private bool moveThis;

	private bool closeThis;

	private bool opened;

	private CardItem[] cardsCI = new CardItem[7];

	private Transform[] cardsT = new Transform[7];

	private Transform[] cardsNumT = new Transform[7];

	private TMP_Text[] cardsNumText = new TMP_Text[7];

	private CardItem[] cardsSING = new CardItem[15];

	public Transform initialCards;

	public Transform checkSingularityCards;

	public Transform cardItemT;

	public Transform cardItemNoneT;

	private CardItem cardItemCI;

	private Vector3 destinationCenter = Vector3.zero;

	private Vector3 destinationOut = Vector3.zero;

	private GameObject heroAnimated;

	private string activeId = "";

	public PerkColumn[] perkColumns;

	public BotonSkin[] botonSkinBase;

	[SerializeField]
	private RectTransform progressionRowsContainer;

	public ProgressionRow[] progressionRows;

	private List<SpriteRenderer> animatedSprites;

	private List<SetSpriteLayerFromBase> animatedSpritesOutOfCharacter;

	public TMP_Text useSuppliesAvailable;

	public BotonGeneric useSuppliesButton;

	public Transform useSupplyDisclaimer;

	public SpriteRenderer useSuppliesBg;

	private Color useSuppliesBgOn = new Color(0.41f, 0.3f, 0.2f, 0.5f);

	private Color useSuppliesBgOff = new Color(0.4f, 0.13f, 0.1f, 0.5f);

	private Animator heroAnim;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private List<Transform> _controllerVerticalList = new List<Transform>();

	private Color botColorEnabled;

	private Color botColorDisabled;

	[SerializeField]
	private Transform movableElements;

	public bool MoveThis
	{
		get
		{
			return moveThis;
		}
		set
		{
			moveThis = value;
		}
	}

	private void Awake()
	{
		if (Globals.Instance?.CurrentLang == "zh-CN" || Globals.Instance?.CurrentLang == "zh-TW" || Globals.Instance?.CurrentLang == "jp")
		{
			traitListTxt.lineSpacing = 16f;
		}
		botColorEnabled = new Color(0.25f, 0.25f, 0.25f, 1f);
		botColorDisabled = new Color(0.96f, 0.62f, 0.05f, 1f);
		groupStats.gameObject.SetActive(value: true);
		_Trait0RO = _Trait0.transform.parent.GetComponent<TraitRollOver>();
		_Trait1ARO = _Trait1A.transform.parent.GetComponent<TraitRollOver>();
		_Trait1BRO = _Trait1B.transform.parent.GetComponent<TraitRollOver>();
		_Trait2ARO = _Trait2A.transform.parent.GetComponent<TraitRollOver>();
		_Trait2BRO = _Trait2B.transform.parent.GetComponent<TraitRollOver>();
		_Trait3ARO = _Trait3A.transform.parent.GetComponent<TraitRollOver>();
		_Trait3BRO = _Trait3B.transform.parent.GetComponent<TraitRollOver>();
		_Trait4ARO = _Trait4A.transform.parent.GetComponent<TraitRollOver>();
		_Trait4BRO = _Trait4B.transform.parent.GetComponent<TraitRollOver>();
		for (int i = 0; i < 7; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, _CardParent);
			CardItem component = obj.GetComponent<CardItem>();
			obj.name = "card_" + i;
			component.cardoutsidecombat = true;
			component.cardoutsideselection = true;
			component.TopLayeringOrder("UI");
			Transform transform = obj.transform;
			transform.localPosition = new Vector3(-1.8f + (float)i * 1.3f, -1.15f, 0f);
			transform.localScale = new Vector3(0.65f, 0.65f, 1f);
			transform.gameObject.SetActive(value: false);
			cardsCI[i] = component;
			cardsT[i] = transform;
		}
		for (int j = 0; j < 15; j++)
		{
			GameObject obj2 = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, groupSingularityCardsContainer);
			CardItem component = obj2.GetComponent<CardItem>();
			obj2.name = "card_" + j;
			component.cardmakebig = true;
			component.TopLayeringOrder("UI");
			Transform transform = obj2.transform;
			transform.localPosition = new Vector3((float)(j % 8) * 1.75f, -2.7f * (float)(j / 8), 0f - (1f + (float)j * 0.01f));
			transform.localScale = new Vector3(0.9f, 0.9f, 1f);
			cardsSING[j] = component;
		}
		cardsNumT[0] = _CardN1.transform.parent;
		cardsNumText[0] = _CardN1;
		cardsNumT[1] = _CardN2.transform.parent;
		cardsNumText[1] = _CardN2;
		cardsNumT[2] = _CardN3.transform.parent;
		cardsNumText[2] = _CardN3;
		cardsNumT[3] = _CardN4.transform.parent;
		cardsNumText[3] = _CardN4;
		cardsNumT[4] = _CardN5.transform.parent;
		cardsNumText[4] = _CardN5;
		cardsNumT[5] = _CardN6.transform.parent;
		cardsNumText[5] = _CardN6;
		cardsNumT[6] = _CardN7.transform.parent;
		cardsNumText[6] = _CardN7;
		cardItemCI = cardItemT.GetComponent<CardItem>();
	}

	private void Start()
	{
		if (GameManager.Instance.IsObeliskChallenge())
		{
			buttonRank.GetComponent<PopupText>().id = "perksNotChallenge";
			buttonPerks.GetComponent<PopupText>().id = "perksNotChallenge";
		}
		else
		{
			buttonRank.GetComponent<PopupText>().id = "";
			buttonPerks.GetComponent<PopupText>().id = "";
		}
		if (GameManager.Instance.IsSingularity())
		{
			buttonSingularityCards.gameObject.SetActive(value: true);
		}
		else
		{
			buttonSingularityCards.gameObject.SetActive(value: false);
		}
		HideNow();
	}

	private void Update()
	{
		if (!moveThis)
		{
			return;
		}
		movableElements.position = Vector3.Lerp(movableElements.position, destination, Time.deltaTime * 15f);
		if (Vector3.Distance(movableElements.position, destination) < 0.02f)
		{
			movableElements.position = destination;
			moveThis = false;
			if (closeThis)
			{
				closeThis = false;
				HideNow();
				opened = false;
			}
			else
			{
				opened = true;
			}
		}
	}

	public bool IsOpened()
	{
		return opened;
	}

	public string GetActive()
	{
		return activeId;
	}

	public void Trigger(SubClassData _scd, bool isHeroStats)
	{
		if (!opened)
		{
			if (activeId != _scd.Id)
			{
				Init(_scd, isHeroStats);
			}
			if (isHeroStats)
			{
				Show();
			}
		}
		else if (isHeroStats)
		{
			if (groupStats.gameObject.activeSelf && activeId == _scd.Id)
			{
				Close();
			}
			else if (activeId == _scd.Id)
			{
				ShowStats();
			}
			else
			{
				Init(_scd, isHeroStats);
			}
		}
		else if (groupPerks.gameObject.activeSelf && activeId == _scd.Id)
		{
			Close();
		}
		else if (activeId == _scd.Id)
		{
			ShowPerks();
		}
		else
		{
			Init(_scd, isHeroStats);
		}
	}

	public void RefreshBecauseOfPerks()
	{
		Init(SCD, doStats: false);
	}

	public void Init(SubClassData _scd, bool doStats = true, bool showNothing = false)
	{
		if (_scd == null)
		{
			return;
		}
		SCD = _scd;
		int characterTier = PlayerManager.Instance.GetCharacterTier(_scd.Id, "trait");
		int characterTier2 = PlayerManager.Instance.GetCharacterTier(_scd.Id, "item");
		int characterTier3 = PlayerManager.Instance.GetCharacterTier(_scd.Id, "card");
		activeId = _scd.Id;
		_OverAnimated.color = Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), _scd.HeroClass)]);
		_OverAnimated.color = new Color(_OverAnimated.color.r, _OverAnimated.color.g, _OverAnimated.color.b, 0.2f);
		_Name.text = (buttonResetPerks.auxString = _scd.CharacterName);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Functions.UppercaseFirst(Texts.Instance.GetText(_scd.SubClassName)));
		stringBuilder.Append("  <size=-.4><sprite name=");
		stringBuilder.Append(Functions.ClassIconFromString(Enum.GetName(typeof(Enums.HeroClass), _scd.HeroClass)));
		stringBuilder.Append(">");
		if (_scd.HeroClassSecondary != Enums.HeroClass.None)
		{
			stringBuilder.Append("<size=2><voffset=.5><color=#444>|</color></voffset></size>  <sprite name=");
			stringBuilder.Append(Functions.ClassIconFromString(Enum.GetName(typeof(Enums.HeroClass), _scd.HeroClassSecondary)));
			stringBuilder.Append(">");
		}
		if (_scd.HeroClassThird != Enums.HeroClass.None)
		{
			stringBuilder.Append("</size>");
			stringBuilder.Append("<size=2><voffset=.5><color=#444>|</color></voffset></size>  <sprite name=");
			stringBuilder.Append(Functions.ClassIconFromString(Enum.GetName(typeof(Enums.HeroClass), _scd.HeroClassThird)));
			stringBuilder.Append(">");
		}
		stringBuilder.Append("</size>");
		_Class.text = stringBuilder.ToString();
		stringBuilder.Clear();
		_Class.color = Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), _scd.HeroClass)]);
		stringBuilder.Append("<size=-.3>");
		stringBuilder.Append(Texts.Instance.GetText(_scd.Id + "_description", "class"));
		stringBuilder.Append("</size><br><br><color=#F1D2A9>");
		stringBuilder.Append(Texts.Instance.GetText(_scd.Id + "_strength", "class"));
		stringBuilder.Append("</color>");
		_Fluff.text = stringBuilder.ToString();
		if (_scd.Item == null)
		{
			cardItemT.gameObject.SetActive(value: false);
			cardItemCI.enabled = false;
			cardItemNoneT.gameObject.SetActive(value: true);
		}
		else
		{
			cardItemNoneT.gameObject.SetActive(value: false);
			cardItemT.gameObject.SetActive(value: true);
			cardItemCI.enabled = true;
			string id = _scd.Item.Id;
			switch (characterTier2)
			{
			case 1:
				id = Globals.Instance.GetCardData(id, instantiate: false).UpgradesTo1;
				break;
			case 2:
				id = Globals.Instance.GetCardData(id, instantiate: false).UpgradesTo2;
				break;
			}
			cardItemCI.SetCard(id);
			cardItemCI.cardoutsidecombat = true;
			cardItemCI.cardoutsideselection = true;
			cardItemCI.TopLayeringOrder("UI", 10000);
			cardItemT.transform.localScale = new Vector3(0.83f, 0.83f, 1f);
		}
		int num = _scd.Hp;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num += PlayerManager.Instance.GetPerkMaxHealth(_scd.Id);
		}
		_HP.text = num.ToString();
		stringBuilder.Clear();
		int num2 = _scd.Energy;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num2 += PlayerManager.Instance.GetPerkEnergyBegin(_scd.Id);
		}
		stringBuilder.Append(num2);
		stringBuilder.Append(" <size=1.3>");
		stringBuilder.Append(Texts.Instance.GetText("dataPerTurn").Replace("<%>", _scd.EnergyTurn.ToString()));
		stringBuilder.Append("</size>");
		_Energy.text = stringBuilder.ToString();
		int num3 = _scd.Speed;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num3 += PlayerManager.Instance.GetPerkSpeed(_scd.Id);
		}
		_Speed.text = num3.ToString();
		Color white = Color.white;
		Color color = new Color(0.665f, 0.665f, 0.665f);
		int num4 = 0;
		num4 = _scd.ResistSlashing;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Slashing);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Slashing.text = num4 + " %";
		}
		else
		{
			_Slashing.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Slashing.color = white;
		}
		else
		{
			_Slashing.color = color;
		}
		num4 = _scd.ResistBlunt;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Blunt);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Blunt.text = num4 + " %";
		}
		else
		{
			_Blunt.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Blunt.color = white;
		}
		else
		{
			_Blunt.color = color;
		}
		num4 = _scd.ResistPiercing;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Piercing);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Piercing.text = num4 + " %";
		}
		else
		{
			_Piercing.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Piercing.color = white;
		}
		else
		{
			_Piercing.color = color;
		}
		num4 = _scd.ResistFire;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Fire);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Fire.text = num4 + " %";
		}
		else
		{
			_Fire.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Fire.color = white;
		}
		else
		{
			_Fire.color = color;
		}
		num4 = _scd.ResistCold;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Cold);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Cold.text = num4 + " %";
		}
		else
		{
			_Cold.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Cold.color = white;
		}
		else
		{
			_Cold.color = color;
		}
		num4 = _scd.ResistLightning;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Lightning);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Lightning.text = num4 + " %";
		}
		else
		{
			_Lightning.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Lightning.color = white;
		}
		else
		{
			_Lightning.color = color;
		}
		num4 = _scd.ResistMind;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Mind);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Mind.text = num4 + " %";
		}
		else
		{
			_Mind.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Mind.color = white;
		}
		else
		{
			_Mind.color = color;
		}
		num4 = _scd.ResistHoly;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Holy);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Holy.text = num4 + " %";
		}
		else
		{
			_Holy.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Holy.color = white;
		}
		else
		{
			_Holy.color = color;
		}
		num4 = _scd.ResistShadow;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num4 += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, Enums.DamageType.Shadow);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			_Shadow.text = num4 + " %";
		}
		else
		{
			_Shadow.text = num4 + "%";
		}
		if (num4 > 0)
		{
			_Shadow.color = white;
		}
		else
		{
			_Shadow.color = color;
		}
		_Trait0RO.SetTrait(_scd.Trait0.Id, characterTier);
		_Trait1ARO.SetTrait(_scd.Trait1A.Id, characterTier);
		_Trait1BRO.SetTrait(_scd.Trait1B.Id, characterTier);
		_Trait2ARO.SetTrait(_scd.Trait2A.Id, characterTier);
		_Trait2BRO.SetTrait(_scd.Trait2B.Id, characterTier);
		_Trait3ARO.SetTrait(_scd.Trait3A.Id, characterTier);
		_Trait3BRO.SetTrait(_scd.Trait3B.Id, characterTier);
		_Trait4ARO.SetTrait(_scd.Trait4A.Id, characterTier);
		_Trait4BRO.SetTrait(_scd.Trait4B.Id, characterTier);
		for (int i = 0; i < 7; i++)
		{
			if (i < _scd.Cards.Length && _scd.Cards[i] != null)
			{
				string id2 = _scd.Cards[i].Card.Id;
				if (_scd.Cards[i].Card.Starter)
				{
					switch (characterTier3)
					{
					case 1:
						id2 = Globals.Instance.GetCardData(id2, instantiate: false).UpgradesTo1.ToLower();
						break;
					case 2:
						id2 = Globals.Instance.GetCardData(id2, instantiate: false).UpgradesTo2.ToLower();
						break;
					}
				}
				cardsCI[i].SetCard(id2, deckScale: false);
				cardsNumText[i].text = _scd.Cards[i].UnitsInDeck.ToString();
				cardsT[i].gameObject.SetActive(value: true);
				cardsNumT[i].gameObject.SetActive(value: true);
			}
			else
			{
				cardsT[i].gameObject.SetActive(value: false);
				cardsNumT[i].gameObject.SetActive(value: false);
			}
		}
		if (oldSCD != SCD)
		{
			oldSCD = SCD;
			SetHeroAnimated();
		}
		SetButtonPerkPoints();
		if (!showNothing)
		{
			if (doStats)
			{
				ShowStats();
			}
			else
			{
				ShowPerks();
			}
		}
		if (!_scd.Sku.IsNullOrEmpty())
		{
			dlcCharacter.gameObject.SetActive(value: true);
			dlcCharacter.GetComponent<PopupText>().text = string.Format(Texts.Instance.GetText("requiredDLC").Replace("#FFF", "#CEA843"), SteamManager.Instance.GetDLCName(_scd.Sku));
		}
		else
		{
			dlcCharacter.gameObject.SetActive(value: false);
		}
		if (!GameManager.Instance.IsSingularity())
		{
			initialCards.gameObject.SetActive(value: true);
			checkSingularityCards.gameObject.SetActive(value: false);
		}
		else
		{
			initialCards.gameObject.SetActive(value: false);
			checkSingularityCards.gameObject.SetActive(value: true);
		}
	}

	private void SetHeroAnimated(GameObject heroSkin = null)
	{
		if (heroAnimated != null && heroSkin != null && heroAnimated.name == heroSkin.name)
		{
			return;
		}
		float num = 0f;
		if (heroAnimated != null)
		{
			float normalizedTime = heroAnimated.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
			num = normalizedTime - Mathf.Floor(normalizedTime);
		}
		UnityEngine.Object.Destroy(heroAnimated);
		GameObject gameObject = heroSkin;
		float num2 = 1f;
		float x = 0f;
		string activeSkin = PlayerManager.Instance.GetActiveSkin(SCD.Id);
		SkinData skinData = Globals.Instance.GetSkinData(activeSkin);
		if (heroSkin == null)
		{
			if (skinData != null)
			{
				num2 = skinData.HeroSelectionScreenScale;
				gameObject = skinData.SkinGo;
			}
			else
			{
				gameObject = SCD.GameObjectAnimated;
			}
		}
		else if (skinData != null)
		{
			num2 = skinData.HeroSelectionScreenScale;
		}
		if (gameObject != null)
		{
			_SpriteSubstitution.transform.gameObject.SetActive(value: false);
			Vector3 vector = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
			heroAnimated = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity, _HeroParent);
			_HeroParent.transform.localScale = new Vector3(num2, num2, num2);
			heroAnimated.transform.localPosition += new Vector3(x, 0f, 0f);
			heroAnimated.name = gameObject.name;
			heroAnimated.GetComponent<BoxCollider2D>().enabled = false;
			heroAnim = heroAnimated.GetComponent<Animator>();
			heroAnim.enabled = false;
			heroAnimated.transform.localPosition = Vector3.zero;
			if (SCD.SubClassName.ToLower() == "alchemist")
			{
				heroAnimated.transform.localPosition = new Vector3(0.2f, 0.1f, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "cleric")
			{
				heroAnimated.transform.localPosition = new Vector3(0f, -0.17f, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "engineer")
			{
				float num3 = 0.95f;
				float y = -0.1f;
				if (gameObject.TryGetComponent<UICharacterScale>(out var component))
				{
					num3 = component.uiCharacterScale;
					y = 0f;
				}
				heroAnimated.transform.localScale = new Vector3(num3, num3, 1f);
				heroAnimated.transform.localPosition = new Vector3(-0.125f, y, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "minstrel")
			{
				heroAnimated.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "paladin")
			{
				heroAnimated.transform.localPosition = new Vector3(0f, -0.17f, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "priest")
			{
				heroAnimated.transform.localPosition = new Vector3(0.15f, -0.15f, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "prophet")
			{
				heroAnimated.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else if (SCD.SubClassName.ToLower() == "queen" || SCD.SubClassName.ToLower() == "valkyrie")
			{
				heroAnimated.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
				heroAnimated.transform.localPosition = new Vector3(0.15f, -0.45f, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "voodoowitch")
			{
				heroAnimated.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
				heroAnimated.transform.localPosition = new Vector3(0f, -0.1f, 0f);
			}
			else if (SCD.SubClassName.ToLower() == "deathknight")
			{
				heroAnimated.transform.localPosition = new Vector3(-0.38f, 0f, 0f);
				heroAnimated.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			}
			else if (SCD.HeroClass == Enums.HeroClass.Warrior)
			{
				heroAnimated.transform.localPosition = new Vector3(0f, -0.15f, 0f);
				if (SCD.SubClassName.ToLower() == "bandit")
				{
					heroAnimated.transform.localScale = new Vector3(0.93f, 0.93f, 1f);
				}
				else if (SCD.SubClassName.ToLower() == "sentinel")
				{
					heroAnimated.transform.localScale = new Vector3(0.96f, 0.96f, 1f);
				}
				else if (SCD.SubClassName.ToLower() == "warden")
				{
					heroAnimated.transform.localScale = new Vector3(0.92f, 0.92f, 1f);
				}
			}
			heroAnimated.transform.localPosition += vector;
			animatedSprites = new List<SpriteRenderer>();
			animatedSpritesOutOfCharacter = new List<SetSpriteLayerFromBase>();
			GetSpritesFromAnimated(heroAnimated);
			int num4 = 2000;
			if (animatedSprites != null)
			{
				for (int i = 0; i < animatedSprites.Count; i++)
				{
					if (animatedSprites[i] != null)
					{
						animatedSprites[i].sortingOrder = num4 - i;
						animatedSprites[i].sortingLayerName = "UI";
					}
				}
			}
			if (animatedSpritesOutOfCharacter != null)
			{
				for (int j = 0; j < animatedSpritesOutOfCharacter.Count; j++)
				{
					if (animatedSpritesOutOfCharacter[j] != null)
					{
						animatedSpritesOutOfCharacter[j].ReOrderLayer();
					}
				}
			}
			if (num > 0f)
			{
				heroAnim.enabled = true;
				heroAnim.Play("Base Layer." + heroAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name, -1, num);
			}
			else
			{
				heroAnim.enabled = true;
			}
		}
		else
		{
			_SpriteSubstitution.gameObject.SetActive(value: true);
			_SpriteSubstitution.sprite = SCD.Sprite;
		}
	}

	public void GetSpritesFromAnimated(GameObject GO)
	{
		foreach (Transform item in GO.transform)
		{
			if ((bool)item.GetComponent<SpriteRenderer>())
			{
				animatedSprites.Add(item.GetComponent<SpriteRenderer>());
			}
			if (item.GetComponent<SetSpriteLayerFromBase>() != null)
			{
				animatedSpritesOutOfCharacter.Add(item.GetComponent<SetSpriteLayerFromBase>());
			}
			if (item.childCount > 0)
			{
				GetSpritesFromAnimated(item.gameObject);
			}
		}
	}

	public void SetButtonPerkPoints()
	{
		if (GameManager.Instance.IsLoadingGame() || GameManager.Instance.IsObeliskChallenge())
		{
			warningPerkT.gameObject.SetActive(value: false);
			return;
		}
		int perkPointsAvailable = PlayerManager.Instance.GetPerkPointsAvailable(SCD.Id);
		if (perkPointsAvailable > 0)
		{
			warningPerk.text = perkPointsAvailable.ToString();
			warningPerkT.gameObject.SetActive(value: true);
		}
		else
		{
			warningPerkT.gameObject.SetActive(value: false);
		}
	}

	public void RepositionResolution()
	{
		SetPositions();
		if (opened)
		{
			movableElements.position = destinationCenter;
		}
		else
		{
			movableElements.position = destinationOut;
		}
	}

	public void SetPositions()
	{
		destinationCenter = new Vector3(Globals.Instance.sizeW * 0.5f - 9.7f, -0.5f, -1f);
		destinationOut = new Vector3(Globals.Instance.sizeW + 2.5f * Globals.Instance.scale, -0.5f, -1f);
	}

	public void HideNow()
	{
		movableElements.position = new Vector3(movableElements.position.x, movableElements.position.y, -100f);
	}

	public void Close()
	{
		if (opened)
		{
			SetPositions();
			activeId = "";
			moveThis = false;
			destination = destinationOut;
			closeThis = true;
			moveThis = true;
			opened = false;
			HeroSelectionManager.Instance.ShowMask(state: false);
			EnableCharButton(buttonStats, state: true);
			EnableCharButton(buttonSkins, state: true);
			EnableCharButton(buttonCardback, state: true);
			EnableCharButton(buttonPerks, state: true);
			EnableCharButton(buttonRank, state: true);
			EnableCharButton(buttonSingularityCards, state: true, isSingularityButton: true);
			if (HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].blocked || HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].DlcBlocked)
			{
				DisableBlockedElements();
			}
			if (GameManager.Instance.IsObeliskChallenge())
			{
				buttonPerks.Disable();
				buttonRank.Disable();
			}
			HeroSelectionManager.Instance?.charPopupMini.SetSubClassData(SCD);
		}
	}

	public void AllowAllButtons()
	{
		EnableCharButton(buttonStats, state: true);
		EnableCharButton(buttonSkins, state: true);
		EnableCharButton(buttonCardback, state: true);
		EnableCharButton(buttonPerks, state: true);
		EnableCharButton(buttonRank, state: true);
		EnableCharButton(buttonSingularityCards, state: true, isSingularityButton: true);
		if (HeroSelectionManager.Instance.heroSelectionDictionary.ContainsKey(SCD.Id) && (HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].blocked || HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].DlcBlocked))
		{
			DisableBlockedElements();
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			buttonPerks.Disable();
			buttonRank.Disable();
		}
	}

	public void Show()
	{
		if (!opened)
		{
			closeThis = false;
			SetPositions();
			destination = destinationCenter;
			if (!moveThis)
			{
				movableElements.position = new Vector3(movableElements.position.x, movableElements.position.y, -1f);
				StartCoroutine(moveWindow());
			}
			opened = true;
			controllerHorizontalIndex = -1;
			HeroSelectionManager.Instance.ShowMask(state: true);
		}
	}

	private IEnumerator moveWindow()
	{
		yield return null;
		moveThis = true;
	}

	private void EnableCharButton(BotonGeneric bot, bool state, bool isSingularityButton = false)
	{
		if (state)
		{
			bot.Enable();
			bot.transform.localPosition = new Vector3(-0.4f, bot.transform.localPosition.y, bot.transform.localPosition.z);
			bot.SetBackgroundColor(botColorEnabled);
		}
		else
		{
			bot.Disable();
			bot.transform.localPosition = new Vector3(-0.5f, bot.transform.localPosition.y, bot.transform.localPosition.z);
			bot.SetBackgroundColor(botColorDisabled);
			bot.ShowBackgroundDisable(state: false);
		}
	}

	public void ShowStats()
	{
		EnableCharButton(buttonStats, state: false);
		EnableCharButton(buttonSkins, state: true);
		EnableCharButton(buttonCardback, state: true);
		EnableCharButton(buttonPerks, state: true);
		EnableCharButton(buttonRank, state: true);
		EnableCharButton(buttonSingularityCards, state: true, isSingularityButton: true);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			buttonPerks.Disable();
			buttonRank.Disable();
		}
		groupCharacter.gameObject.SetActive(value: true);
		groupStats.gameObject.SetActive(value: true);
		groupPerks.gameObject.SetActive(value: false);
		groupSkins.gameObject.SetActive(value: false);
		groupRank.gameObject.SetActive(value: false);
		groupCardback.gameObject.SetActive(value: false);
		groupSingularityCards.gameObject.SetActive(value: false);
		perksNotAvailable.gameObject.SetActive(value: false);
		if (HeroSelectionManager.Instance.heroSelectionDictionary != null && HeroSelectionManager.Instance.heroSelectionDictionary.ContainsKey(SCD.Id) && (HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].blocked || HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].DlcBlocked))
		{
			DisableBlockedElements();
		}
	}

	private void DisableBlockedElements()
	{
		buttonPerks.Disable();
		buttonCardback.Disable();
		buttonRank.Disable();
		warningPerkT.gameObject.SetActive(value: false);
	}

	public void ShowSkins()
	{
		EnableCharButton(buttonStats, state: true);
		EnableCharButton(buttonSkins, state: false);
		EnableCharButton(buttonCardback, state: true);
		EnableCharButton(buttonPerks, state: true);
		EnableCharButton(buttonRank, state: true);
		EnableCharButton(buttonSingularityCards, state: true, isSingularityButton: true);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			buttonPerks.Disable();
			buttonRank.Disable();
		}
		groupCharacter.gameObject.SetActive(value: true);
		groupStats.gameObject.SetActive(value: false);
		groupPerks.gameObject.SetActive(value: false);
		groupSkins.gameObject.SetActive(value: true);
		groupRank.gameObject.SetActive(value: false);
		groupCardback.gameObject.SetActive(value: false);
		groupSingularityCards.gameObject.SetActive(value: false);
		perksNotAvailable.gameObject.SetActive(value: false);
		DoSkins();
		ShowSkin();
		if (HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].blocked || HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].DlcBlocked)
		{
			DisableBlockedElements();
		}
	}

	public void ShowPerks()
	{
		DoPerks();
	}

	public void ShowRank()
	{
		EnableCharButton(buttonStats, state: true);
		EnableCharButton(buttonPerks, state: true);
		EnableCharButton(buttonSkins, state: true);
		EnableCharButton(buttonRank, state: false);
		EnableCharButton(buttonCardback, state: true);
		EnableCharButton(buttonSingularityCards, state: true, isSingularityButton: true);
		groupCharacter.gameObject.SetActive(value: true);
		groupStats.gameObject.SetActive(value: false);
		groupPerks.gameObject.SetActive(value: false);
		groupSkins.gameObject.SetActive(value: false);
		groupRank.gameObject.SetActive(value: true);
		groupCardback.gameObject.SetActive(value: false);
		groupSingularityCards.gameObject.SetActive(value: false);
		DoRank();
		if (GameManager.Instance.IsObeliskChallenge())
		{
			perksNotAvailable.gameObject.SetActive(value: true);
		}
		else
		{
			perksNotAvailable.gameObject.SetActive(value: false);
		}
	}

	public void ShowSingularityCards()
	{
		EnableCharButton(buttonStats, state: true);
		EnableCharButton(buttonSkins, state: true);
		EnableCharButton(buttonCardback, state: true);
		EnableCharButton(buttonPerks, state: true);
		EnableCharButton(buttonRank, state: true);
		EnableCharButton(buttonSingularityCards, state: false);
		if (HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].blocked || HeroSelectionManager.Instance.heroSelectionDictionary[SCD.Id].DlcBlocked)
		{
			DisableBlockedElements();
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			buttonPerks.Disable();
			buttonRank.Disable();
		}
		groupCharacter.gameObject.SetActive(value: false);
		groupStats.gameObject.SetActive(value: false);
		groupPerks.gameObject.SetActive(value: false);
		groupSkins.gameObject.SetActive(value: false);
		groupRank.gameObject.SetActive(value: false);
		groupCardback.gameObject.SetActive(value: false);
		groupSingularityCards.gameObject.SetActive(value: true);
		List<string> list = new List<string>();
		for (int i = 0; i < 15; i++)
		{
			if (i < SCD.CardsSingularity.Length && SCD.CardsSingularity[i] != null)
			{
				list.Add(SCD.CardsSingularity[i].Id);
			}
		}
		list.Sort();
		int count = list.Count;
		for (int j = 0; j < 15; j++)
		{
			if (j < count)
			{
				if (cardsSING[j] != null)
				{
					if (!cardsSING[j].gameObject.activeSelf)
					{
						cardsSING[j].gameObject.SetActive(value: true);
					}
					cardsSING[j].SetCard(list[j], deckScale: false);
				}
			}
			else if (cardsSING[j] != null && cardsSING[j].gameObject.activeSelf)
			{
				cardsSING[j].gameObject.SetActive(value: false);
			}
		}
	}

	public void ShowCardbacks()
	{
		if (!opened)
		{
			ShowStats();
		}
		HeroSelectionManager.Instance.CardBacksPopUp.SetActive(value: true);
		DoCardbacks();
	}

	public void DoCardbacks(bool RefreshPage = false)
	{
		CardBackSelectionPanel component = HeroSelectionManager.Instance.CardBacksPopUp.GetComponent<CardBackSelectionPanel>();
		CardBackSelectionPanel.CardStartingPositions[] cardStartingPositions = component.cardStartingPositions;
		for (int i = 0; i < cardStartingPositions.Length; i++)
		{
			CardBackSelectionPanel.CardStartingPositions cardStartingPositions2 = cardStartingPositions[i];
			for (int num = cardStartingPositions2.refTransform.childCount - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(cardStartingPositions2.refTransform.GetChild(num).gameObject);
			}
		}
		string id = SCD.Id;
		int perkRank = PlayerManager.Instance.GetPerkRank(SCD.Id);
		SortedDictionary<string, CardbackData> sortedDictionary = new SortedDictionary<string, CardbackData>();
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, CardbackData> item in Globals.Instance.CardbackDataSource)
		{
			bool flag = false;
			bool flag2 = false;
			if (item.Value.CardbackSubclass == null || item.Value.CardbackSubclass.Id == SCD.Id)
			{
				flag = AtOManager.Instance.ValidCardback(item.Value, perkRank);
				if (!flag && item.Value.RankLevel > 0)
				{
					flag2 = true;
				}
			}
			if (flag || item.Value.ShowIfLocked)
			{
				flag2 = true;
			}
			if (flag2)
			{
				string text = "";
				text = ((item.Value.CardbackOrder < 10) ? ("000" + item.Value.CardbackOrder) : ((item.Value.CardbackOrder < 100) ? ("00" + item.Value.CardbackOrder) : ((item.Value.CardbackOrder >= 1000) ? item.Value.CardbackOrder.ToString() : ("0" + item.Value.CardbackOrder))));
				text = ((!item.Value.BaseCardback) ? ("1" + text) : ("0" + text));
				text = text + "_" + item.Value.CardbackName;
				sortedDictionary.Add(text, item.Value);
				if (flag)
				{
					list.Add(text);
				}
			}
		}
		string activeCardback = PlayerManager.Instance.GetActiveCardback(id);
		if (activeCardback != "")
		{
			CardbackData cardbackData = Globals.Instance.GetCardbackData(activeCardback);
			if (cardbackData == null)
			{
				cardbackData = Globals.Instance.GetCardbackData(Globals.Instance.GetCardbackBaseIdBySubclass(id));
				if (cardbackData != null)
				{
					PlayerManager.Instance.SetCardback(id, cardbackData.CardbackId);
				}
			}
		}
		float cardSize = component.cardSize;
		int cardPerRow = component.cardPerRow;
		float distX = component.distX;
		float distY = component.distY;
		Dictionary<string, List<KeyValuePair<string, CardbackData>>> dictionary = new Dictionary<string, List<KeyValuePair<string, CardbackData>>>
		{
			{
				"character",
				new List<KeyValuePair<string, CardbackData>>()
			},
			{
				"general",
				new List<KeyValuePair<string, CardbackData>>()
			},
			{
				"others",
				new List<KeyValuePair<string, CardbackData>>()
			}
		};
		foreach (KeyValuePair<string, CardbackData> item2 in sortedDictionary)
		{
			int num2 = int.Parse(item2.Key.Split('_')[0]);
			if (num2 < 10010)
			{
				dictionary["character"].Add(item2);
			}
			else if (num2 < 10050)
			{
				dictionary["general"].Add(item2);
			}
			else
			{
				dictionary["others"].Add(item2);
			}
		}
		int num3 = 3;
		int num4 = cardPerRow * num3;
		foreach (KeyValuePair<string, List<KeyValuePair<string, CardbackData>>> item3 in dictionary)
		{
			string key = item3.Key;
			List<KeyValuePair<string, CardbackData>> value = item3.Value;
			Transform startingRefTransform = component.GetStartingRefTransform(key);
			for (int num5 = startingRefTransform.childCount - 1; num5 >= 0; num5--)
			{
				UnityEngine.Object.Destroy(startingRefTransform.GetChild(num5).gameObject);
			}
			int count = value.Count;
			int num6 = Mathf.CeilToInt((float)count / (float)num4);
			int num7 = 0;
			for (int j = 0; j < num6; j++)
			{
				GameObject gameObject = new GameObject($"Page_{j + 1}");
				gameObject.transform.SetParent(startingRefTransform, worldPositionStays: false);
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.localPosition = Vector3.zero;
				rectTransform.localScale = Vector3.one;
				gameObject.SetActive(j == 0);
				for (int k = 0; k < num3; k++)
				{
					for (int l = 0; l < cardPerRow; l++)
					{
						if (num7 >= count)
						{
							break;
						}
						KeyValuePair<string, CardbackData> keyValuePair = value[num7++];
						GameObject obj = UnityEngine.Object.Instantiate(goCardback, Vector3.zero, Quaternion.identity, gameObject.transform);
						Vector3 localPosition = new Vector3(distX * (float)l, (0f - distY) * (float)k, -1f);
						obj.transform.localScale = new Vector3(cardSize, cardSize, 1f);
						obj.transform.localPosition = localPosition;
						bool unlocked = list.Contains(keyValuePair.Key);
						obj.GetComponent<BotonCardback>().SetCardbackData(keyValuePair.Value.CardbackId, unlocked, id);
					}
				}
			}
			component.UpdateButtonText(key);
		}
	}

	public void DoRank()
	{
		string key = Enum.GetName(typeof(Enums.HeroClass), SCD.HeroClass);
		string id = SCD.Id;
		string hex = Globals.Instance.ClassColor[key];
		SpriteRenderer spriteRenderer = perkBar;
		TMP_Text tMP_Text = maxProgress;
		Color color = (rankProgress.color = Functions.HexToColor(hex));
		Color color3 = (tMP_Text.color = color);
		spriteRenderer.color = color3;
		int progress = PlayerManager.Instance.GetProgress(id);
		int perkPrevLevelPoints = PlayerManager.Instance.GetPerkPrevLevelPoints(id);
		int perkNextLevelPoints = PlayerManager.Instance.GetPerkNextLevelPoints(id);
		maxProgress.text = "<color=#FFF>" + progress + "</color><size=-.5> / " + perkNextLevelPoints;
		int perkRank = PlayerManager.Instance.GetPerkRank(id);
		rankProgress.text = string.Format(Texts.Instance.GetText("rankProgress"), perkRank);
		float x = ((float)progress - (float)perkPrevLevelPoints) / ((float)perkNextLevelPoints - (float)perkPrevLevelPoints) * 3.365f;
		perkBarMask.localScale = new Vector3(x, perkBarMask.localScale.y, perkBarMask.localScale.z);
		for (int i = 0; i < progressionRows.Length; i++)
		{
			progressionRows[i].Init(i);
		}
		int num = 0;
		for (int j = 0; j < Globals.Instance.RankLevel.Count && Globals.Instance.RankLevel[j] <= perkRank; j++)
		{
			num++;
		}
		for (int k = 0; k < num; k++)
		{
			progressionRows[k].Enable(_state: true);
		}
		DoSuppliesBlock();
	}

	public void DoSuppliesBlock()
	{
		int playerSupplyActual = PlayerManager.Instance.GetPlayerSupplyActual();
		useSuppliesAvailable.text = string.Format(Texts.Instance.GetText("useSuppliesAvailable"), playerSupplyActual + "  ");
		useSuppliesButton.auxString = SCD.Id;
		if (!AtOManager.Instance.IsCombatTool)
		{
			if (playerSupplyActual < 1 || PlayerManager.Instance.GetPerkRank(SCD.Id) < 10 || PlayerManager.Instance.GetPerkRank(SCD.Id) >= 50 || PlayerManager.Instance.TotalPointsSpentInSupplys() < 276)
			{
				useSuppliesButton.Disable();
			}
			else
			{
				useSuppliesButton.Enable();
				useSupplyDisclaimer.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (playerSupplyActual >= 1 && PlayerManager.Instance.GetPerkRank(SCD.Id) < 50)
			{
				useSuppliesButton.Enable();
			}
			useSupplyDisclaimer.gameObject.SetActive(value: false);
		}
		if (GameManager.Instance.GetDeveloperMode() || (GameManager.Instance.CheatMode && GameManager.Instance.EnableButtons))
		{
			useSuppliesButton.Enable();
		}
	}

	private void DoPerks()
	{
		Enum.GetName(typeof(Enums.HeroClass), SCD.HeroClass);
		if (!opened)
		{
			ShowStats();
		}
		PerkTree.Instance.Show(SCD.Id);
	}

	public void DoSkins()
	{
		List<SkinData> skinsBySubclass = Globals.Instance.GetSkinsBySubclass(SCD.Id);
		for (int i = 0; i < botonSkinBase.Length; i++)
		{
			botonSkinBase[i].gameObject.SetActive(value: false);
		}
		StringBuilder stringBuilder = new StringBuilder();
		SortedDictionary<string, SkinData> sortedDictionary = new SortedDictionary<string, SkinData>();
		for (int j = 0; j < skinsBySubclass.Count; j++)
		{
			stringBuilder.Clear();
			if (skinsBySubclass[j].BaseSkin)
			{
				stringBuilder.Append("0");
			}
			else
			{
				stringBuilder.Append("1");
			}
			stringBuilder.Append(skinsBySubclass[j].SkinOrder.ToString("D2"));
			stringBuilder.Append(skinsBySubclass[j].SkinId.ToLower());
			sortedDictionary.Add(stringBuilder.ToString(), skinsBySubclass[j]);
		}
		int num = 0;
		foreach (KeyValuePair<string, SkinData> item in sortedDictionary)
		{
			botonSkinBase[num].SetSkinData(item.Value);
			botonSkinBase[num].gameObject.SetActive(value: true);
			num++;
		}
	}

	public void ShowSkin(GameObject _skin = null)
	{
		if (_skin == null)
		{
			string text = "";
			if (SCD != null)
			{
				text = PlayerManager.Instance.GetActiveSkin(SCD.Id);
			}
			if (!(text == ""))
			{
				SkinData skinData = Globals.Instance.GetSkinData(text);
				if (!(skinData == null))
				{
					SetHeroAnimated(skinData.SkinGo);
				}
			}
		}
		else
		{
			SetHeroAnimated(_skin);
		}
	}

	public void ResetSkin()
	{
		ShowSkin();
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		_controllerList.Add(buttonStats.transform);
		_controllerList.Add(buttonRank.transform);
		_controllerList.Add(buttonPerks.transform);
		_controllerList.Add(buttonSkins.transform);
		_controllerList.Add(buttonCardback.transform);
		if (Functions.TransformIsVisible(groupStats))
		{
			_controllerList.Add(_Trait0.transform);
			_controllerList.Add(_Trait1A.transform);
			_controllerList.Add(_Trait1B.transform);
			_controllerList.Add(_Trait2A.transform);
			_controllerList.Add(_Trait2B.transform);
			_controllerList.Add(_Trait3A.transform);
			_controllerList.Add(_Trait3B.transform);
			_controllerList.Add(_Trait4A.transform);
			_controllerList.Add(_Trait4B.transform);
			_controllerList.Add(cardItemT);
			foreach (Transform item in _CardParent)
			{
				if ((bool)item.GetComponent<CardItem>() && Functions.TransformIsVisible(item))
				{
					_controllerList.Add(item);
				}
			}
		}
		else if (Functions.TransformIsVisible(groupRank))
		{
			if (Functions.TransformIsVisible(useSuppliesButton.transform))
			{
				_controllerList.Add(useSuppliesButton.transform);
			}
		}
		else if (Functions.TransformIsVisible(groupSkins))
		{
			for (int i = 0; i < botonSkinBase.Length; i++)
			{
				if (Functions.TransformIsVisible(botonSkinBase[i].transform))
				{
					_controllerList.Add(botonSkinBase[i].transform);
				}
			}
		}
		else if (Functions.TransformIsVisible(groupCardback))
		{
			foreach (Transform item2 in groupCardbackGOsCharacter)
			{
				if ((bool)item2.GetComponent<BotonCardback>() && Functions.TransformIsVisible(item2))
				{
					_controllerList.Add(item2);
				}
			}
			foreach (Transform item3 in groupCardbackGOsGeneral)
			{
				if ((bool)item3.GetComponent<BotonCardback>() && Functions.TransformIsVisible(item3))
				{
					_controllerList.Add(item3);
				}
			}
			foreach (Transform item4 in groupCardbackGOsMisc)
			{
				if ((bool)item4.GetComponent<BotonCardback>() && Functions.TransformIsVisible(item4))
				{
					_controllerList.Add(item4);
				}
			}
		}
		_controllerList.Add(buttonClose.transform);
		if (controllerHorizontalIndex <= -1)
		{
			controllerHorizontalIndex = 0;
		}
		else
		{
			if (controllerHorizontalIndex > _controllerList.Count - 1)
			{
				controllerHorizontalIndex = _controllerList.Count - 1;
			}
			Vector3 position = _controllerList[controllerHorizontalIndex].position;
			bool flag = false;
			float num = 0f;
			int num2 = 0;
			int num3 = controllerHorizontalIndex;
			while (!flag && num2 < 20)
			{
				num = (float)(num2 + 1) * 0.5f;
				controllerHorizontalIndex = num3;
				if (goingDown)
				{
					controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(0f, 0f - num, 0f));
					if (controllerHorizontalIndex > -1 && position.y - _controllerList[controllerHorizontalIndex].position.y > 0.25f)
					{
						flag = true;
					}
				}
				else if (goingUp)
				{
					controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(0f, num, 0f));
					if (controllerHorizontalIndex > -1 && _controllerList[controllerHorizontalIndex].position.y - position.y > 0.25f)
					{
						flag = true;
					}
				}
				else if (goingRight)
				{
					if (controllerHorizontalIndex < 5 && Functions.TransformIsVisible(groupStats))
					{
						controllerHorizontalIndex = 5;
						flag = true;
					}
					else if (controllerHorizontalIndex < 5 && Functions.TransformIsVisible(groupRank))
					{
						controllerHorizontalIndex = 5;
						flag = true;
					}
					else
					{
						controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(num, 0f, 0f));
						if (controllerHorizontalIndex > -1 && _controllerList[controllerHorizontalIndex].position.x - position.x > 0.5f)
						{
							flag = true;
						}
					}
				}
				else if (goingLeft)
				{
					controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(0f - num, 0f, 0f));
					if (controllerHorizontalIndex > -1 && position.x - _controllerList[controllerHorizontalIndex].position.x > 0.5f)
					{
						flag = true;
					}
					else if (num2 > 15)
					{
						controllerHorizontalIndex = 0;
						flag = true;
					}
				}
				if (!flag)
				{
					num2++;
				}
			}
			if (!flag)
			{
				controllerHorizontalIndex = num3;
			}
		}
		if (controllerHorizontalIndex > _controllerList.Count - 1)
		{
			controllerHorizontalIndex = _controllerList.Count - 1;
		}
		else if (controllerHorizontalIndex < 0)
		{
			controllerHorizontalIndex = 0;
		}
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
