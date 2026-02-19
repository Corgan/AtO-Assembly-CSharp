using UnityEngine;

public class botTownUpgrades : MonoBehaviour
{
	public SpriteRenderer bgRenderer;

	private void Awake()
	{
		base.gameObject.SetActive(!AtOManager.Instance.IsCombatTool);
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			base.transform.localScale = new Vector3(0.52f, 0.52f, 1f);
			bgRenderer.color = new Color(0.7f, 0.7f, 0.7f);
			GameManager.Instance.PlayLibraryAudio("ui_click");
		}
	}

	private void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			base.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
			bgRenderer.color = Color.white;
		}
	}

	private void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform) && !AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			if (AtOManager.Instance.TownTutorialStep > -1 && AtOManager.Instance.TownTutorialStep < 3)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
			}
			else
			{
				TownManager.Instance.ShowTownUpgrades(state: true);
			}
		}
	}
}
