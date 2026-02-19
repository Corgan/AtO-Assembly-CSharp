using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
	private SpriteRenderer sr;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	public void Stop()
	{
	}
}
