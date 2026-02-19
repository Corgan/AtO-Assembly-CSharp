using UnityEngine;
using UnityEngine.EventSystems;

public class TestingHeroBuild : MonoBehaviour
{
	[SerializeField]
	private int id;

	private void OnMouseUpAsButton()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			TeamManagement.Instance.ShowHeroBuildPanel(id);
		}
	}
}
