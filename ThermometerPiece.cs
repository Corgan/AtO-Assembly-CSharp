using UnityEngine;
using UnityEngine.EventSystems;

public class ThermometerPiece : MonoBehaviour
{
	public int piece;

	public int round;

	private ThermometerTierData thermometerTierData;

	private ThermometerData thermometerData;

	private SpriteRenderer spr;

	private Color oriColor;

	private int pieceRound;

	private void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		Init(round);
	}

	public void SetThermometerTierData(ThermometerTierData _thermometerTierData)
	{
		thermometerTierData = _thermometerTierData;
	}

	public void SetThermometerData(ThermometerData _thermometerData)
	{
		thermometerData = _thermometerData;
	}

	public void Init(int _currentRound)
	{
		if (thermometerTierData != null)
		{
			thermometerData = null;
			round = _currentRound;
			pieceRound = round + piece - 2;
			if (pieceRound > 0)
			{
				for (int i = 0; i < thermometerTierData.RoundThermometer.Length; i++)
				{
					if (pieceRound >= thermometerTierData.RoundThermometer[i].Round)
					{
						thermometerData = thermometerTierData.RoundThermometer[i].ThermometerData;
						if (MadnessManager.Instance.IsMadnessTraitActive("equalizer") && thermometerTierData != null && thermometerTierData.RoundThermometer[1] != null)
						{
							thermometerData = Object.Instantiate(thermometerTierData.RoundThermometer[1].ThermometerData);
							thermometerData.ThermometerId = thermometerTierData.RoundThermometer[i].ThermometerData.ThermometerId;
							thermometerData.ThermometerColor = thermometerTierData.RoundThermometer[i].ThermometerData.ThermometerColor;
						}
						if (pieceRound == thermometerTierData.RoundThermometer[i].Round)
						{
							break;
						}
					}
				}
			}
			if (thermometerData == null)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			base.gameObject.SetActive(value: true);
			spr.color = thermometerData.ThermometerColor;
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnMouseEnter()
	{
		if (!MatchManager.Instance.CardDrag && !EventSystem.current.IsPointerOverGameObject())
		{
			oriColor = spr.color;
			spr.color = new Color(oriColor.r - 0.15f, oriColor.g - 0.15f, oriColor.b - 0.15f, 1f);
			PopupManager.Instance.SetText(Functions.ThermometerTextForPopup(thermometerData), fast: true, "followdown", alwaysCenter: true);
			MatchManager.Instance.AdjustRoundForThermoDisplay(piece, pieceRound, oriColor);
		}
	}

	private void OnMouseExit()
	{
		if (!MatchManager.Instance.CardDrag && !EventSystem.current.IsPointerOverGameObject())
		{
			spr.color = oriColor;
			PopupManager.Instance.ClosePopup();
			MatchManager.Instance.AdjustRoundForThermoDisplay(2, 0, new Color(1f, 1f, 1f, 1f));
		}
	}
}
