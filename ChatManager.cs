using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatManager : MonoBehaviour
{
	public Transform close;

	public Transform open;

	public Transform chatMessages;

	public TMP_Text chatMessagesText;

	public Transform playersButton;

	public TMP_Text playersButtonText;

	public Transform chatGO;

	public TMP_Text chatText;

	private StringBuilder chatSB;

	public TMP_InputField chatInput;

	private List<string> chatContent = new List<string>();

	private string status = "closed";

	private int messages;

	public static ChatManager Instance { get; private set; }

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
		DisableChat();
	}

	public void ChatSend(string chatStr = "")
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			string text = chatInput.text.Trim();
			if (chatStr != "")
			{
				text = chatStr.Trim();
			}
			if (text != "")
			{
				int myPosition = NetworkManager.Instance.GetMyPosition();
				string playerNick = NetworkManager.Instance.GetPlayerNick();
				string value = NetworkManager.Instance.ColorFromPosition(myPosition);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<b><color=");
				stringBuilder.Append(value);
				stringBuilder.Append(">[");
				stringBuilder.Append(NetworkManager.Instance.GetPlayerNickReal(playerNick));
				stringBuilder.Append("]</color></b> ");
				stringBuilder.Append(text);
				NetworkManager.Instance.ChatSend(stringBuilder.ToString(), showAlertIfClosed: true, NetworkManager.Instance.GetPlayerNickReal(playerNick));
				ChatText(stringBuilder.ToString(), showAlertIfClosed: false);
				stringBuilder = null;
				chatInput.text = string.Empty;
				chatInput.ActivateInputField();
			}
		}
	}

	public void WelcomeMsg(string roomName)
	{
		chatSB = new StringBuilder();
		chatSB.Append("<size=-1><color=#EFEAC5>");
		chatSB.Append(string.Format(Texts.Instance.GetText("chatWelcome"), roomName));
		chatSB.Append("</color></size>");
		chatContent = new List<string>();
		chatContent.Add(chatSB.ToString());
		chatSB.Append("\n");
		chatText.text = chatSB.ToString();
	}

	public void ChatText(string text, bool showAlertIfClosed)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<size=-2><color=#777>[");
			stringBuilder.Append(Functions.GetTimestampString());
			stringBuilder.Append("]</color></size> ");
			stringBuilder.Append(text);
			chatContent.Add(stringBuilder.ToString());
			if (chatContent.Count > 20)
			{
				chatContent.RemoveAt(0);
			}
			if (chatSB != null)
			{
				chatSB.Clear();
			}
			else
			{
				chatSB = new StringBuilder();
			}
			for (int i = 0; i < chatContent.Count; i++)
			{
				chatSB.Append(chatContent[i]);
				chatSB.Append("\n");
			}
			chatText.text = chatSB.ToString();
			if ((bool)KeyboardManager.Instance)
			{
				KeyboardManager.Instance.ChatText(chatSB.ToString());
			}
			if (showAlertIfClosed && status == "closed")
			{
				messages++;
				WriteChatMessagesWarning();
			}
		}
	}

	public void ClearMessages()
	{
		messages = 0;
		WriteChatMessagesWarning();
	}

	private void WriteChatMessagesWarning()
	{
		if (messages > 0)
		{
			chatMessagesText.text = messages.ToString();
			chatMessages.gameObject.SetActive(value: true);
		}
		else
		{
			chatMessages.gameObject.SetActive(value: false);
		}
	}

	public void ChatButton()
	{
		ClearMessages();
		if (status == "opened")
		{
			HideChat();
		}
		else
		{
			ShowChat();
		}
	}

	public void ShowChat()
	{
		ClearMessages();
		chatInput.onSubmit.AddListener(ChatSend);
		chatMessages.gameObject.SetActive(value: false);
		chatGO.gameObject.SetActive(value: true);
		close.gameObject.SetActive(value: true);
		open.gameObject.SetActive(value: false);
		status = "closed";
		SaveManager.SaveIntoPrefsBool("collapsedChat", value: false);
	}

	public void HideChat()
	{
		ClearMessages();
		chatInput.onSubmit.RemoveListener(ChatSend);
		chatGO.gameObject.SetActive(value: false);
		close.gameObject.SetActive(value: false);
		open.gameObject.SetActive(value: true);
		SaveManager.SaveIntoPrefsBool("collapsedChat", value: true);
	}

	public void DisableChat()
	{
		if (base.gameObject != null)
		{
			chatText.text = "";
			base.gameObject.SetActive(value: false);
			chatMessages.gameObject.SetActive(value: false);
		}
	}

	public void EnableChat()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(value: true);
			bool flag = false;
			if (Gamepad.current != null)
			{
				flag = true;
			}
			else if (SaveManager.PrefsHasKey("collapsedChat"))
			{
				flag = SaveManager.LoadPrefsBool("collapsedChat");
			}
			if (!flag)
			{
				ShowChat();
			}
			else
			{
				HideChat();
			}
		}
	}

	public void ShowPlayers()
	{
		AlertManager.Instance.ShowPlayers();
	}

	public void UpdatePlayersButton()
	{
		int numPlayers = NetworkManager.Instance.GetNumPlayers();
		if (numPlayers < 2)
		{
			if (playersButton.gameObject.activeSelf)
			{
				playersButton.gameObject.SetActive(value: false);
			}
			return;
		}
		if (!playersButton.gameObject.activeSelf)
		{
			playersButton.gameObject.SetActive(value: true);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("players"));
		stringBuilder.Append(" (");
		stringBuilder.Append(numPlayers);
		stringBuilder.Append(")");
	}
}
