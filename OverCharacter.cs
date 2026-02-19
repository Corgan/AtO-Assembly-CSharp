using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class OverCharacter : MonoBehaviour
{
	private Animator anim;

	public SpriteRenderer shadowSPR;

	public Transform shadowOver;

	public Transform shadowHover;

	public Transform borderActive;

	public Transform lifeMod;

	public TMP_Text lifeModTxt;

	public Transform lifeModOk;

	public Transform lifeModKo;

	public Transform expMod;

	public Transform upgradeMod;

	public SpriteRenderer characterSR;

	public TMP_Text nameText;

	public TMP_Text hpText;

	public TMP_Text experienceText;

	public Transform deckT;

	public TMP_Text deckText;

	public Transform injuryT;

	public TMP_Text injuryText;

	public OverCharacterDeck overDeckDeck;

	public OverCharacterDeck overDeckInjury;

	public Transform mpMark;

	public SpriteRenderer mpMarkSPR;

	public SpriteRenderer mpMarkLife;

	private int currentHp = -1;

	private Hero hero;

	private bool active;

	private new bool enabled = true;

	private bool cardEnabled = true;

	private bool clickable;

	private bool inCharacterScreen;

	private int charIndex;

	private Color colorShadow = new Color(0.15f, 0.15f, 0.15f, 0.7f);

	private Color colorShadowActive = new Color(0f, 0.6f, 1f, 0.7f);

	public Transform challengeReadyButtons;

	public Transform challengeReadyOk;

	public Transform challengeReadyKo;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void Init(int index)
	{
		charIndex = index;
		lifeMod.gameObject.SetActive(value: false);
		upgradeMod.gameObject.SetActive(value: false);
		challengeReadyButtons.gameObject.SetActive(value: false);
		overDeckDeck.SetIndex(index);
		overDeckInjury.SetIndex(index);
		hero = AtOManager.Instance.GetHero(index);
		if (hero != null && !(hero.HeroData == null))
		{
			characterSR.sprite = hero.HeroData.HeroSubClass.SpriteSpeed;
			nameText.text = hero.SourceName;
			DoStats();
			DoCards();
			SetClickable(status: true);
			ShowLevelUp();
		}
	}

	public void ShowChallengeButtonReady(bool state)
	{
		challengeReadyButtons.gameObject.SetActive(value: true);
		if (state)
		{
			challengeReadyOk.gameObject.SetActive(value: true);
			challengeReadyKo.gameObject.SetActive(value: false);
		}
		else
		{
			challengeReadyKo.gameObject.SetActive(value: true);
			challengeReadyOk.gameObject.SetActive(value: false);
		}
	}

	public Vector3 CharacterIconPosition()
	{
		if (characterSR != null)
		{
			return characterSR.transform.position;
		}
		Debug.LogError("[CharacterIconPosition] characterSR = null");
		return Vector3.zero;
	}

	public void DoCards()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < hero.Cards.Count; i++)
		{
			CardData cardData = Globals.Instance.GetCardData(hero.Cards[i], instantiate: false);
			if (cardData != null)
			{
				if (cardData.CardClass != Enums.CardClass.Injury)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		deckText.text = num.ToString();
		if (num2 == 0)
		{
			injuryT.gameObject.SetActive(value: false);
			return;
		}
		injuryText.text = num2.ToString();
		injuryT.gameObject.SetActive(value: true);
	}

	public void DoStats()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(hero.HpCurrent);
		stringBuilder.Append("<size=-.5><color=#FFA7A5>/");
		stringBuilder.Append(hero.Hp);
		stringBuilder.Append("</color></size>");
		hpText.text = stringBuilder.ToString();
		if (currentHp != -1 && currentHp != hero.HpCurrent)
		{
			ShowLifeMod(hero.HpCurrent - currentHp);
		}
		currentHp = hero.HpCurrent;
		stringBuilder.Clear();
		stringBuilder.Append("L");
		stringBuilder.Append(hero.Level);
		if (hero.Level < 5)
		{
			stringBuilder.Append("  <voffset=.15><size=-.5><color=#FFC086>[");
			stringBuilder.Append(hero.Experience);
			stringBuilder.Append("/");
			stringBuilder.Append(Globals.Instance.GetExperienceByLevel(hero.Level));
			stringBuilder.Append("]");
		}
		experienceText.text = stringBuilder.ToString();
		ShowLevelUp();
		mpMark.gameObject.SetActive(value: true);
		if (!GameManager.Instance.IsMultiplayer())
		{
			mpMarkSPR.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
		}
		else
		{
			mpMarkSPR.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(hero.Owner));
		}
		SetMPBarLife();
	}

	private void SetMPBarLife()
	{
		if (hero.HpCurrent < hero.Hp)
		{
			float num = (float)hero.HpCurrent / (float)hero.Hp * 100f;
			mpMarkLife.size = new Vector2(6f, 4f - num * 4f / 100f);
		}
		else
		{
			mpMarkLife.size = new Vector2(6f, 0f);
		}
	}

	public void EnableCards(bool status)
	{
	}

	public void ShowActiveStatus(bool status)
	{
		active = status;
		if (status)
		{
			shadowSPR.color = colorShadowActive;
			shadowHover.gameObject.SetActive(value: false);
			challengeReadyButtons.transform.localPosition = new Vector3(1.79f, -0.35f, 1f);
		}
		else
		{
			shadowSPR.color = colorShadow;
			challengeReadyButtons.transform.localPosition = new Vector3(1.79f, -0.35f, 1f);
		}
	}

	public void SetActive(bool status)
	{
		ShowActiveStatus(status);
		if (status && ChallengeSelectionManager.Instance != null)
		{
			ChallengeSelectionManager.Instance.ChangeCharacter(charIndex);
		}
		if (((MapManager.Instance != null && MapManager.Instance.characterWindow.IsActive()) || !(CardCraftManager.Instance != null) || CardCraftManager.Instance.craftType == 3) && active)
		{
			if (MapManager.Instance != null)
			{
				MapManager.Instance.ShowCharacterWindow("", charIndex);
			}
			else if (TownManager.Instance != null)
			{
				TownManager.Instance.ShowCharacterWindow("", charIndex);
			}
			else if (MatchManager.Instance != null)
			{
				MatchManager.Instance.ShowCharacterWindow("", isHero: true, charIndex);
			}
		}
	}

	public void SetClickable(bool status)
	{
		GetComponent<BoxCollider2D>().enabled = status;
		clickable = status;
	}

	public void Enable()
	{
		shadowOver.gameObject.SetActive(value: false);
		enabled = true;
		cardEnabled = true;
	}

	public void Disable()
	{
		shadowOver.gameObject.SetActive(value: true);
		enabled = false;
	}

	public bool IsEnabled()
	{
		return enabled;
	}

	public bool IsCardEnabled()
	{
		return cardEnabled;
	}

	private void ShowLifeMod(int value)
	{
		if (!MatchManager.Instance)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (value < 0)
			{
				stringBuilder.Append(value.ToString());
				stringBuilder.Append(" <sprite name=heart>");
				lifeModTxt.text = stringBuilder.ToString();
				lifeModKo.gameObject.SetActive(value: true);
				lifeModOk.gameObject.SetActive(value: false);
				lifeMod.gameObject.SetActive(value: true);
			}
			else if (value > 0)
			{
				stringBuilder.Append("+");
				stringBuilder.Append(value.ToString());
				stringBuilder.Append(" <sprite name=heart>");
				lifeModTxt.text = stringBuilder.ToString();
				lifeModKo.gameObject.SetActive(value: false);
				lifeModOk.gameObject.SetActive(value: true);
				lifeMod.gameObject.SetActive(value: true);
			}
		}
	}

	public void ShowLevelUp()
	{
		if (hero != null && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(ShowLevelUpCo());
		}
	}

	private IEnumerator ShowLevelUpCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if ((CardCraftManager.Instance != null && CardCraftManager.Instance.gameObject.activeSelf && CardCraftManager.Instance.craftType != 3) || CardPlayerManager.Instance != null || (bool)MatchManager.Instance)
		{
			expMod.gameObject.SetActive(value: false);
			yield break;
		}
		string playerNick = NetworkManager.Instance.GetPlayerNick();
		if ((hero.Owner == null || hero.Owner == "" || hero.Owner == playerNick) && hero.IsReadyForLevelUp())
		{
			expMod.gameObject.SetActive(value: true);
		}
		else
		{
			expMod.gameObject.SetActive(value: false);
		}
	}

	public void ShowUpgrade()
	{
		if (hero != null)
		{
			string playerNick = NetworkManager.Instance.GetPlayerNick();
			if (hero.Owner == null || hero.Owner == "" || hero.Owner == playerNick)
			{
				upgradeMod.gameObject.SetActive(value: false);
				upgradeMod.gameObject.SetActive(value: true);
			}
		}
	}

	private void SetDeckNum(int num)
	{
		deckText.text = num.ToString();
	}

	private void SetInjuryNum(int num)
	{
		if (num == 0)
		{
			injuryT.gameObject.SetActive(value: false);
			return;
		}
		injuryT.gameObject.SetActive(value: true);
		injuryText.text = num.ToString();
	}

	public void InCharacterScreen(bool state)
	{
		inCharacterScreen = state;
		if (hero != null && anim != null)
		{
			anim.SetBool("show", state);
		}
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform) && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && !MadnessManager.Instance.IsActive() && !GiveManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()) && clickable && enabled && !active)
		{
			Clicked();
		}
	}

	public void Clicked()
	{
		if (!(CardCraftManager.Instance != null) || !CardCraftManager.Instance.blocked)
		{
			GameManager.Instance.SetCursorPlain();
			AtOManager.Instance.SideBarCharacterClicked(charIndex);
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && !MadnessManager.Instance.IsActive() && !GiveManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()) && clickable && enabled && !active)
		{
			GameManager.Instance.SetCursorHover();
			shadowHover.gameObject.SetActive(value: true);
			GameManager.Instance.PlayLibraryAudio("ui_click");
			if (!inCharacterScreen)
			{
				anim.SetBool("show", value: true);
			}
		}
	}

	private void OnMouseExit()
	{
		if (clickable && enabled)
		{
			GameManager.Instance.SetCursorPlain();
			shadowHover.gameObject.SetActive(value: false);
			if (!inCharacterScreen)
			{
				anim.SetBool("show", value: false);
			}
		}
	}
}
