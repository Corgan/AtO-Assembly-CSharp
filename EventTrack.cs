// Decompiled with JetBrains decompiler
// Type: EventTrack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class EventTrack : MonoBehaviour
{
  public TMP_Text trackName;
  private string description;
  public Transform textT;
  public Transform bgT;
  public SpriteRenderer iconSprite;
  private BoxCollider2D collider;

  private void Awake() => this.collider = this.GetComponent<BoxCollider2D>();

  public void SetTrack(string erq)
  {
    EventRequirementData requirementData = Globals.Instance.GetRequirementData(erq);
    this.trackName.text = requirementData.RequirementName;
    this.description = requirementData.Description;
    if ((Object) requirementData.TrackSprite != (Object) null)
      this.iconSprite.sprite = requirementData.TrackSprite;
    this.StartCoroutine(this.ShowTrack(true));
  }

  private void OnMouseEnter()
  {
    if (AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive() || (bool) (Object) MapManager.Instance && MapManager.Instance.IsCharacterUnlock() || (bool) (Object) EventManager.Instance && this.textT.gameObject.activeSelf)
      return;
    if (this.textT.gameObject.activeSelf)
      this.StartCoroutine(this.ShowTrack(true));
    else
      this.collider.offset = new Vector2(0.0f, 0.0f);
    PopupManager.Instance.SetText(this.description, true);
  }

  private IEnumerator ShowTrack(bool state)
  {
    float posEndText = 0.0f;
    float posEndBg = 0.0f;
    Vector3 stepBg = Vector3.zero;
    Vector3 stepText = Vector3.zero;
    int steps = 10;
    stepBg = new Vector3(2.54f / (float) steps, 0.0f, 0.0f);
    stepText = new Vector3(4.1f / (float) steps, 0.0f, 0.0f);
    if (state)
    {
      posEndText = -0.12f;
      posEndBg = -0.5f;
      this.collider.offset = new Vector2(-0.45f, -0.03f);
    }
    else
    {
      posEndText = -4.1f;
      posEndBg = -3.4f;
      this.collider.offset = new Vector2(-2.4f, -0.03f);
    }
    for (int i = 0; i < steps; ++i)
    {
      if (state)
      {
        if ((double) this.textT.localPosition.x < (double) posEndText)
          this.textT.localPosition += stepText;
        if ((double) this.bgT.localPosition.x < (double) posEndBg)
          this.bgT.localPosition += stepBg;
      }
      else
      {
        if ((double) this.textT.localPosition.x > (double) posEndText)
          this.textT.localPosition -= stepText;
        if ((double) this.bgT.localPosition.x > (double) posEndBg)
          this.bgT.localPosition -= stepBg;
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    }
  }

  private void OnMouseExit() => PopupManager.Instance.ClosePopup();
}
