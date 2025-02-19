// Decompiled with JetBrains decompiler
// Type: TomeNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TomeNumber : MonoBehaviour
{
  public Transform backgroundT;
  public Transform activeT;
  public SpriteRenderer background;
  public SpriteRenderer border;
  public TMP_Text numberTxt;
  private string number;
  private Color colorActive;
  private Color colorDeactive;
  private bool active;
  private bool visible;
  private Vector3 positionOri;
  private Vector3 positionShow;
  private Vector3 positionHide;
  private Vector3 positionActiveVector = new Vector3(0.2f, 0.0f, 0.0f);

  private void Awake()
  {
    this.colorActive = Functions.HexToColor("#FFA400");
    this.colorDeactive = new Color(1f, 1f, 1f, 1f);
  }

  public void Activate()
  {
    this.active = true;
    this.activeT.gameObject.SetActive(true);
    this.border.color = this.colorActive;
    this.background.color = this.colorActive;
    this.transform.localPosition = this.transform.localPosition - this.positionActiveVector;
  }

  public void Deactivate()
  {
    this.active = false;
    this.background.color = this.colorDeactive;
    this.activeT.gameObject.SetActive(false);
    this.transform.localPosition = this.positionOri;
  }

  public bool IsActive() => this.active;

  public void Init(int _number)
  {
    this.number = _number.ToString();
    this.numberTxt.text = this.number;
    this.positionOri = this.transform.localPosition;
    this.positionShow = new Vector3(this.positionOri.x, this.transform.localPosition.y, 0.0f);
    this.positionHide = new Vector3(this.positionOri.x + 1f, this.transform.localPosition.y, 100f);
  }

  public void SetText(string _text) => this.numberTxt.text = _text;

  public void Show()
  {
    if (!this.gameObject.activeSelf || !this.transform.parent.gameObject.activeSelf)
      return;
    this.transform.localPosition = this.positionShow;
    this.visible = true;
  }

  public void Hide()
  {
    if (!this.gameObject.activeSelf || !this.transform.parent.gameObject.activeSelf)
      return;
    this.transform.localPosition = this.positionHide;
    this.visible = false;
  }

  public bool IsVisible() => this.visible;

  private void OnMouseEnter()
  {
    if (this.active)
      return;
    this.border.color = Functions.HexToColor("#BBBBBB");
    this.activeT.gameObject.SetActive(true);
    GameManager.Instance.SetCursorHover();
  }

  private void OnMouseExit()
  {
    if (!this.active)
      this.activeT.gameObject.SetActive(false);
    GameManager.Instance.SetCursorPlain();
  }

  public void OnMouseUp()
  {
    if (!Functions.ClickedThisTransform(this.transform))
      return;
    TomeManager.Instance.SetPage(int.Parse(this.number));
  }
}
