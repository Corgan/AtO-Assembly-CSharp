using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	public enum Axis
	{
		X,
		Y,
		Z
	}

	[Header("Rotation Settings")]
	public Axis rotationAxis = Axis.Z;

	public float speed = 30f;

	public bool clockwise = true;

	private void Update()
	{
		float num = (clockwise ? speed : (0f - speed));
		Vector3 vector = Vector3.zero;
		switch (rotationAxis)
		{
		case Axis.X:
			vector = new Vector3(num, 0f, 0f);
			break;
		case Axis.Y:
			vector = new Vector3(0f, num, 0f);
			break;
		case Axis.Z:
			vector = new Vector3(0f, 0f, num);
			break;
		}
		base.transform.Rotate(vector * Time.deltaTime);
	}
}
