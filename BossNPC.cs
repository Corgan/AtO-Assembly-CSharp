// Decompiled with JetBrains decompiler
// Type: BossNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class BossNPC : IDisposable
{
  internal NPC npc;

  public BossNPC(NPC npc) => this.npc = npc;

  public virtual void Dispose() => this.npc = (NPC) null;

  public virtual bool IsValidTarget() => true;
}
