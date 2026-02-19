using System;
using UnityEngine;

namespace Cards;

[Serializable]
public class CopyConfig
{
	[SerializeField]
	private bool copyNameFromOriginal;

	[SerializeField]
	private bool copyImageFromOriginal;

	[SerializeField]
	private bool copyCardUpgradedFromOriginal;

	[SerializeField]
	private bool copyCardTypeFromOriginal;

	public bool CopyNameFromOriginal => copyNameFromOriginal;

	public bool CopyImageFromOriginal => copyImageFromOriginal;

	public bool CopyCardUpgradedFromOriginal => copyCardUpgradedFromOriginal;

	public bool CopyCardTypeFromOriginal => copyCardTypeFromOriginal;
}
