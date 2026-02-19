using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupNodeCombatTool : MonoBehaviour
{
	public TMP_Text Title;

	public Image[] CharIcon;

	public GameObject SelectedIcon;

	private CombatData CombatData;

	public Image LockIcon;

	public Image CombatIcon;

	public void NodeSelected()
	{
		if (CombatToolManager.Instance.LastCombatButtonSelected != null && CombatToolManager.Instance.LastCombatButtonSelected.SelectedIcon != null)
		{
			CombatToolManager.Instance.LastCombatButtonSelected.SelectedIcon.gameObject.SetActive(value: false);
		}
		CombatToolManager.Instance.CurrentCombat = CombatData;
		SelectedIcon.SetActive(value: true);
		CombatToolManager.Instance.LastCombatButtonSelected = this;
		CombatToolManager.Instance.ConfirmButton.interactable = true;
	}

	public void SetCombatData(CombatData combat)
	{
		CombatData = combat;
	}

	public void SetTitle(string name)
	{
		Title.text = name;
	}

	public void SetIcon(Sprite icon, int index)
	{
		CharIcon[index].GetComponent<Image>().enabled = true;
		CharIcon[index].sprite = icon;
	}
}
