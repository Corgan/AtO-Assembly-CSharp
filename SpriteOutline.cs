using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
	public Color color = Color.white;

	[Range(0f, 16f)]
	public float outlineSize = 1f;

	public bool autoResize = true;

	private float outlineSizeDest;

	private float outlineSizeShow = 3f;

	private SpriteRenderer spriteRenderer;

	private Color colorWhite = new Color(1f, 0.8f, 0f, 0.9f);

	private Color colorRed = new Color(1f, 0f, 0f, 0.5f);

	private Color colorGreen = new Color(0f, 1f, 0f, 0.5f);

	public void EnableGreen()
	{
		if (color != colorGreen)
		{
			color = colorGreen;
			if (autoResize)
			{
				outlineSizeDest = outlineSizeShow;
			}
			else
			{
				outlineSize = outlineSizeDest;
			}
			UpdateOutline(outline: true);
		}
	}

	public void EnableRed()
	{
		if (color != colorRed)
		{
			color = colorRed;
			if (autoResize)
			{
				outlineSizeDest = outlineSizeShow;
			}
			else
			{
				outlineSize = outlineSizeDest;
			}
			UpdateOutline(outline: true);
		}
	}

	public void EnableWhite()
	{
		if (color != colorWhite)
		{
			color = colorWhite;
			if (autoResize)
			{
				outlineSizeDest = outlineSizeShow;
			}
			else
			{
				outlineSize = outlineSizeDest;
			}
			UpdateOutline(outline: true);
		}
	}

	public void Hide()
	{
		color = new Color(0f, 0f, 0f, 0f);
		outlineSizeDest = 0f;
	}

	private void OnEnable()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		UpdateOutline(outline: true);
	}

	private void OnDisable()
	{
		UpdateOutline(outline: false);
	}

	private void UpdateOutline(bool outline)
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		spriteRenderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetFloat("_Outline", outline ? 1f : 0f);
		materialPropertyBlock.SetColor("_OutlineColor", color);
		materialPropertyBlock.SetFloat("_OutlineSize", outlineSize);
		spriteRenderer.SetPropertyBlock(materialPropertyBlock);
	}
}
