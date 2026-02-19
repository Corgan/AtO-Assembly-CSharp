using System.Collections;
using UnityEngine;

public class RandomHeroSelector : MonoBehaviour
{
	public int boxId;

	private SpriteRenderer diceSpr;

	private Color colorOver = new Color(0.5f, 0.5f, 0.5f, 1f);

	private Color colorOut = new Color(1f, 1f, 1f, 1f);

	private Vector3 oriScale;

	private Vector3 maxScale;

	private Coroutine selectionCo;

	private bool selecting;

	private void Awake()
	{
		diceSpr = GetComponent<SpriteRenderer>();
		oriScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
		maxScale = oriScale + new Vector3(0.1f, 0.1f, 0f);
	}

	public void SetId(int _id)
	{
		boxId = _id;
	}

	private void OnMouseEnter()
	{
		diceSpr.color = colorOver;
		base.transform.localScale = maxScale;
	}

	private void OnMouseExit()
	{
		diceSpr.color = colorOut;
		base.transform.localScale = oriScale;
	}

	public void OnMouseUp()
	{
		if (selectionCo == null)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
			selectionCo = StartCoroutine(SelectRandomHero());
		}
	}

	private IEnumerator SelectRandomHero()
	{
		HeroSelectionManager.Instance.SetRandomHero(boxId);
		selecting = true;
		yield return Globals.Instance.WaitForSeconds(0.3f);
		selecting = false;
		selectionCo = null;
	}
}
