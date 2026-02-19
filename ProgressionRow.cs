using System.Text;
using TMPro;
using UnityEngine;

public class ProgressionRow : MonoBehaviour
{
	public TMP_Text title;

	public TMP_Text description;

	private int rank;

	private bool isIgnored;

	public int Rank => rank;

	private void Awake()
	{
		Enable(_state: false);
	}

	public void Enable(bool _state)
	{
		if (!isIgnored)
		{
			if (_state)
			{
				title.color = Functions.HexToColor("#F1D2A9");
				description.color = Functions.HexToColor("#F1D2A9");
				TMP_Text tMP_Text = title;
				FontStyles fontStyle = (description.fontStyle = FontStyles.Bold);
				tMP_Text.fontStyle = fontStyle;
			}
			else
			{
				title.color = Functions.HexToColor("#A0A0A0");
				description.color = Functions.HexToColor("#A0A0A0");
				TMP_Text tMP_Text2 = title;
				FontStyles fontStyle = (description.fontStyle = FontStyles.Normal);
				tMP_Text2.fontStyle = fontStyle;
			}
		}
	}

	public void Init(int _rank)
	{
		InitRank(_rank);
		Enable(_state: false);
	}

	public void InitRank(int _rank)
	{
		title.text = Globals.Instance.RankLevel[_rank].ToString();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("rankDescription");
		stringBuilder.Append(_rank);
		description.text = Texts.Instance.GetText(stringBuilder.ToString());
	}
}
