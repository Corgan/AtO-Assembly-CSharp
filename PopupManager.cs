using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebSocketSharp;

public class PopupManager : MonoBehaviour
{
	public GameObject popupPrefab;

	private Image imageBg;

	private bool popupActive;

	private bool followMouse;

	private bool fastPopup;

	private Coroutine coroutine;

	private GameObject GO_Popup;

	private Transform canvasContainer;

	private CanvasScaler canvasContainerScaler;

	private RectTransform canvasRect;

	private Transform popupContainer;

	private RectTransform popupRect;

	private RectTransform textRect;

	private GameObject pop;

	private GameObject popTrait;

	private GameObject popTown;

	private GameObject popUnlocked;

	private GameObject popPerk;

	private TMP_Text popText;

	private TMP_Text popTraitText;

	private TMP_Text popTownText;

	private TMP_Text popPerkText;

	private List<string> initialPopupGOs;

	private string lastPop = "";

	private Transform lastTransform;

	private string popupText = "<line-height=30><size=22><color=#{2}>{0}</color></size>{3}</line-height>\n{1}";

	private Dictionary<string, GameObject> KeyNotesGO;

	private List<string> KeyNotesActive;

	private Transform theTF;

	private Vector3 destinationPosition;

	private Vector3 adjustPosition;

	private Vector3 theTFposition;

	private Vector3 absolutePosition;

	private bool absolutePositionStablished;

	private Vector2 followSizeDelta;

	private string position = "";

	private Color colorBad = new Color(0.49f, 0f, 0.04f, 1f);

	private Color colorGood = new Color(0.01f, 0.22f, 0.52f, 1f);

	private Color colorPlain = new Color(0.25f, 0.25f, 0.25f, 1f);

	private Color colorCardtype = new Color(0.35f, 0.16f, 0f, 1f);

	private Color colorVanish = new Color(0.36f, 0f, 0.47f, 1f);

	private Color colorInnate = new Color(0f, 0.44f, 0.14f, 1f);

	private Color colorTrait = new Color(0.33f, 0.3f, 0.23f, 1f);

	private Color colorUnlocked = new Color(0.02f, 0.65f, 0f, 1f);

	private Color colorTown = new Color(0.29f, 0.34f, 0.47f, 1f);

	private int adjustCardX = 60;

	public static PopupManager Instance { get; private set; }

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
		KeyNotesGO = new Dictionary<string, GameObject>();
		KeyNotesActive = new List<string>();
	}

	private void Start()
	{
		GO_Popup = UnityEngine.Object.Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
		canvasContainer = GO_Popup.transform.GetChild(0);
		canvasContainerScaler = canvasContainer.GetComponent<CanvasScaler>();
		canvasRect = canvasContainer.GetComponent<RectTransform>();
		popupContainer = canvasContainer.GetChild(0);
		popupRect = popupContainer.GetComponent<RectTransform>();
		popUnlocked = popupContainer.GetChild(0).gameObject;
		popUnlocked.gameObject.SetActive(value: false);
		pop = popupContainer.GetChild(1).gameObject;
		popText = pop.transform.Find("Background/Text").GetComponent<TMP_Text>();
		textRect = popText.GetComponent<RectTransform>();
		imageBg = pop.transform.GetChild(0).GetComponent<Image>();
		imageBg.color = colorPlain;
		pop.SetActive(value: false);
		GO_Popup.SetActive(value: false);
		initialPopupGOs = new List<string>();
		foreach (Transform item in popupContainer)
		{
			initialPopupGOs.Add(item.gameObject.name);
		}
		CreateKeyNotes();
		Resize();
	}

	private void Update()
	{
		if (!popupActive)
		{
			return;
		}
		if (followMouse)
		{
			if (theTF != null)
			{
				if (position == "followright")
				{
					adjustPosition = new Vector3(followSizeDelta.x + (float)adjustCardX, theTF.GetComponent<BoxCollider2D>().size.y, 0f);
				}
				else
				{
					adjustPosition = new Vector3(0f - followSizeDelta.x - (float)adjustCardX, theTF.GetComponent<BoxCollider2D>().size.y, 0f);
				}
				destinationPosition = new Vector3(theTF.position.x * 100f + adjustPosition.x * 0.9f, theTF.position.y * 100f + adjustPosition.y * 1f - followSizeDelta.y * 5f * 0.1f, 1f);
				destinationPosition = RecalcPositionLimits(destinationPosition);
				if (Vector3.Distance(popupContainer.localPosition, destinationPosition) > 1f)
				{
					popupContainer.localPosition = Vector3.Lerp(popupContainer.localPosition, destinationPosition, 8f * Time.deltaTime);
				}
				return;
			}
			Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (position == "followdown")
			{
				destinationPosition = new Vector3(vector.x * 100f, (vector.y - 1.5f - followSizeDelta.y * 0.001f) * 100f, 1f);
			}
			else if (position == "followdown2")
			{
				destinationPosition = new Vector3((vector.x - 3f) * 100f, (vector.y - 5.4f - followSizeDelta.y * 0.001f) * 100f, 1f);
			}
			else
			{
				destinationPosition = new Vector3(vector.x * 100f, (vector.y + 0.2f) * 100f, 1f);
			}
			float num = followSizeDelta.x * 0.6f;
			if (destinationPosition.x * 0.01f - num * 0.01f < Globals.Instance.sizeW * -0.48f)
			{
				destinationPosition = new Vector3(Globals.Instance.sizeW * -0.48f * 100f + num, destinationPosition.y, destinationPosition.z);
			}
			else if (destinationPosition.x * 0.01f + num * 0.01f > Globals.Instance.sizeW * 0.48f)
			{
				destinationPosition = new Vector3(Globals.Instance.sizeW * 0.48f * 100f - num, destinationPosition.y, destinationPosition.z);
			}
			if (destinationPosition.y * 0.01f + followSizeDelta.y * 0.01f > Globals.Instance.sizeH * 0.45f)
			{
				destinationPosition = new Vector3(destinationPosition.x, Globals.Instance.sizeH * 0.45f * 100f - followSizeDelta.y, destinationPosition.z);
			}
			if (Vector3.Distance(popupContainer.localPosition, destinationPosition) > 1f)
			{
				popupContainer.localPosition = Vector3.Lerp(popupContainer.localPosition, destinationPosition, 8f * Time.deltaTime);
			}
			return;
		}
		if (theTF != null)
		{
			Vector3 zero = Vector3.zero;
			if (theTFposition != Vector3.zero)
			{
				zero = theTFposition - theTF.localPosition;
				theTFposition = theTF.localPosition;
				destinationPosition -= new Vector3(zero.x * 10f, zero.y * 10f, 0f);
			}
		}
		float num2 = followSizeDelta.x * 0.5f + 50f;
		if (destinationPosition.x * 0.01f - num2 * 0.01f < Globals.Instance.sizeW * -0.5f)
		{
			destinationPosition = new Vector3(Globals.Instance.sizeW * -0.485f * 100f + num2, destinationPosition.y, destinationPosition.z);
		}
		else if (destinationPosition.x * 0.01f + num2 * 0.01f > Globals.Instance.sizeW * 0.5f)
		{
			destinationPosition = new Vector3(Globals.Instance.sizeW * 0.485f * 100f - num2, destinationPosition.y, destinationPosition.z);
		}
		else if (destinationPosition.y * 0.01f + followSizeDelta.y * 0.01f > Globals.Instance.sizeH * 0.45f)
		{
			destinationPosition = new Vector3(destinationPosition.x, Globals.Instance.sizeH * 0.45f * 100f - followSizeDelta.y, destinationPosition.z);
		}
		if (Vector3.Distance(popupContainer.localPosition, destinationPosition) > 1f)
		{
			popupContainer.localPosition = Vector3.Lerp(popupContainer.localPosition, destinationPosition, 8f * Time.deltaTime);
			return;
		}
		popupActive = false;
		popupContainer.localPosition = new Vector3(Mathf.CeilToInt(destinationPosition.x), Mathf.CeilToInt(destinationPosition.y), popupContainer.localPosition.z);
	}

	public Vector3 GetPopupActiveCoordinates()
	{
		return GameManager.Instance.cameraMain.ScreenToWorldPoint(popupContainer.position);
	}

	public Transform GetPopupT()
	{
		if (GO_Popup != null)
		{
			return GO_Popup.transform;
		}
		return null;
	}

	public void Resize()
	{
		if (canvasContainerScaler != null)
		{
			if (Globals.Instance.scale < 1f)
			{
				canvasContainerScaler.matchWidthOrHeight = 1f;
			}
			else
			{
				canvasContainerScaler.matchWidthOrHeight = 0f;
			}
		}
	}

	public void StablishPopupPositionSize(Vector3 position, Vector3 scale)
	{
		absolutePosition = position;
		popupContainer.transform.localScale = scale;
		absolutePositionStablished = true;
		popupActive = false;
	}

	private IEnumerator ShowPopup()
	{
		ClosePopup();
		popupContainer.transform.localScale = new Vector3(1f, 1f, 1f);
		if (theTF != null)
		{
			theTFposition = theTF.localPosition;
		}
		else
		{
			theTFposition = Vector3.zero;
		}
		if (TomeManager.Instance.IsActive())
		{
			fastPopup = true;
		}
		if (!fastPopup)
		{
			yield return Globals.Instance.WaitForSeconds(0.9f);
		}
		else
		{
			yield return Globals.Instance.WaitForSeconds(0.15f);
		}
		if (position != "follow" && position != "followdown" && position != "followdown2" && theTF == null)
		{
			yield break;
		}
		followMouse = false;
		if (position == "right" || position == "followright")
		{
			adjustPosition = new Vector3(popupContainer.GetComponent<RectTransform>().sizeDelta.x + 80f, theTF.GetComponent<BoxCollider2D>().size.y, 0f);
			destinationPosition = new Vector3(theTF.position.x * 100f + adjustPosition.x * 0.8f, theTF.position.y * 100f + adjustPosition.y * 6f, 1f);
			if (position == "followright")
			{
				followMouse = true;
			}
		}
		else if (position == "left" || position == "followleft")
		{
			adjustPosition = new Vector3(0f - popupContainer.GetComponent<RectTransform>().sizeDelta.x - 80f, theTF.GetComponent<BoxCollider2D>().size.y, 0f);
			destinationPosition = new Vector3(theTF.position.x * 100f + adjustPosition.x * 0.8f, theTF.position.y * 100f + adjustPosition.y * 6f, 1f);
			if (position == "followleft")
			{
				followMouse = true;
			}
		}
		else if (position == "centerright")
		{
			adjustPosition = new Vector3(popupContainer.GetComponent<RectTransform>().sizeDelta.x, theTF.GetComponent<BoxCollider2D>().size.y, 0f);
			destinationPosition = new Vector3(theTF.position.x * 100f + adjustPosition.x * 0.7f, theTF.position.y * 100f - adjustPosition.y * 130f * 0.5f, 1f);
		}
		else if (position == "center")
		{
			adjustPosition = new Vector3(0f, 30f, 0f);
			Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (theTF != null)
			{
				destinationPosition = new Vector3(theTF.position.x * 100f + adjustPosition.x, theTF.position.y * 100f + adjustPosition.y, 1f);
				if (destinationPosition.x * 0.01f > (float)Screen.width * 0.005f)
				{
					float num = destinationPosition.x * 0.01f - (float)Screen.width * 0.005f;
					destinationPosition -= new Vector3(num * 100f, 0f, 0f);
				}
				if (destinationPosition.x * 0.01f < (float)(-Screen.width) * 0.005f)
				{
					float num2 = (float)(-Screen.width) * 0.005f - destinationPosition.x * 0.01f;
					destinationPosition += new Vector3(num2 * 100f, 0f, 0f);
				}
			}
			else
			{
				Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				destinationPosition = new Vector3(vector.x * 100f, vector.y * 100f, 1f);
			}
		}
		else if (position == "follow")
		{
			Vector3 vector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			destinationPosition = new Vector3(vector2.x * 100f, (vector2.y + 0.2f) * 100f, 1f);
			followMouse = true;
		}
		else if (position == "followdown")
		{
			Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			destinationPosition = new Vector3(vector3.x * 100f, (vector3.y - 1f) * 100f, 1f);
			followMouse = true;
		}
		else if (position == "followdown2")
		{
			Vector3 vector4 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			destinationPosition = new Vector3((vector4.x - 2f) * 100f, (vector4.y - 4f) * 100f, 1f);
			followMouse = true;
		}
		destinationPosition = new Vector3(Mathf.CeilToInt(destinationPosition.x), Mathf.CeilToInt(destinationPosition.y), 1f);
		GO_Popup.SetActive(value: true);
		popupContainer.localPosition = new Vector3(1000f, 1000f, -10f);
		coroutine = StartCoroutine(CalcSize());
	}

	private IEnumerator CalcSize()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		float num = ((!popupContainer.GetComponent<RectTransform>().sizeDelta.y.Equals(62.05f)) ? (followSizeDelta = popupContainer.GetComponent<RectTransform>().sizeDelta) : (followSizeDelta = pop.GetComponent<RectTransform>().sizeDelta)).y * 0.5f;
		if (!absolutePositionStablished)
		{
			if (position == "right" || position == "left" || position == "followleft" || position == "followright")
			{
				destinationPosition = new Vector3(destinationPosition.x, destinationPosition.y - num, destinationPosition.z);
			}
			destinationPosition = RecalcPositionLimits(destinationPosition);
			if (position == "followdown")
			{
				destinationPosition -= new Vector3(0f, followSizeDelta.y * 0.5f - 0.2f, 0f);
			}
		}
		else
		{
			destinationPosition = new Vector3(absolutePosition.x, absolutePosition.y - num, absolutePosition.z);
			absolutePositionStablished = false;
		}
		popupContainer.localPosition = destinationPosition - new Vector3(0f, 5f, 0f);
		popupActive = true;
	}

	private Vector3 RecalcPositionLimits(Vector3 destinationPosition)
	{
		if (destinationPosition.x * 0.01f < Globals.Instance.sizeW * -0.5f)
		{
			destinationPosition = new Vector3(Globals.Instance.sizeW * -0.5f * 100f, destinationPosition.y, destinationPosition.z);
		}
		else if (destinationPosition.x * 0.01f > Globals.Instance.sizeW * 0.5f)
		{
			destinationPosition = new Vector3(Globals.Instance.sizeW * 0.5f * 100f, destinationPosition.y, destinationPosition.z);
		}
		else if (destinationPosition.y * 0.01f + followSizeDelta.y * 0.01f > Globals.Instance.sizeH * 0.45f)
		{
			destinationPosition = new Vector3(destinationPosition.x, Globals.Instance.sizeH * 0.45f * 100f - followSizeDelta.y, destinationPosition.z);
		}
		return destinationPosition;
	}

	public void ClosePopup()
	{
		if (!(this == null))
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}
			popupActive = false;
			absolutePositionStablished = false;
			if (popUnlocked != null)
			{
				popUnlocked.gameObject.SetActive(value: false);
			}
			if (GO_Popup != null)
			{
				GO_Popup.SetActive(value: false);
			}
		}
	}

	public bool IsActive()
	{
		return GO_Popup.gameObject.activeSelf;
	}

	private void FollowRight()
	{
		position = "followright";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void FollowPop()
	{
		position = "follow";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void FollowPopDown()
	{
		position = "followdown";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void FollowPopDown2()
	{
		position = "followdown2";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void CenterPop()
	{
		position = "center";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void RightPop()
	{
		position = "right";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void LeftPop()
	{
		position = "left";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void Bottom()
	{
		position = "bottom";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void CenterRightPop()
	{
		position = "centerright";
		coroutine = StartCoroutine(ShowPopup());
	}

	private void ShowInPosition(string _position)
	{
		position = _position;
		coroutine = StartCoroutine(ShowPopup());
	}

	public void SetCard(Transform tf, CardData cardData, List<KeyNotesData> cardDataKeyNotes, string position = "right", bool fast = false)
	{
		if (CardScreenManager.Instance.IsActive())
		{
			fast = true;
		}
		else if (MatchManager.Instance == null)
		{
			fast = true;
		}
		fastPopup = fast;
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		if (position == "right" && tf != null && tf.transform.position.x > (float)Screen.width * 0.005f - 6f * ((float)Screen.width / 1920f))
		{
			position = "left";
		}
		if (!PlayerManager.Instance.IsCardUnlocked(cardData.Id))
		{
			popUnlocked.gameObject.SetActive(value: true);
		}
		else
		{
			popUnlocked.gameObject.SetActive(value: false);
		}
		bool flag = false;
		if (cardData.EnergyReductionToZeroPermanent || cardData.EnergyReductionToZeroTemporal || cardData.EnergyReductionPermanent > 0 || cardData.EnergyReductionTemporal > 0 || cardData.ExhaustCounter > 0)
		{
			flag = true;
		}
		if (lastPop == cardData.Id && !flag)
		{
			DrawPopup(tf);
			imageBg.color = colorCardtype;
			ShowInPosition(position);
			return;
		}
		popTrait.SetActive(value: false);
		popTown.SetActive(value: false);
		popPerk.SetActive(value: false);
		adjustCardX = 60;
		if (cardDataKeyNotes == null)
		{
			return;
		}
		int count = cardDataKeyNotes.Count;
		if (count <= 0 && cardData.CardType == Enums.CardType.None)
		{
			return;
		}
		DrawPopup(tf);
		imageBg.color = colorCardtype;
		if (cardData.CardType != Enums.CardType.None)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<line-height=140%><voffset=-1><size=26><sprite name=cards></size></voffset>");
			stringBuilder.Append("<size=21><color=#fc0><b>");
			stringBuilder.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), cardData.CardType)));
			stringBuilder.Append("</b></color>");
			for (int i = 0; i < cardData.CardTypeAux.Length; i++)
			{
				if (cardData.CardTypeAux[i] != Enums.CardType.None)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), cardData.CardTypeAux[i])));
				}
			}
			stringBuilder.Append("</size></line-height>");
			if (cardData.CardType == Enums.CardType.Enchantment)
			{
				stringBuilder.Append("<br><size=17><color=#AAAAAA>");
				stringBuilder.Append(Texts.Instance.GetText("maximumEnchantments"));
				stringBuilder.Append("</size></color>");
			}
			if (flag)
			{
				stringBuilder.Append("<br><sprite name=energy>");
				StringBuilder stringBuilder2 = new StringBuilder();
				if (cardData.EnergyReductionToZeroPermanent)
				{
					stringBuilder2.Append(" <color=#FFF>");
					stringBuilder2.Append(string.Format(Texts.Instance.GetText("cardsCost"), 0));
					stringBuilder2.Append("</color>");
				}
				else if (cardData.EnergyReductionToZeroTemporal)
				{
					stringBuilder2.Append(" <color=#FFF>");
					stringBuilder2.Append(string.Format(Texts.Instance.GetText("cardsCost"), 0));
					stringBuilder2.Append("</color>");
					stringBuilder2.Append(" (");
					stringBuilder2.Append(Texts.Instance.GetText("cardsCostUntilDiscarded"));
					stringBuilder2.Append(")");
				}
				if (cardData.EnergyReductionPermanent > 0 || cardData.EnergyReductionTemporal > 0)
				{
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.Append(" <voffset=1.5>|</voffset>");
					}
					int num = cardData.EnergyReductionPermanent + cardData.EnergyReductionTemporal;
					stringBuilder2.Append(" <color=#FFF>");
					stringBuilder2.Append(string.Format(Texts.Instance.GetText("cardsCostReducedBy"), num));
					stringBuilder2.Append("</color> ");
					if (cardData.EnergyReductionPermanent == 0)
					{
						stringBuilder2.Append("(");
						stringBuilder2.Append(Texts.Instance.GetText("cardsCostUntilDiscarded"));
						stringBuilder2.Append(")");
					}
					else if (cardData.EnergyReductionTemporal > 0)
					{
						stringBuilder2.Append("(");
						stringBuilder2.Append(cardData.EnergyReductionPermanent);
						stringBuilder2.Append(" + ");
						stringBuilder2.Append(cardData.EnergyReductionTemporal);
						stringBuilder2.Append(" ");
						stringBuilder2.Append(Texts.Instance.GetText("cardsCostUntilDiscarded"));
						stringBuilder2.Append(") ");
					}
				}
				if (cardData.ExhaustCounter > 0)
				{
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.Append(" <voffset=1.5>|</voffset>");
					}
					stringBuilder2.Append(" <color=#EC75D3>");
					stringBuilder2.Append(Texts.Instance.GetText("exhaustion"));
					stringBuilder2.Append(" +");
					stringBuilder2.Append(cardData.ExhaustCounter);
					stringBuilder2.Append("</color>");
				}
				stringBuilder.Append(stringBuilder2.ToString());
			}
			if (cardData.Sku != "")
			{
				stringBuilder.Append("<br><size=16><color=#66CCBB>");
				stringBuilder.Append(SteamManager.Instance.GetDLCName(cardData.Sku));
				stringBuilder.Append(" ");
				stringBuilder.Append(Texts.Instance.GetText("dlcAcronymForCharSelection"));
				stringBuilder.Append("</color></size>");
			}
			TextAdjust(stringBuilder.ToString());
			stringBuilder = null;
			pop.SetActive(value: true);
		}
		else
		{
			pop.SetActive(value: false);
		}
		CleanKeyNotes();
		if (count > 0)
		{
			for (int j = 0; j < count; j++)
			{
				if (cardDataKeyNotes[j] != null)
				{
					KeyNotesActive.Add(cardDataKeyNotes[j].Id);
					KeyNotesGO[cardDataKeyNotes[j].Id].SetActive(value: true);
				}
			}
		}
		else
		{
			float x = 300f;
			adjustCardX = -30;
			RectTransform component = popText.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(x, component.sizeDelta.y);
			popText.ForceMeshUpdate(ignoreActiveState: true);
		}
		ShowInPosition(position);
		if (flag)
		{
			lastPop = null;
		}
		else
		{
			lastPop = cardData.Id;
		}
	}

	public void ShowKeyNote(Transform tf, string keyNote, string position = "right", bool fast = false)
	{
		fastPopup = fast;
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		popTrait.SetActive(value: false);
		popTown.SetActive(value: false);
		popPerk.SetActive(value: false);
		pop.SetActive(value: false);
		CleanKeyNotes();
		KeyNotesActive.Add(keyNote);
		KeyNotesGO[keyNote].SetActive(value: true);
		DrawPopup(tf);
		ShowInPosition(position);
		lastPop = keyNote;
	}

	public void SetTrait(TraitData td, bool includeDescription = true)
	{
		if (td == null)
		{
			return;
		}
		fastPopup = true;
		DrawPopup();
		pop.SetActive(value: false);
		popPerk.SetActive(value: false);
		popTown.SetActive(value: false);
		if (includeDescription)
		{
			string[] array = new string[4] { td.TraitName, null, null, null };
			if (td.TraitCard == null)
			{
				array[1] = td.Description;
			}
			else
			{
				array[1] = string.Format(Texts.Instance.GetText("traitAddCard"), td.TraitCard.CardName);
			}
			array[2] = "D4AC5B";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" <voffset=0><size=21><sprite name=experience></size></voffset> ");
			StringBuilder stringBuilder2 = stringBuilder;
			string format = popupText;
			object[] args = array;
			stringBuilder2.Append(string.Format(format, args));
			popTraitText.text = stringBuilder.ToString();
			popTrait.SetActive(value: true);
			stringBuilder = null;
		}
		CleanKeyNotes();
		int count = td.KeyNotes.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				if (td.KeyNotes[i] != null)
				{
					KeyNotesActive.Add(td.KeyNotes[i].Id);
					KeyNotesGO[td.KeyNotes[i].Id].SetActive(value: true);
				}
			}
		}
		if (position == "followdown")
		{
			FollowPopDown();
		}
		else
		{
			FollowPop();
		}
		lastPop = td.TraitName;
	}

	public void SetPerk(string title, string text, string keynote = "")
	{
		fastPopup = true;
		DrawPopup();
		pop.SetActive(value: false);
		popTown.SetActive(value: false);
		popTrait.SetActive(value: false);
		string[] array = new string[4] { text, "", "D4AC5B", "" };
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = stringBuilder;
		string format = popupText;
		object[] args = array;
		stringBuilder2.Append(string.Format(format, args));
		popPerkText.text = stringBuilder.ToString();
		popPerk.SetActive(value: true);
		stringBuilder = null;
		CleanKeyNotes();
		if (keynote != "")
		{
			keynote = keynote.ToLower();
			KeyNotesActive.Add(keynote);
			if (KeyNotesGO.ContainsKey(keynote))
			{
				KeyNotesGO[keynote].SetActive(value: true);
			}
		}
		FollowPop();
		lastPop = title;
	}

	public void SetTown(string idTitle, string idDescription, bool showDisableText)
	{
		if (lastPop == idTitle)
		{
			DrawPopup();
			FollowPop();
			return;
		}
		fastPopup = true;
		DrawPopup();
		string[] array = new string[4]
		{
			Texts.Instance.GetText(idTitle),
			Texts.Instance.GetText(idDescription),
			"FFC88F",
			""
		};
		StringBuilder stringBuilder = new StringBuilder();
		string value = "";
		switch (idTitle)
		{
		case "craftCards":
			value = "cards";
			break;
		case "upgradeCards":
			value = "nodeUpgrade";
			break;
		case "removeCards":
			value = "nodeHeal";
			break;
		case "divinationCards":
			value = "nodeDivination";
			break;
		case "buyItems":
			value = "nodeShop";
			break;
		}
		stringBuilder.Append("<line-height=20><br></line-height>");
		stringBuilder.Append(" <voffset=-3><size=30><sprite name=");
		stringBuilder.Append(value);
		stringBuilder.Append("></size></voffset> ");
		string format = popupText;
		object[] args = array;
		stringBuilder.Append(string.Format(format, args));
		if (showDisableText)
		{
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("disabledInSingularity"));
		}
		pop.SetActive(value: false);
		popTrait.SetActive(value: false);
		popPerk.SetActive(value: false);
		popTownText.fontSize = 22f;
		popTownText.text = stringBuilder.ToString();
		popTown.SetActive(value: true);
		CleanKeyNotes();
		FollowPop();
		lastPop = idTitle;
	}

	private void AddReplacement(Dictionary<string, string> replacements, string placeholder, int value)
	{
		replacements[placeholder] = value.ToString();
	}

	private void AddReplacement(Dictionary<string, string> replacements, string placeholder, string value)
	{
		replacements[placeholder] = value;
	}

	private int CalculateChargesMultiplier(float multiplier, int charges)
	{
		return Functions.FuncRoundToInt(multiplier * (float)charges);
	}

	private int ParseCharges(string charges)
	{
		return int.Parse(charges);
	}

	private int CalculateChargesAux(int charges, float chargesAuxNeedForOne)
	{
		if (chargesAuxNeedForOne == 0f)
		{
			return 0;
		}
		return Mathf.FloorToInt((float)charges / chargesAuxNeedForOne);
	}

	private int CalculateAuraDamage(int charges, float damagePerStack, float increasedDirectDamageChargesMultiplierNeededForOne = 0f)
	{
		if (increasedDirectDamageChargesMultiplierNeededForOne == 0f)
		{
			return Functions.FuncRoundToInt((float)charges * damagePerStack);
		}
		float num = 1f / increasedDirectDamageChargesMultiplierNeededForOne;
		int num2 = 0;
		if (damagePerStack < 0f)
		{
			return -1 * Mathf.FloorToInt(Mathf.Abs(num * (float)charges * damagePerStack));
		}
		return Mathf.FloorToInt(num * (float)charges * damagePerStack);
	}

	private int CalculateResistAux(int charges, float resistModifiedPercentage)
	{
		return Functions.FuncRoundToInt((float)charges * resistModifiedPercentage);
	}

	private void UpdateReplacements(Dictionary<string, string> replacements, int charges, int chargesSecondary, AuraCurseData acData, Character theChar)
	{
		float num = 1f / (float)acData.CharacterStatChargesMultiplierNeededForOne;
		int value = Mathf.FloorToInt(Mathf.Abs(1f / 14f * (float)charges));
		AddReplacement(replacements, "<ChargesValueBy14>", value);
		AddReplacement(replacements, "<ChargesCurrent>", charges);
		AddReplacement(replacements, "<ChargesCurrentHalf>", Functions.FuncRoundToInt((float)charges * 0.5f));
		AddReplacement(replacements, "<ChargesMultiplier>", CalculateChargesMultiplier(acData.ChargesMultiplierDescription, charges));
		if (chargesSecondary != -1000)
		{
			AddReplacement(replacements, "<ChargesMultiplier_sec>", CalculateChargesMultiplier(acData.ChargesMultiplierDescription, chargesSecondary));
		}
		AddReplacement(replacements, "<ChargesMultiplierHalf>", CalculateChargesMultiplier(acData.ChargesMultiplierDescription, Functions.FuncRoundToInt((float)charges * 0.5f)));
		AddReplacement(replacements, "<ChargesAux1>", CalculateChargesAux(charges, acData.ChargesAuxNeedForOne1));
		AddReplacement(replacements, "<CustomAuxValue>", acData.CustomAuxValue);
		if (chargesSecondary != -1000)
		{
			AddReplacement(replacements, "<ChargesAux1_sec>", CalculateChargesAux(chargesSecondary, acData.ChargesAuxNeedForOne1));
		}
		AddReplacement(replacements, "<ChargesAux2>", CalculateChargesAux(charges, acData.ChargesAuxNeedForOne2));
		if (chargesSecondary != -1000)
		{
			AddReplacement(replacements, "<ChargesAux2_sec>", CalculateChargesAux(chargesSecondary, acData.ChargesAuxNeedForOne2));
		}
		AddReplacement(replacements, "<AuraDamageIncreasedPerStack>", CalculateAuraDamage(charges, acData.AuraDamageIncreasedPerStack));
		AddReplacement(replacements, "<AuraDamageIncreasedPerStack2>", CalculateAuraDamage(charges, acData.AuraDamageIncreasedPerStack2));
		AddReplacement(replacements, "<AuraDamageIncreasedPercentPerStack>", CalculateAuraDamage(charges, acData.AuraDamageIncreasedPercentPerStack));
		AddReplacement(replacements, "<IncreasedDirectDamageReceivedPerStack>", CalculateAuraDamage(charges, acData.IncreasedDirectDamageReceivedPerStack, acData.IncreasedDirectDamageChargesMultiplierNeededForOne));
		AddReplacement(replacements, "<IncreasedDirectDamageReceivedPerStack2>", CalculateAuraDamage(charges, acData.IncreasedDirectDamageReceivedPerStack2, acData.IncreasedDirectDamageChargesMultiplierNeededForOne));
		int value2 = CalculateAuraDamage(charges, acData.AuraDamageIncreasedPercentPerStack);
		AddReplacement(replacements, "<DamageAux1>", Mathf.Abs(value2));
		if (chargesSecondary != -1000)
		{
			value2 = CalculateAuraDamage(chargesSecondary, acData.AuraDamageIncreasedPercentPerStack);
			AddReplacement(replacements, "<DamageAux1_sec>", Mathf.Abs(value2));
		}
		int value3 = CalculateAuraDamage(charges, acData.AuraDamageIncreasedPercentPerStack2);
		AddReplacement(replacements, "<DamageAux2>", Mathf.Abs(value3));
		if (chargesSecondary != -1000)
		{
			value3 = CalculateAuraDamage(chargesSecondary, acData.AuraDamageIncreasedPercentPerStack2);
			AddReplacement(replacements, "<DamageAux2_sec>", Mathf.Abs(value3));
		}
		AddReplacement(replacements, "<ResistAux1>", Math.Abs(CalculateResistAux(charges, acData.ResistModifiedPercentagePerStack)));
		AddReplacement(replacements, "<ResistAux2>", Math.Abs(CalculateResistAux(charges, acData.ResistModifiedPercentagePerStack2)));
		int value4 = acData.MaxCharges;
		if (MadnessManager.Instance.IsMadnessTraitActive("restrictedpower") || AtOManager.Instance.IsChallengeTraitActive("restrictedpower"))
		{
			value4 = acData.MaxMadnessCharges;
		}
		AddReplacement(replacements, "<MaxCharges>", value4);
		AddReplacement(replacements, "<HealReceivedPercent>", acData.HealReceivedPercent);
		AddReplacement(replacements, "<CharacterStatModifiedValue>", acData.CharacterStatModifiedValue);
		string text = Mathf.FloorToInt((float)charges * acData.CharacterStatModifiedValuePerStack).ToString();
		if (acData.CharacterStatModifiedValuePerStack > 0f)
		{
			text = "+" + text;
		}
		if (text == "+0")
		{
			text = "0";
		}
		replacements["<CharacterStatModifiedPerStack>"] = text;
		string text2 = "";
		text2 = ((!(acData.CharacterStatModifiedValuePerStack < 0f)) ? ("+" + Mathf.FloorToInt(num * (float)charges * acData.CharacterStatModifiedValuePerStack)) : ("-" + Mathf.FloorToInt(Mathf.Abs(num * (float)charges * acData.CharacterStatModifiedValuePerStack))));
		replacements["<CharacterStatModifiedValuePerStackTotal>"] = text2;
		AddReplacement(replacements, "<DamageWhenConsumed>", acData.DamageWhenConsumed);
		AddReplacement(replacements, "<DamageSidesWhenConsumed>", acData.DamageSidesWhenConsumed);
		AddReplacement(replacements, "<HealAttackerConsumeCharges>", acData.HealAttackerConsumeCharges);
		int value5 = Functions.FuncRoundToInt(acData.ResistModifiedValue + (float)CalculateResistAux(charges, acData.ResistModifiedPercentagePerStack));
		AddReplacement(replacements, "<Resistance1>", value5);
		int num2 = CalculateAuraDamage(charges, acData.DamageWhenConsumedPerCharge);
		if (acData.Id == "scourge" && !theChar.IsHero)
		{
			if (AtOManager.Instance.TeamHavePerk("mainperkscourge0b"))
			{
				num2 += theChar.GetAuraCharges("burn");
			}
			if (AtOManager.Instance.TeamHavePerk("mainperkscourge0c"))
			{
				num2 += theChar.GetAuraCharges("insane");
			}
		}
		AddReplacement(replacements, "<DamageWhenConsumedPerCharge>", num2);
		AddReplacement(replacements, "<ExplodeAtStacks>", acData.ExplodeAtStacks);
		if (acData.ACOnExplode != null)
		{
			AddReplacement(replacements, "<ACOnExplode>", Texts.Instance.GetText(acData.ACOnExplode.Id));
		}
		AddReplacement(replacements, "<HealReceivedPercentPerStack>", CalculateResistAux(charges, acData.HealReceivedPercentPerStack));
		AddReplacement(replacements, "<HealDonePercentPerStack>", CalculateResistAux(charges, acData.HealDonePercentPerStack));
		AddReplacement(replacements, "<HealPerChargeOnExplode>", CalculateResistAux(charges, acData.HealPerChargeOnExplode));
		if (acData.Id == "zeal")
		{
			int num3 = 0;
			if (theChar != null)
			{
				num3 = Functions.FuncRoundToInt((float)theChar.GetAuraCharges("burn") * acData.AuraDamageIncreasedPercentPerStack);
				if (theChar.HaveTrait("righteousflame"))
				{
					num3 += Functions.FuncRoundToInt((float)charges * acData.AuraDamageIncreasedPercentPerStack2);
				}
			}
			AddReplacement(replacements, "<DamageZeal>", num3);
		}
		if (acData.Id == "rust")
		{
			AddReplacement(replacements, "<RustEffectIncrease>", "50%");
			if (AtOManager.Instance.TeamHavePerk("mainperkrust0b") && !theChar.IsHero)
			{
				AddReplacement(replacements, "<RustEffectIncrease>", Texts.Instance.GetText("customAuxValuePerCharge").Replace("{0}", "20"));
			}
		}
	}

	private string ApplyReplacements(string textDescription, Dictionary<string, string> replacements)
	{
		StringBuilder stringBuilder = new StringBuilder(textDescription);
		foreach (KeyValuePair<string, string> replacement in replacements)
		{
			stringBuilder.Replace(replacement.Key, replacement.Value);
		}
		return stringBuilder.ToString();
	}

	public void SetAuraCurse(Transform tf, string acDataId, string charges, bool fast = false, string charId = "")
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		fastPopup = fast;
		if (charges == "" || acDataId.IsNullOrEmpty())
		{
			return;
		}
		AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(acDataId);
		AuraCurseData auraCurseData2 = null;
		if ((bool)MatchManager.Instance)
		{
			Character characterById = MatchManager.Instance.GetCharacterById(charId);
			if (characterById != null)
			{
				auraCurseData2 = Globals.Instance.GetAuraCurseData(acDataId);
				auraCurseData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", auraCurseData.Id, characterById, characterById, forPopup: true);
			}
		}
		if (lastPop == auraCurseData.ACName + charges && lastTransform == tf)
		{
			DrawPopup(tf);
			if (auraCurseData.IsAura)
			{
				imageBg.color = colorGood;
			}
			else
			{
				imageBg.color = colorBad;
			}
			CenterPop();
			return;
		}
		DrawPopup(tf);
		CleanKeyNotes();
		string[] array = new string[4];
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<b>");
		stringBuilder.Append(auraCurseData.ACName);
		stringBuilder.Append("</b>");
		array[0] = stringBuilder.ToString();
		int num = -1000;
		string text = auraCurseData.Description;
		Character character = null;
		if ((bool)MatchManager.Instance)
		{
			character = MatchManager.Instance.GetCharacterById(charId);
			if (character != null)
			{
				stringBuilder.Clear();
				if (auraCurseData.Id == "bleed" && auraCurseData.ConsumedAtTurn)
				{
					text = Texts.Instance.GetText("bleedendturn_description", "auracurse");
				}
				if (auraCurseData.Id == "bless")
				{
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "MainperkBless1a"))
					{
						text = Texts.Instance.GetText("blessNoHeal_description", "auracurse");
					}
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "MainperkBless1b"))
					{
						text = text + "<br3>" + Texts.Instance.GetText("healingDonePercentPerStack");
					}
					if (AtOManager.Instance.TeamHavePerk("MainperkBless1c") && character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("holyResistance");
					}
				}
				else if (auraCurseData.Id == "burn")
				{
					if (!character.IsHero)
					{
						if (AtOManager.Instance.TeamHavePerk("mainperkBurn2c"))
						{
							text = Texts.Instance.GetText("burnColdDamage_description", "auracurse");
						}
						if (character.GetAuraCurseTotal(_auras: true, _curses: true) <= 2)
						{
							text = text + "<br3>" + Texts.Instance.GetText("burnDoubleDamage_description", "auracurse");
						}
					}
					else if (character.HaveTrait("righteousflame"))
					{
						int num2 = Functions.FuncRoundToInt((float)int.Parse(charges) * auraCurseData.HealWhenConsumedPerCharge);
						text = Texts.Instance.GetText("regeneration_description", "auracurse").Replace("<ChargesMultiplier>", num2.ToString());
					}
				}
				else if (auraCurseData.Id == "chill")
				{
					if (AtOManager.Instance.TeamHavePerk("MainperkChill2b") && !character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("bluntResistanceNegative").Replace("ResistAux1", "ResistAux2");
					}
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "MainperkChill2d"))
					{
						int num3 = Mathf.FloorToInt(1f / 14f * (float)int.Parse(charges));
						if (num3 > 0)
						{
							text = text + "<br3>" + string.Format(Texts.Instance.GetText("chillReinforce_description", "auracurse"), num3);
						}
					}
				}
				else if (auraCurseData.Id == "crack")
				{
					if (!character.IsHero)
					{
						if (AtOManager.Instance.TeamHavePerk("mainperkcrack2b"))
						{
							int num4 = Functions.FuncRoundToInt(float.Parse(charges) * 0.5f);
							if (num4 > 0)
							{
								text = text + "<br3>" + string.Format(Texts.Instance.GetText("crackBlock_description", "auracurse"), num4);
							}
						}
						if (AtOManager.Instance.TeamHavePerk("mainperkcrack2c"))
						{
							int num5 = Mathf.FloorToInt(1f / 14f * (float)int.Parse(charges));
							if (num5 > 0)
							{
								text = text + "<br3>" + string.Format(Texts.Instance.GetText("crackVulnerable_description", "auracurse"), num5);
							}
						}
					}
				}
				else if (auraCurseData.Id == "dark")
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkDark2d") && !character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("darkEndOfTurn_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "decay")
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkDecay1c") && !character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("shadowResistanceNegative");
					}
				}
				else if (auraCurseData.Id == "fortify")
				{
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkFortify1a"))
					{
						text = text + "<br3>" + Texts.Instance.GetText("bluntAndFireDamage_description", "auracurse");
					}
					if (AtOManager.Instance.TeamHavePerk("mainperkFortify1c"))
					{
						text = text + "<br3>" + string.Format(Texts.Instance.GetText("lossesAllChargesEndRoundGainBlock"), int.Parse(charges) * 10);
					}
				}
				else if (auraCurseData.Id == "fury" && AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkfury1b"))
				{
					text = Texts.Instance.GetText("furynobleed_description", "auracurse");
				}
				else if (auraCurseData.Id == "mark")
				{
					if (!character.IsHero)
					{
						if (AtOManager.Instance.TeamHavePerk("mainperkmark1c"))
						{
							text = text + "<br3>" + Texts.Instance.GetText("markAlsoSlashingResistance_description", "auracurse");
						}
						if (AtOManager.Instance.TeamHavePerk("mainperkmark1b"))
						{
							text = text + "<br3>" + Texts.Instance.GetText("notLoseChargesPreventingStealth");
						}
					}
				}
				else if (auraCurseData.Id == "poison")
				{
					if (!character.IsHero && AtOManager.Instance.TeamHavePerk("mainperkpoison2b"))
					{
						text = text + "\n" + Texts.Instance.GetText("shadowResistanceNegative");
					}
				}
				else if (auraCurseData.Id == "regeneration")
				{
					if (character.IsHero)
					{
						if (AtOManager.Instance.TeamHavePerk("mainperkRegeneration1b"))
						{
							text = text + "<br3>" + Texts.Instance.GetText("healReceivedPercentPerStack");
						}
						if (AtOManager.Instance.TeamHavePerk("mainperkRegeneration1c"))
						{
							text = text + "<br3>" + Texts.Instance.GetText("shadowResistance");
						}
						if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkRegeneration1a"))
						{
							text = text + "<br3>" + Texts.Instance.GetText("regenerationSides_description", "auracurse");
						}
					}
				}
				else if (auraCurseData.Id == "runeblue")
				{
					if (character.IsHero)
					{
						text = text.Replace("{0}", (10 + character.GetAuraCurseQuantityModification("shield", Enums.CardClass.None)).ToString());
					}
				}
				else if (auraCurseData.Id == "scourge")
				{
					if (!character.IsHero)
					{
						if (AtOManager.Instance.TeamHaveTrait("unholyblight"))
						{
							text = text.Replace("dark,50", "dark,100");
						}
						if (AtOManager.Instance.TeamHaveTrait("auraofdespair"))
						{
							text = text + "<br3>" + Texts.Instance.GetText("allResistancesNegative");
						}
					}
				}
				else if (auraCurseData.Id == "shackle")
				{
					if (!character.IsHero && AtOManager.Instance.TeamHaveTrait("webweaver"))
					{
						text = Texts.Instance.GetText("shackleWebweaver_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "sharp")
				{
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkSharp1a"))
					{
						text = Texts.Instance.GetText("sharpnopierce_description", "auracurse");
					}
					else if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkSharp1b"))
					{
						text = Texts.Instance.GetText("sharpnoslash_description", "auracurse");
					}
					else if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkSharp1d"))
					{
						text = Texts.Instance.GetText("sharpShadow_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "spark")
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkSpark2b") && !character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("sparkSideCharges_description", "auracurse");
					}
					if (AtOManager.Instance.TeamHavePerk("mainperkSpark2c") && !character.IsHero)
					{
						int num6 = Mathf.FloorToInt(1f / 14f * (float)int.Parse(charges));
						if (num6 > 0)
						{
							text = text + "<br3>" + string.Format(Texts.Instance.GetText("sparkSlow_description", "auracurse"), num6);
						}
					}
					if (AtOManager.Instance.TeamHaveTrait("voltaicarc") && !character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("sparkVoltaicTrait_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "stanzai")
				{
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkStanza0b"))
					{
						text = Texts.Instance.GetText("stanzaiInspire_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "stanzaii")
				{
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkStanza0c"))
					{
						text = Texts.Instance.GetText("stanzaiiNo3_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "stealth")
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkStealth1c") && character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("allResistances");
					}
				}
				else if (auraCurseData.Id == "taunt")
				{
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkTaunt1b"))
					{
						text = text + "<br3>" + Texts.Instance.GetText("reinforce_description", "auracurse");
					}
					else if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkTaunt1c"))
					{
						text = text + "<br3>" + Texts.Instance.GetText("insulate_description", "auracurse");
					}
					else if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkTaunt1d"))
					{
						text = text + "<br3>" + Texts.Instance.GetText("courage_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "thorns")
				{
					if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkthorns1b"))
					{
						text = Texts.Instance.GetText("thornsHoly_description", "auracurse");
					}
					else if (AtOManager.Instance.CharacterHavePerk(character.SubclassName, "mainperkthorns1c"))
					{
						text = Texts.Instance.GetText("thornspoison_description", "auracurse");
					}
				}
				else if (auraCurseData.Id == "vitality")
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkVitality1c") && character.IsHero)
					{
						text = text + "<br3>" + Texts.Instance.GetText("mindResistance");
					}
				}
				else if (auraCurseData.Id == "vulnerable")
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkvulnerable0c") && !character.IsHero)
					{
						text = Texts.Instance.GetText("reinforce_description", "auracurse").Replace("+", "");
						text = text + "<br3>" + Texts.Instance.GetText("maximumCharges");
					}
				}
				else if (auraCurseData.Id == "wet" && !character.IsHero)
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkwet1a"))
					{
						text = text + "<br3>" + Texts.Instance.GetText("wetAlsoColdDamage_description", "auracurse");
					}
					if (AtOManager.Instance.TeamHavePerk("mainperkwet1b"))
					{
						text = text + "<br3>" + Texts.Instance.GetText("wetAlsoLightningResistance_description", "auracurse");
					}
				}
				if (character.IsHero && (auraCurseData.Id == "stanzai" || auraCurseData.Id == "stanzaii" || auraCurseData.Id == "stanzaiii") && AtOManager.Instance.TeamHavePerk("mainperkStanza0a"))
				{
					string pattern = "<y>(\\+\\d+)</y>";
					Match match = Regex.Match(text, pattern);
					if (match.Success)
					{
						string pattern2 = "<min>(.*?)</min>";
						if (Regex.Match(text, pattern2).Success)
						{
							_ = match.Groups[1].Value;
							text = Regex.Replace(text, pattern2, Texts.Instance.GetText("allDamage"));
						}
					}
				}
				stringBuilder.Append(text);
				if (auraCurseData2.Removable && !auraCurseData.Removable && auraCurseData2.Preventable && !auraCurseData.Preventable)
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("cannotBeDispelledOrPrevented"));
				}
				else
				{
					if (auraCurseData2.Removable && !auraCurseData.Removable)
					{
						stringBuilder.Append("<br3>");
						if (auraCurseData2.IsAura)
						{
							stringBuilder.Append(Texts.Instance.GetText("cannotBePurged"));
						}
						else
						{
							stringBuilder.Append(Texts.Instance.GetText("cannotBeDispelled"));
						}
					}
					if (auraCurseData2.Preventable && !auraCurseData.Preventable)
					{
						stringBuilder.Append("<br3>");
						stringBuilder.Append(Texts.Instance.GetText("cannotBePrevented"));
					}
				}
				if (auraCurseData2.MaxCharges == -1 && auraCurseData.MaxCharges > 0)
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("maximumCharges"));
				}
				if (!auraCurseData2.GainCharges && auraCurseData.GainCharges)
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("canStackCharges"));
				}
				if (!auraCurseData2.ConsumeAll && auraCurseData.ConsumeAll)
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("lossesAllChargesEndOfTurn"));
				}
				if ((auraCurseData2.ConsumedAtTurn && !auraCurseData.ConsumedAtTurn) || (auraCurseData.ConsumedAtTurn && auraCurseData.AuraConsumed == 0 && auraCurseData2.AuraConsumed > 0))
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("notLoseChargesEndTurn"));
				}
				if ((auraCurseData2.ConsumedAtTurnBegin && !auraCurseData.ConsumedAtTurnBegin) || (auraCurseData.ConsumedAtTurnBegin && auraCurseData.AuraConsumed == 0 && auraCurseData2.AuraConsumed > 0))
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("notLoseChargesStartTurn"));
				}
				if (auraCurseData2.CharacterStatModified != Enums.CharacterStat.Hp && auraCurseData.CharacterStatModified == Enums.CharacterStat.Hp && auraCurseData.CharacterStatModifiedValuePerStack != 0f)
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("maxHPModified"));
				}
				if (auraCurseData2.CharacterStatModified != Enums.CharacterStat.Speed && auraCurseData.CharacterStatModified == Enums.CharacterStat.Speed && auraCurseData.CharacterStatModifiedValuePerStack != 0f)
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(Texts.Instance.GetText("speedModified"));
				}
				if (auraCurseData.AuraConsumed > 0 && auraCurseData2.AuraConsumed != auraCurseData.AuraConsumed)
				{
					stringBuilder.Append("<br3>");
					stringBuilder.Append(string.Format(Texts.Instance.GetText("chargesReducedByEndTurn"), auraCurseData.AuraConsumed));
				}
				text = stringBuilder.ToString();
				Match match2 = new Regex("_([^>]*)>").Match(text);
				if (match2.Success)
				{
					string value = match2.Groups[1].Value;
					num = character.GetAuraCharges(value);
					if (auraCurseData.Id == "scourge" && character != null && !character.IsHero)
					{
						if (AtOManager.Instance.TeamHavePerk("mainperkscourge0b"))
						{
							num += character.GetAuraCharges("burn");
						}
						if (AtOManager.Instance.TeamHavePerk("mainperkscourge0c"))
						{
							num += character.GetAuraCharges("insane");
						}
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder2.Append("_");
					stringBuilder3.Append("_");
					stringBuilder2.Append(value);
					stringBuilder3.Append("sec");
					stringBuilder2.Append(">");
					stringBuilder3.Append(">");
					text = text.Replace(stringBuilder2.ToString(), stringBuilder3.ToString());
				}
			}
		}
		int charges2 = ParseCharges(charges);
		Dictionary<string, string> replacements = new Dictionary<string, string>();
		UpdateReplacements(replacements, charges2, num, auraCurseData, character);
		text = ApplyReplacements(text, replacements);
		if (AtOManager.Instance.TeamHavePerk("mainperkrust0c") && !character.IsHero)
		{
			text = text + "\n" + Texts.Instance.GetText("thorn_increase").Replace("<CustomAuxValue>", ((float)character.GetAuraCharges("rust") * 5f).ToString());
		}
		var (text2, num7, text3) = GetAuraAndChargesPatter(text);
		if (!text3.IsNullOrEmpty() && !text2.IsNullOrEmpty() && num7 > 0)
		{
			int num8 = character.GetAuraCharges(text2);
			if (num8 > 0)
			{
				num8 = Functions.FuncRoundToInt((float)num8 * (float)num7 / 100f);
			}
			text = text.Replace(text3, num8.ToString());
		}
		text = ReplaceGeneralTags(text);
		array[1] = text;
		array[2] = "#FFF";
		array[3] = "";
		if (auraCurseData != null)
		{
			if (auraCurseData.IsAura)
			{
				array[2] = "AADDFF";
				imageBg.color = colorGood;
			}
			else
			{
				array[2] = "FF8181";
				imageBg.color = colorBad;
			}
			if (!auraCurseData.GainCharges)
			{
				array[3] = "<space=.5em><size=18><color=#AAAAAA>(" + Texts.Instance.GetText("notStackPlain") + ")</color></size>";
			}
		}
		stringBuilder.Clear();
		stringBuilder.Append("<voffset=-1><size=26><sprite name=");
		stringBuilder.Append(auraCurseData.Id.ToLower());
		stringBuilder.Append("></size></voffset>");
		string format = popupText;
		object[] args = array;
		stringBuilder.Append(string.Format(format, args));
		TextAdjust(stringBuilder.ToString());
		pop.SetActive(value: true);
		popTrait.SetActive(value: false);
		popTown.SetActive(value: false);
		popPerk.SetActive(value: false);
		CenterPop();
		lastPop = auraCurseData.ACName + charges;
		lastTransform = tf;
	}

	private (string auracurse, int auracurseCharges, string auracurseString) GetAuraAndChargesPatter(string cadena)
	{
		string pattern = "<aura:(\\w+),\\s*(\\d+)>";
		Match match = Regex.Match(cadena, pattern);
		if (match.Success)
		{
			string value = match.Groups[1].Value;
			int item = int.Parse(match.Groups[2].Value);
			string value2 = match.Value;
			return (auracurse: value, auracurseCharges: item, auracurseString: value2);
		}
		return (auracurse: null, auracurseCharges: 0, auracurseString: null);
	}

	private string ReplaceGeneralTags(string textDescription)
	{
		textDescription = Functions.FormatString(textDescription);
		return textDescription;
	}

	public void SetText(string theText, bool fast = false, string position = "", bool alwaysCenter = false, string keynote = "")
	{
		popTrait.SetActive(value: false);
		popTown.SetActive(value: false);
		popPerk.SetActive(value: false);
		fastPopup = fast;
		if (lastPop == theText)
		{
			DrawPopup();
			switch (position)
			{
			case "followdown":
				FollowPopDown();
				break;
			case "centerpop":
				CenterPop();
				break;
			case "followdown2":
				FollowPopDown2();
				break;
			default:
				FollowPop();
				break;
			}
			return;
		}
		DrawPopup();
		CleanKeyNotes();
		if (keynote != "")
		{
			KeyNotesActive.Add(keynote);
			KeyNotesGO[keynote].SetActive(value: true);
		}
		TextAdjust(theText, adjust: true, alwaysCenter);
		switch (position)
		{
		case "followdown":
			FollowPopDown();
			break;
		case "centerpop":
			CenterPop();
			break;
		case "followdown2":
			FollowPopDown2();
			break;
		default:
			FollowPop();
			break;
		}
		lastPop = theText;
	}

	public void SetBackgroundColor(string _color)
	{
		imageBg.color = Functions.HexToColor(_color);
	}

	private void TextAdjust(string theText, bool adjust = false, bool alwaysCenter = false)
	{
		if (!GO_Popup.activeSelf)
		{
			GO_Popup.SetActive(value: true);
		}
		if (!pop.activeSelf)
		{
			pop.SetActive(value: true);
		}
		popText.horizontalAlignment = HorizontalAlignmentOptions.Left;
		popText.fontSize = 21f;
		popText.paragraphSpacing = 30f;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<line-height=30><voffset=3>");
		stringBuilder.Append(theText);
		popText.text = stringBuilder.ToString();
		textRect.sizeDelta = new Vector2(500f, textRect.sizeDelta.y);
		popText.ForceMeshUpdate(ignoreActiveState: true);
		if (adjust && alwaysCenter)
		{
			float x = popText.bounds.max.x;
			textRect.sizeDelta = new Vector2(x + 50f, textRect.sizeDelta.y);
			popText.horizontalAlignment = HorizontalAlignmentOptions.Center;
			popText.ForceMeshUpdate(ignoreActiveState: true);
		}
		if (GO_Popup.activeSelf)
		{
			GO_Popup.SetActive(value: false);
		}
	}

	public void SetConsoleText(Transform tf, string theText)
	{
		if (lastPop == theText)
		{
			DrawPopup(tf);
			CenterPop();
			return;
		}
		DrawPopup(tf);
		CleanKeyNotes();
		popText.text = MatchManager.Instance.console.GetText(theText);
		pop.SetActive(value: true);
		CenterPop();
		lastPop = theText;
	}

	public void DrawPopup(Transform tf = null)
	{
		theTF = tf;
		adjustPosition = popupContainer.GetComponent<RectTransform>().sizeDelta;
		imageBg.color = colorPlain;
	}

	public void RefreshKeyNotes()
	{
		StartCoroutine(RefreshKeyNotesCo());
	}

	private IEnumerator RefreshKeyNotesCo()
	{
		if (KeyNotesGO.Count <= 0)
		{
			yield break;
		}
		CleanKeyNotes();
		foreach (KeyValuePair<string, GameObject> item in KeyNotesGO)
		{
			UnityEngine.Object.Destroy(KeyNotesGO[item.Key]);
		}
		foreach (Transform item2 in popupContainer)
		{
			if (!initialPopupGOs.Contains(item2.gameObject.name))
			{
				UnityEngine.Object.Destroy(item2.gameObject);
			}
		}
		while (popupContainer.childCount > initialPopupGOs.Count)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		CreateKeyNotes();
		lastPop = "";
	}

	public void CreateKeyNotes()
	{
		if (popupContainer.childCount > initialPopupGOs.Count)
		{
			Debug.LogWarning("[PopupManager] CreateKeyNotes exit because child counter > 5");
			return;
		}
		KeyNotesData[] array = new KeyNotesData[Globals.Instance.KeyNotes.Count];
		int num = 0;
		foreach (KeyValuePair<string, KeyNotesData> keyNote in Globals.Instance.KeyNotes)
		{
			array[num] = keyNote.Value;
			num++;
		}
		imageBg.color = colorPlain;
		HashSet<string> hashSet = new HashSet<string>
		{
			"overcharge", "chain", "jump", "jump(bonus%)", "repeat", "repeatupto", "dispel", "purge", "discover", "reveal",
			"transfer", "steal", "escapes", "metamorph", "nightmareecho", "nightmareechoa", "nightmareechob", "corruptedecho", "transferpain", "nightmareimage",
			"mindspike"
		};
		for (int i = 0; i < array.Length; i++)
		{
			string id = array[i].Id;
			GameObject gameObject = UnityEngine.Object.Instantiate(pop, Vector3.zero, Quaternion.identity, popupContainer);
			gameObject.name = id;
			TMP_Text component = gameObject.transform.Find("Background/Text").GetComponent<TMP_Text>();
			Image component2 = gameObject.transform.GetChild(0).GetComponent<Image>();
			string[] array2 = new string[4];
			string text = Texts.Instance.GetText(id);
			array2[0] = ((!string.IsNullOrEmpty(text)) ? text : id);
			array2[0] = "<b>" + array2[0] + "</b>";
			array2[1] = (GameManager.Instance.ConfigExtendedDescriptions ? array[i].DescriptionExtended : array[i].Description);
			switch (id)
			{
			case "vanish":
				component2.color = colorVanish;
				array2[2] = "EBAAFF";
				break;
			case "innate":
				component2.color = colorInnate;
				array2[2] = "AFFFC9";
				break;
			case "energy":
				component2.color = colorGood;
				array2[2] = "AADDFF";
				break;
			default:
			{
				if (hashSet.Contains(id))
				{
					component2.color = colorPlain;
					array2[2] = "FFF";
					if (id == "overcharge")
					{
						array2[0] = "[" + Texts.Instance.GetText("overchargeAcronym") + "] " + text;
					}
					break;
				}
				component2.color = colorPlain;
				AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(id);
				if (auraCurseData != null)
				{
					if (auraCurseData.IsAura)
					{
						component2.color = colorGood;
						array2[2] = "AADDFF";
					}
					else
					{
						component2.color = colorBad;
						array2[2] = "FF8181";
					}
					if (!auraCurseData.GainCharges)
					{
						ref string reference = ref array2[0];
						reference = reference + "<space=.5em><size=18><color=#AAAAAA>(" + Texts.Instance.GetText("notStackPlain") + ")</color></size>";
					}
				}
				break;
			}
			}
			array2[3] = "";
			string format = popupText;
			object[] args = array2;
			string value = ReplaceGeneralTags(string.Format(format, args));
			string value2 = (hashSet.Contains(id) ? "cards" : id);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<voffset=-1><size=24><sprite name=");
			stringBuilder.Append(value2);
			stringBuilder.Append("></size></voffset>");
			stringBuilder.Append(value);
			component.text = stringBuilder.ToString();
			gameObject.SetActive(value: false);
			KeyNotesGO[id] = gameObject;
		}
		popTrait = UnityEngine.Object.Instantiate(pop, Vector3.zero, Quaternion.identity, popupContainer);
		popTrait.transform.Find("Background").GetComponent<Image>().color = colorTrait;
		popTraitText = popTrait.transform.Find("Background/Text").GetComponent<TMP_Text>();
		popTrait.transform.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 20, 0);
		popTrait.transform.localScale = new Vector3(1f, 1f, 1f);
		popTrait.name = "trait";
		popTown = UnityEngine.Object.Instantiate(pop, Vector3.zero, Quaternion.identity, popupContainer);
		popTown.transform.Find("Background").GetComponent<Image>().color = colorTown;
		popTownText = popTown.transform.Find("Background/Text").GetComponent<TMP_Text>();
		popTown.transform.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 20, 0);
		popTown.transform.localScale = new Vector3(1f, 1f, 1f);
		popTown.name = "town";
		popPerk = UnityEngine.Object.Instantiate(pop, Vector3.zero, Quaternion.identity, popupContainer);
		popPerk.transform.Find("Background").GetComponent<Image>().color = colorTrait;
		popPerkText = popPerk.transform.Find("Background/Text").GetComponent<TMP_Text>();
		popPerk.transform.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 10, 0);
		popPerk.transform.localScale = new Vector3(1f, 1f, 1f);
		popPerk.name = "perk";
		popPerkText.lineSpacing = 10f;
	}

	private void CleanKeyNotes()
	{
		if (KeyNotesActive.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < KeyNotesActive.Count; i++)
		{
			if (KeyNotesGO.ContainsKey(KeyNotesActive[i]) && KeyNotesGO[KeyNotesActive[i]] != null)
			{
				KeyNotesGO[KeyNotesActive[i]].SetActive(value: false);
			}
		}
		KeyNotesActive.Clear();
	}
}
