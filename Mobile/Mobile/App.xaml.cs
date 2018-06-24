using System;
using Microsoft.Identity.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Mobile
{
	public partial class App : Application
	{
	    public static PublicClientApplication PCA = null;

	    // Azure AD B2C Coordinates
	    public static string Tenant = "erdbeerundvanille.onmicrosoft.com";
	    public static string ClientID = "bff9f38b-1b35-4b50-b0cb-aadb6cc9ccfb";
	    public static string PolicySignUpSignIn = "b2c_1_SuSi";
	    public static string PolicyEditProfile = "b2c_1_PE";
	    public static string PolicyResetPassword = "b2c_1_RP";

	    public static string[] Scopes = { "https://erdbeerundvanille.onmicrosoft.com/api/vote" };
	    public static string ApiEndpoint = "https://6e52469f.eu.ngrok.io/api/votes";

	    public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
	    public static string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";
	    public static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
	    public static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";

	    public static UIParent UiParent = null;
        public App ()
		{
			InitializeComponent();
		    PCA = new PublicClientApplication(ClientID, Authority);
		    PCA.RedirectUri = $"msal{ClientID}://auth";
            MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
