using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace FamilyAgenda.Droid
{
    [Activity(Label = "Family Hub", Icon = "@mipmap/ic_launcher", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.SetFlags("SwipeView_Experimental");

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Forms.FormsMaterial.Init(this, bundle);

            UserDialogs.Init(this);

            LoadApplication(new App(new AndroidInitializer()));

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

