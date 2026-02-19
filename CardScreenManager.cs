using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardScreenManager : MonoBehaviour
{
	public Transform content;

	public SpriteRenderer backgroundSPR;

	public SpriteRenderer backgroundSmooth;

	public TitleMovement titleMovement;

	public BotonGeneric bGeneric;

	public Transform closeButton;

	public Transform cardContainer;

	public Transform cardUpgrade;

	public Transform cardUpgradeContainer;

	public TitleMovement titleMovementUp;

	public TitleMovement titleMovementDown;

	public GameObject relatedCard;

	public Transform relatedContainer;

	public Transform relatedText;

	public Transform cardBasicValues;

	public Transform cardItemCorruption;

	public Transform rareText;

	public TMP_Text rarityText;

	public Transform dropPlace;

	private bool state;

	private CanvasScaler relatedScaler;

	private CardData cardData;

	private CardItem CI;

	private CardItem CIBack;

	private CardItem CIUp;

	private CardItem CIDown;

	private GameObject cardGO;

	private GameObject cardGOBack;

	private GameObject cardGOUp;

	private GameObject cardGODown;

	private Coroutine co;

	private List<Transform> controllerList = new List<Transform>();

	private int controllerHorizontalIndex = -1;

	private Vector2 warpPosition;

	public static CardScreenManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		relatedScaler = relatedContainer.GetComponent<CanvasScaler>();
		state = false;
	}

	private void Start()
	{
		if (cardGOUp == null)
		{
			cardGOUp = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, cardUpgradeContainer);
			CIUp = cardGOUp.GetComponent<CardItem>();
			CIUp.SetDestinationScaleRotation(new Vector3(0f, 0f, 0f), 1.4f, Quaternion.Euler(0f, 0f, 0f));
		}
		if (cardGODown == null)
		{
			cardGODown = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, cardUpgradeContainer);
			CIDown = cardGODown.GetComponent<CardItem>();
			CIDown.SetDestinationScaleRotation(new Vector3(0f, 0f, 0f), 1.4f, Quaternion.Euler(0f, 0f, 0f));
		}
	}

	public void Resize()
	{
		if (Globals.Instance.scale < 1f)
		{
			relatedScaler.matchWidthOrHeight = 1f;
		}
		else
		{
			relatedScaler.matchWidthOrHeight = 0f;
		}
	}

	public void ShowCardScreen(bool _state)
	{
		PopupManager.Instance.ClosePopup();
		GameManager.Instance.CleanTempContainer();
		state = _state;
		content.gameObject.SetActive(_state);
		if (!_state)
		{
			foreach (Transform item in relatedContainer)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.ShowSearch(state: true);
			}
			OptionsManager.Instance.Show();
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				PlayerUIManager.Instance.Show();
			}
		}
		else
		{
			OptionsManager.Instance.Hide();
			if ((bool)MapManager.Instance || (bool)TownManager.Instance)
			{
				PlayerUIManager.Instance.Hide();
			}
			if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.ShowSearch(state: false);
			}
		}
	}

	public bool IsActive()
	{
		return state;
	}

	public void SetCardData(CardData _cardData)
	{
		if (co != null)
		{
			StopCoroutine(co);
		}
		cardData = _cardData;
		co = StartCoroutine(SetCardDataCo());
	}

	private IEnumerator SetCardDataCo()
	{
		string hex = ((this.cardData.CardClass != Enums.CardClass.Boon && this.cardData.CardClass != Enums.CardClass.Injury && this.cardData.CardClass != Enums.CardClass.Item && this.cardData.CardClass != Enums.CardClass.Special && this.cardData.CardClass != Enums.CardClass.Enchantment) ? Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), this.cardData.CardClass)] : Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.CardClass), this.cardData.CardClass)]);
		SpriteRenderer spriteRenderer = backgroundSPR;
		BotonGeneric botonGeneric = bGeneric;
		Color color = (backgroundSmooth.color = Functions.HexToColor(hex));
		spriteRenderer.color = (botonGeneric.color = color);
		backgroundSPR.color = new Color(backgroundSPR.color.r, backgroundSPR.color.g, backgroundSPR.color.b, 0.97f);
		backgroundSmooth.color = new Color(backgroundSmooth.color.r, backgroundSmooth.color.g, backgroundSmooth.color.b, 0.4f);
		bGeneric.SetColor();
		bGeneric.SetBorderColorFromColor();
		dropPlace.transform.gameObject.SetActive(value: false);
		if (this.cardData.Item != null)
		{
			if (cardBasicValues.gameObject.activeSelf)
			{
				cardBasicValues.gameObject.SetActive(value: false);
			}
			if (!cardItemCorruption.gameObject.activeSelf)
			{
				cardItemCorruption.gameObject.SetActive(value: true);
			}
			if (this.cardData.Item.SpriteBossDrop != null)
			{
				dropPlace.transform.gameObject.SetActive(value: true);
				dropPlace.GetComponent<SpriteRenderer>().sprite = this.cardData.Item.SpriteBossDrop;
			}
		}
		else
		{
			if (!cardBasicValues.gameObject.activeSelf)
			{
				cardBasicValues.gameObject.SetActive(value: true);
			}
			if (cardItemCorruption.gameObject.activeSelf)
			{
				cardItemCorruption.gameObject.SetActive(value: false);
			}
		}
		Vector3 position = new Vector3(-5f, 0.2f, 0f);
		if (cardGO == null)
		{
			cardGO = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, position, Quaternion.identity, cardContainer);
			CI = cardGO.GetComponent<CardItem>();
		}
		if (cardGOBack == null)
		{
			cardGOBack = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, position, Quaternion.identity, cardContainer);
			CIBack = cardGOBack.GetComponent<CardItem>();
		}
		if (this.cardData.CardUpgraded == Enums.CardUpgraded.No)
		{
			titleMovement.SetText(this.cardData.CardName);
			titleMovement.SetColor(Globals.Instance.ColorColor["white"]);
		}
		else if (Globals.Instance.GetCardData(this.cardData.UpgradedFrom, instantiate: false) != null)
		{
			if (this.cardData.CardUpgraded == Enums.CardUpgraded.A)
			{
				titleMovement.SetText(Globals.Instance.GetCardData(this.cardData.UpgradedFrom).CardName);
				titleMovement.SetColor(Globals.Instance.ColorColor["blueCardTitle"]);
			}
			else if (this.cardData.CardUpgraded == Enums.CardUpgraded.B)
			{
				titleMovement.SetText(Globals.Instance.GetCardData(this.cardData.UpgradedFrom).CardName);
				titleMovement.SetColor(Globals.Instance.ColorColor["goldCardTitle"]);
			}
			else if (this.cardData.CardUpgraded == Enums.CardUpgraded.Rare)
			{
				titleMovement.SetText(Globals.Instance.GetCardData(this.cardData.UpgradedFrom).CardName);
				titleMovement.SetColor(Globals.Instance.ColorColor["purple"]);
			}
		}
		string text = this.cardData.Id.Split('_')[0];
		GameObject obj = cardGO;
		string text2 = (cardGOBack.name = text);
		obj.name = text2;
		CI.SetCard(text, deckScale: true, null, null, GetFromGlobal: true);
		CIBack.SetCard(text, deckScale: true, null, null, GetFromGlobal: true);
		CI.TopLayeringOrder("UI", 31600);
		CIBack.TopLayeringOrder("UI", 31500);
		CI.DisableTrail();
		CIBack.DisableTrail();
		CI.DrawBorder("black");
		CIBack.DrawBorder("black");
		CI.SetDestinationScaleRotation(position, 2f, Quaternion.Euler(0f, 0f, 0f));
		CIBack.SetDestinationScaleRotation(position, 2f, Quaternion.Euler(0f, 0f, 7f));
		CI.active = true;
		CIBack.active = true;
		CardItem cI = CI;
		CardItem cIBack = CIBack;
		bool flag = true;
		cIBack.enabled = true;
		cI.enabled = flag;
		cardGO.GetComponent<BoxCollider2D>().enabled = false;
		cardGOBack.GetComponent<BoxCollider2D>().enabled = false;
		if (this.cardData.RelatedCard == "")
		{
			relatedText.gameObject.SetActive(value: false);
			relatedContainer.gameObject.SetActive(value: false);
		}
		else
		{
			relatedText.gameObject.SetActive(value: true);
			relatedContainer.gameObject.SetActive(value: true);
			foreach (Transform item in relatedContainer)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			GameObject obj2 = UnityEngine.Object.Instantiate(relatedCard, Vector3.zero, Quaternion.identity, relatedContainer);
			obj2.transform.localScale = new Vector3(1f, 1f, 1f);
			obj2.transform.localPosition = new Vector3(0f, -0.05f, 0f);
			obj2.GetComponent<CardVertical>().SetCard(this.cardData.RelatedCard);
			if (this.cardData.RelatedCard2 != "")
			{
				GameObject obj3 = UnityEngine.Object.Instantiate(relatedCard, Vector3.zero, Quaternion.identity, relatedContainer);
				obj3.transform.localScale = new Vector3(1f, 1f, 1f);
				obj3.transform.localPosition = new Vector3(0f, -0.45f, 0f);
				obj3.GetComponent<CardVertical>().SetCard(this.cardData.RelatedCard2);
				if (this.cardData.RelatedCard3 != "")
				{
					GameObject obj4 = UnityEngine.Object.Instantiate(relatedCard, Vector3.zero, Quaternion.identity, relatedContainer);
					obj4.transform.localScale = new Vector3(1f, 1f, 1f);
					obj4.transform.localPosition = new Vector3(0f, -0.85f, 0f);
					obj4.GetComponent<CardVertical>().SetCard(this.cardData.RelatedCard3);
				}
			}
		}
		if (this.cardData.CardRarity == Enums.CardRarity.Common)
		{
			rarityText.text = Texts.Instance.GetText("cardCommon");
			rarityText.color = new Color(0.66f, 0.66f, 0.66f);
		}
		else if (this.cardData.CardRarity == Enums.CardRarity.Uncommon)
		{
			rarityText.text = Texts.Instance.GetText("cardUncommon");
			rarityText.color = Globals.Instance.RarityColor["uncommon"];
		}
		else if (this.cardData.CardRarity == Enums.CardRarity.Rare)
		{
			rarityText.text = Texts.Instance.GetText("cardRare");
			rarityText.color = Globals.Instance.RarityColor["rare"];
		}
		else if (this.cardData.CardRarity == Enums.CardRarity.Epic)
		{
			rarityText.text = Texts.Instance.GetText("cardEpic");
			rarityText.color = Globals.Instance.RarityColor["epic"];
		}
		else if (this.cardData.CardRarity == Enums.CardRarity.Mythic)
		{
			rarityText.text = Texts.Instance.GetText("cardMythic");
			rarityText.color = Globals.Instance.RarityColor["mythic"];
		}
		else
		{
			rarityText.text = "";
		}
		if (this.cardData.CardUpgraded == Enums.CardUpgraded.Rare || this.cardData.CardClass == Enums.CardClass.Special)
		{
			rareText.gameObject.SetActive(value: true);
			if (this.cardData.CardUpgraded == Enums.CardUpgraded.Rare)
			{
				rareText.GetComponent<TMP_Text>().text = Texts.Instance.GetText("corruptedVersion");
			}
			else if (this.cardData.Starter)
			{
				rareText.GetComponent<TMP_Text>().text = Texts.Instance.GetText("characterCard");
			}
			else
			{
				rareText.GetComponent<TMP_Text>().text = Texts.Instance.GetText("specialCard");
			}
		}
		else
		{
			rareText.gameObject.SetActive(value: false);
		}
		if (this.cardData.CardClass == Enums.CardClass.Monster || (this.cardData.CardUpgraded == Enums.CardUpgraded.No && this.cardData.UpgradesTo1 == "" && this.cardData.Item == null))
		{
			cardUpgrade.gameObject.SetActive(value: false);
		}
		else
		{
			cardUpgrade.gameObject.SetActive(value: true);
			float posY = 2.4f;
			if (cardGOUp == null)
			{
				cardGOUp = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, cardUpgradeContainer);
				CIUp = cardGOUp.GetComponent<CardItem>();
			}
			if (cardGODown == null)
			{
				cardGODown = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, cardUpgradeContainer);
				CIDown = cardGODown.GetComponent<CardItem>();
			}
			CardItem cIUp = CIUp;
			CardItem cIDown = CIDown;
			flag = false;
			cIDown.active = false;
			cIUp.active = flag;
			CIUp.transform.localPosition = new Vector3(0f, posY, -1f);
			CIDown.transform.localPosition = new Vector3(0f, 0f - posY, -1f);
			bool hideItemDown = true;
			if (this.cardData.Item != null)
			{
				if (this.cardData.UpgradesToRare != null)
				{
					titleMovementUp.SetText(Texts.Instance.GetText("corruptedVersion"));
					titleMovementUp.SetColor(Globals.Instance.ColorColor["purple"]);
					CIUp.SetCard(this.cardData.UpgradesToRare.Id, deckScale: true, null, null, GetFromGlobal: true);
					if (!CIUp.gameObject.activeSelf)
					{
						CIUp.gameObject.SetActive(value: true);
						titleMovementUp.gameObject.SetActive(value: true);
					}
				}
				else if (this.cardData.UpgradedFrom != "")
				{
					titleMovementUp.SetText(Texts.Instance.GetText("sourceCard"));
					titleMovementUp.SetColor(Globals.Instance.ColorColor["white"]);
					CIUp.SetCard(this.cardData.UpgradedFrom, deckScale: true, null, null, GetFromGlobal: true);
					if (!CIUp.gameObject.activeSelf)
					{
						CIUp.gameObject.SetActive(value: true);
						titleMovementUp.gameObject.SetActive(value: true);
					}
				}
				else if (this.cardData.CardUpgraded == Enums.CardUpgraded.No)
				{
					if (this.cardData.UpgradesTo1 != "")
					{
						titleMovementUp.SetText(Texts.Instance.GetText("upgradesTo"));
						titleMovementUp.SetColor(Globals.Instance.ColorColor["blueCardTitle"]);
						CIUp.SetCard(this.cardData.UpgradesTo1, deckScale: true, null, null, GetFromGlobal: true);
						CIUp.ShowDifferences(this.cardData);
					}
					else if (CIUp.gameObject.activeSelf)
					{
						CIUp.gameObject.SetActive(value: false);
						titleMovementUp.gameObject.SetActive(value: false);
					}
					if (this.cardData.UpgradesTo2 != "")
					{
						titleMovementDown.SetText(Texts.Instance.GetText("upgradesTo"));
						titleMovementDown.SetColor(Globals.Instance.ColorColor["goldCardTitle"]);
						CIDown.SetCard(this.cardData.UpgradesTo2, deckScale: true, null, null, GetFromGlobal: true);
						CIDown.ShowDifferences(this.cardData);
						hideItemDown = false;
					}
					else if (CIDown.gameObject.activeSelf)
					{
						CIDown.gameObject.SetActive(value: false);
						titleMovementDown.gameObject.SetActive(value: false);
					}
				}
				else if (cardUpgrade.gameObject.activeSelf)
				{
					cardUpgrade.gameObject.SetActive(value: false);
				}
				if (!CIUp.gameObject.activeSelf && !CIDown.gameObject.activeSelf && cardUpgrade.gameObject.activeSelf)
				{
					cardUpgrade.gameObject.SetActive(value: false);
				}
			}
			else if (this.cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				titleMovementUp.SetText(Texts.Instance.GetText("upgradesTo"));
				titleMovementUp.SetColor(Globals.Instance.ColorColor["blueCardTitle"]);
				titleMovementDown.SetText(Texts.Instance.GetText("upgradesTo"));
				titleMovementDown.SetColor(Globals.Instance.ColorColor["goldCardTitle"]);
				CIUp.SetCard(this.cardData.UpgradesTo1, deckScale: true, null, null, GetFromGlobal: true);
				CIDown.SetCard(this.cardData.UpgradesTo2, deckScale: true, null, null, GetFromGlobal: true);
				CIUp.ShowDifferences(this.cardData);
				CIDown.ShowDifferences(this.cardData);
			}
			else
			{
				titleMovementUp.SetText(Texts.Instance.GetText("upgradedFrom"));
				titleMovementUp.SetColor(Globals.Instance.ColorColor["white"]);
				titleMovementDown.SetText(Texts.Instance.GetText("transmutesTo"));
				CIUp.SetCard(this.cardData.UpgradedFrom, deckScale: true, null, null, GetFromGlobal: true);
				CIUp.ShowDifferences(this.cardData);
				CardData cardData = Globals.Instance.GetCardData(this.cardData.UpgradedFrom);
				if (cardData.UpgradesTo1.ToLower() == text)
				{
					CIDown.SetCard(cardData.UpgradesTo2, deckScale: true, null, null, GetFromGlobal: true);
					CIDown.ShowDifferences(this.cardData);
					titleMovementDown.SetColor(Globals.Instance.ColorColor["goldCardTitle"]);
				}
				else
				{
					CIDown.SetCard(cardData.UpgradesTo1, deckScale: true, null, null, GetFromGlobal: true);
					CIDown.ShowDifferences(this.cardData);
					titleMovementDown.SetColor(Globals.Instance.ColorColor["blueCardTitle"]);
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.05f);
			CIUp.TopLayeringOrder("UI", 31551);
			CIDown.TopLayeringOrder("UI", 31550);
			CIUp.DisableTrail();
			CIDown.DisableTrail();
			CIUp.active = true;
			CIDown.active = true;
			CIUp.SetDestinationScaleRotation(new Vector3(0f, posY, -1f), 1.4f, Quaternion.Euler(0f, 0f, 0f));
			CIDown.SetDestinationScaleRotation(new Vector3(0f, 0f - posY, -1f), 1.4f, Quaternion.Euler(0f, 0f, 0f));
			if (this.cardData.Item != null)
			{
				if (hideItemDown)
				{
					if (CIDown.gameObject.activeSelf)
					{
						CIDown.gameObject.SetActive(value: false);
						titleMovementDown.gameObject.SetActive(value: false);
					}
				}
				else if (!CIDown.gameObject.activeSelf)
				{
					CIDown.gameObject.SetActive(value: true);
					titleMovementDown.gameObject.SetActive(value: true);
				}
				CIUp.HideDifferences();
				CIDown.HideDifferences();
			}
			else if (this.cardData.CardUpgraded == Enums.CardUpgraded.Rare)
			{
				CIUp.ShowDifferences(this.cardData);
				titleMovementUp.SetText(Texts.Instance.GetText("upgradesTo"));
				titleMovementUp.SetText(Texts.Instance.GetText("sourceCard"));
				if (CIDown.gameObject.activeSelf)
				{
					CIDown.gameObject.SetActive(value: false);
					titleMovementDown.gameObject.SetActive(value: false);
				}
			}
			else
			{
				if (!CIUp.gameObject.activeSelf)
				{
					CIUp.gameObject.SetActive(value: true);
					titleMovementUp.gameObject.SetActive(value: true);
				}
				if (!CIDown.gameObject.activeSelf)
				{
					CIDown.gameObject.SetActive(value: true);
					titleMovementDown.gameObject.SetActive(value: true);
				}
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		CI.ShowKeyNotes();
		Vector3 offset = Vector3.zero;
		Vector3 scale = new Vector3(1.1f, 1.1f, 1f);
		if (this.cardData.KeyNotes.Count > 7)
		{
			offset = new Vector3(0f, 120f, 0f);
			scale = new Vector3(0.8f, 0.8f, 1f);
		}
		else if (this.cardData.KeyNotes.Count > 6)
		{
			offset -= new Vector3(0f, 80f, 0f);
		}
		PopupManager.Instance.StablishPopupPositionSize(offset, scale);
		yield return Globals.Instance.WaitForSeconds(0.2f);
		PopupManager.Instance.StablishPopupPositionSize(offset, scale);
		CardItem cI2 = CI;
		CardItem cIBack2 = CIBack;
		flag = false;
		cIBack2.enabled = false;
		cI2.enabled = flag;
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
		controllerList.Clear();
		controllerList.Add(closeButton);
		if (Functions.TransformIsVisible(CIUp.transform))
		{
			controllerList.Add(CIUp.transform);
		}
		if (Functions.TransformIsVisible(CIDown.transform))
		{
			controllerList.Add(CIDown.transform);
		}
		if (Functions.TransformIsVisible(relatedContainer.transform))
		{
			controllerList.Add(relatedContainer.transform);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
