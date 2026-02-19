using System.Collections;
using UnityEngine;

public class OpacityOverLifetime : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private float waitBeforeSquence = 0.2f;

	[SerializeField]
	private float timeIncreaseOpacity = 1.3f;

	[SerializeField]
	private float waitBetweenSequence = 7f;

	[SerializeField]
	private float timeDecreaseOpacity = 1.2f;

	[SerializeField]
	private float maxOpacity = 0.7f;

	private float timeIncreaseOpacityStepSize;

	private float timeDecreaseOpacityStepSize;

	private void Start()
	{
		spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
		timeIncreaseOpacityStepSize = 1f / (timeIncreaseOpacity * 100f);
		timeDecreaseOpacityStepSize = 1f / (timeDecreaseOpacity * 100f);
		GameManager.Instance.StartCoroutine(OpacityLifetimeCo());
		Object.Destroy(this, 10f);
	}

	private IEnumerator OpacityLifetimeCo()
	{
		yield return new WaitForSeconds(waitBeforeSquence);
		float t = 0f;
		Color c = spriteRenderer.color;
		while (t < timeIncreaseOpacity)
		{
			t += Time.deltaTime;
			float a = Mathf.Lerp(0f, maxOpacity, t / timeIncreaseOpacity);
			spriteRenderer.color = new Color(c.r, c.g, c.b, a);
			yield return null;
		}
		yield return new WaitForSeconds(waitBetweenSequence);
		t = 0f;
		c = spriteRenderer.color;
		while (t < timeDecreaseOpacity)
		{
			t += Time.deltaTime;
			float a2 = Mathf.Lerp(maxOpacity, 0f, t / timeDecreaseOpacity);
			spriteRenderer.color = new Color(c.r, c.g, c.b, a2);
			yield return null;
		}
	}
}
