// Decompiled with JetBrains decompiler
// Type: BotonEndTurn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

#nullable disable
public class BotonEndTurn : MonoBehaviour
{
  public Transform border;
  public Transform background;
  private SpriteRenderer backgroundSR;
  private SpriteRenderer borderSR;
  private Color sourceColor;
  private Color bgColor;
  private bool showBorder;
  private Vector3 sizeOn = new Vector3(1.05f, 1.05f, 1f);
  private Vector3 sizeOff = new Vector3(1f, 1f, 1f);
  private Color colorFade = new Color(0.0f, 0.0f, 0.0f, 0.1f);

  private void Awake()
  {
    this.borderSR = this.border.GetComponent<SpriteRenderer>();
    this.backgroundSR = this.background.GetComponent<SpriteRenderer>();
    this.bgColor = this.backgroundSR.color;
    this.sourceColor = new Color(this.borderSR.color.r, this.borderSR.color.g, this.borderSR.color.b, 0.0f);
  }

  private void Update()
  {
    if (this.showBorder && (double) this.borderSR.color.a < 0.699999988079071)
    {
      this.borderSR.color += this.colorFade;
    }
    else
    {
      if (this.showBorder || (double) this.borderSR.color.a <= 0.0)
        return;
      this.borderSR.color -= this.colorFade;
    }
  }

  private void OnMouseEnter()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
      return;
    this.transform.localScale = this.sizeOn;
    GameManager.Instance.SetCursorHover();
    this.borderSR.color = this.sourceColor;
    this.showBorder = true;
  }

  private void OnMouseExit()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
      return;
    GameManager.Instance.SetCursorPlain();
    this.transform.localScale = this.sizeOff;
    this.showBorder = false;
  }

  public void OnMouseUp()
  {
    if (!Functions.ClickedThisTransform(this.transform) || AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
      return;
    GameManager.Instance.SetCursorPlain();
    if (EventSystem.current.IsPointerOverGameObject())
      return;
    this.showBorder = false;
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    Scene activeScene = SceneManager.GetActiveScene();
    string name = this.gameObject.name;
    if (activeScene.name == "Combat")
    {
      if (MatchManager.Instance.CardDrag)
        return;
      MatchManager.Instance.EndTurn();
    }
    else if (activeScene.name == "Game")
    {
      if (name == "SinglePlayer")
        SceneManager.LoadScene("TeamManagement");
      else
        SceneManager.LoadScene("Lobby");
    }
    else if (activeScene.name == "TeamManagement")
    {
      TeamManagement.Instance.LaunchCombat();
    }
    else
    {
      if (!(activeScene.name == "Lobby"))
        return;
      switch (name)
      {
        case "ButtonMultiplayerCreate":
          LobbyManager.Instance.ShowCreate();
          break;
        case "ButtonMultiplayerJoin":
          LobbyManager.Instance.ShowJoin();
          break;
        case "ButtonMultiplayerBack":
          LobbyManager.Instance.GoBack();
          break;
        case "SetReady":
          LobbyManager.Instance.SetReady();
          break;
        case "AllUnready":
          LobbyManager.Instance.AllUnready();
          break;
      }
    }
  }
}
