// Decompiled with JetBrains decompiler
// Type: ItemCorruptionIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class ItemCorruptionIcon : MonoBehaviour
{
  private string internalId;
  private SpriteRenderer itemSpriteRenderer;
  private Transform rareParticles;
  private bool allowInput;
  private Coroutine showCardCo;
  private CardItem CI;
  private GameObject card;
  private CardData cardData;
  private bool mouseIsOver;
  [SerializeField]
  private Transform spriteBackgroundHover;
  private string itemType;

  private void Awake()
  {
    this.itemSpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    this.rareParticles = this.GetComponentInChildren<ParticleSystem>(true).transform;
  }

  private void OnMouseUp()
  {
    if (!this.allowInput)
      return;
    this.fOnMouseUP();
  }

  private void OnMouseOver() => this.DoHover(true);

  private void OnMouseExit() => this.DoHover(false);

  private void fOnMouseUP()
  {
    if (this.internalId.IsNullOrEmpty())
      return;
    GameManager.Instance.CleanTempContainer();
    CardCraftManager.Instance.SelectCard(this.internalId);
  }

  public void SetSprite(Sprite sprite) => this.itemSpriteRenderer.sprite = sprite;

  public void SetInternalId(string internalId)
  {
    this.internalId = internalId;
    this.cardData = Globals.Instance.GetCardData(internalId.Split("_", StringSplitOptions.None)[0]);
  }

  public void AllowInput(bool state) => this.allowInput = state;

  public void ShowRareParticles(bool state = true)
  {
    this.rareParticles.gameObject.SetActive(state);
  }

  public void DoHover(bool state)
  {
    if (this.mouseIsOver == state || !this.allowInput)
      return;
    this.mouseIsOver = state;
    if (state)
    {
      if (this.showCardCo != null)
        this.StopCoroutine(this.showCardCo);
      if (this.gameObject.activeSelf && this.gameObject.activeInHierarchy)
        this.showCardCo = this.StartCoroutine(this.ShowCardCo());
    }
    else
    {
      if (this.showCardCo != null)
        this.StopCoroutine(this.showCardCo);
      if ((UnityEngine.Object) this.CI != (UnityEngine.Object) null)
        this.CI.HideKeyNotes();
      if ((UnityEngine.Object) this.card != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.card);
        this.CI = (CardItem) null;
      }
    }
    if (!((UnityEngine.Object) this.cardData == (UnityEngine.Object) null) && this.cardData.CardType == Enums.CardType.Corruption)
      return;
    this.spriteBackgroundHover.gameObject.SetActive(state);
  }

  private IEnumerator ShowCardCo()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ItemCorruptionIcon itemCorruptionIcon = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if ((UnityEngine.Object) itemCorruptionIcon.cardData == (UnityEngine.Object) null)
      return false;
    Vector3 position = new Vector3(-2.1f, 0.0f, 0.0f);
    UnityEngine.Object.Destroy((UnityEngine.Object) itemCorruptionIcon.card);
    Transform transform = itemCorruptionIcon.transform;
    itemCorruptionIcon.card = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, transform);
    itemCorruptionIcon.card.name = itemCorruptionIcon.cardData.Id;
    itemCorruptionIcon.CI = itemCorruptionIcon.card.GetComponent<CardItem>();
    itemCorruptionIcon.CI.SetCard(itemCorruptionIcon.cardData.Id, _theHero: AtOManager.Instance.GetHero(CardCraftManager.Instance.heroIndex), GetFromGlobal: true);
    itemCorruptionIcon.CI.DisableTrail();
    if ((bool) (UnityEngine.Object) LootManager.Instance)
      itemCorruptionIcon.CI.TopLayeringOrder("UI", 32000);
    else
      itemCorruptionIcon.CI.TopLayeringOrder("UI", 32000);
    if (true)
      itemCorruptionIcon.CI.DisableCollider();
    else
      itemCorruptionIcon.CI.EnableCollider();
    itemCorruptionIcon.card.transform.position = itemCorruptionIcon.transform.position + new Vector3(0.0f, 0.4f, 0.0f);
    itemCorruptionIcon.CI.SetDestinationScaleRotation(position, 1.2f, Quaternion.Euler(0.0f, 0.0f, 0.0f));
    if ((UnityEngine.Object) CardCraftManager.Instance != (UnityEngine.Object) null)
      itemCorruptionIcon.CI.SetDestinationLocalScale(1.4f);
    itemCorruptionIcon.CI.active = true;
    return false;
  }
}
