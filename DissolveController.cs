// Decompiled with JetBrains decompiler
// Type: DissolveController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DissolveController : MonoBehaviour
{
  public float dissolveAmount;
  public float dissolveSpeed;
  public bool isDissolving;
  [ColorUsage(true, true)]
  public Color outColor;
  [ColorUsage(true, true)]
  public Color inColor;
  private Material mat;

  private void Start() => this.mat = this.GetComponent<SpriteRenderer>().material;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.A))
      this.isDissolving = true;
    if (Input.GetKeyDown(KeyCode.S))
      this.isDissolving = false;
    if (this.isDissolving)
      this.DissolveOut(this.dissolveSpeed, this.outColor);
    if (!this.isDissolving)
      this.DissolveIn(this.dissolveSpeed, this.inColor);
    this.mat.SetFloat("_DissolveAmount", this.dissolveAmount);
  }

  public void DissolveOut(float speed, Color color)
  {
    this.mat.SetColor("_DissolveColor", color);
    if ((double) this.dissolveAmount <= -0.1)
      return;
    this.dissolveAmount -= Time.deltaTime * speed;
  }

  public void DissolveIn(float speed, Color color)
  {
    this.mat.SetColor("_DissolveColor", color);
    if ((double) this.dissolveAmount >= 1.0)
      return;
    this.dissolveAmount += Time.deltaTime * this.dissolveSpeed;
  }
}
