// Decompiled with JetBrains decompiler
// Type: NodesConnectedRequirement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
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
    get => this.nodeData;
    set => this.nodeData = value;
  }

  public EventRequirementData ConectionRequeriment
  {
    get => this.conectionRequeriment;
    set => this.conectionRequeriment = value;
  }

  public NodeData ConectionIfNotNode
  {
    get => this.conectionIfNotNode;
    set => this.conectionIfNotNode = value;
  }
}
