using UnityEngine;
using UnityEngine.UI;

public class FPSCounterUI : MonoBehaviour
{
	private static FPSCounterUI instance;

	private Text statsText;

	private void Awake()
	{
		base.enabled = true;
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		CreateUI();
	}

	private void CreateUI()
	{
		GameObject gameObject = new GameObject("FPSCanvas");
		Canvas canvas = gameObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 9999;
		gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		Object.DontDestroyOnLoad(gameObject);
		GameObject gameObject2 = new GameObject("StatsText");
		gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
		statsText = gameObject2.AddComponent<Text>();
		statsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
		statsText.fontSize = 10;
		statsText.alignment = TextAnchor.UpperRight;
		statsText.color = Color.white;
		RectTransform component = statsText.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(1f, 1f);
		component.anchorMax = new Vector2(1f, 1f);
		component.pivot = new Vector2(1f, 1f);
		component.anchoredPosition = new Vector2(-20f, -20f);
		component.sizeDelta = new Vector2(500f, 200f);
	}

	private void Update()
	{
		float num = 1f / Time.deltaTime;
		statsText.text = $"FPS: {num:0}\n" + $"Time Scale: {Time.timeScale:0.00}";
	}
}
