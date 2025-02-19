// Decompiled with JetBrains decompiler
// Type: Paradox.Profile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using PDX.SDK.Contracts.Service.Profile.Result;
using System.Threading.Tasks;

#nullable disable
namespace Paradox
{
  public class Profile
  {
    public static async Task ProfileGet()
    {
      GetProfileResult getProfileResult = await Startup.PDXContext.Profile.Get();
      if (!getProfileResult.Success)
        return;
      Startup.userNamespaceName = getProfileResult.Game.DisplayName;
      Startup.userSocialName = getProfileResult.Social.DisplayName;
    }
  }
}
