using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaterialSettings;

public class IllusionMaterialSettings : MonoBehaviour
{
	private enum MaterialType
	{
		NoAlpha,
		WithAlpha
	}

	[Serializable]
	private class MaterialGroup
	{
		public MaterialType Type;

		public Material Material;

		public List<SpriteRenderer> Parts = new List<SpriteRenderer>();
	}

	[SerializeField]
	private List<MaterialGroup> _materialGroups = new List<MaterialGroup>();

	private static readonly int Reveal = Shader.PropertyToID("_Reveal");

	private static readonly int UseAlpha = Shader.PropertyToID("_UseAlpha");

	private void Awake()
	{
		foreach (MaterialGroup materialGroup in _materialGroups)
		{
			if (materialGroup.Material == null)
			{
				continue;
			}
			materialGroup.Material = new Material(materialGroup.Material);
			SetMaterialProperty(materialGroup.Material, UseAlpha, 0f);
			SetMaterialProperty(materialGroup.Material, Reveal, 0f);
			foreach (SpriteRenderer part in materialGroup.Parts)
			{
				if (part != null)
				{
					part.material = materialGroup.Material;
				}
			}
		}
	}

	public void EnableExposeIllusionMaterial()
	{
		foreach (MaterialGroup materialGroup in _materialGroups)
		{
			SetPropertyBlock(materialGroup.Parts, Reveal, 1f);
			if (materialGroup.Type == MaterialType.WithAlpha)
			{
				SetPropertyBlock(materialGroup.Parts, UseAlpha, 1f);
			}
		}
	}

	public void DisableExposeIllusionMaterial()
	{
		foreach (MaterialGroup materialGroup in _materialGroups)
		{
			SetPropertyBlock(materialGroup.Parts, Reveal, 0f);
			if (materialGroup.Type == MaterialType.WithAlpha)
			{
				SetPropertyBlock(materialGroup.Parts, UseAlpha, 0f);
			}
		}
	}

	private void SetMaterialProperty(Material mat, int propertyID, float value)
	{
		if (mat != null)
		{
			mat.SetFloat(propertyID, value);
		}
	}

	private void SetPropertyBlock(List<SpriteRenderer> parts, int propertyID, float value)
	{
		foreach (SpriteRenderer part in parts)
		{
			if (!(part == null))
			{
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				part.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(propertyID, value);
				part.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	private void OnDestroy()
	{
		foreach (MaterialGroup materialGroup in _materialGroups)
		{
			if (!(materialGroup.Material == null))
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(materialGroup.Material);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(materialGroup.Material);
				}
			}
		}
	}
}
