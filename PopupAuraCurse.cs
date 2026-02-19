using UnityEngine;

public class PopupAuraCurse : MonoBehaviour
{
	private AuraCurseData acData;

	private int charges;

	private bool fast;

	public void SetAuraCurse(AuraCurseData _acData, int _charges, bool _fast)
	{
		acData = _acData;
		charges = _charges;
		fast = _fast;
	}

	private void OnMouseEnter()
	{
		if (acData != null)
		{
			PopupManager.Instance.SetAuraCurse(base.transform, acData.Id, charges.ToString(), fast);
		}
	}

	private void OnMouseExit()
	{
		PopupManager.Instance.ClosePopup();
	}
}
