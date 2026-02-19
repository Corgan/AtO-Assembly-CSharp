using UnityEngine;

public class SideCharacters : MonoBehaviour
{
	public OverCharacter[] charArray = new OverCharacter[4];

	private Transform[] charTransforms = new Transform[4];

	private int heroActive = -1;

	private void Awake()
	{
		for (int i = 0; i < 4; i++)
		{
			charTransforms[i] = charArray[i].transform;
		}
	}

	private void Start()
	{
		if (!(AtOManager.Instance == null) && AtOManager.Instance.GetTeam().Length != 0 && !MatchManager.Instance)
		{
			Show();
		}
	}

	public void Show()
	{
		Resize();
		Hero[] team = AtOManager.Instance.GetTeam();
		for (int i = 0; i < 4; i++)
		{
			if (AtOManager.Instance.currentMapNode == "tutorial_0" || AtOManager.Instance.currentMapNode == "tutorial_1")
			{
				if (i == 1 || i == 2)
				{
					continue;
				}
				if (i == 3)
				{
					charArray[i].transform.localPosition = new Vector3(0f, -1.24f * Globals.Instance.multiplierY, 0f);
				}
			}
			if (team != null && i < team.Length && team[i] != null && team[i].HeroData != null)
			{
				charArray[i].gameObject.SetActive(value: true);
				charArray[i].Init(i);
				charArray[i].Enable();
			}
		}
		if ((bool)TownManager.Instance || (bool)ChallengeSelectionManager.Instance || (bool)EventManager.Instance)
		{
			InCharacterScreen(state: true);
		}
	}

	public void InCharacterScreen(bool state)
	{
		for (int i = 0; i < 4; i++)
		{
			charArray[i].InCharacterScreen(state);
		}
	}

	public void Hide()
	{
		if ((bool)MatchManager.Instance)
		{
			for (int i = 0; i < 4; i++)
			{
				charArray[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void Resize()
	{
		float num = 1920f * (float)Screen.height / (1080f * (float)Screen.width);
		float x = (0f - Globals.Instance.sizeW) * 0.5f + 0.39f * Globals.Instance.multiplierX * num;
		float y = Globals.Instance.sizeH * 0.5f - 1.9f * Globals.Instance.multiplierY * num;
		base.transform.position = new Vector3(x, y, base.transform.position.z);
		for (int i = 0; i < 4; i++)
		{
			charTransforms[i].localPosition = new Vector3(0f, (float)i * -1.24f * Globals.Instance.multiplierY, 0f);
		}
	}

	public Vector3 CharacterIconPosition(int index)
	{
		return charArray[index].CharacterIconPosition();
	}

	public void Refresh()
	{
		if (AtOManager.Instance.currentMapNode == "tutorial_0" || AtOManager.Instance.currentMapNode == "tutorial_1" || AtOManager.Instance.currentMapNode == "tutorial_2")
		{
			Show();
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				charArray[i].Init(i);
			}
		}
		if (heroActive > -1)
		{
			charArray[heroActive].SetActive(status: true);
		}
	}

	public void RefreshCards(int hero = -1)
	{
		for (int i = 0; i < 4; i++)
		{
			if (hero == -1 || hero == i)
			{
				charArray[i].DoCards();
			}
		}
	}

	public void ShowChallengeButtons(int hero = -1, bool state = true)
	{
		for (int i = 0; i < 4; i++)
		{
			if (hero == -1 || hero == i)
			{
				charArray[i].ShowChallengeButtonReady(state);
			}
		}
	}

	public void ShowUpgrade(int hero = -1)
	{
		for (int i = 0; i < 4; i++)
		{
			if (hero == -1 || hero == i)
			{
				charArray[i].ShowUpgrade();
			}
		}
	}

	public void ResetCharacters()
	{
		for (int i = 0; i < 4; i++)
		{
			charArray[i].Enable();
			charArray[i].SetActive(status: false);
			charArray[i].SetClickable(status: true);
			charArray[i].SetClickable(status: true);
		}
		ShowLevelUpCharacters();
		heroActive = -1;
	}

	public void EnableOwnedCharacters(bool clickable = true)
	{
		Hero[] team = AtOManager.Instance.GetTeam();
		if (team == null || team.Length == 0)
		{
			return;
		}
		string playerNick = NetworkManager.Instance.GetPlayerNick();
		for (int i = 0; i < 4; i++)
		{
			if (team[i] == null || team[i].HeroData == null)
			{
				continue;
			}
			if (team[i].Owner == null || team[i].Owner == "" || team[i].Owner == playerNick)
			{
				charArray[i].Enable();
				charArray[i].SetClickable(clickable);
				if (clickable)
				{
					charArray[i].EnableCards(status: false);
				}
				else
				{
					charArray[i].EnableCards(status: true);
				}
			}
			else
			{
				charArray[i].Disable();
				charArray[i].EnableCards(status: false);
			}
			charArray[i].Enable();
			charArray[i].SetClickable(status: true);
		}
	}

	public void ShowLevelUpCharacters()
	{
		for (int i = 0; i < 4; i++)
		{
			charArray[i].ShowLevelUp();
		}
	}

	public void ShowActiveStatus(int characterIndex)
	{
		if (characterIndex > -1)
		{
			charArray[characterIndex].ShowActiveStatus(status: true);
		}
	}

	public void SetActive(int characterIndex)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != characterIndex)
			{
				charArray[i].SetActive(status: false);
			}
			else
			{
				charArray[i].SetActive(status: true);
			}
		}
		heroActive = characterIndex;
	}

	public int GetFirstEnabledCharacter()
	{
		string playerNick = NetworkManager.Instance.GetPlayerNick();
		Hero[] team = AtOManager.Instance.GetTeam();
		if (team == null || team.Length == 0)
		{
			return 0;
		}
		for (int i = 0; i < 4; i++)
		{
			if (team[i] != null && !(team[i].HeroData == null) && (team[i].Owner == null || team[i].Owner == "" || team[i].Owner == playerNick))
			{
				return i;
			}
		}
		return 0;
	}

	public void EnableAll()
	{
		for (int i = 0; i < 4; i++)
		{
			charArray[i].Enable();
		}
	}

	public void DisableAll(int _enableChar = -1)
	{
		for (int i = 0; i < 4; i++)
		{
			if (_enableChar != i)
			{
				charArray[i].Disable();
			}
		}
	}
}
