using UnityEngine;

[CreateAssetMenu(fileName = "New KeyNote", menuName = "KeyNote Data", order = 57)]
public class KeyNotesData : ScriptableObject
{
	[SerializeField]
	[HideInInspector]
	private string id;

	[SerializeField]
	private string keynoteName;

	[TextArea]
	[SerializeField]
	private string description;

	[TextArea]
	[SerializeField]
	private string descriptionExtended;

	public string KeynoteName
	{
		get
		{
			return keynoteName;
		}
		set
		{
			keynoteName = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public string DescriptionExtended
	{
		get
		{
			return descriptionExtended;
		}
		set
		{
			descriptionExtended = value;
		}
	}

	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}
}
