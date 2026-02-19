using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class NodeEditorName : MonoBehaviour
{
	public TMP_Text nodeIdText;

	private void Awake()
	{
	}

	private void Start()
	{
		nodeIdText.text = "";
		nodeIdText.gameObject.SetActive(value: false);
	}

	private void Update()
	{
	}

	private void DoName()
	{
		NodeData nodeData = GetComponent<Node>().nodeData;
		if (nodeData != null)
		{
			nodeIdText.text = nodeData.NodeId;
		}
		nodeIdText.gameObject.SetActive(value: true);
	}
}
