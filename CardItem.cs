using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using WebSocketSharp;

[Serializable]
public class CardItem : MonoBehaviour
{
	[ContextMenuItem("Init Card", "Init")]
	[SerializeField]
	private CardData cardData;

	private ItemData itemData;

	private string internalId;

	public SpriteRenderer portraitDrop;

	public Transform keyTransform;

	public Transform keyRed;

	public Transform keyBackground;

	public TMP_Text keyNumber;

	private Floating floating;

	public Transform cardSpriteT;

	public Transform rightClick;

	public Transform diffEnergy;

	public Transform diffTarget;

	public Transform diffRequire;

	public Transform diffVanish;

	public Transform diffInnate;

	public Transform diffSkillA;

	public Transform diffSkillB;

	public Transform soldT;

	public Transform singularityT;

	public Transform disableT;

	public Transform cardRelatedT;

	public SpriteRenderer cardRelatedBg;

	public Transform cardUpgradeT;

	public Transform cardUnlockT;

	public Transform cardElementsT;

	public Transform skillImageT;

	public Transform titleTextT;

	public Transform titleTextTBlue;

	public Transform titleTextTGold;

	public Transform titleTextTRed;

	public Transform titleTextTPurple;

	public Transform requireTextT;

	public Transform targetT;

	public Transform targetTextT;

	public Transform descriptionTextT;

	public Transform energyText;

	public Transform energyTextBg;

	private SpriteRenderer energyBg;

	public Transform energyTextItemBg;

	public Transform typeText;

	public Transform typeTextImage;

	public Transform backImageT;

	public Transform skullImageT;

	public Transform checkImageT;

	public Transform lockImage;

	public Transform lockShadowImage;

	public Transform skillUpgradedShader;

	public Transform skillRare;

	public Transform energyModification;

	public TMP_Text energyModificationTM;

	public Transform innateT;

	public Transform innateIconParticle;

	public Transform vanishT;

	public Transform vanishIconParticle;

	public Transform cardAlreadyInDeckT;

	public TMP_Text cardAlreadyInDeckText;

	public SpriteRenderer borderSprite;

	public Transform rarityUncommonParticle;

	public Transform rarityRareParticle;

	public Transform rarityEpicParticle;

	public Transform rarityMythicParticle;

	public Transform cardBorder;

	public Transform cardBorderMythic;

	private SpriteRenderer cardBorderSR;

	public ParticleSystem trailParticlesNPC;

	public ParticleSystem sightParticle;

	public ParticleSystem vanishParticle;

	public ParticleSystem borderParticle;

	public Transform dissolveParticleT;

	private ParticleSystem dissolveParticle;

	public Transform backParticleT;

	public ParticleSystem backParticle;

	public ParticleSystem backParticleSpark;

	public int tableCards;

	public int tablePosition;

	private BoxCollider2D cardCollider;

	private CursorArrow cursorArrow;

	private SpriteRenderer cardSpriteSR;

	private SpriteRenderer skillImageSR;

	private TMP_Text titleTextTM;

	private TMP_Text titleTextTMBlue;

	private TMP_Text titleTextTMGold;

	private TMP_Text titleTextTMRed;

	private TMP_Text titleTextTMPurple;

	private TMP_Text requireTextTM;

	private TMP_Text targetTextTM;

	private TMP_Text descriptionTextTM;

	private TMP_Text energyTextTM;

	private TMP_Text typeTextTM;

	public TMP_Text nameTome;

	public TMP_Text numberTome;

	public Transform numberTomeBg;

	public string lootId;

	private List<Renderer> childRendererList;

	private SpriteRenderer iconTSpriteRenderer;

	private Vector3 initialLocalPosition;

	private Vector3 destinationLocalPosition;

	private Quaternion initialLocalRotation;

	private Quaternion destinationLocalRotation;

	private Vector3 mouseClickedPosition;

	private Vector3 transformVerticalDesplazament;

	private int canInstaCast = -1;

	private int canEnergyCast = -1;

	private int canTargetCast = -1;

	private float cardSize = 1f;

	private float cardSizeTable = 1.2f;

	private float cardSizeAmplified = 1.4f;

	private float smooth = 0.18f;

	private float tiltAngle = 60f;

	private float cardScaleNPC = 0.3f;

	private float cardDistanceNPC = 0.45f;

	private Vector3 NPCTargetPosition = new Vector3(-100f, -100f, 0f);

	private NPC theNPC;

	private Hero theHero;

	public bool prediscard;

	public bool discard;

	public bool active;

	public bool casting;

	public bool cardnpc;

	public bool cardhided;

	public bool cardrevealed;

	public bool cardoutsidecombat;

	public bool cardoutsidecombatamplified;

	public bool cardoutsidelibary;

	public bool cardoutsideselection;

	public bool cardoutsidereward;

	public bool cardoutsideloot;

	public bool cardmakebig;

	public float cardmakebigSize;

	public float cardmakebigSizeMax;

	public bool cardoutsideverticallist;

	public bool cardFromEventTracker;

	public bool lockPosition;

	private bool cardDraggedCanCast;

	private int distanceForDragCast = 80;

	public bool destroyAtLocation;

	public bool cardVanish;

	public bool cardfordiscard;

	public bool cardforaddcard;

	public bool cardselectedfordiscard;

	public bool cardselectedforaddcard;

	public bool cardfordisplay;

	private int sortingOrderDiscard = -30000;

	private GameObject cardAmplifyNPC;

	private GameObject cardAmplifyOutside;

	private Coroutine revealedCoroutine;

	private Color greenColor = new Color(0f, 1f, 0.05f, 0.75f);

	private Color redColor = new Color(1f, 0f, 0.07f, 0.75f);

	private Color blueColor = new Color(0f, 0.5f, 1f, 0.75f);

	private Color orangeColor = new Color(1f, 0.6f, 0f, 0.75f);

	private Color blackColor = new Color(0f, 0f, 0f, 0.5f);

	private Color purpleColor = new Color(0.23f, 0.05f, 0.24f, 0.5f);

	public GameObject relatedCard;

	public int CardPlayerIndex = -1;

	public Transform mpMarks;

	public SpriteRenderer[] mpMark;

	public Transform portraits;

	public SpriteRenderer[] portrait;

	public Transform emotes;

	public SpriteRenderer emote0;

	public SpriteRenderer emote1;

	public SpriteRenderer emote2;

	public Transform emoteIcon;

	private float adjustForCardSize;

	private float adjustForCardHeight;

	public Transform itemBuyer;

	private string cardAlreadyHaveName;

	[SerializeField]
	private Transform crossIcon;

	private Dictionary<Renderer, int> originalOrders = new Dictionary<Renderer, int>();

	private List<Renderer> modifiedRenderers = new List<Renderer>();

	public string TargetTextTM
	{
		get
		{
			return targetTextTM.text;
		}
		set
		{
			targetTextTM.text = value;
		}
	}

	public CardData CardData
	{
		get
		{
			return cardData;
		}
		set
		{
			cardData = value;
		}
	}

	public ItemData ItemData
	{
		get
		{
			return itemData;
		}
		set
		{
			itemData = value;
		}
	}

	public string InternalId
	{
		get
		{
			return internalId;
		}
		set
		{
			internalId = value;
		}
	}

	public int TablePosition
	{
		get
		{
			return tablePosition;
		}
		set
		{
			tablePosition = value;
		}
	}

	public float CardSize
	{
		get
		{
			return cardSize;
		}
		set
		{
			cardSize = value;
		}
	}

	private void Awake()
	{
		Init();
	}

	private void Start()
	{
		transformVerticalDesplazament = Vector3.zero;
		adjustForCardSize = Globals.Instance.sizeW * 0.15f;
		adjustForCardHeight = Globals.Instance.sizeH * 0.25f;
	}

	public void Init()
	{
		floating = GetComponent<Floating>();
		cardCollider = GetComponent<BoxCollider2D>();
		if (cardSpriteT != null)
		{
			cardSpriteSR = cardSpriteT.GetComponent<SpriteRenderer>();
			skillImageSR = skillImageT.GetComponent<SpriteRenderer>();
			titleTextTM = titleTextT.GetComponent<TMP_Text>();
			titleTextTMBlue = titleTextTBlue.GetComponent<TMP_Text>();
			titleTextTMGold = titleTextTGold.GetComponent<TMP_Text>();
			titleTextTMRed = titleTextTRed.GetComponent<TMP_Text>();
			titleTextTMPurple = titleTextTPurple.GetComponent<TMP_Text>();
			descriptionTextTM = descriptionTextT.GetComponent<TMP_Text>();
			targetTextTM = targetTextT.GetComponent<TMP_Text>();
			requireTextTM = requireTextT.GetComponent<TMP_Text>();
			energyTextTM = energyText.GetComponent<TMP_Text>();
			energyBg = energyTextBg.GetComponent<SpriteRenderer>();
			typeTextTM = typeText.GetComponent<TMP_Text>();
			cardBorderSR = cardBorder.GetComponent<SpriteRenderer>();
			dissolveParticle = dissolveParticleT.GetComponent<ParticleSystem>();
			childRendererList = new List<Renderer>();
			foreach (Transform item in base.transform)
			{
				Renderer component = item.GetComponent<Renderer>();
				if (component != null)
				{
					component.sortingLayerName = "Cards";
					childRendererList.Add(component);
				}
				else
				{
					if (!(item.gameObject.name == "CardGO") && !(item.gameObject.name == "Emotes") && !(item.gameObject.name == "Key"))
					{
						continue;
					}
					foreach (Transform item2 in item)
					{
						Renderer component2 = item2.GetComponent<Renderer>();
						if (component2 != null)
						{
							component2.sortingLayerName = "Cards";
							childRendererList.Add(component2);
						}
					}
				}
			}
			if (MatchManager.Instance != null)
			{
				cursorArrow = MatchManager.Instance.cursorArrow;
			}
			if (backImageT.gameObject.activeSelf)
			{
				backImageT.gameObject.SetActive(value: false);
			}
			HideDifferences();
		}
		if (keyTransform != null && keyTransform.gameObject.activeSelf)
		{
			keyTransform.gameObject.SetActive(value: false);
		}
	}

	public void ChangeLayer()
	{
		Transform[] componentsInChildren = GetComponentsInChildren<Transform>(includeInactive: true);
		foreach (Transform transform in componentsInChildren)
		{
			Renderer component = transform.GetComponent<Renderer>();
			if (component != null)
			{
				if (transform.name == "TitleTextTab" || transform.name == "TypeTextTab" || transform.name == "TitleBlueText" || transform.name == "TitleGoldText" || transform.name == "TitlePurpleText")
				{
					component.sortingLayerName = "UI";
					component.sortingOrder = 99;
				}
				else
				{
					ApplyLayerChange(component, 40);
				}
			}
			else if (transform.name == "CardGO" || transform.name == "Emotes" || transform.name == "Key")
			{
				foreach (Transform item in transform)
				{
					Renderer component2 = item.GetComponent<Renderer>();
					if (component2 != null)
					{
						if (item.name == "TitleTextTab" || item.name == "TypeTextTab")
						{
							component2.sortingLayerName = "UI";
							component2.sortingOrder = 98;
						}
						else
						{
							ApplyLayerChange(component2, 55);
						}
					}
					TMP_SubMesh component3 = item.GetComponent<TMP_SubMesh>();
					if (component3 != null && component3.renderer != null)
					{
						if (item.name == "TitleTextTab" || item.name == "TypeTextTab")
						{
							component3.renderer.sortingLayerName = "UI";
							component3.renderer.sortingOrder = 99;
						}
						else
						{
							ApplyLayerChange(component3.renderer, 55);
						}
					}
				}
			}
			TMP_SubMesh component4 = transform.GetComponent<TMP_SubMesh>();
			if (component4 != null && component4.renderer != null)
			{
				if (transform.name == "TitleTextTab" || transform.name == "TypeTextTab")
				{
					component4.renderer.sortingLayerName = "UI";
					component4.renderer.sortingOrder = 99;
				}
				else
				{
					ApplyLayerChange(component4.renderer, 55);
				}
			}
			TMP_SubMesh[] componentsInChildren2 = transform.GetComponentsInChildren<TMP_SubMesh>(includeInactive: true);
			foreach (TMP_SubMesh tMP_SubMesh in componentsInChildren2)
			{
				if (tMP_SubMesh.renderer != null)
				{
					if (tMP_SubMesh.name == "TitleTextTab" || tMP_SubMesh.name == "TypeTextTab")
					{
						tMP_SubMesh.renderer.sortingLayerName = "UI";
						tMP_SubMesh.renderer.sortingOrder = 99;
					}
					else
					{
						ApplyLayerChange(tMP_SubMesh.renderer, 55);
					}
				}
			}
		}
	}

	private void ApplyLayerChange(Renderer renderer, int offset)
	{
		renderer.sortingLayerName = "UI";
		if (!originalOrders.ContainsKey(renderer))
		{
			originalOrders[renderer] = renderer.sortingOrder;
			renderer.sortingOrder = Mathf.Clamp(renderer.sortingOrder + offset, 0, 98);
		}
		if (!modifiedRenderers.Contains(renderer))
		{
			modifiedRenderers.Add(renderer);
		}
	}

	private void Update()
	{
		if (cardoutsidecombatamplified)
		{
			Vector3 vector = default(Vector3);
			vector.x = cardSize;
			vector.y = cardSize;
			vector.z = 1f;
			if (Mathf.Abs(base.transform.localScale.x - cardSize) > 0.005f)
			{
				base.transform.localScale = Vector3.Slerp(base.transform.localScale, vector, smooth);
			}
			else if (base.transform.localScale != vector)
			{
				base.transform.localScale = vector;
			}
			Vector3 vector2 = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
			Vector3 b = default(Vector3);
			if (cardoutsideselection)
			{
				b.x = vector2.x;
				b.y = vector2.y + 3.5f;
				b.z = 1f;
			}
			else if (cardoutsidelibary)
			{
				if (vector2.x > 0f)
				{
					b.x = vector2.x - 2f;
					b.y = vector2.y;
					b.z = 1f;
				}
				else
				{
					b.x = vector2.x + 2f;
					b.y = vector2.y;
					b.z = 1f;
				}
			}
			else if (cardFromEventTracker)
			{
				if (PopupManager.Instance.GetPopupT() != null)
				{
					b = PopupManager.Instance.GetPopupActiveCoordinates();
					b.y -= 1.7f;
				}
				else
				{
					b = vector2;
				}
			}
			else
			{
				b.x = vector2.x + 2f;
				b.y = vector2.y;
				b.z = 1f;
			}
			if (cardoutsideverticallist)
			{
				b.x = base.transform.localPosition.x;
				b.y -= 1.6f;
				b.z = 1f;
				if ((bool)ChallengeSelectionManager.Instance)
				{
					b.y += 2f;
				}
			}
			if (b.x + adjustForCardSize > Globals.Instance.sizeW * 0.5f)
			{
				b.x = Globals.Instance.sizeW * 0.5f - adjustForCardSize;
			}
			if (b.y + adjustForCardHeight > Globals.Instance.sizeH * 0.5f)
			{
				b.y = Globals.Instance.sizeH * 0.5f - adjustForCardHeight;
			}
			else if (b.y - adjustForCardHeight < (0f - Globals.Instance.sizeH) * 0.5f)
			{
				b.y = (0f - Globals.Instance.sizeH) * 0.5f + adjustForCardHeight;
			}
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, b, smooth);
		}
		else
		{
			if (cardoutsidelibary && TomeManager.Instance.IsActive())
			{
				return;
			}
			if (active || discard || cardnpc || cardrevealed || (MatchManager.Instance != null && MatchManager.Instance.GameStatus == "BeginTurnHero" && !cardfordisplay))
			{
				if (MatchManager.Instance != null && MatchManager.Instance.CardActiveT == base.transform)
				{
					if (Input.GetMouseButtonUp(1))
					{
						DoReturnCardToDeckFromDrag();
						return;
					}
					float num;
					float num2;
					Quaternion b2;
					if (canInstaCast == 1 && canEnergyCast == 1)
					{
						num = (0f - (mouseClickedPosition.x - Input.mousePosition.x)) * 0.004f;
						num2 = (mouseClickedPosition.y - Input.mousePosition.y) * 0.004f;
						b2 = Quaternion.Euler(0f, 0f, 0f);
					}
					else
					{
						num = (mouseClickedPosition.x - Input.mousePosition.x) * 0.0005f;
						num2 = (mouseClickedPosition.y - Input.mousePosition.y) * 0.0007f;
						float num3 = num * tiltAngle;
						float num4 = num2 * tiltAngle;
						if (num4 < -30f)
						{
							num4 = -30f;
						}
						if (num3 < -40f)
						{
							num3 = -40f;
						}
						if (num3 > 40f)
						{
							num3 = 40f;
						}
						b2 = Quaternion.Euler(num4, 0f, num3);
					}
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b2, smooth);
					base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, initialLocalPosition + transformVerticalDesplazament + new Vector3(num, 0f - num2, 0f) * 2f, smooth);
				}
				else
				{
					if (base.transform.localScale.x != cardSize)
					{
						if (Mathf.Abs(base.transform.localScale.x - cardSize) > 0.005f)
						{
							base.transform.localScale = Vector3.Slerp(base.transform.localScale, new Vector3(cardSize, cardSize, 1f), Time.deltaTime * 14f);
						}
						else
						{
							base.transform.localScale = new Vector3(cardSize, cardSize, 1f);
						}
					}
					if (!lockPosition && base.transform.localPosition != destinationLocalPosition)
					{
						if (Vector3.Distance(base.transform.localPosition, destinationLocalPosition) > 0.005f)
						{
							if (!casting)
							{
								if (!cardnpc)
								{
									base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, destinationLocalPosition, Time.deltaTime * 14f);
								}
								else
								{
									base.transform.localPosition = Vector3.Slerp(base.transform.localPosition, destinationLocalPosition, Time.deltaTime * 14f);
								}
							}
							else
							{
								base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, destinationLocalPosition, Time.deltaTime * 10f);
							}
						}
						else
						{
							base.transform.localPosition = destinationLocalPosition;
							if (cardforaddcard || cardfordiscard)
							{
								active = false;
							}
							if (destroyAtLocation)
							{
								UnityEngine.Object.Destroy(base.gameObject);
							}
						}
					}
					if (base.transform.rotation != destinationLocalRotation)
					{
						base.transform.rotation = Quaternion.Slerp(base.transform.rotation, destinationLocalRotation, Time.deltaTime * 14f);
					}
					if (MatchManager.Instance != null && discard && Vector3.Distance(base.transform.localPosition, MatchManager.Instance.GetDiscardPilePosition()) < 0.01f)
					{
						discard = false;
						base.transform.localPosition = MatchManager.Instance.GetDiscardPilePosition();
						base.transform.rotation = destinationLocalRotation;
						base.enabled = false;
					}
				}
			}
			if ((bool)MatchManager.Instance && MatchManager.Instance.controllerClickedCard && MatchManager.Instance.CardActiveT == base.transform)
			{
				OnMouseDrag();
			}
		}
	}

	public void ShowSpriteOverCard()
	{
		if (itemData != null && itemData.SpriteBossDrop != null)
		{
			portraitDrop.transform.gameObject.SetActive(value: true);
			portraitDrop.sprite = itemData.SpriteBossDrop;
		}
	}

	private void CardChildSorting(string _layerName = "Cards", int _position = 0, int _offset = 100)
	{
		if (childRendererList == null)
		{
			return;
		}
		int num = _offset;
		for (int i = 0; i < childRendererList.Count; i++)
		{
			if (this == null)
			{
				break;
			}
			if (base.gameObject == null)
			{
				break;
			}
			if (_layerName != "")
			{
				childRendererList[i].sortingLayerName = _layerName;
			}
			childRendererList[i].sortingOrder = _position + num;
			if (childRendererList[i].transform.childCount > 0 && childRendererList[i].transform.GetComponent<MeshRenderer>() != null)
			{
				foreach (Transform item in childRendererList[i].transform)
				{
					MeshRenderer component = item.GetComponent<MeshRenderer>();
					if (component != null)
					{
						if (_layerName != "")
						{
							component.sortingLayerName = _layerName;
						}
						component.sortingOrder = _position + num;
					}
					ParticleSystemRenderer component2 = item.GetComponent<ParticleSystemRenderer>();
					if (component2 != null)
					{
						if (_layerName != "")
						{
							component2.sortingLayerName = _layerName;
						}
						component2.sortingOrder = _position + num - 1;
					}
				}
			}
			if (cardData != null && childRendererList[i].name == "EnergyItemBg" && childRendererList[i].gameObject.activeSelf)
			{
				if (iconTSpriteRenderer == null)
				{
					if (cardData.CardType == Enums.CardType.Weapon)
					{
						iconTSpriteRenderer = childRendererList[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
					}
					else if (cardData.CardType == Enums.CardType.Armor)
					{
						iconTSpriteRenderer = childRendererList[i].transform.GetChild(1).GetComponent<SpriteRenderer>();
					}
					else if (cardData.CardType == Enums.CardType.Jewelry)
					{
						iconTSpriteRenderer = childRendererList[i].transform.GetChild(2).GetComponent<SpriteRenderer>();
					}
					else if (cardData.CardType == Enums.CardType.Pet)
					{
						iconTSpriteRenderer = childRendererList[i].transform.GetChild(4).GetComponent<SpriteRenderer>();
					}
					else
					{
						iconTSpriteRenderer = childRendererList[i].transform.GetChild(3).GetComponent<SpriteRenderer>();
					}
				}
				SpriteRenderer component3 = childRendererList[i].transform.GetComponent<SpriteRenderer>();
				iconTSpriteRenderer.sortingOrder = component3.sortingOrder + 1;
				iconTSpriteRenderer.sortingLayerName = component3.sortingLayerName;
			}
			num--;
		}
	}

	public void DefaultElementsLayeringOrder(int auxPosition = 20)
	{
		if (!discard)
		{
			int num = 100;
			int position = ((auxPosition != -1) ? (auxPosition + num * tablePosition) : (-20 * -MatchManager.Instance.CountHeroDiscard()));
			CardChildSorting("Cards", position, num);
		}
	}

	public void TopLayeringOrder(string layerName = "Cards", int _position = 3000)
	{
		int offset = 100;
		CardChildSorting(layerName, _position, offset);
	}

	public IEnumerator SelfDestruct(float delay = 0f)
	{
		yield return Globals.Instance.WaitForSeconds(0.2f);
		DisableTrail();
		HideKeyNotes();
		active = false;
		discard = false;
		base.transform.localPosition += new Vector3(0f, 0f, 1000f);
		yield return Globals.Instance.WaitForSeconds(delay);
		if ((bool)MatchManager.Instance)
		{
			base.enabled = false;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void CreateColliderAdjusted(float sizeX = 0f, float sizeY = 0f)
	{
		if (!(cardCollider == null))
		{
			cardCollider.enabled = true;
			Sprite sprite = backImageT.GetComponent<SpriteRenderer>().sprite;
			cardCollider.size = new Vector2(sprite.bounds.size.x + sizeX, sprite.bounds.size.y + sizeY);
			cardCollider.offset = new Vector2(0f, 0f);
		}
	}

	public void CreateColliderHand()
	{
		cardCollider.enabled = true;
		Sprite sprite = backImageT.GetComponent<SpriteRenderer>().sprite;
		cardCollider.size = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y * 1.3f);
		cardCollider.offset = new Vector2(0f, (0f - (sprite.bounds.size.y * 1.3f - sprite.bounds.size.y)) * 0.5f);
	}

	public void DisableCollider()
	{
		if (cardCollider != null)
		{
			cardCollider.enabled = false;
		}
	}

	public void EnableCollider()
	{
		if (cardCollider != null)
		{
			cardCollider.enabled = true;
		}
	}

	public CardData GetCardData()
	{
		return cardData;
	}

	public void DoTome(bool showName, int position, int total)
	{
		if (PlayerManager.Instance.IsCardUnlocked(internalId))
		{
			numberTome.gameObject.SetActive(value: true);
			numberTomeBg.gameObject.SetActive(value: true);
			numberTome.text = position.ToString();
			return;
		}
		if (numberTome.gameObject.activeSelf)
		{
			numberTome.gameObject.SetActive(value: false);
		}
		if (numberTomeBg.gameObject.activeSelf)
		{
			numberTomeBg.gameObject.SetActive(value: false);
		}
	}

	public void ShowSold(bool _state)
	{
		if (soldT.gameObject.activeSelf == _state)
		{
			return;
		}
		soldT.gameObject.SetActive(_state);
		if (GameManager.Instance.IsSingularity() && CardCraftManager.Instance != null && CardData.CardClass != Enums.CardClass.Item)
		{
			if (_state)
			{
				singularityT.gameObject.SetActive(value: true);
				soldT.gameObject.SetActive(value: false);
			}
			else
			{
				singularityT.gameObject.SetActive(value: false);
			}
		}
	}

	public void ShowSingularity(bool _state)
	{
		if (singularityT.gameObject.activeSelf != _state)
		{
			singularityT.gameObject.SetActive(_state);
		}
	}

	public void ShowLock()
	{
		if (!lockImage.gameObject.activeSelf)
		{
			lockImage.gameObject.SetActive(value: true);
		}
		if (!lockShadowImage.gameObject.activeSelf)
		{
			lockShadowImage.gameObject.SetActive(value: true);
		}
	}

	public void HideLock()
	{
		if (lockImage.gameObject.activeSelf)
		{
			lockImage.gameObject.SetActive(value: false);
		}
		if (lockShadowImage.gameObject.activeSelf)
		{
			lockShadowImage.gameObject.SetActive(value: false);
		}
	}

	public void ShowRelated()
	{
		cardRelatedT.gameObject.SetActive(value: true);
		cardRelatedBg.sortingOrder = cardRelatedT.GetComponent<Renderer>().sortingOrder - 1;
		cardRelatedBg.sortingLayerName = cardRelatedT.GetComponent<Renderer>().sortingLayerName;
	}

	public void RemoveCardData()
	{
		cardData = null;
	}

	public void SetCardback(Hero theHero)
	{
		string cardbackUsed = theHero.CardbackUsed;
		if (!(cardbackUsed != ""))
		{
			return;
		}
		CardbackData cardbackData = Globals.Instance.GetCardbackData(cardbackUsed);
		if (cardbackData == null)
		{
			cardbackData = Globals.Instance.GetCardbackData(Globals.Instance.GetCardbackBaseIdBySubclass(theHero.HeroData.HeroSubClass.Id));
			if (cardbackData == null)
			{
				cardbackData = Globals.Instance.GetCardbackData("defaultCardback");
			}
		}
		Sprite cardbackSprite = cardbackData.CardbackSprite;
		if (cardbackSprite != null)
		{
			backImageT.GetComponent<SpriteRenderer>().sprite = cardbackSprite;
		}
	}

	public void SetCard(string id, bool deckScale = true, Hero _theHero = null, NPC _theNPC = null, bool GetFromGlobal = false, bool _generated = false)
	{
		iconTSpriteRenderer = null;
		if (_theHero != null)
		{
			SetCardback(_theHero);
		}
		if (descriptionTextTM != null)
		{
			descriptionTextTM.fontSizeMax = 1.8f;
		}
		if (rightClick != null && rightClick.gameObject.activeSelf)
		{
			rightClick.gameObject.SetActive(value: false);
		}
		if (soldT != null && soldT.gameObject.activeSelf)
		{
			soldT.gameObject.SetActive(value: false);
		}
		if (singularityT != null && singularityT.gameObject.activeSelf)
		{
			singularityT.gameObject.SetActive(value: false);
		}
		if (MatchManager.Instance != null && !GetFromGlobal)
		{
			cardData = MatchManager.Instance.GetCardData(id, createInDict: false);
			if (cardData == null)
			{
				id = MatchManager.Instance.CreateCardInDictionary(id);
				cardData = MatchManager.Instance.GetCardData(id, createInDict: false);
			}
		}
		else
		{
			cardData = Globals.Instance.GetCardData(id);
		}
		if (cardData == null)
		{
			return;
		}
		if (MatchManager.Instance != null && _theHero != null)
		{
			Enums.DamageType enchantModifiedDamageType = _theHero.GetEnchantModifiedDamageType();
			cardData.ModifyDamageType(enchantModifiedDamageType, _theHero);
		}
		cardData.InternalId = (internalId = id);
		if (lockImage != null)
		{
			if (GameManager.Instance.IsObeliskChallenge() || PlayerManager.Instance.IsCardUnlocked(id) || (cardData.Item != null && cardData.Item.QuestItem) || SceneStatic.GetSceneName() == "CardPlayer" || !cardData.ShowInTome)
			{
				if (lockImage.gameObject.activeSelf)
				{
					lockImage.gameObject.SetActive(value: false);
				}
			}
			else if (!lockImage.gameObject.activeSelf)
			{
				lockImage.gameObject.SetActive(value: true);
			}
		}
		cardData.Visible = false;
		if (_theHero != null)
		{
			RedrawDescriptionPrecalculated(_theHero);
			theHero = _theHero;
		}
		else if (_theNPC != null)
		{
			RedrawDescriptionPrecalculatedNPC(_theNPC);
			theNPC = _theNPC;
		}
		if (cardSpriteSR == null)
		{
			return;
		}
		cardSpriteSR.sprite = GameManager.Instance.cardSprites[(int)cardData.CardClass];
		skillImageSR.sprite = cardData.Sprite;
		if (!cardData.FlipSprite)
		{
			skillImageSR.flipX = false;
		}
		else if (cardData.FlipSprite)
		{
			skillImageSR.flipX = true;
		}
		itemData = null;
		if (skillRare != null && skillRare.gameObject.activeSelf)
		{
			skillRare.gameObject.SetActive(value: false);
		}
		if (portraitDrop != null && portraitDrop.gameObject.activeSelf)
		{
			portraitDrop.transform.gameObject.SetActive(value: false);
		}
		if (cardData.CardUpgraded == Enums.CardUpgraded.No)
		{
			if (!titleTextT.gameObject.activeSelf)
			{
				titleTextT.gameObject.SetActive(value: true);
			}
			titleTextTM.text = cardData.CardName;
			if (titleTextTBlue.gameObject.activeSelf)
			{
				titleTextTBlue.gameObject.SetActive(value: false);
			}
			if (titleTextTGold.gameObject.activeSelf)
			{
				titleTextTGold.gameObject.SetActive(value: false);
			}
			if (titleTextTRed.gameObject.activeSelf)
			{
				titleTextTRed.gameObject.SetActive(value: false);
			}
			if (titleTextTPurple.gameObject.activeSelf)
			{
				titleTextTPurple.gameObject.SetActive(value: false);
			}
		}
		else if (cardData.CardUpgraded == Enums.CardUpgraded.A)
		{
			if (!titleTextTBlue.gameObject.activeSelf)
			{
				titleTextTBlue.gameObject.SetActive(value: true);
			}
			if (cardData.UpgradedFrom.Trim() != "")
			{
				if (Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false) != null)
				{
					titleTextTMBlue.text = cardData.CardName;
				}
				else
				{
					titleTextTMBlue.text = "";
				}
			}
			else
			{
				titleTextTMBlue.text = "";
			}
			if (titleTextT.gameObject.activeSelf)
			{
				titleTextT.gameObject.SetActive(value: false);
			}
			if (titleTextTGold.gameObject.activeSelf)
			{
				titleTextTGold.gameObject.SetActive(value: false);
			}
			if (titleTextTRed.gameObject.activeSelf)
			{
				titleTextTRed.gameObject.SetActive(value: false);
			}
			if (titleTextTPurple.gameObject.activeSelf)
			{
				titleTextTPurple.gameObject.SetActive(value: false);
			}
		}
		else if (cardData.CardUpgraded == Enums.CardUpgraded.B)
		{
			if (!titleTextTGold.gameObject.activeSelf)
			{
				titleTextTGold.gameObject.SetActive(value: true);
			}
			if (cardData.UpgradedFrom.Trim() != "")
			{
				if (Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false) != null)
				{
					titleTextTMGold.text = cardData.CardName;
				}
				else
				{
					titleTextTMGold.text = "";
				}
			}
			else
			{
				titleTextTMGold.text = "";
			}
			if (titleTextT.gameObject.activeSelf)
			{
				titleTextT.gameObject.SetActive(value: false);
			}
			if (titleTextTBlue.gameObject.activeSelf)
			{
				titleTextTBlue.gameObject.SetActive(value: false);
			}
			if (titleTextTRed.gameObject.activeSelf)
			{
				titleTextTRed.gameObject.SetActive(value: false);
			}
			if (titleTextTPurple.gameObject.activeSelf)
			{
				titleTextTPurple.gameObject.SetActive(value: false);
			}
		}
		else if (cardData.CardUpgraded == Enums.CardUpgraded.Rare)
		{
			if (!titleTextTPurple.gameObject.activeSelf)
			{
				titleTextTPurple.gameObject.SetActive(value: true);
			}
			titleTextTMPurple.text = cardData.CardName;
			if (titleTextT.gameObject.activeSelf)
			{
				titleTextT.gameObject.SetActive(value: false);
			}
			if (titleTextTGold.gameObject.activeSelf)
			{
				titleTextTGold.gameObject.SetActive(value: false);
			}
			if (titleTextTBlue.gameObject.activeSelf)
			{
				titleTextTBlue.gameObject.SetActive(value: false);
			}
			if (titleTextTRed.gameObject.activeSelf)
			{
				titleTextTRed.gameObject.SetActive(value: false);
			}
		}
		if (cardData.Innate)
		{
			if (!innateIconParticle.gameObject.activeSelf)
			{
				innateIconParticle.gameObject.SetActive(value: true);
			}
			if (!innateT.gameObject.activeSelf)
			{
				innateT.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (innateIconParticle.gameObject.activeSelf)
			{
				innateIconParticle.gameObject.SetActive(value: false);
			}
			if (innateT.gameObject.activeSelf)
			{
				innateT.gameObject.SetActive(value: false);
			}
		}
		if (cardData.Vanish)
		{
			SetVanishIcon(state: true);
		}
		else
		{
			SetVanishIcon(state: false);
		}
		ShowRarity(cardData);
		if (cardData.CardRarity == Enums.CardRarity.Mythic)
		{
			if (!cardBorderMythic.gameObject.activeSelf)
			{
				cardBorderMythic.gameObject.SetActive(value: true);
			}
		}
		else if (cardBorderMythic.gameObject.activeSelf)
		{
			cardBorderMythic.gameObject.SetActive(value: false);
		}
		descriptionTextTM.text = cardData.DescriptionNormalized;
		NormalizeHeight(descriptionTextTM, cardData.Item);
		if (cardData.CardClass != Enums.CardClass.Item)
		{
			if (!targetTextT.gameObject.activeSelf)
			{
				targetTextT.gameObject.SetActive(value: true);
			}
			if (!targetT.gameObject.activeSelf)
			{
				targetT.gameObject.SetActive(value: true);
			}
			if (!requireTextT.gameObject.activeSelf)
			{
				requireTextT.gameObject.SetActive(value: true);
			}
			if (!typeText.gameObject.activeSelf)
			{
				typeText.gameObject.SetActive(value: true);
			}
			if (!typeTextImage.gameObject.activeSelf)
			{
				typeTextImage.gameObject.SetActive(value: true);
			}
			targetTextTM.text = cardData.Target;
		}
		else
		{
			if (targetTextT.gameObject.activeSelf)
			{
				targetTextT.gameObject.SetActive(value: false);
			}
			if (targetT.gameObject.activeSelf)
			{
				targetT.gameObject.SetActive(value: false);
			}
			if (requireTextT.gameObject.activeSelf)
			{
				requireTextT.gameObject.SetActive(value: false);
			}
			if (typeText.gameObject.activeSelf)
			{
				typeText.gameObject.SetActive(value: false);
			}
			if (typeTextImage.gameObject.activeSelf)
			{
				typeTextImage.gameObject.SetActive(value: false);
			}
			itemData = cardData.Item;
			if (itemData != null && itemData.CursedItem)
			{
				if (titleTextT.gameObject.activeSelf)
				{
					titleTextT.gameObject.SetActive(value: false);
				}
				if (titleTextTPurple.gameObject.activeSelf)
				{
					titleTextTPurple.gameObject.SetActive(value: false);
				}
				if (!titleTextTRed.gameObject.activeSelf)
				{
					titleTextTRed.gameObject.SetActive(value: true);
				}
				titleTextTMRed.text = cardData.CardName;
			}
		}
		string[] array = cardData.Id.Split('_');
		if (array != null && array[0] == "success")
		{
			if (targetTextT.gameObject.activeSelf)
			{
				targetTextT.gameObject.SetActive(value: false);
			}
			if (targetT.gameObject.activeSelf)
			{
				targetT.gameObject.SetActive(value: false);
			}
		}
		if (cardData.CardType != Enums.CardType.None && cardData.CardClass != Enums.CardClass.Item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), cardData.CardType)));
			if (cardData.CardTypeAux.Length != 0)
			{
				stringBuilder.Append(" <size=-.2>[</size><size=+.1>...</size><size=-.2>]</size>");
			}
			typeTextTM.text = stringBuilder.ToString();
			typeTextImage.gameObject.SetActive(value: true);
		}
		else
		{
			typeTextTM.text = "";
			if (typeTextImage.gameObject.activeSelf)
			{
				typeTextImage.gameObject.SetActive(value: false);
			}
		}
		if (cardData.CardClass != Enums.CardClass.Item)
		{
			string text = "";
			bool flag = false;
			if (cardData.CardClass != Enums.CardClass.Monster)
			{
				text = cardData.GetRequireText();
				if (text != "")
				{
					flag = true;
				}
			}
			else
			{
				if (_theNPC != null)
				{
					text = _theNPC.GetCardPriorityText(cardData.Id);
				}
				else if (theNPC != null)
				{
					text = theNPC.GetCardPriorityText(cardData.Id);
				}
				if (text != "")
				{
					requireTextTM.color = Functions.HexToColor("#5E0016");
					flag = true;
				}
			}
			requireTextTM.text = text;
			if (!flag)
			{
				descriptionTextTM.margin = new Vector4(0.02f, 0.16f, 0.02f, -0f);
				if (cardData.CardClass == Enums.CardClass.Monster)
				{
					descriptionTextTM.margin = new Vector4(0.02f, 0.16f, 0.02f, -0.02f);
				}
				else if (cardData.CardType == Enums.CardType.Enchantment)
				{
					descriptionTextTM.margin = new Vector4(0.02f, 0.14f, 0.02f, -0.02f);
				}
			}
			else
			{
				descriptionTextTM.margin = new Vector4(0.02f, 0.28f, 0.02f, 0f);
				if (cardData.CardType == Enums.CardType.Enchantment)
				{
					descriptionTextTM.margin = new Vector4(0.02f, 0.28f, 0.02f, -0.02f);
				}
			}
		}
		else
		{
			descriptionTextTM.margin = new Vector4(0.02f, 0f, 0.02f, -0.04f);
		}
		if (energyTextItemBg.gameObject.activeSelf)
		{
			energyTextItemBg.gameObject.SetActive(value: false);
		}
		if (cardData.Playable)
		{
			if (_generated && _theHero != null && _theHero.HasEffect("Exhaust"))
			{
				cardData.DoExhaust();
			}
			DrawEnergyCost();
		}
		else
		{
			if (energyText.gameObject.activeSelf)
			{
				energyText.gameObject.SetActive(value: false);
			}
			if (energyTextBg.gameObject.activeSelf)
			{
				energyTextBg.gameObject.SetActive(value: false);
			}
			if (cardData.CardClass == Enums.CardClass.Item)
			{
				if (!energyTextItemBg.gameObject.activeSelf)
				{
					energyTextItemBg.gameObject.SetActive(value: true);
				}
				for (int i = 0; i < 5; i++)
				{
					if (energyTextItemBg.GetChild(i).gameObject.activeSelf)
					{
						energyTextItemBg.GetChild(i).gameObject.SetActive(value: false);
					}
				}
				if (cardData.CardType == Enums.CardType.Weapon)
				{
					if (!energyTextItemBg.GetChild(0).gameObject.activeSelf)
					{
						energyTextItemBg.GetChild(0).gameObject.SetActive(value: true);
					}
				}
				else if (cardData.CardType == Enums.CardType.Armor)
				{
					if (!energyTextItemBg.GetChild(1).gameObject.activeSelf)
					{
						energyTextItemBg.GetChild(1).gameObject.SetActive(value: true);
					}
				}
				else if (cardData.CardType == Enums.CardType.Jewelry)
				{
					if (!energyTextItemBg.GetChild(2).gameObject.activeSelf)
					{
						energyTextItemBg.GetChild(2).gameObject.SetActive(value: true);
					}
				}
				else if (cardData.CardType == Enums.CardType.Pet)
				{
					if (!energyTextItemBg.GetChild(4).gameObject.activeSelf)
					{
						energyTextItemBg.GetChild(4).gameObject.SetActive(value: true);
					}
				}
				else if (!energyTextItemBg.GetChild(3).gameObject.activeSelf)
				{
					energyTextItemBg.GetChild(3).gameObject.SetActive(value: true);
				}
			}
		}
		if (deckScale)
		{
			base.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
		}
		if (cardData.CardType == Enums.CardType.Corruption)
		{
			if (targetTextT.gameObject.activeSelf)
			{
				targetTextT.gameObject.SetActive(value: false);
			}
			if (targetT.gameObject.activeSelf)
			{
				targetT.gameObject.SetActive(value: false);
			}
			if (requireTextT.gameObject.activeSelf)
			{
				requireTextT.gameObject.SetActive(value: false);
			}
			if (typeText.gameObject.activeSelf)
			{
				typeText.gameObject.SetActive(value: false);
			}
			if (typeTextImage.gameObject.activeSelf)
			{
				typeTextImage.gameObject.SetActive(value: false);
			}
			descriptionTextT.localPosition = new Vector3(descriptionTextT.localPosition.x, -0.63f, descriptionTextT.localPosition.z);
			descriptionTextTM.margin = new Vector4(0.02f, -0.02f, 0.02f, -0.04f);
		}
		if (SceneManager.GetActiveScene().name != "HeroSelection")
		{
			CreateColliderHand();
		}
		if (descriptionTextTM != null)
		{
			string text2 = Functions.StripTagsString(descriptionTextTM.text);
			if (text2.Length < 20)
			{
				descriptionTextTM.fontSizeMax = 1.8f;
			}
			else if (text2.Length < 50)
			{
				descriptionTextTM.fontSizeMax = 1.6f;
			}
			else
			{
				descriptionTextTM.fontSizeMax = 1.45f;
			}
		}
	}

	public void ShowRarity(CardData _cardData)
	{
		if (rarityUncommonParticle.gameObject.activeSelf)
		{
			rarityUncommonParticle.gameObject.SetActive(value: false);
		}
		if (rarityRareParticle.gameObject.activeSelf)
		{
			rarityRareParticle.gameObject.SetActive(value: false);
		}
		if (rarityEpicParticle.gameObject.activeSelf)
		{
			rarityEpicParticle.gameObject.SetActive(value: false);
		}
		if (rarityMythicParticle.gameObject.activeSelf)
		{
			rarityMythicParticle.gameObject.SetActive(value: false);
		}
		if (_cardData.CardRarity == Enums.CardRarity.Common)
		{
			borderSprite.sprite = GameManager.Instance.cardBorderSprites[0];
			energyBg.sprite = GameManager.Instance.cardEnergySprites[0];
		}
		else if (_cardData.CardRarity == Enums.CardRarity.Uncommon)
		{
			borderSprite.sprite = GameManager.Instance.cardBorderSprites[1];
			energyBg.sprite = GameManager.Instance.cardEnergySprites[1];
			rarityUncommonParticle.gameObject.SetActive(value: true);
		}
		else if (_cardData.CardRarity == Enums.CardRarity.Rare)
		{
			energyBg.sprite = GameManager.Instance.cardEnergySprites[2];
			borderSprite.sprite = GameManager.Instance.cardBorderSprites[2];
			rarityRareParticle.gameObject.SetActive(value: true);
		}
		else if (_cardData.CardRarity == Enums.CardRarity.Epic)
		{
			energyBg.sprite = GameManager.Instance.cardEnergySprites[3];
			borderSprite.sprite = GameManager.Instance.cardBorderSprites[3];
			rarityEpicParticle.gameObject.SetActive(value: true);
		}
		else if (_cardData.CardRarity == Enums.CardRarity.Mythic)
		{
			energyBg.sprite = GameManager.Instance.cardEnergySprites[4];
			borderSprite.sprite = GameManager.Instance.cardBorderSprites[4];
			rarityMythicParticle.gameObject.SetActive(value: true);
		}
	}

	public void HideRarityParticles()
	{
		if (!(cardData != null))
		{
			return;
		}
		if (cardData.CardRarity == Enums.CardRarity.Uncommon)
		{
			if (rarityUncommonParticle.gameObject.activeSelf)
			{
				rarityUncommonParticle.gameObject.SetActive(value: false);
			}
		}
		else if (cardData.CardRarity == Enums.CardRarity.Rare)
		{
			if (rarityRareParticle.gameObject.activeSelf)
			{
				rarityRareParticle.gameObject.SetActive(value: false);
			}
		}
		else if (cardData.CardRarity == Enums.CardRarity.Epic)
		{
			if (rarityEpicParticle.gameObject.activeSelf)
			{
				rarityEpicParticle.gameObject.SetActive(value: false);
			}
		}
		else if (cardData.CardRarity == Enums.CardRarity.Mythic && rarityMythicParticle.gameObject.activeSelf)
		{
			rarityMythicParticle.gameObject.SetActive(value: false);
		}
	}

	public void DoReward(bool fromReward = true, bool fromEvent = false, bool fromLoot = false, bool selectable = true, float modspeed = 1f)
	{
		ShowBackImage(state: true);
		SetLocalScale(new Vector3(0f, 0f, 1f));
		active = true;
		cardoutsidecombat = true;
		cardoutsidereward = true;
		TopLayeringOrder("Book");
		HideRarityParticles();
		HideCardIconParticles();
		SetDestinationScaleRotation(base.transform.localPosition, 0.8f, base.transform.localRotation);
		if ((fromReward || fromLoot) && cardData.CardRarity != Enums.CardRarity.Common)
		{
			Color color;
			if (cardData.CardRarity == Enums.CardRarity.Uncommon)
			{
				color = Globals.Instance.RarityColor["uncommon"];
				color = new Color(color.r, color.g, color.b, 0.1f);
			}
			else if (cardData.CardRarity == Enums.CardRarity.Rare)
			{
				color = Globals.Instance.RarityColor["rare"];
				color = new Color(color.r, color.g, color.b, 0.3f);
			}
			else if (cardData.CardRarity == Enums.CardRarity.Epic)
			{
				color = Globals.Instance.RarityColor["epic"];
				color = new Color(color.r, color.g, color.b, 0.6f);
			}
			else
			{
				color = Globals.Instance.RarityColor["mythic"];
				color = new Color(color.r, color.g, color.b, 0.7f);
			}
			ParticleSystem.MainModule main = backParticle.main;
			main.startColor = color;
			main = backParticleSpark.main;
			main.startColor = color;
			backParticleT.gameObject.SetActive(value: true);
			backParticle.Play();
			backParticleSpark.Play();
		}
		StartCoroutine(DoRewardCo(fromReward, fromEvent, fromLoot, selectable, modspeed));
	}

	public void SetVanishIcon(bool state)
	{
		vanishIconParticle.gameObject.SetActive(state);
		vanishT.gameObject.SetActive(state);
	}

	private IEnumerator DoRewardCo(bool fromReward, bool fromEvent, bool fromLoot, bool selectable, float modspeed)
	{
		if (fromEvent)
		{
			GameManager.Instance.PlayLibraryAudio("dealcard");
		}
		yield return Globals.Instance.WaitForSeconds(0.2f * modspeed);
		if (fromReward)
		{
			if (cardData.CardUpgraded != Enums.CardUpgraded.No)
			{
				GameManager.Instance.PlayLibraryAudio("ui_cardupgrade");
				cardUpgradeT.gameObject.SetActive(value: true);
			}
			yield return Globals.Instance.WaitForSeconds(1.6f * modspeed);
		}
		if (fromEvent)
		{
			yield return Globals.Instance.WaitForSeconds(1.8f * modspeed);
		}
		active = false;
		ShowBackImage(state: false);
		DisableTrail();
		backImageT.gameObject.SetActive(value: true);
		backImageT.GetComponent<Animator>().enabled = true;
		cardElementsT.GetComponent<Animator>().enabled = true;
		if (fromReward || fromLoot)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f * modspeed);
			_ = cardData.CardRarity;
			GameManager.Instance.PlayLibraryAudio("castnpccard");
		}
		if (RewardsManager.Instance != null)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			CreateColliderAdjusted();
			if (GameManager.Instance.IsSingularity())
			{
				CardData cardDataFromCardData = Functions.GetCardDataFromCardData(cardData, "");
				for (int i = 0; i < theHero.Cards.Count; i++)
				{
					CardData card = Globals.Instance.GetCardData(theHero.Cards[i], instantiate: false);
					if (Functions.GetCardDataFromCardData(card, "").Id == cardDataFromCardData.Id)
					{
						ShowSingularity(_state: true);
						StablishAlreadyHaveAVersionText(card);
						break;
					}
				}
			}
		}
		else if (!(EventManager.Instance != null) || !(base.gameObject.name == "EventRollCard"))
		{
			yield return Globals.Instance.WaitForSeconds(0.2f);
			CreateColliderAdjusted();
		}
		if (fromEvent)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f * modspeed);
			PlayDissolveParticle();
		}
		if (!selectable)
		{
			DisableCollider();
		}
	}

	public void TurnBack()
	{
		cardElementsT.GetComponent<Animator>().enabled = true;
		cardElementsT.GetComponent<Animator>().SetTrigger("turnBack");
		backImageT.GetComponent<Animator>().enabled = true;
		backImageT.GetComponent<Animator>().SetTrigger("turnBack");
		DisableCollider();
		if (backParticleT.gameObject.activeSelf)
		{
			backParticleT.gameObject.SetActive(value: false);
		}
		StartCoroutine(TurnBackCo());
	}

	private IEnumerator TurnBackCo()
	{
		yield return Globals.Instance.WaitForSeconds(1f);
		ShowBackImage(state: true);
	}

	public void ShowUnlocked(bool showEffects = true)
	{
		HideLock();
		if (showEffects)
		{
			GameManager.Instance.PlayLibraryAudio("ui_cardupgrade", 0.05f);
			cardUnlockT.gameObject.SetActive(value: true);
			PlaySightParticle();
		}
	}

	public void ShowLockedBackground(bool status)
	{
		lockShadowImage.gameObject.SetActive(status);
	}

	public void HideCardIconParticles()
	{
		if (innateIconParticle.gameObject.activeSelf)
		{
			innateIconParticle.gameObject.SetActive(value: false);
		}
		if (vanishIconParticle.gameObject.activeSelf)
		{
			vanishIconParticle.gameObject.SetActive(value: false);
		}
	}

	public void ShowBackImage(bool state)
	{
		backImageT.gameObject.SetActive(state);
		cardElementsT.gameObject.SetActive(!state);
		trailParticlesNPC.gameObject.SetActive(!state);
		if (!state)
		{
			HideCardIconParticles();
			HideRarityParticles();
		}
	}

	public void ShowDisable(bool state)
	{
		if (disableT != null && disableT.gameObject.activeSelf != state)
		{
			disableT.gameObject.SetActive(state);
		}
	}

	public void ShowDisableReward()
	{
		disableT.gameObject.SetActive(value: true);
		Color color = disableT.GetComponent<SpriteRenderer>().color;
		disableT.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.35f);
	}

	public void StablishAlreadyHaveAVersionText(CardData _card)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (_card.CardUpgraded == Enums.CardUpgraded.No)
		{
			stringBuilder.Append("<color=#AAA>");
		}
		else
		{
			stringBuilder.Append(_card.ColorFromCardDataRarity(_card, _useLightVersion: true));
		}
		stringBuilder.Append(_card.CardName);
		stringBuilder.Append("</color>");
		cardAlreadyHaveName = stringBuilder.ToString();
		cardAlreadyInDeckText.text = string.Format(Texts.Instance.GetText("cardNameAlreadyInDeck"), stringBuilder.ToString());
	}

	public void ShowAlreadyHaveAVersion(bool state)
	{
		if (cardAlreadyInDeckT.gameObject.activeSelf != state)
		{
			cardAlreadyInDeckT.gameObject.SetActive(state);
		}
	}

	public void ShowDiscardImage()
	{
		backImageT.gameObject.SetActive(value: false);
		cardSpriteT.gameObject.SetActive(value: true);
		skillImageT.gameObject.SetActive(value: true);
		descriptionTextT.gameObject.SetActive(value: true);
		energyTextTM.gameObject.SetActive(value: false);
		energyTextBg.gameObject.SetActive(value: false);
		typeText.gameObject.SetActive(value: false);
		typeTextImage.gameObject.SetActive(value: false);
		trailParticlesNPC.gameObject.SetActive(value: false);
		titleTextT.gameObject.SetActive(value: false);
		titleTextTMBlue.gameObject.SetActive(value: false);
		titleTextTMGold.gameObject.SetActive(value: false);
	}

	public void RevealCard()
	{
		cardrevealed = true;
		cardSize = cardScaleNPC;
		RedrawDescriptionPrecalculatedNPC(theNPC);
		ShowBackImage(state: false);
		PlaySightParticle();
		IsDivineExecutionPlayable();
	}

	public void UnrevealCard()
	{
		cardrevealed = false;
		cardSize = cardScaleNPC;
		ShowBackImage(state: true);
		crossIcon.gameObject.SetActive(value: false);
	}

	public void IsDivineExecutionPlayable()
	{
		if (!IsRevealed() || crossIcon == null)
		{
			return;
		}
		if (cardData.BaseCard == "DivineExecutionMNp")
		{
			MatchManager instance = MatchManager.Instance;
			if ((object)instance == null || instance.NumNPCsAlive() >= 4)
			{
				MatchManager instance2 = MatchManager.Instance;
				if ((object)instance2 == null || !instance2.AnyNPCIsParalyzed())
				{
					goto IL_0069;
				}
			}
			crossIcon.gameObject.SetActive(value: true);
			return;
		}
		goto IL_0069;
		IL_0069:
		crossIcon.gameObject.SetActive(value: false);
	}

	public bool IsRevealed()
	{
		return cardrevealed;
	}

	public void SetTablePositionValues(int _position, int _totalCards)
	{
		tablePosition = _position;
		tableCards = _totalCards;
		if (!discard)
		{
			DefaultElementsLayeringOrder();
		}
	}

	public Vector3 GetDestination()
	{
		return destinationLocalPosition;
	}

	public void SetDestination(Vector3 position)
	{
		destinationLocalPosition = position;
	}

	public void SetLocalPosition(Vector3 position)
	{
		base.transform.localPosition = position;
	}

	public void SetDestinationScaleRotation(Vector3 position, float scale, Quaternion rotation)
	{
		destinationLocalPosition = position;
		cardSize = scale;
		destinationLocalRotation = rotation;
	}

	public void SetDestinationLocalScale(float scale)
	{
		cardSize = scale;
	}

	public void SetLocalScale(Vector3 scale)
	{
		base.transform.localScale = scale;
	}

	public void PositionCardInTable()
	{
		float num = (float)tableCards * 0.5f - 0.5f;
		float num2 = Mathf.Abs(num - (float)tablePosition);
		float num3 = 1.4f;
		num3 = cardSizeTable + 0.4f;
		int num4 = 5;
		if (tableCards > num4)
		{
			float num5 = (float)(tableCards - num4) * 0.14f;
			num3 -= num5;
		}
		float num6 = 0f;
		if (Mathf.Floor(num2) > 2f)
		{
			num6 = ((!((float)tablePosition < num)) ? (num6 + num2 * 0.5f) : (num6 - num2 * 0.5f));
		}
		float x = 1.2f + (0f - num3) * ((float)tableCards - (float)tablePosition * 1.1f);
		float y = -1f * (num2 * 0.05f + (num2 - 1f) * 0.01f * num2);
		if (MatchManager.Instance.GameStatus == "BeginTurnHero" || !discard)
		{
			Quaternion quaternion = Quaternion.Euler(0f, 0f, 0f);
			quaternion = Quaternion.Euler(0f, 0f, 3f * (num - (float)tablePosition) + num6);
			destinationLocalRotation = (initialLocalRotation = quaternion);
			destinationLocalPosition = (initialLocalPosition = new Vector3(x, y, 1f - (float)tablePosition * 0.01f));
			cardSize = cardSizeTable;
		}
		if (MatchManager.Instance.GameStatus != "BeginTurnHero" && !active && !discard)
		{
			active = true;
		}
		EnableCollider();
	}

	public void AmplifySetEnergy()
	{
		int position = 20000;
		int offset = 100;
		CardChildSorting("Default", position, offset);
		destinationLocalPosition = Vector3.zero - new Vector3(3.5f, 0.5f, 0f) - GameObject.Find("GOs/Hand").transform.localPosition;
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 0f);
		cardSize = 1.4f;
		HideEnergyBorder();
	}

	public void AmplifyForSelection(int index, int total)
	{
		int position = 20000;
		int offset = 100 * -index;
		CardChildSorting("Default", position, offset);
		CreateColliderAdjusted(0.2f, 0.15f);
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = vector;
		float y = vector.y;
		float num = vector.y + 1.6f;
		float num2 = vector.y - 1.6f;
		float num3 = 2.3f;
		int num4 = 0;
		if (total > 10)
		{
			num4 = Mathf.FloorToInt((float)index / 10f);
			total = 10;
			index %= 10;
		}
		num2 -= (float)num4 * 6.4f;
		num -= (float)num4 * 6.4f;
		switch (total)
		{
		case 2:
			vector2 = ((index != 0) ? new Vector3(vector.x + num3 * 0.5f, y) : new Vector3(vector.x - num3 * 0.5f, y));
			break;
		case 3:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - num3, y), 
				1 => vector, 
				_ => new Vector3(vector.x + num3, y), 
			};
			break;
		case 4:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - num3 * 0.5f, num), 
				1 => new Vector3(vector.x + num3 * 0.5f, num), 
				2 => new Vector3(vector.x - num3 * 0.5f, num2), 
				_ => new Vector3(vector.x + num3 * 0.5f, num2), 
			};
			break;
		case 5:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - num3, num), 
				1 => new Vector3(vector.x, num), 
				2 => new Vector3(vector.x + num3, num), 
				3 => new Vector3(vector.x - num3 * 0.5f, num2), 
				_ => new Vector3(vector.x + num3 * 0.5f, num2), 
			};
			break;
		case 6:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - 2.2f, num), 
				1 => new Vector3(vector.x, num), 
				2 => new Vector3(vector.x + 2.2f, num), 
				3 => new Vector3(vector.x - 2.2f, num2), 
				4 => new Vector3(vector.x, num2), 
				_ => new Vector3(vector.x + 2.2f, num2), 
			};
			break;
		case 7:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - (num3 * 0.5f + num3), num), 
				1 => new Vector3(vector.x - num3 * 0.5f, num), 
				2 => new Vector3(vector.x + num3 * 0.5f, num), 
				3 => new Vector3(vector.x + num3 * 0.5f + num3, num), 
				4 => new Vector3(vector.x - num3, num2), 
				5 => new Vector3(vector.x, num2), 
				_ => new Vector3(vector.x + num3, num2), 
			};
			break;
		case 8:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - (num3 * 0.5f + num3), num), 
				1 => new Vector3(vector.x - num3 * 0.5f, num), 
				2 => new Vector3(vector.x + num3 * 0.5f, num), 
				3 => new Vector3(vector.x + num3 * 0.5f + num3, num), 
				4 => new Vector3(vector.x - (num3 * 0.5f + num3), num2), 
				5 => new Vector3(vector.x - num3 * 0.5f, num2), 
				6 => new Vector3(vector.x + num3 * 0.5f, num2), 
				_ => new Vector3(vector.x + num3 * 0.5f + num3, num2), 
			};
			break;
		case 9:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - num3 * 2f, num), 
				1 => new Vector3(vector.x - num3, num), 
				2 => new Vector3(vector.x, num), 
				3 => new Vector3(vector.x + num3, num), 
				4 => new Vector3(vector.x + num3 * 2f, num), 
				5 => new Vector3(vector.x - (num3 * 0.5f + num3), num2), 
				6 => new Vector3(vector.x - num3 * 0.5f, num2), 
				7 => new Vector3(vector.x + num3 * 0.5f, num2), 
				_ => new Vector3(vector.x + num3 * 0.5f + num3, num2), 
			};
			break;
		case 10:
			vector2 = index switch
			{
				0 => new Vector3(vector.x - num3 * 2f, num), 
				1 => new Vector3(vector.x - num3, num), 
				2 => new Vector3(vector.x, num), 
				3 => new Vector3(vector.x + num3, num), 
				4 => new Vector3(vector.x + num3 * 2f, num), 
				5 => new Vector3(vector.x - num3 * 2f, num2), 
				6 => new Vector3(vector.x - num3, num2), 
				7 => new Vector3(vector.x, num2), 
				8 => new Vector3(vector.x + num3, num2), 
				_ => new Vector3(vector.x + num3 * 2f, num2), 
			};
			break;
		}
		vector2 = new Vector3(vector2.x, vector2.y, -3f + (float)index * -0.01f);
		destinationLocalPosition = vector2;
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 0f);
		cardSize = cardSizeTable;
		HideEnergyBorder();
		ShowDisable(state: false);
	}

	public void EnableDisableDiscardAction(bool state, bool iconSkull, bool graphicalAdjustments = true)
	{
		cardselectedfordiscard = state;
		if (!graphicalAdjustments)
		{
			return;
		}
		if (state)
		{
			if (iconSkull)
			{
				DrawSkull(state: true);
				DrawBorder("red");
			}
			else
			{
				DrawCheck(state: true);
				DrawBorder("green");
			}
		}
		else
		{
			DrawSkull(state: false);
			DrawCheck(state: false);
			DrawBorder("");
		}
	}

	public void EnableDisableAddcardAction(bool state, bool graphicalAdjustments = true)
	{
		cardselectedforaddcard = state;
		if (graphicalAdjustments)
		{
			if (state)
			{
				DrawBorder("green");
				DrawCheck(state: true);
			}
			else
			{
				DrawBorder("");
				DrawCheck(state: false);
				DrawSkull(state: false);
			}
		}
	}

	private IEnumerator OnStopFloating(Vector3 destination)
	{
		Vector3 position = base.transform.localPosition;
		while (Vector3.Distance(position, destination) > 0.1f)
		{
			position = base.transform.localPosition;
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		floating.enabled = true;
	}

	public void CenterToDiscard()
	{
		discard = true;
		active = false;
		trailParticlesNPC.gameObject.SetActive(value: true);
		int position = MatchManager.Instance.CountHeroDiscard() * 20 + 100;
		CardChildSorting("Default", position);
		DrawSkull(state: true);
		destinationLocalPosition = new Vector3(base.transform.localPosition.x, 2f, base.transform.localPosition.z);
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 0f);
	}

	public bool CardIsPrediscarding()
	{
		if (prediscard && discard)
		{
			return true;
		}
		return false;
	}

	public void PreDiscardCard()
	{
		RemoveEmotes();
		ShowKeyNum(_state: false);
		if (tablePosition != -1)
		{
			floating.enabled = false;
			DisableCollider();
			prediscard = true;
			discard = true;
			active = false;
			casting = false;
			base.transform.parent = MatchManager.Instance.tempTransform;
			destinationLocalPosition = base.transform.localPosition + (MatchManager.Instance.GetDiscardPileTransform().position - base.transform.position) + new Vector3(2.5f, 0f, 0f);
			destinationLocalRotation = Quaternion.Euler(0f, 0f, 0f);
			cardSize = 0.8f;
			int position = (MatchManager.Instance.CountHeroDiscard() + MatchManager.Instance.NumChildsInTemporal()) * 100;
			int offset = 100;
			CardChildSorting("Default", position, offset);
			DrawBorder("blue");
		}
	}

	public void MoveCardToDeckPile()
	{
		base.transform.parent = MatchManager.Instance.GetDeckPileTransform();
		destinationLocalPosition = MatchManager.Instance.GetDeckPilePosition();
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 180f);
		cardSize = 0f;
		EnableTrail();
		int num = (MatchManager.Instance.CountHeroDiscard() - tablePosition) * 100;
		if (num != 0)
		{
			int position = -30000 + num;
			int offset = 100;
			CardChildSorting("Cards", position, offset);
		}
		destroyAtLocation = true;
	}

	public void DiscardCard(bool discardedFromHand, Enums.CardPlace whereToDiscard = Enums.CardPlace.Discard, int auxIndex = -1)
	{
		if (base.gameObject == null || base.transform == null)
		{
			return;
		}
		RemoveEmotes();
		ShowKeyNum(_state: false);
		DisableCollider();
		if (tablePosition <= -1)
		{
			return;
		}
		if (floating != null)
		{
			floating.enabled = false;
		}
		prediscard = false;
		discard = true;
		active = false;
		casting = false;
		cardSize = 0.75f;
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 0f);
		int num = sortingOrderDiscard;
		if (cardVanish)
		{
			whereToDiscard = Enums.CardPlace.Vanish;
		}
		switch (whereToDiscard)
		{
		case Enums.CardPlace.Discard:
			base.transform.parent = MatchManager.Instance.GetDiscardPileTransform();
			if (base.transform.parent == null)
			{
				base.transform.parent = MatchManager.Instance.tempTransform;
			}
			SetDiscardSortingOrder(auxIndex);
			destinationLocalPosition = MatchManager.Instance.GetDiscardPilePosition();
			MatchManager.Instance.RedoDiscardPileDepth();
			break;
		case Enums.CardPlace.TopDeck:
		case Enums.CardPlace.BottomDeck:
		case Enums.CardPlace.RandomDeck:
		{
			if (whereToDiscard != Enums.CardPlace.RandomDeck)
			{
				cardData.Visible = true;
			}
			MoveCardToDeckPile();
			int position = num;
			CardChildSorting("Cards", position);
			break;
		}
		case Enums.CardPlace.Vanish:
			base.transform.parent = MatchManager.Instance.GetWorldTransform();
			if (trailParticlesNPC.gameObject.activeSelf)
			{
				trailParticlesNPC.gameObject.SetActive(value: false);
			}
			StartCoroutine(VanishToZero());
			break;
		}
		DrawBorder("");
		HideRarityParticles();
		HideCardIconParticles();
		if (skullImageT.gameObject.activeSelf)
		{
			skullImageT.gameObject.SetActive(value: false);
		}
		if (checkImageT.gameObject.activeSelf)
		{
			checkImageT.gameObject.SetActive(value: false);
		}
		if (discardedFromHand)
		{
			MatchManager.Instance.DiscardCard(tablePosition, whereToDiscard);
		}
		tablePosition = -1;
	}

	public IEnumerator VanishToZero()
	{
		if (base.gameObject != null && base.gameObject.activeSelf)
		{
			lockPosition = true;
			PlayVanishParticle();
			yield return Globals.Instance.WaitForSeconds(0.1f);
			SetDestinationLocalScale(0f);
			yield return Globals.Instance.WaitForSeconds(1f);
			if ((bool)MatchManager.Instance)
			{
				base.transform.parent = MatchManager.Instance.tempVanishedTransform;
				StartCoroutine(SelfDestruct(3f));
			}
			else
			{
				StartCoroutine(SelfDestruct(2f));
			}
		}
	}

	public void Vanish()
	{
		PlayVanishParticle();
		if ((bool)MatchManager.Instance)
		{
			base.transform.parent = MatchManager.Instance.tempVanishedTransform;
			StartCoroutine(SelfDestruct(10f));
		}
		else
		{
			StartCoroutine(SelfDestruct(2f));
		}
	}

	public void PlayDissolveParticle()
	{
		dissolveParticleT.gameObject.SetActive(value: true);
		dissolveParticle.Play();
	}

	public void PlayVanishParticle()
	{
		GameManager.Instance.PlayLibraryAudio("vanish_woosh", 0.5f);
		vanishParticle.gameObject.SetActive(value: false);
		vanishParticle.gameObject.SetActive(value: true);
	}

	public void PlaySightParticle()
	{
		if (sightParticle.gameObject.activeSelf)
		{
			sightParticle.gameObject.SetActive(value: false);
		}
		if (!cardoutsidelibary && !cardoutsidereward)
		{
			sightParticle.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			sightParticle.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		}
		sightParticle.gameObject.SetActive(value: true);
	}

	public void RedrawCardsDamageType(Hero theHero)
	{
	}

	public void RedrawDescriptionPrecalculated(Hero theHero, bool includeInSearch = true)
	{
		if (theHero != null)
		{
			Enums.DamageType itemModifiedDamageType = theHero.GetItemModifiedDamageType();
			Enums.DamageType enchantModifiedDamageType = theHero.GetEnchantModifiedDamageType();
			Enums.DamageType dt = Enums.DamageType.None;
			if (enchantModifiedDamageType != Enums.DamageType.None)
			{
				dt = enchantModifiedDamageType;
			}
			else if (itemModifiedDamageType != Enums.DamageType.None)
			{
				dt = itemModifiedDamageType;
			}
			cardData.ModifyDamageType(dt);
			MatchManager.Instance?.ApplyHeroModsToPetCard(CardData, theHero);
			cardData.SetDamagePrecalculated(theHero.DamageWithCharacterBonus(cardData.Damage, cardData.DamageType, cardData.CardClass) + TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(cardData, theHero, cardData.Damage, cardData.DamageType, cardData.CardClass, 0));
			cardData.SetDamagePrecalculated2(theHero.DamageWithCharacterBonus(cardData.Damage2, cardData.DamageType2, cardData.CardClass) + TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(cardData, theHero, cardData.Damage2, cardData.DamageType2, cardData.CardClass, 0));
			cardData.SetDamagePrecalculatedCombined(theHero.DamageWithCharacterBonus(cardData.Damage, cardData.DamageType, cardData.CardClass, 0, cardData.Damage2) + TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(cardData, theHero, cardData.Damage, cardData.DamageType, cardData.CardClass, 0));
			cardData.SetDamageSelfPrecalculated(theHero.DamageWithCharacterBonus(cardData.DamageSelf, cardData.DamageType, cardData.CardClass) + TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(cardData, theHero, cardData.DamageSelf, cardData.DamageType, cardData.CardClass, 0));
			cardData.SetDamageSelfPrecalculated2(theHero.DamageWithCharacterBonus(cardData.DamageSelf2, cardData.DamageType, cardData.CardClass) + TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(cardData, theHero, cardData.DamageSelf2, cardData.DamageType, cardData.CardClass, 0));
			cardData.SetDamageSidesPrecalculated(theHero.DamageWithCharacterBonus(cardData.DamageSides, cardData.DamageType, cardData.CardClass) + TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(cardData, theHero, cardData.DamageSides, cardData.DamageType, cardData.CardClass, 0));
			cardData.SetDamageSidesPrecalculated2(theHero.DamageWithCharacterBonus(cardData.DamageSides2, cardData.DamageType2, cardData.CardClass) + TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(cardData, theHero, cardData.DamageSides2, cardData.DamageType, cardData.CardClass, 0));
			cardData.SetHealPrecalculated(theHero.HealWithCharacterBonus(cardData.Heal, cardData.CardClass));
			cardData.SetHealSelfPrecalculated(theHero.HealWithCharacterBonus(cardData.HealSelf, cardData.CardClass));
			if (cardData.Aura != null || cardData.AuraSelf != null)
			{
				cardData.AuraCharges += TeamBonusHotline.GetAuraBonusFromTeamHeroesForPets(theHero, cardData, (cardData.Aura == null) ? cardData.AuraSelf.Id : cardData.Aura.Id);
			}
			if (cardData.Aura2 != null || cardData.AuraSelf2 != null)
			{
				cardData.AuraCharges2 += TeamBonusHotline.GetAuraBonusFromTeamHeroesForPets(theHero, cardData, (cardData.Aura2 == null) ? cardData.AuraSelf2.Id : cardData.Aura2.Id);
			}
			if (cardData.Aura3 != null || cardData.AuraSelf3 != null)
			{
				cardData.AuraCharges3 += TeamBonusHotline.GetAuraBonusFromTeamHeroesForPets(theHero, cardData, (cardData.Aura3 == null) ? cardData.AuraSelf3.Id : cardData.Aura3.Id);
			}
			if (cardData.Curse != null || cardData.CurseSelf != null)
			{
				cardData.CurseCharges += TeamBonusHotline.GetAuraBonusFromTeamHeroesForPets(theHero, cardData, (cardData.Curse == null) ? cardData.CurseSelf.Id : cardData.Curse.Id);
			}
			if (cardData.Curse2 != null || cardData.CurseSelf2 != null)
			{
				cardData.CurseCharges2 += TeamBonusHotline.GetAuraBonusFromTeamHeroesForPets(theHero, cardData, (cardData.Curse2 == null) ? cardData.CurseSelf2.Id : cardData.Curse2.Id);
			}
			if (cardData.Curse3 != null || cardData.CurseSelf3 != null)
			{
				cardData.CurseCharges3 += TeamBonusHotline.GetAuraBonusFromTeamHeroesForPets(theHero, cardData, (cardData.Curse3 == null) ? cardData.CurseSelf3.Id : cardData.Curse3.Id);
			}
			if (cardData.ItemEnchantment != null)
			{
				cardData.SetEnchantDamagePrecalculated1(theHero.DamageWithCharacterBonus(cardData.ItemEnchantment.DamageToTarget1, cardData.ItemEnchantment.DamageToTargetType1, cardData.CardClass));
				cardData.SetEnchantDamagePrecalculated2(theHero.DamageWithCharacterBonus(cardData.ItemEnchantment.DamageToTarget2, cardData.ItemEnchantment.DamageToTargetType2, cardData.CardClass));
			}
			else if (cardData.Item != null)
			{
				cardData.SetEnchantDamagePrecalculated1(theHero.DamageWithCharacterBonus(cardData.Item.DamageToTarget1, cardData.Item.DamageToTargetType1, cardData.CardClass));
				cardData.SetEnchantDamagePrecalculated2(theHero.DamageWithCharacterBonus(cardData.Item.DamageToTarget2, cardData.Item.DamageToTargetType2, cardData.CardClass));
			}
			cardData.SetDescriptionNew(forceDescription: true, theHero, includeInSearch);
			descriptionTextTM.text = cardData.DescriptionNormalized;
			NormalizeHeight(descriptionTextTM, cardData.Item);
			DrawEnergyCost();
		}
	}

	public void NormalizeHeight(TMP_Text textField, ItemData item)
	{
		textField.ForceMeshUpdate();
		float preferredHeight = textField.preferredHeight;
		string text = textField.text;
		StringBuilder stringBuilder = new StringBuilder();
		if (preferredHeight < 0.5f && item == null)
		{
			text = new Regex(" <nobr>").Replace(text, "<br><nobr>", 1);
		}
		stringBuilder.Append(text);
		textField.text = stringBuilder.ToString();
	}

	public int GetEnergyCost()
	{
		int num = 0;
		num = ((theHero != null) ? theHero.GetCardFinalCost(cardData) : ((theNPC == null) ? cardData.GetCardFinalCost() : theNPC.GetCardFinalCost(cardData)));
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public void DrawEnergyCost(bool ActualCost = true)
	{
		if (!cardData || cardData.Playable)
		{
			energyText.gameObject.SetActive(value: true);
			energyTextBg.gameObject.SetActive(value: true);
			int num;
			if (ActualCost)
			{
				num = GetEnergyCost();
				energyTextTM.text = num.ToString();
			}
			else
			{
				num = cardData.EnergyCostForShow;
				energyTextTM.text = num.ToString();
			}
			if (num == GetEnergyCost() && MatchManager.Instance != null && MatchManager.Instance.WaitingForActionScreen())
			{
				energyTextTM.color = Globals.Instance.ColorColor["white"];
			}
			else if (num < cardData.EnergyCostOriginal)
			{
				energyTextTM.color = Globals.Instance.ColorColor["greenCard"];
			}
			else if (num > cardData.EnergyCostOriginal)
			{
				energyTextTM.color = Globals.Instance.ColorColor["redCard"];
			}
			else
			{
				energyTextTM.color = Globals.Instance.ColorColor["white"];
			}
		}
	}

	public void ShowEnergyModification(int value)
	{
		energyModificationTM.text = value.ToString();
		if (value < 0)
		{
			energyModificationTM.color = Globals.Instance.ColorColor["greenCard"];
		}
		else
		{
			energyModificationTM.color = Globals.Instance.ColorColor["redCard"];
		}
		energyModification.gameObject.SetActive(value: false);
		energyModification.gameObject.SetActive(value: true);
	}

	public bool IsPlayable()
	{
		if (GameManager.Instance.DisableCardCast)
		{
			return false;
		}
		if (cardData != null && !cardData.Playable)
		{
			return false;
		}
		if (theHero != null)
		{
			return theHero.CanPlayCard(cardData);
		}
		if (theNPC != null)
		{
			return theNPC.CanPlayCard(cardData);
		}
		return false;
	}

	public bool IsPlayableRightNow()
	{
		if (IsPlayable() && active && GetEnergyCost() <= MatchManager.Instance.GetHeroEnergy() && MatchManager.Instance.IsThereAnyTargetForCard(cardData))
		{
			return true;
		}
		return false;
	}

	public void DrawEnergyBorder()
	{
		if (cardData == null)
		{
			return;
		}
		ShowDisable(state: false);
		if (MatchManager.Instance != null && (MatchManager.Instance.MatchIsOver || MatchManager.Instance.GameStatus == "DrawingCards"))
		{
			DrawBorder("");
		}
		else
		{
			if (cardData.CardClass == Enums.CardClass.Injury || (cardData.CardClass == Enums.CardClass.Boon && cardData.AutoplayDraw))
			{
				return;
			}
			if (IsPlayable() && active && GetEnergyCost() <= MatchManager.Instance.GetHeroEnergy())
			{
				if (MatchManager.Instance.IsThereAnyTargetForCard(cardData))
				{
					if (cardData.LookCards > 0 && cardData.LookCards > MatchManager.Instance.CountHeroDeck())
					{
						DrawBorder("orange");
					}
					else
					{
						DrawBorder("green");
					}
				}
				else
				{
					DrawBorder("red");
				}
			}
			else
			{
				if (theHero != null && active)
				{
					if (!IsPlayable())
					{
						DrawBorder("red");
					}
					else
					{
						DrawBorder("");
					}
				}
				else
				{
					DrawBorder("");
				}
				if (IsPlayable() && theHero != null && active && GetEnergyCost() > MatchManager.Instance.GetHeroEnergy())
				{
					ShowDisable(state: true);
				}
			}
			if (!(cardData.EffectRequired != "") || theHero == null)
			{
				return;
			}
			if (!theHero.HasEffect(cardData.EffectRequired))
			{
				if (cardData.EffectRequired == "stanzai")
				{
					if (!theHero.HasEffect("stanzaii") && !theHero.HasEffect("stanzaiii"))
					{
						requireTextTM.color = Functions.HexToColor("#910303");
					}
					else
					{
						requireTextTM.color = Functions.HexToColor("#1E650F");
					}
				}
				else if (cardData.EffectRequired == "stanzaii")
				{
					if (!theHero.HasEffect("stanzaiii"))
					{
						requireTextTM.color = Functions.HexToColor("#910303");
					}
					else
					{
						requireTextTM.color = Functions.HexToColor("#1E650F");
					}
				}
				else
				{
					requireTextTM.color = Functions.HexToColor("#910303");
				}
			}
			else
			{
				requireTextTM.color = Functions.HexToColor("#1E650F");
			}
		}
	}

	public void DrawSkull(bool state)
	{
		if (skullImageT.gameObject.activeSelf != state)
		{
			skullImageT.gameObject.SetActive(state);
		}
	}

	public void DrawCheck(bool state)
	{
		if (checkImageT.gameObject.activeSelf != state)
		{
			checkImageT.gameObject.SetActive(state);
		}
	}

	public void DrawBorder(string color)
	{
		if ((color != "blue" && color != "" && disableT != null && disableT.gameObject.activeSelf) || cardBorder == null || cardBorderSR == null)
		{
			return;
		}
		if (color == "")
		{
			if (cardBorder.gameObject.activeSelf)
			{
				cardBorder.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!cardBorder.gameObject.activeSelf)
		{
			cardBorder.gameObject.SetActive(value: true);
		}
		switch (color)
		{
		case "green":
			if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && !MatchManager.Instance.IsYourTurn())
			{
				cardBorderSR.color = orangeColor;
			}
			else
			{
				cardBorderSR.color = greenColor;
			}
			break;
		case "red":
			cardBorderSR.color = redColor;
			break;
		case "blue":
			cardBorderSR.color = blueColor;
			break;
		case "orange":
			cardBorderSR.color = orangeColor;
			break;
		case "black":
			cardBorderSR.color = blackColor;
			break;
		case "purple":
			cardBorderSR.color = purpleColor;
			break;
		}
	}

	public void HideEnergyBorder()
	{
		DrawBorder("");
	}

	public void AdjustPositionBecauseHover(int cardAmplifiedPosition, bool open, int totalCards)
	{
		if (open)
		{
			float num = Mathf.Abs(cardAmplifiedPosition - tablePosition);
			Vector3 vector = new Vector3(0.4f * (1f / num), 0f, 0f);
			if (tablePosition < cardAmplifiedPosition)
			{
				float x = 0.012f * (float)Mathf.Abs(totalCards - 5) * (9f - num);
				destinationLocalPosition = initialLocalPosition - vector - new Vector3(x, 0f, 0f);
			}
			else
			{
				float x2 = 0.018f * (float)Mathf.Abs(totalCards - 5) * (9f - num);
				destinationLocalPosition = initialLocalPosition + vector + new Vector3(x2, 0f, 0f);
			}
		}
		else
		{
			destinationLocalPosition = initialLocalPosition;
		}
	}

	public void AmplifyCard()
	{
		MatchManager.Instance.SetCardHover(tablePosition, state: true);
		int position = 10000;
		if ((bool)MatchManager.Instance)
		{
			CardChildSorting("Book", position);
		}
		else
		{
			CardChildSorting("", position);
		}
		if (transformVerticalDesplazament == Vector3.zero)
		{
			transformVerticalDesplazament = new Vector3(0f, cardCollider.bounds.size.y * 0.25f, -1f);
		}
		destinationLocalPosition = initialLocalPosition + transformVerticalDesplazament;
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 0f);
		cardSize = cardSizeAmplified;
	}

	public void RestoreCard()
	{
		if (floating != null)
		{
			floating.enabled = false;
		}
		DefaultElementsLayeringOrder();
		cardSize = cardSizeTable;
		destinationLocalPosition = initialLocalPosition;
		destinationLocalRotation = initialLocalRotation;
		MatchManager.Instance.SetCardHover(tablePosition, state: false);
		EnableDisableAddcardAction(state: false, graphicalAdjustments: false);
		EnableDisableDiscardAction(state: false, iconSkull: false, graphicalAdjustments: false);
		if (!MatchManager.Instance.controllerClickedCard && MatchManager.Instance.PreCastNum == -1 && !(MatchManager.Instance.CardActive != null))
		{
			MatchManager.Instance.SetTarget(null);
		}
		cursorArrow.StopDraw();
		DrawEnergyBorder();
		if (MatchManager.Instance.controllerClickedCard)
		{
			MatchManager.Instance.ResetController();
		}
	}

	public void SetDiscardSortingOrder(int index = -1)
	{
		int sortingOrder = sortingOrderDiscard;
		Transform discardPileTransform = MatchManager.Instance.GetDiscardPileTransform();
		if (discardPileTransform.childCount > 1)
		{
			Transform child = discardPileTransform.GetChild(discardPileTransform.childCount - 2);
			if (child != null)
			{
				foreach (Transform item in child)
				{
					SpriteRenderer component = item.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						sortingOrder = component.sortingOrder;
						break;
					}
				}
			}
		}
		int num = 100;
		int num2 = 0;
		num2 = ((index == -1) ? sortingOrder : (sortingOrder + index * num));
		CardChildSorting("Discards", num2, num);
	}

	public void SetColorArrow(string theColor)
	{
		cursorArrow.SetColor(theColor);
	}

	public void PositionCardInNPC(int order, int total)
	{
		float num = (float)total * 0.5f - 0.5f;
		float num2 = Mathf.Abs(num - (float)order);
		Quaternion quaternion = Quaternion.Euler(0f, 0f, -6f * (num - (float)order));
		destinationLocalRotation = (initialLocalRotation = quaternion);
		if (total > 4)
		{
			cardDistanceNPC *= 1f;
		}
		else if (total > 3)
		{
			cardDistanceNPC *= 0.85f;
		}
		float x = cardDistanceNPC * (num - (float)order);
		float y = num2 * num2 * -0.02f;
		Vector3 vector = (base.transform.localPosition = new Vector3(x, y, 1f + (float)order * 0.1f));
		destinationLocalPosition = (initialLocalPosition = vector);
		base.transform.localScale = new Vector3(0f, 0f, 0f);
	}

	public void ShowCardNPC(int iteration)
	{
		base.gameObject.SetActive(value: true);
		RedrawDescriptionPrecalculatedNPC(theNPC);
		StartCoroutine(ShowCardNPCCo(iteration));
	}

	private IEnumerator ShowCardNPCCo(int iteration)
	{
		yield return Globals.Instance.WaitForSeconds(0.2f * (float)iteration);
		cardSize = cardScaleNPC;
		discard = true;
		yield return Globals.Instance.WaitForSeconds(0.5f);
		discard = false;
	}

	public void RedrawDescriptionPrecalculatedNPC(NPC theNPC)
	{
		if (theNPC != null && !(cardData == null))
		{
			Enums.DamageType itemModifiedDamageType = theNPC.GetItemModifiedDamageType();
			Enums.DamageType enchantModifiedDamageType = theNPC.GetEnchantModifiedDamageType();
			Enums.DamageType dt = Enums.DamageType.None;
			if (enchantModifiedDamageType != Enums.DamageType.None)
			{
				dt = enchantModifiedDamageType;
			}
			else if (itemModifiedDamageType != Enums.DamageType.None)
			{
				dt = itemModifiedDamageType;
			}
			cardData.ModifyDamageType(dt);
			int energyCost = GetEnergyCost();
			cardData.SetDamagePrecalculated(theNPC.DamageWithCharacterBonus(cardData.Damage, cardData.DamageType, cardData.CardClass, energyCost));
			cardData.SetDamagePrecalculated2(theNPC.DamageWithCharacterBonus(cardData.Damage2, cardData.DamageType2, cardData.CardClass, energyCost));
			cardData.SetDamagePrecalculatedCombined(theNPC.DamageWithCharacterBonus(cardData.Damage, cardData.DamageType, cardData.CardClass, energyCost, cardData.Damage2));
			cardData.SetDamageSelfPrecalculated(theNPC.DamageWithCharacterBonus(cardData.DamageSelf, cardData.DamageType, cardData.CardClass, energyCost));
			cardData.SetDamageSelfPrecalculated2(theNPC.DamageWithCharacterBonus(cardData.DamageSelf2, cardData.DamageType, cardData.CardClass, energyCost));
			cardData.SetDamageSidesPrecalculated(theNPC.DamageWithCharacterBonus(cardData.DamageSides, cardData.DamageType, cardData.CardClass, energyCost));
			cardData.SetDamageSidesPrecalculated2(theNPC.DamageWithCharacterBonus(cardData.DamageSides2, cardData.DamageType2, cardData.CardClass, energyCost));
			cardData.SetHealPrecalculated(theNPC.HealWithCharacterBonus(cardData.Heal, cardData.CardClass, energyCost));
			cardData.SetHealSelfPrecalculated(theNPC.HealWithCharacterBonus(cardData.HealSelf, cardData.CardClass, energyCost));
			if (cardData.ItemEnchantment != null)
			{
				cardData.SetEnchantDamagePrecalculated1(theNPC.DamageWithCharacterBonus(cardData.ItemEnchantment.DamageToTarget1, cardData.ItemEnchantment.DamageToTargetType1, cardData.CardClass, energyCost));
				cardData.SetEnchantDamagePrecalculated2(theNPC.DamageWithCharacterBonus(cardData.ItemEnchantment.DamageToTarget2, cardData.ItemEnchantment.DamageToTargetType2, cardData.CardClass, energyCost));
			}
			else if (cardData.Item != null)
			{
				cardData.SetEnchantDamagePrecalculated1(theNPC.DamageWithCharacterBonus(cardData.Item.DamageToTarget1, cardData.Item.DamageToTargetType1, cardData.CardClass, energyCost));
				cardData.SetEnchantDamagePrecalculated2(theNPC.DamageWithCharacterBonus(cardData.Item.DamageToTarget2, cardData.Item.DamageToTargetType2, cardData.CardClass, energyCost));
			}
			cardData.SetDescriptionNew(forceDescription: true, theNPC);
			descriptionTextTM.text = cardData.DescriptionNormalized;
			NormalizeHeight(descriptionTextTM, cardData.Item);
		}
	}

	public void CastCardNPC(Transform theTransform)
	{
		cardCollider.enabled = false;
		RemoveAmplifyNewCard();
		StartCoroutine(CastCardNPCCo(theTransform));
	}

	private IEnumerator CastCardNPCCo(Transform theTransform)
	{
		yield return Globals.Instance.WaitForSeconds(0.25f);
		cardrevealed = false;
		RedrawDescriptionPrecalculatedNPC(theNPC);
		Vector3 vector = base.transform.parent.localPosition + base.transform.parent.transform.parent.localPosition;
		destinationLocalPosition = -vector - new Vector3(0f, 2.1f, 0f);
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 0f);
		cardSize = 1.4f;
		DrawBorder("orange");
		NPCTargetPosition = theTransform.localPosition + new Vector3(0f, vector.y * 0.5f, 0f);
		TopLayeringOrder("Book");
		cardnpc = true;
		ShowBackImage(state: false);
		yield return Globals.Instance.WaitForSeconds(0.5f);
		cardnpc = false;
		destinationLocalRotation = Quaternion.Euler(0f, 0f, 179f);
	}

	public void DiscardCardNPC(int i)
	{
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(DiscardCardNPCCo(i));
		}
	}

	private IEnumerator DiscardCardNPCCo(int i)
	{
		yield return Globals.Instance.WaitForSeconds(0.1f * (float)i);
		discard = true;
		cardSize = 0f;
		yield return Globals.Instance.WaitForSeconds(0.3f);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void EnableTrail()
	{
		trailParticlesNPC.gameObject.SetActive(value: true);
	}

	public void DisableTrail()
	{
		trailParticlesNPC.Stop();
		trailParticlesNPC.gameObject.SetActive(value: false);
	}

	public IEnumerator DrawArrowRemote(Vector3 ori, Vector3 dest)
	{
		if (!(base.gameObject == null))
		{
			cursorArrow.SetColor("gold");
			float num = Mathf.Clamp(Mathf.Abs(ori.x - dest.x) * 0.01f, 4f, 6f);
			Vector3 point = new Vector3(base.transform.position.x + (ori.x - dest.x) * 1E-05f, base.transform.position.y + 0.5f, -10f);
			Vector3 point2 = new Vector3(cursorArrow.point1.x, cursorArrow.point3.y + num, 0f);
			Vector3 point3 = new Vector3(dest.x, dest.y, 5f);
			cursorArrow.point1 = point;
			cursorArrow.point2 = point2;
			cursorArrow.point3 = point3;
			cursorArrow.StartDraw(MatchManager.Instance.CanInstaCast(cardData));
		}
		yield break;
	}

	public void DrawArrow(Vector3 ori, Vector3 dest)
	{
		float num = Mathf.Clamp(Mathf.Abs(ori.x - dest.x) * 0.01f, 4f, 6f);
		cursorArrow.point1 = new Vector3(base.transform.position.x + (ori.x - dest.x) * 1E-05f, base.transform.position.y + 0.5f, 10f);
		cursorArrow.point2 = new Vector3(cursorArrow.point1.x, cursorArrow.point3.y + num, 0f);
		cursorArrow.point3 = new Vector3(dest.x, dest.y, 5f);
		cursorArrow.StartDraw(MatchManager.Instance.CanInstaCast(cardData));
	}

	private void CreateAmplifyNewCard(Vector3 oriPosition)
	{
		oriPosition -= new Vector3(2f, 1f, 0f);
		MatchManager.Instance.amplifiedTransformShow = true;
		cardAmplifyNPC = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, oriPosition, Quaternion.identity, MatchManager.Instance.amplifiedTransform);
		cardAmplifyNPC.transform.localScale = Vector3.zero;
		cardAmplifyNPC.name = internalId;
		CardItem component = cardAmplifyNPC.GetComponent<CardItem>();
		component.SetCard(cardData.InternalId, deckScale: false, theHero, theNPC);
		component.discard = true;
		component.RedrawDescriptionPrecalculatedNPC(theNPC);
		component.DisableTrail();
		if ((bool)MatchManager.Instance)
		{
			component.CardChildSorting("UI", 30000);
		}
		else
		{
			component.CardChildSorting("UI", 15000);
		}
		component.SetDestinationScaleRotation(oriPosition, cardSizeTable + 0.2f, Quaternion.Euler(0f, 0f, 0f));
		component.ShowKeyNotes(followCardPosition: false, "left");
	}

	private void CreateAmplifyRelatedCard(string cardId, Transform theTransform)
	{
		if (relatedCard != null)
		{
			UnityEngine.Object.Destroy(relatedCard);
		}
		relatedCard = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, theTransform.position + new Vector3(0f, 0f, 1f), Quaternion.identity, theTransform);
		relatedCard.gameObject.SetActive(value: false);
		CardItem component = relatedCard.GetComponent<CardItem>();
		component.SetCard(cardId, deckScale: false, null, null, GetFromGlobal: true);
		component.DisableCollider();
		component.DisableTrail();
		component.TopLayeringOrder("UI", 32000);
		component.ShowRelated();
		if (theTransform.position.y > -1f)
		{
			component.SetDestinationScaleRotation(new Vector3(0f, -2.6f, 0f), 0.8f, Quaternion.Euler(0f, 0f, 0f));
		}
		else
		{
			component.SetDestinationScaleRotation(new Vector3(0f, 2.5f, 0f), 0.8f, Quaternion.Euler(0f, 0f, 0f));
		}
		component.discard = true;
		relatedCard.gameObject.SetActive(value: true);
	}

	public void CreateAmplifyOutsideCard(CardData _cardData = null, BoxCollider2D collider = null, Hero hero = null, bool disableCollider = false)
	{
		if ((bool)CardPlayerManager.Instance || (bool)CardPlayerPairsManager.Instance)
		{
			return;
		}
		if (_cardData != null)
		{
			cardData = _cardData;
		}
		if (hero == null)
		{
			hero = theHero;
		}
		Vector3 position = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
		if (cardoutsidelibary)
		{
			if (position.x > 0f)
			{
				position.x -= 2f;
			}
			else
			{
				position.x += 2f;
			}
		}
		else if (cardoutsidereward)
		{
			position.x += 2.5f;
		}
		else if (cardoutsideverticallist)
		{
			if (CardScreenManager.Instance.IsActive())
			{
				position.x = -8.05f;
			}
			else if (collider != null)
			{
				_ = collider.size;
				new Vector3(collider.offset.x, collider.offset.y, 0f);
				float num = -1f;
				if ((bool)CardCraftManager.Instance && CardCraftManager.Instance.craftType == 5)
				{
					num = -2f;
				}
				if (base.transform.position.x > num)
				{
					position.x = base.transform.TransformPoint(collider.offset).x - collider.size.x * 0.5f - cardCollider.size.x * 0.5f;
				}
				else
				{
					position.x = base.transform.TransformPoint(collider.offset).x + collider.size.x * 0.5f + cardCollider.size.x * 0.5f + 0.1f;
				}
				if ((bool)ChallengeSelectionManager.Instance)
				{
					position.y += 2f;
				}
				else if ((bool)EventManager.Instance)
				{
					position.y -= 2.5f;
					position.x += 0.7f;
				}
			}
		}
		else if (cardFromEventTracker)
		{
			position = PopupManager.Instance.GetPopupActiveCoordinates();
		}
		else
		{
			position += new Vector3(0f, 4f, 0f);
		}
		if (cardAmplifyOutside == null)
		{
			Transform transform = null;
			if (CardCraftManager.Instance != null)
			{
				transform = CardCraftManager.Instance.cardListContainer.Find("CardAmplifyOutside");
				if (transform != null)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
				cardAmplifyOutside = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, position, Quaternion.identity, GameManager.Instance.TempContainer);
			}
			else
			{
				transform = GameManager.Instance.TempContainer.Find("CardAmplifyOutside");
				if (transform != null)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
				cardAmplifyOutside = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, position, Quaternion.identity, GameManager.Instance.TempContainer);
			}
		}
		cardAmplifyOutside.name = "CardAmplifyOutside";
		CardItem component = cardAmplifyOutside.GetComponent<CardItem>();
		component.SetCard(cardData.InternalId, deckScale: false, hero, theNPC, GetFromGlobal: true);
		if (disableCollider)
		{
			component.DisableCollider();
		}
		component.TopLayeringOrder("UI", 31600);
		component.cardoutsidelibary = cardoutsidelibary;
		component.cardoutsideselection = cardoutsideselection;
		component.cardoutsidereward = cardoutsidereward;
		component.cardoutsideverticallist = cardoutsideverticallist;
		component.cardFromEventTracker = cardFromEventTracker;
		component.DisableTrail();
		if (cardoutsidelibary)
		{
			component.SetDestinationScaleRotation(position, 1.2f, Quaternion.Euler(0f, 0f, 0f));
			component.ShowKeyNotes(followCardPosition: true);
		}
		else if (cardoutsidereward)
		{
			component.SetDestinationScaleRotation(position, 1.4f, Quaternion.Euler(0f, 0f, 0f));
			component.ShowKeyNotes(followCardPosition: true);
			if (base.gameObject.name == "EventRollCard")
			{
				component.DisableCollider();
			}
		}
		else if (cardoutsideverticallist)
		{
			component.SetDestinationScaleRotation(position, 1.4f, Quaternion.Euler(0f, 0f, 0f));
			if (!CardScreenManager.Instance.IsActive())
			{
				if (base.transform.position.x > -2f)
				{
					component.ShowKeyNotes(followCardPosition: true, "followleft");
				}
				else
				{
					component.ShowKeyNotes(followCardPosition: true);
				}
			}
		}
		else if (cardFromEventTracker)
		{
			component.SetDestinationScaleRotation(position, 1.2f, Quaternion.Euler(0f, 0f, 0f));
		}
		else
		{
			component.SetDestinationScaleRotation(position, 1.6f, Quaternion.Euler(0f, 0f, 0f));
			component.ShowKeyNotes(followCardPosition: true);
		}
		component.cardoutsidecombatamplified = true;
		if (cardData.RelatedCard != "")
		{
			CreateAmplifyRelatedCard(cardData.RelatedCard, component.transform);
		}
	}

	public void ShowKeyNotes(bool followCardPosition = false, string position = "right")
	{
		if ((backImageT != null && backImageT.gameObject.activeSelf && backImageT.GetComponent<SpriteRenderer>().enabled) || cardData == null)
		{
			return;
		}
		if (rightClick != null && cardData.RelatedCard == "" && !CardScreenManager.Instance.IsActive() && (TomeManager.Instance.IsActive() || (bool)RewardsManager.Instance || (bool)HeroSelectionManager.Instance))
		{
			rightClick.gameObject.SetActive(value: true);
		}
		if (followCardPosition)
		{
			Vector3 vector = GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition);
			if (position != "followleft" && position != "followright")
			{
				position = ((!(vector.x > 0f)) ? "followright" : "followleft");
			}
		}
		if (cardoutsidereward)
		{
			position = "followright";
			if (ChallengeSelectionManager.Instance != null && GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition).x > 5f)
			{
				position = "followleft";
			}
		}
		PopupManager.Instance.SetCard(base.transform, cardData, cardData.KeyNotes, position);
	}

	public void HideKeyNotes()
	{
		if (rightClick != null && rightClick.gameObject.activeSelf)
		{
			rightClick.gameObject.SetActive(value: false);
		}
		if (!CardScreenManager.Instance.IsActive())
		{
			PopupManager.Instance.ClosePopup();
		}
	}

	public void RemoveAmplifyNewCard()
	{
		if (cardAmplifyNPC != null)
		{
			DrawBorder("");
			cardSize = cardScaleNPC;
			UnityEngine.Object.Destroy(cardAmplifyNPC);
			cardAmplifyNPC = null;
		}
	}

	public void ShowBorderParticle()
	{
		DrawBorder("blue");
		borderParticle.GetComponent<ParticleSystemRenderer>().sortingOrder = cardBorderSR.sortingOrder - 1;
		borderParticle.gameObject.SetActive(value: true);
		borderParticle.Clear();
		borderParticle.Play();
	}

	private IEnumerator RevealedCoroutine()
	{
		if (revealedCoroutine == null)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (!CardScreenManager.Instance.IsActive())
		{
			DrawBorder("blue");
			CreateAmplifyNewCard(base.transform.position);
			revealedCoroutine = null;
		}
	}

	private IEnumerator RevealedOutsideCoroutine()
	{
		DrawBorder("blue");
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowCardsMask(_status: true);
		}
		CreateAmplifyOutsideCard(null, null, null, disableCollider: true);
		if ((bool)RewardsManager.Instance && cardoutsidereward && !cardAlreadyInDeckText.text.IsNullOrEmpty())
		{
			ShowAlreadyHaveAVersion(state: true);
			ShowSingularity(_state: false);
		}
		yield return null;
	}

	public void DestroyReveleadOutside()
	{
		HideKeyNotes();
		UnityEngine.Object.Destroy(cardAmplifyOutside);
		cardAmplifyOutside = null;
	}

	public void HideReveleadOutside()
	{
		DrawBorder("");
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowCardsMask(_status: false);
		}
		DestroyReveleadOutside();
	}

	public void DoReturnCardToDeckFromDrag()
	{
		MatchManager.Instance.CardDrag = false;
		MatchManager.Instance.CardActiveT = null;
		MatchManager.Instance.SetDamagePreview(theCasterIsHero: false);
		MatchManager.Instance.SetOverDeck(state: false);
		cursorArrow.StopDraw();
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance.IsYourTurn())
		{
			MatchManager.Instance.StopArrowNet(tablePosition);
		}
		RestoreCard();
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && MatchManager.Instance.IsYourTurn())
		{
			MatchManager.Instance.AmplifyCardOut(tablePosition);
		}
		Cursor.visible = true;
	}

	public void OnMouseOver()
	{
		if (!(cardData == null) && !SettingsManager.Instance.IsActive() && !AlertManager.Instance.IsActive() && !MadnessManager.Instance.IsActive() && !SandboxManager.Instance.IsActive() && (!MatchManager.Instance || !MatchManager.Instance.console.IsActive() || !(base.transform.parent != MatchManager.Instance.console.transform)) && (!(backImageT != null) || !backImageT.gameObject.activeSelf || !backImageT.GetComponent<SpriteRenderer>().enabled || TomeManager.Instance.IsActive()))
		{
			if (Input.GetMouseButtonUp(1))
			{
				RightClick();
			}
			if (Input.GetMouseButtonUp(0))
			{
				LeftClick();
			}
			if (cardAmplifyNPC != null)
			{
				MatchManager.Instance.amplifiedTransformShow = true;
			}
		}
	}

	public void RightClick()
	{
		if (cardData == null || (PerkTree.Instance.IsActive() && !TomeManager.Instance.IsActive()) || (MatchManager.Instance != null && (MatchManager.Instance.CardDrag || MatchManager.Instance.waitingDeathScreen)) || (MatchManager.Instance != null && (MatchManager.Instance.DeckCardsWindow.IsActive() || MatchManager.Instance.EnergySelector.IsActive() || MatchManager.Instance.WaitingForActionScreen()) && destinationLocalPosition != base.transform.localPosition) || (backImageT != null && backImageT.gameObject.activeSelf && backImageT.GetComponent<SpriteRenderer>().enabled && !TomeManager.Instance.IsActive()))
		{
			return;
		}
		if (cardData.CardClass == Enums.CardClass.Item && CardCraftManager.Instance != null)
		{
			CardCraftManager.Instance.HoverItem(state: false, cardData.CardType);
		}
		if (!(CardScreenManager.Instance != null))
		{
			return;
		}
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowCardsMask(_status: false);
		}
		CardScreenManager.Instance.ShowCardScreen(_state: true);
		CardScreenManager.Instance.SetCardData(cardData);
		DrawBorder("");
		if (cardforaddcard || cardfordiscard)
		{
			if (cardmakebig)
			{
				SetDestinationLocalScale(cardmakebigSize);
			}
			return;
		}
		if (MatchManager.Instance != null && active && !MatchManager.Instance.characterWindow.IsActive() && !MatchManager.Instance.WaitingForCardEnergyAssignment)
		{
			fOnMouseExit();
		}
		if (revealedCoroutine != null)
		{
			StopCoroutine(revealedCoroutine);
		}
		RemoveAmplifyNewCard();
		UnityEngine.Object.Destroy(cardAmplifyOutside);
		UnityEngine.Object.Destroy(cardAmplifyNPC);
		cardAmplifyNPC = null;
		if (relatedCard != null)
		{
			UnityEngine.Object.Destroy(relatedCard);
		}
		if (cardmakebig)
		{
			SetDestinationLocalScale(cardmakebigSize);
			HideKeyNotes();
		}
	}

	public void LeftClick()
	{
		if (cardData == null || (PerkTree.Instance.IsActive() && !TomeManager.Instance.IsActive()) || (MatchManager.Instance != null && (MatchManager.Instance.CardDrag || MatchManager.Instance.waitingDeathScreen)) || (MatchManager.Instance != null && (MatchManager.Instance.DeckCardsWindow.IsActive() || MatchManager.Instance.EnergySelector.IsActive() || MatchManager.Instance.WaitingForActionScreen()) && destinationLocalPosition != base.transform.localPosition) || (backImageT != null && backImageT.gameObject.activeSelf && backImageT.GetComponent<SpriteRenderer>().enabled && !TomeManager.Instance.IsActive()) || !(TeamManagement.Instance != null) || !TomeManager.Instance.IsActive())
		{
			return;
		}
		TeamManagement.Instance.AddToSelectedList(cardData.Id);
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Added to card " + cardData.Id + " to list with new value becomes: " + string.Join(",", TeamManagement.Instance.CardsSelectedFromTome.Select((KeyValuePair<string, int> kv) => $"{kv.Key}:{kv.Value}")));
		}
	}

	public void OnMouseEnter()
	{
		if (KeyboardManager.Instance.IsActive())
		{
			return;
		}
		if (MatchManager.Instance != null && base.transform.parent == MatchManager.Instance.tempTransform && (MatchManager.Instance.CardActive == null || MatchManager.Instance.CardActive.InternalId != InternalId))
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Destroyed because card is temporal", "general");
			}
			UnityEngine.Object.Destroy(base.gameObject);
			HideKeyNotes();
		}
		else
		{
			fOnMouseEnterMaster();
		}
	}

	private void fOnMouseEnterMaster()
	{
		if (cardData == null || SettingsManager.Instance.IsActive() || AlertManager.Instance.IsActive() || ((bool)MatchManager.Instance && MatchManager.Instance.console.IsActive()) || CardScreenManager.Instance.IsActive() || ((bool)CardPlayerManager.Instance && !CardPlayerManager.Instance.CanClick()) || ((bool)CardPlayerPairsManager.Instance && !CardPlayerPairsManager.Instance.CanClick()) || (PerkTree.Instance.IsActive() && !TomeManager.Instance.IsActive()) || ((bool)MatchManager.Instance && MatchManager.Instance.PreCastNum > -1 && tablePosition != MatchManager.Instance.PreCastNum - 1))
		{
			return;
		}
		if (cardAmplifyNPC != null)
		{
			MatchManager.Instance.amplifiedTransformShow = true;
		}
		if ((bool)MatchManager.Instance && MatchManager.Instance.GameStatus == "" && cardData.CardType == Enums.CardType.Enchantment && base.transform.parent == MatchManager.Instance.tempTransform && (MatchManager.Instance.CardActive == null || MatchManager.Instance.CardActive.InternalId != InternalId))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (LootManager.Instance != null && CardCraftManager.Instance == null)
		{
			LootManager.Instance.HighLight(state: true, Enum.GetName(typeof(Enums.CardType), cardData.CardType));
		}
		if (cardData.CardClass == Enums.CardClass.Item && !TomeManager.Instance.IsActive() && CardCraftManager.Instance != null)
		{
			CardCraftManager.Instance.HoverItem(state: true, cardData.CardType);
		}
		if (cardmakebig)
		{
			if (cardmakebigSize == 0f)
			{
				cardmakebigSize = base.transform.localScale.x;
			}
			GameManager.Instance.PlayLibraryAudio("castnpccardfast");
			if (cardmakebigSizeMax == 0f)
			{
				cardmakebigSizeMax = cardmakebigSize + 0.2f;
			}
			SetDestinationLocalScale(cardmakebigSizeMax);
			SetDestination(base.transform.localPosition);
			cardrevealed = true;
			ShowKeyNotes();
			if (cardData.CardType != Enums.CardType.Corruption)
			{
				DrawBorder("blue");
			}
			else if (cardData.RelatedCard != "")
			{
				CreateAmplifyRelatedCard(cardData.RelatedCard, base.transform);
			}
			return;
		}
		if (cardoutsidecombatamplified)
		{
			HideReveleadOutside();
			return;
		}
		if (cardoutsidecombat)
		{
			GameManager.Instance.PlayLibraryAudio("ui_menu_popup_01");
			if (revealedCoroutine != null)
			{
				StopCoroutine(revealedCoroutine);
			}
			revealedCoroutine = StartCoroutine(RevealedOutsideCoroutine());
			return;
		}
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && !cardrevealed)
		{
			if (MatchManager.Instance.heroIndexWaitingForAddDiscard > -1)
			{
				if (!MatchManager.Instance.IsYourTurnForAddDiscard())
				{
					return;
				}
			}
			else if (!MatchManager.Instance.IsYourTurn())
			{
				ShowEmoteButton(_state: true);
				return;
			}
		}
		if (cardforaddcard || cardfordiscard || cardfordisplay || cardoutsidelibary || ((bool)EventManager.Instance && cardrevealed))
		{
			ShowKeyNotes();
		}
		if (cardfordiscard && !cardselectedfordiscard)
		{
			DrawBorder("blue");
		}
		else if (cardforaddcard && !cardselectedforaddcard)
		{
			DrawBorder("blue");
		}
		else
		{
			if (!MatchManager.Instance || MatchManager.Instance.WaitingForActionScreen() || MatchManager.Instance.WaitingForCardEnergyAssignment)
			{
				return;
			}
			if (cardrevealed && !MatchManager.Instance.CardDrag && MatchManager.Instance.GetNPCActive() != theNPC.NPCIndex)
			{
				if (revealedCoroutine != null)
				{
					StopCoroutine(revealedCoroutine);
				}
				revealedCoroutine = StartCoroutine(RevealedCoroutine());
			}
			if (active && !discard && !MatchManager.Instance.CardDrag && ((GameManager.Instance.IsMultiplayer() && !MatchManager.Instance.IsYourTurn()) || !MatchManager.Instance.HandMask.gameObject.activeSelf))
			{
				if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance.IsYourTurn())
				{
					MatchManager.Instance.AmplifyCard(tablePosition);
				}
				fOnMouseEnter();
			}
		}
	}

	public void fOnMouseEnter()
	{
		GameManager.Instance.SetCursorHover();
		if (GameManager.Instance.ConfigUseLegacySounds || AudioManager.Instance.soundCardHover == null)
		{
			GameManager.Instance.PlayLibraryAudio("card_play");
		}
		else
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardHover, 0.1f);
		}
		AmplifyCard();
		if (MatchManager.Instance.IsYourTurn())
		{
			int num = 0;
			if (MatchManager.Instance.GetHeroActive() > -1)
			{
				if (MatchManager.Instance.CountHeroDeck() > 0)
				{
					num++;
				}
				if (MatchManager.Instance.CountHeroDiscard() > 0)
				{
					num++;
				}
			}
			MatchManager.Instance.controllerCurrentIndex = tablePosition + num;
		}
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && MatchManager.Instance.IsYourTurn()) || (GameManager.Instance.IsMultiplayer() && MatchManager.Instance == null))
		{
			ShowKeyNotes();
		}
	}

	public void OnMouseExit()
	{
		if (cardData == null || SettingsManager.Instance.IsActive() || AlertManager.Instance.IsActive() || ((bool)MatchManager.Instance && MatchManager.Instance.console.IsActive()) || CardScreenManager.Instance.IsActive() || MadnessManager.Instance.IsActive() || SandboxManager.Instance.IsActive() || (PerkTree.Instance.IsActive() && !TomeManager.Instance.IsActive()) || ((bool)MatchManager.Instance && MatchManager.Instance.PreCastNum > -1))
		{
			return;
		}
		GameManager.Instance.SetCursorPlain();
		if (LootManager.Instance != null)
		{
			LootManager.Instance.HighLight(state: false, Enum.GetName(typeof(Enums.CardType), cardData.CardType));
		}
		if (cardData.CardClass == Enums.CardClass.Item && CardCraftManager.Instance != null)
		{
			CardCraftManager.Instance.HoverItem(state: false, cardData.CardType);
		}
		if (cardmakebig)
		{
			SetDestinationLocalScale(cardmakebigSize);
			HideKeyNotes();
			if (cardData.CardType != Enums.CardType.Corruption)
			{
				DrawBorder("");
			}
			if (relatedCard != null)
			{
				UnityEngine.Object.Destroy(relatedCard);
			}
			return;
		}
		RemoveAmplifyNewCard();
		HideKeyNotes();
		if (cardoutsidecombatamplified)
		{
			return;
		}
		if (cardoutsidecombat)
		{
			HideReveleadOutside();
			if ((bool)RewardsManager.Instance && cardoutsidereward && !cardAlreadyInDeckText.text.IsNullOrEmpty())
			{
				ShowAlreadyHaveAVersion(state: false);
				ShowSingularity(_state: true);
			}
			return;
		}
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && cardData.CardClass != Enums.CardClass.Monster)
		{
			if (MatchManager.Instance.heroIndexWaitingForAddDiscard > -1)
			{
				if (!MatchManager.Instance.IsYourTurnForAddDiscard())
				{
					return;
				}
			}
			else if (!MatchManager.Instance.IsYourTurn())
			{
				ShowEmoteButton(_state: false);
				return;
			}
		}
		if (cardfordiscard)
		{
			if (!cardselectedfordiscard)
			{
				DrawBorder("");
			}
		}
		else if (cardforaddcard)
		{
			if (!cardselectedforaddcard)
			{
				DrawBorder("");
			}
		}
		else
		{
			if (!(MatchManager.Instance != null))
			{
				return;
			}
			if (!MatchManager.Instance.CardDrag)
			{
				MatchManager.Instance.SetDamagePreview(theCasterIsHero: false);
				MatchManager.Instance.SetOverDeck(state: false);
			}
			if (MatchManager.Instance.WaitingForActionScreen() || MatchManager.Instance.WaitingForCardEnergyAssignment)
			{
				return;
			}
			if (revealedCoroutine != null)
			{
				StopCoroutine(revealedCoroutine);
				revealedCoroutine = null;
			}
			HideKeyNotes();
			if (!EventSystem.current.IsPointerOverGameObject() && active && !discard && !MatchManager.Instance.CardDrag)
			{
				if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && MatchManager.Instance.IsYourTurn())
				{
					MatchManager.Instance.AmplifyCardOut(tablePosition);
				}
				fOnMouseExit();
			}
		}
	}

	public void fOnMouseExit()
	{
		RestoreCard();
		MatchManager.Instance.SetGlobalOutlines(state: false);
	}

	private void OnMouseDown()
	{
		if (SettingsManager.Instance.IsActive() || AlertManager.Instance.IsActive() || ((bool)MatchManager.Instance && MatchManager.Instance.console.IsActive()) || MadnessManager.Instance.IsActive() || SandboxManager.Instance.IsActive())
		{
			return;
		}
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null)
		{
			if (MatchManager.Instance.heroIndexWaitingForAddDiscard > -1)
			{
				if (!MatchManager.Instance.IsYourTurnForAddDiscard())
				{
					return;
				}
			}
			else if (!MatchManager.Instance.IsYourTurn())
			{
				return;
			}
		}
		if (cardoutsidereward || MatchManager.Instance == null)
		{
			return;
		}
		if (cardfordiscard)
		{
			MatchManager.Instance.PreSelectCard();
		}
		else if (cardforaddcard)
		{
			MatchManager.Instance.PreSelectCard();
		}
		else
		{
			if (MatchManager.Instance.WaitingForActionScreen() || !IsPlayable() || MatchManager.Instance.IsGameBusy())
			{
				return;
			}
			HideKeyNotes();
			if (!EventSystem.current.IsPointerOverGameObject() && active && !discard)
			{
				AmplifyCard();
				if (SceneStatic.GetSceneName() == "Combat")
				{
					GameManager.Instance.EscapeFunction(activateExit: false);
				}
				MatchManager.Instance.SetTarget(null);
				MatchManager.Instance.CardDrag = true;
				MatchManager.Instance.SetCardActive(cardData);
				MatchManager.Instance.CardActiveT = base.transform;
				mouseClickedPosition = Input.mousePosition;
				GameManager.Instance.cameraMain.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
				Cursor.visible = false;
				canInstaCast = -1;
				MatchManager.Instance.SetDamagePreview(theCasterIsHero: true, cardData, tablePosition);
				if (MatchManager.Instance.CanInstaCast(cardData))
				{
					canInstaCast = 1;
				}
				else
				{
					canInstaCast = 0;
				}
				if (MatchManager.Instance.IsThereAnyTargetForCard(cardData))
				{
					canTargetCast = 1;
				}
				else
				{
					canTargetCast = 0;
				}
				if (GetEnergyCost() <= MatchManager.Instance.GetHeroEnergy())
				{
					canEnergyCast = 1;
				}
				else
				{
					canEnergyCast = 0;
				}
				fOnMouseDownCardData();
			}
		}
	}

	public void fOnMouseDownCardData()
	{
		MatchManager.Instance.CardItemActive = this;
		MatchManager.Instance.SetCardHover(tablePosition, state: false);
		if (!GameManager.Instance.ConfigUseLegacySounds)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
			AudioClip soundDrag = cardData.GetSoundDrag();
			if (soundDrag != null)
			{
				GameManager.Instance.PlayAudio(soundDrag, 0.25f);
			}
		}
	}

	private void OnMouseDrag()
	{
		if (!(MatchManager.Instance == null) && MatchManager.Instance.CardDrag && !cardforaddcard && !cardfordiscard && (!GameManager.Instance.IsMultiplayer() || !(MatchManager.Instance != null) || MatchManager.Instance.IsYourTurn()))
		{
			fOnMouseDrag();
		}
	}

	private void fOnMouseDrag()
	{
		if (!IsPlayable() || MatchManager.Instance.IsGameBusy() || !active || discard)
		{
			return;
		}
		Vector3 mousePosition = Input.mousePosition;
		if (canTargetCast != 1)
		{
			return;
		}
		if (canInstaCast != 1)
		{
			DrawArrow(mouseClickedPosition, GameManager.Instance.cameraMain.ScreenToWorldPoint(mousePosition));
			Cursor.visible = false;
		}
		else if (mousePosition.y - mouseClickedPosition.y > (float)distanceForDragCast)
		{
			if (!cardDraggedCanCast)
			{
				ShowBorderParticle();
			}
			cardDraggedCanCast = true;
		}
		else
		{
			if (cardDraggedCanCast)
			{
				DrawEnergyBorder();
			}
			cardDraggedCanCast = false;
		}
	}

	public void OnMouseUpController()
	{
		if (!MatchManager.Instance)
		{
			return;
		}
		if (MatchManager.Instance.DeckCardsWindow.IsActive())
		{
			OnMouseUp();
		}
		else if (MatchManager.Instance.DiscardSelector.IsActive())
		{
			OnMouseUp();
		}
		else
		{
			if (!IsPlayable() || MatchManager.Instance.IsGameBusy())
			{
				return;
			}
			if (MatchManager.Instance.IsYourTurn())
			{
				if (MatchManager.Instance.CanInstaCast(cardData))
				{
					int cardIndexInTableById = MatchManager.Instance.GetCardIndexInTableById(internalId);
					if (cardIndexInTableById != -1)
					{
						MatchManager.Instance.CastCardNum(cardIndexInTableById + 1);
						MatchManager.Instance.ResetCardHoverIndex();
					}
				}
				else
				{
					OnMouseDown();
					MatchManager.Instance.SetControllerCardClicked();
				}
			}
			else
			{
				MatchManager.Instance.SendEmoteCard(tablePosition);
			}
		}
	}

	public void OnMouseUp()
	{
		if (SettingsManager.Instance.IsActive() || AlertManager.Instance.IsActive() || ((bool)MatchManager.Instance && MatchManager.Instance.console.IsActive()) || MadnessManager.Instance.IsActive() || SandboxManager.Instance.IsActive() || (PerkTree.Instance.IsActive() && !CardScreenManager.Instance.IsActive()) || ((!MatchManager.Instance || !MatchManager.Instance.CardDrag) && (!MatchManager.Instance || !(keyTransform != null) || !keyTransform.gameObject.activeSelf) && !Functions.ClickedThisTransform(base.transform)))
		{
			return;
		}
		if (CardPlayerManager.Instance != null && CardPlayerIndex > -1)
		{
			CardPlayerManager.Instance.SelectCard(CardPlayerIndex);
			GameManager.Instance.CleanTempContainer();
			if (!GameManager.Instance.ConfigUseLegacySounds)
			{
				GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
			}
		}
		else if (CardPlayerPairsManager.Instance != null && CardPlayerIndex > -1)
		{
			CardPlayerPairsManager.Instance.SelectCard(CardPlayerIndex);
			GameManager.Instance.CleanTempContainer();
			if (!GameManager.Instance.ConfigUseLegacySounds)
			{
				GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
			}
		}
		else if (cardoutsideloot)
		{
			if ((!(LootManager.Instance != null) || LootManager.Instance.IsMyLoot) && (bool)LootManager.Instance)
			{
				LootManager.Instance.Looted(lootId);
				DisableCollider();
				GameManager.Instance.CleanTempContainer();
				if (!GameManager.Instance.ConfigUseLegacySounds)
				{
					GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
				}
			}
		}
		else if (cardoutsidereward)
		{
			HideReveleadOutside();
			if (RewardsManager.Instance != null)
			{
				if (GameManager.Instance.IsSingularity() && cardAlreadyInDeckT.gameObject.activeSelf)
				{
					AlertManager.buttonClickDelegate = ExecuteCardRewardConfirm;
					AlertManager.Instance.AlertConfirmDouble(string.Format(Texts.Instance.GetText("cardWillOverwrite"), cardAlreadyHaveName));
				}
				else
				{
					ExecuteCardRewardAction();
				}
			}
		}
		else
		{
			if (MatchManager.Instance == null)
			{
				return;
			}
			Cursor.visible = true;
			if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null)
			{
				if (MatchManager.Instance.heroIndexWaitingForAddDiscard > -1)
				{
					if (!MatchManager.Instance.IsYourTurnForAddDiscard())
					{
						return;
					}
				}
				else if (!MatchManager.Instance.IsYourTurn())
				{
					if (MatchManager.Instance.GetHeroHand(MatchManager.Instance.GetHeroActive()).Contains(cardData.Id))
					{
						MatchManager.Instance.SendEmoteCard(tablePosition);
					}
					return;
				}
			}
			if (cardfordiscard)
			{
				MatchManager.Instance.SelectCardToDiscard(this);
				if (!GameManager.Instance.ConfigUseLegacySounds)
				{
					GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
				}
			}
			else if (cardforaddcard)
			{
				MatchManager.Instance.SelectCardToAddcard(this);
				if (!GameManager.Instance.ConfigUseLegacySounds)
				{
					GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
				}
			}
			else
			{
				if (!MatchManager.Instance.CardDrag || MatchManager.Instance.WaitingForActionScreen() || !IsPlayable() || MatchManager.Instance.IsGameBusy() || EventSystem.current.IsPointerOverGameObject() || !active || discard)
				{
					return;
				}
				if (MatchManager.Instance != null)
				{
					cursorArrow.StopDraw();
					if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance.IsYourTurn())
					{
						MatchManager.Instance.StopArrowNet(tablePosition);
					}
				}
				MatchManager.Instance.CardDrag = false;
				MatchManager.Instance.CardActiveT = null;
				MatchManager.Instance.SetDamagePreview(theCasterIsHero: false);
				MatchManager.Instance.SetOverDeck(state: false);
				if (MatchManager.Instance.CheckTarget(null, cardData) || (MatchManager.Instance.CanInstaCast(cardData) && cardDraggedCanCast))
				{
					if (canEnergyCast == 1)
					{
						StartCoroutine(MatchManager.Instance.CastCard(this));
						StartCoroutine(MatchManager.Instance.JustCastedCo());
						return;
					}
					MatchManager.Instance.NoEnergy();
				}
				if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && MatchManager.Instance.IsYourTurn())
				{
					MatchManager.Instance.AmplifyCardOut(tablePosition);
				}
				if (MatchManager.Instance.CanInstaCast(cardData))
				{
					fOnMouseEnter();
				}
				RestoreCard();
			}
		}
	}

	private void ExecuteCardRewardConfirm()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ExecuteCardRewardConfirm));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			ExecuteCardRewardAction();
		}
	}

	private void ExecuteCardRewardAction()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			RewardsManager.Instance.SetCardReward(NetworkManager.Instance.GetPlayerNick(), base.gameObject.name);
		}
		else
		{
			RewardsManager.Instance.SetCardReward("", base.gameObject.name);
		}
		GameManager.Instance.CleanTempContainer();
		if (!GameManager.Instance.ConfigUseLegacySounds)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundCardClick);
		}
	}

	public void ShowEmoteButton(bool _state)
	{
		if (GameManager.Instance.IsMultiplayer() && !(MatchManager.Instance == null) && emoteIcon != null && (!_state || (active && !discard && !prediscard && !cardoutsidecombat && !cardoutsidelibary && !cardoutsideselection && !MatchManager.Instance.WaitingForActionScreen() && !MatchManager.Instance.WaitingForCardEnergyAssignment)))
		{
			emoteIcon.gameObject.SetActive(_state);
			emoteIcon.GetComponent<EmoteTarget>().SetActiveHeroOnCardEmoteButton();
		}
	}

	public void ShowEmoteIcon(byte _heroIndex)
	{
		ShowEmoteTransform();
		Hero hero = MatchManager.Instance.GetHero(_heroIndex);
		if (theHero == null || !(theHero.HeroData != null) || !(theHero.HeroData.HeroSubClass != null))
		{
			return;
		}
		Sprite stickerBase = hero.HeroData.HeroSubClass.StickerBase;
		if (!(emote0.sprite == stickerBase) && !(emote1.sprite == stickerBase) && !(emote2.sprite == stickerBase))
		{
			if (emote0.sprite == null)
			{
				emote0.sprite = stickerBase;
				emote0.gameObject.SetActive(value: true);
			}
			else if (emote1.sprite == null)
			{
				emote1.sprite = stickerBase;
				emote1.gameObject.SetActive(value: true);
			}
			else if (emote2.sprite == null)
			{
				emote2.sprite = stickerBase;
				emote2.gameObject.SetActive(value: true);
			}
		}
	}

	public void RemoveEmotes()
	{
		ShowEmoteButton(_state: false);
		RemoveEmoteIcons();
	}

	public void ShowEmoteTransform()
	{
		if (!emotes.transform.gameObject.activeSelf)
		{
			emotes.transform.gameObject.SetActive(value: true);
		}
	}

	public void RemoveEmoteIcons()
	{
		emote0.sprite = null;
		emote0.gameObject.SetActive(value: false);
		emote1.sprite = null;
		emote1.gameObject.SetActive(value: false);
		emote2.sprite = null;
		emote2.gameObject.SetActive(value: false);
	}

	public void RemoveEmoteIcon(byte _heroIndex)
	{
		Hero hero = MatchManager.Instance.GetHero(_heroIndex);
		if (theHero != null && theHero.HeroData != null && theHero.HeroData.HeroSubClass != null)
		{
			Sprite stickerBase = hero.HeroData.HeroSubClass.StickerBase;
			if (emote0.sprite == stickerBase)
			{
				emote0.sprite = null;
				emote0.gameObject.SetActive(value: false);
			}
			else if (emote1.sprite == stickerBase)
			{
				emote1.sprite = null;
				emote1.gameObject.SetActive(value: false);
			}
			else if (emote2.sprite == stickerBase)
			{
				emote2.sprite = null;
				emote2.gameObject.SetActive(value: false);
			}
		}
	}

	public bool HaveEmoteIcon(byte _heroIndex)
	{
		Hero hero = MatchManager.Instance.GetHero(_heroIndex);
		if (hero != null && hero.HeroData != null && hero.HeroData.HeroSubClass != null)
		{
			Sprite stickerBase = hero.HeroData.HeroSubClass.StickerBase;
			if (emote0.sprite == stickerBase)
			{
				return true;
			}
			if (emote1.sprite == stickerBase)
			{
				return true;
			}
			if (emote2.sprite == stickerBase)
			{
				return true;
			}
		}
		return false;
	}

	public void ShowPortrait(Sprite _sprite, int _position = 0)
	{
		portraits.gameObject.SetActive(value: true);
		if (_position == 1)
		{
			portraits.transform.localPosition = new Vector3(0.45f, 0.85f, portraits.transform.localPosition.z);
		}
		for (int i = 0; i < 4; i++)
		{
			if (!portrait[i].transform.gameObject.activeSelf)
			{
				portrait[i].transform.gameObject.SetActive(value: true);
				portrait[i].sprite = _sprite;
				break;
			}
		}
	}

	public void ClearMPMark()
	{
		for (int i = 0; i < 4; i++)
		{
			mpMark[i].transform.gameObject.SetActive(value: false);
		}
	}

	public void ShowMPMark(string _nick)
	{
		mpMarks.gameObject.SetActive(value: true);
		for (int i = 0; i < 4; i++)
		{
			if (!mpMark[i].transform.gameObject.activeSelf)
			{
				mpMark[i].color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(_nick));
				mpMark[i].transform.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ShowDifferences(CardData targetData)
	{
		if (targetData == null || cardData == null)
		{
			return;
		}
		if (cardData.EnergyCost != targetData.EnergyCost)
		{
			if (!diffEnergy.gameObject.activeSelf)
			{
				diffEnergy.gameObject.SetActive(value: true);
			}
		}
		else if (diffEnergy.gameObject.activeSelf)
		{
			diffEnergy.gameObject.SetActive(value: false);
		}
		if (cardData.Vanish != targetData.Vanish)
		{
			if (!diffVanish.gameObject.activeSelf)
			{
				diffVanish.gameObject.SetActive(value: true);
			}
		}
		else if (diffVanish.gameObject.activeSelf)
		{
			diffVanish.gameObject.SetActive(value: false);
		}
		if (cardData.Innate != targetData.Innate)
		{
			if (!diffInnate.gameObject.activeSelf)
			{
				diffInnate.gameObject.SetActive(value: true);
			}
		}
		else if (diffInnate.gameObject.activeSelf)
		{
			diffInnate.gameObject.SetActive(value: false);
		}
		if (cardData.TargetType != targetData.TargetType || cardData.TargetSide != targetData.TargetSide || cardData.TargetPosition != targetData.TargetPosition)
		{
			if (!diffTarget.gameObject.activeSelf)
			{
				diffTarget.gameObject.SetActive(value: true);
			}
		}
		else if (diffTarget.gameObject.activeSelf)
		{
			diffTarget.gameObject.SetActive(value: false);
		}
		if (cardData.EffectRequired != targetData.EffectRequired)
		{
			if (!diffRequire.gameObject.activeSelf)
			{
				diffRequire.gameObject.SetActive(value: true);
			}
		}
		else if (diffRequire.gameObject.activeSelf)
		{
			diffRequire.gameObject.SetActive(value: false);
		}
	}

	public void HideDifferences()
	{
		if (diffEnergy.gameObject.activeSelf)
		{
			diffEnergy.gameObject.SetActive(value: false);
		}
		if (diffTarget.gameObject.activeSelf)
		{
			diffTarget.gameObject.SetActive(value: false);
		}
		if (diffRequire.gameObject.activeSelf)
		{
			diffRequire.gameObject.SetActive(value: false);
		}
		if (diffVanish.gameObject.activeSelf)
		{
			diffVanish.gameObject.SetActive(value: false);
		}
		if (diffInnate.gameObject.activeSelf)
		{
			diffInnate.gameObject.SetActive(value: false);
		}
		if (diffSkillA.gameObject.activeSelf)
		{
			diffSkillA.gameObject.SetActive(value: false);
		}
		if (diffSkillB.gameObject.activeSelf)
		{
			diffSkillB.gameObject.SetActive(value: false);
		}
	}

	public void ShowKeyNum(bool _state, string _num = "", bool _disabled = false)
	{
		if (_state && !GameManager.Instance.ConfigKeyboardShortcuts && !MatchManager.Instance.KeyClickedCard)
		{
			_state = false;
		}
		if (keyTransform.gameObject.activeSelf != _state)
		{
			keyTransform.gameObject.SetActive(_state);
		}
		if (!_state)
		{
			return;
		}
		if (_num == "10")
		{
			_num = "0";
		}
		keyNumber.text = _num;
		if (!_disabled && (disableT.gameObject.activeSelf || cardBorderSR.color == redColor))
		{
			_disabled = true;
		}
		if (_disabled)
		{
			if (!keyRed.gameObject.activeSelf)
			{
				keyRed.gameObject.SetActive(value: true);
			}
			if (keyBackground.gameObject.activeSelf)
			{
				keyBackground.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (keyRed.gameObject.activeSelf)
			{
				keyRed.gameObject.SetActive(value: false);
			}
			if (!keyBackground.gameObject.activeSelf)
			{
				keyBackground.gameObject.SetActive(value: true);
			}
		}
		if (MatchManager.Instance.DeckCardsWindow.IsActive() || MatchManager.Instance.DiscardSelector.IsActive())
		{
			keyTransform.localPosition = new Vector3(0.75f, 1.1f, 0f);
		}
		else
		{
			keyTransform.localPosition = new Vector3(0f, 1.6f, 0f);
		}
	}
}
