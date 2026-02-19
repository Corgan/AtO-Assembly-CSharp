using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
	public float horizontalResolution = 1920f;

	public float verticalResolution = 1080f;

	public bool resize = true;

	private float ortoDestine;

	private float ortoInit;

	private CameraShake cameraShake;

	private float lastScreenWidth;

	private float lastScreenHeight;

	private Coroutine zoomCoroutine;

	private void Awake()
	{
		cameraShake = GetComponent<CameraShake>();
		ortoInit = Camera.main.orthographicSize;
		Camera.main.orthographicSize = 5.4f;
	}

	private void Update()
	{
		if (Time.frameCount % 24 != 0 || (lastScreenWidth == (float)Screen.width && lastScreenHeight == (float)Screen.height))
		{
			return;
		}
		lastScreenWidth = Screen.width;
		lastScreenHeight = Screen.height;
		resize = true;
		if (resize)
		{
			float num = 1.7777778f;
			float num2 = (float)Screen.width / (float)Screen.height / num;
			Camera component = GetComponent<Camera>();
			if (num2 < 1f)
			{
				Rect rect = component.rect;
				rect.width = 1f;
				rect.height = num2;
				rect.x = 0f;
				rect.y = (1f - num2) / 2f;
				component.rect = rect;
			}
			else
			{
				float num3 = 1f / num2;
				Rect rect2 = component.rect;
				rect2.width = num3;
				rect2.height = 1f;
				rect2.x = (1f - num3) / 2f;
				rect2.y = 0f;
				component.rect = rect2;
			}
			if (GameManager.Instance != null)
			{
				GameManager.Instance.Resize();
			}
			resize = false;
		}
	}

	public void Shake()
	{
		if (GameManager.Instance.ConfigScreenShakeOption)
		{
			cameraShake.Shake();
		}
	}

	public void Zoom()
	{
		ortoDestine = Camera.main.orthographicSize - 0.15f;
		if (zoomCoroutine != null)
		{
			StopCoroutine(zoomCoroutine);
		}
		zoomCoroutine = StartCoroutine(ZoomCo(ortoInit, ortoDestine));
	}

	private IEnumerator ZoomCo(float source, float destine)
	{
		float step = Mathf.Abs(source - destine) / 10f;
		float index = source;
		if (source > destine)
		{
			while (index > destine)
			{
				index -= step;
				Camera.main.orthographicSize = index;
				yield return Globals.Instance.WaitForSeconds(0.005f);
			}
		}
		else
		{
			step = (destine - source) / 10f;
			while (index < destine)
			{
				index += step;
				Camera.main.orthographicSize = index;
				yield return Globals.Instance.WaitForSeconds(0.005f);
			}
		}
		Camera.main.orthographicSize = destine;
	}

	public void ZoomBack()
	{
		if (zoomCoroutine != null)
		{
			StopCoroutine(zoomCoroutine);
		}
		zoomCoroutine = StartCoroutine(ZoomCo(ortoDestine, ortoInit));
	}

	public void ZoomToTransform(Transform transformPosition)
	{
		ortoInit = Camera.main.orthographicSize;
		ortoDestine = Camera.main.orthographicSize - 2f;
		StartCoroutine(ZoomToTransformCO(transformPosition));
	}

	private IEnumerator ZoomToTransformCO(Transform transformPosition)
	{
		resize = false;
		float index = Camera.main.orthographicSize;
		while (index > ortoDestine)
		{
			index -= 0.2f;
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, Quaternion.Euler(0f, 0f, -15f), Time.deltaTime * 5f);
			Camera.main.orthographicSize = index;
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		yield return Globals.Instance.WaitForSeconds(1.5f);
		ortoDestine = ortoInit;
		index = Camera.main.orthographicSize;
		Debug.Log(index + "<" + ortoDestine);
		while (index < ortoDestine)
		{
			index += 0.2f;
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 5f);
			Camera.main.orthographicSize = index;
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		base.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
	}
}
