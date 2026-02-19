using System.Collections.Generic;
using UnityEngine;

public class NenukilLayers : MonoBehaviour
{
	public List<Transform> layersToHide = new List<Transform>();

	public List<Transform> layersCannon = new List<Transform>();

	public List<Transform> layersGun = new List<Transform>();

	public List<Transform> layersDoubleGun = new List<Transform>();

	private void Start()
	{
		if ((bool)MatchManager.Instance)
		{
			Hero[] teamHero = MatchManager.Instance.GetTeamHero();
			for (int i = 0; i < teamHero.Length; i++)
			{
				if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].SubclassName == "engineer")
				{
					HideLayers();
					if (teamHero[i].HaveTrait("doublebarrel"))
					{
						ShowLayersDoubleGun();
					}
					else if (teamHero[i].HaveTrait("mountedcannon"))
					{
						ShowLayersCannon();
					}
					else
					{
						ShowLayersGun();
					}
				}
			}
		}
		else
		{
			HideLayers();
			ShowLayersGun();
		}
	}

	private void HideLayers()
	{
		for (int i = 0; i < layersToHide.Count; i++)
		{
			if (layersToHide[i] != null && layersToHide[i].gameObject.activeSelf)
			{
				layersToHide[i].gameObject.SetActive(value: false);
			}
		}
	}

	private void ShowLayersGun()
	{
		for (int i = 0; i < layersGun.Count; i++)
		{
			if (layersGun[i] != null && !layersGun[i].gameObject.activeSelf)
			{
				layersGun[i].gameObject.SetActive(value: true);
			}
		}
	}

	private void ShowLayersDoubleGun()
	{
		for (int i = 0; i < layersDoubleGun.Count; i++)
		{
			if (layersDoubleGun[i] != null && !layersDoubleGun[i].gameObject.activeSelf)
			{
				layersDoubleGun[i].gameObject.SetActive(value: true);
			}
		}
	}

	private void ShowLayersCannon()
	{
		for (int i = 0; i < layersCannon.Count; i++)
		{
			if (layersCannon[i] != null && !layersCannon[i].gameObject.activeSelf)
			{
				layersCannon[i].gameObject.SetActive(value: true);
			}
		}
	}
}
