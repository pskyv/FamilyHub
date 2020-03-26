using Prism;
using Prism.Ioc;
using FamilyAgenda.ViewModels;
using FamilyAgenda.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FamilyAgenda.Utils;
using FamilyAgenda.Services;
using FamilyAgenda.Models;
using Xamarin.Essentials;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FamilyAgenda
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        public static User ApplicationUser { get; set; }

        protected override void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SyncfusionLicenseKey);

            InitializeComponent();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            MainPage = new AppShell();

            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void OnStart()
        {
            base.OnStart();
            PushNotificationsService.ConfigureFirebasePushNotifications();
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                Helpers.ShowToastMessage(Constants.ConnectivityLostMsg);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<TodosPage, TodosPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatPage, ChatPageViewModel>();
            containerRegistry.RegisterForNavigation<SchedulerPage, SchedulerPageViewModel>();
            containerRegistry.RegisterForNavigation<NewEventPage, NewEventPageViewModel>();

            containerRegistry.RegisterSingleton(typeof(IFirebaseDbService), typeof(FirebaseDbService));            
        }
    }
}
