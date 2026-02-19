using System.Text;
using TMPro;
using UnityEngine;

public class CardCraftItem : MonoBehaviour
{
	public Transform container;

	public Transform button;

	public Transform buttonItem;

	public Transform availability;

	public Transform availabilityYes;

	public Transform availabilityNo;

	public TMP_Text availabilityYesText;

	public TMP_Text availabilityNoText;

	public Transform lockIcon;

	public Transform lockIconBackground;

	public PopupText lockPopup;

	public string cardId;

	private string shopId;

	private CardItem CI;

	private BotonGeneric bGeneric;

	private BotonGeneric bGenericItem;

	public Vector3 position;

	private Hero currentHero;

	private bool available = true;

	private int index;

	private new bool enabled;

	private CardData cData;

	private Vector3 singularityVector3Offset;

	public Transform arrowPointer;

	public bool Available
	{
		get
		{
			return available;
		}
		set
		{
			available = value;
		}
	}

	public bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			enabled = value;
		}
	}

	private void Awake()
	{
		singularityVector3Offset = Vector3.zero;
		singularityVector3Offset.x = 0.23f;
	}

	public void SetHero(Hero _hero)
	{
		currentHero = _hero;
	}

	public void SetIndex(int _index)
	{
		index = _index;
	}

	public void SetButtonText(string _buttonText)
	{
		bGeneric.gameObject.SetActive(value: true);
		availability.gameObject.SetActive(value: true);
		bGenericItem.gameObject.SetActive(value: false);
		bGeneric.SetText(_buttonText);
		if (GameManager.Instance.IsSingularity())
		{
			availability.gameObject.SetActive(value: false);
		}
	}

	public void SetButtonTextItem(string _buttonTextItem)
	{
		bGeneric.gameObject.SetActive(value: false);
		availability.gameObject.SetActive(value: false);
		bGenericItem.gameObject.SetActive(value: true);
		bGenericItem.SetText(_buttonTextItem);
	}

	public void SetPosition(Vector3 _position)
	{
		position = _position;
	}

	public CardItem GetCI()
	{
		if (CI != null)
		{
			return CI;
		}
		return null;
	}

	public void EnableButton(bool _state)
	{
		enabled = _state;
		if (_state)
		{
			bGeneric.Enable();
			bGenericItem.Enable();
		}
		else
		{
			bGeneric.Disable();
			bGenericItem.Disable();
		}
	}

	public void ShowSold(bool _state)
	{
		if (CI != null)
		{
			CI.ShowSold(_state);
		}
	}

	public void ShowDisable(bool _state)
	{
		if (CI != null)
		{
			CI.ShowDisable(_state);
		}
	}

	public void ShowLock(bool _state, string _text = "")
	{
		if (lockIcon.gameObject.activeSelf != _state)
		{
			lockIcon.gameObject.SetActive(_state);
		}
		if (lockIconBackground.gameObject.activeSelf != _state)
		{
			lockIconBackground.gameObject.SetActive(_state);
		}
		if (_state)
		{
			lockPopup.text = _text;
		}
	}

	public void SetGenericCard(bool item = false)
	{
		bGeneric = button.transform.GetComponent<BotonGeneric>();
		bGenericItem = buttonItem.transform.GetComponent<BotonGeneric>();
		GameObject gameObject = Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, container);
		CI = gameObject.GetComponent<CardItem>();
		CI.TopLayeringOrder("UI", -500);
		CI.SetLocalPosition(position);
		if (item)
		{
			CI.SetLocalScale(new Vector3(1.25f, 1.25f, 1f));
		}
		else
		{
			CI.SetLocalScale(new Vector3(1.1f, 1.1f, 1f));
		}
		CI.cardforaddcard = true;
		CI.DisableTrail();
		if (item)
		{
			buttonItem.localPosition = new Vector3(position.x, position.y - 2.05f, 0f);
			buttonItem.gameObject.name = "CraftItemBuyButton";
			return;
		}
		availability.localPosition = new Vector3(position.x - 0.75f, position.y - 1.82f, 0f);
		button.localPosition = new Vector3(position.x + 0.25f, position.y - 1.82f, 0f);
		button.gameObject.name = "CraftBuyButton";
		if (GameManager.Instance.IsSingularity())
		{
			button.localPosition -= singularityVector3Offset;
		}
	}

	public void SetCard(string _cardId, string _shopId = "", Hero _hero = null)
	{
		if (_hero == null)
		{
			_hero = currentHero;
		}
		cardId = _cardId;
		shopId = _shopId;
		cData = Globals.Instance.GetCardData(cardId, instantiate: false);
		CI.SetCard(cardId, deckScale: false, _hero);
		CI.CreateColliderAdjusted();
		CI.cardmakebig = true;
		if (cData.CardClass != Enums.CardClass.Item)
		{
			CI.DrawEnergyCost();
			CI.cardmakebigSizeMax = 1.25f;
		}
		else
		{
			CI.cardmakebigSizeMax = 1.4f;
		}
		CI.TopLayeringOrder("UI", 1000);
		bGeneric.auxString = cardId;
		bGenericItem.auxString = cardId;
		SetAvailability();
	}

	public void SetAvailability()
	{
		int[] cardAvailability = CardCraftManager.Instance.GetCardAvailability(cData.Id, shopId);
		int num = cardAvailability[0];
		int num2 = cardAvailability[1];
		if (cData.Item == null && AtOManager.Instance.Sandbox_unlimitedAvailableCards)
		{
			num2 = num + 10;
			availability.gameObject.SetActive(value: false);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=1.6><color=#BDA08A>");
		stringBuilder.Append(Texts.Instance.GetText("maximumAbb"));
		stringBuilder.Append("</size></color>\n");
		stringBuilder.Append((num2 - num).ToString());
		if (AtOManager.Instance.IsCombatTool)
		{
			availabilityYesText.text = stringBuilder.ToString();
			availabilityYes.gameObject.SetActive(value: false);
			availabilityNo.gameObject.SetActive(value: false);
			button.position = new Vector3(position.x, button.position.y, button.position.z);
			return;
		}
		if (num < num2)
		{
			availabilityYesText.text = stringBuilder.ToString();
			availabilityYes.gameObject.SetActive(value: true);
			availabilityNo.gameObject.SetActive(value: false);
		}
		else
		{
			availabilityNoText.text = stringBuilder.ToString();
			availabilityYes.gameObject.SetActive(value: false);
			availabilityNo.gameObject.SetActive(value: true);
		}
		if (num >= num2 || CardCraftManager.Instance.HavePortraitItemBought(this))
		{
			available = false;
			EnableButton(_state: false);
			CI.ShowDisable(state: true);
			if (num2 > 0 || CardCraftManager.Instance.HavePortraitItemBought(this))
			{
				CI.ShowSold(_state: true);
			}
			return;
		}
		available = true;
		CI.ShowDisable(state: false);
		if (cData.CardClass == Enums.CardClass.Item)
		{
			if (CardCraftManager.Instance.CanBuy("Item", cardId))
			{
				EnableButton(_state: true);
			}
			else
			{
				EnableButton(_state: false);
			}
		}
		else if (CardCraftManager.Instance.CanBuy("Craft", cardId))
		{
			EnableButton(_state: true);
		}
		else
		{
			EnableButton(_state: false);
		}
	}
}
