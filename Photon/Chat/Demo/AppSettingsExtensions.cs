// Decompiled with JetBrains decompiler
// Type: Photon.Chat.Demo.AppSettingsExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Photon.Realtime;

#nullable disable
namespace Photon.Chat.Demo
{
  public static class AppSettingsExtensions
  {
    public static ChatAppSettings GetChatSettings(this AppSettings appSettings)
    {
      return new ChatAppSettings()
      {
        AppIdChat = appSettings.AppIdChat,
        AppVersion = appSettings.AppVersion,
        FixedRegion = appSettings.IsBestRegion ? (string) null : appSettings.FixedRegion,
        NetworkLogging = appSettings.NetworkLogging,
        Protocol = appSettings.Protocol,
        EnableProtocolFallback = appSettings.EnableProtocolFallback,
        Server = appSettings.IsDefaultNameServer ? (string) null : appSettings.Server,
        Port = (ushort) appSettings.Port
      };
    }
  }
}
