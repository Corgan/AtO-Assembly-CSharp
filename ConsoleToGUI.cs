using TMPro;
using UnityEngine;

public class ConsoleToGUI : MonoBehaviour
{
	public GameObject consoleCanvas;

	public TextMeshProUGUI consoleText;

	private string myLog = "*begin log";

	private bool doShow = true;

	private int kChars = 1200;

	private void OnEnable()
	{
		Application.logMessageReceived += Log;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= Log;
	}

	private void Start()
	{
		doShow = false;
	}

	public void ConsoleShow()
	{
		doShow = !doShow;
		if (doShow)
		{
			consoleCanvas.gameObject.SetActive(value: true);
			SetText();
		}
		else
		{
			consoleCanvas.gameObject.SetActive(value: false);
		}
	}

	private void SetText()
	{
		consoleText.text = myLog;
	}

	public void Log(string logString, string stackTrace, LogType type)
	{
		myLog = myLog + "\n" + logString;
		if (myLog.Length > kChars)
		{
			myLog = myLog.Substring(myLog.Length - kChars);
		}
		if (consoleCanvas.gameObject.activeSelf)
		{
			SetText();
		}
	}
}
