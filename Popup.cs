using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
	public GameObject popupPrefab;

	public Sprite plainBg;

	public Sprite warriorBg;

	private Image imageBg;

	private bool popupActive;

	private Coroutine coroutine;

	private GameObject GO_Popup;

	private Transform canvasContainer;

	private RectTransform canvasRect;

	private Transform popupContainer;

	private RectTransform popupRect;

	private GameObject pop;

	private TMP_Text popText;

	private string lastPop = "";

	private string popupText = "<line-height=45><size=26><color=#{2}><b>{0}</b></color></size>\n</line-height>{1}";

	private Dictionary<string, GameObject> KeyNotesGO;

	private List<string> KeyNotesActive;

	private Transform theTF;

	private Vector3 destinationPosition;

	private Vector3 adjustPosition;

	private string position = "";

	private void Awake()
	{
		KeyNotesGO = new Dictionary<string, GameObject>();
		KeyNotesActive = new List<string>();
	}

	private void Start()
	{
		GO_Popup = UnityEngine.Object.Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
		canvasContainer = GO_Popup.transform.GetChild(0);
		canvasRect = canvasContainer.GetComponent<RectTransform>();
		popupContainer = canvasContainer.GetChild(0);
		popupRect = popupContainer.GetComponent<RectTransform>();
		pop = popupContainer.GetChild(0).gameObject;
		popText = pop.transform.Find("Background/Text").GetComponent<TMP_Text>();
		imageBg = pop.transform.GetChild(0).GetComponent<Image>();
		pop.SetActive(value: false);
		GO_Popup.SetActive(value: false);
		CreateKeyNotes();
	}

	private void Update()
	{
		if (popupActive)
		{
			if (Vector3.Distance(popupContainer.localPosition, destinationPosition) > 0.02f)
			{
				popupContainer.localPosition = Vector3.Lerp(popupContainer.localPosition, destinationPosition, 6f * Time.deltaTime);
				return;
			}
			popupActive = false;
			popupContainer.localPosition = new Vector3(Mathf.CeilToInt(destinationPosition.x), Mathf.CeilToInt(destinationPosition.y), 1f);
		}
	}

	private IEnumerator ShowPopup()
	{
		yield return new WaitForSeconds(0.25f);
		if (theTF == null)
		{
			yield break;
		}
		if (position == "right")
		{
			adjustPosition = new Vector3(popupContainer.GetComponent<RectTransform>().sizeDelta.x, theTF.GetComponent<BoxCollider2D>().size.y, 0f);
			destinationPosition = new Vector3(theTF.position.x * 100f + adjustPosition.x * 0.85f, theTF.position.y * 100f + adjustPosition.y * 30f * 0.2f, 1f);
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
				if (destinationPosition.x * 0.01f > (float)Screen.width * 0.005f - 2f)
				{
					float num = destinationPosition.x * 0.01f - ((float)Screen.width * 0.005f - 2f);
					destinationPosition -= new Vector3(num * 100f, 0f, 0f);
				}
				if (destinationPosition.x * 0.01f < (float)(-Screen.width) * 0.005f + 2f)
				{
					float num2 = (float)(-Screen.width) * 0.005f + 2f - destinationPosition.x * 0.01f;
					destinationPosition += new Vector3(num2 * 100f, 0f, 0f);
				}
			}
			else
			{
				Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				destinationPosition = new Vector3(vector.x * 100f, vector.y * 100f, 1f);
			}
		}
		destinationPosition = new Vector3(Mathf.CeilToInt(destinationPosition.x), Mathf.CeilToInt(destinationPosition.y), 1f);
		GO_Popup.SetActive(value: true);
		popupContainer.localPosition = destinationPosition - new Vector3(0f, 20f, 0f);
		popupActive = true;
	}

	public void ClosePopup()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		popupActive = false;
		GO_Popup.SetActive(value: false);
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

	private void CenterRightPop()
	{
		position = "centerright";
		coroutine = StartCoroutine(ShowPopup());
	}

	public void SetCard(Transform tf, CardData cardData, List<KeyNotesData> cardDataKeyNotes)
	{
		if (lastPop == cardData.Id)
		{
			DrawPopup(tf);
			RightPop();
			return;
		}
		int count = cardDataKeyNotes.Count;
		if (count <= 0 && cardData.CardType == Enums.CardType.None)
		{
			return;
		}
		DrawPopup(tf);
		if (cardData.CardType != Enums.CardType.None)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<voffset=-2><size=30><sprite name=cards></size></voffset> ");
			stringBuilder.Append("<size=26><color=#fc0><b>");
			stringBuilder.Append(Enum.GetName(typeof(Enums.CardType), cardData.CardType).Replace("_", " "));
			stringBuilder.Append("</b></color>");
			for (int i = 0; i < cardData.CardTypeAux.Length; i++)
			{
				if (cardData.CardTypeAux[i] != Enums.CardType.None)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(Enum.GetName(typeof(Enums.CardType), cardData.CardTypeAux[i]));
				}
			}
			stringBuilder.Append("</size>");
			popText.text = stringBuilder.ToString();
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
					KeyNotesActive.Add(cardDataKeyNotes[j].KeynoteName);
					KeyNotesGO[cardDataKeyNotes[j].KeynoteName].SetActive(value: true);
				}
			}
		}
		RightPop();
		lastPop = cardData.Id;
	}

	public void SetAuraCurse(Transform tf, AuraCurseData acData, string charges)
	{
		if (!EventSystem.current.IsPointerOverGameObject() && !(charges == ""))
		{
			imageBg.sprite = plainBg;
			if (lastPop == acData.ACName + charges)
			{
				DrawPopup(tf);
				CenterPop();
				return;
			}
			DrawPopup(tf);
			CleanKeyNotes();
			string[] array = new string[3] { acData.ACName, null, null };
			string description = acData.Description;
			string newValue = (acData.ChargesMultiplierDescription * int.Parse(charges)).ToString();
			description = description.Replace("%ChargesMultiplier%", newValue);
			description = description.Replace("%ChargesAux1%", Mathf.FloorToInt((float)int.Parse(charges) / acData.ChargesAuxNeedForOne1).ToString());
			description = description.Replace("%ChargesAux2%", Mathf.FloorToInt(int.Parse(charges) / acData.ChargesAuxNeedForOne2).ToString());
			string newValue2 = acData.CharacterStatModifiedValue.ToString();
			description = description.Replace("%CharacterStatModifiedValue%", newValue2);
			description = description.Replace("%DamageWhenConsumed%", acData.DamageWhenConsumed.ToString());
			description = description.Replace("%DamageSidesWhenConsumed%", acData.DamageSidesWhenConsumed.ToString());
			description = description.Replace("%HealAttackerConsumeCharges%", acData.HealAttackerConsumeCharges.ToString());
			description = ReplaceGeneralTags(description);
			array[1] = description;
			array[2] = "#FFF";
			string text = "<voffset=-2><size=30><sprite name=" + array[0].Replace(" ", "").ToLower() + "></size></voffset> ";
			TMP_Text tMP_Text = popText;
			string format = popupText;
			object[] args = array;
			tMP_Text.text = text + string.Format(format, args);
			pop.SetActive(value: true);
			CenterPop();
			lastPop = acData.ACName + charges;
		}
	}

	private string ReplaceGeneralTags(string textDescription)
	{
		textDescription = Functions.FormatString(textDescription);
		return textDescription;
	}

	public void SetConsoleText(Transform tf, string theText)
	{
		imageBg.sprite = plainBg;
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
	}

	private void CreateKeyNotes()
	{
		KeyNotesData[] array = Resources.LoadAll<KeyNotesData>("KeyNotes");
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(pop, Vector3.zero, Quaternion.identity, popupContainer);
			gameObject.name = array[i].KeynoteName;
			TMP_Text component = gameObject.transform.Find("Background/Text").GetComponent<TMP_Text>();
			string[] array2 = new string[3]
			{
				array[i].KeynoteName,
				array[i].Description,
				"FFF"
			};
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<voffset=-2><size=30><sprite name=");
			stringBuilder.Append(array2[0].Replace(" ", "").ToLower());
			stringBuilder.Append("></size></voffset> ");
			string format = popupText;
			object[] args = array2;
			stringBuilder.Append(ReplaceGeneralTags(string.Format(format, args)));
			component.text = stringBuilder.ToString();
			gameObject.SetActive(value: false);
			KeyNotesGO[array[i].KeynoteName] = gameObject;
		}
	}

	private void CleanKeyNotes()
	{
		for (int i = 0; i < KeyNotesActive.Count; i++)
		{
			KeyNotesGO[KeyNotesActive[i]].SetActive(value: false);
		}
		KeyNotesActive.Clear();
	}
}
