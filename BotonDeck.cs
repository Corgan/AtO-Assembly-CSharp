using UnityEngine;

public class BotonDeck : MonoBehaviour
{
	public SpriteRenderer backgroundSR;

	public SpriteRenderer cardSR;

	public int index;

	public void Show(bool state)
	{
		base.gameObject.SetActive(state);
	}

	private void OnMouseExit()
	{
		backgroundSR.color = new Color(1f, 0f, 0f);
		cardSR.color = new Color(1f, 1f, 1f);
	}

	private void OnMouseEnter()
	{
		backgroundSR.color = new Color(0.3f, 0.3f, 0.3f);
		cardSR.color = new Color(0.7f, 0.7f, 0.7f);
	}
}
