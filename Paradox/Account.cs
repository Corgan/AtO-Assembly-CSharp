using System;
using System.Globalization;
using System.Threading.Tasks;
using PDX.SDK.Contracts.Credential;
using PDX.SDK.Contracts.Enums;
using PDX.SDK.Contracts.Enums.Errors;
using PDX.SDK.Contracts.Service.Account.Result;
using UnityEngine;

namespace Paradox;

public class Account
{
	public static EmailAndPasswordCredential emailAndPasswordCredential;

	private static Language lang;

	private static Country country;

	private static DateTime birthdate;

	private static bool marketing;

	private static string email;

	public static string Email => email;

	public static void SetEmailAndPasswordCredential(string _email, string _password)
	{
		email = _email;
		emailAndPasswordCredential = new EmailAndPasswordCredential(_email, _password);
	}

	public static void SetLang()
	{
		lang = Paradox.Startup.GetLangFromGlobalLang();
	}

	public static void SetCountry(string _country)
	{
		country = (Country)Enum.Parse(typeof(Country), _country.Substring(0, 2));
	}

	public static void SetBirthdate(string _date)
	{
		birthdate = DateTime.ParseExact(_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
	}

	public static void SetMarketingPermission(bool _marketing)
	{
		marketing = _marketing;
	}

	public static async Task CreateParadoxAccount()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("CreateParadoxAccount");
		}
		await HandleLoginResult(await Paradox.Startup.PDXContext.Account.Create(emailAndPasswordCredential, lang, country, birthdate, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, marketing));
	}

	public static async Task LoginWithEmailAndPassword()
	{
		await HandleLoginResult(await Paradox.Startup.PDXContext.Account.Login(emailAndPasswordCredential));
	}

	public static async Task LoginWithSessionToken(string _token = "")
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("LoginWithSessionToken()");
		}
		LoginResult loginResult = await Paradox.Startup.PDXContext.Account.Login(new SessionToken(GameManager.Instance.PDXCliToken));
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("loginResult=>" + loginResult);
		}
		await HandleLoginResult(loginResult);
	}

	public static async Task LoginWithRefreshToken()
	{
		await HandleLoginResult(await Paradox.Startup.PDXContext.Account.Login(new RefreshToken()));
	}

	public static async Task Startup()
	{
		StartupResult startupResult = await Paradox.Startup.PDXContext.Account.Startup();
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log(startupResult);
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("IsLoggedIn=>" + startupResult.IsLoggedIn);
		}
		await HandleLoginResult(startupResult);
	}

	public static async Task GetEmail()
	{
		email = (await Paradox.Startup.PDXContext.Account.GetDetails()).Email;
	}

	public static async Task HandleLoginResult(LoginResult loginResult)
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("HandleLoginResult");
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log(loginResult);
		}
		if (loginResult.Success)
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("[LoginResult] Success");
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("UserId=>" + loginResult.UserId);
			}
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("SessionToken=>" + loginResult.SessionToken);
			}
			Paradox.Startup.userId = loginResult.UserId;
			Paradox.Startup.userToken = loginResult.SessionToken;
			await Profile.ProfileGet();
			await GetEmail();
			Paradox.Startup.isLoggedIn = true;
			MainMenuManager.Instance.ShowPDXLogged();
		}
		else
		{
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("[LoginResult] Error => " + loginResult.Error);
			}
			if (loginResult.Error == NotAuthorized.InvalidAuthentication)
			{
				MainMenuManager.Instance.ShowPDXError("pdxErrorInvalidAuthentication");
			}
			else if (loginResult.Error == BaseCategory.InvalidNetworkResponse)
			{
				MainMenuManager.Instance.ShowPDXError("pdxErrorInvalidAuthentication");
			}
			else if (loginResult.Error == BaseCategory.NotAuthorized)
			{
				MainMenuManager.Instance.ShowPDXError("pdxErrorInvalidAuthentication");
			}
			else
			{
				MainMenuManager.Instance.ShowPDXError("pdxErrorMiscErrorRetry");
			}
		}
		Paradox.Startup.loginFinished = true;
	}

	public static async Task Logout()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("Logout");
		}
		if ((await Paradox.Startup.PDXContext.Account.Logout()).Success && GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("User Logout success");
		}
		Paradox.Startup.isLoggedIn = false;
		MainMenuManager.Instance.ShowPDXLogin();
	}
}
