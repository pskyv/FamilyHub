using Prism;
using Prism.Ioc;
using FamilyAgenda.ViewModels;
using FamilyAgenda.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FamilyAgenda.Utils;
using FamilyAgenda.Services;

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

        protected override void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SyncfusionLicenseKey);

            InitializeComponent();

            MainPage = new AppShell();

            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TodosPage, TodosPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatPage, ChatPageViewModel>();
            containerRegistry.RegisterForNavigation<CalendarPage, CalendarPageViewModel>();

            containerRegistry.RegisterSingleton(typeof(IFirebaseDbService), typeof(FirebaseDbService));
        }
    }
}
