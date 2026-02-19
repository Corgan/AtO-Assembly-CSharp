using UnityEngine;

public class BotHeroChar : MonoBehaviour
{
	public bool isHeroStats;

	private SubClassData subClassData;

	private SpriteRenderer spr;

	public void SetSubClassData(SubClassData _subClassData)
	{
		subClassData = _subClassData;
	}

	private void Awake()
	{
		spr = GetComponent<BotonRollover>().image.GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		turnOffColor();
	}

	private void OnMouseEnter()
	{
		turnOnColor();
	}

	private void OnMouseExit()
	{
		turnOffColor();
	}

	private void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive())
		{
			if (isHeroStats)
			{
				HeroSelectionManager.Instance.charPopup.Init(subClassData);
				HeroSelectionManager.Instance.charPopup.Show();
			}
			else
			{
				PerkTree.Instance.Show(subClassData.Id);
			}
			turnOffColor();
		}
	}

	private void turnOffColor()
	{
		spr.color = new Color(0.8f, 0.8f, 0.8f, 1f);
	}

	private void turnOnColor()
	{
		spr.color = new Color(1f, 1f, 1f, 1f);
	}
}
