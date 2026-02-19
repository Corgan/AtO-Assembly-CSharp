using System.Threading.Tasks;
using PDX.SDK.Contracts.Service.Profile.Result;

namespace Paradox;

public class Profile
{
	public static async Task ProfileGet()
	{
		GetProfileResult getProfileResult = await Startup.PDXContext.Profile.Get();
		if (getProfileResult.Success)
		{
			Startup.userNamespaceName = getProfileResult.Game.DisplayName;
			Startup.userSocialName = getProfileResult.Social.DisplayName;
		}
	}
}
