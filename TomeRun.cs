using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class TomeRun : MonoBehaviour
{
	public Transform hoverT;

	public Transform[] characters;

	private SpriteRenderer[] charactersSpr;

	private TMP_Text[] charactersName;

	public Transform madness;

	public TMP_Text madnessText;

	public TMP_Text description;

	public TMP_Text date;

	public TMP_Text type;

	public TMP_Text score;

	private PlayerRun playerRun;

	private int runIndex = -1;

	private void Awake()
	{
		if (characters.Length != 0)
		{
			charactersSpr = new SpriteRenderer[characters.Length];
			charactersName = new TMP_Text[characters.Length];
			for (int i = 0; i < characters.Length; i++)
			{
				charactersSpr[i] = characters[i].GetComponent<SpriteRenderer>();
				charactersName[i] = characters[i].GetChild(0).GetComponent<TMP_Text>();
			}
		}
	}

	public void SetRun(int _index = -1)
	{
		runIndex = _index;
		if (_index <= -1)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		playerRun = TomeManager.Instance.playerRunsList[_index];
		madness.gameObject.SetActive(value: false);
		if (!playerRun.ObeliskChallenge)
		{
			if (!playerRun.Singularity)
			{
				stringBuilder.Append(Texts.Instance.GetText("adventure"));
			}
			else
			{
				stringBuilder.Append(Texts.Instance.GetText("singularity"));
			}
		}
		else if (playerRun.WeeklyChallenge)
		{
			stringBuilder.Append(Texts.Instance.GetText("menuWeekly"));
		}
		else
		{
			stringBuilder.Append(Texts.Instance.GetText("menuChallenge"));
		}
		if (playerRun.TotalPlayers > 1)
		{
			stringBuilder.Append(" <color=#24646D><size=-.5>(");
			stringBuilder.Append(Texts.Instance.GetText("menuMultiplayer"));
			stringBuilder.Append(")</color></size>");
		}
		type.text = stringBuilder.ToString();
		stringBuilder.Clear();
		int num = 0;
		if (!playerRun.ObeliskChallenge)
		{
			num = (playerRun.Singularity ? playerRun.SingularityMadness : MadnessManager.Instance.CalculateMadnessTotal(playerRun.MadnessLevel, playerRun.MadnessCorruptors));
			if (num > 0)
			{
				madness.gameObject.SetActive(value: true);
				madnessText.text = num.ToString();
			}
		}
		else if (!playerRun.WeeklyChallenge && playerRun.ObeliskMadness > 0)
		{
			num = playerRun.ObeliskMadness;
			madness.gameObject.SetActive(value: true);
			madnessText.text = num.ToString();
		}
		date.text = Convert.ToDateTime(playerRun.gameDate).ToString("d");
		stringBuilder.Append(Functions.ScoreFormat(playerRun.FinalScore));
		stringBuilder.Append("  <voffset=.2><size=-.6><sprite name=experience></size></voffset>");
		score.text = stringBuilder.ToString();
		stringBuilder.Clear();
		stringBuilder.Append(Texts.Instance.GetText("placesVisited"));
		stringBuilder.Append(": <color=#222>");
		stringBuilder.Append(playerRun.VisitedNodes.Count);
		stringBuilder.Append("</color>\n");
		stringBuilder.Append(Texts.Instance.GetText("monstersKilledTome"));
		stringBuilder.Append(" <color=#222>");
		stringBuilder.Append(playerRun.MonstersKilled);
		stringBuilder.Append("</color>\n");
		stringBuilder.Append(Texts.Instance.GetText("bossesKilledTome"));
		stringBuilder.Append(" <color=#222>");
		stringBuilder.Append(playerRun.BossesKilled);
		stringBuilder.Append("</color>\n");
		description.text = stringBuilder.ToString();
		for (int i = 0; i < 4; i++)
		{
			charactersName[i].text = "";
		}
		DoPortraits();
	}

	private void DoPortraits()
	{
		SkinData skinData = null;
		string[] source = new string[4] { "warrior", "scout", "mage", "healer" };
		if (source.Contains(playerRun.Char0))
		{
			playerRun.Char0 = Functions.GetClassFromCards(playerRun.Char0Cards);
		}
		if (playerRun.Char0 != "")
		{
			if (playerRun.Char0Skin != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char0Skin);
			}
			if (playerRun.Char0Skin == "" || skinData == null)
			{
				skinData = Globals.Instance.GetSkinData(Globals.Instance.GetSkinBaseIdBySubclass(playerRun.Char0));
			}
			if (skinData != null)
			{
				charactersSpr[0].sprite = skinData.SpritePortrait;
			}
		}
		charactersName[0].text = playerRun.Char0Owner;
		if (source.Contains(playerRun.Char1))
		{
			playerRun.Char1 = Functions.GetClassFromCards(playerRun.Char1Cards);
		}
		if (playerRun.Char1 != "")
		{
			if (playerRun.Char1Skin != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char1Skin);
			}
			if (playerRun.Char1Skin == "" || skinData == null)
			{
				skinData = Globals.Instance.GetSkinData(Globals.Instance.GetSkinBaseIdBySubclass(playerRun.Char1));
			}
			if (skinData != null)
			{
				charactersSpr[1].sprite = skinData.SpritePortrait;
			}
		}
		charactersName[1].text = playerRun.Char1Owner;
		if (source.Contains(playerRun.Char2))
		{
			playerRun.Char2 = Functions.GetClassFromCards(playerRun.Char2Cards);
		}
		if (playerRun.Char2 != "")
		{
			if (playerRun.Char2Skin != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char2Skin);
			}
			if (playerRun.Char2Skin == "" || skinData == null)
			{
				skinData = Globals.Instance.GetSkinData(Globals.Instance.GetSkinBaseIdBySubclass(playerRun.Char2));
			}
			if (skinData != null)
			{
				charactersSpr[2].sprite = skinData.SpritePortrait;
			}
		}
		charactersName[2].text = playerRun.Char2Owner;
		if (source.Contains(playerRun.Char3))
		{
			playerRun.Char3 = Functions.GetClassFromCards(playerRun.Char3Cards);
		}
		if (playerRun.Char3 != "")
		{
			if (playerRun.Char3Skin != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char3Skin);
			}
			if (playerRun.Char3Skin == "" || skinData == null)
			{
				skinData = Globals.Instance.GetSkinData(Globals.Instance.GetSkinBaseIdBySubclass(playerRun.Char3));
			}
			if (skinData != null)
			{
				charactersSpr[3].sprite = skinData.SpritePortrait;
			}
		}
		charactersName[3].text = playerRun.Char3Owner;
	}

	private void OnMouseEnter()
	{
		hoverT.gameObject.SetActive(value: true);
		GameManager.Instance.SetCursorHover();
	}

	private void OnMouseExit()
	{
		hoverT.gameObject.SetActive(value: false);
		GameManager.Instance.SetCursorPlain();
	}

	public void OnMouseUp()
	{
		hoverT.gameObject.SetActive(value: false);
		GameManager.Instance.SetCursorPlain();
		if (runIndex > -1)
		{
			TomeManager.Instance.DoRun(runIndex);
		}
	}
}
