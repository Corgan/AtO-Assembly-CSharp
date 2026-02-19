using System;
using UnityEngine;

[Serializable]
public class NodesConnectedRequirement
{
	[SerializeField]
	private NodeData nodeData;

	[SerializeField]
	private EventRequirementData conectionRequeriment;

	[SerializeField]
	private NodeData conectionIfNotNode;

	public NodeData NodeData
	{
		get
		{
			return nodeData;
		}
		set
		{
			nodeData = value;
		}
	}

	public EventRequirementData ConectionRequeriment
	{
		get
		{
			return conectionRequeriment;
		}
		set
		{
			conectionRequeriment = value;
		}
	}

	public NodeData ConectionIfNotNode
	{
		get
		{
			return conectionIfNotNode;
		}
		set
		{
			conectionIfNotNode = value;
		}
	}
}
