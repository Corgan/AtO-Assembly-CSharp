using TMPro;
using UnityEngine;

public class EmoteSmall : MonoBehaviour
{
	public TMP_Text letter;

	public SpriteRenderer icon;

	public Transform background;

	public Transform backgroundHover;

	public Transform backgroundBlocked;

	public int action;

	private bool blocked;

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		backgroundHover.gameObject.SetActive(value: false);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void SetSprite(Sprite _sprite)
	{
		icon.sprite = _sprite;
	}

	public void SetAction(int _action)
	{
		action = _action;
		icon.sprite = MatchManager.Instance.emoteManager.emotesSprite[_action];
		switch (_action)
		{
		case 0:
			letter.text = "R";
			break;
		case 1:
			letter.text = "E";
			break;
		case 2:
			letter.text = "S";
			break;
		case 3:
			letter.text = "A";
			break;
		case 4:
			letter.text = "W";
			break;
		case 5:
			letter.text = "Q";
			break;
		}
	}

	public void SetBlocked(bool _state)
	{
		blocked = _state;
		backgroundBlocked.gameObject.SetActive(_state);
	}

	public void OnMouseEnter()
	{
		if ((bool)MatchManager.Instance)
		{
			MatchManager.Instance.emoteManager.ShowEmotes();
			if (!blocked)
			{
				backgroundHover.gameObject.SetActive(value: true);
			}
		}
	}

	public void OnMouseExit()
	{
		if ((bool)MatchManager.Instance)
		{
			MatchManager.Instance.emoteManager.HideEmotesCo();
			backgroundHover.gameObject.SetActive(value: false);
		}
	}

	public void OnMouseUp()
	{
		if (!blocked && (bool)MatchManager.Instance)
		{
			MatchManager.Instance.SetCharactersPing(action);
		}
	}
}
