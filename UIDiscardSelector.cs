using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDiscardSelector : MonoBehaviour
{
	public TMP_Text textInstructions;

	public Transform button;

	public Transform buttonHide;

	public Canvas canvas;

	private Button buttonB;

	public GameObject elements;

	public TMP_Text buttonShowText;

	private bool showStatus = true;

	public Transform cardContainer;

	private bool nonLimitedNumCards;

	private Enums.CardPlace cardPlace;

	private void Awake()
	{
		buttonB = button.GetComponent<Button>();
		canvas.gameObject.SetActive(value: false);
	}

	public bool IsActive()
	{
		return canvas.gameObject.activeSelf;
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
		}
		else
		{
			elements.gameObject.SetActive(value: true);
			buttonShowText.text = Texts.Instance.GetText("hide").ToUpper();
			if (doMask)
			{
				MatchManager.Instance.ShowMaskFromUIScreen(state: true);
			}
		}
		showStatus = !showStatus;
	}

	public void TextInstructions()
	{
		int num = MatchManager.Instance.CardsLeftForDiscard();
		if (num < 0)
		{
			num = 0;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (cardPlace == Enums.CardPlace.TopDeck)
		{
			stringBuilder.Append(Texts.Instance.GetText("chooseTopDeck"));
		}
		else if (cardPlace == Enums.CardPlace.Vanish)
		{
			stringBuilder.Append(Texts.Instance.GetText("chooseVanish"));
		}
		else
		{
			stringBuilder.Append(Texts.Instance.GetText("chooseDiscard"));
		}
		if (!nonLimitedNumCards)
		{
			stringBuilder.Append("\n<size=3><color=#bbb>");
			stringBuilder.Append(Texts.Instance.GetText("cardsLeft"));
			stringBuilder.Append(" <color=green>");
			stringBuilder.Append(num.ToString());
			stringBuilder.Append("</color>");
		}
		textInstructions.text = stringBuilder.ToString();
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && !MatchManager.Instance.IsYourTurn())
		{
			buttonB.gameObject.SetActive(value: false);
			buttonB.interactable = false;
			return;
		}
		buttonB.gameObject.SetActive(value: true);
		if (num == 0 || nonLimitedNumCards)
		{
			buttonB.interactable = true;
		}
		else
		{
			buttonB.interactable = false;
		}
	}

	public void TurnOn(Enums.CardPlace _cardPlace, bool _nonLimitedNumCards = false)
	{
		buttonB.interactable = false;
		MatchManager.Instance.ShowMask(state: true);
		MatchManager.Instance.lockHideMask = true;
		cardPlace = _cardPlace;
		nonLimitedNumCards = _nonLimitedNumCards;
		TextInstructions();
		showStatus = false;
		HideShow(doMask: false);
		canvas.gameObject.SetActive(value: true);
		MatchManager.Instance.WarningMultiplayerIfNotActive();
	}

	public void Action()
	{
		if (buttonB.interactable && (MatchManager.Instance.WaitingForDiscardAssignment || MatchManager.Instance.WaitingForLookDiscardWindow))
		{
			buttonB.gameObject.SetActive(value: false);
			MatchManager.Instance.AssignDiscardAction();
		}
	}

	public void TurnOff()
	{
		MatchManager.Instance.lockHideMask = false;
		MatchManager.Instance.ShowMask(state: false);
		canvas.gameObject.SetActive(value: false);
	}
}
