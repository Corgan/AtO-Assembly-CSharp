using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Transform camTransform;

	public float shakeDuration = 0.8f;

	public float shakeAmount = 0.7f;

	public float decreaseFactor = 2f;

	private Vector3 originalPos;

	private Coroutine cameraShakeCo;

	private void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	private void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	public void Shake()
	{
		shakeDuration = 0.2f;
		if (cameraShakeCo == null)
		{
			cameraShakeCo = StartCoroutine(ShakeAction());
		}
	}

	private IEnumerator ShakeAction()
	{
		while (shakeDuration > 0f)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
			yield return null;
		}
		camTransform.localPosition = originalPos;
		cameraShakeCo = null;
	}
}
