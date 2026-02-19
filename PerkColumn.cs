using TMPro;
using UnityEngine;

public class PerkColumn : MonoBehaviour
{
	public TMP_Text columnTitle;

	public Transform[] perkTransform;

	private PerkColumnItem[] perkColumnItem;

	public Transform lockIcon;

	public Transform disableMask;

	private void AwakeInit()
	{
		perkColumnItem = new PerkColumnItem[perkTransform.Length];
		for (int i = 0; i < perkTransform.Length; i++)
		{
			perkColumnItem[i] = perkTransform[i].GetComponent<PerkColumnItem>();
		}
	}

	public void Init(string _className, string _heroName, int _index, int _pointsAvailable, int _pointsUsed)
	{
		if (perkColumnItem == null)
		{
			AwakeInit();
		}
		for (int i = 0; i < perkColumnItem.Length; i++)
		{
			perkColumnItem[i].SetPerk(_className, _heroName, _index, i, _pointsAvailable, _pointsUsed);
		}
	}

	public void DisableColumn(bool state)
	{
		lockIcon.gameObject.SetActive(state);
		disableMask.gameObject.SetActive(state);
		if (perkColumnItem == null)
		{
			AwakeInit();
		}
		for (int i = 0; i < perkColumnItem.Length; i++)
		{
			perkColumnItem[i].EnablePerk(!state);
		}
	}
}
