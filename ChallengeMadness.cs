using UnityEngine;

public class ChallengeMadness : MonoBehaviour
{
	public SpriteRenderer background;

	public SpriteRenderer icon;

	public void SetBackground(string _color)
	{
		background.color = Functions.HexToColor(_color);
	}

	public void SetDisable()
	{
		SetBackground("#353535");
	}

	public void SetActive()
	{
		SetBackground("#AD844D");
	}

	public void SetDefault()
	{
		SetBackground("#5D3578");
	}

	public void SetIcon(Sprite _sprite)
	{
		icon.sprite = _sprite;
	}
}
