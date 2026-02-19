using System.Text;
using TMPro;
using UnityEngine;

public class RoomList : MonoBehaviour
{
	[SerializeField]
	private string _roomName = "";

	private string _roomPwd = "";

	public TMP_Text _Name;

	public TMP_Text _Creator;

	public TMP_Text _Players;

	public TMP_Text _Version;

	public GameObject lfm;

	private string _crossplay;

	private string _platform;

	private int _ActivePlayers;

	private int _MaxPlayers;

	public Transform _Lock;

	public string RoomName => _roomName;

	public bool Updated { get; set; }

	public string Crossplay
	{
		get
		{
			return _crossplay;
		}
		set
		{
			_crossplay = value;
		}
	}

	public string Platform
	{
		get
		{
			return _platform;
		}
		set
		{
			_platform = value;
		}
	}

	public void SetRoomName(string text)
	{
		_roomName = text;
	}

	public void SetLfm(string state)
	{
		if (state != "")
		{
			lfm.SetActive(value: true);
		}
		else
		{
			lfm.SetActive(value: false);
		}
	}

	public void SetRoomDescription(string text)
	{
		_Name.text = text;
	}

	public void SetRoomPlayers(int numPlayers, int maxPlayers)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(numPlayers).Append("<size=-10><voffset=4>/</voffset></size>").Append(maxPlayers);
		_Players.text = stringBuilder.ToString();
		_ActivePlayers = numPlayers;
		_MaxPlayers = maxPlayers;
	}

	public void SetRoomCrossplayPlatform(string crossplay, string platform)
	{
		_crossplay = crossplay;
		_platform = platform;
	}

	public void SetRoomVersion(string version, string platform = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (platform != null && platform.Trim() != string.Empty)
		{
			stringBuilder.Append(platform).Append("<space=8><size=-4><voffset=3>|</voffset></size><space=8>");
		}
		stringBuilder.Append(version);
		_Version.text = stringBuilder.ToString();
	}

	public bool IsEmpty()
	{
		if (_ActivePlayers == 0)
		{
			return true;
		}
		return false;
	}

	public bool IsFull()
	{
		if (_ActivePlayers == _MaxPlayers)
		{
			return true;
		}
		return false;
	}

	private int GetRoomPlayers()
	{
		return _ActivePlayers;
	}

	public void SetRoomCreator(string text)
	{
		_Creator.text = text;
	}

	public void SetRoomPassword(string text)
	{
		_roomPwd = text;
		if (text != "")
		{
			SetLock(value: true);
		}
		else
		{
			SetLock(value: false);
		}
	}

	private void SetLock(bool value)
	{
		if (!(_Lock == null) && !(_Lock.gameObject == null))
		{
			if (value)
			{
				_Lock.gameObject.SetActive(value: true);
			}
			else
			{
				_Lock.gameObject.SetActive(value: false);
			}
		}
	}

	public void JoinRoom()
	{
		LobbyManager.Instance.JoinRoom(_roomName, _roomPwd);
	}
}
