// Decompiled with JetBrains decompiler
// Type: BotonMenuGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using TMPro.Examples;
using UnityEngine;

#nullable disable
public class BotonMenuGameMode : MonoBehaviour
{
  public TMP_Text optionText;
  public SpriteRenderer cenefa;
  public Transform description;
  public Transform grayMask;
  public int gameMode;

  private void Awake()
  {
    this.grayMask.gameObject.SetActive(true);
    this.description.gameObject.SetActive(false);
  }

  private void Start() => this.TurnOffState();

  public void TurnOnState()
  {
    this.grayMask.gameObject.SetActive(false);
    this.cenefa.color = new Color(1f, 1f, 1f, 0.85f);
  }

  public void TurnOffState()
  {
    this.grayMask.gameObject.SetActive(true);
    this.cenefa.color = new Color(0.78f, 0.55f, 0.56f, 0.75f);
  }

  private void OnMouseEnter()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover);
    this.description.gameObject.SetActive(true);
    this.TurnOnState();
  }

  private void OnMouseExit()
  {
    this.description.gameObject.SetActive(false);
    this.TurnOffState();
  }

  private IEnumerator RebuildWarp()
  {
    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    this.optionText.GetComponent<WarpTextExample>().CurveScale = 4.8f;
  }

  public void OnMouseUp()
  {
    if (AlertManager.Instance.IsActive())
      return;
    MainMenuManager.Instance.ShowSaveGame(true, this.gameMode);
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
  }
}
