using UnityEngine;

public class CollidersRefresher : MonoBehaviour
{
	private void Start()
	{
		Collider[] array = Object.FindObjectsOfType<Collider>();
		foreach (Collider obj in array)
		{
			obj.enabled = !obj.enabled;
			obj.enabled = !obj.enabled;
		}
	}
}
