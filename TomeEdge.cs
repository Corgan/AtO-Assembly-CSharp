using System.Collections;
using UnityEngine;

public class TomeEdge : MonoBehaviour
{
	private GameObject arrow;

	private SpriteRenderer arrowSprite;

	private SpriteRenderer arrowBg;

	private Coroutine moveCo;

	public bool isPrev;

	private void Start()
	{
		arrow = base.transform.GetChild(0).transform.gameObject;
		arrowBg = arrow.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
		arrowSprite = arrow.transform.GetChild(1).transform.GetComponent<SpriteRenderer>();
		SpriteRenderer spriteRenderer = arrowBg;
		Color color = (arrowSprite.color = new Color(1f, 1f, 1f, 0f));
		spriteRenderer.color = color;
	}

	public void Show()
	{
		if (moveCo != null)
		{
			StopCoroutine(moveCo);
		}
		moveCo = StartCoroutine(ShowCo());
	}

	private IEnumerator ShowCo()
	{
		Color col = arrowBg.color;
		arrow.SetActive(value: true);
		while (col.a < 0.5f)
		{
			col.a += 0.05f;
			arrowBg.color = col;
			arrowSprite.color = col;
			yield return Globals.Instance.WaitForSeconds(0.025f);
		}
	}

	public void Hide()
	{
		if (moveCo != null)
		{
			StopCoroutine(moveCo);
		}
		moveCo = StartCoroutine(HideCo());
	}

	private IEnumerator HideCo()
	{
		Color col = arrowBg.color;
		while (col.a > 0f)
		{
			col.a -= 0.05f;
			arrowBg.color = col;
			arrowSprite.color = col;
			yield return Globals.Instance.WaitForSeconds(0.025f);
		}
		arrow.SetActive(value: false);
	}

	private void OnMouseEnter()
	{
		if (isPrev)
		{
			if (TomeManager.Instance.IsTherePrev())
			{
				Show();
			}
		}
		else if (TomeManager.Instance.IsThereNext())
		{
			Show();
		}
	}

	private void OnMouseExit()
	{
		Hide();
	}

	private void OnMouseUp()
	{
		if (isPrev)
		{
			if (TomeManager.Instance.IsTherePrev())
			{
				if (TomeManager.Instance.IsFirstPage())
				{
					Hide();
				}
				TomeManager.Instance.DoPrevPage();
			}
		}
		else if (TomeManager.Instance.IsThereNext())
		{
			if (TomeManager.Instance.IsLastPage())
			{
				Hide();
			}
			TomeManager.Instance.DoNextPage();
		}
	}
}
