using System.Collections;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
	public Transform theMask;

	private Vector3 initialScale = new Vector3(0f, 0.33f, 1f);

	public void Init()
	{
		StartCoroutine(Animation());
	}

	private void Shake()
	{
	}

	private IEnumerator Animation()
	{
		float scaleY = 0.01f;
		while ((double)scaleY < 0.75)
		{
			theMask.localScale = new Vector3(1f, scaleY, 1f);
			scaleY += 0.035f;
			yield return new WaitForSeconds(0.01f);
		}
		yield return new WaitForSeconds(0.5f);
		while (scaleY > 0f)
		{
			theMask.localScale = new Vector3(1f, scaleY, 1f);
			scaleY -= 0.035f;
			yield return new WaitForSeconds(0.01f);
		}
		Object.Destroy(base.gameObject);
	}
}
