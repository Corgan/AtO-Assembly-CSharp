using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOpacityPulsator : MonoBehaviour
{
	[Header("Pulse Settings")]
	public float duration = 1f;

	[Range(0f, 1f)]
	public float minOpacity = 0.2f;

	[Range(0f, 1f)]
	public float maxOpacity = 0.8f;

	private SpriteRenderer sr;

	private float timer;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		timer = Random.Range(0f, duration);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		float t = Mathf.PingPong(timer / duration, 1f);
		float a = Mathf.Lerp(minOpacity, maxOpacity, t);
		Color color = sr.color;
		color.a = a;
		sr.color = color;
	}
}
