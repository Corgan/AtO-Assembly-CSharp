using System.Collections;
using TMPro;
using UnityEngine;

public class BoxSelection : MonoBehaviour
{
	public SpriteRenderer sr;

	private Coroutine coColor;

	public Transform selection;

	public Transform boxBorder;

	public BoxPlayer[] boxPlayer;

	public TMP_Text playerOwner;

	public Transform lockImg;

	public Transform classImg;

	public Transform dice;

	public Transform disabledLayer;

	private int id;

	private string owner = "";

	public Transform playerReadyT;

	public Transform iconReady;

	public Transform iconNotReady;

	public Transform arrowController;

	private bool enabledRandom = true;

	private void Awake()
	{
		HidePlayers();
	}

	private void Start()
	{
		id = int.Parse(base.gameObject.name.Split('_')[1]);
		SetOwner("");
		ShowHideArrowController(_state: false);
		if (GameManager.Instance.IsLoadingGame() || GameManager.Instance.IsWeeklyChallenge())
		{
			enabledRandom = false;
		}
		if (enabledRandom)
		{
			dice.GetChild(0).GetComponent<RandomHeroSelector>().SetId(id);
			if (GameManager.Instance.IsMultiplayer())
			{
				dice.gameObject.SetActive(value: false);
			}
			else
			{
				dice.gameObject.SetActive(value: true);
			}
		}
		else
		{
			dice.gameObject.SetActive(value: false);
		}
	}

	private void HidePlayers()
	{
		for (int i = 0; i < 4; i++)
		{
			boxPlayer[i].gameObject.SetActive(value: false);
		}
	}

	public void ShowHideArrowController(bool _state)
	{
		if (arrowController.gameObject.activeSelf != _state)
		{
			arrowController.gameObject.SetActive(_state);
		}
	}

	public void ShowPlayer(int num)
	{
		boxPlayer[num].gameObject.SetActive(value: true);
	}

	public void SetOwner(string playerNick)
	{
		owner = playerNick;
		playerReadyT.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsLoadingGame() || GameManager.Instance.IsWeeklyChallenge())
		{
			enabledRandom = false;
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (playerNick != "")
			{
				playerReadyT.gameObject.SetActive(value: true);
			}
			if (playerNick != NetworkManager.Instance.GetPlayerNick())
			{
				lockImg.gameObject.SetActive(value: true);
				classImg.gameObject.SetActive(value: false);
				dice.gameObject.SetActive(value: false);
			}
			else
			{
				lockImg.gameObject.SetActive(value: false);
				classImg.gameObject.SetActive(value: true);
				if (enabledRandom)
				{
					dice.gameObject.SetActive(value: true);
				}
				else
				{
					dice.gameObject.SetActive(value: false);
				}
			}
		}
		else
		{
			lockImg.gameObject.SetActive(value: false);
			classImg.gameObject.SetActive(value: true);
			if (enabledRandom)
			{
				dice.gameObject.SetActive(value: true);
			}
			else
			{
				dice.gameObject.SetActive(value: false);
			}
		}
		playerOwner.text = NetworkManager.Instance.GetPlayerNickReal(playerNick);
		ActivePlayerNick(playerNick);
		if (playerNick != "")
		{
			playerOwner.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(playerNick));
			TransformColor(NetworkManager.Instance.GetColorFromNick(playerNick));
		}
		else
		{
			TransformColor("#87552D");
		}
	}

	public void SetReady(bool _state)
	{
		iconReady.gameObject.SetActive(_state);
		iconNotReady.gameObject.SetActive(!_state);
	}

	public int GetId()
	{
		return id;
	}

	public string GetOwner()
	{
		return owner;
	}

	public void HideSelection()
	{
		selection.gameObject.SetActive(value: false);
	}

	public void ShowSelection()
	{
		selection.gameObject.SetActive(value: true);
	}

	private void SelectPlayer(int num)
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != num)
			{
				boxPlayer[i].HideBorder();
			}
			else
			{
				boxPlayer[i].DrawBorder();
			}
		}
	}

	public void ActivePlayerNick(string playerNick)
	{
		for (int i = 0; i < 4; i++)
		{
			if (boxPlayer[i].playerNick != playerNick || playerNick == "")
			{
				boxPlayer[i].Activate(state: false);
			}
			else
			{
				boxPlayer[i].Activate(state: true);
			}
		}
	}

	public void SetPlayerPosition(int position, string playerName)
	{
		boxPlayer[position].SetName(playerName);
	}

	public void CheckSkuForHero()
	{
		if (!GameManager.Instance.IsMultiplayer() || !GameManager.Instance.IsLoadingGame() || GameManager.Instance.IsWeeklyChallenge() || !NetworkManager.Instance.IsMaster() || !(HeroSelectionManager.Instance.GetBoxHeroFromIndex(id) != null))
		{
			return;
		}
		string subclassName = HeroSelectionManager.Instance.GetBoxHeroFromIndex(id).GetSubclassName();
		SubClassData subClassData = Globals.Instance.GetSubClassData(subclassName);
		for (int i = 0; i < boxPlayer.Length; i++)
		{
			if (subClassData != null && subClassData.Sku != "" && !NetworkManager.Instance.PlayerHaveSku(boxPlayer[i].playerName.text, subClassData.Sku))
			{
				boxPlayer[i].DisableSKU(subClassData.Sku);
			}
		}
	}

	public void TransformColor(string color)
	{
		if (coColor != null)
		{
			StopCoroutine(coColor);
		}
		coColor = StartCoroutine(TransformColorCo(color));
	}

	private IEnumerator TransformColorCo(string color)
	{
		Color targetColor = Functions.HexToColor(color);
		float timeLeft = 0.2f;
		while (timeLeft > 0f)
		{
			sr.color = Color.Lerp(sr.color, targetColor, Time.deltaTime / timeLeft);
			timeLeft -= Time.deltaTime;
			yield return null;
		}
	}

	private void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && SceneStatic.GetSceneName() == "HeroSelection" && (!(HeroSelectionManager.Instance.charPopupGO != null) || !HeroSelectionManager.Instance.charPopup.IsOpened()))
		{
			HeroSelectionManager.Instance.MouseOverBox(null);
			boxBorder.gameObject.SetActive(value: false);
			if (HeroSelectionManager.Instance.dragging)
			{
				HeroSelectionManager.Instance.MouseOverBox(null);
			}
		}
	}

	private void OnMouseOver()
	{
		if (!AlertManager.Instance.IsActive() && SceneStatic.GetSceneName() == "HeroSelection" && (!(HeroSelectionManager.Instance.charPopupGO != null) || !HeroSelectionManager.Instance.charPopup.IsOpened()))
		{
			HeroSelectionManager.Instance.MouseOverBox(base.gameObject);
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && SceneStatic.GetSceneName() == "HeroSelection" && (!(HeroSelectionManager.Instance.charPopupGO != null) || !HeroSelectionManager.Instance.charPopup.IsOpened()) && HeroSelectionManager.Instance.dragging)
		{
			if (HeroSelectionManager.Instance.IsYourBox(base.gameObject.name))
			{
				boxBorder.gameObject.SetActive(value: true);
			}
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		}
	}

	public void ShowDisabledLayer(bool state)
	{
		disabledLayer.gameObject.SetActive(state);
	}
}
