// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXEffectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace EpicToonFX
{
  public class ETFXEffectController : MonoBehaviour
  {
    public GameObject[] effects;
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
      this.etfxMouseOrbit.etfxEffectController = this;
    }

    private void Start()
    {
      this.etfxMouseOrbit = Camera.main.GetComponent<ETFXMouseOrbit>();
      this.etfxMouseOrbit.etfxEffectController = this;
      this.Invoke("InitializeLoop", this.startDelay);
    }

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
        Object.Destroy((Object) this.currentEffect);
      this.StartCoroutine(this.EffectLoop());
    }

    private IEnumerator EffectLoop()
    {
      ETFXEffectController effectController = this;
      GameObject gameObject = Object.Instantiate<GameObject>(effectController.effects[effectController.effectIndex], effectController.transform.position, Quaternion.identity);
      effectController.currentEffect = gameObject;
      if (effectController.disableLights && (bool) (Object) gameObject.GetComponent<Light>())
        gameObject.GetComponent<Light>().enabled = false;
      if (effectController.disableSound && (bool) (Object) gameObject.GetComponent<AudioSource>())
        gameObject.GetComponent<AudioSource>().enabled = false;
      effectController.effectNameText.text = effectController.effects[effectController.effectIndex].name;
      effectController.effectIndexText.text = (effectController.effectIndex + 1).ToString() + " of " + effectController.effects.Length.ToString();
      ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
      while (true)
      {
        do
        {
          yield return (object) new WaitForSeconds(particleSystem.main.duration + effectController.respawnDelay);
          if (effectController.slideshowMode)
            goto label_8;
        }
        while (particleSystem.main.loop);
        effectController.currentEffect.SetActive(false);
        effectController.currentEffect.SetActive(true);
        continue;
label_8:
        if (particleSystem.main.loop)
          yield return (object) new WaitForSeconds(effectController.respawnDelay);
        effectController.NextEffect();
      }
    }
  }
}
