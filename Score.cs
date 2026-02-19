using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
	public Transform icon;

	public TMP_Text index;

	public new TMP_Text name;

	public TMP_Text score;

	public Transform sep;

	private Color indexColor;

	private Color nameColor;

	private void Awake()
	{
		indexColor = index.color;
		nameColor = name.color;
	}

	public void SetScore(int _index, string _name, int _score = -1, ulong _userId = 999999999999999uL)
	{
		if (_index > 0)
		{
			index.text = _index.ToString();
			name.text = _name;
			score.text = Functions.ScoreFormat(_score);
			sep.gameObject.SetActive(value: true);
			if (_userId != (ulong)SteamManager.Instance.steamId)
			{
				icon.gameObject.SetActive(value: false);
				name.color = nameColor;
			}
			else
			{
				icon.gameObject.SetActive(value: true);
				name.color = indexColor;
				TomeManager.Instance.playerOnScoreboard = true;
			}
		}
		else
		{
			index.text = "";
			name.text = _name;
			score.text = "";
			sep.gameObject.SetActive(value: false);
		}
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
