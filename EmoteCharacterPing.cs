using TMPro;
using UnityEngine;

public class EmoteCharacterPing : MonoBehaviour
{
	public TMP_Text letter;

	public SpriteRenderer icon;

	public Transform background;

	public Transform backgroundHover;

	public int action;

	private string characterId;

	private void Start()
	{
		Hide();
	}

	public void Show(string _id, int _action)
	{
		if ((bool)MatchManager.Instance)
		{
			characterId = _id;
			action = _action;
			icon.sprite = MatchManager.Instance.emoteManager.emotesSprite[_action];
			base.gameObject.SetActive(value: true);
			backgroundHover.gameObject.SetActive(value: false);
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void OnMouseEnter()
	{
		if ((bool)MatchManager.Instance)
		{
			backgroundHover.gameObject.SetActive(value: true);
		}
	}

	public void OnMouseExit()
	{
		if ((bool)MatchManager.Instance)
		{
			backgroundHover.gameObject.SetActive(value: false);
		}
	}

	public void OnMouseUp()
	{
		if ((bool)MatchManager.Instance)
		{
			MatchManager.Instance.EmoteTarget(characterId, action);
		}
	}
}
