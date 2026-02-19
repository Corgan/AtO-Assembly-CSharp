using System;
using System.Threading.Tasks;
using PDX.SDK;
using PDX.SDK.Contracts;
using PDX.SDK.Contracts.Configuration;
using PDX.SDK.Contracts.Enums;
using PDX.SDK.Contracts.Service.Account.Result;
using PDX.SDK.Contracts.Service.Legal.Models;
using UnityEngine;

namespace Paradox;

public class Startup
{
	private static IContext context;

	private static StartupResult startup;

	private static bool usingLauncher = true;

	public static bool loginFinished = false;

	public static bool waitingForLoginDocuments = false;

	public static bool isLoggedIn = false;

	public static string userId = "";

	public static string userToken = "";

	public static string userNamespaceName = "";

	public static string userSocialName = "";

	private static int userDocuments = 0;

	private static int currentDocument = 0;

	private static string Namespace = "across_the_obelisk";

	private static Platform Platform = Platform.Windows;

	public static IContext PDXContext => context;

	public static async Task Start()
	{
		if (context != null)
		{
			return;
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("--------- Startup ---------");
		}
		Config config = new Config
		{
			GameVersion = GameManager.Instance.gameVersion,
			GameChecksum = "",
			Environment = BackendEnvironment.Live,
			Ecosystem = Ecosystem.Steam,
			UserIdType = "steam",
			UserId = SteamManager.Instance.steamId.ToString(),
			Telemetry = new TelemetryConfig(),
			Language = GetLangFromGlobalLang()
		};
		if (GameManager.Instance.PDXCliToken == "")
		{
			usingLauncher = false;
		}
		_ = usingLauncher;
		context = await Context.Create(Platform, Namespace, config);
		if (usingLauncher)
		{
			await Account.LoginWithSessionToken();
		}
		else
		{
			startup = await context.Account.Startup();
			if (startup.Success)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.Log("Startup Success");
				}
				userId = startup.UserId;
				userToken = startup.SessionToken;
				if (startup.LegalDocuments != null)
				{
					userDocuments = startup.LegalDocuments.Count;
				}
				if (userDocuments == 0)
				{
					if (startup.IsLoggedIn)
					{
						if (GameManager.Instance.GetDeveloperMode())
						{
							Debug.Log("Startup IsLoggedIn");
						}
						await Profile.ProfileGet();
						await Account.GetEmail();
					}
					else if (GameManager.Instance.GetDeveloperMode())
					{
						Debug.Log("Startup NOT LoggedIn");
					}
				}
				else
				{
					if (GameManager.Instance.GetDeveloperMode())
					{
						Debug.Log("Showing documents");
					}
					if (GameManager.Instance.GetDeveloperMode())
					{
						Debug.Log(startup.IsLoggedIn);
					}
					waitingForLoginDocuments = true;
					currentDocument = -1;
					GameManager.Instance.SceneLoaded();
					ShowDocumentFromStartup();
					while (waitingForLoginDocuments)
					{
						await Task.Delay(10);
					}
				}
				isLoggedIn = startup.IsLoggedIn;
			}
		}
		GameManager.Instance.GetDisabledDLCs();
		Telemetry.SendStartGame();
		await SettingsManager.Instance.InitTelemetryToggle();
	}

	public static void Shutdown()
	{
		if (context != null)
		{
			context.Shutdown();
		}
	}

	public static void ShowDocumentFromStartup()
	{
		currentDocument++;
		if (currentDocument < startup.LegalDocuments.Count)
		{
			ShowDocumentAction(startup.LegalDocuments[currentDocument], isFromStartup: true);
			return;
		}
		waitingForLoginDocuments = false;
		MainMenuManager.Instance.ClosePDXDocument();
	}

	public static async Task ShowDocumentFromForm(int document)
	{
		switch (document)
		{
		case 0:
			await ShowDocumentAction((await context.Legal.GetEula((Language)Enum.Parse(typeof(Language), Globals.Instance.CurrentLang.Substring(0, 2)))).Document);
			break;
		case 1:
			await ShowDocumentAction((await context.Legal.GetPrivacy((Language)Enum.Parse(typeof(Language), Globals.Instance.CurrentLang.Substring(0, 2)))).Document);
			break;
		}
	}

	public static async Task ShowDocumentAction(Document doc, bool isFromStartup = false)
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("ShowDocumentAction " + doc.Title);
		}
		MainMenuManager.Instance.paradoxDocumentText.text = doc.Text;
		MainMenuManager.Instance.ShowPDXDocument();
		if (isFromStartup)
		{
			await context.Legal.MarkAsViewed(doc);
		}
	}

	public static Language GetLangFromGlobalLang()
	{
		string currentLang = Globals.Instance.CurrentLang;
		if (currentLang == "zh-TW")
		{
			return (Language)Enum.Parse(typeof(Language), "zh_Hant");
		}
		if (currentLang == "jp")
		{
			return (Language)Enum.Parse(typeof(Language), "ja");
		}
		return (Language)Enum.Parse(typeof(Language), currentLang.Substring(0, 2));
	}
}
