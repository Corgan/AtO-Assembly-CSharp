using UnityEngine;

public class TrailerPoster : MonoBehaviour
{
	public GameObject[] poster;

	public static TrailerPoster Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(this);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ShowPoster(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ShowPoster(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ShowPoster(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			ShowPoster(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			ShowPoster(4);
		}
	}

	private void ShowPoster(int _poster)
	{
		Debug.Log("Show Poster " + _poster);
		for (int i = 0; i < poster.Length; i++)
		{
			poster[i].gameObject.SetActive(value: false);
		}
		if (_poster < poster.Length && poster[_poster] != null)
		{
			poster[_poster].gameObject.SetActive(value: true);
		}
	}
}
