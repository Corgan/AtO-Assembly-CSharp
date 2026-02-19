using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BotonEndTurn : MonoBehaviour
{
	public Transform border;

	public Transform background;

	private SpriteRenderer backgroundSR;

	private SpriteRenderer borderSR;

	private Color sourceColor;

	private Color bgColor;

	private bool showBorder;

	private Vector3 sizeOn = new Vector3(1.05f, 1.05f, 1f);

	private Vector3 sizeOff = new Vector3(1f, 1f, 1f);

	private Color colorFade = new Color(0f, 0f, 0f, 0.1f);

	private void Awake()
	{
		borderSR = border.GetComponent<SpriteRenderer>();
		backgroundSR = background.GetComponent<SpriteRenderer>();
		bgColor = backgroundSR.color;
		sourceColor = new Color(borderSR.color.r, borderSR.color.g, borderSR.color.b, 0f);
	}

	private void Update()
	{
		if (showBorder && borderSR.color.a < 0.7f)
		{
			borderSR.color += colorFade;
		}
		else if (!showBorder && borderSR.color.a > 0f)
		{
			borderSR.color -= colorFade;
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			base.transform.localScale = sizeOn;
			GameManager.Instance.SetCursorHover();
			borderSR.color = sourceColor;
			showBorder = true;
		}
	}

	private void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			GameManager.Instance.SetCursorPlain();
			base.transform.localScale = sizeOff;
			showBorder = false;
		}
	}

	public void OnMouseUp()
	{
		if (GameManager.Instance.DisableCardCast || !Functions.ClickedThisTransform(base.transform) || AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
		{
			return;
		}
		GameManager.Instance.SetCursorPlain();
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		showBorder = false;
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		Scene activeScene = SceneManager.GetActiveScene();
		string text = base.gameObject.name;
		if (activeScene.name == "Combat")
		{
			if (!MatchManager.Instance.CardDrag)
			{
				MatchManager.Instance.EndTurn();
			}
		}
		else if (activeScene.name == "Game")
		{
			if (text == "SinglePlayer")
			{
				SceneManager.LoadScene("TeamManagement");
			}
			else
			{
				SceneManager.LoadScene("Lobby");
			}
		}
		else if (activeScene.name == "TeamManagement")
		{
			TeamManagement.Instance.LaunchCombat();
		}
		else if (activeScene.name == "Lobby")
		{
			switch (text)
			{
			case "ButtonMultiplayerCreate":
				LobbyManager.Instance.ShowCreate();
				break;
			case "ButtonMultiplayerJoin":
				LobbyManager.Instance.ShowJoin();
				break;
			case "ButtonMultiplayerBack":
				LobbyManager.Instance.GoBack();
				break;
			case "SetReady":
				LobbyManager.Instance.SetReady();
				break;
			case "AllUnready":
				LobbyManager.Instance.AllUnready();
				break;
			}
		}
	}
}
