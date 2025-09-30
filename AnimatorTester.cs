// Decompiled with JetBrains decompiler
// Type: AnimatorTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnimatorTester : MonoBehaviour
{
  private Animator _animator;

  private void Awake() => this._animator = this.GetComponent<Animator>();

  private void Update()
  {
    if (!((Object) this._animator != (Object) null))
      return;
    if (Input.GetKeyDown(KeyCode.Alpha1))
      this._animator.SetTrigger("hit");
    if (Input.GetKeyDown(KeyCode.Alpha2))
      this._animator.SetTrigger("attack");
    if (!Input.GetKeyDown(KeyCode.Alpha3))
      return;
    this._animator.SetTrigger("cast");
  }
}
