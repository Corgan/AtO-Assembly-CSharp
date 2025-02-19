// Decompiled with JetBrains decompiler
// Type: LogItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
