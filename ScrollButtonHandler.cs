using UnityEngine;
using UnityEngine.UI;

public class ScrollButtonHandler : MonoBehaviour
{
	public ScrollRect targetScrollRect;

	public float scrollStep = 0.1f;

	[SerializeField]
	private Transform scrollUpButton;

	[SerializeField]
	private Transform scrollDownButton;

	public void ScrollUp()
	{
		if (targetScrollRect != null)
		{
			targetScrollRect.verticalNormalizedPosition = Mathf.Min(1f, targetScrollRect.verticalNormalizedPosition + scrollStep);
		}
	}

	public void ScrollDown()
	{
		if (targetScrollRect != null)
		{
			targetScrollRect.verticalNormalizedPosition = Mathf.Max(0f, targetScrollRect.verticalNormalizedPosition - scrollStep);
		}
	}

	private void Awake()
	{
		scrollDownButton.gameObject.SetActive(value: false);
		scrollUpButton.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (targetScrollRect.content.rect.height > targetScrollRect.viewport.rect.height)
		{
			scrollDownButton.gameObject.SetActive(targetScrollRect.verticalNormalizedPosition > 0.03f);
			scrollUpButton.gameObject.SetActive(targetScrollRect.verticalNormalizedPosition < 0.97f);
		}
	}
}
