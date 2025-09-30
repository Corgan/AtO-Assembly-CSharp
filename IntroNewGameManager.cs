// Decompiled with JetBrains decompiler
// Type: IntroNewGameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Paradox;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zone;

#nullable disable
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
    if ((Object) IntroNewGameManager.Instance == (Object) null)
      IntroNewGameManager.Instance = this;
    else if ((Object) IntroNewGameManager.Instance != (Object) this)
      Object.Destroy((Object) this.gameObject);
    if ((Object) GameManager.Instance == (Object) null)
    {
      SceneStatic.LoadByName("IntroNewGame");
    }
    else
    {
      GameManager.Instance.SetCamera();
      NetworkManager.Instance.StartStopQueue(true);
    }
  }

  private void Start()
  {
    string currentMapNode = AtOManager.Instance.currentMapNode;
    this.bgSenenthia.gameObject.SetActive(false);
    this.bgHatch.gameObject.SetActive(false);
    this.bgVelkarath.gameObject.SetActive(false);
    this.bgAquarfall.gameObject.SetActive(false);
    this.bgSpiderLair.gameObject.SetActive(false);
    this.bgFaeborg.gameObject.SetActive(false);
    this.bgVoid.gameObject.SetActive(false);
    this.bgEndEarly.gameObject.SetActive(false);
    this.bgFrozenSewers.gameObject.SetActive(false);
    this.bgBlackForge.gameObject.SetActive(false);
    this.bgWolfWars.gameObject.SetActive(false);
    this.bgUlminin.gameObject.SetActive(false);
    this.bgPyramid.gameObject.SetActive(false);
    this.bgUprising.gameObject.SetActive(false);
    this.bgSahti.gameObject.SetActive(false);
    this.bgDreadnought.gameObject.SetActive(false);
    this.bgSunken.gameObject.SetActive(false);
    if (GameManager.Instance.CheatMode && GameManager.Instance.StartFromMap != MapType.None)
    {
      currentMapNode = MapUtils.GetNodeName(GameManager.Instance.StartFromMap);
      AtOManager.Instance.currentMapNode = currentMapNode;
    }
    if (AtOManager.Instance.IsAdventureCompleted())
      this.DoFinishGame();
    else if (currentMapNode != "tutorial_0" && currentMapNode != "sen_0" && currentMapNode != "secta_0" && currentMapNode != "velka_0" && currentMapNode != "aqua_0" && !currentMapNode.StartsWith("spider_") && currentMapNode != "voidlow_0" && currentMapNode != "faen_0" && currentMapNode != "forge_0" && !currentMapNode.StartsWith("sewers_") && currentMapNode != "wolf_0" && currentMapNode != "ulmin_0" && !currentMapNode.StartsWith("pyr_") && currentMapNode != "uprising_0" && currentMapNode != "sahti_0" && currentMapNode != "dread_0" && currentMapNode != "sunken_0" && currentMapNode != "dream_0")
      GameManager.Instance.ChangeScene("Map");
    else
      this.DoIntro(currentMapNode);
    if (!GameManager.Instance.CheatMode || GameManager.Instance.StartFromMap == MapType.None)
      return;
    GameManager.Instance.StartFromMap = MapType.None;
  }

  private void DoFinishGame()
  {
    this.bgEndEarly.gameObject.SetActive(true);
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<size=+1>");
    stringBuilder.Append(Texts.Instance.GetText("congratulations"));
    stringBuilder.Append("</size><br><color=#FFF>");
    stringBuilder.Append("Across the Obelisk");
    this.title.text = stringBuilder.ToString();
    this.body.text = Texts.Instance.GetText("actEndGame");
    this.buttonContinue.gameObject.SetActive(true);
    GameManager.Instance.SceneLoaded();
  }

  private void DoIntro(string currentMapNode)
  {
    if (currentMapNode == "sen_0" || currentMapNode == "tutorial_0")
    {
      AtOManager.Instance.CinematicId = "intro";
      AtOManager.Instance.LaunchCinematic();
    }
    else
    {
      int townTier = AtOManager.Instance.GetTownTier();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("<size=+2>");
      string _id = "";
      if (currentMapNode == "sen_0" || currentMapNode == "tutorial_0")
      {
        stringBuilder.Append(string.Format(Texts.Instance.GetText("actNumber"), (object) 1));
        this.bgSenenthia.gameObject.SetActive(true);
        _id = "Senenthia";
      }
      else if (currentMapNode == "secta_0")
      {
        stringBuilder.Append(Texts.Instance.GetText("theHatch"));
        this.bgHatch.gameObject.SetActive(true);
        _id = "Senenthia";
      }
      else if (currentMapNode.StartsWith("spider_"))
      {
        stringBuilder.Append(Texts.Instance.GetText("spiderLair"));
        this.bgSpiderLair.gameObject.SetActive(true);
        _id = "Aquarfall";
      }
      else if (currentMapNode == "forge_0")
      {
        stringBuilder.Append(Texts.Instance.GetText("blackForge"));
        this.bgBlackForge.gameObject.SetActive(true);
        _id = "Velkarath";
      }
      else if (currentMapNode.StartsWith("sewers_"))
      {
        stringBuilder.Append(Texts.Instance.GetText("frozenSewers"));
        this.bgFrozenSewers.gameObject.SetActive(true);
        _id = "Faeborg";
      }
      else if (currentMapNode == "wolf_0")
      {
        stringBuilder.Append(Texts.Instance.GetText("wolfWars"));
        this.bgWolfWars.gameObject.SetActive(true);
        _id = "Senenthia";
      }
      else if (currentMapNode.StartsWith("pyr_"))
      {
        stringBuilder.Append(Texts.Instance.GetText("pyramid"));
        this.bgPyramid.gameObject.SetActive(true);
        _id = "Ulminin";
      }
      else if (currentMapNode.StartsWith("dread_"))
      {
        stringBuilder.Append(Texts.Instance.GetText("dreadnought"));
        this.bgDreadnought.gameObject.SetActive(true);
        _id = "Sahti";
      }
      else
      {
        switch (currentMapNode)
        {
          case "sunken_0":
            stringBuilder.Append(string.Format(Texts.Instance.GetText("actNumber"), (object) 2));
            this.bgSunken.gameObject.SetActive(true);
            _id = "Sunken";
            break;
          case "dream_0":
            this.bgDreams.gameObject.SetActive(true);
            _id = "Dreams";
            break;
          default:
            stringBuilder.Append(string.Format(Texts.Instance.GetText("actNumber"), (object) (townTier + 2)));
            switch (currentMapNode)
            {
              case "velka_0":
                this.bgVelkarath.gameObject.SetActive(true);
                _id = "Velkarath";
                break;
              case "aqua_0":
                this.bgAquarfall.gameObject.SetActive(true);
                _id = "Aquarfall";
                break;
              case "voidlow_0":
                this.bgVoid.gameObject.SetActive(true);
                _id = "TheVoid";
                break;
              case "faen_0":
                this.bgFaeborg.gameObject.SetActive(true);
                _id = "Faeborg";
                break;
              case "ulmin_0":
                this.bgUlminin.gameObject.SetActive(true);
                _id = "Ulminin";
                break;
              case "uprising_0":
                this.bgUprising.gameObject.SetActive(true);
                _id = "Uprising";
                break;
              case "sahti_0":
                this.bgSahti.gameObject.SetActive(true);
                _id = "Sahti";
                break;
            }
            break;
        }
      }
      stringBuilder.Append("</size><br><color=#FFF>");
      stringBuilder.Append(Texts.Instance.GetText(_id));
      this.title.text = stringBuilder.ToString();
      if (currentMapNode == "sen_0" || currentMapNode == "tutorial_0" || currentMapNode == "velka_0" || currentMapNode == "aqua_0" || currentMapNode == "voidlow_0" || currentMapNode == "faen_0" || currentMapNode == "ulmin_0" || currentMapNode == "sahti_0")
      {
        this.body.text = currentMapNode == "sen_0" || currentMapNode == "tutorial_0" ? Texts.Instance.GetText("act0Intro") : Texts.Instance.GetText("act" + (townTier + 1).ToString() + "Intro");
        this.buttonContinue.gameObject.SetActive(true);
      }
      else
      {
        switch (currentMapNode)
        {
          case "wolf_0":
            this.body.text = Texts.Instance.GetText("wolfWarsIntro");
            this.buttonContinue.gameObject.SetActive(true);
            break;
          case "uprising_0":
            this.body.text = Texts.Instance.GetText("uprisingIntro");
            this.buttonContinue.gameObject.SetActive(true);
            break;
          case "sunken_0":
            this.body.text = Texts.Instance.GetText("sunkenIntro");
            this.buttonContinue.gameObject.SetActive(true);
            break;
          case "dream_0":
            this.body.text = Texts.Instance.GetText("dreamIntro");
            this.buttonContinue.gameObject.SetActive(true);
            break;
          default:
            this.body.text = "";
            this.buttonContinue.gameObject.SetActive(true);
            this.coFade = this.StartCoroutine(this.FadeOut());
            break;
        }
      }
      this.body.GetComponent<TextFade>().enabled = true;
      Telemetry.SendActStart();
      GameManager.Instance.SceneLoaded();
      this.ControllerMovement(true);
    }
  }

  private IEnumerator FadeOut()
  {
    yield return (object) Globals.Instance.WaitForSeconds(4f);
    this.SkipIntro();
  }

  public void SkipIntro()
  {
    if (this.coFade != null)
      this.StopCoroutine(this.coFade);
    if (AtOManager.Instance.IsAdventureCompleted())
      SceneStatic.LoadByName("FinishRun");
    else
      GameManager.Instance.ChangeScene("Map");
  }

  public void ControllerMovement(
    bool goingUp = false,
    bool goingRight = false,
    bool goingDown = false,
    bool goingLeft = false,
    bool shoulderLeft = false,
    bool shoulderRight = false)
  {
    if (!(goingUp | goingLeft | goingRight | goingDown))
      return;
    Mouse.current.WarpCursorPosition((Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.buttonContinue.position));
  }
}
