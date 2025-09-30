// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXEffectControllerPooled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace EpicToonFX
{
  public class ETFXEffectControllerPooled : MonoBehaviour
  {
    public GameObject[] effects;
    private List<GameObject> effectsPool;
    private int effectIndex;
    [Space(10f)]
    [Header("Spawn Settings")]
    public bool disableLights = true;
    public bool disableSound = true;
    public float startDelay = 0.2f;
    public float respawnDelay = 0.5f;
    public bool slideshowMode;
    public bool autoRotation;
    [Range(0.001f, 0.5f)]
    public float autoRotationSpeed = 0.1f;
    private GameObject currentEffect;
    private Text effectNameText;
    private Text effectIndexText;
    private ETFXMouseOrbit etfxMouseOrbit;

    private void Awake()
    {
      this.effectNameText = GameObject.Find("EffectName").GetComponent<Text>();
      this.effectIndexText = GameObject.Find("EffectIndex").GetComponent<Text>();
      this.etfxMouseOrbit = Camera.main.GetComponent<ETFXMouseOrbit>();
      this.etfxMouseOrbit.etfxEffectControllerPooled = this;
      this.effectsPool = new List<GameObject>();
      for (int index = 0; index < this.effects.Length; ++index)
      {
        GameObject gameObject = Object.Instantiate<GameObject>(this.effects[index], this.transform.position, Quaternion.identity);
        gameObject.transform.parent = this.transform;
        this.effectsPool.Add(gameObject);
        gameObject.SetActive(false);
      }
    }

    private void Start() => this.Invoke("InitializeLoop", this.startDelay);

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        this.NextEffect();
      if (!Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.LeftArrow))
        return;
      this.PreviousEffect();
    }

    private void FixedUpdate()
    {
      if (!this.autoRotation)
        return;
      this.etfxMouseOrbit.SetAutoRotationSpeed(this.autoRotationSpeed);
      if (this.etfxMouseOrbit.isAutoRotating)
        return;
      this.etfxMouseOrbit.InitializeAutoRotation();
    }

    public void InitializeLoop() => this.StartCoroutine(this.EffectLoop());

    public void NextEffect()
    {
      if (this.effectIndex < this.effects.Length - 1)
        ++this.effectIndex;
      else
        this.effectIndex = 0;
      this.CleanCurrentEffect();
    }

    public void PreviousEffect()
    {
      if (this.effectIndex > 0)
        --this.effectIndex;
      else
        this.effectIndex = this.effects.Length - 1;
      this.CleanCurrentEffect();
    }

    private void CleanCurrentEffect()
    {
      this.StopAllCoroutines();
      if ((Object) this.currentEffect != (Object) null)
        this.currentEffect.SetActive(false);
      this.StartCoroutine(this.EffectLoop());
    }

    private IEnumerator EffectLoop()
    {
      this.currentEffect = this.effectsPool[this.effectIndex];
      this.currentEffect.SetActive(true);
      if (this.disableLights && (bool) (Object) this.currentEffect.GetComponent<Light>())
        this.currentEffect.GetComponent<Light>().enabled = false;
      if (this.disableSound && (bool) (Object) this.currentEffect.GetComponent<AudioSource>())
        this.currentEffect.GetComponent<AudioSource>().enabled = false;
      this.effectNameText.text = this.effects[this.effectIndex].name;
      this.effectIndexText.text = (this.effectIndex + 1).ToString() + " of " + this.effects.Length.ToString();
      ParticleSystem particleSystem = this.currentEffect.GetComponent<ParticleSystem>();
      while (true)
      {
        do
        {
          yield return (object) new WaitForSeconds(particleSystem.main.duration + this.respawnDelay);
          if (this.slideshowMode)
            goto label_8;
        }
        while (particleSystem.main.loop);
        this.currentEffect.SetActive(false);
        this.currentEffect.SetActive(true);
        continue;
label_8:
        if (particleSystem.main.loop)
          yield return (object) new WaitForSeconds(this.respawnDelay);
        this.NextEffect();
      }
    }
  }
}
