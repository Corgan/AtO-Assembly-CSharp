using UnityEngine;
using UnityEngine.UI;

public class CycleColor : MonoBehaviour
{
	private float timeLeft;

	private Color targetColor = new Color(1f, 1f, 1f);

	private Image img;

	private void Awake()
	{
		img = GetComponent<Image>();
	}

	private void Update()
	{
		if (timeLeft <= Time.deltaTime)
		{
			img.color = targetColor;
			targetColor = new Color(Random.value, Random.value, Random.value);
			while (targetColor.r < 0.2f && targetColor.g < 0.2f && targetColor.b < 0.2f)
			{
				targetColor = new Color(Random.value, Random.value, Random.value);
			}
			timeLeft = 10f;
		}
		else
		{
			img.color = Color.Lerp(img.color, targetColor, Time.deltaTime / timeLeft);
			timeLeft -= Time.deltaTime;
		}
	}
}
