using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
	public Transform elements;

	private TMP_InputField inputField;

	public TMP_InputField inputShow;

	public Transform chat;

	public TMP_Text textChat;

	private bool isChat;

	public RectTransform chatScroll;

	public List<TMP_Text> keyList;

	private string stringText = "";

	private bool shiftState = true;

	private int characterLimit;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static KeyboardManager Instance { get; private set; }

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
		for (int i = 0; i < keyList.Count; i++)
		{
			if (keyList[i] != null)
			{
				Button button = keyList[i].transform.parent.GetComponent<Button>();
				button.onClick.AddListener(delegate
				{
					Instance.DoKey(button.name.ToLower(), button.transform.GetChild(0).GetComponent<TMP_Text>().text);
				});
				if (Texts.Instance.GetText(button.name) != "")
				{
					button.transform.GetChild(0).GetComponent<TMP_Text>().text = Texts.Instance.GetText(button.name);
				}
			}
		}
		DoShift();
		ShowKeyboard(state: false);
	}

	public void ShowChat(bool state)
	{
		if (chat.gameObject.activeSelf != state)
		{
			chat.gameObject.SetActive(state);
		}
		isChat = state;
		if (isChat)
		{
			ChatText(ChatManager.Instance.chatText.text);
			inputField = ChatManager.Instance.chatInput;
			ChatManager.Instance.ClearMessages();
		}
		else
		{
			ChatManager.Instance.chatInput.text = "";
		}
	}

	public bool IsChat()
	{
		return isChat;
	}

	public void ChatText(string text)
	{
		if (isChat)
		{
			textChat.text = text;
			ChatManager.Instance.ClearMessages();
		}
	}

	public void DoKey(string name, string value)
	{
		if (inputField != null)
		{
			stringText = inputField.text;
		}
		switch (name)
		{
		case "keyspace":
			stringText += " ";
			break;
		case "keydelete":
			DoDelete();
			return;
		case "keyshift":
			DoShift();
			return;
		case "keyreturn":
			DoReturn();
			return;
		default:
			if (characterLimit <= 0 || stringText.Length < characterLimit)
			{
				stringText += value;
			}
			break;
		}
		WriteInputText();
	}

	private void WriteInputText()
	{
		inputShow.text = stringText;
		inputShow.MoveTextEnd(shift: true);
		if (inputField != null)
		{
			inputField.text = stringText;
			inputField.MoveTextEnd(shift: true);
		}
	}

	public bool IsActive()
	{
		return elements.gameObject.activeSelf;
	}

	public void ShowKeyboard(bool state, bool chat = false)
	{
		if (elements.gameObject.activeSelf != state)
		{
			elements.gameObject.SetActive(state);
			ShowChat(chat);
			if (state)
			{
				controllerHorizontalIndex = 24;
				Mouse.current.WarpCursorPosition(keyList[controllerHorizontalIndex].transform.position);
			}
			else if (inputField != null && Functions.TransformIsVisible(inputField.transform))
			{
				Mouse.current.WarpCursorPosition(inputField.transform.position);
			}
			stringText = "";
			inputShow.text = "";
			inputField = null;
		}
	}

	public void SetInputField(TMP_InputField input)
	{
		inputField = input;
		stringText = input.text;
		if (input.characterLimit > 0)
		{
			characterLimit = input.characterLimit;
		}
		else
		{
			characterLimit = -1;
		}
		WriteInputText();
	}

	public void DoDelete()
	{
		if (stringText.Length > 0)
		{
			stringText = stringText.Remove(stringText.Length - 1, 1);
			WriteInputText();
		}
	}

	public void DoShift()
	{
		shiftState = !shiftState;
		string text = "";
		for (int i = 0; i < keyList.Count; i++)
		{
			if (!(keyList[i] != null))
			{
				continue;
			}
			text = keyList[i].transform.parent.name.ToLower();
			if (text != "keyspace" && text != "keyshift" && text != "keyreturn" && text != "keydelete")
			{
				if (shiftState)
				{
					keyList[i].text = keyList[i].text.ToUpper();
				}
				else
				{
					keyList[i].text = keyList[i].text.ToLower();
				}
			}
		}
	}

	public void DoScroll(bool goUp)
	{
		if (goUp)
		{
			chatScroll.anchoredPosition = new Vector2(chatScroll.anchoredPosition.x, chatScroll.anchoredPosition.y - 20f);
		}
		else
		{
			chatScroll.anchoredPosition = new Vector2(chatScroll.anchoredPosition.x, chatScroll.anchoredPosition.y + 20f);
		}
	}

	private void DoReturn()
	{
		if (isChat)
		{
			ChatManager.Instance.ChatSend(stringText);
		}
		else if (Functions.TransformIsVisible(inputField.transform))
		{
			Mouse.current.WarpCursorPosition(GameManager.Instance.cameraMain.WorldToScreenPoint(inputField.transform.position));
		}
		ShowKeyboard(state: false);
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
		_controllerList.Clear();
		for (int i = 0; i < keyList.Count; i++)
		{
			_controllerList.Add(keyList[i].transform);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		if (controllerHorizontalIndex < 0)
		{
			controllerHorizontalIndex = 0;
		}
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft, checkUiItems: true);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = _controllerList[controllerHorizontalIndex].position;
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
