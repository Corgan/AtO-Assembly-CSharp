using UnityEngine;

public class ItemsWindowUI : MonoBehaviour
{
	private void Start()
	{
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}
}
