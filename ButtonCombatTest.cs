using UnityEngine;
using UnityEngine.UI;

public class ButtonCombatTest : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(OnTestCombatButtonClickEvent);
	}

	private void OnTestCombatButtonClickEvent()
	{
		bool flag = true;
		if (!PlayerManager.Instance.IsNodeUnlocked("town"))
		{
			flag = false;
		}
		if (!flag)
		{
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialNeedComplete"));
			return;
		}
		AtOManager.Instance.IsCombatTool = true;
		PlayerManager.Instance.UnlockAll();
		CombatToolManager.Instance.LoadCombatToolConfig();
		SandboxManager.Instance.SbReset();
		SandboxManager.Instance.SaveValuesToAtOManager();
		SandboxManager.Instance.SetCombatToolCombo();
		SceneStatic.LoadByName("HeroSelection");
	}
}
