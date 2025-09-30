// Decompiled with JetBrains decompiler
// Type: AudioManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AudioManager : MonoBehaviour
{
  public AudioSource _audioSource;
  public AudioSource _audioSourceBSO;
  public AudioSource _audioSourceAmbience;
  public AudioClip[] audioArray;
  public AudioClip[] audioArrayNew;
  public AudioClip[] ambienceArray;
  public Dictionary<string, AudioClip> audioLibrary;
  public Dictionary<string, AudioClip> audioLibraryNew;
  public Dictionary<string, AudioClip> ambienceLibrary;
  public AudioClip BSO_Game;
  public AudioClip BSO_Combat;
  public AudioClip BSO_Town;
  public AudioClip BSO_Map;
  public AudioClip BSO_Event;
  public AudioClip BSO_Craft;
  public AudioClip BSO_Rewards;
  private float maxVolumeBSO = 1f;
  private float maxVolumeAmbience = 1f;
  private AudioClip BSO;
  private AudioClip currentBSO;
  private Coroutine coFadeAmbience;
  private Coroutine coFadeBSO;
  [Header("Custom Sounds")]
  public AudioClip soundButtonHover;
  public AudioClip soundButtonClick;
  public AudioClip soundCardClick;
  public AudioClip soundCardHover;
  public AudioClip soundCardDrag;
  public AudioClip soundCombatBegins;
  public AudioClip soundCombatIsYourTurn;
  public AudioClip walkStone;
  public AudioClip walkGrass;
  public AudioClip walkWater;

  public static AudioManager Instance { get; private set; }

  private void Awake()
  {
    if ((Object) AudioManager.Instance == (Object) null)
      AudioManager.Instance = this;
    else if ((Object) AudioManager.Instance != (Object) this)
      Object.Destroy((Object) this.gameObject);
    Object.DontDestroyOnLoad((Object) this.gameObject);
    this.GenerateLibrary();
  }

  private void Start()
  {
    this._audioSourceBSO.volume = this.maxVolumeBSO;
    this._audioSourceBSO.loop = true;
  }

  public void StartStopBSO(bool status)
  {
    if (!GameManager.Instance.ConfigBackgroundMute)
      this._audioSourceBSO.mute = false;
    else
      this._audioSourceBSO.mute = !status;
  }

  public void StartStopAmbience(bool status)
  {
    if (!GameManager.Instance.ConfigBackgroundMute)
      this._audioSourceAmbience.mute = false;
    else
      this._audioSourceAmbience.mute = !status;
  }

  public void DoAmbience(string whatAmbience)
  {
    if (this.coFadeAmbience != null)
      this.StopCoroutine(this.coFadeAmbience);
    this.coFadeAmbience = this.StartCoroutine(this.FadeSound(this._audioSourceAmbience, this.ambienceLibrary[whatAmbience], this.maxVolumeAmbience));
  }

  public void StopAmbience()
  {
    if (this.coFadeAmbience != null)
      this.StopCoroutine(this.coFadeAmbience);
    this.coFadeAmbience = this.StartCoroutine(this.FadeSound(this._audioSourceAmbience));
  }

  public void DoBSO(string whatBSO = "", AudioClip acBSO = null)
  {
    if ((Object) acBSO != (Object) null)
    {
      this.BSO = acBSO;
    }
    else
    {
      switch (whatBSO)
      {
        case "Game":
          this.BSO = this.BSO_Game;
          break;
        case "Town":
          this.BSO = this.BSO_Town;
          break;
        case "Combat":
          this.BSO = this.BSO_Combat;
          break;
        case "Event":
          this.BSO = this.BSO_Event;
          break;
        case "Map":
          this.BSO = this.BSO_Map;
          break;
        case "Craft":
          this.BSO = this.BSO_Craft;
          break;
        case "Rewards":
          this.BSO = this.BSO_Rewards;
          break;
        default:
          return;
      }
    }
    if ((Object) this.currentBSO == (Object) this.BSO)
      return;
    this.currentBSO = this.BSO;
    if (this.coFadeBSO != null)
      this.StopCoroutine(this.coFadeBSO);
    this.coFadeBSO = this.StartCoroutine(this.FadeSound(this._audioSourceBSO, this.BSO, this.maxVolumeBSO));
  }

  public void DoBSOAudioClip(AudioClip audioClip)
  {
    if (!((Object) audioClip != (Object) null))
      return;
    this.currentBSO = this.BSO = audioClip;
    if (this.coFadeBSO != null)
      this.StopCoroutine(this.coFadeBSO);
    this.coFadeBSO = this.StartCoroutine(this.FadeSound(this._audioSourceBSO, this.BSO, this.maxVolumeBSO));
  }

  public void FadeOutBSO()
  {
    if (this.coFadeBSO != null)
      this.StopCoroutine(this.coFadeBSO);
    this.coFadeBSO = this.StartCoroutine(this.FadeSound(this._audioSourceBSO, maxVolume: this.maxVolumeBSO));
  }

  public IEnumerator FadeSound(AudioSource audioSource, AudioClip AC = null, float maxVolume = 1f)
  {
    float fadeTimeOut = 0.3f;
    float fadeTimeIn = 0.3f;
    float startVolume = audioSource.volume;
    while ((double) audioSource.volume > 0.0)
    {
      audioSource.volume -= startVolume * Time.deltaTime / fadeTimeOut;
      yield return (object) null;
    }
    audioSource.Stop();
    if ((Object) AC == (Object) null)
    {
      audioSource.clip = (AudioClip) null;
    }
    else
    {
      audioSource.clip = AC;
      startVolume = 0.2f;
      audioSource.volume = 0.0f;
      audioSource.Play();
      while ((double) audioSource.volume < (double) this.maxVolumeBSO)
      {
        audioSource.volume += startVolume * Time.deltaTime / fadeTimeIn;
        yield return (object) null;
      }
      audioSource.volume = this.maxVolumeBSO;
    }
  }

  public void StopBSO()
  {
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) nameof (StopBSO));
    this.coFadeBSO = this.StartCoroutine(this.FadeSound(this._audioSourceBSO));
  }

  public void StopBSOInstant() => this._audioSourceBSO.Stop();

  private void GenerateLibrary()
  {
    this.audioLibrary = new Dictionary<string, AudioClip>();
    for (int index = 0; index < this.audioArray.Length; ++index)
    {
      if ((Object) this.audioArray[index] != (Object) null && !this.audioLibrary.ContainsKey(this.audioArray[index].name))
        this.audioLibrary.Add(this.audioArray[index].name, this.audioArray[index]);
    }
    this.audioLibraryNew = new Dictionary<string, AudioClip>();
    for (int index = 0; index < this.audioArrayNew.Length; ++index)
    {
      if ((Object) this.audioArrayNew[index] != (Object) null && !this.audioLibraryNew.ContainsKey(this.audioArrayNew[index].name))
        this.audioLibraryNew.Add(this.audioArrayNew[index].name, this.audioArrayNew[index]);
    }
    this.ambienceLibrary = new Dictionary<string, AudioClip>();
    for (int index = 0; index < this.ambienceArray.Length; ++index)
    {
      if ((Object) this.ambienceArray[index] != (Object) null && !this.ambienceLibrary.ContainsKey(this.ambienceArray[index].name))
        this.ambienceLibrary.Add(this.ambienceArray[index].name, this.ambienceArray[index]);
    }
  }

  public AudioSource AudioSource
  {
    get => this._audioSource;
    set => this._audioSource = value;
  }
}
