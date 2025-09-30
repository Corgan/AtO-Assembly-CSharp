// Decompiled with JetBrains decompiler
// Type: ButtonCombatTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ButtonCombatTest : MonoBehaviour
{
  private void Start()
  {
    if (GameManager.Instance.GetDeveloperMode())
    {
      this.gameObject.SetActive(true);
      this.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnTestCombatButtonClickEvent));
    }
    else
      this.gameObject.SetActive(false);
  }

  private void OnTestCombatButtonClickEvent() => SceneStatic.LoadByName("TeamManagement");
}
