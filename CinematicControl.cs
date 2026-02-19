using UnityEngine;

public class CinematicControl : MonoBehaviour
{
	public void SetTextIntro(int _textIndex)
	{
		CinematicManager.Instance.DoText("intro", _textIndex, 0);
	}

	public void SetTextIntroBig(int _textIndex)
	{
		CinematicManager.Instance.DoText("intro", _textIndex, 1);
	}

	public void SetTextOutro(int _textIndex)
	{
		CinematicManager.Instance.DoText("outro", _textIndex, 0);
	}

	public void SetTextOutroBig(int _textIndex)
	{
		CinematicManager.Instance.DoText("outro", _textIndex, 1);
	}
}
