// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXSceneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
namespace EpicToonFX
{
  public class ETFXSceneManager : MonoBehaviour
  {
    public bool GUIHide;
    public bool GUIHide2;
    public bool GUIHide3;
    public bool GUIHide4;

    public void LoadScene2DDemo() => SceneManager.LoadScene("etfx_2ddemo");

    public void LoadSceneCards() => SceneManager.LoadScene("etfx_cards");

    public void LoadSceneCombat() => SceneManager.LoadScene("etfx_combat");

    public void LoadSceneDecals() => SceneManager.LoadScene("etfx_decals");

    public void LoadSceneDecals2() => SceneManager.LoadScene("etfx_decals2");

    public void LoadSceneEmojis() => SceneManager.LoadScene("etfx_emojis");

    public void LoadSceneEmojis2() => SceneManager.LoadScene("etfx_emojis2");

    public void LoadSceneExplosions() => SceneManager.LoadScene("etfx_explosions");

    public void LoadSceneExplosions2() => SceneManager.LoadScene("etfx_explosions2");

    public void LoadSceneFire() => SceneManager.LoadScene("etfx_fire");

    public void LoadSceneOnomatopoeia() => SceneManager.LoadScene("etfx_onomatopoeia");

    public void LoadSceneFireworks() => SceneManager.LoadScene("etfx_fireworks");

    public void LoadSceneFlares() => SceneManager.LoadScene("etfx_flares");

    public void LoadSceneMagic() => SceneManager.LoadScene("etfx_magic");

    public void LoadSceneMagic2() => SceneManager.LoadScene("etfx_magic2");

    public void LoadSceneMagic3() => SceneManager.LoadScene("etfx_magic3");

    public void LoadSceneMainDemo() => SceneManager.LoadScene("etfx_maindemo");

    public void LoadSceneMissiles() => SceneManager.LoadScene("etfx_missiles");

    public void LoadScenePortals() => SceneManager.LoadScene("etfx_portals");

    public void LoadScenePortals2() => SceneManager.LoadScene("etfx_portals2");

    public void LoadScenePowerups() => SceneManager.LoadScene("etfx_powerups");

    public void LoadScenePowerups2() => SceneManager.LoadScene("etfx_powerups2");

    public void LoadScenePowerups3() => SceneManager.LoadScene("etfx_powerups3");

    public void LoadSceneSparkles() => SceneManager.LoadScene("etfx_sparkles");

    public void LoadSceneSwordCombat() => SceneManager.LoadScene("etfx_swordcombat");

    public void LoadSceneSwordCombat2() => SceneManager.LoadScene("etfx_swordcombat2");

    public void LoadSceneMoney() => SceneManager.LoadScene("etfx_money");

    public void LoadSceneHealing() => SceneManager.LoadScene("etfx_healing");

    public void LoadSceneWind() => SceneManager.LoadScene("etfx_wind");

    public void LoadSceneWater() => SceneManager.LoadScene("etfx_water");

    public void LoadSceneFruit() => SceneManager.LoadScene("etfx_fruit");

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.L))
      {
        this.GUIHide = !this.GUIHide;
        if (this.GUIHide)
          GameObject.Find("CanvasSceneSelect").GetComponent<Canvas>().enabled = false;
        else
          GameObject.Find("CanvasSceneSelect").GetComponent<Canvas>().enabled = true;
      }
      if (Input.GetKeyDown(KeyCode.J))
      {
        this.GUIHide2 = !this.GUIHide2;
        if (this.GUIHide2)
          GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        else
          GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
      }
      if (Input.GetKeyDown(KeyCode.H))
      {
        this.GUIHide3 = !this.GUIHide3;
        if (this.GUIHide3)
          GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas>().enabled = false;
        else
          GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas>().enabled = true;
      }
      if (!Input.GetKeyDown(KeyCode.K))
        return;
      this.GUIHide4 = !this.GUIHide4;
      if (this.GUIHide3)
        GameObject.Find("CanvasTips").GetComponent<Canvas>().enabled = false;
      else
        GameObject.Find("CanvasTips").GetComponent<Canvas>().enabled = true;
    }
  }
}
