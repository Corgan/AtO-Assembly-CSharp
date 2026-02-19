using UnityEngine;

public class TrailerManager : MonoBehaviour
{
	public static TrailerManager Instance { get; private set; }

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("TrailerEnd");
			return;
		}
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(this);
		}
		GameManager.Instance.SetCamera();
		NetworkManager.Instance.StartStopQueue(state: true);
		GameManager.Instance.SceneLoaded();
	}
}
