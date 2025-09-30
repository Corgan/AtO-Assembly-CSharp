// Decompiled with JetBrains decompiler
// Type: Cheats.CheatGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Cheats
{
  public class CheatGUI : MonoBehaviour
  {
    private bool _showCheats;
    private GameManager _gameManager;
    private GUIStyle _headerStyle;
    private Vector2 _scrollPos;
    private GUIStyle _toggleStyle;
    private GUIStyle _buttonStyle;
    private GUIStyle _windowStyle;
    private Texture2D _transparentTex;
    private int _fontSize = 24;
    public float panelAlpha = 0.8f;

    private void Awake()
    {
      this._gameManager = GameManager.Instance;
      if (!this._gameManager.CheatMode || Object.FindObjectsOfType<CheatGUI>().Length > 1)
      {
        Object.Destroy((Object) this.gameObject);
      }
      else
      {
        Object.DontDestroyOnLoad((Object) this.gameObject);
        this._transparentTex = new Texture2D(1, 1);
        this._transparentTex.SetPixel(0, 0, new Color(0.0f, 0.0f, 0.0f, this.panelAlpha));
        this._transparentTex.Apply();
      }
    }

    private void Update()
    {
      if (!Input.GetKeyDown(KeyCode.C))
        return;
      this._showCheats = !this._showCheats;
    }

    private void OnGUI()
    {
      if (!this._showCheats || (Object) this._gameManager == (Object) null)
        return;
      if (this._windowStyle == null)
        this._windowStyle = new GUIStyle(GUI.skin.window);
      this._windowStyle.normal.background = this._transparentTex;
      if (this._headerStyle == null)
        this._headerStyle = new GUIStyle(GUI.skin.label)
        {
          fontSize = this._fontSize,
          fontStyle = FontStyle.Bold,
          normal = {
            textColor = Color.white
          }
        };
      if (this._toggleStyle == null)
        this._toggleStyle = new GUIStyle(GUI.skin.toggle)
        {
          fontSize = this._fontSize,
          fixedHeight = 25f,
          normal = {
            textColor = Color.white
          }
        };
      if (this._buttonStyle == null)
        this._buttonStyle = new GUIStyle(GUI.skin.button)
        {
          fontSize = this._fontSize,
          fixedHeight = 30f
        };
      GUILayout.BeginArea(new Rect(500f, 10f, 650f, 950f), "Cheats", this._windowStyle);
      this._scrollPos = GUILayout.BeginScrollView(this._scrollPos, GUILayout.Width(550f), GUILayout.Height(850f));
      CheatDrawer.DrawCheatsRuntime(this._gameManager, this._headerStyle, this._toggleStyle, this._buttonStyle);
      GUILayout.EndScrollView();
      GUILayout.EndArea();
    }
  }
}
