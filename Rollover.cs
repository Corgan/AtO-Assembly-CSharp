using UnityEngine;

public class Rollover : MonoBehaviour
{
	public AudioClip rollOverSound;

	public void PlayRollOver()
	{
		if (!AlertManager.Instance.IsActive())
		{
			GameManager.Instance.PlayAudio(rollOverSound);
		}
	}

	private void OnMouseEnter()
	{
		if (AlertManager.Instance == null && rollOverSound != null)
		{
			BotonGeneric component = base.transform.GetComponent<BotonGeneric>();
			if (!(component != null) || component.buttonEnabled)
			{
				PlayRollOver();
			}
		}
	}
}
