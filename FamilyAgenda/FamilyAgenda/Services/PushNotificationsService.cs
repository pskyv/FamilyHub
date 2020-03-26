using FamilyAgenda.Models;
using FamilyAgenda.Utils;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FamilyAgenda.Services
{
    public static class PushNotificationsService
    {
        public static void ConfigureFirebasePushNotifications()
        {
            // Handle when your app starts
            CrossFirebasePushNotification.Current.UnsubscribeAll();
            
            var user = Preferences.Get("user", string.Empty);
            if (!string.IsNullOrEmpty(user))
            {
                var topic = "Sofi";
                if (Preferences.Get("user", "") == "Sofi")
                {
                    topic = "Panos";
                }

                CrossFirebasePushNotification.Current.Subscribe(topic);
            }

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };
            System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");

            // On notification received
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //App.Current.MainPage.Message = $"{p.Data["body"]}";
                        });

                    }
                }
                catch (Exception ex)
                {

                }

            };


            // On notification opened
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //App.Current.MainPage.Message = p.Identifier;
                    });
                }
                else if (p.Data.ContainsKey("color"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                    App.Current.MainPage.Navigation.PushAsync(new ContentPage()
                    {
                        BackgroundColor = Color.FromHex($"{p.Data["color"]}")

                        });
                    });

                }
                else if (p.Data.ContainsKey("aps.alert.title"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //App.Current.MainPage.Message = $"{p.Data["aps.alert.title"]}";
                    });

                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };


            // On notification deleted
            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };
        }

        public static async void SendNotificationAsync(string mesage, string username)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Constants.FirebaseBasePostUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + Constants.FirebaseFCMServerKey);

                try
                {
                    string jsonString = JsonConvert.SerializeObject(new NotificationMessage
                    {
                        To =  $"/topics/{username}",
                        Data = new Data { Message = mesage }
                    });

                    var response = await httpClient.PostAsync("send", new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Helpers.ShowToastMessage(response.StatusCode.ToString());
                    }
                }

                catch (Exception e)
                {
                    Helpers.ShowToastMessage(e.Message);
                }
            }
        }
    }
}
