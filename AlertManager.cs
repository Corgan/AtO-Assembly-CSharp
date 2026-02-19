using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WebSocketSharp;

public class AlertManager : MonoBehaviour
{
	public delegate void OnButtonClickDelegate();

	public Image backgroundImage;

	public Transform popupT;

	public Transform playersT;

	public GameObject canvas;

	public TMP_Text alertText;

	public TMP_InputField alertInput;

	public TMP_Text alertTextCP;

	public TMP_InputField alertInputCP;

	public TMP_InputField alertInputPC;

	public Transform exitButton;

	public Button SingleButton;

	public Button LeftButton;

	public Button RightButton;

	public TMP_Text alertInputButtonText;

	public TMP_Text alertTextSingleButton;

	public TMP_Text alertTextLeftButton;

	public TMP_Text alertTextRightButton;

	private string inputValue;

	private bool confirmAnswer;

	public Transform doorIcon;

	public Transform resignIcon;

	public Transform reloadIcon;

	public List<AlertPlayer> playerList;

	public static OnButtonClickDelegate buttonClickDelegate;

	public List<Transform> menuControllerAlert;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static AlertManager Instance { get; private set; }

	public void OnButtonClick()
	{
		buttonClickDelegate();
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		HideAlert();
	}

	public void Abort()
	{
		buttonClickDelegate = null;
		HideAlert();
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	public void HideAlert()
	{
		if (popupT.gameObject.activeSelf)
		{
			alertText.text = "";
			alertInput.transform.gameObject.SetActive(value: false);
			alertTextSingleButton.transform.parent.gameObject.SetActive(value: false);
			alertTextLeftButton.transform.parent.gameObject.SetActive(value: false);
			alertTextRightButton.transform.parent.gameObject.SetActive(value: false);
			alertTextCP.transform.gameObject.SetActive(value: false);
			alertInputCP.transform.gameObject.SetActive(value: false);
			alertInputPC.transform.gameObject.SetActive(value: false);
			SingleButton.interactable = false;
			SingleButton.interactable = true;
			LeftButton.interactable = false;
			LeftButton.interactable = true;
			RightButton.interactable = false;
			RightButton.interactable = true;
		}
		base.gameObject.SetActive(value: false);
		KeyboardManager.Instance.ShowKeyboard(state: false);
	}

	public void ShowDoorIcon()
	{
		doorIcon.gameObject.SetActive(value: true);
	}

	public void ShowResignIcon()
	{
		resignIcon.gameObject.SetActive(value: true);
	}

	public void ShowReloadIcon()
	{
		reloadIcon.gameObject.SetActive(value: true);
	}

	private void ShowAlert(bool isInput)
	{
		canvas.gameObject.SetActive(value: true);
		SetInitialPosition();
		base.gameObject.SetActive(value: true);
		popupT.gameObject.SetActive(value: true);
		playersT.gameObject.SetActive(value: false);
		doorIcon.gameObject.SetActive(value: false);
		resignIcon.gameObject.SetActive(value: false);
		reloadIcon.gameObject.SetActive(value: false);
		GameManager.Instance.PlayLibraryAudio("ui_menu_popup_01");
		if (isInput)
		{
			alertText.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 130f);
			exitButton.gameObject.SetActive(value: true);
			inputValue = "";
		}
		else
		{
			alertText.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 190f);
			exitButton.gameObject.SetActive(value: false);
		}
		if ((bool)PopupManager.Instance)
		{
			PopupManager.Instance.ClosePopup();
		}
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.HidePopup();
		}
		GameManager.Instance.CleanTempContainer();
	}

	public void CloseAlert(bool force = false)
	{
		if (alertTextLeftButton.transform.parent.gameObject.activeSelf || exitButton.gameObject.activeSelf)
		{
			SetConfirmAnswer(status: false);
		}
		else if (alertTextSingleButton.transform.parent.gameObject.activeSelf || force)
		{
			confirmAnswer = false;
			if (buttonClickDelegate != null)
			{
				buttonClickDelegate();
			}
			HideAlert();
		}
	}

	public void SetConfirmAnswer(bool status)
	{
		confirmAnswer = status;
		if (buttonClickDelegate != null)
		{
			buttonClickDelegate();
		}
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		HideAlert();
	}

	public bool GetConfirmAnswer()
	{
		return confirmAnswer;
	}

	public string GetInputValue()
	{
		return inputValue;
	}

	public void AlertInputSuccess()
	{
		inputValue = alertInput.text;
		if (inputValue != "" && buttonClickDelegate != null)
		{
			buttonClickDelegate();
		}
		HideAlert();
	}

	public void HideConfirmButton()
	{
		alertTextSingleButton.transform.parent.gameObject.SetActive(value: false);
	}

	public void AlertConfirm(string text, string textButton = "")
	{
		alertText.text = text;
		if (textButton != "")
		{
			alertTextSingleButton.text = textButton;
		}
		else
		{
			alertTextSingleButton.text = Texts.Instance.GetText("accept");
		}
		alertTextSingleButton.transform.parent.gameObject.SetActive(value: true);
		ShowAlert(isInput: false);
	}

	public void AlertConfirmDouble(string text, string textButtonRight = "", string textButtonLeft = "", bool showButtons = true)
	{
		alertText.text = text;
		if (textButtonRight != "")
		{
			alertTextRightButton.text = textButtonRight;
		}
		else
		{
			alertTextRightButton.text = Texts.Instance.GetText("accept");
		}
		if (textButtonLeft != "")
		{
			alertTextLeftButton.text = textButtonLeft;
		}
		else
		{
			alertTextLeftButton.text = Texts.Instance.GetText("cancel");
		}
		if (showButtons)
		{
			alertTextLeftButton.transform.parent.gameObject.SetActive(value: true);
			alertTextRightButton.transform.parent.gameObject.SetActive(value: true);
		}
		else
		{
			alertTextLeftButton.transform.parent.gameObject.SetActive(value: false);
			alertTextRightButton.transform.parent.gameObject.SetActive(value: false);
		}
		ShowAlert(isInput: false);
	}

	public void AlertCopyPaste(string titleText, string inputText)
	{
		alertTextCP.transform.gameObject.SetActive(value: true);
		alertInputCP.transform.gameObject.SetActive(value: true);
		alertTextCP.text = titleText;
		alertInputCP.text = inputText;
		alertTextSingleButton.text = Texts.Instance.GetText("close");
		alertTextSingleButton.transform.parent.gameObject.SetActive(value: true);
		ShowAlert(isInput: false);
	}

	public void AlertPasteCopy(string text, string textButton = "")
	{
		alertTextCP.transform.gameObject.SetActive(value: true);
		alertInputPC.transform.gameObject.SetActive(value: true);
		alertTextCP.text = text;
		alertInputPC.text = textButton;
		alertTextSingleButton.text = Texts.Instance.GetText("accept");
		alertTextSingleButton.transform.parent.gameObject.SetActive(value: true);
		ShowAlert(isInput: true);
		StartCoroutine(ActivateInputPC());
	}

	private IEnumerator ActivateInputPC()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		alertInputPC.Select();
		alertInputPC.ActivateInputField();
	}

	public string GetInputPCValue()
	{
		return alertInputPC.text;
	}

	public void AlertInput(string text, string textButton = "")
	{
		alertText.text = text;
		if (textButton != "")
		{
			alertInputButtonText.text = textButton;
		}
		else
		{
			alertInputButtonText.text = Texts.Instance.GetText("accept");
		}
		alertInput.transform.gameObject.SetActive(value: true);
		alertInput.text = "";
		ShowAlert(isInput: true);
		StartCoroutine(ActivateInput());
	}

	private IEnumerator ActivateInput()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		alertInput.Select();
		alertInput.ActivateInputField();
	}

	public void SetRestartPosition()
	{
		backgroundImage.enabled = false;
		popupT.localPosition = new Vector3(popupT.localPosition.x, 130f, popupT.localPosition.z);
	}

	public void SetInitialPosition()
	{
		backgroundImage.enabled = true;
		popupT.localPosition = new Vector3(popupT.localPosition.x, 40f, popupT.localPosition.z);
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
		_controllerList.Clear();
		for (int i = 0; i < menuControllerAlert.Count; i++)
		{
			if (Functions.TransformIsVisible(menuControllerAlert[i]))
			{
				_controllerList.Add(menuControllerAlert[i]);
			}
		}
		if (goingUp || goingLeft)
		{
			controllerHorizontalIndex--;
		}
		else
		{
			controllerHorizontalIndex++;
		}
		if (controllerHorizontalIndex < 0)
		{
			controllerHorizontalIndex = 0;
		}
		else if (controllerHorizontalIndex > _controllerList.Count - 1)
		{
			controllerHorizontalIndex = _controllerList.Count - 1;
		}
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = _controllerList[controllerHorizontalIndex].position;
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void MustSelect(string _charName)
	{
		string text = string.Format(Texts.Instance.GetText("youMustSelectHero"), _charName);
		AlertConfirm(text);
	}

	public void HoverOn()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover);
	}

	public void ShowPlayers()
	{
		canvas.gameObject.SetActive(value: true);
		SetInitialPosition();
		base.gameObject.SetActive(value: true);
		popupT.gameObject.SetActive(value: false);
		playersT.gameObject.SetActive(value: true);
		GameManager.Instance.PlayLibraryAudio("ui_menu_popup_01");
		if ((bool)PopupManager.Instance)
		{
			PopupManager.Instance.ClosePopup();
		}
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.HidePopup();
		}
		GameManager.Instance.CleanTempContainer();
	}

	public void SetPlayers()
	{
		for (int i = 0; i < 4; i++)
		{
			string playerNickPosition = NetworkManager.Instance.GetPlayerNickPosition(i);
			if (!playerNickPosition.IsNullOrEmpty())
			{
				playerList[i].gameObject.SetActive(value: true);
				playerList[i].SetPlayer(i, playerNickPosition);
			}
			else
			{
				playerList[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void UpdatePlayer(int slot)
	{
		if (slot > -1 && slot < playerList.Count && playerList[slot] != null)
		{
			playerList[slot].SetDescription();
		}
	}
}
