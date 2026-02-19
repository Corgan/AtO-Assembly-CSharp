using System;
using UnityEngine;

public class Floating : MonoBehaviour
{
	private float originalY;

	private float originalX;

	public float floatStrength = 0.03f;

	public bool moveX = true;

	public bool moveY = true;

	private bool locked = true;

	private float random;

	public bool floatingTimeRandom = true;

	private float posX;

	private float posY;

	private Vector3 destination;

	private void Start()
	{
		originalY = base.transform.position.y;
		originalX = base.transform.position.x;
		if (floatingTimeRandom)
		{
			random = UnityEngine.Random.Range(0f, 25f);
		}
		locked = false;
	}

	private void OnEnable()
	{
		originalX = base.transform.position.y;
		originalY = base.transform.position.y;
	}

	private void Update()
	{
		if (!locked && (moveX || moveY))
		{
			posX = base.transform.localPosition.x;
			posY = base.transform.localPosition.y;
			if (moveX)
			{
				posX += (float)Math.Sin(Time.time + random) * floatStrength * 0.1f;
			}
			if (moveY)
			{
				posY += (float)Math.Sin(Time.time + random) * floatStrength;
			}
			destination = new Vector3(posX, posY, base.transform.localPosition.z);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, destination, 0.5f);
		}
	}
}
