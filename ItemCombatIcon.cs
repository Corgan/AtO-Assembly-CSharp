using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCombatIcon : MonoBehaviour
{
	public string itemType;

	public Sprite[] spriteBackgroundList;

	public Sprite[] spriteBackgroundHoverList;

	public SpriteRenderer spriteBackground;

	public SpriteRenderer spriteBackgroundHover;

	public SpriteRenderer spriteBackgroundHoverBorder;

	public SpriteRenderer spriteBorder;

	public Transform rareParticles;

	public TMP_Text timesExecuted;

	public Transform animatedUse;

	public TMP_Text animatedUseText;

	private Transform iconOff;

	private Transform iconOn;

	private SpriteRenderer iconSpr;

	private Vector3 vectorDestination;

	private float movementSpeed = 0.2f;

	private CardData cardData;

	private CardItem CI;

	private BoxCollider2D collider;

	private GameObject card;

	private Coroutine showCardCo;

	private bool mouseIsOver;

	private Hero theHero;

	private NPC theNPC;

	public Hero TheHero
	{
		get
		{
			return theHero;
		}
		set
		{
			theHero = value;
		}
	}

	public NPC TheNPC
	{
		get
		{
			return theNPC;
		}
		set
		{
			theNPC = value;
		}
	}

	private void Awake()
	{
		iconOn = base.transform.GetChild(0).transform;
		iconOff = base.transform.GetChild(1).transform;
		iconSpr = iconOn.GetChild(0).transform.GetComponent<SpriteRenderer>();
		CI = GetComponent<CardItem>();
		collider = GetComponent<BoxCollider2D>();
		animatedUse.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		if (MatchManager.Instance != null && base.transform.parent.gameObject.name != "TomeItemIcons")
		{
			string text = base.gameObject.name.ToLower();
			if (text != "corruptionicon" && text != "enchantment" && text != "enchantment2" && text != "enchantment3")
			{
				float y = (Globals.Instance.sizeH * 0.5f - 4.5f) * -2f;
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, y, base.transform.localPosition.z);
			}
		}
		SetBackground();
	}

	public void SetActivated()
	{
		StartCoroutine(SetActivatedCo());
	}

	private IEnumerator SetActivatedCo()
	{
		GameManager.Instance.PlayLibraryAudio("ui_item_usedcharge");
		animatedUse.gameObject.SetActive(value: false);
		animatedUse.gameObject.SetActive(value: true);
		animatedUseText.text = Texts.Instance.GetText("itemActivated");
		yield return null;
	}

	public void SetTimesExecuted(int times, bool doAnim = true)
	{
		if (animatedUse == null)
		{
			return;
		}
		animatedUse.gameObject.SetActive(value: false);
		if (times < 0)
		{
			times = 0;
		}
		if (!(cardData != null))
		{
			return;
		}
		int num = 0;
		if (cardData.Item != null)
		{
			num = ((cardData.Item.TimesPerCombat <= 0) ? cardData.Item.TimesPerTurn : cardData.Item.TimesPerCombat);
		}
		else if (cardData.ItemEnchantment != null && MatchManager.Instance != null && cardData.ItemEnchantment.DestroyAfterUses > 0)
		{
			num = cardData.ItemEnchantment.DestroyAfterUses;
			if (theHero != null)
			{
				times = MatchManager.Instance.EnchantmentExecutedTimes(theHero.Id, cardData.ItemEnchantment.Id);
			}
			else if (theNPC != null)
			{
				times = MatchManager.Instance.EnchantmentExecutedTimes(theNPC.Id, cardData.ItemEnchantment.Id);
			}
		}
		if (num == 0)
		{
			if (timesExecuted.gameObject.activeSelf)
			{
				timesExecuted.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!timesExecuted.gameObject.activeSelf)
		{
			timesExecuted.gameObject.SetActive(value: true);
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (times == num)
		{
			stringBuilder.Append("<color=#F3404E>");
			stringBuilder.Append(0);
			stringBuilder.Append("</color>");
		}
		else if (times > 0 && times < num)
		{
			stringBuilder.Append("<color=#FFFFFF>");
			stringBuilder.Append(num - times);
			stringBuilder.Append("</color>");
		}
		else
		{
			stringBuilder.Append(num);
		}
		stringBuilder.Append("/");
		stringBuilder.Append(num);
		timesExecuted.text = stringBuilder.ToString();
		if (times != 0 && doAnim && !animatedUse.gameObject.activeSelf)
		{
			animatedUse.gameObject.SetActive(value: true);
		}
		stringBuilder = null;
	}

	private void SetBackground()
	{
		if (itemType == "weapon")
		{
			spriteBackground.sprite = spriteBackgroundList[0];
			spriteBackgroundHover.sprite = spriteBackgroundList[0];
		}
		else if (itemType == "armor")
		{
			spriteBackground.sprite = spriteBackgroundList[1];
			spriteBackgroundHover.sprite = spriteBackgroundList[1];
		}
		else if (itemType == "jewelry")
		{
			spriteBackground.sprite = spriteBackgroundList[2];
			spriteBackgroundHover.sprite = spriteBackgroundList[2];
		}
		else if (itemType == "accesory")
		{
			spriteBackground.sprite = spriteBackgroundList[3];
			spriteBackgroundHover.sprite = spriteBackgroundList[3];
		}
		else if (itemType == "pet")
		{
			spriteBackground.sprite = spriteBackgroundList[4];
			spriteBackgroundHover.sprite = spriteBackgroundList[4];
		}
	}

	public void MoveIn(string type, float delay, Hero _hero)
	{
		animatedUse.gameObject.SetActive(value: false);
		theHero = _hero;
		base.gameObject.SetActive(value: true);
		timesExecuted.text = "";
		StartCoroutine(MoveInCo(type, delay));
	}

	private IEnumerator MoveInCo(string type, float delay)
	{
		ShowIcon(type);
		yield return Globals.Instance.WaitForSeconds(delay);
		float y = 0f;
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, -4.5f, base.transform.localPosition.z);
		vectorDestination = new Vector3(base.transform.localPosition.x, y, base.transform.localPosition.z);
		while (Vector3.Distance(base.transform.localPosition, vectorDestination) > 0.05f)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vectorDestination, movementSpeed);
			yield return null;
		}
		base.transform.localPosition = vectorDestination;
	}

	public void MoveOut(float delay)
	{
		if (base.transform.gameObject.activeSelf)
		{
			StartCoroutine(MoveOutCo(delay));
		}
	}

	private IEnumerator MoveOutCo(float delay)
	{
		yield return Globals.Instance.WaitForSeconds(delay);
		float y = (Globals.Instance.sizeH * 0.5f - 4.5f) * -2f;
		vectorDestination = new Vector3(base.transform.localPosition.x, y, base.transform.localPosition.z);
		while (Vector3.Distance(base.transform.localPosition, vectorDestination) > 0.05f)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vectorDestination, movementSpeed * 2f);
			yield return null;
		}
		base.transform.localPosition = vectorDestination - new Vector3(0f, 100f, 0f);
		animatedUse.gameObject.SetActive(value: false);
	}

	public void ShowIconCorruption(CardData cData)
	{
		cardData = cData;
		collider.enabled = true;
		iconSpr.sprite = cardData.Sprite;
		iconOn.gameObject.SetActive(value: true);
		iconOff.gameObject.SetActive(value: false);
	}

	public void ShowIconExternal(string type, Character _character)
	{
		if (_character == null)
		{
			return;
		}
		string text = "";
		switch (type)
		{
		case "weapon":
			text = _character.Weapon;
			break;
		case "armor":
			text = _character.Armor;
			break;
		case "jewelry":
			text = _character.Jewelry;
			break;
		case "accesory":
			text = _character.Accesory;
			break;
		case "pet":
			text = _character.Pet;
			break;
		case "enchantment":
			text = _character.Enchantment;
			break;
		case "enchantment2":
			text = _character.Enchantment2;
			break;
		case "enchantment3":
			text = _character.Enchantment3;
			break;
		}
		if (text != "")
		{
			if (_character.IsHero)
			{
				theHero = (Hero)_character;
				if (!theHero.Alive)
				{
					return;
				}
			}
			else
			{
				theNPC = (NPC)_character;
				if (!theNPC.Alive)
				{
					return;
				}
			}
		}
		ShowIconFunc(text);
	}

	public void ShowIcon(string type, string itemId = "", bool fromTome = false)
	{
		string cardItem = "";
		if ((bool)MatchManager.Instance && !fromTome)
		{
			Hero heroHeroActive = MatchManager.Instance.GetHeroHeroActive();
			if (heroHeroActive != null)
			{
				switch (type)
				{
				case "weapon":
					cardItem = heroHeroActive.Weapon;
					break;
				case "armor":
					cardItem = heroHeroActive.Armor;
					break;
				case "jewelry":
					cardItem = heroHeroActive.Jewelry;
					break;
				case "accesory":
					cardItem = heroHeroActive.Accesory;
					break;
				case "pet":
					cardItem = heroHeroActive.Pet;
					break;
				}
				ShowIconFunc(cardItem);
			}
		}
		else
		{
			cardItem = itemId;
			ShowIconFunc(cardItem);
		}
	}

	private void ShowIconFunc(string cardItem)
	{
		if (cardItem == "")
		{
			cardData = null;
			collider.enabled = false;
			iconOn.gameObject.SetActive(value: false);
			iconOff.gameObject.SetActive(value: true);
			rareParticles.gameObject.SetActive(value: false);
			return;
		}
		cardData = Globals.Instance.GetCardData(cardItem, instantiate: false);
		if (cardData == null)
		{
			return;
		}
		if ((bool)MatchManager.Instance && theHero != null && cardData.Item != null && cardData.Item.TimesPerCombat > 0)
		{
			SetTimesExecuted(MatchManager.Instance.ItemExecutedInThisCombat(theHero.Id, cardItem), doAnim: false);
		}
		else
		{
			SetTimesExecuted(0, doAnim: false);
		}
		collider.enabled = true;
		iconSpr.sprite = cardData.Sprite;
		iconOn.gameObject.SetActive(value: true);
		iconOff.gameObject.SetActive(value: false);
		if (rareParticles.gameObject.activeSelf)
		{
			rareParticles.gameObject.SetActive(value: false);
		}
		if (cardData.CardUpgraded != Enums.CardUpgraded.Rare)
		{
			return;
		}
		if (base.transform.parent.gameObject.name == "ItemIcons")
		{
			if (rareParticles.gameObject.activeSelf)
			{
				rareParticles.gameObject.SetActive(value: false);
			}
		}
		else if (!rareParticles.gameObject.activeSelf)
		{
			rareParticles.gameObject.SetActive(value: true);
		}
	}

	public void DoHover(bool state)
	{
		if (state)
		{
			if (showCardCo != null)
			{
				StopCoroutine(showCardCo);
			}
			if (base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
			{
				showCardCo = StartCoroutine(ShowCardCo());
			}
		}
		else
		{
			if (showCardCo != null)
			{
				StopCoroutine(showCardCo);
			}
			if (CI != null)
			{
				CI.HideKeyNotes();
			}
			if (card != null)
			{
				Object.Destroy(card);
				CI = null;
			}
		}
		if (cardData == null || cardData.CardType != Enums.CardType.Corruption)
		{
			spriteBackgroundHover.gameObject.SetActive(state);
		}
	}

	public void StopCardAnimation()
	{
		if (showCardCo != null)
		{
			StopCoroutine(showCardCo);
		}
	}

	private IEnumerator ShowCardCo()
	{
		if (cardData == null)
		{
			yield break;
		}
		bool isCorruptionOrEnchantment = false;
		Vector3 offsetDestination;
		if (!(MatchManager.Instance != null) || !(base.transform.parent.gameObject.name != "TomeItemIcons"))
		{
			offsetDestination = ((!(LootManager.Instance != null)) ? new Vector3(-2.1f, 0f, 0f) : new Vector3(0f, 1.95f, 0f));
		}
		else
		{
			switch (base.gameObject.name.ToLower())
			{
			case "corruptionicon":
			case "enchantment":
			case "enchantment2":
			case "enchantment3":
				offsetDestination = new Vector3(0f, -2.1f, 0f);
				isCorruptionOrEnchantment = false;
				break;
			default:
				offsetDestination = new Vector3(0f, 2.1f, 0f);
				if (itemType == "pet")
				{
					offsetDestination -= new Vector3(0.4f, 0f, 0f);
				}
				break;
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.02f);
		Object.Destroy(card);
		Transform parent = GameManager.Instance.TempContainer;
		if (MatchManager.Instance != null)
		{
			parent = MatchManager.Instance.amplifiedTransform;
		}
		card = Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, parent);
		card.name = cardData.Id;
		CI = card.GetComponent<CardItem>();
		CI.SetCard(cardData.Id, deckScale: true, theHero, theNPC, GetFromGlobal: true);
		CI.DisableTrail();
		if ((bool)LootManager.Instance)
		{
			CI.TopLayeringOrder("UI", 32000);
		}
		else
		{
			CI.TopLayeringOrder("UI", 32000);
		}
		if (!isCorruptionOrEnchantment)
		{
			CI.DisableCollider();
		}
		else
		{
			CI.EnableCollider();
		}
		card.transform.position = base.transform.position + new Vector3(0f, 0.4f, 0f);
		CI.SetDestinationScaleRotation(base.transform.position + offsetDestination, 1.2f, Quaternion.Euler(0f, 0f, 0f));
		if (mouseIsOver)
		{
			CI.ShowKeyNotes(followCardPosition: true);
		}
		if (LootManager.Instance != null)
		{
			CI.SetDestinationLocalScale(1.2f);
		}
		else if (CardCraftManager.Instance != null)
		{
			CI.SetDestinationLocalScale(1.4f);
		}
		CI.active = true;
	}

	private void ShowCardScreenManager()
	{
		if (CardScreenManager.Instance != null)
		{
			CardScreenManager.Instance.ShowCardScreen(_state: true);
			CardScreenManager.Instance.SetCardData(cardData);
		}
	}

	private void OnMouseEnter()
	{
		if (!SettingsManager.Instance.IsActive() && !AlertManager.Instance.IsActive() && (!MatchManager.Instance || !MatchManager.Instance.CardDrag) && (!MatchManager.Instance || !EventSystem.current.IsPointerOverGameObject() || TomeManager.Instance.IsActive()) && !CardScreenManager.Instance.IsActive())
		{
			mouseIsOver = true;
			DoHover(state: true);
			GameManager.Instance.SetCursorHover();
			GameManager.Instance.PlayLibraryAudio("ui_menu_popup_01");
		}
	}

	private void OnMouseExit()
	{
		if ((!MatchManager.Instance || !MatchManager.Instance.CardDrag) && (!MatchManager.Instance || !EventSystem.current.IsPointerOverGameObject() || TomeManager.Instance.IsActive()))
		{
			mouseIsOver = false;
			DoHover(state: false);
			GameManager.Instance.SetCursorPlain();
		}
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform))
		{
			fOnMouseUp();
		}
	}

	private void fOnMouseUp()
	{
		if (!SettingsManager.Instance.IsActive() && !AlertManager.Instance.IsActive() && (!MatchManager.Instance || (!MatchManager.Instance.CardDrag && !EventSystem.current.IsPointerOverGameObject())))
		{
			ShowCardScreenManager();
		}
	}

	private void OnMouseOver()
	{
		if (MatchManager.Instance != null && !MatchManager.Instance.amplifiedTransformShow)
		{
			MatchManager.Instance.amplifiedTransformShow = true;
		}
		if (Input.GetMouseButtonUp(1))
		{
			fOnMouseUp();
		}
	}
}
