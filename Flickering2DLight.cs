using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flickering2DLight : MonoBehaviour
{
	public Light2D light2D;

	public float intensityMin = 0.8f;

	public float intensityMax = 1.2f;

	public float flickerSpeed = 0.1f;

	private void Start()
	{
		if (light2D == null)
		{
			light2D = GetComponent<Light2D>();
		}
		InvokeRepeating("Flicker", 0f, flickerSpeed);
	}

	private void Flicker()
	{
		light2D.intensity = Random.Range(intensityMin, intensityMax);
	}
}
