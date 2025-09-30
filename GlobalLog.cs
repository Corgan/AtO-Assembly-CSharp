// Decompiled with JetBrains decompiler
// Type: GlobalLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class GlobalLog : MonoBehaviour
{
  private TMP_Text logTxt;

  public static GlobalLog Instance { get; private set; }

  private void Awake()
  {
    if ((Object) GlobalLog.Instance == (Object) null)
      GlobalLog.Instance = this;
    else if ((Object) GlobalLog.Instance != (Object) this)
      Object.Destroy((Object) this.gameObject);
    this.logTxt = this.GetComponent<TMP_Text>();
  }

  public void Log(string module = "", string text = "")
  {
    string str = "";
    if (module != "")
      str = str + "<color=#999>[" + module + "]</color> ";
    this.logTxt.text = str + text + "\n\n" + this.logTxt.text;
  }
}
