using UnityEngine;

public class SetSpriteLayerFromBase : MonoBehaviour
{
	public SpriteRenderer sprBase;

	public void ReOrderLayer()
	{
		if (!(sprBase == null))
		{
			if (GetComponent<SpriteRenderer>() != null)
			{
				SpriteRenderer component = GetComponent<SpriteRenderer>();
				component.sortingOrder = sprBase.sortingOrder + 1;
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, sprBase.transform.localPosition.z - 1f);
				component.sortingLayerName = sprBase.sortingLayerName;
			}
			if (GetComponent<ParticleSystemRenderer>() != null)
			{
				ParticleSystemRenderer component2 = GetComponent<ParticleSystemRenderer>();
				component2.sortingOrder = sprBase.sortingOrder + 1;
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, sprBase.transform.localPosition.z - 1f);
				component2.sortingLayerName = sprBase.sortingLayerName;
			}
		}
	}
}
