// Decompiled with JetBrains decompiler
// Type: DeckInHero
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DeckInHero : MonoBehaviour
{
  public bool isNormalDeck;
  private SpriteRenderer cardSprite;
  public int heroIndex;

  private void Awake() => this.cardSprite = this.GetComponent<SpriteRenderer>();

  private void OnMouseEnter()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || MatchManager.Instance.CardDrag)
      return;
    this.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
  }

  private void OnMouseExit()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || MatchManager.Instance.CardDrag)
      return;
    this.transform.localScale = new Vector3(1f, 1f, 1f);
  }

  private void OnMouseUp()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || MatchManager.Instance.CardDrag)
      return;
    if (!this.isNormalDeck)
      MatchManager.Instance.ShowCharacterWindow("combatdiscard", characterIndex: this.heroIndex);
    else
      MatchManager.Instance.ShowCharacterWindow("combatdeck", characterIndex: this.heroIndex);
  }
}
