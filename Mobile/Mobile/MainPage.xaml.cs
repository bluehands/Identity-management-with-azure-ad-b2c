#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            foreach (var user in App.PCA.Users)
            {
                App.PCA.Remove(user);
            }

        }
        private void OnVoteErdbeer(object sender, EventArgs e)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
            {
                VoteAsync("Erdbeer");
                return false;
            });

        }
        private void OnVoteVanille(object sender, EventArgs e)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
            {
                VoteAsync("Vanille");
                return false;
            });
        }
        private async Task VoteAsync(string choosenSoftware)
        {
            await SignInAsync();
            await CallVoteApiAsync(choosenSoftware);
            await CallStatiticsAsync();
        }

        private async Task CallStatiticsAsync()
        {
            try
            {
                AuthenticationResult ar = await App.PCA.AcquireTokenSilentAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.Authority, false);
                string token = ar.AccessToken;

                // Get data from API
                HttpClient client = new HttpClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, App.ApiEndpoint);
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }

                var json = await response.Content.ReadAsStringAsync();
                var voteResult = JsonConvert.DeserializeObject<VoteResult>(json);
                NameLabel.Text = voteResult.Res;

            }
            catch (MsalUiRequiredException ex)
            {
                await DisplayAlert("Session has expired, please sign out and back in.", ex.ToString(), "Dismiss");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception:", ex.ToString(), "Dismiss");
            }
        }

        async Task CallVoteApiAsync(string choosenSoftware)
        {
            try
            {
                AuthenticationResult ar = await App.PCA.AcquireTokenSilentAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.Authority, false);
                string token = ar.AccessToken;

                // Get data from API
                HttpClient client = new HttpClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, App.ApiEndpoint);
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var vote = new Vote { Value = choosenSoftware };
                var content = JsonConvert.SerializeObject(vote);
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (MsalUiRequiredException ex)
            {
                await DisplayAlert("Session has expired, please sign out and back in.", ex.ToString(), "Dismiss");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception:", ex.ToString(), "Dismiss");
            }
        }

        private async Task SignInAsync()
        {
            if (App.PCA.Users.Any())
            {
                return;
            }
            try
            {
                await App.PCA.AcquireTokenAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.PolicySignUpSignIn), App.UiParent);
            }
            catch (Exception ex)
            {
                if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                {
                    await DisplayAlert("Exception:", ex.ToString(), "Dismiss");
                }
            }

        }

        private IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower()))
                {
                    return user;
                }
            }

            return null;
        }

        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            return decoded;
        }
    }
}
