using System.Collections;
using UnityEngine;

public class MapLegend : MonoBehaviour
{
	public Transform info;

	public Transform legend;

	private RectTransform rt;

	private RectTransform rtLegend;

	private Coroutine co;

	private Vector3 posEnd;

	private Vector3 posIni;

	private Vector3 step;

	private float legendOffset;

	private bool open;

	private void Awake()
	{
		rt = GetComponent<RectTransform>();
	}

	private void Start()
	{
		rtLegend = legend.GetComponent<RectTransform>();
		legendOffset = rtLegend.position.y;
		legend.gameObject.SetActive(value: false);
		Resize();
	}

	public void Resize()
	{
		posIni = new Vector3(0f, (0f - Globals.Instance.sizeH) * 0.5f - 0.58f * Globals.Instance.multiplierY, -2f);
		posEnd = posIni + new Vector3(0f, 1.72f * Globals.Instance.multiplierY, 0f);
		step = new Vector3(0f, Mathf.Abs(posIni.y - posEnd.y) * 0.1f, 0f);
		rt.position = posIni;
	}

	private void OnMouseEnter()
	{
		if (!MapManager.Instance.corruption.gameObject.activeSelf && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()))
		{
			ShowLegend(state: true);
		}
	}

	private void OnMouseOver()
	{
		if (!MapManager.Instance.corruption.gameObject.activeSelf && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()))
		{
			ShowLegend(state: true);
		}
	}

	private void ShowLegend(bool state)
	{
		if (state != open && !EventManager.Instance && (!state || MapManager.Instance.worldTransform.gameObject.activeSelf))
		{
			open = state;
			if (co != null)
			{
				StopCoroutine(co);
			}
			co = StartCoroutine(ShowLegendCo(state));
		}
	}

	private IEnumerator ShowLegendCo(bool state)
	{
		float destinationY;
		if (state)
		{
			info.gameObject.SetActive(value: false);
			legend.gameObject.SetActive(value: true);
			destinationY = posEnd.y - legendOffset;
			while (rtLegend.localPosition.y < destinationY)
			{
				rtLegend.localPosition += step;
				if (rtLegend.localPosition.y + step.y >= destinationY)
				{
					break;
				}
				yield return null;
			}
			open = true;
		}
		else
		{
			destinationY = posIni.y - legendOffset;
			while (rtLegend.localPosition.y > destinationY)
			{
				rtLegend.localPosition -= step;
				if (rtLegend.localPosition.y - step.y <= destinationY)
				{
					break;
				}
				yield return null;
			}
			info.gameObject.SetActive(value: true);
			legend.gameObject.SetActive(value: false);
			open = false;
		}
		rtLegend.localPosition = new Vector3(rtLegend.localPosition.x, destinationY, rtLegend.localPosition.z);
		yield return null;
	}

	private void OnMouseExit()
	{
		ShowLegend(state: false);
	}
}
