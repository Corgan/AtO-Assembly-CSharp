using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDeckCards : MonoBehaviour
{
	public TMP_Text textInstructions;

	public Transform buttonClose;

	public Transform buttonDiscard;

	public Transform buttonHide;

	public Canvas canvas;

	public Transform cardContainer;

	public Transform scroll;

	public RectTransform cardContainerRT;

	public GameObject elements;

	public TMP_Text buttonShowText;

	private bool showStatus = true;

	private Button buttonCloseB;

	private Button buttonAction;

	private int cardQuantity = -1;

	private bool mustSelect;

	private Coroutine enabledCoroutine;

	private void Awake()
	{
		buttonCloseB = buttonClose.GetComponent<Button>();
		buttonAction = buttonDiscard.GetComponent<Button>();
		canvas.gameObject.SetActive(value: false);
	}

	private IEnumerator mustSelectCo()
	{
		while (true)
		{
			if (mustSelect)
			{
				if (MatchManager.Instance.CardsLeftForAddcard() > 0)
				{
					buttonAction.interactable = false;
				}
				else
				{
					buttonAction.interactable = true;
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
	}

	public void HideShow(bool doMask = true)
	{
		if (showStatus)
		{
			elements.gameObject.SetActive(value: false);
			buttonShowText.text = Texts.Instance.GetText("show").ToUpper();
			if (doMask)
			{
				MatchManager.Instance.ShowMaskFromUIScreen(state: false);
			}
			if (enabledCoroutine != null)
			{
				StopCoroutine(enabledCoroutine);
			}
		}
		else
		{
			elements.gameObject.SetActive(value: true);
			buttonShowText.text = Texts.Instance.GetText("hide").ToUpper();
			if (doMask)
			{
				MatchManager.Instance.ShowMaskFromUIScreen(state: true);
			}
			if (enabledCoroutine != null)
			{
				StopCoroutine(enabledCoroutine);
			}
			enabledCoroutine = StartCoroutine(mustSelectCo());
			MatchManager.Instance.controllerCurrentIndex = -1;
		}
		showStatus = !showStatus;
	}

	public bool IsActive()
	{
		return canvas.gameObject.activeSelf;
	}

	private string FormatNumer(string text, int numCards)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append("<color=green>");
		stringBuilder2.Append(numCards.ToString());
		stringBuilder2.Append("</color>");
		stringBuilder.Append("\n<size=3><color=#bbb>");
		stringBuilder.Append(string.Format(text, stringBuilder2.ToString()));
		stringBuilder.Append("</color></size>");
		return stringBuilder.ToString();
	}

	public void TextInstructions(int type, int _cardQuantity, int _cardTotal = -1, bool _toVanish = false)
	{
		int num = 0;
		string text = "";
		cardQuantity = _cardQuantity;
		buttonCloseB.interactable = true;
		buttonClose.gameObject.SetActive(value: false);
		StringBuilder stringBuilder = new StringBuilder();
		mustSelect = false;
		switch (type)
		{
		case 0:
			num = MatchManager.Instance.CountHeroDeck();
			stringBuilder.Append(Texts.Instance.GetText("deckPile"));
			stringBuilder.Append(FormatNumer(Texts.Instance.GetText("cardsNum"), num));
			text = stringBuilder.ToString();
			buttonClose.gameObject.SetActive(value: true);
			break;
		case 1:
			num = MatchManager.Instance.CountHeroDiscard();
			stringBuilder.Append(Texts.Instance.GetText("discardPile"));
			stringBuilder.Append(FormatNumer(Texts.Instance.GetText("cardsNum"), num));
			text = stringBuilder.ToString();
			buttonClose.gameObject.SetActive(value: true);
			break;
		case 2:
			if (cardQuantity > 0)
			{
				stringBuilder.Append(Texts.Instance.GetText("deckPile"));
				if (!_toVanish)
				{
					stringBuilder.Append(FormatNumer(Texts.Instance.GetText("youCanDiscardUpTo"), cardQuantity));
				}
				else
				{
					stringBuilder.Append(FormatNumer(Texts.Instance.GetText("youCanVanishUpTo"), cardQuantity));
				}
				text = stringBuilder.ToString();
			}
			else
			{
				stringBuilder.Append(Texts.Instance.GetText("deckPile"));
				stringBuilder.Append("\n<size=3><color=#bbb>");
				stringBuilder.Append(Texts.Instance.GetText("pressWhenReady"));
				stringBuilder.Append("</size>");
				text = stringBuilder.ToString();
			}
			num = _cardTotal;
			MatchManager.Instance.CardsLeftForAddcard();
			if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && !MatchManager.Instance.IsYourTurn())
			{
				buttonClose.gameObject.SetActive(value: false);
			}
			break;
		case 3:
		{
			num = _cardTotal;
			stringBuilder.Append(Texts.Instance.GetText("deckPile"));
			int num2 = MatchManager.Instance.CardsLeftForAddcard();
			if (num2 == 1)
			{
				stringBuilder.Append(FormatNumer(Texts.Instance.GetText("youCanAdd"), 1));
			}
			else
			{
				stringBuilder.Append(FormatNumer(Texts.Instance.GetText("youCanAddPlural"), num2));
			}
			text = stringBuilder.ToString();
			mustSelect = true;
			break;
		}
		}
		textInstructions.text = text;
		if (num > 10)
		{
			scroll.gameObject.SetActive(value: true);
			cardContainerRT.sizeDelta = new Vector2(cardContainerRT.sizeDelta.x, 3.2f * (float)Mathf.CeilToInt((float)num / 5f) - 4f + 2f);
			cardContainerRT.anchoredPosition = new Vector2(cardContainerRT.anchoredPosition.x, 3f - cardContainerRT.sizeDelta.y * 0.5f - 1f);
			cardContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(2.5f, cardContainerRT.sizeDelta.y * 0.5f - 7.5f + 1f);
		}
		else
		{
			scroll.gameObject.SetActive(value: false);
			cardContainerRT.sizeDelta = new Vector2(cardContainerRT.sizeDelta.x, 4f);
			cardContainerRT.anchoredPosition = new Vector2(cardContainerRT.anchoredPosition.x, 0f);
			cardContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(2.5f, -4.5f);
		}
		cardContainerRT.gameObject.SetActive(value: true);
	}

	public void TurnOn(int type, int cardQuantity = -1, int cardTotal = -1, bool toVanish = false)
	{
		MatchManager.Instance.ShowMask(state: true);
		MatchManager.Instance.lockHideMask = true;
		buttonAction.interactable = true;
		TextInstructions(type, cardQuantity, cardTotal, toVanish);
		showStatus = false;
		HideShow(doMask: false);
		canvas.gameObject.SetActive(value: true);
		if (cardQuantity != -1)
		{
			buttonDiscard.gameObject.SetActive(value: true);
		}
		else
		{
			buttonDiscard.gameObject.SetActive(value: false);
		}
		if (!GameManager.Instance.IsMultiplayer() || !(MatchManager.Instance != null))
		{
			return;
		}
		MatchManager.Instance.WarningMultiplayerIfNotActive();
		if (MatchManager.Instance.heroIndexWaitingForAddDiscard > -1)
		{
			if (!MatchManager.Instance.IsYourTurnForAddDiscard())
			{
				buttonDiscard.gameObject.SetActive(value: false);
				buttonAction.interactable = false;
			}
		}
		else if (!MatchManager.Instance.IsYourTurn())
		{
			buttonDiscard.gameObject.SetActive(value: false);
		}
	}

	public float GetScrolled()
	{
		return cardContainer.position.y;
	}

	public void Action()
	{
		if (buttonAction.interactable)
		{
			MatchManager.Instance.AssignLookDiscardAction();
		}
	}

	public void CloseBG()
	{
		if (cardQuantity == -1)
		{
			Close();
		}
	}

	public void Close()
	{
		MatchManager.Instance.DrawDeckScreenDestroy();
	}

	public void TurnOff()
	{
		cardContainerRT.gameObject.SetActive(value: false);
		MatchManager.Instance.lockHideMask = false;
		MatchManager.Instance.ShowMask(state: false);
		canvas.gameObject.SetActive(value: false);
		if (enabledCoroutine != null)
		{
			StopCoroutine(enabledCoroutine);
		}
	}
}
