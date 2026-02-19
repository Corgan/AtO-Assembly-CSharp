using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAddcardSelector : MonoBehaviour
{
	public TMP_Text textInstructions;

	public Transform button;

	public Canvas canvas;

	public GameObject elements;

	public TMP_Text buttonShowText;

	private bool showStatus = true;

	private Button buttonB;

	private bool hideEnabled;

	private void Awake()
	{
		buttonB = button.GetComponent<Button>();
		canvas.gameObject.SetActive(value: false);
	}

	public bool IsActive()
	{
		return canvas.gameObject.activeSelf;
	}

	public void HideShow()
	{
		if (hideEnabled)
		{
			if (showStatus)
			{
				elements.gameObject.SetActive(value: false);
				buttonShowText.text = Texts.Instance.GetText("show");
			}
			else
			{
				elements.gameObject.SetActive(value: true);
				buttonShowText.text = Texts.Instance.GetText("hide");
			}
		}
	}

	public void TextInstructions()
	{
		int num = MatchManager.Instance.CardsLeftForAddcard();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("addCardInstructions"));
		stringBuilder.Append("\n<size=3><color=#bbb>");
		stringBuilder.Append(Texts.Instance.GetText("cardsLeft"));
		stringBuilder.Append(" <color=green>");
		stringBuilder.Append(num.ToString());
		stringBuilder.Append("</color>");
		textInstructions.text = stringBuilder.ToString();
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance != null && !MatchManager.Instance.IsYourTurn())
		{
			buttonB.gameObject.SetActive(value: false);
			buttonB.interactable = false;
			return;
		}
		buttonB.gameObject.SetActive(value: true);
		if (num == 0)
		{
			buttonB.interactable = true;
		}
		else
		{
			buttonB.interactable = false;
		}
	}

	public void TurnOn()
	{
		hideEnabled = false;
		buttonB.interactable = false;
		MatchManager.Instance.ShowMask(state: true);
		MatchManager.Instance.lockHideMask = true;
		TextInstructions();
		canvas.gameObject.SetActive(value: true);
		MatchManager.Instance.controllerCurrentIndex = -1;
		StartCoroutine(EnableHide());
	}

	private IEnumerator EnableHide()
	{
		yield return Globals.Instance.WaitForSeconds(10.5f);
		hideEnabled = true;
	}

	public void Action()
	{
		if (buttonB.interactable && MatchManager.Instance.WaitingForAddcardAssignment)
		{
			MatchManager.Instance.AssignAddcardAction();
			TurnOff();
		}
	}

	public void TurnOff()
	{
		MatchManager.Instance.lockHideMask = false;
		MatchManager.Instance.ShowMask(state: false);
		canvas.gameObject.SetActive(value: false);
	}
}
