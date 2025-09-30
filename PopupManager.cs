// Decompiled with JetBrains decompiler
// Type: PopupManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebSocketSharp;

#nullable disable
public class PopupManager : MonoBehaviour
{
  public GameObject popupPrefab;
  private Image imageBg;
  private bool popupActive;
  private bool followMouse;
  private bool fastPopup;
  private Coroutine coroutine;
  private GameObject GO_Popup;
  private Transform canvasContainer;
  private CanvasScaler canvasContainerScaler;
  private RectTransform canvasRect;
  private Transform popupContainer;
  private RectTransform popupRect;
  private RectTransform textRect;
  private GameObject pop;
  private GameObject popTrait;
  private GameObject popTown;
  private GameObject popUnlocked;
  private GameObject popPerk;
  private TMP_Text popText;
  private TMP_Text popTraitText;
  private TMP_Text popTownText;
  private TMP_Text popPerkText;
  private List<string> initialPopupGOs;
  private string lastPop = "";
  private Transform lastTransform;
  private string popupText = "<line-height=30><size=22><color=#{2}>{0}</color></size>{3}</line-height>\n{1}";
  private Dictionary<string, GameObject> KeyNotesGO;
  private List<string> KeyNotesActive;
  private Transform theTF;
  private Vector3 destinationPosition;
  private Vector3 adjustPosition;
  private Vector3 theTFposition;
  private Vector3 absolutePosition;
  private bool absolutePositionStablished;
  private Vector2 followSizeDelta;
  private string position = "";
  private Color colorBad = new Color(0.49f, 0.0f, 0.04f, 1f);
  private Color colorGood = new Color(0.01f, 0.22f, 0.52f, 1f);
  private Color colorPlain = new Color(0.25f, 0.25f, 0.25f, 1f);
  private Color colorCardtype = new Color(0.35f, 0.16f, 0.0f, 1f);
  private Color colorVanish = new Color(0.36f, 0.0f, 0.47f, 1f);
  private Color colorInnate = new Color(0.0f, 0.44f, 0.14f, 1f);
  private Color colorTrait = new Color(0.33f, 0.3f, 0.23f, 1f);
  private Color colorUnlocked = new Color(0.02f, 0.65f, 0.0f, 1f);
  private Color colorTown = new Color(0.29f, 0.34f, 0.47f, 1f);
  private int adjustCardX = 60;

  public static PopupManager Instance { get; private set; }

  private void Awake()
  {
    if ((UnityEngine.Object) PopupManager.Instance == (UnityEngine.Object) null)
      PopupManager.Instance = this;
    else if ((UnityEngine.Object) PopupManager.Instance != (UnityEngine.Object) this)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    this.KeyNotesGO = new Dictionary<string, GameObject>();
    this.KeyNotesActive = new List<string>();
  }

  private void Start()
  {
    this.GO_Popup = UnityEngine.Object.Instantiate<GameObject>(this.popupPrefab, Vector3.zero, Quaternion.identity);
    this.canvasContainer = this.GO_Popup.transform.GetChild(0);
    this.canvasContainerScaler = this.canvasContainer.GetComponent<CanvasScaler>();
    this.canvasRect = this.canvasContainer.GetComponent<RectTransform>();
    this.popupContainer = this.canvasContainer.GetChild(0);
    this.popupRect = this.popupContainer.GetComponent<RectTransform>();
    this.popUnlocked = this.popupContainer.GetChild(0).gameObject;
    this.popUnlocked.gameObject.SetActive(false);
    this.pop = this.popupContainer.GetChild(1).gameObject;
    this.popText = this.pop.transform.Find("Background/Text").GetComponent<TMP_Text>();
    this.textRect = this.popText.GetComponent<RectTransform>();
    this.imageBg = this.pop.transform.GetChild(0).GetComponent<Image>();
    this.imageBg.color = this.colorPlain;
    this.pop.SetActive(false);
    this.GO_Popup.SetActive(false);
    this.initialPopupGOs = new List<string>();
    foreach (Component component in this.popupContainer)
      this.initialPopupGOs.Add(component.gameObject.name);
    this.CreateKeyNotes();
    this.Resize();
  }

  private void Update()
  {
    if (!this.popupActive)
      return;
    if (this.followMouse)
    {
      if ((UnityEngine.Object) this.theTF != (UnityEngine.Object) null)
      {
        this.adjustPosition = !(this.position == "followright") ? new Vector3(-this.followSizeDelta.x - (float) this.adjustCardX, this.theTF.GetComponent<BoxCollider2D>().size.y, 0.0f) : new Vector3(this.followSizeDelta.x + (float) this.adjustCardX, this.theTF.GetComponent<BoxCollider2D>().size.y, 0.0f);
        this.destinationPosition = new Vector3((float) ((double) this.theTF.position.x * 100.0 + (double) this.adjustPosition.x * 0.89999997615814209), (float) ((double) this.theTF.position.y * 100.0 + (double) this.adjustPosition.y * 1.0 - (double) this.followSizeDelta.y * 5.0 * 0.10000000149011612), 1f);
        this.destinationPosition = this.RecalcPositionLimits(this.destinationPosition);
        if ((double) Vector3.Distance(this.popupContainer.localPosition, this.destinationPosition) <= 1.0)
          return;
        this.popupContainer.localPosition = Vector3.Lerp(this.popupContainer.localPosition, this.destinationPosition, 8f * Time.deltaTime);
      }
      else
      {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.destinationPosition = !(this.position == "followdown") ? (!(this.position == "followdown2") ? new Vector3(worldPoint.x * 100f, (float) (((double) worldPoint.y + 0.20000000298023224) * 100.0), 1f) : new Vector3((float) (((double) worldPoint.x - 3.0) * 100.0), (float) (((double) worldPoint.y - 5.4000000953674316 - (double) this.followSizeDelta.y * (1.0 / 1000.0)) * 100.0), 1f)) : new Vector3(worldPoint.x * 100f, (float) (((double) worldPoint.y - 1.5 - (double) this.followSizeDelta.y * (1.0 / 1000.0)) * 100.0), 1f);
        float num = this.followSizeDelta.x * 0.6f;
        if ((double) this.destinationPosition.x * 0.0099999997764825821 - (double) num * 0.0099999997764825821 < (double) Globals.Instance.sizeW * -0.47999998927116394)
          this.destinationPosition = new Vector3((float) ((double) Globals.Instance.sizeW * -0.47999998927116394 * 100.0) + num, this.destinationPosition.y, this.destinationPosition.z);
        else if ((double) this.destinationPosition.x * 0.0099999997764825821 + (double) num * 0.0099999997764825821 > (double) Globals.Instance.sizeW * 0.47999998927116394)
          this.destinationPosition = new Vector3((float) ((double) Globals.Instance.sizeW * 0.47999998927116394 * 100.0) - num, this.destinationPosition.y, this.destinationPosition.z);
        if ((double) this.destinationPosition.y * 0.0099999997764825821 + (double) this.followSizeDelta.y * 0.0099999997764825821 > (double) Globals.Instance.sizeH * 0.44999998807907104)
          this.destinationPosition = new Vector3(this.destinationPosition.x, (float) ((double) Globals.Instance.sizeH * 0.44999998807907104 * 100.0) - this.followSizeDelta.y, this.destinationPosition.z);
        if ((double) Vector3.Distance(this.popupContainer.localPosition, this.destinationPosition) <= 1.0)
          return;
        this.popupContainer.localPosition = Vector3.Lerp(this.popupContainer.localPosition, this.destinationPosition, 8f * Time.deltaTime);
      }
    }
    else
    {
      if ((UnityEngine.Object) this.theTF != (UnityEngine.Object) null)
      {
        Vector3 zero = Vector3.zero;
        if (this.theTFposition != Vector3.zero)
        {
          Vector3 vector3 = this.theTFposition - this.theTF.localPosition;
          this.theTFposition = this.theTF.localPosition;
          this.destinationPosition -= new Vector3(vector3.x * 10f, vector3.y * 10f, 0.0f);
        }
      }
      float num = (float) ((double) this.followSizeDelta.x * 0.5 + 50.0);
      if ((double) this.destinationPosition.x * 0.0099999997764825821 - (double) num * 0.0099999997764825821 < (double) Globals.Instance.sizeW * -0.5)
        this.destinationPosition = new Vector3((float) ((double) Globals.Instance.sizeW * -0.48500001430511475 * 100.0) + num, this.destinationPosition.y, this.destinationPosition.z);
      else if ((double) this.destinationPosition.x * 0.0099999997764825821 + (double) num * 0.0099999997764825821 > (double) Globals.Instance.sizeW * 0.5)
        this.destinationPosition = new Vector3((float) ((double) Globals.Instance.sizeW * 0.48500001430511475 * 100.0) - num, this.destinationPosition.y, this.destinationPosition.z);
      else if ((double) this.destinationPosition.y * 0.0099999997764825821 + (double) this.followSizeDelta.y * 0.0099999997764825821 > (double) Globals.Instance.sizeH * 0.44999998807907104)
        this.destinationPosition = new Vector3(this.destinationPosition.x, (float) ((double) Globals.Instance.sizeH * 0.44999998807907104 * 100.0) - this.followSizeDelta.y, this.destinationPosition.z);
      if ((double) Vector3.Distance(this.popupContainer.localPosition, this.destinationPosition) > 1.0)
      {
        this.popupContainer.localPosition = Vector3.Lerp(this.popupContainer.localPosition, this.destinationPosition, 8f * Time.deltaTime);
      }
      else
      {
        this.popupActive = false;
        this.popupContainer.localPosition = new Vector3((float) Mathf.CeilToInt(this.destinationPosition.x), (float) Mathf.CeilToInt(this.destinationPosition.y), this.popupContainer.localPosition.z);
      }
    }
  }

  public Vector3 GetPopupActiveCoordinates()
  {
    return GameManager.Instance.cameraMain.ScreenToWorldPoint(this.popupContainer.position);
  }

  public Transform GetPopupT()
  {
    return (UnityEngine.Object) this.GO_Popup != (UnityEngine.Object) null ? this.GO_Popup.transform : (Transform) null;
  }

  public void Resize()
  {
    if (!((UnityEngine.Object) this.canvasContainerScaler != (UnityEngine.Object) null))
      return;
    if ((double) Globals.Instance.scale < 1.0)
      this.canvasContainerScaler.matchWidthOrHeight = 1f;
    else
      this.canvasContainerScaler.matchWidthOrHeight = 0.0f;
  }

  public void StablishPopupPositionSize(Vector3 position, Vector3 scale)
  {
    this.absolutePosition = position;
    this.popupContainer.transform.localScale = scale;
    this.absolutePositionStablished = true;
    this.popupActive = false;
  }

  private IEnumerator ShowPopup()
  {
    PopupManager popupManager = this;
    popupManager.ClosePopup();
    popupManager.popupContainer.transform.localScale = new Vector3(1f, 1f, 1f);
    popupManager.theTFposition = !((UnityEngine.Object) popupManager.theTF != (UnityEngine.Object) null) ? Vector3.zero : popupManager.theTF.localPosition;
    if (TomeManager.Instance.IsActive())
      popupManager.fastPopup = true;
    if (!popupManager.fastPopup)
      yield return (object) Globals.Instance.WaitForSeconds(0.9f);
    else
      yield return (object) Globals.Instance.WaitForSeconds(0.15f);
    if (!(popupManager.position != "follow") || !(popupManager.position != "followdown") || !(popupManager.position != "followdown2") || !((UnityEngine.Object) popupManager.theTF == (UnityEngine.Object) null))
    {
      popupManager.followMouse = false;
      if (popupManager.position == "right" || popupManager.position == "followright")
      {
        Vector2 sizeDelta = popupManager.popupContainer.GetComponent<RectTransform>().sizeDelta;
        popupManager.adjustPosition = new Vector3(sizeDelta.x + 80f, popupManager.theTF.GetComponent<BoxCollider2D>().size.y, 0.0f);
        popupManager.destinationPosition = new Vector3((float) ((double) popupManager.theTF.position.x * 100.0 + (double) popupManager.adjustPosition.x * 0.800000011920929), (float) ((double) popupManager.theTF.position.y * 100.0 + (double) popupManager.adjustPosition.y * 6.0), 1f);
        if (popupManager.position == "followright")
          popupManager.followMouse = true;
      }
      else if (popupManager.position == "left" || popupManager.position == "followleft")
      {
        Vector2 sizeDelta = popupManager.popupContainer.GetComponent<RectTransform>().sizeDelta;
        popupManager.adjustPosition = new Vector3((float) (-(double) sizeDelta.x - 80.0), popupManager.theTF.GetComponent<BoxCollider2D>().size.y, 0.0f);
        popupManager.destinationPosition = new Vector3((float) ((double) popupManager.theTF.position.x * 100.0 + (double) popupManager.adjustPosition.x * 0.800000011920929), (float) ((double) popupManager.theTF.position.y * 100.0 + (double) popupManager.adjustPosition.y * 6.0), 1f);
        if (popupManager.position == "followleft")
          popupManager.followMouse = true;
      }
      else if (popupManager.position == "centerright")
      {
        popupManager.adjustPosition = new Vector3(popupManager.popupContainer.GetComponent<RectTransform>().sizeDelta.x, popupManager.theTF.GetComponent<BoxCollider2D>().size.y, 0.0f);
        popupManager.destinationPosition = new Vector3((float) ((double) popupManager.theTF.position.x * 100.0 + (double) popupManager.adjustPosition.x * 0.699999988079071), (float) ((double) popupManager.theTF.position.y * 100.0 - (double) popupManager.adjustPosition.y * 130.0 * 0.5), 1f);
      }
      else if (popupManager.position == "center")
      {
        popupManager.adjustPosition = new Vector3(0.0f, 30f, 0.0f);
        Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if ((UnityEngine.Object) popupManager.theTF != (UnityEngine.Object) null)
        {
          popupManager.destinationPosition = new Vector3(popupManager.theTF.position.x * 100f + popupManager.adjustPosition.x, popupManager.theTF.position.y * 100f + popupManager.adjustPosition.y, 1f);
          if ((double) popupManager.destinationPosition.x * 0.0099999997764825821 > (double) Screen.width * 0.004999999888241291)
          {
            float num = (float) ((double) popupManager.destinationPosition.x * 0.0099999997764825821 - (double) Screen.width * 0.004999999888241291);
            popupManager.destinationPosition -= new Vector3(num * 100f, 0.0f, 0.0f);
          }
          if ((double) popupManager.destinationPosition.x * 0.0099999997764825821 < (double) -Screen.width * 0.004999999888241291)
          {
            float num = (float) ((double) -Screen.width * 0.004999999888241291 - (double) popupManager.destinationPosition.x * 0.0099999997764825821);
            popupManager.destinationPosition += new Vector3(num * 100f, 0.0f, 0.0f);
          }
        }
        else
        {
          Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
          popupManager.destinationPosition = new Vector3(worldPoint.x * 100f, worldPoint.y * 100f, 1f);
        }
      }
      else if (popupManager.position == "follow")
      {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupManager.destinationPosition = new Vector3(worldPoint.x * 100f, (float) (((double) worldPoint.y + 0.20000000298023224) * 100.0), 1f);
        popupManager.followMouse = true;
      }
      else if (popupManager.position == "followdown")
      {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupManager.destinationPosition = new Vector3(worldPoint.x * 100f, (float) (((double) worldPoint.y - 1.0) * 100.0), 1f);
        popupManager.followMouse = true;
      }
      else if (popupManager.position == "followdown2")
      {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        popupManager.destinationPosition = new Vector3((float) (((double) worldPoint.x - 2.0) * 100.0), (float) (((double) worldPoint.y - 4.0) * 100.0), 1f);
        popupManager.followMouse = true;
      }
      popupManager.destinationPosition = new Vector3((float) Mathf.CeilToInt(popupManager.destinationPosition.x), (float) Mathf.CeilToInt(popupManager.destinationPosition.y), 1f);
      popupManager.GO_Popup.SetActive(true);
      popupManager.popupContainer.localPosition = new Vector3(1000f, 1000f, -10f);
      popupManager.coroutine = popupManager.StartCoroutine(popupManager.CalcSize());
    }
  }

  private IEnumerator CalcSize()
  {
    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    float num = (this.followSizeDelta = this.popupContainer.GetComponent<RectTransform>().sizeDelta).y * 0.5f;
    if (!this.absolutePositionStablished)
    {
      if (this.position == "right" || this.position == "left" || this.position == "followleft" || this.position == "followright")
        this.destinationPosition = new Vector3(this.destinationPosition.x, this.destinationPosition.y - num, this.destinationPosition.z);
      this.destinationPosition = this.RecalcPositionLimits(this.destinationPosition);
      if (this.position == "followdown")
        this.destinationPosition -= new Vector3(0.0f, (float) ((double) this.followSizeDelta.y * 0.5 - 0.20000000298023224), 0.0f);
    }
    else
    {
      this.destinationPosition = new Vector3(this.absolutePosition.x, this.absolutePosition.y - num, this.absolutePosition.z);
      this.absolutePositionStablished = false;
    }
    this.popupContainer.localPosition = this.destinationPosition - new Vector3(0.0f, 5f, 0.0f);
    this.popupActive = true;
  }

  private Vector3 RecalcPositionLimits(Vector3 destinationPosition)
  {
    if ((double) destinationPosition.x * 0.0099999997764825821 < (double) Globals.Instance.sizeW * -0.5)
      destinationPosition = new Vector3((float) ((double) Globals.Instance.sizeW * -0.5 * 100.0), destinationPosition.y, destinationPosition.z);
    else if ((double) destinationPosition.x * 0.0099999997764825821 > (double) Globals.Instance.sizeW * 0.5)
      destinationPosition = new Vector3((float) ((double) Globals.Instance.sizeW * 0.5 * 100.0), destinationPosition.y, destinationPosition.z);
    else if ((double) destinationPosition.y * 0.0099999997764825821 + (double) this.followSizeDelta.y * 0.0099999997764825821 > (double) Globals.Instance.sizeH * 0.44999998807907104)
      destinationPosition = new Vector3(destinationPosition.x, (float) ((double) Globals.Instance.sizeH * 0.44999998807907104 * 100.0) - this.followSizeDelta.y, destinationPosition.z);
    return destinationPosition;
  }

  public void ClosePopup()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    if (this.coroutine != null)
      this.StopCoroutine(this.coroutine);
    this.popupActive = false;
    this.absolutePositionStablished = false;
    if ((UnityEngine.Object) this.popUnlocked != (UnityEngine.Object) null)
      this.popUnlocked.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this.GO_Popup != (UnityEngine.Object) null))
      return;
    this.GO_Popup.SetActive(false);
  }

  public bool IsActive() => this.GO_Popup.gameObject.activeSelf;

  private void FollowRight()
  {
    this.position = "followright";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void FollowPop()
  {
    this.position = "follow";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void FollowPopDown()
  {
    this.position = "followdown";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void FollowPopDown2()
  {
    this.position = "followdown2";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void CenterPop()
  {
    this.position = "center";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void RightPop()
  {
    this.position = "right";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void LeftPop()
  {
    this.position = "left";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void Bottom()
  {
    this.position = "bottom";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void CenterRightPop()
  {
    this.position = "centerright";
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  private void ShowInPosition(string _position)
  {
    this.position = _position;
    this.coroutine = this.StartCoroutine(this.ShowPopup());
  }

  public void SetCard(
    Transform tf,
    CardData cardData,
    List<KeyNotesData> cardDataKeyNotes,
    string position = "right",
    bool fast = false)
  {
    if (CardScreenManager.Instance.IsActive())
      fast = true;
    else if ((UnityEngine.Object) MatchManager.Instance == (UnityEngine.Object) null)
      fast = true;
    this.fastPopup = fast;
    if (this.coroutine != null)
      this.StopCoroutine(this.coroutine);
    if (position == "right" && (UnityEngine.Object) tf != (UnityEngine.Object) null && (double) tf.transform.position.x > (double) Screen.width * 0.004999999888241291 - 6.0 * ((double) Screen.width / 1920.0))
      position = "left";
    if (!PlayerManager.Instance.IsCardUnlocked(cardData.Id))
      this.popUnlocked.gameObject.SetActive(true);
    else
      this.popUnlocked.gameObject.SetActive(false);
    bool flag = false;
    if (cardData.EnergyReductionToZeroPermanent || cardData.EnergyReductionToZeroTemporal || cardData.EnergyReductionPermanent > 0 || cardData.EnergyReductionTemporal > 0 || cardData.ExhaustCounter > 0)
      flag = true;
    if (this.lastPop == cardData.Id && !flag)
    {
      this.DrawPopup(tf);
      this.imageBg.color = this.colorCardtype;
      this.ShowInPosition(position);
    }
    else
    {
      this.popTrait.SetActive(false);
      this.popTown.SetActive(false);
      this.popPerk.SetActive(false);
      this.adjustCardX = 60;
      if (cardDataKeyNotes == null)
        return;
      int count = cardDataKeyNotes.Count;
      if (count <= 0 && cardData.CardType == Enums.CardType.None)
        return;
      this.DrawPopup(tf);
      this.imageBg.color = this.colorCardtype;
      if (cardData.CardType != Enums.CardType.None)
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append("<line-height=140%><voffset=-1><size=26><sprite name=cards></size></voffset>");
        stringBuilder1.Append("<size=21><color=#fc0><b>");
        stringBuilder1.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) cardData.CardType)));
        stringBuilder1.Append("</b></color>");
        for (int index = 0; index < cardData.CardTypeAux.Length; ++index)
        {
          if (cardData.CardTypeAux[index] != Enums.CardType.None)
          {
            stringBuilder1.Append(", ");
            stringBuilder1.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) cardData.CardTypeAux[index])));
          }
        }
        stringBuilder1.Append("</size></line-height>");
        if (cardData.CardType == Enums.CardType.Enchantment)
        {
          stringBuilder1.Append("<br><size=17><color=#AAAAAA>");
          stringBuilder1.Append(Texts.Instance.GetText("maximumEnchantments"));
          stringBuilder1.Append("</size></color>");
        }
        if (flag)
        {
          stringBuilder1.Append("<br><sprite name=energy>");
          StringBuilder stringBuilder2 = new StringBuilder();
          if (cardData.EnergyReductionToZeroPermanent)
          {
            stringBuilder2.Append(" <color=#FFF>");
            stringBuilder2.Append(string.Format(Texts.Instance.GetText("cardsCost"), (object) 0));
            stringBuilder2.Append("</color>");
          }
          else if (cardData.EnergyReductionToZeroTemporal)
          {
            stringBuilder2.Append(" <color=#FFF>");
            stringBuilder2.Append(string.Format(Texts.Instance.GetText("cardsCost"), (object) 0));
            stringBuilder2.Append("</color>");
            stringBuilder2.Append(" (");
            stringBuilder2.Append(Texts.Instance.GetText("cardsCostUntilDiscarded"));
            stringBuilder2.Append(")");
          }
          if (cardData.EnergyReductionPermanent > 0 || cardData.EnergyReductionTemporal > 0)
          {
            if (stringBuilder2.Length > 0)
              stringBuilder2.Append(" <voffset=1.5>|</voffset>");
            int num = cardData.EnergyReductionPermanent + cardData.EnergyReductionTemporal;
            stringBuilder2.Append(" <color=#FFF>");
            stringBuilder2.Append(string.Format(Texts.Instance.GetText("cardsCostReducedBy"), (object) num));
            stringBuilder2.Append("</color> ");
            if (cardData.EnergyReductionPermanent == 0)
            {
              stringBuilder2.Append("(");
              stringBuilder2.Append(Texts.Instance.GetText("cardsCostUntilDiscarded"));
              stringBuilder2.Append(")");
            }
            else if (cardData.EnergyReductionTemporal > 0)
            {
              stringBuilder2.Append("(");
              stringBuilder2.Append(cardData.EnergyReductionPermanent);
              stringBuilder2.Append(" + ");
              stringBuilder2.Append(cardData.EnergyReductionTemporal);
              stringBuilder2.Append(" ");
              stringBuilder2.Append(Texts.Instance.GetText("cardsCostUntilDiscarded"));
              stringBuilder2.Append(") ");
            }
          }
          if (cardData.ExhaustCounter > 0)
          {
            if (stringBuilder2.Length > 0)
              stringBuilder2.Append(" <voffset=1.5>|</voffset>");
            stringBuilder2.Append(" <color=#EC75D3>");
            stringBuilder2.Append(Texts.Instance.GetText("exhaustion"));
            stringBuilder2.Append(" +");
            stringBuilder2.Append(cardData.ExhaustCounter);
            stringBuilder2.Append("</color>");
          }
          stringBuilder1.Append(stringBuilder2.ToString());
        }
        if (cardData.Sku != "")
        {
          stringBuilder1.Append("<br><size=16><color=#66CCBB>");
          stringBuilder1.Append(SteamManager.Instance.GetDLCName(cardData.Sku));
          stringBuilder1.Append(" ");
          stringBuilder1.Append(Texts.Instance.GetText("dlcAcronymForCharSelection"));
          stringBuilder1.Append("</color></size>");
        }
        this.TextAdjust(stringBuilder1.ToString());
        this.pop.SetActive(true);
      }
      else
        this.pop.SetActive(false);
      this.CleanKeyNotes();
      if (count > 0)
      {
        for (int index = 0; index < count; ++index)
        {
          if ((UnityEngine.Object) cardDataKeyNotes[index] != (UnityEngine.Object) null)
          {
            this.KeyNotesActive.Add(cardDataKeyNotes[index].Id);
            this.KeyNotesGO[cardDataKeyNotes[index].Id].SetActive(true);
          }
        }
      }
      else
      {
        float x = 300f;
        this.adjustCardX = -30;
        RectTransform component = this.popText.GetComponent<RectTransform>();
        component.sizeDelta = new Vector2(x, component.sizeDelta.y);
        this.popText.ForceMeshUpdate(true);
      }
      this.ShowInPosition(position);
      if (flag)
        this.lastPop = (string) null;
      else
        this.lastPop = cardData.Id;
    }
  }

  public void ShowKeyNote(Transform tf, string keyNote, string position = "right", bool fast = false)
  {
    this.fastPopup = fast;
    if (this.coroutine != null)
      this.StopCoroutine(this.coroutine);
    this.popTrait.SetActive(false);
    this.popTown.SetActive(false);
    this.popPerk.SetActive(false);
    this.pop.SetActive(false);
    this.CleanKeyNotes();
    this.KeyNotesActive.Add(keyNote);
    this.KeyNotesGO[keyNote].SetActive(true);
    this.DrawPopup(tf);
    this.ShowInPosition(position);
    this.lastPop = keyNote;
  }

  public void SetTrait(TraitData td, bool includeDescription = true)
  {
    if ((UnityEngine.Object) td == (UnityEngine.Object) null)
      return;
    this.fastPopup = true;
    this.DrawPopup();
    this.pop.SetActive(false);
    this.popPerk.SetActive(false);
    this.popTown.SetActive(false);
    if (includeDescription)
    {
      string[] strArray = new string[4]
      {
        td.TraitName,
        !((UnityEngine.Object) td.TraitCard == (UnityEngine.Object) null) ? string.Format(Texts.Instance.GetText("traitAddCard"), (object) td.TraitCard.CardName) : td.Description,
        "D4AC5B",
        null
      };
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(" <voffset=0><size=21><sprite name=experience></size></voffset> ");
      stringBuilder.Append(string.Format(this.popupText, (object[]) strArray));
      this.popTraitText.text = stringBuilder.ToString();
      this.popTrait.SetActive(true);
    }
    this.CleanKeyNotes();
    int count = td.KeyNotes.Count;
    if (count > 0)
    {
      for (int index = 0; index < count; ++index)
      {
        if ((UnityEngine.Object) td.KeyNotes[index] != (UnityEngine.Object) null)
        {
          this.KeyNotesActive.Add(td.KeyNotes[index].Id);
          this.KeyNotesGO[td.KeyNotes[index].Id].SetActive(true);
        }
      }
    }
    if (this.position == "followdown")
      this.FollowPopDown();
    else
      this.FollowPop();
    this.lastPop = td.TraitName;
  }

  public void SetPerk(string title, string text, string keynote = "")
  {
    this.fastPopup = true;
    this.DrawPopup();
    this.pop.SetActive(false);
    this.popTown.SetActive(false);
    this.popTrait.SetActive(false);
    string[] strArray = new string[4]
    {
      text,
      "",
      "D4AC5B",
      ""
    };
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(string.Format(this.popupText, (object[]) strArray));
    this.popPerkText.text = stringBuilder.ToString();
    this.popPerk.SetActive(true);
    this.CleanKeyNotes();
    if (keynote != "")
    {
      keynote = keynote.ToLower();
      this.KeyNotesActive.Add(keynote);
      if (this.KeyNotesGO.ContainsKey(keynote))
        this.KeyNotesGO[keynote].SetActive(true);
    }
    this.FollowPop();
    this.lastPop = title;
  }

  public void SetTown(string idTitle, string idDescription, bool showDisableText)
  {
    if (this.lastPop == idTitle)
    {
      this.DrawPopup();
      this.FollowPop();
    }
    else
    {
      this.fastPopup = true;
      this.DrawPopup();
      string[] strArray = new string[4]
      {
        Texts.Instance.GetText(idTitle),
        Texts.Instance.GetText(idDescription),
        "FFC88F",
        ""
      };
      StringBuilder stringBuilder = new StringBuilder();
      string str = "";
      switch (idTitle)
      {
        case "craftCards":
          str = "cards";
          break;
        case "upgradeCards":
          str = "nodeUpgrade";
          break;
        case "removeCards":
          str = "nodeHeal";
          break;
        case "divinationCards":
          str = "nodeDivination";
          break;
        case "buyItems":
          str = "nodeShop";
          break;
      }
      stringBuilder.Append("<line-height=20><br></line-height>");
      stringBuilder.Append(" <voffset=-3><size=30><sprite name=");
      stringBuilder.Append(str);
      stringBuilder.Append("></size></voffset> ");
      stringBuilder.Append(string.Format(this.popupText, (object[]) strArray));
      if (showDisableText)
      {
        stringBuilder.Append("<br>");
        stringBuilder.Append(Texts.Instance.GetText("disabledInSingularity"));
      }
      this.pop.SetActive(false);
      this.popTrait.SetActive(false);
      this.popPerk.SetActive(false);
      this.popTownText.fontSize = 22f;
      this.popTownText.text = stringBuilder.ToString();
      this.popTown.SetActive(true);
      this.CleanKeyNotes();
      this.FollowPop();
      this.lastPop = idTitle;
    }
  }

  private void AddReplacement(
    Dictionary<string, string> replacements,
    string placeholder,
    int value)
  {
    replacements[placeholder] = value.ToString();
  }

  private int CalculateChargesMultiplier(float multiplier, int charges)
  {
    return Functions.FuncRoundToInt(multiplier * (float) charges);
  }

  private int ParseCharges(string charges) => int.Parse(charges);

  private int CalculateChargesAux(int charges, float chargesAuxNeedForOne)
  {
    return Mathf.FloorToInt((float) charges / chargesAuxNeedForOne);
  }

  private int CalculateAuraDamage(
    int charges,
    float damagePerStack,
    float increasedDirectDamageChargesMultiplierNeededForOne = 0.0f)
  {
    if ((double) increasedDirectDamageChargesMultiplierNeededForOne == 0.0)
      return Functions.FuncRoundToInt((float) charges * damagePerStack);
    float num = 1f / increasedDirectDamageChargesMultiplierNeededForOne;
    return (double) damagePerStack >= 0.0 ? Mathf.FloorToInt(num * (float) charges * damagePerStack) : -1 * Mathf.FloorToInt(Mathf.Abs(num * (float) charges * damagePerStack));
  }

  private int CalculateResistAux(int charges, float resistModifiedPercentage)
  {
    return Functions.FuncRoundToInt((float) charges * resistModifiedPercentage);
  }

  private void UpdateReplacements(
    Dictionary<string, string> replacements,
    int charges,
    int chargesSecondary,
    AuraCurseData acData,
    Character theChar)
  {
    float num1 = 1f / (float) acData.CharacterStatChargesMultiplierNeededForOne;
    int num2 = Mathf.FloorToInt(Mathf.Abs(0.0714285746f * (float) charges));
    this.AddReplacement(replacements, "<ChargesValueBy14>", num2);
    this.AddReplacement(replacements, "<ChargesCurrent>", charges);
    this.AddReplacement(replacements, "<ChargesCurrentHalf>", Functions.FuncRoundToInt((float) charges * 0.5f));
    this.AddReplacement(replacements, "<ChargesMultiplier>", this.CalculateChargesMultiplier((float) acData.ChargesMultiplierDescription, charges));
    if (chargesSecondary != -1000)
      this.AddReplacement(replacements, "<ChargesMultiplier_sec>", this.CalculateChargesMultiplier((float) acData.ChargesMultiplierDescription, chargesSecondary));
    this.AddReplacement(replacements, "<ChargesMultiplierHalf>", this.CalculateChargesMultiplier((float) acData.ChargesMultiplierDescription, Functions.FuncRoundToInt((float) charges * 0.5f)));
    this.AddReplacement(replacements, "<ChargesAux1>", this.CalculateChargesAux(charges, (float) acData.ChargesAuxNeedForOne1));
    if (chargesSecondary != -1000)
      this.AddReplacement(replacements, "<ChargesAux1_sec>", this.CalculateChargesAux(chargesSecondary, (float) acData.ChargesAuxNeedForOne1));
    this.AddReplacement(replacements, "<ChargesAux2>", this.CalculateChargesAux(charges, (float) acData.ChargesAuxNeedForOne2));
    if (chargesSecondary != -1000)
      this.AddReplacement(replacements, "<ChargesAux2_sec>", this.CalculateChargesAux(chargesSecondary, (float) acData.ChargesAuxNeedForOne2));
    this.AddReplacement(replacements, "<AuraDamageIncreasedPerStack>", this.CalculateAuraDamage(charges, acData.AuraDamageIncreasedPerStack));
    this.AddReplacement(replacements, "<AuraDamageIncreasedPerStack2>", this.CalculateAuraDamage(charges, acData.AuraDamageIncreasedPerStack2));
    this.AddReplacement(replacements, "<AuraDamageIncreasedPercentPerStack>", this.CalculateAuraDamage(charges, acData.AuraDamageIncreasedPercentPerStack));
    this.AddReplacement(replacements, "<IncreasedDirectDamageReceivedPerStack>", this.CalculateAuraDamage(charges, acData.IncreasedDirectDamageReceivedPerStack, (float) acData.IncreasedDirectDamageChargesMultiplierNeededForOne));
    this.AddReplacement(replacements, "<IncreasedDirectDamageReceivedPerStack2>", this.CalculateAuraDamage(charges, acData.IncreasedDirectDamageReceivedPerStack2, (float) acData.IncreasedDirectDamageChargesMultiplierNeededForOne));
    int auraDamage1 = this.CalculateAuraDamage(charges, acData.AuraDamageIncreasedPercentPerStack);
    this.AddReplacement(replacements, "<DamageAux1>", Mathf.Abs(auraDamage1));
    if (chargesSecondary != -1000)
    {
      int auraDamage2 = this.CalculateAuraDamage(chargesSecondary, acData.AuraDamageIncreasedPercentPerStack);
      this.AddReplacement(replacements, "<DamageAux1_sec>", Mathf.Abs(auraDamage2));
    }
    int auraDamage3 = this.CalculateAuraDamage(charges, acData.AuraDamageIncreasedPercentPerStack2);
    this.AddReplacement(replacements, "<DamageAux2>", Mathf.Abs(auraDamage3));
    if (chargesSecondary != -1000)
    {
      int auraDamage4 = this.CalculateAuraDamage(chargesSecondary, acData.AuraDamageIncreasedPercentPerStack2);
      this.AddReplacement(replacements, "<DamageAux2_sec>", Mathf.Abs(auraDamage4));
    }
    this.AddReplacement(replacements, "<ResistAux1>", Math.Abs(this.CalculateResistAux(charges, acData.ResistModifiedPercentagePerStack)));
    this.AddReplacement(replacements, "<ResistAux2>", Math.Abs(this.CalculateResistAux(charges, acData.ResistModifiedPercentagePerStack2)));
    int num3 = acData.MaxCharges;
    if (MadnessManager.Instance.IsMadnessTraitActive("restrictedpower") || AtOManager.Instance.IsChallengeTraitActive("restrictedpower"))
      num3 = acData.MaxMadnessCharges;
    this.AddReplacement(replacements, "<MaxCharges>", num3);
    this.AddReplacement(replacements, "<HealReceivedPercent>", acData.HealReceivedPercent);
    this.AddReplacement(replacements, "<CharacterStatModifiedValue>", acData.CharacterStatModifiedValue);
    int num4 = Mathf.FloorToInt((float) (charges * acData.CharacterStatModifiedValuePerStack));
    string str1 = num4.ToString();
    if (acData.CharacterStatModifiedValuePerStack > 0)
      str1 = "+" + str1;
    if (str1 == "+0")
      str1 = "0";
    replacements["<CharacterStatModifiedPerStack>"] = str1;
    string str2;
    if (acData.CharacterStatModifiedValuePerStack < 0)
    {
      num4 = Mathf.FloorToInt(Mathf.Abs(num1 * (float) charges * (float) acData.CharacterStatModifiedValuePerStack));
      str2 = "-" + num4.ToString();
    }
    else
    {
      num4 = Mathf.FloorToInt(num1 * (float) charges * (float) acData.CharacterStatModifiedValuePerStack);
      str2 = "+" + num4.ToString();
    }
    replacements["<CharacterStatModifiedValuePerStackTotal>"] = str2;
    this.AddReplacement(replacements, "<DamageWhenConsumed>", acData.DamageWhenConsumed);
    this.AddReplacement(replacements, "<DamageSidesWhenConsumed>", acData.DamageSidesWhenConsumed);
    this.AddReplacement(replacements, "<HealAttackerConsumeCharges>", acData.HealAttackerConsumeCharges);
    int num5 = acData.ResistModifiedValue + this.CalculateResistAux(charges, acData.ResistModifiedPercentagePerStack);
    this.AddReplacement(replacements, "<Resistance1>", num5);
    int auraDamage5 = this.CalculateAuraDamage(charges, acData.DamageWhenConsumedPerCharge);
    this.AddReplacement(replacements, "<DamageWhenConsumedPerCharge>", auraDamage5);
    this.AddReplacement(replacements, "<ExplodeAtStacks>", acData.ExplodeAtStacks);
    this.AddReplacement(replacements, "<HealReceivedPercentPerStack>", this.CalculateResistAux(charges, (float) acData.HealReceivedPercentPerStack));
    this.AddReplacement(replacements, "<HealDonePercentPerStack>", this.CalculateResistAux(charges, (float) acData.HealDonePercentPerStack));
    if (!(acData.Id == "zeal"))
      return;
    int num6 = 0;
    if (theChar != null)
    {
      num6 = Functions.FuncRoundToInt((float) theChar.GetAuraCharges("burn") * acData.AuraDamageIncreasedPercentPerStack);
      if (theChar.HaveTrait("righteousflame"))
        num6 += Functions.FuncRoundToInt((float) charges * acData.AuraDamageIncreasedPercentPerStack2);
    }
    this.AddReplacement(replacements, "<DamageZeal>", num6);
  }

  private string ApplyReplacements(string textDescription, Dictionary<string, string> replacements)
  {
    StringBuilder stringBuilder = new StringBuilder(textDescription);
    foreach (KeyValuePair<string, string> replacement in replacements)
      stringBuilder.Replace(replacement.Key, replacement.Value);
    return stringBuilder.ToString();
  }

  public void SetAuraCurse(
    Transform tf,
    string acDataId,
    string charges,
    bool fast = false,
    string charId = "")
  {
    if (EventSystem.current.IsPointerOverGameObject())
      return;
    this.fastPopup = fast;
    if (charges == "" || acDataId.IsNullOrEmpty())
      return;
    AuraCurseData acData = Globals.Instance.GetAuraCurseData(acDataId);
    AuraCurseData auraCurseData = (AuraCurseData) null;
    if ((bool) (UnityEngine.Object) MatchManager.Instance)
    {
      Character characterById = MatchManager.Instance.GetCharacterById(charId);
      if (characterById != null)
      {
        auraCurseData = Globals.Instance.GetAuraCurseData(acDataId);
        acData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", acData.Id, characterById, characterById);
      }
    }
    if (this.lastPop == acData.ACName + charges && (UnityEngine.Object) this.lastTransform == (UnityEngine.Object) tf)
    {
      this.DrawPopup(tf);
      if (acData.IsAura)
        this.imageBg.color = this.colorGood;
      else
        this.imageBg.color = this.colorBad;
      this.CenterPop();
    }
    else
    {
      this.DrawPopup(tf);
      this.CleanKeyNotes();
      string[] strArray = new string[4];
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("<b>");
      stringBuilder1.Append(acData.ACName);
      stringBuilder1.Append("</b>");
      strArray[0] = stringBuilder1.ToString();
      int chargesSecondary = -1000;
      string str1 = acData.Description;
      Character theChar = (Character) null;
      if ((bool) (UnityEngine.Object) MatchManager.Instance)
      {
        theChar = MatchManager.Instance.GetCharacterById(charId);
        if (theChar != null)
        {
          stringBuilder1.Clear();
          if (acData.Id == "bleed" && acData.ConsumedAtTurn)
            str1 = Texts.Instance.GetText("bleedendturn_description", "auracurse");
          if (acData.Id == "bless")
          {
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "MainperkBless1a"))
              str1 = Texts.Instance.GetText("blessNoHeal_description", "auracurse");
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "MainperkBless1b"))
              str1 = str1 + "<br3>" + Texts.Instance.GetText("healingDonePercentPerStack");
            if (AtOManager.Instance.TeamHavePerk("MainperkBless1c") && theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("holyResistance");
          }
          else if (acData.Id == "burn")
          {
            if (!theChar.IsHero)
            {
              if (AtOManager.Instance.TeamHavePerk("mainperkBurn2c"))
                str1 = Texts.Instance.GetText("burnColdDamage_description", "auracurse");
              if (theChar.GetAuraCurseTotal(true, true) <= 2)
                str1 = str1 + "<br3>" + Texts.Instance.GetText("burnDoubleDamage_description", "auracurse");
            }
            else if (theChar.HaveTrait("righteousflame"))
            {
              int num = Functions.FuncRoundToInt((float) int.Parse(charges) * acData.HealWhenConsumedPerCharge);
              str1 = Texts.Instance.GetText("regeneration_description", "auracurse").Replace("<ChargesMultiplier>", num.ToString());
            }
          }
          else if (acData.Id == "chill")
          {
            if (AtOManager.Instance.TeamHavePerk("MainperkChill2b") && !theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("bluntResistanceNegative").Replace("ResistAux1", "ResistAux2");
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "MainperkChill2d"))
            {
              int num = Mathf.FloorToInt(0.0714285746f * (float) int.Parse(charges));
              if (num > 0)
                str1 = str1 + "<br3>" + string.Format(Texts.Instance.GetText("chillReinforce_description", "auracurse"), (object) num);
            }
          }
          else if (acData.Id == "crack")
          {
            if (!theChar.IsHero)
            {
              if (AtOManager.Instance.TeamHavePerk("mainperkcrack2b"))
              {
                int num = Functions.FuncRoundToInt(float.Parse(charges) * 0.5f);
                if (num > 0)
                  str1 = str1 + "<br3>" + string.Format(Texts.Instance.GetText("crackBlock_description", "auracurse"), (object) num);
              }
              if (AtOManager.Instance.TeamHavePerk("mainperkcrack2c"))
              {
                int num = Mathf.FloorToInt(0.0714285746f * (float) int.Parse(charges));
                if (num > 0)
                  str1 = str1 + "<br3>" + string.Format(Texts.Instance.GetText("crackVulnerable_description", "auracurse"), (object) num);
              }
            }
          }
          else if (acData.Id == "dark")
          {
            if (AtOManager.Instance.TeamHavePerk("mainperkDark2d") && !theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("darkEndOfTurn_description", "auracurse");
          }
          else if (acData.Id == "decay")
          {
            if (AtOManager.Instance.TeamHavePerk("mainperkDecay1c") && !theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("shadowResistanceNegative");
          }
          else if (acData.Id == "fortify")
          {
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkFortify1a"))
              str1 = str1 + "<br3>" + Texts.Instance.GetText("bluntAndFireDamage_description", "auracurse");
            if (AtOManager.Instance.TeamHavePerk("mainperkFortify1c"))
              str1 = str1 + "<br3>" + string.Format(Texts.Instance.GetText("lossesAllChargesEndRoundGainBlock"), (object) (int.Parse(charges) * 10));
          }
          else if (acData.Id == "fury" && AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkfury1b"))
            str1 = Texts.Instance.GetText("furynobleed_description", "auracurse");
          else if (acData.Id == "mark")
          {
            if (!theChar.IsHero)
            {
              if (AtOManager.Instance.TeamHavePerk("mainperkmark1c"))
                str1 = str1 + "<br3>" + Texts.Instance.GetText("markAlsoSlashingResistance_description", "auracurse");
              if (AtOManager.Instance.TeamHavePerk("mainperkmark1b"))
                str1 = str1 + "<br3>" + Texts.Instance.GetText("notLoseChargesPreventingStealth");
            }
          }
          else if (acData.Id == "regeneration")
          {
            if (theChar.IsHero)
            {
              if (AtOManager.Instance.TeamHavePerk("mainperkRegeneration1b"))
                str1 = str1 + "<br3>" + Texts.Instance.GetText("healReceivedPercentPerStack");
              if (AtOManager.Instance.TeamHavePerk("mainperkRegeneration1c"))
                str1 = str1 + "<br3>" + Texts.Instance.GetText("shadowResistance");
              if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkRegeneration1a"))
                str1 = str1 + "<br3>" + Texts.Instance.GetText("regenerationSides_description", "auracurse");
            }
          }
          else if (acData.Id == "scourge")
          {
            if (!theChar.IsHero)
            {
              if (AtOManager.Instance.TeamHaveTrait("unholyblight"))
                str1 = str1.Replace("dark,50", "dark,100");
              if (AtOManager.Instance.TeamHaveTrait("auraofdespair"))
                str1 = str1 + "<br3>" + Texts.Instance.GetText("allResistancesNegative");
            }
          }
          else if (acData.Id == "shackle")
          {
            if (!theChar.IsHero && AtOManager.Instance.TeamHaveTrait("webweaver"))
              str1 = Texts.Instance.GetText("shackleWebweaver_description", "auracurse");
          }
          else if (acData.Id == "sharp")
          {
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkSharp1a"))
              str1 = Texts.Instance.GetText("sharpnopierce_description", "auracurse");
            else if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkSharp1b"))
              str1 = Texts.Instance.GetText("sharpnoslash_description", "auracurse");
            else if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkSharp1d"))
              str1 = Texts.Instance.GetText("sharpShadow_description", "auracurse");
          }
          else if (acData.Id == "spark")
          {
            if (AtOManager.Instance.TeamHavePerk("mainperkSpark2b") && !theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("sparkSideCharges_description", "auracurse");
            if (AtOManager.Instance.TeamHavePerk("mainperkSpark2c") && !theChar.IsHero)
            {
              int num = Mathf.FloorToInt(0.0714285746f * (float) int.Parse(charges));
              if (num > 0)
                str1 = str1 + "<br3>" + string.Format(Texts.Instance.GetText("sparkSlow_description", "auracurse"), (object) num);
            }
            if (AtOManager.Instance.TeamHaveTrait("voltaicarc") && !theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("sparkVoltaicTrait_description", "auracurse");
          }
          else if (acData.Id == "stanzai")
          {
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkStanza0b"))
              str1 = Texts.Instance.GetText("stanzaiInspire_description", "auracurse");
          }
          else if (acData.Id == "stanzaii")
          {
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkStanza0c"))
              str1 = Texts.Instance.GetText("stanzaiiNo3_description", "auracurse");
          }
          else if (acData.Id == "stealth")
          {
            if (AtOManager.Instance.TeamHavePerk("mainperkStealth1c") && theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("allResistances");
          }
          else if (acData.Id == "taunt")
          {
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkTaunt1b"))
              str1 = str1 + "<br3>" + Texts.Instance.GetText("reinforce_description", "auracurse");
            else if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkTaunt1c"))
              str1 = str1 + "<br3>" + Texts.Instance.GetText("insulate_description", "auracurse");
            else if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkTaunt1d"))
              str1 = str1 + "<br3>" + Texts.Instance.GetText("courage_description", "auracurse");
          }
          else if (acData.Id == "thorns")
          {
            if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkthorns1b"))
              str1 = Texts.Instance.GetText("thornsHoly_description", "auracurse");
            else if (AtOManager.Instance.CharacterHavePerk(theChar.SubclassName, "mainperkthorns1c"))
              str1 = Texts.Instance.GetText("thornspoison_description", "auracurse");
          }
          else if (acData.Id == "vitality")
          {
            if (AtOManager.Instance.TeamHavePerk("mainperkVitality1c") && theChar.IsHero)
              str1 = str1 + "<br3>" + Texts.Instance.GetText("mindResistance");
          }
          else if (acData.Id == "vulnerable")
          {
            if (AtOManager.Instance.TeamHavePerk("mainperkvulnerable0c") && !theChar.IsHero)
              str1 = Texts.Instance.GetText("reinforce_description", "auracurse").Replace("+", "") + "<br3>" + Texts.Instance.GetText("maximumCharges");
          }
          else if (acData.Id == "wet" && !theChar.IsHero)
          {
            if (AtOManager.Instance.TeamHavePerk("mainperkwet1a"))
              str1 = str1 + "<br3>" + Texts.Instance.GetText("wetAlsoColdDamage_description", "auracurse");
            if (AtOManager.Instance.TeamHavePerk("mainperkwet1b"))
              str1 = str1 + "<br3>" + Texts.Instance.GetText("wetAlsoLightningResistance_description", "auracurse");
          }
          if (theChar.IsHero && (acData.Id == "stanzai" || acData.Id == "stanzaii" || acData.Id == "stanzaiii") && AtOManager.Instance.TeamHavePerk("mainperkStanza0a"))
          {
            string pattern1 = "<y>(\\+\\d+)</y>";
            Match match = Regex.Match(str1, pattern1);
            if (match.Success)
            {
              string pattern2 = "<min>(.*?)</min>";
              if (Regex.Match(str1, pattern2).Success)
              {
                string str2 = match.Groups[1].Value;
                str1 = Regex.Replace(str1, pattern2, Texts.Instance.GetText("allDamage"));
              }
            }
          }
          stringBuilder1.Append(str1);
          if (auraCurseData.Removable && !acData.Removable && auraCurseData.Preventable && !acData.Preventable)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("cannotBeDispelledOrPrevented"));
          }
          else
          {
            if (auraCurseData.Removable && !acData.Removable)
            {
              stringBuilder1.Append("<br3>");
              if (auraCurseData.IsAura)
                stringBuilder1.Append(Texts.Instance.GetText("cannotBePurged"));
              else
                stringBuilder1.Append(Texts.Instance.GetText("cannotBeDispelled"));
            }
            if (auraCurseData.Preventable && !acData.Preventable)
            {
              stringBuilder1.Append("<br3>");
              stringBuilder1.Append(Texts.Instance.GetText("cannotBePrevented"));
            }
          }
          if (auraCurseData.MaxCharges == -1 && acData.MaxCharges > 0)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("maximumCharges"));
          }
          if (!auraCurseData.GainCharges && acData.GainCharges)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("canStackCharges"));
          }
          if (!auraCurseData.ConsumeAll && acData.ConsumeAll)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("lossesAllChargesEndOfTurn"));
          }
          if (auraCurseData.ConsumedAtTurn && !acData.ConsumedAtTurn || acData.ConsumedAtTurn && acData.AuraConsumed == 0 && auraCurseData.AuraConsumed > 0)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("notLoseChargesEndTurn"));
          }
          if (auraCurseData.ConsumedAtTurnBegin && !acData.ConsumedAtTurnBegin || acData.ConsumedAtTurnBegin && acData.AuraConsumed == 0 && auraCurseData.AuraConsumed > 0)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("notLoseChargesStartTurn"));
          }
          if (auraCurseData.CharacterStatModified != Enums.CharacterStat.Hp && acData.CharacterStatModified == Enums.CharacterStat.Hp && acData.CharacterStatModifiedValuePerStack != 0)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("maxHPModified"));
          }
          if (auraCurseData.CharacterStatModified != Enums.CharacterStat.Speed && acData.CharacterStatModified == Enums.CharacterStat.Speed && acData.CharacterStatModifiedValuePerStack != 0)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(Texts.Instance.GetText("speedModified"));
          }
          if (acData.AuraConsumed > 0 && auraCurseData.AuraConsumed != acData.AuraConsumed)
          {
            stringBuilder1.Append("<br3>");
            stringBuilder1.Append(string.Format(Texts.Instance.GetText("chargesReducedByEndTurn"), (object) acData.AuraConsumed));
          }
          str1 = stringBuilder1.ToString();
          Match match1 = new Regex("_([^>]*)>").Match(str1);
          if (match1.Success)
          {
            string ACName = match1.Groups[1].Value;
            chargesSecondary = theChar.GetAuraCharges(ACName);
            StringBuilder stringBuilder2 = new StringBuilder();
            StringBuilder stringBuilder3 = new StringBuilder();
            stringBuilder2.Append("_");
            stringBuilder3.Append("_");
            stringBuilder2.Append(ACName);
            stringBuilder3.Append("sec");
            stringBuilder2.Append(">");
            stringBuilder3.Append(">");
            str1 = str1.Replace(stringBuilder2.ToString(), stringBuilder3.ToString());
          }
        }
      }
      int charges1 = this.ParseCharges(charges);
      Dictionary<string, string> replacements = new Dictionary<string, string>();
      this.UpdateReplacements(replacements, charges1, chargesSecondary, acData, theChar);
      string str3 = this.ApplyReplacements(str1, replacements);
      (string str4, int auracurseCharges, string str5) = this.GetAuraAndChargesPatter(str3);
      if (!str5.IsNullOrEmpty() && !str4.IsNullOrEmpty() && auracurseCharges > 0)
      {
        int auraCharges = theChar.GetAuraCharges(str4);
        if (auraCharges > 0)
          auraCharges = Functions.FuncRoundToInt((float) ((double) auraCharges * (double) auracurseCharges / 100.0));
        str3 = str3.Replace(str5, auraCharges.ToString());
      }
      string str6 = this.ReplaceGeneralTags(str3);
      strArray[1] = str6;
      strArray[2] = "#FFF";
      strArray[3] = "";
      if ((UnityEngine.Object) acData != (UnityEngine.Object) null)
      {
        if (acData.IsAura)
        {
          strArray[2] = "AADDFF";
          this.imageBg.color = this.colorGood;
        }
        else
        {
          strArray[2] = "FF8181";
          this.imageBg.color = this.colorBad;
        }
        if (!acData.GainCharges)
          strArray[3] = "<space=.5em><size=18><color=#AAAAAA>(" + Texts.Instance.GetText("notStackPlain") + ")</color></size>";
      }
      stringBuilder1.Clear();
      stringBuilder1.Append("<voffset=-1><size=26><sprite name=");
      stringBuilder1.Append(acData.Id.ToLower());
      stringBuilder1.Append("></size></voffset>");
      stringBuilder1.Append(string.Format(this.popupText, (object[]) strArray));
      this.TextAdjust(stringBuilder1.ToString());
      this.pop.SetActive(true);
      this.popTrait.SetActive(false);
      this.popTown.SetActive(false);
      this.popPerk.SetActive(false);
      this.CenterPop();
      this.lastPop = acData.ACName + charges;
      this.lastTransform = tf;
    }
  }

  private (string auracurse, int auracurseCharges, string auracurseString) GetAuraAndChargesPatter(
    string cadena)
  {
    string pattern = "<aura:(\\w+),\\s*(\\d+)>";
    Match match = Regex.Match(cadena, pattern);
    if (!match.Success)
      return ((string) null, 0, (string) null);
    string str1 = match.Groups[1].Value;
    int num1 = int.Parse(match.Groups[2].Value);
    string str2 = match.Value;
    int num2 = num1;
    string str3 = str2;
    return (str1, num2, str3);
  }

  private string ReplaceGeneralTags(string textDescription)
  {
    textDescription = Functions.FormatString(textDescription);
    return textDescription;
  }

  public void SetText(
    string theText,
    bool fast = false,
    string position = "",
    bool alwaysCenter = false,
    string keynote = "")
  {
    this.popTrait.SetActive(false);
    this.popTown.SetActive(false);
    this.popPerk.SetActive(false);
    this.fastPopup = fast;
    if (this.lastPop == theText)
    {
      this.DrawPopup();
      switch (position)
      {
        case "followdown":
          this.FollowPopDown();
          break;
        case "centerpop":
          this.CenterPop();
          break;
        case "followdown2":
          this.FollowPopDown2();
          break;
        default:
          this.FollowPop();
          break;
      }
    }
    else
    {
      this.DrawPopup();
      this.CleanKeyNotes();
      if (keynote != "")
      {
        this.KeyNotesActive.Add(keynote);
        this.KeyNotesGO[keynote].SetActive(true);
      }
      this.TextAdjust(theText, true, alwaysCenter);
      switch (position)
      {
        case "followdown":
          this.FollowPopDown();
          break;
        case "centerpop":
          this.CenterPop();
          break;
        case "followdown2":
          this.FollowPopDown2();
          break;
        default:
          this.FollowPop();
          break;
      }
      this.lastPop = theText;
    }
  }

  public void SetBackgroundColor(string _color)
  {
    this.imageBg.color = Functions.HexToColor(_color);
  }

  private void TextAdjust(string theText, bool adjust = false, bool alwaysCenter = false)
  {
    if (!this.GO_Popup.activeSelf)
      this.GO_Popup.SetActive(true);
    if (!this.pop.activeSelf)
      this.pop.SetActive(true);
    this.popText.horizontalAlignment = HorizontalAlignmentOptions.Left;
    this.popText.fontSize = 21f;
    this.popText.paragraphSpacing = 30f;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<line-height=30><voffset=3>");
    stringBuilder.Append(theText);
    this.popText.text = stringBuilder.ToString();
    this.textRect.sizeDelta = new Vector2(500f, this.textRect.sizeDelta.y);
    this.popText.ForceMeshUpdate(true);
    if (adjust && alwaysCenter)
    {
      this.textRect.sizeDelta = new Vector2(this.popText.bounds.max.x + 50f, this.textRect.sizeDelta.y);
      this.popText.horizontalAlignment = HorizontalAlignmentOptions.Center;
      this.popText.ForceMeshUpdate(true);
    }
    if (!this.GO_Popup.activeSelf)
      return;
    this.GO_Popup.SetActive(false);
  }

  public void SetConsoleText(Transform tf, string theText)
  {
    if (this.lastPop == theText)
    {
      this.DrawPopup(tf);
      this.CenterPop();
    }
    else
    {
      this.DrawPopup(tf);
      this.CleanKeyNotes();
      this.popText.text = MatchManager.Instance.console.GetText(theText);
      this.pop.SetActive(true);
      this.CenterPop();
      this.lastPop = theText;
    }
  }

  public void DrawPopup(Transform tf = null)
  {
    this.theTF = tf;
    this.adjustPosition = (Vector3) this.popupContainer.GetComponent<RectTransform>().sizeDelta;
    this.imageBg.color = this.colorPlain;
  }

  public void RefreshKeyNotes() => this.StartCoroutine(this.RefreshKeyNotesCo());

  private IEnumerator RefreshKeyNotesCo()
  {
    if (this.KeyNotesGO.Count > 0)
    {
      this.CleanKeyNotes();
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.KeyNotesGO)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.KeyNotesGO[keyValuePair.Key]);
      foreach (Transform transform in this.popupContainer)
      {
        if (!this.initialPopupGOs.Contains(transform.gameObject.name))
          UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject);
      }
      while (this.popupContainer.childCount > this.initialPopupGOs.Count)
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      this.CreateKeyNotes();
      this.lastPop = "";
    }
  }

  public void CreateKeyNotes()
  {
    if (this.popupContainer.childCount > this.initialPopupGOs.Count)
    {
      Debug.LogWarning((object) "[PopupManager] CreateKeyNotes exit because child counter > 5");
    }
    else
    {
      KeyNotesData[] keyNotesDataArray = new KeyNotesData[Globals.Instance.KeyNotes.Count];
      int index1 = 0;
      foreach (KeyValuePair<string, KeyNotesData> keyNote in Globals.Instance.KeyNotes)
      {
        keyNotesDataArray[index1] = keyNote.Value;
        ++index1;
      }
      this.imageBg.color = this.colorPlain;
      HashSet<string> stringSet = new HashSet<string>()
      {
        "overcharge",
        "chain",
        "jump",
        "jump(bonus%)",
        "repeat",
        "repeatupto",
        "dispel",
        "purge",
        "discover",
        "reveal",
        "transfer",
        "steal",
        "escapes",
        "metamorph",
        "nightmareecho",
        "nightmareechoa",
        "nightmareechob",
        "corruptedecho",
        "transferpain",
        "nightmareimage",
        "mindspike"
      };
      for (int index2 = 0; index2 < keyNotesDataArray.Length; ++index2)
      {
        string id = keyNotesDataArray[index2].Id;
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pop, Vector3.zero, Quaternion.identity, this.popupContainer);
        gameObject.name = id;
        TMP_Text component1 = gameObject.transform.Find("Background/Text").GetComponent<TMP_Text>();
        Image component2 = gameObject.transform.GetChild(0).GetComponent<Image>();
        string[] strArray = new string[4];
        string text = Texts.Instance.GetText(id);
        strArray[0] = !string.IsNullOrEmpty(text) ? text : id;
        strArray[0] = "<b>" + strArray[0] + "</b>";
        strArray[1] = GameManager.Instance.ConfigExtendedDescriptions ? keyNotesDataArray[index2].DescriptionExtended : keyNotesDataArray[index2].Description;
        switch (id)
        {
          case "vanish":
            component2.color = this.colorVanish;
            strArray[2] = "EBAAFF";
            break;
          case "innate":
            component2.color = this.colorInnate;
            strArray[2] = "AFFFC9";
            break;
          case "energy":
            component2.color = this.colorGood;
            strArray[2] = "AADDFF";
            break;
          default:
            if (stringSet.Contains(id))
            {
              component2.color = this.colorPlain;
              strArray[2] = "FFF";
              if (id == "overcharge")
              {
                strArray[0] = "[" + Texts.Instance.GetText("overchargeAcronym") + "] " + text;
                break;
              }
              break;
            }
            component2.color = this.colorPlain;
            AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(id);
            if ((UnityEngine.Object) auraCurseData != (UnityEngine.Object) null)
            {
              if (auraCurseData.IsAura)
              {
                component2.color = this.colorGood;
                strArray[2] = "AADDFF";
              }
              else
              {
                component2.color = this.colorBad;
                strArray[2] = "FF8181";
              }
              if (!auraCurseData.GainCharges)
              {
                ref string local = ref strArray[0];
                local = local + "<space=.5em><size=18><color=#AAAAAA>(" + Texts.Instance.GetText("notStackPlain") + ")</color></size>";
                break;
              }
              break;
            }
            break;
        }
        strArray[3] = "";
        string str1 = this.ReplaceGeneralTags(string.Format(this.popupText, (object[]) strArray));
        string str2 = stringSet.Contains(id) ? "cards" : id;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<voffset=-1><size=24><sprite name=");
        stringBuilder.Append(str2);
        stringBuilder.Append("></size></voffset>");
        stringBuilder.Append(str1);
        string str3 = stringBuilder.ToString();
        component1.text = str3;
        gameObject.SetActive(false);
        this.KeyNotesGO[id] = gameObject;
      }
      this.popTrait = UnityEngine.Object.Instantiate<GameObject>(this.pop, Vector3.zero, Quaternion.identity, this.popupContainer);
      this.popTrait.transform.Find("Background").GetComponent<Image>().color = this.colorTrait;
      this.popTraitText = this.popTrait.transform.Find("Background/Text").GetComponent<TMP_Text>();
      this.popTrait.transform.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 20, 0);
      this.popTrait.transform.localScale = new Vector3(1f, 1f, 1f);
      this.popTrait.name = "trait";
      this.popTown = UnityEngine.Object.Instantiate<GameObject>(this.pop, Vector3.zero, Quaternion.identity, this.popupContainer);
      this.popTown.transform.Find("Background").GetComponent<Image>().color = this.colorTown;
      this.popTownText = this.popTown.transform.Find("Background/Text").GetComponent<TMP_Text>();
      this.popTown.transform.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 20, 0);
      this.popTown.transform.localScale = new Vector3(1f, 1f, 1f);
      this.popTown.name = "town";
      this.popPerk = UnityEngine.Object.Instantiate<GameObject>(this.pop, Vector3.zero, Quaternion.identity, this.popupContainer);
      this.popPerk.transform.Find("Background").GetComponent<Image>().color = this.colorTrait;
      this.popPerkText = this.popPerk.transform.Find("Background/Text").GetComponent<TMP_Text>();
      this.popPerk.transform.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 10, 0);
      this.popPerk.transform.localScale = new Vector3(1f, 1f, 1f);
      this.popPerk.name = "perk";
      this.popPerkText.lineSpacing = 10f;
    }
  }

  private void CleanKeyNotes()
  {
    if (this.KeyNotesActive.Count <= 0)
      return;
    for (int index = 0; index < this.KeyNotesActive.Count; ++index)
    {
      if (this.KeyNotesGO.ContainsKey(this.KeyNotesActive[index]) && (UnityEngine.Object) this.KeyNotesGO[this.KeyNotesActive[index]] != (UnityEngine.Object) null)
        this.KeyNotesGO[this.KeyNotesActive[index]].SetActive(false);
    }
    this.KeyNotesActive.Clear();
  }
}
