using UnityEngine;

public class FadingLight : MonoBehaviour
{
	private Light myLight;

	public float maxIntensity = 1f;

	public float minIntensity;

	public float pulseSpeed = 1f;

	private float targetIntensity = 1f;

	private float currentIntensity;

	private void Start()
	{
		myLight = GetComponent<Light>();
	}

	private void Update()
	{
		if (Time.frameCount % 3 == 0)
		{
			currentIntensity = Mathf.MoveTowards(myLight.intensity, targetIntensity, Time.deltaTime * pulseSpeed);
			if (currentIntensity >= maxIntensity)
			{
				currentIntensity = maxIntensity;
				targetIntensity = minIntensity;
			}
			else if (currentIntensity <= minIntensity)
			{
				currentIntensity = minIntensity;
				targetIntensity = maxIntensity;
			}
			myLight.intensity = currentIntensity;
		}
	}
}
