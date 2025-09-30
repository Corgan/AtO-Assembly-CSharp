// Decompiled with JetBrains decompiler
// Type: EventItemTrack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Text;
using UnityEngine;

#nullable disable
public class EventItemTrack : MonoBehaviour
{
  private string description;
  private Coroutine timeoutCardCo;
  private EventRequirementData requirementData;

  public void SetItemTrack(EventRequirementData _requirementData)
  {
    this.requirementData = _requirementData;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<size=+3><color=#FFF>");
    stringBuilder.Append(this.requirementData.RequirementName);
    stringBuilder.Append("</color></size><line-height=30><br></line-height>");
    stringBuilder.Append(this.requirementData.Description);
    this.description = stringBuilder.ToString();
    this.GetComponent<SpriteRenderer>().sprite = this.requirementData.ItemSprite;
    if (!((Object) this.transform.GetChild(0).GetComponent<SpriteMask>() != (Object) null))
      return;
    this.transform.GetChild(0).GetComponent<SpriteMask>().sprite = this.requirementData.ItemSprite;
  }

  private void OnMouseEnter()
  {
    if (AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive() || (bool) (Object) MapManager.Instance && MapManager.Instance.IsCharacterUnlock())
      return;
    PopupManager.Instance.SetText(this.description, true, "followdown", true);
    if (this.timeoutCardCo != null)
      this.StopCoroutine(this.timeoutCardCo);
    if (!((Object) this.requirementData.TrackCard != (Object) null))
      return;
    this.timeoutCardCo = this.StartCoroutine(this.TimeOutCard());
  }

  private IEnumerator TimeOutCard()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EventItemTrack eventItemTrack = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      CardData cardData = Globals.Instance.GetCardData(eventItemTrack.requirementData.TrackCard.Id, false);
      CardItem component = eventItemTrack.GetComponent<CardItem>();
      component.cardFromEventTracker = true;
      component.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(cardData.Id, false), eventItemTrack.GetComponent<BoxCollider2D>(), disableCollider: true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) Globals.Instance.WaitForSeconds(0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void OnMouseExit()
  {
    PopupManager.Instance.ClosePopup();
    if (this.timeoutCardCo != null)
      this.StopCoroutine(this.timeoutCardCo);
    if (!((Object) this.requirementData.TrackCard != (Object) null))
      return;
    this.GetComponent<CardItem>().DestroyReveleadOutside();
  }
}
