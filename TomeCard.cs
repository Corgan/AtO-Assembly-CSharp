using UnityEngine;

public class TomeCard : MonoBehaviour
{
	public Transform buttonGold;

	public Transform buttonBlue;

	public Transform buttonRare;

	public void ShowButtons(bool state)
	{
		if (buttonGold.gameObject.activeSelf != state)
		{
			buttonGold.gameObject.SetActive(state);
		}
		if (buttonBlue.gameObject.activeSelf != state)
		{
			buttonBlue.gameObject.SetActive(state);
		}
	}

	public void ShowButtonRare(bool state)
	{
		if (buttonRare.gameObject.activeSelf != state)
		{
			buttonRare.gameObject.SetActive(state);
		}
	}
}
