using UnityEngine;

public class BlurRenderer : MonoBehaviour
{
	public Camera blurCamera;

	public Material blurMaterial;

	private void Start()
	{
		CaptureScreen();
	}

	private void OnEnable()
	{
		CaptureScreen();
	}

	public void CaptureScreen()
	{
		Debug.Log("CaptureScreen");
		int width = Screen.width;
		int height = Screen.height;
		int depth = 24;
		RenderTexture renderTexture = new RenderTexture(height, width, depth);
		Rect source = new Rect(0f, 0f, height, width);
		Texture2D texture2D = new Texture2D(height, width, TextureFormat.RGBA32, mipChain: false);
		blurCamera.targetTexture = renderTexture;
		blurCamera.Render();
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(source, 0, 0);
		texture2D.Apply();
		blurCamera.targetTexture = null;
		RenderTexture.active = active;
		Object.Destroy(renderTexture);
		blurMaterial.SetTexture("_RenTex", texture2D);
	}
}
