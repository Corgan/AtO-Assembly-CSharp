using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PulsatingGlow : MonoBehaviour
{
	public Material glowMaterial;

	public Color glowColor = Color.cyan;

	[Range(0f, 1f)]
	public float minAlpha = 0.2f;

	[Range(0f, 1f)]
	public float maxAlpha = 0.6f;

	public float pulseSpeed = 2f;

	public float glowSize = 1f;

	private SpriteRenderer sr;

	private Material runtimeMat;

	private float timeOffset;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		runtimeMat = new Material(glowMaterial);
		sr.material = runtimeMat;
		runtimeMat.SetColor("_GlowColor", glowColor);
		runtimeMat.SetFloat("_GlowSize", glowSize);
		timeOffset = Random.Range(0f, 100f);
	}

	private void Update()
	{
		float value = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed + timeOffset) + 1f) / 2f);
		runtimeMat.SetFloat("_GlowOpacity", value);
		runtimeMat.SetColor("_GlowColor", glowColor);
	}
}
