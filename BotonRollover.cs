using System.Collections;
using TMPro;
using UnityEngine;

public class BotonRollover : MonoBehaviour
{
	public Transform image;

	private SpriteRenderer imageBanner;

	public AudioClip sound;

	public Transform particles;

	public Transform rollOverText;

	public int auxInt;

	public bool fadeOnRoll = true;

	private TMP_Text textTMP;

	private float textY;

	private Sprite imgOriginal;

	private Vector3 imgSizeOriginal;

	private SpriteRenderer SR;

	private Coroutine Co;

	private float posEndTop;

	private float posEndBottom;

	private void Awake()
	{
		if (image != null)
		{
			SR = image.GetComponent<SpriteRenderer>();
		}
	}

	private void Start()
	{
		if (SR != null)
		{
			imgOriginal = SR.sprite;
		}
		if (image != null)
		{
			imgSizeOriginal = image.localScale;
			if (base.gameObject.name != "BotStats" && base.gameObject.name != "BotPerks" && image.childCount > 0 && image.GetChild(0) != null && image.GetChild(0).GetComponent<SpriteRenderer>() != null)
			{
				imageBanner = image.GetChild(0).GetComponent<SpriteRenderer>();
				imageBanner.color = new Color(0.7f, 0.7f, 0.7f, 0.9f);
			}
		}
		if (rollOverText != null)
		{
			textTMP = rollOverText.GetComponent<TMP_Text>();
			textY = rollOverText.localPosition.y;
			posEndTop = textY + 0.05f;
			posEndBottom = textY;
		}
	}

	private void fRollOver()
	{
		GameManager.Instance.SetCursorHover();
		if (sound != null)
		{
			GameManager.Instance.PlayAudio(sound, 0.1f);
		}
		_ = particles != null;
		if (rollOverText != null)
		{
			Co = StartCoroutine(ShowText(state: true));
		}
		if (image != null)
		{
			image.localScale = imgSizeOriginal + new Vector3(0.1f, 0.1f, 0.1f);
			if (imageBanner != null)
			{
				imageBanner.color = new Color(0.86f, 0.58f, 0.43f, 0.9f);
			}
		}
	}

	private void fRollOut()
	{
		GameManager.Instance.SetCursorPlain();
		if (particles != null)
		{
			particles.gameObject.SetActive(value: false);
		}
		if (rollOverText != null)
		{
			Co = StartCoroutine(ShowText(state: false));
		}
		if (image != null)
		{
			image.localScale = imgSizeOriginal;
			if (imageBanner != null)
			{
				imageBanner.color = new Color(0.7f, 0.7f, 0.7f, 0.9f);
			}
		}
	}

	private IEnumerator ShowText(bool state)
	{
		if (Co != null)
		{
			StopCoroutine(Co);
		}
		float num = ((!state) ? posEndBottom : posEndTop);
		int steps = 10;
		float step = (num - rollOverText.localPosition.y) / (float)steps;
		for (int i = 0; i < steps; i++)
		{
			rollOverText.localPosition += new Vector3(0f, step, 0f);
			if (fadeOnRoll)
			{
				if (state)
				{
					if (textTMP.color.a < 1f)
					{
						textTMP.color = new Color(textTMP.color.r, textTMP.color.g, textTMP.color.b, textTMP.color.a + 0.1f);
					}
				}
				else if (textTMP.color.a > 0f)
				{
					textTMP.color = new Color(textTMP.color.r, textTMP.color.g, textTMP.color.b, textTMP.color.a - 0.1f);
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
	}

	private void CloseWindows(string botName)
	{
		if ((bool)MatchManager.Instance)
		{
			if (MatchManager.Instance.characterWindow.IsActive())
			{
				MatchManager.Instance.characterWindow.Hide();
			}
			if (botName != "OptionsLog" && MatchManager.Instance != null && MatchManager.Instance.console.IsActive())
			{
				MatchManager.Instance.ShowLog();
			}
		}
	}

	public void OnMouseUp()
	{
		if (!Functions.ClickedThisTransform(base.transform) || ((bool)MatchManager.Instance && MatchManager.Instance.CombatLoading) || AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive() || ((bool)MapManager.Instance && MapManager.Instance.IsCharacterUnlock()) || ((bool)MatchManager.Instance && MatchManager.Instance.console.IsActive()))
		{
			return;
		}
		string text = base.gameObject.name;
		CloseWindows(text);
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		switch (text)
		{
		case "OptionsSettings":
			SettingsManager.Instance.ShowSettings(_state: true);
			break;
		case "OptionsExit":
			OptionsManager.Instance.Exit();
			break;
		case "OptionsTome":
			if (TeamManagement.Instance != null)
			{
				TeamManagement.Instance.EnableDisableTestingPanels(state: false);
			}
			TomeManager.Instance.ShowTome(_status: true);
			break;
		case "OptionsResign":
			if (MatchManager.Instance != null)
			{
				MatchManager.Instance.ResignCombat();
			}
			break;
		case "OptionsStats":
			DamageMeterManager.Instance.Show();
			break;
		case "OptionsRetry":
			if (MatchManager.Instance != null)
			{
				MatchManager.Instance.ALL_BreakByDesync();
			}
			break;
		case "MadnessTransform":
			MadnessManager.Instance.ShowMadness();
			break;
		case "CharacterDeck":
			if (CardScreenManager.Instance.IsActive())
			{
				return;
			}
			if (RewardsManager.Instance != null)
			{
				RewardsManager.Instance.ShowDeck(auxInt);
			}
			else if (LootManager.Instance != null)
			{
				LootManager.Instance.ShowDeck(auxInt);
			}
			else if (TownManager.Instance != null)
			{
				TownManager.Instance.ShowDeck(auxInt);
			}
			else if (MapManager.Instance != null)
			{
				MapManager.Instance.ShowDeck(auxInt);
			}
			break;
		case "OptionsLog":
			MatchManager.Instance.ShowLog();
			break;
		}
		fRollOut();
	}

	private void OnMouseExit()
	{
		fRollOut();
	}

	private void OnMouseEnter()
	{
		if ((!MatchManager.Instance || !MatchManager.Instance.CombatLoading) && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()) && (!MatchManager.Instance || !MatchManager.Instance.console.IsActive()))
		{
			fRollOver();
		}
	}
}
