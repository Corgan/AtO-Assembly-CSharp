// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXButtonScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace EpicToonFX
{
  public class ETFXButtonScript : MonoBehaviour
  {
    public GameObject Button;
    private Text MyButtonText;
    private string projectileParticleName;
    private ETFXFireProjectile effectScript;
    private ETFXProjectileScript projectileScript;
    public float buttonsX;
    public float buttonsY;
    public float buttonsSizeX;
    public float buttonsSizeY;
    public float buttonsDistance;

    private void Start()
    {
      this.effectScript = GameObject.Find("ETFXFireProjectile").GetComponent<ETFXFireProjectile>();
      this.getProjectileNames();
      this.MyButtonText = this.Button.transform.Find("Text").GetComponent<Text>();
      this.MyButtonText.text = this.projectileParticleName;
    }

    private void Update() => this.MyButtonText.text = this.projectileParticleName;

    public void getProjectileNames()
    {
      this.projectileScript = this.effectScript.projectiles[this.effectScript.currentProjectile].GetComponent<ETFXProjectileScript>();
      this.projectileParticleName = this.projectileScript.projectileParticle.name;
    }

    public bool overButton()
    {
      Rect rect1 = new Rect(this.buttonsX, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
      Rect rect2 = new Rect(this.buttonsX + this.buttonsDistance, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
      return rect1.Contains(new Vector2(Input.mousePosition.x, (float) Screen.height - Input.mousePosition.y)) || rect2.Contains(new Vector2(Input.mousePosition.x, (float) Screen.height - Input.mousePosition.y));
    }
  }
}
