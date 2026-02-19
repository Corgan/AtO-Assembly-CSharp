using UnityEngine;

public class EventBag : MonoBehaviour
{
	private void OnMouseDown()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()))
		{
			PlayerUIManager.Instance.BagToggle();
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()))
		{
			PopupManager.Instance.SetText(Texts.Instance.GetText("eventBag"), fast: true, "", alwaysCenter: true);
		}
	}

	private void OnMouseExit()
	{
		PopupManager.Instance.ClosePopup();
	}
}
