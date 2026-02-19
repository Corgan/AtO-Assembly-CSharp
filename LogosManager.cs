using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogosManager : MonoBehaviour
{
	public List<SpriteRenderer> srList;

	public Transform sceneCamera;

	private float secondsFadeStep = 0.02f;

	private float secondsFade = 1f;

	private float iterationFadeIncrement;

	private float timeBetweenLogos = 1f;

	private float timeLogoFull = 1f;

	private int logoStep = -1;

	private Coroutine logoCo;

	private Color whiteColor;

	public static LogosManager Instance { get; private set; }

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
		for (int i = 0; i < srList?.Count; i++)
		{
			if (srList[i].gameObject.activeSelf)
			{
				srList[i].transform.gameObject.SetActive(value: false);
			}
		}
	}

	private void Start()
	{
		if (GameManager.Instance == null)
		{
			SceneManager.LoadScene("Game");
			return;
		}
		sceneCamera.gameObject.SetActive(value: false);
		if (srList == null || srList.Count == 0)
		{
			GoToNextScene();
			return;
		}
		whiteColor = new Color(1f, 1f, 1f, 0f);
		for (int i = 0; i < srList.Count; i++)
		{
			srList[i].color = whiteColor;
		}
		iterationFadeIncrement = 1f / (secondsFade / secondsFadeStep);
		GameManager.Instance.SceneLoaded();
		DoLogos();
	}

	private void DoLogos()
	{
		logoStep++;
		if (logoStep < srList.Count)
		{
			logoCo = StartCoroutine(LogoAnimation());
		}
		else
		{
			GoToNextScene();
		}
	}

	private void GoToNextScene()
	{
		GameManager.Instance.ChangeScene("MainMenu");
	}

	private IEnumerator LogoAnimation()
	{
		yield return Globals.Instance.WaitForSeconds(timeBetweenLogos * 0.5f);
		float index = 0f;
		SpriteRenderer sr = srList[logoStep];
		sr.gameObject.SetActive(value: true);
		while (index < 1f)
		{
			index += iterationFadeIncrement;
			whiteColor.a = index;
			sr.color = whiteColor;
			yield return Globals.Instance.WaitForSeconds(secondsFadeStep);
		}
		yield return Globals.Instance.WaitForSeconds(timeLogoFull);
		while (index > 0f)
		{
			index -= iterationFadeIncrement;
			whiteColor.a = index;
			sr.color = whiteColor;
			yield return Globals.Instance.WaitForSeconds(secondsFadeStep);
		}
		sr.gameObject.SetActive(value: false);
		yield return Globals.Instance.WaitForSeconds(timeBetweenLogos * 0.5f);
		DoLogos();
	}

	public void Escape(bool skipAll = false)
	{
		if (logoCo != null)
		{
			StopCoroutine(logoCo);
		}
		srList[logoStep].gameObject.SetActive(value: false);
		if (skipAll)
		{
			logoStep = srList.Count - 1;
		}
		DoLogos();
	}
}
