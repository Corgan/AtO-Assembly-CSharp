// Decompiled with JetBrains decompiler
// Type: LogosManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class LogosManager : MonoBehaviour
{
  public List<SpriteRenderer> srList;
  public Transform sceneCamera;
  private float secondsFadeStep = 0.02f;
  private float secondsFade = 1f;
  private float iterationFadeIncrement;
  private float timeBetweenLogos = 1f;
  private float timeLogoFull = 1f;
  private int logoStep = -1;
  private Coroutine logoCo;
  private Color whiteColor;

  public static LogosManager Instance { get; private set; }

  private void Awake()
  {
    if ((Object) LogosManager.Instance == (Object) null)
      LogosManager.Instance = this;
    else if ((Object) LogosManager.Instance != (Object) this)
      Object.Destroy((Object) this.gameObject);
    int index = 0;
    while (true)
    {
      int num = index;
      int? count = this.srList?.Count;
      int valueOrDefault = count.GetValueOrDefault();
      if (num < valueOrDefault & count.HasValue)
      {
        if (this.srList[index].gameObject.activeSelf)
          this.srList[index].transform.gameObject.SetActive(false);
        ++index;
      }
      else
        break;
    }
  }

  private void Start()
  {
    if ((Object) GameManager.Instance == (Object) null)
    {
      SceneManager.LoadScene("Game");
    }
    else
    {
      this.sceneCamera.gameObject.SetActive(false);
      if (this.srList == null || this.srList.Count == 0)
      {
        this.GoToNextScene();
      }
      else
      {
        this.whiteColor = new Color(1f, 1f, 1f, 0.0f);
        for (int index = 0; index < this.srList.Count; ++index)
          this.srList[index].color = this.whiteColor;
        this.iterationFadeIncrement = (float) (1.0 / ((double) this.secondsFade / (double) this.secondsFadeStep));
        GameManager.Instance.SceneLoaded();
        this.DoLogos();
      }
    }
  }

  private void DoLogos()
  {
    ++this.logoStep;
    if (this.logoStep < this.srList.Count)
      this.logoCo = this.StartCoroutine(this.LogoAnimation());
    else
      this.GoToNextScene();
  }

  private void GoToNextScene() => GameManager.Instance.ChangeScene("MainMenu");

  private IEnumerator LogoAnimation()
  {
    yield return (object) Globals.Instance.WaitForSeconds(this.timeBetweenLogos * 0.5f);
    float index = 0.0f;
    SpriteRenderer sr = this.srList[this.logoStep];
    sr.gameObject.SetActive(true);
    while ((double) index < 1.0)
    {
      index += this.iterationFadeIncrement;
      this.whiteColor.a = index;
      sr.color = this.whiteColor;
      yield return (object) Globals.Instance.WaitForSeconds(this.secondsFadeStep);
    }
    yield return (object) Globals.Instance.WaitForSeconds(this.timeLogoFull);
    while ((double) index > 0.0)
    {
      index -= this.iterationFadeIncrement;
      this.whiteColor.a = index;
      sr.color = this.whiteColor;
      yield return (object) Globals.Instance.WaitForSeconds(this.secondsFadeStep);
    }
    sr.gameObject.SetActive(false);
    yield return (object) Globals.Instance.WaitForSeconds(this.timeBetweenLogos * 0.5f);
    this.DoLogos();
  }

  public void Escape(bool skipAll = false)
  {
    if (this.logoCo != null)
      this.StopCoroutine(this.logoCo);
    this.srList[this.logoStep].gameObject.SetActive(false);
    if (skipAll)
      this.logoStep = this.srList.Count - 1;
    this.DoLogos();
  }
}
