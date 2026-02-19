using UnityEngine;

public class TomeManagerScene : MonoBehaviour
{
	public Transform sceneCamera;

	private void Awake()
	{
		sceneCamera.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		AudioManager.Instance.DoBSO("Town");
		TomeManager.Instance.ShowTome(_status: true);
		GameManager.Instance.SceneLoaded();
	}
}
