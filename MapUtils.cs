// Decompiled with JetBrains decompiler
// Type: MapUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Zone;

#nullable disable
public static class MapUtils
{
  public static string GetNodeName(MapType mapType)
  {
    string nodeName;
    switch (mapType)
    {
      case MapType.Tutorial:
        nodeName = "tutorial_0";
        break;
      case MapType.Sen:
        nodeName = "sen_0";
        break;
      case MapType.Secta:
        nodeName = "secta_0";
        break;
      case MapType.Velka:
        nodeName = "velka_0";
        break;
      case MapType.Aqua:
        nodeName = "aqua_0";
        break;
      case MapType.Spider:
        nodeName = "spider_0";
        break;
      case MapType.Voidlow:
        nodeName = "voidlow_0";
        break;
      case MapType.Faen:
        nodeName = "faen_0";
        break;
      case MapType.Forge:
        nodeName = "forge_0";
        break;
      case MapType.Sewers:
        nodeName = "sewers_0";
        break;
      case MapType.Wolf:
        nodeName = "wolf_0";
        break;
      case MapType.Ulmin:
        nodeName = "ulmin_0";
        break;
      case MapType.Pyr:
        nodeName = "pyr_0";
        break;
      case MapType.Uprising:
        nodeName = "uprising_0";
        break;
      case MapType.Sahti:
        nodeName = "sahti_0";
        break;
      case MapType.Dread:
        nodeName = "dread_0";
        break;
      case MapType.Sunken:
        nodeName = "sunken_0";
        break;
      case MapType.Dream:
        nodeName = "dream_0";
        break;
      default:
        nodeName = (string) null;
        break;
    }
    return nodeName;
  }
}
