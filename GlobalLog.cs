using TMPro;
using UnityEngine;

public class GlobalLog : MonoBehaviour
{
	private TMP_Text logTxt;

	public static GlobalLog Instance { get; private set; }

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
		logTxt = GetComponent<TMP_Text>();
	}

	public void Log(string module = "", string text = "")
	{
		string text2 = "";
		if (module != "")
		{
			text2 = text2 + "<color=#999>[" + module + "]</color> ";
		}
		text2 = text2 + text + "\n\n";
		logTxt.text = text2 + logTxt.text;
	}
}
