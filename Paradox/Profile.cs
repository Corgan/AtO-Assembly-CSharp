// Decompiled with JetBrains decompiler
// Type: Paradox.Profile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
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
