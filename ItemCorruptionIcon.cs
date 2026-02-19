using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using WebSocketSharp;

public class ItemCorruptionIcon : MonoBehaviour
{
	private string internalId;

	private SpriteRenderer itemSpriteRenderer;

	private Transform rareParticles;

	private bool allowInput;

	private Coroutine showCardCo;

	private CardItem CI;

	private GameObject card;

	private CardData cardData;

	private bool mouseIsOver;

	[SerializeField]
	private Transform spriteBackgroundHover;

	private string itemType;

	private void Awake()
	{
		itemSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		rareParticles = GetComponentInChildren<ParticleSystem>(includeInactive: true).transform;
	}

	private void OnMouseUp()
	{
		if (allowInput && !isCardCorrupted(cardData))
		{
			fOnMouseUP();
		}
	}

	private void OnMouseOver()
	{
		DoHover(state: true);
		if (Input.GetMouseButtonUp(1))
		{
			fOnMouseUpRight();
		}
	}

	private void fOnMouseUpRight()
	{
		if (!SettingsManager.Instance.IsActive() && !AlertManager.Instance.IsActive() && (!MatchManager.Instance || (!MatchManager.Instance.CardDrag && !EventSystem.current.IsPointerOverGameObject())))
		{
			ShowCardScreenManager();
		}
	}

	private void ShowCardScreenManager()
	{
		if (CardScreenManager.Instance != null)
		{
			DoHover(state: false);
			CardScreenManager.Instance.ShowCardScreen(_state: true);
			CardScreenManager.Instance.SetCardData(cardData);
		}
	}

	private void OnMouseExit()
	{
		DoHover(state: false);
	}

	private void fOnMouseUP()
	{
		if (!internalId.IsNullOrEmpty())
		{
			GameManager.Instance.CleanTempContainer();
			CardCraftManager.Instance.SelectCard(internalId);
		}
	}

	public void SetSprite(Sprite sprite)
	{
		itemSpriteRenderer.sprite = sprite;
	}

	public void SetInternalId(string internalId)
	{
		this.internalId = internalId;
		cardData = Globals.Instance.GetCardData(internalId.Split("_")[0]);
	}

	public void AllowInput(bool state)
	{
		allowInput = state;
	}

	public void ShowRareParticles(bool state = true)
	{
		rareParticles.gameObject.SetActive(state);
	}

	public void DoHover(bool state)
	{
		if (mouseIsOver == state || !allowInput)
		{
			return;
		}
		mouseIsOver = state;
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

	private IEnumerator ShowCardCo()
	{
		if (!(cardData == null))
		{
			Vector3 position = new Vector3(-2.1f, 0f, 0f);
			Object.Destroy(card);
			card = Object.Instantiate(parent: base.transform, original: GameManager.Instance.CardPrefab, position: Vector3.zero, rotation: Quaternion.identity);
			card.name = cardData.Id;
			CI = card.GetComponent<CardItem>();
			CI.SetCard(cardData.Id, deckScale: true, AtOManager.Instance.GetHero(CardCraftManager.Instance.heroIndex), null, GetFromGlobal: true);
			CI.DisableTrail();
			if ((bool)LootManager.Instance)
			{
				CI.TopLayeringOrder("UI", 32000);
			}
			else
			{
				CI.TopLayeringOrder("UI", 32000);
			}
			if (0 == 0)
			{
				CI.DisableCollider();
			}
			else
			{
				CI.EnableCollider();
			}
			card.transform.position = base.transform.position + new Vector3(0f, 0.4f, 0f);
			CI.SetDestinationScaleRotation(position, 1.2f, Quaternion.Euler(0f, 0f, 0f));
			if (CardCraftManager.Instance != null)
			{
				CI.SetDestinationLocalScale(1.4f);
			}
			CI.active = true;
		}
		yield break;
	}

	private bool isCardCorrupted(CardData cardData)
	{
		if (cardData == null)
		{
			return false;
		}
		if (cardData.CardUpgraded == Enums.CardUpgraded.Rare)
		{
			return true;
		}
		return false;
	}
}
