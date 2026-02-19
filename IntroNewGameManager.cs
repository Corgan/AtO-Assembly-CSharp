using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zone;

public class IntroNewGameManager : MonoBehaviour
{
	public TMP_Text title;

	public TMP_Text body;

	public Transform buttonContinue;

	public Transform bgSenenthia;

	public Transform bgHatch;

	public Transform bgVelkarath;

	public Transform bgAquarfall;

	public Transform bgSpiderLair;

	public Transform bgVoid;

	public Transform bgFaeborg;

	public Transform bgUlminin;

	public Transform bgPyramid;

	public Transform bgWoods;

	public Transform bgEndEarly;

	public Transform bgBlackForge;

	public Transform bgFrozenSewers;

	public Transform bgWolfWars;

	public Transform bgUprising;

	public Transform bgSahti;

	public Transform bgDreadnought;

	public Transform bgSunken;

	public Transform bgDreams;

	private Coroutine coFade;

	public static IntroNewGameManager Instance { get; private set; }

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
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("IntroNewGame");
			return;
		}
		GameManager.Instance.SetCamera();
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	private void Start()
	{
		string text = AtOManager.Instance.currentMapNode;
		bgSenenthia.gameObject.SetActive(value: false);
		bgHatch.gameObject.SetActive(value: false);
		bgVelkarath.gameObject.SetActive(value: false);
		bgAquarfall.gameObject.SetActive(value: false);
		bgSpiderLair.gameObject.SetActive(value: false);
		bgFaeborg.gameObject.SetActive(value: false);
		bgVoid.gameObject.SetActive(value: false);
		bgEndEarly.gameObject.SetActive(value: false);
		bgFrozenSewers.gameObject.SetActive(value: false);
		bgBlackForge.gameObject.SetActive(value: false);
		bgWolfWars.gameObject.SetActive(value: false);
		bgUlminin.gameObject.SetActive(value: false);
		bgPyramid.gameObject.SetActive(value: false);
		bgUprising.gameObject.SetActive(value: false);
		bgSahti.gameObject.SetActive(value: false);
		bgDreadnought.gameObject.SetActive(value: false);
		bgSunken.gameObject.SetActive(value: false);
		bgWoods.gameObject.SetActive(value: false);
		if (GameManager.Instance.CheatMode && GameManager.Instance.StartFromMap != MapType.None)
		{
			text = MapUtils.GetNodeName(GameManager.Instance.StartFromMap);
			AtOManager.Instance.currentMapNode = text;
		}
		if (AtOManager.Instance.IsAdventureCompleted())
		{
			DoFinishGame();
		}
		else if (text != "tutorial_0" && text != "sen_0" && text != "secta_0" && text != "velka_0" && text != "aqua_0" && !text.StartsWith("spider_") && text != "voidlow_0" && text != "faen_0" && text != "forge_0" && !text.StartsWith("sewers_") && text != "wolf_0" && text != "ulmin_0" && !text.StartsWith("pyr_") && text != "uprising_0" && text != "sahti_0" && text != "dread_0" && text != "sunken_0" && text != "dream_0" && text != "woods_0")
		{
			GameManager.Instance.ChangeScene("Map");
		}
		else
		{
			DoIntro(text);
		}
		if (GameManager.Instance.CheatMode && GameManager.Instance.StartFromMap != MapType.None)
		{
			GameManager.Instance.StartFromMap = MapType.None;
		}
	}

	private void DoFinishGame()
	{
		bgEndEarly.gameObject.SetActive(value: true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+1>");
		stringBuilder.Append(Texts.Instance.GetText("congratulations"));
		stringBuilder.Append("</size><br><color=#FFF>");
		stringBuilder.Append("Across the Obelisk");
		title.text = stringBuilder.ToString();
		body.text = Texts.Instance.GetText("actEndGame");
		buttonContinue.gameObject.SetActive(value: true);
		GameManager.Instance.SceneLoaded();
	}

	private void DoIntro(string currentMapNode)
	{
		if (currentMapNode == "sen_0" || currentMapNode == "tutorial_0")
		{
			AtOManager.Instance.CinematicId = "intro";
			AtOManager.Instance.LaunchCinematic();
			return;
		}
		int townTier = AtOManager.Instance.GetTownTier();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+2>");
		string id = "";
		switch (currentMapNode)
		{
		case "sen_0":
		case "tutorial_0":
			stringBuilder.Append(string.Format(Texts.Instance.GetText("actNumber"), 1));
			bgSenenthia.gameObject.SetActive(value: true);
			id = "Senenthia";
			break;
		case "secta_0":
			stringBuilder.Append(Texts.Instance.GetText("theHatch"));
			bgHatch.gameObject.SetActive(value: true);
			id = "Senenthia";
			break;
		default:
			if (currentMapNode.StartsWith("spider_"))
			{
				stringBuilder.Append(Texts.Instance.GetText("spiderLair"));
				bgSpiderLair.gameObject.SetActive(value: true);
				id = "Aquarfall";
				break;
			}
			if (currentMapNode == "forge_0")
			{
				stringBuilder.Append(Texts.Instance.GetText("blackForge"));
				bgBlackForge.gameObject.SetActive(value: true);
				id = "Velkarath";
				break;
			}
			if (currentMapNode.StartsWith("sewers_"))
			{
				stringBuilder.Append(Texts.Instance.GetText("frozenSewers"));
				bgFrozenSewers.gameObject.SetActive(value: true);
				id = "Faeborg";
				break;
			}
			if (currentMapNode == "wolf_0")
			{
				stringBuilder.Append(Texts.Instance.GetText("wolfWars"));
				bgWolfWars.gameObject.SetActive(value: true);
				id = "Senenthia";
				break;
			}
			if (currentMapNode.StartsWith("pyr_"))
			{
				stringBuilder.Append(Texts.Instance.GetText("pyramid"));
				bgPyramid.gameObject.SetActive(value: true);
				id = "Ulminin";
				break;
			}
			if (currentMapNode.StartsWith("dread_"))
			{
				stringBuilder.Append(Texts.Instance.GetText("dreadnought"));
				bgDreadnought.gameObject.SetActive(value: true);
				id = "Sahti";
				break;
			}
			if (currentMapNode == "sunken_0")
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("actNumber"), 2));
				bgSunken.gameObject.SetActive(value: true);
				id = "Sunken";
				break;
			}
			if (currentMapNode == "dream_0")
			{
				bgDreams.gameObject.SetActive(value: true);
				id = "Dreams";
				break;
			}
			stringBuilder.Append(string.Format(Texts.Instance.GetText("actNumber"), townTier + 2));
			switch (currentMapNode)
			{
			case "velka_0":
				bgVelkarath.gameObject.SetActive(value: true);
				id = "Velkarath";
				break;
			case "aqua_0":
				bgAquarfall.gameObject.SetActive(value: true);
				id = "Aquarfall";
				break;
			case "voidlow_0":
				bgVoid.gameObject.SetActive(value: true);
				id = "TheVoid";
				break;
			case "faen_0":
				bgFaeborg.gameObject.SetActive(value: true);
				id = "Faeborg";
				break;
			case "ulmin_0":
				bgUlminin.gameObject.SetActive(value: true);
				id = "Ulminin";
				break;
			case "uprising_0":
				bgUprising.gameObject.SetActive(value: true);
				id = "Uprising";
				break;
			case "sahti_0":
				bgSahti.gameObject.SetActive(value: true);
				id = "Sahti";
				break;
			case "woods_0":
				bgWoods.gameObject.SetActive(value: true);
				id = "WitchWoods";
				break;
			}
			break;
		}
		stringBuilder.Append("</size><br><color=#FFF>");
		stringBuilder.Append(Texts.Instance.GetText(id));
		title.text = stringBuilder.ToString();
		switch (currentMapNode)
		{
		case "sen_0":
		case "tutorial_0":
		case "velka_0":
		case "aqua_0":
		case "voidlow_0":
		case "faen_0":
		case "ulmin_0":
		case "sahti_0":
		case "woods_0":
			switch (currentMapNode)
			{
			case "sen_0":
			case "tutorial_0":
				body.text = Texts.Instance.GetText("act0Intro");
				break;
			case "woods_0":
				body.text = Texts.Instance.GetText("act3bIntro");
				break;
			default:
				body.text = Texts.Instance.GetText("act" + (townTier + 1) + "Intro");
				break;
			}
			buttonContinue.gameObject.SetActive(value: true);
			break;
		case "wolf_0":
			body.text = Texts.Instance.GetText("wolfWarsIntro");
			buttonContinue.gameObject.SetActive(value: true);
			break;
		case "uprising_0":
			body.text = Texts.Instance.GetText("uprisingIntro");
			buttonContinue.gameObject.SetActive(value: true);
			break;
		case "sunken_0":
			body.text = Texts.Instance.GetText("sunkenIntro");
			buttonContinue.gameObject.SetActive(value: true);
			break;
		case "dream_0":
			body.text = Texts.Instance.GetText("dreamIntro");
			buttonContinue.gameObject.SetActive(value: true);
			break;
		default:
			body.text = "";
			buttonContinue.gameObject.SetActive(value: true);
			coFade = StartCoroutine(FadeOut());
			break;
		}
		body.GetComponent<TextFade>().enabled = true;
		GameManager.Instance.SceneLoaded();
		ControllerMovement(goingUp: true);
	}

	private IEnumerator FadeOut()
	{
		yield return Globals.Instance.WaitForSeconds(4f);
		SkipIntro();
	}

	public void SkipIntro()
	{
		if (coFade != null)
		{
			StopCoroutine(coFade);
		}
		if (AtOManager.Instance.IsAdventureCompleted())
		{
			SceneStatic.LoadByName("FinishRun");
		}
		else
		{
			GameManager.Instance.ChangeScene("Map");
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false)
	{
		if (goingUp || goingLeft || goingRight || goingDown)
		{
			Vector2 position = GameManager.Instance.cameraMain.WorldToScreenPoint(buttonContinue.position);
			Mouse.current.WarpCursorPosition(position);
		}
	}
}
