using UnityEngine;

public class DissolveController : MonoBehaviour
{
	public float dissolveAmount;

	public float dissolveSpeed;

	public bool isDissolving;

	[ColorUsage(true, true)]
	public Color outColor;

	[ColorUsage(true, true)]
	public Color inColor;

	private Material mat;

	private void Start()
	{
		mat = GetComponent<SpriteRenderer>().material;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			isDissolving = true;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			isDissolving = false;
		}
		if (isDissolving)
		{
			DissolveOut(dissolveSpeed, outColor);
		}
		if (!isDissolving)
		{
			DissolveIn(dissolveSpeed, inColor);
		}
		mat.SetFloat("_DissolveAmount", dissolveAmount);
	}

	public void DissolveOut(float speed, Color color)
	{
		mat.SetColor("_DissolveColor", color);
		if ((double)dissolveAmount > -0.1)
		{
			dissolveAmount -= Time.deltaTime * speed;
		}
	}

	public void DissolveIn(float speed, Color color)
	{
		mat.SetColor("_DissolveColor", color);
		if (dissolveAmount < 1f)
		{
			dissolveAmount += Time.deltaTime * dissolveSpeed;
		}
	}
}
