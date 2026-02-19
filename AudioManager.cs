using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public AudioSource AudioSource
	{
		get
		{
			return _audioSource;
		}
		set
		{
			_audioSource = value;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
		GenerateLibrary();
	}

	private void Start()
	{
		_audioSourceBSO.volume = maxVolumeBSO;
		_audioSourceBSO.loop = true;
	}

	public void StartStopBSO(bool status)
	{
		if (!GameManager.Instance.ConfigBackgroundMute)
		{
			_audioSourceBSO.mute = false;
		}
		else
		{
			_audioSourceBSO.mute = !status;
		}
	}

	public void StartStopAmbience(bool status)
	{
		if (!GameManager.Instance.ConfigBackgroundMute)
		{
			_audioSourceAmbience.mute = false;
		}
		else
		{
			_audioSourceAmbience.mute = !status;
		}
	}

	public void DoAmbience(string whatAmbience)
	{
		if (coFadeAmbience != null)
		{
			StopCoroutine(coFadeAmbience);
		}
		AudioClip aC = ambienceLibrary[whatAmbience];
		coFadeAmbience = StartCoroutine(FadeSound(_audioSourceAmbience, aC, maxVolumeAmbience));
	}

	public void StopAmbience()
	{
		if (coFadeAmbience != null)
		{
			StopCoroutine(coFadeAmbience);
		}
		coFadeAmbience = StartCoroutine(FadeSound(_audioSourceAmbience));
	}

	public void DoBSO(string whatBSO = "", AudioClip acBSO = null)
	{
		if (acBSO != null)
		{
			BSO = acBSO;
		}
		else
		{
			switch (whatBSO)
			{
			default:
				return;
			case "Game":
				BSO = BSO_Game;
				break;
			case "Town":
				BSO = BSO_Town;
				break;
			case "Combat":
				BSO = BSO_Combat;
				break;
			case "Event":
				BSO = BSO_Event;
				break;
			case "Map":
				BSO = BSO_Map;
				break;
			case "Craft":
				BSO = BSO_Craft;
				break;
			case "Rewards":
				BSO = BSO_Rewards;
				break;
			}
		}
		if (!(currentBSO == BSO))
		{
			currentBSO = BSO;
			if (coFadeBSO != null)
			{
				StopCoroutine(coFadeBSO);
			}
			coFadeBSO = StartCoroutine(FadeSound(_audioSourceBSO, BSO, maxVolumeBSO));
		}
	}

	public void DoBSOAudioClip(AudioClip audioClip)
	{
		if (audioClip != null)
		{
			currentBSO = (BSO = audioClip);
			if (coFadeBSO != null)
			{
				StopCoroutine(coFadeBSO);
			}
			coFadeBSO = StartCoroutine(FadeSound(_audioSourceBSO, BSO, maxVolumeBSO));
		}
	}

	public void FadeOutBSO()
	{
		if (coFadeBSO != null)
		{
			StopCoroutine(coFadeBSO);
		}
		coFadeBSO = StartCoroutine(FadeSound(_audioSourceBSO, null, maxVolumeBSO));
	}

	public IEnumerator FadeSound(AudioSource audioSource, AudioClip AC = null, float maxVolume = 1f)
	{
		float fadeTimeOut = 0.3f;
		float fadeTimeIn = 0.3f;
		float startVolume = audioSource.volume;
		while (audioSource.volume > 0f)
		{
			audioSource.volume -= startVolume * Time.deltaTime / fadeTimeOut;
			yield return null;
		}
		audioSource.Stop();
		if (AC == null)
		{
			audioSource.clip = null;
			yield break;
		}
		audioSource.clip = AC;
		startVolume = 0.2f;
		audioSource.volume = 0f;
		audioSource.Play();
		while (audioSource.volume < maxVolumeBSO)
		{
			audioSource.volume += startVolume * Time.deltaTime / fadeTimeIn;
			yield return null;
		}
		audioSource.volume = maxVolumeBSO;
	}

	public void StopBSO()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("StopBSO");
		}
		coFadeBSO = StartCoroutine(FadeSound(_audioSourceBSO));
	}

	public void StopBSOInstant()
	{
		_audioSourceBSO.Stop();
	}

	private void GenerateLibrary()
	{
		audioLibrary = new Dictionary<string, AudioClip>();
		for (int i = 0; i < audioArray.Length; i++)
		{
			if (audioArray[i] != null && !audioLibrary.ContainsKey(audioArray[i].name))
			{
				audioLibrary.Add(audioArray[i].name, audioArray[i]);
			}
		}
		audioLibraryNew = new Dictionary<string, AudioClip>();
		for (int j = 0; j < audioArrayNew.Length; j++)
		{
			if (audioArrayNew[j] != null && !audioLibraryNew.ContainsKey(audioArrayNew[j].name))
			{
				audioLibraryNew.Add(audioArrayNew[j].name, audioArrayNew[j]);
			}
		}
		ambienceLibrary = new Dictionary<string, AudioClip>();
		for (int k = 0; k < ambienceArray.Length; k++)
		{
			if (ambienceArray[k] != null && !ambienceLibrary.ContainsKey(ambienceArray[k].name))
			{
				ambienceLibrary.Add(ambienceArray[k].name, ambienceArray[k]);
			}
		}
	}
}
