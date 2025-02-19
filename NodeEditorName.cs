// Decompiled with JetBrains decompiler
// Type: NodeEditorName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class NodeEditorName : MonoBehaviour
{
  public TMP_Text nodeIdText;

  private void Awake()
  {
  }

  private void Start()
  {
    this.nodeIdText.text = "";
    this.nodeIdText.gameObject.SetActive(false);
  }

  private void Update()
  {
  }

  private void DoName()
  {
    NodeData nodeData = this.GetComponent<Node>().nodeData;
    if ((Object) nodeData != (Object) null)
      this.nodeIdText.text = nodeData.NodeId;
    this.nodeIdText.gameObject.SetActive(true);
  }
}
