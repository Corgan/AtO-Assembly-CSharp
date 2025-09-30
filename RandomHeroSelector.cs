// Decompiled with JetBrains decompiler
// Type: RandomHeroSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class RandomHeroSelector : MonoBehaviour
{
  public int boxId;
  private SpriteRenderer diceSpr;
  private Color colorOver = new Color(0.5f, 0.5f, 0.5f, 1f);
  private Color colorOut = new Color(1f, 1f, 1f, 1f);
  private Vector3 oriScale;
  private Vector3 maxScale;
  private Coroutine selectionCo;
  private bool selecting;

  private void Awake()
  {
    this.diceSpr = this.GetComponent<SpriteRenderer>();
    this.oriScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
    this.maxScale = this.oriScale + new Vector3(0.1f, 0.1f, 0.0f);
  }

  public void SetId(int _id) => this.boxId = _id;

  private void OnMouseEnter()
  {
    this.diceSpr.color = this.colorOver;
    this.transform.localScale = this.maxScale;
  }

  private void OnMouseExit()
  {
    this.diceSpr.color = this.colorOut;
    this.transform.localScale = this.oriScale;
  }

  public void OnMouseUp()
  {
    if (this.selectionCo != null)
      this.StopCoroutine(this.selectionCo);
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    this.selectionCo = this.StartCoroutine(this.SelectRandomHero());
  }

  private IEnumerator SelectRandomHero()
  {
    if (this.selecting)
      yield return (object) Globals.Instance.WaitForSeconds(0.02f);
    HeroSelectionManager.Instance.SetRandomHero(this.boxId);
    this.selecting = true;
    yield return (object) Globals.Instance.WaitForSeconds(0.02f);
    this.selecting = false;
  }
}
