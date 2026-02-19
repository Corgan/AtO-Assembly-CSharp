using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
	public GameObject playerStatusPrefab;

	private Dictionary<string, PlayerStatus> playerStatusList = new Dictionary<string, PlayerStatus>();

	public static PlayerStatusManager Instance { get; private set; }

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
	}

	private void Start()
	{
		Hide();
	}

	private void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	private void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void DrawPlayers()
	{
		Show();
		for (int i = 0; i < base.gameObject.transform.childCount; i++)
		{
			Object.Destroy(base.gameObject.transform.GetChild(i).gameObject);
		}
		string[] array = NetworkManager.Instance.PlayerPositionListArray();
		playerStatusList.Clear();
		for (int j = 0; j < array.Length; j++)
		{
			AddPlayer(array[j]);
		}
	}

	public void ShowPlayerStatusReady(bool state = true)
	{
		foreach (KeyValuePair<string, PlayerStatus> playerStatus in playerStatusList)
		{
			if (state)
			{
				playerStatus.Value.ShowStatusReady();
			}
			else
			{
				playerStatus.Value.HideStatusReady();
			}
		}
	}

	public void AddPlayer(string nick)
	{
		if (NetworkManager.Instance.GetPlayerListPosition(nick) > -1 && !playerStatusList.ContainsKey(nick))
		{
			PlayerStatus component = Object.Instantiate(playerStatusPrefab, Vector3.zero, Quaternion.identity, base.transform).GetComponent<PlayerStatus>();
			component.SetPlayer(nick);
			component.SetOnline();
			playerStatusList.Add(nick, component);
		}
	}

	public void SetStatus(string nick, bool status)
	{
		foreach (KeyValuePair<string, PlayerStatus> playerStatus in playerStatusList)
		{
			if (playerStatus.Key == nick)
			{
				playerStatus.Value.SetStatus(status);
			}
		}
	}
}
