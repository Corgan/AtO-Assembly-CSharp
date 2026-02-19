using TMPro;
using UnityEngine;

public class PerkBackgroundRow : MonoBehaviour
{
	public TMP_Text rowLevel;

	public TMP_Text rowReq;

	public Transform lockIcon;

	public Transform tabIcon;

	private PopupText popupText;

	private Color colorLevel = new Color(0.88f, 0.71f, 0.16f);

	private Color colorReq = new Color(0.63f, 0.48f, 0f);

	private Color colorDis = new Color(0.63f, 0.63f, 0.63f);

	public void SetRequired(int _value)
	{
		popupText = GetComponent<PopupText>();
		if (_value > 0)
		{
			rowReq.text = string.Format(Texts.Instance.GetText("requireSkill"), _value.ToString());
			popupText.text = string.Format(Texts.Instance.GetText("requiredPoints"), _value.ToString());
		}
		else
		{
			rowReq.text = "";
			ShowLockIcon(_state: false);
		}
	}

	public void ShowLockIcon(bool _state)
	{
		lockIcon.gameObject.SetActive(_state);
		if (_state)
		{
			rowLevel.gameObject.SetActive(value: false);
			tabIcon.gameObject.SetActive(value: false);
		}
		else
		{
			rowLevel.gameObject.SetActive(value: true);
			tabIcon.gameObject.SetActive(value: true);
		}
	}
}
