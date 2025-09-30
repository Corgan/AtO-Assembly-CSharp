// Decompiled with JetBrains decompiler
// Type: LogItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class LogItem
{
  public Enums.LogType LogType;
  public string text;
  public string SkillName = "";
  public string ClassName = "";
  public string CasterName = "";
  public string TargetName = "";
  public Enums.DamageType DamageType;
  public int DamageFinal;
  public int DamagePre;
  public float DamageResist;
  public int DamageBlocked;
  public int DamagePostAuraCurse;
  public int DamageAuraCurse;
  public int HealFinal;
  public bool Immune;
  public bool Invulnerable;
  public bool Evaded;
}
