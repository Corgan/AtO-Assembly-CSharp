using System.Collections;
using TMPro;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
	public EmoteSmall[] emotes;

	public Sprite[] emotesSprite;

	private CircleCollider2D collider;

	private Coroutine hideCo;

	public SpriteRenderer characterPortrait;

	public SpriteRenderer characterPortraitBlocked;

	public Transform emoteText;

	public int heroActive = -1;

	public Transform blockedT;

	public TMP_Text blockedCounter;

	private bool blocked;

	private int counter;

	private int blockedTimeout = 3;

	private Vector3 posIni;

	private Vector3 posIniBlocked;

	private Coroutine blockedCo;

	private bool initiated;

	private void Awake()
	{
		collider = GetComponent<CircleCollider2D>();
		posIni = characterPortrait.transform.localPosition;
		posIniBlocked = characterPortraitBlocked.transform.parent.transform.localPosition;
	}

	private void Start()
	{
		for (int i = 0; i < emotes.Length; i++)
		{
			emotes[i].SetAction(i);
		}
		HideEmotes();
	}

	public void Init()
	{
		if (!initiated)
		{
			SelectNextCharacter();
			initiated = true;
		}
	}

	public void SetBlocked(bool _state)
	{
		blocked = _state;
		if (blockedT != null)
		{
			if (_state)
			{
				emoteText.gameObject.SetActive(value: false);
				blockedT.gameObject.SetActive(value: true);
				SetCounter();
			}
			else
			{
				emoteText.gameObject.SetActive(value: true);
				blockedT.gameObject.SetActive(value: false);
			}
		}
	}

	public bool IsBlocked()
	{
		return blocked;
	}

	private void SetCounter()
	{
		if (blockedCo != null)
		{
			StopCoroutine(blockedCo);
		}
		counter = blockedTimeout;
		blockedCo = StartCoroutine(SetCounterCo());
	}

	private IEnumerator SetCounterCo()
	{
		while (counter > 0)
		{
			if (blockedCounter != null)
			{
				blockedCounter.text = counter.ToString();
			}
			yield return Globals.Instance.WaitForSeconds(1f);
			counter--;
		}
		SetBlocked(_state: false);
	}

	private void SelectNextCharacter()
	{
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		if (teamHero != null)
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < 100)
			{
				num++;
				heroActive++;
				if (heroActive > 3)
				{
					heroActive = 0;
				}
				if ((teamHero[heroActive] != null && (teamHero[heroActive].Owner == NetworkManager.Instance.GetPlayerNick() || teamHero[heroActive].Owner == "")) || !GameManager.Instance.IsMultiplayer())
				{
					flag = true;
				}
			}
			if (flag && teamHero[heroActive] != null && teamHero[heroActive].HeroData != null)
			{
				SpriteRenderer spriteRenderer = characterPortrait;
				Sprite sprite = (characterPortraitBlocked.sprite = teamHero[heroActive].HeroData.HeroSubClass.StickerBase);
				spriteRenderer.sprite = sprite;
				characterPortrait.transform.localPosition = posIni + new Vector3(teamHero[heroActive].HeroData.HeroSubClass.StickerOffsetX, 0f, 0f);
				characterPortraitBlocked.transform.parent.transform.localPosition = posIniBlocked + new Vector3(teamHero[heroActive].HeroData.HeroSubClass.StickerOffsetX, 0f, 0f);
			}
		}
		if (teamHero[heroActive] == null || !(teamHero[heroActive].HeroData != null))
		{
			return;
		}
		if (teamHero[heroActive].Alive)
		{
			for (int i = 0; i < emotes.Length; i++)
			{
				emotes[i].SetBlocked(_state: false);
			}
			return;
		}
		for (int j = 0; j < emotes.Length; j++)
		{
			if (!EmoteNeedsTarget(j))
			{
				emotes[j].SetBlocked(_state: true);
			}
		}
	}

	private void SetEmotesForThisCharacter()
	{
		for (int i = 0; i < emotes.Length; i++)
		{
			emotes[i].SetAction(i);
		}
	}

	public bool EmoteNeedsTarget(int _action)
	{
		if (_action == 2 || _action == 3)
		{
			return true;
		}
		return false;
	}

	private void OnMouseEnter()
	{
		OnMouseEnterF();
	}

	private void OnMouseOver()
	{
		if (!blocked && !emotes[0].gameObject.activeSelf)
		{
			ShowEmotes();
		}
	}

	private void OnMouseEnterF()
	{
		if (!blocked && !MatchManager.Instance.waitingDeathScreen && !MatchManager.Instance.WaitingForActionScreen())
		{
			ShowEmotes();
		}
	}

	private void OnMouseExit()
	{
		if (!blocked)
		{
			HideEmotesCo();
		}
	}

	private void OnMouseUp()
	{
		if (!blocked)
		{
			SelectNextCharacter();
		}
	}

	public void HideEmotesCo()
	{
		if (hideCo != null)
		{
			StopCoroutine(hideCo);
		}
		hideCo = StartCoroutine(HideEmotesCoAction());
	}

	private IEnumerator HideEmotesCoAction()
	{
		yield return Globals.Instance.WaitForSeconds(0.2f);
		HideEmotes();
	}

	public void HideEmotes()
	{
		for (int i = 0; i < emotes.Length; i++)
		{
			emotes[i].Hide();
		}
		collider.radius = 0.48f;
		emoteText.gameObject.SetActive(value: true);
	}

	public void ShowEmotes()
	{
		if (hideCo != null)
		{
			StopCoroutine(hideCo);
		}
		if (!emotes[0].gameObject.activeSelf)
		{
			for (int i = 0; i < emotes.Length; i++)
			{
				emotes[i].Show();
			}
		}
		collider.radius = 1.4f;
		emoteText.gameObject.SetActive(value: false);
	}
}
