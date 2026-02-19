using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DamageMeterManager : MonoBehaviour
{
	public Transform panel;

	public CharacterMeter[] characterMeter;

	public TMP_Text[] statsTotal;

	public Transform panelElements;

	public Transform detailedData;

	public Transform buttonClose;

	public Transform banners;

	private bool opened = true;

	public int[,] combatStatsOld;

	public int[,] combatStatsCurrentOld;

	public static DamageMeterManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		Hide();
	}

	public bool IsActive()
	{
		return panel.gameObject.activeSelf;
	}

	public void ShowHide()
	{
		if (opened)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	public void SaveATOStats()
	{
		if (AtOManager.Instance.combatStats != null)
		{
			combatStatsOld = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
			for (int i = 0; i < AtOManager.Instance.combatStats.GetLength(0); i++)
			{
				for (int j = 0; j < AtOManager.Instance.combatStats.GetLength(1); j++)
				{
					combatStatsOld[i, j] = AtOManager.Instance.combatStats[i, j];
				}
			}
		}
		if (AtOManager.Instance.combatStatsCurrent == null)
		{
			return;
		}
		combatStatsCurrentOld = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
		for (int k = 0; k < AtOManager.Instance.combatStatsCurrent.GetLength(0); k++)
		{
			for (int l = 0; l < AtOManager.Instance.combatStatsCurrent.GetLength(1); l++)
			{
				combatStatsCurrentOld[k, l] = AtOManager.Instance.combatStatsCurrent[k, l];
			}
		}
	}

	public void RestoreATOStats()
	{
		if (combatStatsOld != null)
		{
			AtOManager.Instance.combatStats = new int[combatStatsOld.GetLength(0), combatStatsOld.GetLength(1)];
			for (int i = 0; i < combatStatsOld.GetLength(0); i++)
			{
				for (int j = 0; j < combatStatsOld.GetLength(1); j++)
				{
					AtOManager.Instance.combatStats[i, j] = combatStatsOld[i, j];
				}
			}
		}
		if (combatStatsCurrentOld == null)
		{
			return;
		}
		AtOManager.Instance.combatStatsCurrent = new int[combatStatsCurrentOld.GetLength(0), combatStatsCurrentOld.GetLength(1)];
		for (int k = 0; k < combatStatsCurrentOld.GetLength(0); k++)
		{
			for (int l = 0; l < combatStatsCurrentOld.GetLength(1); l++)
			{
				AtOManager.Instance.combatStatsCurrent[k, l] = combatStatsCurrentOld[k, l];
			}
		}
	}

	public void Show(int[,] _combatStats = null)
	{
		opened = true;
		panel.gameObject.SetActive(value: true);
		if (AtOManager.Instance.combatStats != null && AtOManager.Instance.combatStats.GetLength(1) == 10)
		{
			detailedData.gameObject.SetActive(value: false);
			for (int i = 5; i < 12; i++)
			{
				statsTotal[i].gameObject.SetActive(value: false);
			}
		}
		else
		{
			detailedData.gameObject.SetActive(value: true);
			for (int j = 5; j < 12; j++)
			{
				statsTotal[j].gameObject.SetActive(value: true);
			}
		}
		if (AtOManager.Instance.combatStats.GetLength(1) > 10)
		{
			for (int k = 0; k < AtOManager.Instance.combatStats.GetLength(0); k++)
			{
				AtOManager.Instance.combatStats[k, 5] = AtOManager.Instance.combatStats[k, 0];
				if (AtOManager.Instance.combatStatsCurrent != null)
				{
					AtOManager.Instance.combatStatsCurrent[k, 5] = AtOManager.Instance.combatStatsCurrent[k, 0];
				}
				for (int l = 6; l < AtOManager.Instance.combatStats.GetLength(1); l++)
				{
					AtOManager.Instance.combatStats[k, 5] -= AtOManager.Instance.combatStats[k, l];
					if (AtOManager.Instance.combatStatsCurrent != null && AtOManager.Instance.combatStatsCurrent.GetLength(1) > 10)
					{
						AtOManager.Instance.combatStatsCurrent[k, 5] -= AtOManager.Instance.combatStatsCurrent[k, l];
					}
				}
			}
		}
		DoStats();
	}

	public void Hide()
	{
		StartCoroutine(HideCo());
	}

	private IEnumerator HideCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		opened = false;
		panel.gameObject.SetActive(value: false);
	}

	public void DoStats()
	{
		for (int i = 0; i < characterMeter.Length; i++)
		{
			characterMeter[i].DoStats(i);
		}
		if (TomeManager.Instance.IsActive())
		{
			RestoreATOStats();
		}
	}

	public void SetTotal(int index, int total)
	{
		statsTotal[index].text = total.ToString();
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
		Vector2 position = buttonClose.position + new Vector3(0f, 70f, 0f);
		Mouse.current.WarpCursorPosition(position);
	}
}
