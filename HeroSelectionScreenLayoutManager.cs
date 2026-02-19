using System;
using UnityEngine;

public class HeroSelectionScreenLayoutManager : MonoBehaviour
{
	[Serializable]
	public struct LayoutElement
	{
		public Transform transform;

		public bool useCustomPosition;

		public Vector3 singlePlayerPosition;

		public Vector3 multiPlayerPosition;

		public bool useCustomScale;

		public Vector3 singlePlayerScale;

		public Vector3 multiPlayerScale;

		public void SetMultiplayerLayout()
		{
			if (useCustomPosition)
			{
				transform.localPosition = multiPlayerPosition;
			}
			if (useCustomScale)
			{
				transform.localScale = multiPlayerScale;
			}
		}

		public void SetSingleplayerLayout()
		{
			if (useCustomPosition)
			{
				transform.localPosition = singlePlayerPosition;
			}
			if (useCustomScale)
			{
				transform.localScale = singlePlayerScale;
			}
		}
	}

	[SerializeField]
	private LayoutElement[] layoutElements;

	private void Start()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			LayoutElement[] array = layoutElements;
			foreach (LayoutElement layoutElement in array)
			{
				layoutElement.SetMultiplayerLayout();
			}
		}
		else
		{
			LayoutElement[] array = layoutElements;
			foreach (LayoutElement layoutElement2 in array)
			{
				layoutElement2.SetSingleplayerLayout();
			}
		}
	}
}
