// Decompiled with JetBrains decompiler
// Type: Paradox.Account
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using PDX.SDK.Contracts.Credential;
using PDX.SDK.Contracts.Enums;
using PDX.SDK.Contracts.Enums.Errors;
using PDX.SDK.Contracts.Service.Account.Result;
using System;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Paradox
{
  public class Account
  {
    public static EmailAndPasswordCredential emailAndPasswordCredential;
    private static Language lang;
    private static Country country;
    private static DateTime birthdate;
    private static bool marketing;
    private static string email;

    public static string Email => Paradox.Account.email;

    public static void SetEmailAndPasswordCredential(string _email, string _password)
    {
      Paradox.Account.email = _email;
      Paradox.Account.emailAndPasswordCredential = new EmailAndPasswordCredential(_email, _password);
    }

    public static void SetLang() => Paradox.Account.lang = Paradox.Startup.GetLangFromGlobalLang();

    public static void SetCountry(string _country)
    {
      Paradox.Account.country = (Country) Enum.Parse(typeof (Country), _country.Substring(0, 2));
    }

    public static void SetBirthdate(string _date)
    {
      Paradox.Account.birthdate = DateTime.ParseExact(_date, "yyyy-MM-dd", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static void SetMarketingPermission(bool _marketing) => Paradox.Account.marketing = _marketing;

    public static async Task CreateParadoxAccount()
    {
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) nameof (CreateParadoxAccount));
      await Paradox.Account.HandleLoginResult((LoginResult) await Paradox.Startup.PDXContext.Account.Create(Paradox.Account.emailAndPasswordCredential, Paradox.Account.lang, Paradox.Account.country, Paradox.Account.birthdate, marketingPermission: Paradox.Account.marketing));
    }

    public static async Task LoginWithEmailAndPassword()
    {
      await Paradox.Account.HandleLoginResult(await Paradox.Startup.PDXContext.Account.Login((ICredential) Paradox.Account.emailAndPasswordCredential));
    }

    public static async Task LoginWithSessionToken(string _token = "")
    {
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) "LoginWithSessionToken()");
      LoginResult loginResult = await Paradox.Startup.PDXContext.Account.Login((ICredential) new SessionToken(GameManager.Instance.PDXCliToken));
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) ("loginResult=>" + loginResult?.ToString()));
      await Paradox.Account.HandleLoginResult(loginResult);
    }

    public static async Task LoginWithRefreshToken()
    {
      await Paradox.Account.HandleLoginResult(await Paradox.Startup.PDXContext.Account.Login((ICredential) new RefreshToken()));
    }

    public static async Task Startup()
    {
      StartupResult message = await Paradox.Startup.PDXContext.Account.Startup();
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) message);
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) ("IsLoggedIn=>" + message.IsLoggedIn.ToString()));
      await Paradox.Account.HandleLoginResult((LoginResult) message);
    }

    public static async Task GetEmail()
    {
      Paradox.Account.email = (await Paradox.Startup.PDXContext.Account.GetDetails()).Email;
    }

    public static async Task HandleLoginResult(LoginResult loginResult)
    {
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) nameof (HandleLoginResult));
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) loginResult);
      if (loginResult.Success)
      {
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) "[LoginResult] Success");
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("UserId=>" + loginResult.UserId));
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("SessionToken=>" + loginResult.SessionToken));
        Paradox.Startup.userId = loginResult.UserId;
        Paradox.Startup.userToken = loginResult.SessionToken;
        await Profile.ProfileGet();
        await Paradox.Account.GetEmail();
        Paradox.Startup.isLoggedIn = true;
        MainMenuManager.Instance.ShowPDXLogged();
      }
      else
      {
        if (GameManager.Instance.GetDeveloperMode())
          Debug.Log((object) ("[LoginResult] Error => " + loginResult.Error?.ToString()));
        if (loginResult.Error == (Enum) NotAuthorized.InvalidAuthentication)
          MainMenuManager.Instance.ShowPDXError("pdxErrorInvalidAuthentication");
        else if (loginResult.Error == BaseCategory.InvalidNetworkResponse)
          MainMenuManager.Instance.ShowPDXError("pdxErrorInvalidAuthentication");
        else if (loginResult.Error == BaseCategory.NotAuthorized)
          MainMenuManager.Instance.ShowPDXError("pdxErrorInvalidAuthentication");
        else
          MainMenuManager.Instance.ShowPDXError("pdxErrorMiscErrorRetry");
      }
      Paradox.Startup.loginFinished = true;
    }

    public static async Task Logout()
    {
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) nameof (Logout));
      if ((await Paradox.Startup.PDXContext.Account.Logout()).Success && GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) "User Logout success");
      Paradox.Startup.isLoggedIn = false;
      MainMenuManager.Instance.ShowPDXLogin();
    }
  }
}
