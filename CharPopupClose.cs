using UnityEngine;

public class CharPopupClose : MonoBehaviour
{
	public CharPopup charPopup;

	public GameObject closeRollver;

	private void OnMouseEnter()
	{
		closeRollver.gameObject.SetActive(value: true);
	}

	private void OnMouseExit()
	{
		closeRollver.gameObject.SetActive(value: false);
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform))
		{
			charPopup.Close();
		}
	}
}
