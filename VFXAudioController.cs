using UnityEngine;

public class VFXAudioController : MonoBehaviour
{
	public AudioSource AudioSource;

	private void Awake()
	{
		if (AudioSource != null)
		{
			AudioSource.volume = AudioSource.volume * SaveManager.LoadPrefsFloat("masterVolume") * SaveManager.LoadPrefsFloat("effectsVolume");
		}
	}
}
