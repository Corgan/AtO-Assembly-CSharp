// Decompiled with JetBrains decompiler
// Type: KeyNotesData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
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
    get => this.keynoteName;
    set => this.keynoteName = value;
  }

  public string Description
  {
    get => this.description;
    set => this.description = value;
  }

  public string DescriptionExtended
  {
    get => this.descriptionExtended;
    set => this.descriptionExtended = value;
  }

  public string Id
  {
    get => this.id;
    set => this.id = value;
  }
}
