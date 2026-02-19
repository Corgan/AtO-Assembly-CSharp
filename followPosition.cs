using UnityEngine;

public class followPosition : MonoBehaviour
{
	[SerializeField]
	private Transform sourcePosition;

	private void Update()
	{
		if (!(sourcePosition == null))
		{
			base.transform.position = sourcePosition.position;
		}
	}
}
