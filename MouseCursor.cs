using UnityEngine;

public class MouseCursor : MonoBehaviour
{
	public Transform imageUI;

	private RectTransform imageUIR;

	private float originalSize = 25f;

	private float scale = 1f;

	private float cursorOffset;

	public static MouseCursor Instance { get; private set; }

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
		imageUIR = imageUI.GetComponent<RectTransform>();
	}

	private void Start()
	{
		Cursor.visible = false;
		scale = Screen.width / 1920;
		imageUI.localScale = new Vector3(scale, scale, 1f);
		cursorOffset = originalSize * scale;
	}

	public void Show()
	{
		imageUI.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		imageUI.gameObject.SetActive(value: false);
	}
}
